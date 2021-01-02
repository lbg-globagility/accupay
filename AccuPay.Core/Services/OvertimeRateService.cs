using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using AccuPay.Utilities.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class OvertimeRateService : IOvertimeRateService
    {
        private readonly IListOfValueRepository _listOfValueRepository;

        public OvertimeRateService(IListOfValueRepository listOfValueRepository)
        {
            _listOfValueRepository = listOfValueRepository;
        }

        public async Task<OvertimeRate> GetOvertimeRates()
        {
            var listOfValuesType = "Pay rate";
            var regularDayLIC = CalendarConstant.RegularDay;
            var specialHolidayLIC = CalendarConstant.SpecialNonWorkingHoliday;
            var regularHolidayLIC = CalendarConstant.RegularHoliday;
            var doubleHolidayLIC = CalendarConstant.DoubleHoliday;

            var payRates = await _listOfValueRepository.GetListOfValuesAsync(listOfValuesType);

            decimal basePay = 0;
            decimal overtime = 0;
            decimal nightDifferential = 0;
            decimal nightDifferentialOvertime = 0;
            decimal restDay = 0;
            decimal restDayOvertime = 0;
            decimal restDayNightDifferential = 0;
            decimal restDayNightDifferentialOvertime = 0;
            decimal specialHoliday = 0;
            decimal specialHolidayOvertime = 0;
            decimal specialHolidayNightDifferential = 0;
            decimal specialHolidayNightDifferentialOvertime = 0;
            decimal specialHolidayRestDay = 0;
            decimal specialHolidayRestDayOvertime = 0;
            decimal specialHolidayRestDayNightDifferential = 0;
            decimal specialHolidayRestDayNightDifferentialOvertime = 0;
            decimal regularHoliday = 0;
            decimal regularHolidayOvertime = 0;
            decimal regularHolidayNightDifferential = 0;
            decimal regularHolidayNightDifferentialOvertime = 0;
            decimal regularHolidayRestDay = 0;
            decimal regularHolidayRestDayOvertime = 0;
            decimal regularHolidayRestDayNightDifferential = 0;
            decimal regularHolidayRestDayNightDifferentialOvertime = 0;
            decimal doubleHoliday = 0;
            decimal doubleHolidayOvertime = 0;
            decimal doubleHolidayNightDifferential = 0;
            decimal doubleHolidayNightDifferentialOvertime = 0;
            decimal doubleHolidayRestDay = 0;
            decimal doubleHolidayRestDayOvertime = 0;
            decimal doubleHolidayRestDayNightDifferential = 0;
            decimal doubleHolidayRestDayNightDifferentialOvertime = 0;

            var regularDayRate = payRates
                .Where(l => l.LIC == regularDayLIC)
                .FirstOrDefault()?
                .DisplayValue;

            if (string.IsNullOrWhiteSpace(regularDayRate) == false)
            {
                var regularDayRates = regularDayRate.Split(',');

                basePay = ConvertToRate(regularDayRates[0]);
                overtime = ConvertToRate(regularDayRates[1]);
                nightDifferential = ConvertToRate(regularDayRates[2]);
                nightDifferentialOvertime = ConvertToRate(regularDayRates[3]);
                restDay = ConvertToRate(regularDayRates[4]);
                restDayOvertime = ConvertToRate(regularDayRates[5]);
                restDayNightDifferential = ConvertToRate(regularDayRates[6]);
                restDayNightDifferentialOvertime = ConvertToRate(regularDayRates[7]);
            }

            var specialHolidayRate = payRates
                .Where(l => l.LIC == specialHolidayLIC)
                .FirstOrDefault()?
                .DisplayValue;

            if (string.IsNullOrWhiteSpace(specialHolidayRate) == false)
            {
                var specialHolidayRates = specialHolidayRate.Split(',');

                specialHoliday = ConvertToRate(specialHolidayRates[0]);
                specialHolidayOvertime = ConvertToRate(specialHolidayRates[1]);
                specialHolidayNightDifferential = ConvertToRate(specialHolidayRates[2]);
                specialHolidayNightDifferentialOvertime = ConvertToRate(specialHolidayRates[3]);
                specialHolidayRestDay = ConvertToRate(specialHolidayRates[4]);
                specialHolidayRestDayOvertime = ConvertToRate(specialHolidayRates[5]);
                specialHolidayRestDayNightDifferential = ConvertToRate(specialHolidayRates[6]);
                specialHolidayRestDayNightDifferentialOvertime = ConvertToRate(specialHolidayRates[7]);
            }

            var regularHolidayRate = payRates
                .Where(l => l.LIC == regularHolidayLIC)
                .FirstOrDefault()?
                .DisplayValue;

            if (string.IsNullOrWhiteSpace(regularHolidayRate) == false)
            {
                var regularHolidayRates = regularHolidayRate.Split(',');

                regularHoliday = ConvertToRate(regularHolidayRates[0]);
                regularHolidayOvertime = ConvertToRate(regularHolidayRates[1]);
                regularHolidayNightDifferential = ConvertToRate(regularHolidayRates[2]);
                regularHolidayNightDifferentialOvertime = ConvertToRate(regularHolidayRates[3]);
                regularHolidayRestDay = ConvertToRate(regularHolidayRates[4]);
                regularHolidayRestDayOvertime = ConvertToRate(regularHolidayRates[5]);
                regularHolidayRestDayNightDifferential = ConvertToRate(regularHolidayRates[6]);
                regularHolidayRestDayNightDifferentialOvertime = ConvertToRate(regularHolidayRates[7]);
            }

            var doubleHolidayRate = payRates
                .Where(l => l.LIC == doubleHolidayLIC)
                .FirstOrDefault()?
                .DisplayValue;

            if (string.IsNullOrWhiteSpace(doubleHolidayRate) == false)
            {
                var doubleHolidayRates = doubleHolidayRate.Split(',');

                doubleHoliday = ConvertToRate(doubleHolidayRates[0]);
                doubleHolidayOvertime = ConvertToRate(doubleHolidayRates[1]);
                doubleHolidayNightDifferential = ConvertToRate(doubleHolidayRates[2]);
                doubleHolidayNightDifferentialOvertime = ConvertToRate(doubleHolidayRates[3]);
                doubleHolidayRestDay = ConvertToRate(doubleHolidayRates[4]);
                doubleHolidayRestDayOvertime = ConvertToRate(doubleHolidayRates[5]);
                doubleHolidayRestDayNightDifferential = ConvertToRate(doubleHolidayRates[6]);
                doubleHolidayRestDayNightDifferentialOvertime = ConvertToRate(doubleHolidayRates[7]);
            }

            return new OvertimeRate(
                basePay: basePay,
                overtime: overtime,
                nightDifferential: nightDifferential,
                nightDifferentialOvertime: nightDifferentialOvertime,
                restDay: restDay,
                restDayOvertime: restDayOvertime,
                restDayNightDifferential: restDayNightDifferential,
                restDayNightDifferentialOvertime: restDayNightDifferentialOvertime,
                specialHoliday: specialHoliday,
                specialHolidayOvertime: specialHolidayOvertime,
                specialHolidayNightDifferential: specialHolidayNightDifferential,
                specialHolidayNightDifferentialOvertime: specialHolidayNightDifferentialOvertime,
                specialHolidayRestDay: specialHolidayRestDay,
                specialHolidayRestDayOvertime: specialHolidayRestDayOvertime,
                specialHolidayRestDayNightDifferential: specialHolidayRestDayNightDifferential,
                specialHolidayRestDayNightDifferentialOvertime: specialHolidayRestDayNightDifferentialOvertime,
                regularHoliday: regularHoliday,
                regularHolidayOvertime: regularHolidayOvertime,
                regularHolidayNightDifferential: regularHolidayNightDifferential,
                regularHolidayNightDifferentialOvertime: regularHolidayNightDifferentialOvertime,
                regularHolidayRestDay: regularHolidayRestDay,
                regularHolidayRestDayOvertime: regularHolidayRestDayOvertime,
                regularHolidayRestDayNightDifferential: regularHolidayRestDayNightDifferential,
                regularHolidayRestDayNightDifferentialOvertime: regularHolidayRestDayNightDifferentialOvertime,
                doubleHoliday: doubleHoliday,
                doubleHolidayOvertime: doubleHolidayOvertime,
                doubleHolidayNightDifferential: doubleHolidayNightDifferential,
                doubleHolidayNightDifferentialOvertime: doubleHolidayNightDifferentialOvertime,
                doubleHolidayRestDay: doubleHolidayRestDay,
                doubleHolidayRestDayOvertime: doubleHolidayRestDayOvertime,
                doubleHolidayRestDayNightDifferential: doubleHolidayRestDayNightDifferential,
                doubleHolidayRestDayNightDifferentialOvertime: doubleHolidayRestDayNightDifferentialOvertime);
        }

        private decimal ConvertToRate(string input)
        {
            if (input.Length > 2)

                // add a decimal to convert it properly
                // since the inputs are either
                // 286 which is equals 2.86 or
                // 1375 which is equals 1.375
                input = input.Insert(1, ".");

            return input.ToDecimal();
        }
    }
}
