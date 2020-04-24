using System;

namespace AccuPay.Data
{
    public interface IPayrate
    {
        DateTime Date { get; }

        decimal RegularRate { get; }

        decimal OvertimeRate { get; }

        decimal NightDiffRate { get; }

        decimal NightDiffOTRate { get; }

        decimal RestDayRate { get; }

        decimal RestDayOTRate { get; }

        decimal RestDayNDRate { get; }

        decimal RestDayNDOTRate { get; }

        bool IsRegularDay { get; }

        bool IsRegularHoliday { get; }

        bool IsSpecialNonWorkingHoliday { get; }

        bool IsHoliday { get; }

        bool IsDoubleHoliday { get; }
    }
}