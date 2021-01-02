namespace AccuPay.Core.Interfaces
{
    public interface IEmploymentPolicy
    {
        decimal WorkDaysPerYear { get; }

        int GracePeriod { get; }

        bool ComputeNightDiff { get; }

        bool ComputeNightDiffOT { get; }

        bool ComputeRestDay { get; }

        bool ComputeRestDayOT { get; }

        bool ComputeSpecialHoliday { get; }

        bool ComputeRegularHoliday { get; }
    }
}
