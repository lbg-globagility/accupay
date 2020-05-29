using AccuPay.Data.Entities;
using AccuPay.Utilities;
using System;

namespace AccuPay.Data.Services.Imports
{
    public class ShiftImportModel : ShiftModel
    {
        public string EmployeeNo { get; set; }
        public string FullName { get; set; }
        public string Remarks { get; set; }

        public ShiftImportModel(Employee employee)
        {
            AssignEmployee(employee);
        }

        private void AssignEmployee(Employee employee)
        {
            EmployeeId = employee?.RowID;
            EmployeeNo = employee?.EmployeeNo;
            FullName = employee?.FullNameLastNameFirst;
        }

        // used in DataGridView as well as TimeToDisplay and BreakFromDisplay
        public DateTime? TimeFromDisplay => TimeUtility.ToDateTime(StartTime);

        public DateTime? TimeToDisplay => TimeUtility.ToDateTime(EndTime);

        public DateTime? BreakFromDisplay => TimeUtility.ToDateTime(BreakTime);

        public bool IsValidToSave
        {
            get
            {
                var hasShiftTime = StartTime.HasValue && EndTime.HasValue;

                return hasShiftTime || IsRestDay;
            }
        }

        public bool IsExistingEmployee => EmployeeId.HasValue;
    }
}