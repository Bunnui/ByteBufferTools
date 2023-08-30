namespace ByteBufferTools;

public enum ByteEndian { Little, Big }
public enum BytePrefix { Byte, Int16LE, Int32LE, Int16BE, Int32BE }

/// <summary>
/// 字节缓冲区
/// </summary>
public partial class ByteBuffer
{
    protected byte[] _buffer;
    protected long _readPosition;
    protected long _writePosition;

    public ByteBuffer(byte[]? buffer = null)
    {
        _buffer = buffer ?? Array.Empty<byte>();
        _writePosition = buffer == null ? 0 : buffer.LongLength;
        _readPosition = 0;
        if (buffer != null && buffer.LongLength > 0)
        {
            Write(buffer, 0, buffer.LongLength);
        }
    }

    #region Method

    public byte[] ToArray()
    {
        if (_writePosition > 0 && _buffer.LongLength > 0)
        {
            byte[] bytes = new byte[_writePosition];
            Array.Copy(_buffer, 0, bytes, 0, _writePosition);
            return bytes;
        }
        return Array.Empty<byte>();
    }

    /// <summary>
    /// 到十六进制字符串
    /// </summary>
    /// <param name="singleHexFormat">单个字节的十六进制格式，注意：%hex% 是变量，会替换成字节，如果有需要%字符，那么请多加一个%进行转义</param>
    /// <param name="singleHexUppercase">单个字节的十六进制大写</param>
    /// <param name="singleHexFillZero">单个字节的十六进制不足一位则填充零</param>
    /// <param name="singleHexSplicer">拼接符号</param>
    /// <returns></returns>
    public string ToHexString(bool singleHexUppercase = StringExtensionMethods.DefaultSingleHexUppercase, bool singleHexFillZero = StringExtensionMethods.DefaultSingleHexFillZero, string? singleHexSplicer = StringExtensionMethods.DefaultSingleHexSplicer, string? singleHexFormat = StringExtensionMethods.DefaultSingleHexFormat)
    {
        return ToArray().ToHexString(singleHexUppercase, singleHexFillZero, singleHexSplicer, singleHexFormat);
    }

    #endregion

    #region Write

    /// <summary>
    /// 扩展缓冲区大小
    /// </summary>
    /// <param name="length">长度</param>
    private void ExtendBufferSize(long length)
    {
        lock (this)
        {
            if (length > 0 && _buffer.LongLength < length)
            {
                // 先至少扩展到256字节大小
                long newLength = Math.Max(length, 256);
                // 如果新长度小于缓冲区的两倍，那么就扩展缓冲区长度的两倍
                if (newLength < _buffer.LongLength * 2)
                {
                    newLength = _buffer.LongLength * 2;
                }
                // 为避免双倍增加的泛滥创建新的缓冲区，新长度到达数组的最大值时
                // 不在增加双倍，而使用扩展的长度值
                if (_buffer.LongLength * 2 > Array.MaxLength)
                {
                    newLength = Math.Max(length, Array.MaxLength);
                }
                byte[] newBuffer = new byte[newLength];
                // 缓冲区有数据才有拷贝的意义
                if (_buffer.LongLength > 0)
                {
                    Array.Copy(_buffer, 0, newBuffer, 0, _buffer.LongLength);
                }
                _buffer = newBuffer;
            }
        }
    }

    public long Write(byte[] buffer, long offset, long count)
    {
        lock (this)
        {
            if (count > 0)
            {
                long remainderBufferCount = buffer.LongLength - offset;
                if (count > remainderBufferCount)
                {
                    count = remainderBufferCount;
                }
                long newWritePosition = _writePosition + count;
                // 如果新写入后的位置超过缓冲区，那么进行扩展字节缓冲区空间大小
                if (newWritePosition > _buffer.LongLength)
                {
                    ExtendBufferSize(newWritePosition);
                }
                Array.Copy(buffer, offset, _buffer, _writePosition, count);
                _writePosition = newWritePosition;
                return newWritePosition;
            }
            return 0;
        }
    }

    public long Write(byte[] buffer) => Write(buffer, 0, buffer.LongLength);


    #endregion

    #region Peek

    public long Peek(byte[] buffer, long offset, long count, bool throwException = true)
    {
        lock (this)
        {
            if (count > 0)
            {
                // 这个无效长度就是缓冲区长度减去已写入的数据，剩余的预先申请内存空间长度
                long invalidLength = _buffer.Length - _writePosition;
                // 缓冲区有效数据的读取的剩余长度
                long remainingLength = _buffer.Length - invalidLength - _readPosition;
                // 如果已经没有有效数据可以读取那么抛出异常
                if (remainingLength <= 0)
                {
                    if (throwException)
                    {
                        throw new EndOfStreamException();
                    }
                    return 0;
                }
                // 计算可以读取的有效的长度，溢出的长度不进行读取
                long effectiveLength = remainingLength > count ? count : remainingLength;
                Array.Copy(_buffer, _readPosition, buffer, offset, effectiveLength);
                return effectiveLength;
            }
            return 0;
        }
    }

    public byte[] Peek(long length, bool throwException = true)
    {
        lock (this)
        {
            if (length > 0)
            {
                long offset = 0;
                byte[] buffer = new byte[length];
                while (offset < buffer.LongLength)
                {
                    long count = Peek(buffer, offset, buffer.LongLength - offset, throwException);
                    if (count == 0) { break; }
                    offset += count;
                }
                return buffer;
            }
            return Array.Empty<byte>();
        }
    }

    #endregion

    #region Read

    public long Read(byte[] buffer, long offset, long count, bool throwException = true)
    {
        long len = Peek(buffer, offset, count, throwException);
        if (len > 0) { _readPosition += len; }
        return len;
    }

    public byte[] Read(long length, bool throwException = true)
    {
        lock (this)
        {
            if (length > 0)
            {
                long offset = 0;
                byte[] buffer = new byte[length];
                while (offset < buffer.LongLength)
                {
                    long count = Read(buffer, offset, buffer.LongLength - offset, throwException);
                    if (count == 0) { break; }
                    offset += count;
                }
                return buffer;
            }
            return Array.Empty<byte>();
        }
    }

    #endregion

    #region Operators

    public static ByteBuffer operator +(ByteBuffer a, ByteBuffer b)
    {
        var buffer = new ByteBuffer();
        buffer.Write(a.ToArray());
        buffer.Write(b.ToArray());
        return buffer;
    }

    public static ByteBuffer operator +(byte[] a, ByteBuffer b)
    {
        var buffer = new ByteBuffer();
        buffer.Write(a);
        buffer.Write(b.ToArray());
        return buffer;
    }

    public static ByteBuffer operator +(ByteBuffer a, byte[] b)
    {
        var buffer = new ByteBuffer();
        buffer.Write(a.ToArray());
        buffer.Write(b);
        return buffer;
    }

    public static bool operator ==(ByteBuffer a, ByteBuffer b)
    {
        return a is null ? b is null : a.Equals(b);
    }

    public static bool operator !=(ByteBuffer a, ByteBuffer b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        ByteBuffer? other = obj as ByteBuffer;
        return other is not null
            && _buffer.Length == other._buffer.Length
            && (_buffer == null || other._buffer == null || Enumerable.SequenceEqual(_buffer, other._buffer));
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    #endregion
}

