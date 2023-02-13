using AccuPay.Core.Entities;
using System;

namespace AccuPay.Core.Services.Imports.ResetLeaveCredits
{
    public class ResetLeaveCreditItemModel
    {
        public ResetLeaveCreditItemModel(ResetLeaveCreditItem item)
        {
            OriginalResetLeaveCreditItem = item;
            Id = item.RowID;
            EmployeeRowID = item.EmployeeID;
            VacationLeaveCredit = item.VacationLeaveCredit;
            SickLeaveCredit = item.SickLeaveCredit;
            IsSelected = item.IsSelected;
            IsApplied = item.IsApplied;
            if (item.Employee != null)
            {
                var employee = item.Employee;
                EmployeeNo = employee.EmployeeNo;
                LastName = employee.LastName;
                FirstName = employee.FirstName;
                StartDate = $"{employee.StartDate:d}";
                if (DateRegularized != null)
                    DateRegularized = $"{employee.DateRegularized:d}";
            }
        }

        public ResetLeaveCreditItem OriginalResetLeaveCreditItem { get; }
        public int? Id { get; }
        public int? EmployeeRowID { get; }
        public string EmployeeNo { get; }
        public string LastName { get; }
        public string FirstName { get; }
        public string StartDate { get; }
        public string DateRegularized { get; }
        public decimal VacationLeaveCredit { get; set; }
        public decimal SickLeaveCredit { get; set; }
        public bool IsSelected { get; set; }
        public bool IsApplied { get; }
        public string IsAppliedText => IsApplied ? "ü" : string.Empty;
        public bool IsNew => Id == null;
        public bool HasChanged => OriginalResetLeaveCreditItem.VacationLeaveCredit != VacationLeaveCredit ||
            OriginalResetLeaveCreditItem.SickLeaveCredit != SickLeaveCredit ||
            OriginalResetLeaveCreditItem.IsSelected != IsSelected ||
            OriginalResetLeaveCreditItem.IsApplied != IsApplied;

        public void ChangeLastUpdBy(int z_User)
        {
            OriginalResetLeaveCreditItem.LastUpdBy = z_User;
        }
    }
}
