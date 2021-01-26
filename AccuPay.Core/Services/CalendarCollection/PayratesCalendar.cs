using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class PayratesCalendar
    {
        /// <summary>
        /// List of payrates included in this calendar
        /// </summary>
        private readonly IDictionary<DateTime, IPayrate> _payrates;

        private readonly DefaultRates _defaultRates;

        public PayratesCalendar(IEnumerable<IPayrate> payrates, DefaultRates defaultRates)
        {
            _payrates = payrates.ToDictionary(p => p.Date);
            _defaultRates = defaultRates;
        }

        public IPayrate Find(DateTime date)
        {
            if (_payrates.ContainsKey(date))
            {
                return _payrates[date];
            }
            else
            {
                return CreateDefault(date);
            }
        }

        private IPayrate CreateDefault(DateTime date)
        {
            var defaultPayrate = new DefaultPayrate(
                date,
                _defaultRates);

            return defaultPayrate;
        }
    }
}
