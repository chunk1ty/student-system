using System.Collections.Generic;
using System.Text;

namespace StudentSystem.Common.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetUtf8Bytes(this string str)
        {
            return str == null ? new List<byte>().ToArray() : Encoding.UTF8.GetBytes(str);
        }
    }
}