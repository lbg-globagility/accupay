using AccuPay.Data.Enums;
using AccuPay.Data.Interfaces;

namespace AccuPay.Data.Helpers
{
    public class EmployeeResult : IEmployeeResult
    {
        public string EmployeeNumber { get; }
        public string EmployeeFullName { get; }
        public int EmployeeId { get; }
        public string Description { get; }
        public ResultStatus Status { get; }
        public bool IsSuccess => Status == ResultStatus.Success;
        public bool IsError => Status == ResultStatus.Error;

        public EmployeeResult(string employeeNumber, string employeeFullName, int employeeId, string description, ResultStatus status)
        {
            EmployeeNumber = employeeNumber;
            EmployeeFullName = employeeFullName;
            EmployeeId = employeeId;
            Description = description;
            Status = status;
        }

        public static EmployeeResult Success(string employeeNumber, string employeeFullName, int employeeId)
        {
            return new EmployeeResult(
                employeeNumber: employeeNumber,
                employeeFullName: employeeFullName,
                employeeId: employeeId,
                description: string.Empty,
                status: ResultStatus.Success);
        }

        public static EmployeeResult Error(string employeeNumber, string employeeFullName, int employeeId, string description)
        {
            return new EmployeeResult(
                employeeNumber: employeeNumber,
                employeeFullName: employeeFullName,
                employeeId: employeeId,
                description: description,
                status: ResultStatus.Error);
        }
    }
}