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

        public static bool NullableEquals<T>(this T source, T comparedTo)
        {
            // if both are null, they are equal
            if (source == null && comparedTo == null)
                return true;

            // if one is null and the other is not, they are not equal
            if ((source == null && comparedTo != null) || (source == null && comparedTo != null))
                return false;

            // if both have value, compare them
            return source.Equals(comparedTo);
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