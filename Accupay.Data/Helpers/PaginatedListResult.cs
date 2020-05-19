using System.Collections.Generic;

namespace AccuPay.Data.Helpers
{
    public class PaginatedListResult<T>
    {
        public ICollection<T> List { get; }
        public int TotalCount { get; }

        public PaginatedListResult(ICollection<T> list, int totalCount)
        {
            List = list;
            TotalCount = totalCount;
        }
    }
}