using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpBizz.Http.Tests
{
    static class TestUtils
    {
        public static Encoding HttpEncoding = Encoding.ASCII; // Encoding.GetEncoding("ISO-8859-1")

        public static string[] SeparateValues(string values, ValuesTypeEnum valuesType)
        {
            Match match;
            switch (valuesType)
            {
                case ValuesTypeEnum.CommaSeparated:
                    match = Regex.Match(values, @"^((\s*(?<value>[^,]+?)\s*),)*(\s*(?<value>[^,]+?)\s*)$",
                        RegexOptions.ExplicitCapture | RegexOptions.Compiled);
                    break;
                case ValuesTypeEnum.ProductComments:
                    match = Regex.Match(values, @"^\s*((?<value>((?<product>[^\(\s/]+(/[^\(\s/]+)?)|(?<comment>\([^\(\)]*\))))\s*)*$",
                        RegexOptions.ExplicitCapture | RegexOptions.Compiled);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("valuesType");
            }
            if (!match.Success) return new string[0];
            var valueGroup = match.Groups["value"];
            if (!valueGroup.Success) return new string[0];
            var captures = valueGroup.Captures.Cast<Capture>().Select(c => c.Value).ToArray();
            return captures;
        }

        public static int GetStreamLength(Stream stream)
        {
            if (stream == null) return 0;
            if (stream.CanSeek) return (int) stream.Length;
            var buff = new byte[1024];
            var count = 0;
            int currCount;
            while ((currCount = stream.Read(buff, 0, 1024)) > 0)
                count += currCount;
            return count;
        }

        public static string AsString(byte[] array)
        {
            using (var stream = new MemoryStream(array, false))
            {
                return AsString(stream);
            }
        }

        public static string AsString(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            var sb = new StringBuilder();
            var isAscii = true;
            int b;

            while ((b = stream.ReadByte()) >= 0)
            {
                var currentAscii = b >= 32 && b <= 127;
                var txt = currentAscii ? new string((char) b, 1) : b.ToString("X2");
                if (currentAscii != isAscii)
                    if (currentAscii)
                        sb.Append(" ");
                    else
                        sb.Append(@"\x");
                sb.Append(txt);
                isAscii = currentAscii;
            }

            return sb.ToString();
        }
    }
    public enum ValuesTypeEnum
    {
        CommaSeparated,
        ProductComments,
    }
}