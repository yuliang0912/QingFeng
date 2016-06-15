using System;
using System.Collections.Generic;
using System.Linq;

namespace QingFeng.Common.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// 递归
        /// </summary>
        public static void Recursion<T>(this T col, Action<T> handler, Func<T, IEnumerable<T>> acquireNextCollection)
            where T : class
        {
            col.Recursion(null, 0, (parent, item, index) => handler(item), acquireNextCollection);
        }

        /// <summary>
        /// 递归
        /// </summary>
        public static void Recursion<T>(this T col, Action<T, int> handler, Func<T, IEnumerable<T>> acquireNextCollection)
            where T : class
        {
            col.Recursion(null, 0, (parent, item, index) => handler(item, index), acquireNextCollection);
        }

        /// <summary>
        /// 递归
        /// </summary>
        public static void Recursion<T>(this T col, T parent, Action<T, T> handler, Func<T, IEnumerable<T>> acquireNextCollection)
            where T : class
        {
            col.Recursion(parent, 0, (p, item, index) => handler(p, item), acquireNextCollection);
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col">需要进行递归处理的对象</param>
        /// <param name="parent">当前对象的上一层级对象</param>
        /// <param name="index"></param>
        /// <param name="handler">处理函数，第一个参数T：上一层级对象,第二个参数T：当前要处理的对象，第三个参数为当前对象在集合中的索引位置</param>
        /// <param name="acquireNextCollection">获取递归下一层级</param>
        public static void Recursion<T>(this T col, T parent, int index, Action<T, T, int> handler,
            Func<T, IEnumerable<T>> acquireNextCollection)
            where T : class
        {
            if (col == null)
            {
                return;
            }

            handler(parent, col, index);

            var nextCollection = acquireNextCollection(col);
            if (nextCollection == null || !nextCollection.Any())
            {
                return;
            }

            for (int i = 0, j = nextCollection.Count(); i < j; i++)
            {
                var item = nextCollection.ElementAt(i);
                item.Recursion(col, i, handler, acquireNextCollection);
            }
        }
    }
}
