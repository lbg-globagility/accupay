using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Web.Shifts.Models
{
    public interface ICrudShiftDto
    {
        DateTime DateSched { get; set; }

        DateTime? StartTime { get; set; }

        DateTime? EndTime { get; set; }

        DateTime? BreakStartTime { get; set; }

        decimal BreakLength { get; set; }

        bool IsRestDay { get; set; }
    }
}
