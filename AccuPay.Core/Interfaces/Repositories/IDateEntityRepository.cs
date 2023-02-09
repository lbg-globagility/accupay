using AccuPay.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Repositories
{
    public interface IDateEntityRepository
    {
        Task<ICollection<DateEntity>> GetByYearAsync(int year);

        Task<ICollection<DateEntity>> GetByYearAndMonthAsync(int year, int month);

        Task<ICollection<DateEntity>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
}
