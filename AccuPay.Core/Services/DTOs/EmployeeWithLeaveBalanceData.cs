using AccuPay.Core.Entities;
using System;

namespace AccuPay.Core.Services
{
    public class EmployeeWithLeaveBalanceData
    {
        public Employee Employee { get; set; }

        public decimal VacationLeaveBalance { get; set; }

        public decimal SickLeaveBalance { get; set; }

        public EmployeeWithLeaveBalanceData(Employee employee, decimal vacationLeaveBalance, decimal sickLeaveBalance)
        {
            if (employee == null)
                throw new ArgumentNullException();

            Employee = employee;
            VacationLeaveBalance = vacationLeaveBalance;
            SickLeaveBalance = sickLeaveBalance;
        }
    }
}