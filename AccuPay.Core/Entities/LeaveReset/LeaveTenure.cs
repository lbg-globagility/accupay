using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Core.Entities.LeaveReset
{
    [Table("leavetenure")]
    public class LeaveTenure
    {
        public int LeaveResetId { get; internal set; }
        public int OrdinalValue { get; internal set; }
        public decimal Min { get; internal set; }
        public decimal Max { get; internal set; }
        public decimal VacationLeaveHours { get; internal set; }
        public decimal SickLeaveHours { get; internal set; }
        public decimal OthersLeaveHours { get; internal set; }
        public decimal ParentalLeaveHours { get; internal set; }

        public LeaveReset LeaveReset { get; set; }

        public bool IsMatch(DateTime hireDate,
            TimePeriod yearRangePeriod)
        {
            var matchAnniversaryDate = GetMatchAnniversaryDate(hireDate: hireDate,
                yearRangePeriod: yearRangePeriod);
            return matchAnniversaryDate != null;
        }

        public DateTime? GetMatchAnniversaryDate(DateTime hireDate,
            TimePeriod yearRangePeriod)
        {
            var bracketDates = Enumerable.
                Range((int)Min, (int)Max - (int)Min + 1).
                Select(i => hireDate.AddYears(i));

            var matchTenureBracket = bracketDates.
                Where(d => yearRangePeriod.Start <= d).
                Where(d => yearRangePeriod.End >= d);

            if (!matchTenureBracket.Any()) return null;

            return matchTenureBracket.LastOrDefault();
        }

        public bool IsProrated => OrdinalValue == 1;

        public decimal GetVacationLeaveHours(Employee employee, TimePeriod yearRangePeriod)
        {
            var leaveType = LeaveTypeEnum.Vacation;
            var startDate = LeaveReset.GetBasisDate(employee: employee,
                leaveTypeEnum: leaveType);
            if (IsProrated)
            {
                return GetCorrectLeaveHours(leaveTypeEnum: leaveType,
                    startDate: startDate,
                    yearRangePeriod: yearRangePeriod);
            }

            return VacationLeaveHours;
        }

        public decimal GetSickLeaveHours(Employee employee, TimePeriod yearRangePeriod)
        {
            var leaveType = LeaveTypeEnum.Sick;
            var startDate = LeaveReset.GetBasisDate(employee: employee,
                leaveTypeEnum: leaveType);
            if (IsProrated)
            {
                return GetCorrectLeaveHours(leaveTypeEnum: leaveType,
                    startDate: startDate,
                    yearRangePeriod: yearRangePeriod);
            }

            return SickLeaveHours;
        }

        public decimal GetOthersLeaveHours(Employee employee, TimePeriod yearRangePeriod)
        {
            var leaveType = LeaveTypeEnum.Others;
            var startDate = LeaveReset.GetBasisDate(employee: employee,
                leaveTypeEnum: leaveType);
            if (IsProrated)
            {
                return GetCorrectLeaveHours(leaveTypeEnum: leaveType,
                    startDate: startDate,
                    yearRangePeriod: yearRangePeriod);
            }

            return OthersLeaveHours;
        }

        public decimal GetParentalLeaveHours(Employee employee, TimePeriod yearRangePeriod)
        {
            var leaveType = LeaveTypeEnum.Parental;
            var startDate = LeaveReset.GetBasisDate(employee: employee,
                leaveTypeEnum: leaveType);
            if (IsProrated)
            {
                return GetCorrectLeaveHours(leaveTypeEnum: leaveType,
                    startDate: startDate,
                    yearRangePeriod: yearRangePeriod);
            }

            return ParentalLeaveHours;
        }

        public decimal GetCorrectLeaveHours(LeaveTypeEnum leaveTypeEnum,
            DateTime startDate,
            TimePeriod yearRangePeriod)
        {
            var matchAnniversaryDate = GetMatchAnniversaryDate(startDate, yearRangePeriod);
            if (matchAnniversaryDate == null)
                return 0;

            var yearRangePeriodDays =
                yearRangePeriod.End.Subtract(yearRangePeriod.Start).TotalDays;
            var dayDiffYearRangePeriodStartAndStartDate =
                matchAnniversaryDate.Value.Subtract(yearRangePeriod.Start).TotalDays;

            var percentage = (decimal)(dayDiffYearRangePeriodStartAndStartDate / yearRangePeriodDays);

            switch (leaveTypeEnum.Type)
            {
                case ProductConstant.VACATION_LEAVE:
                    return percentage * VacationLeaveHours;

                case ProductConstant.SICK_LEAVE:
                    return percentage * SickLeaveHours;

                case ProductConstant.OTHERS_LEAVE:
                    return percentage * OthersLeaveHours;

                case ProductConstant.PARENT_LEAVE:
                    return percentage * ParentalLeaveHours;

                default:
                    return 0;
            }
        }
    }
}
