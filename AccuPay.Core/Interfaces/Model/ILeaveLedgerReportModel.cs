﻿using AccuPay.Core.Enums;

namespace AccuPay.Core
{
    public interface ILeaveLedgerReportModel
    {
        decimal AvailedLeave { get; }
        decimal AvailedLeaveInDays { get; }
        decimal BeginningBalance { get; }
        decimal BeginningBalanceInDays { get; }
        string EmployeeNumber { get; }
        decimal EndingBalance { get; }
        decimal EndingBalanceInDays { get; }
        string FullName { get; }
        LeaveType LeaveType { get; }
        string LeaveTypeDescription { get; }
    }
}