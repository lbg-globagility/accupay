using AccuPay.Data.Entities;
using System;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    internal static class SalaryQueryHelper
    {
        internal static IOrderedQueryable<Salary> GetLatestSalaryQuery(IQueryable<Salary> query, DateTime? cutOffEnd)
        {
            cutOffEnd = cutOffEnd ?? DateTime.Now;

            return query
                .Where(x => x.EffectiveFrom <= cutOffEnd)
                .OrderByDescending(x => x.EffectiveFrom);
        }
    }
}
