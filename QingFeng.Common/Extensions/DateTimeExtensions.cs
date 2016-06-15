using System;

namespace QingFeng.Common.Extensions
{
	public static class DateTimeExtensions
	{
	    private static readonly DateTime SpanDate = new DateTime(1970, 1, 1);

		/// <summary>
		/// DateTime转换时间戳(秒)
		/// </summary>
		public static int Epoch(this DateTime time)
		{
			return (int)(time.ToUniversalTime() - SpanDate).TotalSeconds;
		}

		/// <summary>
		/// 时间戳(秒)转DateTime
		/// </summary>
		public static DateTime FromEpoch(int time)
		{
			return SpanDate.AddSeconds(time).ToLocalTime();
		}
	}
}
