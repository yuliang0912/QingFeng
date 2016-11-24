using System;

namespace QingFeng.Common.Extensions
{
    public static class StringExtensions
    {
        public static string CutString(this string str, int length, string additional = "")
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (length < 0 || str.Length <= length)
            {
                return str;
            }
            if (!string.IsNullOrEmpty(additional))
            {
                return str.Substring(0, length) + additional;
            }
            return str.Substring(0, length);
        }

        public static string GetRandomString()
        {
            return Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        }

        public static string FillZeroNumber(int number, int totalLength)
        {
            var numLength = number.ToString().Length;
            if (numLength >= totalLength)
            {
                return number.ToString();
            }
            var tempString = string.Empty;
            while (totalLength-- > numLength)
            {
                tempString += "0";
            }
            return string.Concat(tempString, number);
        }

        public static string FormatSqlLikeString(this string str, int type = 0)
        {
            return string.IsNullOrWhiteSpace(str)
                ? str
                : $"{(type == 0 || type == 1 ? "%" : string.Empty)}{str.Trim()}{(type == 0 || type == 2 ? "%" : string.Empty)}";
        }
    }
}
