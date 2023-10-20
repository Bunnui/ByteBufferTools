using System.Buffers.Binary;
using System.Text;

namespace ByteBufferTools;

/// <summary>
/// 字节缓冲区（读取方法）
/// </summary>
public partial class ByteBuffer
{
    public sbyte ReadSByte() => (sbyte)Read(1)[0];

    public byte ReadByte() => Read(1)[0];

    public bool ReadBool() => Read(1)[0] != 0;

    public bool ReadBool(int length, ByteEndian endian = ByteEndian.Little) => Read(length)[endian == ByteEndian.Little ? 0 : length - 1] != 0;

    public short ReadInt16(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadInt16LittleEndian(Read(2))
            : BinaryPrimitives.ReadInt16BigEndian(Read(2));
    }

    public ushort ReadUInt16(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadUInt16LittleEndian(Read(2))
            : BinaryPrimitives.ReadUInt16BigEndian(Read(2));
    }

    public int ReadInt32(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadInt32LittleEndian(Read(4))
            : BinaryPrimitives.ReadInt32BigEndian(Read(4));
    }

    public uint ReadUInt32(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadUInt32LittleEndian(Read(4))
            : BinaryPrimitives.ReadUInt32BigEndian(Read(4));
    }

    public long ReadInt64(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadInt64LittleEndian(Read(8))
            : BinaryPrimitives.ReadInt64BigEndian(Read(8));
    }

    public ulong ReadUInt64(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadUInt64LittleEndian(Read(8))
            : BinaryPrimitives.ReadUInt64BigEndian(Read(8));
    }

    public Half ReadHalf(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadHalfLittleEndian(Read(2))
            : BinaryPrimitives.ReadHalfBigEndian(Read(2));
    }

    public float ReadFloat(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadSingleLittleEndian(Read(4))
            : BinaryPrimitives.ReadSingleBigEndian(Read(4));
    }

    public double ReadDouble(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadDoubleLittleEndian(Read(8))
            : BinaryPrimitives.ReadDoubleBigEndian(Read(8));
    }

    public string ReadString(int length, Encoding encoding)
    {
        return encoding.GetString(Read(length), 0, (int)length);
    }

    public string ReadString(BytePrefix prefix, Encoding encoding)
    {
        lock (_lock)
        {
            int length = 0;
            // 写入数据长度前缀信息
            switch (prefix)
            {
                case BytePrefix.Byte:
                    length = ReadByte();
                    break;
                case BytePrefix.Int16LE:
                    length = ReadInt16(ByteEndian.Little);
                    break;
                case BytePrefix.Int32LE:
                    length = ReadInt32(ByteEndian.Little);
                    break;
                case BytePrefix.Int16BE:
                    length = ReadInt16(ByteEndian.Big);
                    break;
                case BytePrefix.Int32BE:
                    length = ReadInt32(ByteEndian.Big);
                    break;
            }
            if (length > 0)
            {
                return ReadString(length, encoding);
            }
            return string.Empty;
        }
    }
}
