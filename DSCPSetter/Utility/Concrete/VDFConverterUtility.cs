//Code taken from : https://github.com/Grub4K/VDFparse
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Utility.Abstract;
using DSCPSetter.Utility.Concrete;
using System.Text;
using System.Text.Json;

public class VDFConverterUtility : IDisposable, IVDFConverterUtilityService
{
    private readonly IPathHelperService _pathHelperService;
    public BinaryReader Reader { get; private set; }
    public Utf8JsonWriter Writer { get; private set; }

    private bool disposed;
    public VDFConverterUtility(IPathHelperService pathHelperService)
    {
        _pathHelperService = pathHelperService;

        Reader = new BinaryReader(new FileStream(Path.Combine(_pathHelperService.GetSteamPath(), "appcache", "appinfo.vdf"), FileMode.Open, FileAccess.Read, FileShare.None));
        Writer = new Utf8JsonWriter(new FileStream(Path.Combine(Path.GetTempPath(), "appinfo.json"), FileMode.Create, FileAccess.Write, FileShare.None));
    }

    public void Transform() => Transform(null);

    public void Transform(HashSet<uint> ids)
    {
        var magic = Reader.ReadUInt32();

        var type = magic switch
        {
            0x07_56_44_27 => AppinfoType.AppInfoV1,
            0x07_56_44_28 => AppinfoType.AppInfoV2,
            _ => throw new InvalidDataException($"Unknown header: {magic:X8}"),
        };

        var endMarker = type switch
        {
            AppinfoType.AppInfoV1 or AppinfoType.AppInfoV2 => 0u,
            _ => throw new Exception($"{nameof(type)} was checked before"),
        };

        using (Writer)
        {
            Writer.WriteStartObject();
            Writer.WriteString("magic", $"0x{magic:X8}");
            Writer.WriteString("e_universe", Encoding.UTF8.GetBytes(Reader.ReadUInt32() switch
            {
                0 => "invalid",
                1 => "public",
                2 => "beta",
                3 => "internal",
                4 => "dev",
                5 => "max",
                _ => throw new InvalidDataException("Invalid value for EUniverse"),
            }));


            Writer.WriteStartArray("datasets");
            while (true)
            {
                var id = Reader.ReadUInt32();
                if (id == endMarker)
                {
                    break;
                }

                if (ids is not null && !ids.Contains(id))
                {
                    ConsumeSingleData(type);
                }
                else
                {
                    TransformSingleData(id, type);
                }
            }
            Writer.WriteEndArray();

            Writer.WriteEndObject();
            Writer.Flush();
        }
    }

    private void ConsumeSingleData(AppinfoType type)
    {
        switch (type)
        {
            case AppinfoType.AppInfoV1:
            case AppinfoType.AppInfoV2:
                Reader.ReadBytes(Reader.ReadInt32());
                break;

            default:
                throw new Exception($"{nameof(type)} was checked before");
        }
    }

    private void TransformSingleData(uint id, AppinfoType type)
    {
        Writer.WriteStartObject();
        Writer.WriteNumber("id", id);
        if (type is AppinfoType.AppInfoV1 or AppinfoType.AppInfoV2)
        {
            Writer.WriteNumber("size", Reader.ReadUInt32());
            Writer.WriteNumber("info_state", Reader.ReadUInt32());
            Writer.WriteString(
                "last_updated",
                DateTimeOffset.FromUnixTimeSeconds(Reader.ReadUInt32()).DateTime
            );
            Writer.WriteNumber("token", Reader.ReadUInt64());
        }

        Writer.WriteBase64String("hash", Reader.ReadBytes(20));


        Writer.WriteNumber("change_number", Reader.ReadUInt32());

        if (type == AppinfoType.AppInfoV2)
        {
            Writer.WriteBase64String("vdf_hash", Reader.ReadBytes(20));
        }

        KVTransformerUtility.BinaryToJson(Reader, Writer);


        Writer.WriteEndObject();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
            return;

        if (disposing)
        {
            Reader.Dispose();
            Writer.Dispose();
        }
        Reader = null!;
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

public enum AppinfoType
{
    AppInfoV1,
    AppInfoV2,
}