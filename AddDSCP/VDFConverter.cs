using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace AddDSCP
{
    //All the code taken from : https://github.com/Grub4K/VDFparse
    internal class VDFConverter : IDisposable
    {
        public BinaryReader Reader { get; private set; }
        public Utf8JsonWriter Writer { get; private set; }
        private bool disposedValue;
        public VDFConverter(string path)
        {
            Reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
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

            BinaryToJson(Reader, Writer);


            Writer.WriteEndObject();
        }

        public static void BinaryToJson(BinaryReader reader, Utf8JsonWriter writer)
        {
            writer.WriteStartObject("data");
            var startDepth = writer.CurrentDepth;
            while (true)
            {
                var current = (DataType)reader.ReadByte();
                if (current == DataType.END || current == DataType.ENDB)
                {
                    writer.WriteEndObject();

                    if (writer.CurrentDepth < startDepth)
                        return;
                    continue;
                }
                writer.WritePropertyName(ReadString(reader));
                switch (current)
                {
                    case DataType.START:
                        writer.WriteStartObject();
                        break;
                    case DataType.STRING:
                        writer.WriteStringValue(ReadString(reader));
                        break;
                    case DataType.INT:
                        writer.WriteNumberValue(reader.ReadInt32());
                        break;
                    case DataType.FLOAT:
                        writer.WriteNumberValue(reader.ReadSingle());
                        break;
                    case DataType.PTR:
                        writer.WriteNumberValue(reader.ReadUInt32());
                        break;
                    // case DataType.WSTRING:
                    //     writer.WriteStringValue(ReadWideString(reader));
                    //     break;
                    case DataType.COLOR:
                        writer.WriteStringValue($"#{reader.ReadUInt32():X8}");
                        break;
                    case DataType.INT64:
                        writer.WriteNumberValue(reader.ReadInt64());
                        break;
                    case DataType.UINT64:
                        writer.WriteNumberValue(reader.ReadUInt64());
                        break;
                    default:
                        throw new InvalidDataException($"Unexpected type for value ({current})");
                }
            }

        }

        private static byte[] ReadString(BinaryReader reader)
        {
            using var buffer = new MemoryStream();
            byte current;

            while ((current = reader.ReadByte()) != 0)
            {
                buffer.WriteByte(current);
            }

            return buffer.ToArray();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
                return;

            if (disposing)
            {
                Writer.Flush();
                Reader.Dispose();
                Writer.Dispose();
            }
            Reader = null!;
            Writer = null!;

            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);

            // Force close
            GC.WaitForPendingFinalizers();
            GC.Collect();

            GC.SuppressFinalize(this);

            ReadJsonAppInfo.Read.ReadAppInfo(Path.Combine(Path.GetTempPath(), "appinfo.json"));
        }

    }
    internal enum DataType : byte
    {
        START = 0,
        STRING,
        INT,
        FLOAT,
        PTR,
        WSTRING,
        COLOR,
        UINT64,
        END,
        INT64 = 10,
        ENDB = 11,
    }
    enum AppinfoType
    {
        AppInfoV1,
        AppInfoV2,
    }

}
