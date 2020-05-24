using AccuPay.Data.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace AccuPay.Data.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderBy<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, string direction)
        {
            if (direction == "desc")
            {
                return query.OrderByDescending(keySelector);
            }
            else
            {
                return query.OrderBy(keySelector);
            }
        }

        public static IOrderedQueryable<T> ThenBy<T, TKey>(this IOrderedQueryable<T> query, Expression<Func<T, TKey>> keySelector, string direction)
        {
            if (direction == "desc")
            {
                return query.ThenByDescending(keySelector);
            }
            else
            {
                return query.ThenBy(keySelector);
            }
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> query, PageOptions options)
        {
            if (options.All)
            {
                return query;
            }

            return query.Skip(options.Offset).Take(options.PageSize);
        }
    }
}