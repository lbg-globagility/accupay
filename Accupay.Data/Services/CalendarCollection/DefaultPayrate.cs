using System;

namespace AccuPay.Data.Services
{
    public class DefaultPayrate : IPayrate
    {
        private readonly DefaultRates _defaultRates;

        public DefaultPayrate(
            DateTime date,
            DefaultRates defaultRates)
        {
            Date = date;
            _defaultRates = defaultRates;
        }

        public DateTime Date { get; private set; }

        public decimal RegularRate => _defaultRates.RegularRate;

        public decimal OvertimeRate => _defaultRates.OvertimeRate;

        public decimal NightDiffRate => _defaultRates.NightDiffRate;

        public decimal NightDiffOTRate => _defaultRates.NightDiffOTRate;

        public decimal RestDayRate => _defaultRates.RestDayRate;

        public decimal RestDayOTRate => _defaultRates.RestDayOTRate;

        public decimal RestDayNDRate => _defaultRates.RestDayNDRate;

        public decimal RestDayNDOTRate => _defaultRates.RestDayNDOTRate;

        public bool IsRegularDay => true;

        public bool IsRegularHoliday => false;

        public bool IsSpecialNonWorkingHoliday => false;

        public bool IsHoliday => false;

        public bool IsDoubleHoliday => false;
    }
}