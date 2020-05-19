using AccuPay.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AccuPay.Web.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, string direction)
        {
            if (direction == "desc")
            {
                return source.OrderByDescending(keySelector);
            }
            else
            {
                return source.OrderBy(keySelector);
            }
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, PageOptions options)
        {
            if (options.All)
            {
                return source;
            }

            return source.Skip(options.Offset).Take(options.PageSize);
        }
    }
}
