using ByteBufferTools;
using System.Text;

// 测试数据
ByteEndian writeByteEndian = ByteEndian.Little;
ByteEndian readByteEndian = ByteEndian.Little;
Encoding encoding = Encoding.UTF8;
string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
// 字节数组
byte[] bytes = Encoding.UTF8.GetBytes(str);
string hexBytes = bytes.ToHexString();
// 布尔
bool @bool = true;
int @boolLength = 4;
// 有符合
sbyte @sbyte = sbyte.MinValue;
short int16 = short.MinValue;
int int32 = int.MinValue;
long int64 = long.MinValue;
Half half = Half.MinValue;
float @float = float.MinValue;
double @double = double.MinValue;
// 无符号（因为无符号最大值大小端都一样，无法检验数据，所以-1）
byte @byte = byte.MaxValue - 1;
ushort uint16 = ushort.MaxValue - 1;
uint uint32 = uint.MaxValue - 1;
ulong uint64 = ulong.MaxValue - 1;


// 创建字节缓冲区对象
ByteBuffer byteBuffer = new ByteBuffer();
// 写入测试
{
    // 字节数组
    byteBuffer.Write(bytes, 0, bytes.LongLength);

    // 字符串
    byteBuffer.WriteString(str, encoding);
    byteBuffer.WriteString(str, encoding, BytePrefix.Byte);
    byteBuffer.WriteString(str, encoding, BytePrefix.Int16LE);
    byteBuffer.WriteString(str, encoding, BytePrefix.Int32LE);
    byteBuffer.WriteString(str, encoding, BytePrefix.Int16BE);
    byteBuffer.WriteString(str, encoding, BytePrefix.Int32BE);

    // 布尔
    byteBuffer.WriteBool(@bool);
    byteBuffer.WriteBool(@bool, @boolLength, writeByteEndian);

    // 有符合
    byteBuffer.WriteSByte(@sbyte);
    byteBuffer.WriteInt16(int16, writeByteEndian);
    byteBuffer.WriteInt32(int32, writeByteEndian);
    byteBuffer.WriteInt64(int64, writeByteEndian);
    // 无符号
    byteBuffer.WriteByte(@byte);
    byteBuffer.WriteUInt16(uint16, writeByteEndian);
    byteBuffer.WriteUInt32(uint32, writeByteEndian);
    byteBuffer.WriteUInt64(uint64, writeByteEndian);
    // 浮点数
    byteBuffer.WriteHalf(half, writeByteEndian);
    byteBuffer.WriteFloat(@float, writeByteEndian);
    byteBuffer.WriteDouble(@double, writeByteEndian);
}

// 重载方法测试
{
    // 字符串
    byteBuffer.Write(str, encoding);
    byteBuffer.Write(str, encoding, BytePrefix.Byte);
    byteBuffer.Write(str, encoding, BytePrefix.Int16LE);
    byteBuffer.Write(str, encoding, BytePrefix.Int32LE);
    byteBuffer.Write(str, encoding, BytePrefix.Int16BE);
    byteBuffer.Write(str, encoding, BytePrefix.Int32BE);

    // 布尔
    byteBuffer.Write(@bool);
    byteBuffer.Write(@bool, @boolLength, writeByteEndian);

    // 有符合
    byteBuffer.Write(@sbyte);
    byteBuffer.Write(int16, writeByteEndian);
    byteBuffer.Write(int32, writeByteEndian);
    byteBuffer.Write(int64, writeByteEndian);
    // 无符号
    byteBuffer.Write(@byte);
    byteBuffer.Write(uint16, writeByteEndian);
    byteBuffer.Write(uint32, writeByteEndian);
    byteBuffer.Write(uint64, writeByteEndian);
    // 浮点数
    byteBuffer.Write(half, writeByteEndian);
    byteBuffer.Write(@float, writeByteEndian);
    byteBuffer.Write(@double, writeByteEndian);
}



