using System;
using System.Data;

namespace AccuPay.Core.ReportModels
{
    public class SalaryModel
    {
        public SalaryModel(DataRow dataRow)
        {
            RowID = (int)dataRow["RowID"];
            PositionID = (int?)dataRow["EmployeeID"];
            //PositionID = (int?)dataRow["PositionID"];
            PhilHealthDeduction = (decimal)dataRow["PhilHealthDeduction"];
            HDMFAmount = (decimal)dataRow["HDMFAmount"];
            BasicSalary = (decimal)dataRow["BasicPay"];
            AllowanceSalary = (decimal)dataRow["TrueSalary"];
            //MaritalStatus = dataRow["MaritalStatus"];
            //EffectiveFrom = (DateTime)dataRow["EffectiveDateFrom"];
            //DoPaySSSContribution = (bool)dataRow["DoPaySSSContribution"];
            //AutoComputePhilHealthContribution = (bool)dataRow["AutoComputePhilHealthContribution"];
            //AutoComputeHDMFContribution = (bool)dataRow["AutoComputeHDMFContribution"];
            //IsMinimumWage = (bool)dataRow["IsMinimumWage"];
        }

        public int RowID { get; }
        public int? EmployeeID { get; internal set; }
        public int? PositionID { get; internal set; }
        public decimal PhilHealthDeduction { get; internal set; }
        public decimal HDMFAmount { get; internal set; }
        public decimal BasicSalary { get; internal set; }
        public decimal AllowanceSalary { get; internal set; }
        public string MaritalStatus { get; internal set; }
        public DateTime EffectiveFrom { get; internal set; }
        public bool DoPaySSSContribution { get; internal set; }
        public bool AutoComputePhilHealthContribution { get; internal set; }
        public bool AutoComputeHDMFContribution { get; internal set; }
        public bool IsMinimumWage { get; internal set; }
    }
}
