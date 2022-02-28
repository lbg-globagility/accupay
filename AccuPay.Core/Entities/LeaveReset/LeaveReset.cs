using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Core.Entities.LeaveReset
{
    [Table("leavereset")]
    public class LeaveReset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; internal set; }

        public int OrganizationId { get; internal set; }
        public string Name { get; internal set; }
        public DateTime EffectiveDate { get; internal set; }
        public DateTime StartPeriodDate { get; internal set; }
        public DateTime EndPeriodDate { get; internal set; }
        public bool IsUnusedLeaveCashable { get; internal set; }

        public virtual ICollection<LeaveTenure> LeaveTenures { get; set; }

        public virtual ICollection<LeaveTypeRenewable> LeaveTypeRenewables { get; set; }

        public void ChangeDateAccordingToYear(int? year = null)
        {
            if (year == null) year = DateTime.Now.Year;

            if (year.Value == 0) year = DateTime.Now.Year;

            StartPeriodDate = new DateTime(year.Value, StartPeriodDate.Month, StartPeriodDate.Day);
            EndPeriodDate = StartPeriodDate.AddYears(1).AddSeconds(-1);
        }

        public LeaveTenure GetLeaveTenureTier(Employee employee)
        {
            // TODO: check if based on HIRE DATE or REGULARIZATION DATE
            // by default based on HIRE DATE

            if (LeaveTenures == null) return null;

            var timePeriod = new TimePeriod(StartPeriodDate, EndPeriodDate);
            return LeaveTenures.
                FirstOrDefault(l => l.IsMatch(hireDate: employee.StartDate, yearRangePeriod: timePeriod));
        }

        private LeaveTypeRenewable GetLeaveType(LeaveTypeEnum leaveTypeName)
        {
            return LeaveTypeRenewables.
                FirstOrDefault(l => l.LeaveName == leaveTypeName.Type);
        }

        public bool IsVacationLeaveSupported => IsLeaveTypeSupprted(LeaveTypeEnum.Vacation);

        public bool IsSickLeaveSupported => IsLeaveTypeSupprted(LeaveTypeEnum.Sick);

        public bool IsOthersLeaveSupported => IsLeaveTypeSupprted(LeaveTypeEnum.Others);

        public bool IsParentalLeaveSupported => IsLeaveTypeSupprted(LeaveTypeEnum.Parental);

        public TimePeriod YearRangePeriod => new TimePeriod(StartPeriodDate, EndPeriodDate);

        public bool IsLeaveTypeSupprted(LeaveTypeEnum leaveTypeName)
        {
            var leaveType = GetLeaveType(leaveTypeName);
            return leaveType?.IsSupported ?? false;
        }

        public DateTime GetBasisDate(Employee employee, LeaveTypeEnum leaveTypeEnum)
        {
            var leaveType = GetLeaveType(leaveTypeEnum);
            if (leaveType == null) return employee.StartDate;

            switch (leaveType.BasisStartDate)
            {
                case BasisStartDateEnum.DateRegularized:
                    return employee.DateRegularized ?? employee.StartDate;
                case BasisStartDateEnum.StartDate:
                    return employee.StartDate;
                default:
                    return employee.StartDate;
            }
        }

        public BasisStartDateEnum VacationLeaveBasisStartDate() => GetLeaveType(LeaveTypeEnum.Vacation)?.BasisStartDate ?? BasisStartDateEnum.StartDate;
        public BasisStartDateEnum SickLeaveBasisStartDate() => GetLeaveType(LeaveTypeEnum.Sick)?.BasisStartDate ?? BasisStartDateEnum.StartDate;
        public BasisStartDateEnum OthersLeaveBasisStartDate() => GetLeaveType(LeaveTypeEnum.Others)?.BasisStartDate ?? BasisStartDateEnum.StartDate;
        public BasisStartDateEnum ParentalLeaveBasisStartDate() => GetLeaveType(LeaveTypeEnum.Parental)?.BasisStartDate ?? BasisStartDateEnum.StartDate;

        public TimePeriod GetTimePeriod() => new TimePeriod(StartPeriodDate, EndPeriodDate);
        //internal static LeaveReset NewLeaveReset(int organizationId,
        //    string name,
        //    DateTime effectiveDate,
        //    DateTime startPeriodDate,
        //    DateTime endPeriodDate,
        //    bool isUnusedLeaveCashable)
        //{
        //    var leaveTenures = new List<LeaveTenure>();
        //    var leaveTypeRenewables = new List<LeaveTypeRenewable>();

        //    return new LeaveReset()
        //    {
        //        OrganizationId = organizationId,
        //        Name = name,
        //        EffectiveDate = effectiveDate,
        //        StartPeriodDate = startPeriodDate,
        //        EndPeriodDate = endPeriodDate,
        //        IsUnusedLeaveCashable = isUnusedLeaveCashable,
        //        LeaveTenures=leaveTenures,
        //        LeaveTypeRenewables=leaveTypeRenewables
        //    };
        //}
    }
}
