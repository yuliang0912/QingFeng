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


        public static string ConvertInterestRate(this decimal d1, decimal d2)
        {
            var difference = d1 - d2;

            return $"{difference / d2:0.00%}";
        }

        public static string GetOrderStyle(this AgentEnums.MasterOrderStatus orderStatus)
        {
            var style = string.Empty;
            switch (orderStatus)
            {
                case AgentEnums.MasterOrderStatus.待发货:
                    style = "color:#428bca;";
                    break;
                case AgentEnums.MasterOrderStatus.已发货:
                    style = "color:#5cb85c;";
                    break;
                case AgentEnums.MasterOrderStatus.已完成:
                    style = "color:#5cb85c;";
                    break;
                case AgentEnums.MasterOrderStatus.已取消:
                    style = "color:#ccc;";
                    break;
                case AgentEnums.MasterOrderStatus.异常:
                    style = "color:#f00;";
                    break;
                case AgentEnums.MasterOrderStatus.待支付:
                    style = "color:#f89406;";
                    break;
            }
            return style;
        }


        public static string GetOrderStyle(this AgentEnums.OrderDetailStatus orderStatus)
        {
            var style = string.Empty;
            switch (orderStatus)
            {
                case AgentEnums.OrderDetailStatus.待发货:
                    style = "color:#428bca;";
                    break;
                case AgentEnums.OrderDetailStatus.已发货:
                    style = "color:#5cb85c;";
                    break;
                case AgentEnums.OrderDetailStatus.无货取消:
                    style = "color:#5cb85c;";
                    break;
                case AgentEnums.OrderDetailStatus.已取消:
                    style = "color:#ccc;";
                    break;
                case AgentEnums.OrderDetailStatus.无货异常:
                    style = "color:#f00;";
                    break;
            }
            return style;
        }
    }
}
