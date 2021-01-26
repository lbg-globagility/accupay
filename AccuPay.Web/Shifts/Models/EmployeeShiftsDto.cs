using System.Collections.Generic;

namespace AccuPay.Web.Shifts.Models
{
    public class EmployeeShiftsDto
    {
        public int EmployeeId { get; set; }

        public string EmployeeNo { get; set; }

        public string FullName { get; set; }

        public ICollection<EmployeeDutyScheduleDto> Shifts { get; set; }
    }
}
