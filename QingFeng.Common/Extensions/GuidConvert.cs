using System;

namespace QingFeng.Common.Extensions
{
    public class GuidConvert
    {
        /// <summary>
        /// 根据GUID获取16位的唯一字符串
        /// </summary>
        /// <returns></returns>
        public static string ToString16()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= b + 1;
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 根据GUID获取19位的唯一数字序列
        /// </summary>
        /// <returns></returns>
        public static long ToLong()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>  
        /// 生成22位唯一的数字 并发可用  
        /// </summary>  
        /// <returns></returns>
        public static string ToUniqueId()
        {
            System.Threading.Thread.Sleep(1); //保证yyyyMMddHHmmssffff唯一  
            Random d = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            string strUnique = DateTime.Now.ToString("yyyyMMddHHmmssffff") + d.Next(1000, 9999);
            return strUnique;
        }

        /// <summary>
        /// 创建随即的唯一Id
        /// </summary>
        /// <returns></returns>
        public static long GeneratePk()
        {
            var buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0)/1000;
        }
    }
}
