using System;

namespace AccuPay.Data
{
    public interface ISalary
    {
        decimal AllowanceSalary { get; set; }
        bool AutoComputeHDMFContribution { get; set; }
        bool AutoComputePhilHealthContribution { get; set; }
        decimal BasicSalary { get; set; }
        DateTime Created { get; set; }
        int? CreatedBy { get; set; }
        decimal DailyRate { get; set; }
        bool DoPaySSSContribution { get; set; }
        DateTime EffectiveFrom { get; set; }
        DateTime? EffectiveTo { get; set; }
        int? EmployeeID { get; set; }
        int? FilingStatusID { get; set; }
        decimal HDMFAmount { get; set; }
        decimal HourlyRate { get; set; }
        bool IsIndefinite { get; }
        DateTime? LastUpd { get; set; }
        int? LastUpdBy { get; set; }
        string MaritalStatus { get; set; }
        int NoOfDependents { get; set; }
        int? OrganizationID { get; set; }
        int? PayPhilHealthID { get; set; }
        decimal PhilHealthDeduction { get; set; }
        int? PositionID { get; set; }
        int? RowID { get; set; }
        decimal TotalSalary { get; set; }
    }
}