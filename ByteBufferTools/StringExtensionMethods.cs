using System.Text;

namespace ByteBufferTools;

public static class StringExtensionMethods
{
    public const string DefaultSingleHexFormat = "%hex%";
    public const string DefaultSingleHexSplicer = " ";
    public const bool DefaultSingleHexFillZero = true;
    public const bool DefaultSingleHexUppercase = true;

    /// <summary>
    /// 到十六进制字符串
    /// </summary>
    /// <param name="bytes">待转换的字节数组</param>
    /// <param name="singleHexUppercase">单个字节的十六进制大写</param>
    /// <param name="singleHexFillZero">单个字节的十六进制不足一位则填充零</param>
    /// <param name="singleHexSplicer">拼接符号</param>
    /// <param name="singleHexFormat">单个字节的十六进制格式，注意：%hex% 是变量，会替换成字节，如果有需要%字符，那么请多加一个%进行转义</param>
    /// <returns></returns>
    public static string ToHexString(this byte[] bytes, bool singleHexUppercase = DefaultSingleHexUppercase, bool singleHexFillZero = DefaultSingleHexFillZero, string? singleHexSplicer = DefaultSingleHexSplicer, string? singleHexFormat = DefaultSingleHexFormat)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < bytes.LongLength; i++)
        {
            string hexFormat = singleHexUppercase ? "X" : "x";
            if (singleHexFillZero) { hexFormat += "2"; }
            string hex = bytes[i].ToString(hexFormat);
            string? str = null;
            if (singleHexFormat != null)
            {
                int start = -1;
                int end = -1;
                bool tag = false;
                int lastIndex = 0;
                while (true)
                {
                    int index = singleHexFormat.IndexOf("%", lastIndex);
                    if (index == -1) { break; }
                    lastIndex = index + 1;
                    // 检查转义符号情况
                    if (index + 1 < singleHexFormat.Length && singleHexFormat[index + 1] == '%')
                    {
                        lastIndex += 1;
                        continue;
                    }
                    if (tag)
                    {
                        end = index;
                        break;
                    }
                    start = index + 1;
                    tag = true;

                }
                if (start != -1 && end != -1)
                {
                    string middle = singleHexFormat[start..end].Trim();
                    if (middle.Equals("hex", StringComparison.OrdinalIgnoreCase))
                    {
                        string left = singleHexFormat[..(start - 1)];
                        string right = singleHexFormat[(end + 1)..];
                        str = $"{left}{hex}{right}";
                    }
                }
                else
                {
                    str = singleHexFormat;
                }
            }
            if (string.IsNullOrWhiteSpace(str))
            {
                str = hex;
            }
            stringBuilder.Append(str);
            if (i < bytes.LongLength - 1)
            {
                stringBuilder.Append(singleHexSplicer);
            }
        }
        return stringBuilder.ToString();
    }

    //TODO: 未实现
    //public static byte[] ToHexBytes(this string str, bool singleHexFillZero = true, string? splicer = " ", string? singleHexFormat = "%hex%")
    //{
    //    List<byte> bytes = new List<byte>();
    //    // 处理分割符号
    //    if (!string.IsNullOrWhiteSpace(splicer))
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        string[] parts = str.Split(splicer);
    //        foreach (string part in parts)
    //        {
    //            sb.Append(part);
    //        }
    //        str = sb.ToString();
    //    }
    //    Regex.Replace(str, "[aeiou]", "", RegexOptions.IgnoreCase);

    //    return bytes.ToArray();
    //}
}
