using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Helpers
{
    public class PaginatedList<T>
    {
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public IEnumerable<T> Items { get; }

        public PaginatedList(IEnumerable<T> items)
        {
            Items = items;
        }

        public PaginatedList(IEnumerable<T> items, int total)
        {
            Items = items;
            TotalCount = total;
        }

        public PaginatedList(IEnumerable<T> items, int total, int pageSize)
        {
            Items = items;
            TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            TotalCount = total;
        }

        public PaginatedList(IEnumerable<T> items, int total, int pageNumber, int pageSize)
        {
            Items = items;
            TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            PageNumber = pageNumber;
            TotalCount = total;
        }

        public PaginatedList<R> Select<R>(Func<T, R> func)
        {
            var items = Items.Select(func);

            return new PaginatedList<R>(items, TotalCount);
        }
    }
}