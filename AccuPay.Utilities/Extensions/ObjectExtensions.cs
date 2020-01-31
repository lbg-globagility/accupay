using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AccuPay.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        public static T CloneJson<T>(this T source)
        {
            return CloneObject(source);
        }

        public static List<T> CloneListJson<T>(this List<T> source)
        {
            var newList = new List<T>();

            if (source == null) return newList;

            foreach (var item in source)
            {
                newList.Add(item.CloneJson());
            }

            return newList;
        }

        private static T CloneObject<T>(T source)
        {
            if (Object.ReferenceEquals(source, null)) return default(T);

            var deserializeSettings = new JsonSerializerSettings()
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source),
                                                    deserializeSettings);
        }
    }
}