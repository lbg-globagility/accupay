using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Web.Shifts.Models
{
    public interface ICrudShiftDto
    {
        DateTime DateSched { get; set; }

        TimeSpan? StartTime { get; set; }

        TimeSpan? EndTime { get; set; }

        TimeSpan? BreakStartTime { get; set; }

        decimal BreakLength { get; set; }

        bool IsRestDay { get; set; }

        decimal ShiftHours { get; set; }

        decimal WorkHours { get; set; }
    }
}
