using System.Collections.Generic;

namespace AccuPay.Core.Services
{
    public class BatchApplyResult<T>
    {
        public IReadOnlyCollection<T> AddedList { get; }
        public IReadOnlyCollection<T> UpdatedList { get; }

        public BatchApplyResult(IReadOnlyCollection<T> addedList, IReadOnlyCollection<T> updatedList)
        {
            AddedList = addedList;
            UpdatedList = updatedList;
        }
    }
}