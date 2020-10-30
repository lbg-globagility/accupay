using System;

namespace AccuPay.Data.Interfaces
{
    public interface IShift
    {
        int? EmployeeId { get; set; }
        DateTime Date { get; set; }
        TimeSpan? StartTime { get; }
        TimeSpan? EndTime { get; }
        TimeSpan? BreakTime { get; }
        decimal BreakLength { get; set; }
        bool IsRestDay { get; }
    }
}