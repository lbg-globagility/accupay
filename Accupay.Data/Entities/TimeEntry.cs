using AccuPay.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities

{
    [Table("employeetimeentry")]
    public class TimeEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        public int? EmployeeID { get; set; }

        public int? BranchID { get; set; }

        public int? EmployeeShiftID { get; set; }

        public int? EmployeeSalaryID { get; set; }

        public int? PayRateID { get; set; }

        [Column("Date")]
        public DateTime Date { get; set; }

        [Column("RegularHoursWorked")]
        public decimal RegularHours { get; set; }

        [Column("RegularHoursAmount")]
        public decimal RegularPay { get; set; }

        [Column("OvertimeHoursWorked")]
        public decimal OvertimeHours { get; set; }

        [Column("OvertimeHoursAmount")]
        public decimal OvertimePay { get; set; }

        [Column("NightDifferentialHours")]
        public decimal NightDiffHours { get; set; }

        [Column("NightDiffHoursAmount")]
        public decimal NightDiffPay { get; set; }

        [Column("NightDifferentialOTHours")]
        public decimal NightDiffOTHours { get; set; }

        [Column("NightDiffOTHoursAmount")]
        public decimal NightDiffOTPay { get; set; }

        public decimal RestDayHours { get; set; }

        [Column("RestDayAmount")]
        public decimal RestDayPay { get; set; }

        public decimal RestDayOTHours { get; set; }

        public decimal RestDayOTPay { get; set; }

        public decimal VacationLeaveHours { get; set; }

        public decimal SickLeaveHours { get; set; }

        public decimal MaternityLeaveHours { get; set; }

        public decimal OtherLeaveHours { get; set; }

        [Column("Leavepayment")]
        public decimal LeavePay { get; set; }

        public decimal SpecialHolidayHours { get; set; }

        public decimal SpecialHolidayPay { get; set; }

        public decimal SpecialHolidayOTHours { get; set; }

        public decimal SpecialHolidayOTPay { get; set; }

        public decimal RegularHolidayHours { get; set; }

        public decimal RegularHolidayPay { get; set; }

        [NotMapped]
        public decimal BasicRegularHolidayPay { get; set; }

        public decimal RegularHolidayOTHours { get; set; }

        public decimal RegularHolidayOTPay { get; set; }

        [Column("HolidayPayAmount")]
        public decimal HolidayPay { get; set; }

        [Column("HoursLate")]
        public decimal LateHours { get; set; }

        [Column("HoursLateAmount")]
        public decimal LateDeduction { get; set; }

        public decimal UndertimeHours { get; set; }

        [Column("UndertimeHoursAmount")]
        public decimal UndertimeDeduction { get; set; }

        public decimal AbsentHours { get; set; }

        [Column("Absent")]
        public decimal AbsentDeduction { get; set; }

        [NotMapped]
        public decimal BasicHours { get; set; }

        [Column("BasicDayPay")]
        public decimal BasicDayPay { get; set; }

        [Column("TotalHoursWorked")]
        public decimal TotalHours { get; set; }

        [Column("TotalDayPay")]
        public decimal TotalDayPay { get; set; }

        [NotMapped]
        public DateTime DutyStart { get; set; }

        [NotMapped]
        public DateTime DutyEnd { get; set; }

        public decimal WorkHours { get; set; }

        public decimal ShiftHours { get; set; }

        public bool IsRestDay { get; set; }

        public bool HasShift { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public TimeEntry()
        {
        }

        public void SetLeaveHours(string type, decimal leaveHours)
        {
            switch (type)
            {
                case ProductConstant.SICK_LEAVE:
                    SickLeaveHours = leaveHours;
                    break;

                case ProductConstant.VACATION_LEAVE:
                    VacationLeaveHours = leaveHours;
                    break;

                case ProductConstant.MATERNITY_LEAVE:
                    MaternityLeaveHours = leaveHours;
                    break;

                case ProductConstant.OTHERS_LEAVE:
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