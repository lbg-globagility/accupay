using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities

{
    [Table("employeetimeentry")]
    public class TimeEntry : BaseTimeEntry
    {
        [Column("RegularHoursWorked")]
        public decimal RegularHours { get; set; }

        [Column("OvertimeHoursWorked")]
        public decimal OvertimeHours { get; set; }

        [Column("NightDifferentialHours")]
        public decimal NightDiffHours { get; set; }

        [Column("NightDifferentialOTHours")]
        public decimal NightDiffOTHours { get; set; }

        public decimal RestDayHours { get; set; }

        public decimal RestDayOTHours { get; set; }

        public decimal VacationLeaveHours { get; set; }

        public decimal SickLeaveHours { get; set; }

        public decimal MaternityLeaveHours { get; set; }

        public decimal OtherLeaveHours { get; set; }

        public decimal SpecialHolidayHours { get; set; }

        public decimal SpecialHolidayOTHours { get; set; }

        public decimal RegularHolidayHours { get; set; }

        [NotMapped]
        public decimal BasicRegularHolidayPay { get; set; }

        public decimal RegularHolidayOTHours { get; set; }

        [Column("HoursLate")]
        public decimal LateHours { get; set; }

        public decimal UndertimeHours { get; set; }

        public decimal AbsentHours { get; set; }

        [NotMapped]
        public decimal BasicHours { get; set; }

        [Column("TotalHoursWorked")]
        public decimal TotalHours { get; set; }

        public decimal WorkHours { get; set; }

        public decimal ShiftHours { get; set; }

        public bool IsRestDay { get; set; }

        public bool HasShift { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public int? BranchID { get; set; }

        public int CreatedBy { get; set; }

        public int? LastUpdBy { get; set; }

        public void SetLeaveHours(string leaveType, decimal leaveHours)
        {
            switch (leaveType.ToTrimmedLowerCase())
            {
                case var type when string.Equals(type, ProductConstant.SICK_LEAVE, System.StringComparison.InvariantCultureIgnoreCase):
                    SickLeaveHours = leaveHours;
                    break;

                case var type when string.Equals(type, ProductConstant.VACATION_LEAVE, System.StringComparison.InvariantCultureIgnoreCase):
                    VacationLeaveHours = leaveHours;
                    break;

                case var type when string.Equals(type, ProductConstant.MATERNITY_LEAVE, System.StringComparison.InvariantCultureIgnoreCase):
                    MaternityLeaveHours = leaveHours;
                    break;

                case var type when string.Equals(type, ProductConstant.OTHERS_LEAVE, System.StringComparison.InvariantCultureIgnoreCase):
                    OtherLeaveHours = leaveHours;
                    break;
            }
        }

        public decimal TotalLeaveHours => VacationLeaveHours +
                                            SickLeaveHours +
                                            MaternityLeaveHours +
                                            OtherLeaveHours;

        public void ComputeTotalHours()
        {
            TotalHours = RegularHours +
                            OvertimeHours +
                            RestDayHours +
                            RestDayOTHours +
                            SpecialHolidayHours +
                            SpecialHolidayOTHours +
                            RegularHolidayHours +
                            RegularHolidayOTHours;
        }

        public void ComputeTotalPay()
        {
            TotalDayPay = GetTotalDayPay();
        }

        public decimal GetTotalDayPay() => RegularPay +
                                            OvertimePay +
                                            NightDiffPay +
                                            NightDiffOTPay +
                                            RestDayPay +
                                            RestDayOTPay +
                                            SpecialHolidayPay +
                                            SpecialHolidayOTPay +
                                            RegularHolidayPay +
                                            RegularHolidayOTPay +
                                            LeavePay;

        public void Reset()
        {
            IsRestDay = false;
            HasShift = false;
            ResetHours();
            ResetPay();
        }

        private void ResetHours()
        {
            BasicHours = 0;
            RegularHours = 0;
            OvertimeHours = 0;
            NightDiffHours = 0;
            NightDiffOTHours = 0;
            RestDayHours = 0;
            RestDayOTHours = 0;
            VacationLeaveHours = 0;
            SickLeaveHours = 0;
            MaternityLeaveHours = 0;
            OtherLeaveHours = 0;
            SpecialHolidayHours = 0;
            SpecialHolidayOTHours = 0;
            RegularHolidayHours = 0;
            RegularHolidayOTHours = 0;
            LateHours = 0;
            UndertimeHours = 0;
            AbsentHours = 0;
            TotalHours = 0;

            ShiftHours = 0;
            WorkHours = 0;
        }

        private void ResetPay()
        {
            BasicDayPay = 0;
            RegularPay = 0;
            OvertimePay = 0;
            NightDiffPay = 0;
            NightDiffOTPay = 0;
            RestDayPay = 0;
            RestDayOTPay = 0;
            LeavePay = 0;
            SpecialHolidayPay = 0;
            SpecialHolidayOTPay = 0;
            RegularHolidayPay = 0;
            RegularHolidayOTPay = 0;
            LateDeduction = 0;
            UndertimeDeduction = 0;
            AbsentDeduction = 0;
            TotalDayPay = 0;
        }
    }
}