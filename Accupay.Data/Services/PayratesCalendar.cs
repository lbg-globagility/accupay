using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class PayratesCalendar
    {
        private readonly IDictionary<DateTime, IPayrate> _payrates;

        public PayratesCalendar(IEnumerable<IPayrate> payrates)
        {
            _payrates = payrates.ToDictionary(p => p.Date);
        }

        public IPayrate Find(DateTime date)
        {
            if (_payrates.ContainsKey(date))
                return _payrates[date];

            return null;
        }

        public IList<IPayrate> LegalHolidays()
        {
            return _payrates.Where(p => p.Value.IsRegularHoliday).Select(p => p.Value).ToList();
        }
    }
}