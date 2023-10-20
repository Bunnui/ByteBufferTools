using System.Buffers.Binary;
using System.Text;

namespace ByteBufferTools;

/// <summary>
/// 字节缓冲区（写入方法）
/// </summary>
public partial class ByteBuffer
{
    #region 基础

    public void WriteSByte(sbyte value) => Write(new byte[] { (byte)value });

    public void WriteByte(byte value) => Write(new byte[] { value });

    public void WriteBool(bool value) => Write(new byte[] { (byte)(value ? 1 : 0) });

    public void WriteBool(bool value, int length, ByteEndian endian = ByteEndian.Little)
    {
        byte[] buffer = new byte[length > 0 ? length : 1];
        buffer[endian == ByteEndian.Little ? 0 : buffer.Length - 1] = (byte)(value ? 1 : 0);
        Write(buffer);
    }

    public void WriteInt16(short value, ByteEndian endian = ByteEndian.Little)
    {
        Span<byte> buffer = stackalloc byte[sizeof(short)];
        if (endian == ByteEndian.Little)
        {
            BinaryPrimitives.WriteInt16LittleEndian(buffer, value);
        }
        else
        {
            BinaryPrimitives.WriteInt16BigEndian(buffer, value);
        }
        Write(buffer.ToArray());
    }

    public void WriteUInt16(ushort value, ByteEndian endian = ByteEndian.Little)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ushort)];
        if (endian == ByteEndian.Little)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(buffer, value);
        }
        else
        {
            BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
        }
        Write(buffer.ToArray());
    }

    public void WriteInt32(int value, ByteEndian endian = ByteEndian.Little)
    {
        Span<byte> buffer = stackalloc byte[sizeof(int)];
        if (endian == ByteEndian.Little)
        {
            BinaryPrimitives.WriteInt32LittleEndian(buffer, value);
        }
        else
        {
            BinaryPrimitives.WriteInt32BigEndian(buffer, value);
        }
        Write(buffer.ToArray());
    }

    public void WriteUInt32(uint value, ByteEndian endian = ByteEndian.Little)
    {
        Span<byte> buffer = stackalloc byte[sizeof(uint)];
        if (endian == ByteEndian.Little)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(buffer, value);
        }
        else
        {
            BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
        }
        Write(buffer.ToArray());
    }

    public void WriteInt64(long value, ByteEndian endian = ByteEndian.Little)
    {
        Span<byte> buffer = stackalloc byte[sizeof(long)];
        if (endian == ByteEndian.Little)
        {
            BinaryPrimitives.WriteInt64LittleEndian(buffer, value);
        }
        else
        {
            BinaryPrimitives.WriteInt64BigEndian(buffer, value);
        }
        Write(buffer.ToArray());
    }

    public void WriteUInt64(ulong value, ByteEndian endian = ByteEndian.Little)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ulong)];
        if (endian == ByteEndian.Little)
        {
            BinaryPrimitives.WriteUInt64LittleEndian(buffer, value);
        }
        else
        {
            BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
        }
        Write(buffer.ToArray());
    }

    public void WriteHalf(Half value, ByteEndian endian = ByteEndian.Little)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ushort) /* = sizeof(Half) */ ];
        if (endian == ByteEndian.Little)
        {
            BinaryPrimitives.WriteHalfLittleEndian(buffer, value);
        }
        else
        {
            BinaryPrimitives.WriteHalfBigEndian(buffer, value);
        }
        Write(buffer.ToArray());
    }

    public void WriteFloat(float value, ByteEndian endian = ByteEndian.Little)
    {
        Span<byte> buffer = stackalloc byte[sizeof(float)];
        if (endian == ByteEndian.Little)
        {
            BinaryPrimitives.WriteSingleLittleEndian(buffer, value);
        }
        else
        {
            BinaryPrimitives.WriteSingleBigEndian(buffer, value);
        }
        Write(buffer.ToArray());
    }

    public void WriteDouble(double value, ByteEndian endian = ByteEndian.Little)
    {
        Span<byte> buffer = stackalloc byte[sizeof(double)];
        if (endian == ByteEndian.Little)
        {
            BinaryPrimitives.WriteDoubleLittleEndian(buffer, value);
        }
        else
        {
            BinaryPrimitives.WriteDoubleBigEndian(buffer, value);
        }
        Write(buffer.ToArray());
    }

    public void WriteString(string value, Encoding encoding)
    {
        Write(encoding.GetBytes(value));
    }

    public void WriteString(string value, Encoding encoding, BytePrefix prefix)
    {
        lock (_lock)
        {
            int length = 0;
            byte[] buffer = encoding.GetBytes(value);
            // 写入数据长度前缀信息
            switch (prefix)
            {
                case BytePrefix.Byte:
                    length = byte.MaxValue;
                    WriteByte((byte)buffer.Length);
                    break;
                case BytePrefix.Int16LE:
                    length = short.MaxValue;
                    WriteInt16((short)buffer.Length, ByteEndian.Little);
                    break;
                case BytePrefix.Int32LE:
                    length = int.MaxValue;
                    WriteInt32(buffer.Length, ByteEndian.Little);
                    break;
                case BytePrefix.Int16BE:
                    length = short.MaxValue;
                    WriteInt16((short)buffer.Length, ByteEndian.Big);
                    break;
                case BytePrefix.Int32BE:
                    length = int.MaxValue;
                    WriteInt32(buffer.Length, ByteEndian.Big);
                    break;
            }
            if (buffer.Length > length)
            {
                Write(buffer, 0, length);
            }
            else
            {
                Write(buffer);
            }
        }
    }

    #endregion

    #region 扩展基础方法

    public long WriteBytes(byte[] buffer) => Write(buffer);

    public void WriteInt8(byte value) => WriteByte(value);

    public void WriteUInt8(sbyte value) => WriteSByte(value);

    public void WriteString(string value) => WriteString(value);

    public void WriteString(string value, BytePrefix prefix) => WriteString(value, prefix);

    public void Write(sbyte value) => WriteSByte(value);

    public void Write(byte value) => WriteByte(value);

    public void Write(bool value) => WriteBool(value);

    public void Write(bool value, int length, ByteEndian endian = ByteEndian.Little) => WriteBool(value, length, endian);

    public void Write(short value, ByteEndian endian = ByteEndian.Little) => WriteInt16(value, endian);

    public void Write(ushort value, ByteEndian endian = ByteEndian.Little) => WriteUInt16(value, endian);

    public void Write(int value, ByteEndian endian = ByteEndian.Little) => WriteInt32(value, endian);

    public void Write(uint value, ByteEndian endian = ByteEndian.Little) => WriteUInt32(value, endian);

    public void Write(long value, ByteEndian endian = ByteEndian.Little) => WriteInt64(value, endian);

    public void Write(ulong value, ByteEndian endian = ByteEndian.Little) => WriteUInt64(value, endian);

    public void Write(Half value, ByteEndian endian = ByteEndian.Little) => WriteHalf(value, endian);

    public void Write(float value, ByteEndian endian = ByteEndian.Little) => WriteFloat(value, endian);

    public void Write(double value, ByteEndian endian = ByteEndian.Little) => WriteDouble(value, endian);

    public void Write(string value, Encoding encoding) => WriteString(value, encoding);

    public void Write(string value, Encoding encoding, BytePrefix prefix) => WriteString(value, encoding, prefix);

    public void Write(string value) => WriteString(value);

    public void Write(string value, BytePrefix prefix) => WriteString(value, prefix);

    #endregion
}
