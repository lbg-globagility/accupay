using AccuPay.Data.Services.Policies;
using AccuPay.Utilities;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PolicyHelper
    {
        public const string PayPeriodPolicyType = "Pay Period Policy";
        public const string DefaultFirstHalfDaysSpanPolicyLIC = "DefaultFirstHalfDaysSpan";
        public const string DefaultEndOfTheMonthDaysSpanPolicyLIC = "DefaultEndOfTheMonthDaysSpan";

        private readonly ListOfValueService _listOfValueService;
        private readonly string CurrentSystemOwner;
        private ActualTimeEntryPolicy _actualTimeEntryPolicy;
        private TimeEntryPolicy _timeEntryPolicy;
        private ListOfValueCollection _settings;

        public PolicyHelper(ListOfValueService listOfValueService, SystemOwnerService systemOwnerService)
        {
            _listOfValueService = listOfValueService;
            CurrentSystemOwner = systemOwnerService.GetCurrentSystemOwner();

            _settings = _listOfValueService.Create();
            SetPolicyGroups();
        }

        public async Task Refresh()
        {
            _settings = await _listOfValueService.CreateAsync();
            SetPolicyGroups();
        }

        private void SetPolicyGroups()
        {
            _actualTimeEntryPolicy = new ActualTimeEntryPolicy(_settings);
            _timeEntryPolicy = new TimeEntryPolicy(_settings);
        }

        public bool ShowActual => _settings.GetBoolean("Policy.ShowActual", true);

        public bool UseUserLevel => _settings.GetBoolean("User Policy.UseUserLevel", false);

        public bool UseEmailPayslip => _settings.GetBoolean("Payroll Policy.EmailPayslip", false);

        public decimal DefaultBPIInsurance => _settings.GetDecimal("Default.BPIInsurance");

        public bool UseBPIInsurance => _settings.GetBoolean("Employee Policy.UseBPIInsurance", false);

        public bool UseDefaultShiftAndTimeLogs => _settings.GetBoolean("Data Policy.UseDefaultShiftAndTimeLogs", false);

        public bool HasDifferentPayPeriodDates => _settings.GetBoolean("Payroll Policy.HasDifferentPayPeriodDates", false);

        public bool UseCostCenter => _settings.GetBoolean("Policy.UseCostCenter", false);

        public bool UseMassOvertime => _settings.GetBoolean("Policy.UseMassOvertime", false);

        public string HolidayCalculationType => _settings.GetStringOrDefault("Payroll Policy.HolidayPay", "Daily");

        public bool UseAgency => CurrentSystemOwner == SystemOwnerService.Hyundai || CurrentSystemOwner == SystemOwnerService.Goldwings;

        #region ActualTimeEntryPolicy

        public bool AllowanceForOvertime => _actualTimeEntryPolicy.AllowanceForOvertime;

        public bool AllowanceForNightDiff => _actualTimeEntryPolicy.AllowanceForNightDiff;

        public bool AllowanceForNightDiffOT => _actualTimeEntryPolicy.AllowanceForNightDiffOT;

        public bool AllowanceForRestDay => _actualTimeEntryPolicy.AllowanceForRestDay;

        public bool AllowanceForRestDayOT => _actualTimeEntryPolicy.AllowanceForRestDayOT;

        public bool AllowanceForHoliday => _actualTimeEntryPolicy.AllowanceForHoliday;

        #endregion ActualTimeEntryPolicy

        #region TimeEntryPolicy

        public bool AbsencesOnHoliday => _timeEntryPolicy.AbsencesOnHoliday;

        public bool ComputeBreakTimeLate => _timeEntryPolicy.ComputeBreakTimeLate;

        public bool HasNightBreaktime => _timeEntryPolicy.HasNightBreaktime;

        public bool IgnoreShiftOnRestDay => _timeEntryPolicy.IgnoreShiftOnRestDay;

        public bool LateHoursRoundingUp => _timeEntryPolicy.LateHoursRoundingUp;

        public bool PaidAsLongAsHasTimeLog => _timeEntryPolicy.PaidAsLongAsHasTimeLog;

        public bool PostLegalHolidayCheck => _timeEntryPolicy.PostLegalHolidayCheck;

        public bool RequiredToWorkTheDayBeforeHoliday => _timeEntryPolicy.RequiredToWorkTheDayBeforeHoliday;

        public bool RespectDefaultRestDay => _timeEntryPolicy.RespectDefaultRestDay;

        public bool RestDayInclusive => _timeEntryPolicy.RestDayInclusive;

        public bool ValidateLeaveBalance => _timeEntryPolicy.ValidateLeaveBalance;

        #endregion TimeEntryPolicy

        public ShiftBasedAutomaticOvertimePolicy ShiftBasedAutomaticOvertimePolicy => _timeEntryPolicy.ShiftBasedAutomaticOvertimePolicy;

        #region Pay Period Default Dates Policy ("16,31,false,true,false,false" means cutoff start day is "16", cutoff end day is "31", first day "is NOT last day of the month", second day "is last day of the month", first day "is not previous month", second day "is not previous month"

        public DayValueSpan DefaultFirstHalfDaysSpan(int? organizationId)
        {
            bool findByOrganization = DefaultPayPeriodFindByOrganization(organizationId);

            var value = _settings.GetString(
                name: $"{PayPeriodPolicyType}.{DefaultFirstHalfDaysSpanPolicyLIC}",
                findByOrganization: findByOrganization,
                organizationId: organizationId);

            return ParseDaysSpan(value, DayValueSpan.DefaultFirstHalf);
        }

        public DayValueSpan DefaultEndOfTheMonthDaysSpan(int? organizationId)
        {
            bool findByOrganization = DefaultPayPeriodFindByOrganization(organizationId);

            var value = _settings.GetString(
                name: $"{PayPeriodPolicyType}.{DefaultEndOfTheMonthDaysSpanPolicyLIC}",
                findByOrganization: findByOrganization,
                organizationId: organizationId);

            return ParseDaysSpan(value, DayValueSpan.DefaultEndOfTheMonth);
        }

        private bool DefaultPayPeriodFindByOrganization(int? organizationId) =>
            HasDifferentPayPeriodDates && organizationId.HasValue;

        private DayValueSpan ParseDaysSpan(string policyValue, DayValueSpan defaultValue)
        {
            if (string.IsNullOrWhiteSpace(policyValue))
            {
                return defaultValue;
            }

            var values = policyValue.Split(',');
            if (values.Length < 2 || values.Length > 6)
            {
                return defaultValue;
            }

            int? startDay = ObjectUtils.ToNullableInteger(values[0]);
            int? endDay = ObjectUtils.ToNullableInteger(values[1]);

            if (startDay == null && endDay == null)
            {
                return defaultValue;
            }

            bool startDayIsLastDay = ObjectUtils.ToNullableBoolean(values[2]) ?? false;
            bool endDayIsLastDay = ObjectUtils.ToNullableBoolean(values[3]) ?? false;

            bool startDayIsLastMonth = ObjectUtils.ToNullableBoolean(values[4]) ?? false;
            bool endDayIsLastMonth = ObjectUtils.ToNullableBoolean(values[5]) ?? false;

            var startDayValue = DayValue.Create(
                startDay.Value,
                startDayIsLastDay,
                startDayIsLastMonth);

            var endDayValue = DayValue.Create(
                endDay.Value,
                endDayIsLastDay,
                endDayIsLastMonth);

            return DayValueSpan.Create(startDayValue, endDayValue);
        }

        #endregion Pay Period Default Dates Policy ("16,31,false,true,false,false" means cutoff start day is "16", cutoff end day is "31", first day "is NOT last day of the month", second day "is last day of the month", first day "is not previous month", second day "is not previous month"
    }
}
