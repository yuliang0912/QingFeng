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
            while (totalLength - numLength == 0)
            {
                tempString += "0";
                totalLength--;
            }
            return string.Concat(tempString, number);
        }
    }
}
