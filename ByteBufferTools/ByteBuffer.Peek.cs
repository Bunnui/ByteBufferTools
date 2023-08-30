using System.Buffers.Binary;
using System.Text;

namespace ByteBufferTools;

public partial class ByteBuffer
{
    public sbyte PeekSByte() => (sbyte)Peek(1)[0];

    public byte PeekByte() => Peek(1)[0];

    public bool PeekBool() => Peek(1)[0] != 0;

    public bool PeekBool(int length, ByteEndian endian = ByteEndian.Little) => Peek(length)[endian == ByteEndian.Little ? 0 : length - 1] != 0;

    public short PeekInt16(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadInt16LittleEndian(Peek(2))
            : BinaryPrimitives.ReadInt16BigEndian(Peek(2));
    }

    public ushort PeekUInt16(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadUInt16LittleEndian(Peek(2))
            : BinaryPrimitives.ReadUInt16BigEndian(Peek(2));
    }

    public int PeekInt32(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadInt32LittleEndian(Peek(4))
            : BinaryPrimitives.ReadInt32BigEndian(Peek(4));
    }

    public uint PeekUInt32(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadUInt32LittleEndian(Peek(4))
            : BinaryPrimitives.ReadUInt32BigEndian(Peek(4));
    }

    public long PeekInt64(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadInt64LittleEndian(Peek(8))
            : BinaryPrimitives.ReadInt64BigEndian(Peek(8));
    }

    public ulong PeekUInt64(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadUInt64LittleEndian(Peek(8))
            : BinaryPrimitives.ReadUInt64BigEndian(Peek(8));
    }

    public Half PeekHalf(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadHalfLittleEndian(Peek(2))
            : BinaryPrimitives.ReadHalfBigEndian(Peek(2));
    }

    public float PeekFloat(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadSingleLittleEndian(Peek(4))
            : BinaryPrimitives.ReadSingleBigEndian(Peek(4));
    }

    public double PeekDouble(ByteEndian endian = ByteEndian.Little)
    {
        return endian == ByteEndian.Little
            ? BinaryPrimitives.ReadDoubleLittleEndian(Peek(8))
            : BinaryPrimitives.ReadDoubleBigEndian(Peek(8));
    }

    public string PeekString(int length, Encoding encoding)
    {
        return encoding.GetString(Peek(length), 0, length);
    }

    public string PeekString(BytePrefix prefix, Encoding encoding)
    {
        lock (this)
        {
            int prefixLength = 0;
            int stringLength = 0;
            // 写入数据长度前缀信息
            switch (prefix)
            {
                case BytePrefix.Byte:
                    prefixLength = 1;
                    stringLength = PeekByte();
                    break;
                case BytePrefix.Int16LE:
                    prefixLength = 2;
                    stringLength = PeekInt16(ByteEndian.Little);
                    break;
                case BytePrefix.Int32LE:
                    prefixLength = 4;
                    stringLength = PeekInt32(ByteEndian.Little);
                    break;
                case BytePrefix.Int16BE:
                    prefixLength = 2;
                    stringLength = PeekInt16(ByteEndian.Big);
                    break;
                case BytePrefix.Int32BE:
                    prefixLength = 4;
                    stringLength = PeekInt32(ByteEndian.Big);
                    break;
            }
            if (stringLength > 0)
            {
                return encoding.GetString(Peek(prefixLength + stringLength), prefixLength, stringLength);
            }
            return string.Empty;
        }
    }
}