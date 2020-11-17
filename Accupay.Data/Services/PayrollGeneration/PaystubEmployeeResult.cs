using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Interfaces;

namespace AccuPay.Data.Services
{
    public class PaystubEmployeeResult : IEmployeeResult
    {
        public int EmployeeId { get; set; }

        public int? PaystubId { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeFullName { get; set; }

        public ResultStatus Status { get; set; }

        public string Description { get; set; }

        public bool IsSuccess => Status == ResultStatus.Success;

        public bool IsError => Status == ResultStatus.Error;

        private PaystubEmployeeResult(int employeeId, int? paystubId, string employeeNo, string fullName, ResultStatus status, string description)
        {
            EmployeeId = employeeId;
            EmployeeNumber = employeeNo;
            EmployeeFullName = fullName;
            Status = status;
            Description = description;
            PaystubId = paystubId;
        }

        public static PaystubEmployeeResult Success(Employee employee, Paystub paystub)
        {
            var result = new PaystubEmployeeResult(
                employee.RowID.Value,
                paystub.RowID.Value,
                employee.EmployeeNo,
                employee.FullNameWithMiddleInitialLastNameFirst,
                ResultStatus.Success,
                "");

            return result;
        }

        public static PaystubEmployeeResult Error(Employee employee, string description)
        {
            var result = new PaystubEmployeeResult(
                employee.RowID.Value,
                paystubId: null,
                employee.EmployeeNo,
                employee.FullNameWithMiddleInitialLastNameFirst,
                ResultStatus.Error,
                description);

            return result;
        }
    }
}