// 校验内容
{
    Console.WriteLine("--------------十六进制字符串，读取--------------");
    byte[] data = byteBuffer.Read(bytes.LongLength);
    string hexData = data.ToHexString();
    Console.WriteLine("[{0}] \n[读] {1} \n[原] {2}", hexData.Equals(hexBytes), hexData, hexBytes);
    Console.WriteLine();


    Console.WriteLine("--------------原始字符串--------------");
    Console.WriteLine(str);
    Console.WriteLine("--------------读取字符串--------------");
    Console.WriteLine("[{0}] [Length] \n{1} ", str.Equals(byteBuffer.PeekString(bytes.Length, encoding)), byteBuffer.ReadString(bytes.Length, encoding));
    Console.WriteLine("[{0}] [Byte] \n{1} ", str.Equals(byteBuffer.PeekString(BytePrefix.Byte, encoding)), byteBuffer.ReadString(BytePrefix.Byte, encoding));
    Console.WriteLine("[{0}] [Int16LE] \n{1} ", str.Equals(byteBuffer.PeekString(BytePrefix.Int16LE, encoding)), byteBuffer.ReadString(BytePrefix.Int16LE, encoding));
    Console.WriteLine("[{0}] [Int32LE] \n{1} ", str.Equals(byteBuffer.PeekString(BytePrefix.Int32LE, encoding)), byteBuffer.ReadString(BytePrefix.Int32LE, encoding));
    Console.WriteLine("[{0}] [Int16BE] \n{1} ", str.Equals(byteBuffer.PeekString(BytePrefix.Int16BE, encoding)), byteBuffer.ReadString(BytePrefix.Int16BE, encoding));
    Console.WriteLine("[{0}] [Int32BE] \n{1} ", str.Equals(byteBuffer.PeekString(BytePrefix.Int32BE, encoding)), byteBuffer.ReadString(BytePrefix.Int32BE, encoding));
    Console.WriteLine();

    Console.WriteLine("--------------布尔值--------------");
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekBool() == @bool, byteBuffer.ReadBool(), @bool);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekBool(boolLength, readByteEndian) == @bool, byteBuffer.ReadBool(boolLength, readByteEndian), @bool);
    Console.WriteLine();

    Console.WriteLine("--------------无符号数值--------------");
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekSByte() == @sbyte, byteBuffer.ReadSByte(), @sbyte);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekInt16(readByteEndian) == int16, byteBuffer.ReadInt16(readByteEndian), int16);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekInt32(readByteEndian) == int32, byteBuffer.ReadInt32(readByteEndian), int32);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekInt64(readByteEndian) == int64, byteBuffer.ReadInt64(readByteEndian), int64);
    Console.WriteLine();

    Console.WriteLine("--------------有符号数值--------------");
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekByte() == @byte, byteBuffer.ReadByte(), @byte);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekUInt16(readByteEndian) == uint16, byteBuffer.ReadUInt16(readByteEndian), uint16);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekUInt32(readByteEndian) == uint32, byteBuffer.ReadUInt32(readByteEndian), uint32);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekUInt64(readByteEndian) == uint64, byteBuffer.ReadUInt64(readByteEndian), uint64);
    Console.WriteLine();

    Console.WriteLine("--------------浮点数数值--------------");
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekHalf(readByteEndian) == half, byteBuffer.ReadHalf(readByteEndian), half);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekFloat(readByteEndian) == @float, byteBuffer.ReadFloat(readByteEndian), @float);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekDouble(readByteEndian) == @double, byteBuffer.ReadDouble(readByteEndian), @double);
    Console.WriteLine();
}

Console.WriteLine();

// 重载校验内容
{
    Console.WriteLine("--------------原始字符串--------------");
    Console.WriteLine(str);
    Console.WriteLine("--------------读取字符串--------------");
    Console.WriteLine("[{0}] [Length] \n{1} ", str.Equals(byteBuffer.PeekString(bytes.Length, encoding)), byteBuffer.ReadString(bytes.Length, encoding));
    Console.WriteLine("[{0}] [Byte] \n{1} ", str.Equals(byteBuffer.PeekString(BytePrefix.Byte, encoding)), byteBuffer.ReadString(BytePrefix.Byte, encoding));
    Console.WriteLine("[{0}] [Int16LE] \n{1} ", str.Equals(byteBuffer.PeekString(BytePrefix.Int16LE, encoding)), byteBuffer.ReadString(BytePrefix.Int16LE, encoding));
    Console.WriteLine("[{0}] [Int32LE] \n{1} ", str.Equals(byteBuffer.PeekString(BytePrefix.Int32LE, encoding)), byteBuffer.ReadString(BytePrefix.Int32LE, encoding));
    Console.WriteLine("[{0}] [Int16BE] \n{1} ", str.Equals(byteBuffer.PeekString(BytePrefix.Int16BE, encoding)), byteBuffer.ReadString(BytePrefix.Int16BE, encoding));
    Console.WriteLine("[{0}] [Int32BE] \n{1} ", str.Equals(byteBuffer.PeekString(BytePrefix.Int32BE, encoding)), byteBuffer.ReadString(BytePrefix.Int32BE, encoding));
    Console.WriteLine();

    Console.WriteLine("--------------布尔值--------------");
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekBool() == @bool, byteBuffer.ReadBool(), @bool);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekBool(boolLength, readByteEndian) == @bool, byteBuffer.ReadBool(boolLength, readByteEndian), @bool);
    Console.WriteLine();

    Console.WriteLine("--------------无符号数值--------------");
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekSByte() == @sbyte, byteBuffer.ReadSByte(), @sbyte);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekInt16(readByteEndian) == int16, byteBuffer.ReadInt16(readByteEndian), int16);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekInt32(readByteEndian) == int32, byteBuffer.ReadInt32(readByteEndian), int32);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekInt64(readByteEndian) == int64, byteBuffer.ReadInt64(readByteEndian), int64);
    Console.WriteLine();

    Console.WriteLine("--------------有符号数值--------------");
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekByte() == @byte, byteBuffer.ReadByte(), @byte);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekUInt16(readByteEndian) == uint16, byteBuffer.ReadUInt16(readByteEndian), uint16);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekUInt32(readByteEndian) == uint32, byteBuffer.ReadUInt32(readByteEndian), uint32);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekUInt64(readByteEndian) == uint64, byteBuffer.ReadUInt64(readByteEndian), uint64);
    Console.WriteLine();

    Console.WriteLine("--------------浮点数数值--------------");
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekHalf(readByteEndian) == half, byteBuffer.ReadHalf(readByteEndian), half);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekFloat(readByteEndian) == @float, byteBuffer.ReadFloat(readByteEndian), @float);
    Console.WriteLine("[{0}] {1}, {2}", byteBuffer.PeekDouble(readByteEndian) == @double, byteBuffer.ReadDouble(readByteEndian), @double);
    Console.WriteLine();
}