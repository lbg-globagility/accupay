using AccuPay.Core.Entities;

namespace AccuPay.Core.ValueObjects
{
    public class SubstituteEmploymentPolicy : IEmploymentPolicy
    {
        public decimal WorkDaysPerYear { get; private set; }

        public int GracePeriod { get; private set; }

        public bool ComputeNightDiff { get; private set; }

        public bool ComputeNightDiffOT { get; private set; }

        public bool ComputeRestDay { get; private set; }

        public bool ComputeRestDayOT { get; private set; }

        public bool ComputeSpecialHoliday { get; private set; }

        public bool ComputeRegularHoliday { get; private set; }

        public SubstituteEmploymentPolicy(Employee employee)
        {
            WorkDaysPerYear = employee.WorkDaysPerYear;
            GracePeriod = (int)employee.LateGracePeriod;
            ComputeNightDiff = employee.CalcNightDiff;
            ComputeNightDiffOT = true;
            ComputeRestDay = employee.CalcRestDay;
            ComputeRegularHoliday = employee.CalcHoliday;
            ComputeSpecialHoliday = employee.CalcSpecialHoliday;
        }
    }
}
