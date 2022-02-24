using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Interfaces;

namespace AccuPay.Core.Services.LeaveBalanceReset
{
    public class LeaveResetResult : IEmployeeResult
    {
        public LeaveResetResult(int employeeId,
            string employeeNo,
            string fullName,
            ResultStatus status,
            string description)
        {
            EmployeeId = employeeId;
            EmployeeNumber = employeeNo;
            EmployeeFullName = fullName;
            Status = status;
            Description = description;
        }

        public string EmployeeNumber { get; set; }

        public string EmployeeFullName { get; set; }

        public int EmployeeId { get; set; }

        public ResultStatus Status { get; set; }

        public string Description { get; set; }

        public bool IsSuccess => Status == ResultStatus.Success;

        public bool IsError => Status == ResultStatus.Error;

        public static LeaveResetResult Success(Employee employee, string successMessage)
        {
            return new LeaveResetResult(employeeId: employee.RowID.Value,
                employeeNo: employee.EmployeeNo,
                fullName: employee.FullNameLastNameFirst,
                status: ResultStatus.Success,
                description: successMessage);
        }

        public static LeaveResetResult Error(Employee employee, string errorMessage)
        {
            return new LeaveResetResult(employeeId: employee.RowID.Value,
                employeeNo: employee.EmployeeNo,
                fullName: employee.FullNameLastNameFirst,
                status: ResultStatus.Success,
                description: errorMessage);
        }
    }
}
