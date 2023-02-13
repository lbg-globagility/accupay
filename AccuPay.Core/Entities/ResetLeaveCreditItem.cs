using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("resetleavecredititem")]
    public class ResetLeaveCreditItem : EmployeeDataEntity
    {
        public int? ResetLeaveCreditId { get; set; }
        public virtual ResetLeaveCredit ResetLeaveCredit { get; set; }
        //public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        public decimal VacationLeaveCredit { get; set; }
        public decimal SickLeaveCredit { get; set; }
        public bool IsSelected { get; set; }
        public bool IsApplied { get; set; }

        public static ResetLeaveCreditItem NewResetLeaveCreditItem(
            int organizationId,
            int userId,
            int employeeId,
            decimal vacationCredit = 0,
            decimal sickCredit = 0,
            bool isSelected = false,
            bool isApplied = false,
            Employee employee = null)
        {
            return new ResetLeaveCreditItem() { OrganizationID = organizationId, CreatedBy = userId, EmployeeID = employeeId, VacationLeaveCredit = vacationCredit, SickLeaveCredit = sickCredit, IsSelected = isSelected, IsApplied = isApplied, Employee = employee };
        }

        public void Update(int userId,
            decimal vacationCredit,
            decimal sickCredit,
            bool isSelected,
            bool isApplied)
        {
            LastUpdBy = userId;
            VacationLeaveCredit = vacationCredit;
            SickLeaveCredit = sickCredit;
            IsSelected = isSelected;
            IsApplied = isApplied;
        }
    }
}
