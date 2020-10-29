using System;

namespace AccuPay.Data.Interfaces
{
    public interface IShift
    {
        int? EmployeeId { get; set; }
        DateTime Date { get; set; }
        DateTime? StartTime { get; }
        DateTime? EndTime { get; }
        DateTime? BreakTime { get; }
        decimal BreakLength { get; set; }
        bool IsRestDay { get; }
    }
}