//Code taken from : https://github.com/SteamDatabase/SteamAppInfo
using System.Buffers;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Model;
using DSCPSetter.Utility.Abstract;
using ValveKeyValue;

public class VDFConverterUtility : IDisposable, IVDFConverterUtilityService
{
    private readonly IPathHelperService _pathHelperService;
    public Utf8JsonWriter Writer { get; private set; }

    private bool disposed;

    public VDFConverterUtility(IPathHelperService pathHelperService)
    {
        _pathHelperService = pathHelperService;

        Writer = new Utf8JsonWriter(new FileStream(Path.Combine(Path.GetTempPath(), "appinfo.json"), FileMode.Create, FileAccess.Write, FileShare.None));
    }

    private const uint Magic29 = 0x07_56_44_29;
    private const uint Magic28 = 0x07_56_44_28;
    private const uint Magic = 0x07_56_44_27;
    public EUniverse Universe { get; set; }

    //reads appcache.vdf
    public void Read(string filename)
    {
        using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);
        Read(fs);
    }
    public void Read(Stream input)
    {
        using var reader = new BinaryReader(input);
        var magic = reader.ReadUInt32();

        if (magic != Magic && magic != Magic28 && magic != Magic29)
        {
            throw new InvalidDataException($"Unknown magic header: {magic:X}");
        }

        Universe = (EUniverse)reader.ReadUInt32();

        var options = new KVSerializerOptions();

        if (magic == Magic29)
        {
            var stringTableOffset = reader.ReadInt64();
            var offset = reader.BaseStream.Position;
            reader.BaseStream.Position = stringTableOffset;
            var stringCount = reader.ReadUInt32();
            var stringPool = new string[stringCount];

            for (var i = 0; i < stringCount; i++)
            {
                stringPool[i] = ReadNullTermUtf8String(reader.BaseStream);
            }

            reader.BaseStream.Position = offset;

            options.StringTable = new(stringPool);
        }

        var deserializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);


        Writer.WriteStartArray();

        do
        {
            var appid = reader.ReadUInt32();

            if (appid == 0)
            {
                break;
            }

            var size = reader.ReadUInt32(); // size until end of Data
            var end = reader.BaseStream.Position + size;

            var app = new VDFModel
            {
                AppID = appid,
                InfoState = reader.ReadUInt32(),
                LastUpdated = DateTimeFromUnixTime(reader.ReadUInt32()),
                Token = reader.ReadUInt64(),
                Hash = new ReadOnlyCollection<byte>(reader.ReadBytes(20)),
                ChangeNumber = reader.ReadUInt32(),
            };

            if (magic == Magic28 || magic == Magic29)
            {
                app.BinaryDataHash = new ReadOnlyCollection<byte>(reader.ReadBytes(20));
            }

            app.Data = deserializer.Deserialize(input, options);
            if (reader.BaseStream.Position != end)
            {
                throw new InvalidDataException();
            }

            Writer.WriteStartObject();
            Writer.WriteNumber("AppID", app.AppID);
            Writer.WriteString("Name", (string)app.Data?["common"]?["name"]);
            Writer.WriteString("installdir", (string)app.Data?["config"]?["installdir"]);
            //Loop throughe every key in launch and take the executable name if its end with .exe
            for (int i = 0; ; i++)
            {
                var launchConfig = app.Data?["config"]?["launch"]?[i.ToString()];
                if (launchConfig == null)
                    break;

                string executable = (string)launchConfig["executable"];
                if (executable != null && executable.EndsWith("exe"))
                    Writer.WriteString("executable", executable);
            }
            Writer.WriteEndObject();
        }
        while (true);

        Writer.WriteEndArray();

        Writer.Flush();
    }
    private static DateTime DateTimeFromUnixTime(uint unixTime)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
    }

    private static string ReadNullTermUtf8String(Stream stream)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(32);

        try
        {
            var position = 0;

            do
            {
                var b = stream.ReadByte();

                if (b <= 0) // null byte or stream ended
                {
                    break;
                }

                if (position >= buffer.Length)
                {
                    var newBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length * 2);
                    Buffer.BlockCopy(buffer, 0, newBuffer, 0, buffer.Length);
                    ArrayPool<byte>.Shared.Return(buffer);
                    buffer = newBuffer;
                }

                buffer[position++] = (byte)b;
            }
            while (true);

            return Encoding.UTF8.GetString(buffer[..position]);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        {
            //Reader.Dispose();
            Writer.Dispose();
        }
        //Reader = null!;
        Writer = null!;

        disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);

        // Force close
        GC.WaitForPendingFinalizers();
        GC.Collect();

        GC.SuppressFinalize(this);
    }
}

public enum EUniverse
{
    Invalid = 0,
    Public = 1,
    Beta = 2,
    Internal = 3,
    Dev = 4,
    Max = 5,
}