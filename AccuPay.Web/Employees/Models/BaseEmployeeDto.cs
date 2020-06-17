using AccuPay.Data.Entities;

namespace AccuPay.Web.Employees.Models
{
    public class BaseEmployeeDto
    {
        public string EmployeeNo { get; set; }
        public string FullName { get; set; }

        protected virtual void ApplyData(Employee employee)
        {
            if (employee == null) return;

            EmployeeNo = employee.EmployeeNo;
            FullName = employee.FullName;
        }
    }
}
