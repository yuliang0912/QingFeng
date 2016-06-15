using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QingFeng.Common.Extensions
{
    public static class DataContractExtensions
    {
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> ParamCache =
            new ConcurrentDictionary<Type, List<PropertyInfo>>();

        /// <summary>
        /// 去除或者过滤对象中指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="isRequire">true:过滤 false:保留</param>
        /// <param name="fields">需要过滤or保留的字段</param>
        /// <returns></returns>
        public static object SimpleModel<T>(T model, bool isRequire, params string[] fields)
        {
            if (model == null)
            {
                return null;
            }

            var simpleModel = new Dictionary<string, object>();
            List<PropertyInfo> properties;
            if (!ParamCache.TryGetValue(model.GetType(), out properties))
            {
                properties = model.GetType()
                    .GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public)
                    .ToList();
                ParamCache[model.GetType()] = properties;
            }

            foreach (var pi in properties.Where(pi => (isRequire && fields.Contains(pi.Name)) || (!isRequire && !fields.Contains(pi.Name))))
            {
                simpleModel[pi.Name] = pi.GetValue(model);
            }
            

            return simpleModel;
        }
    }
}
