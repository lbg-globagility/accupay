import { DatePeriod } from 'src/app/core/shared/date-period';

export interface TimeEntry {
  id: number;
  date: Date;
  shift: DatePeriod;
  timeLog: DatePeriod;
  officialBusiness: DatePeriod;
  regularHours: number;
  leave: DatePeriod;
  leavePay: number;
  leaveHours: number;
  overtimes: DatePeriod[];
  overtimeHours: number;
  overtimePay: number;
  nightDiffHours: number;
  nightDiffPay: number;
  nightDiffOTHours: number;
  nightDiffOTPay: number;
  restDayHours: number;
  restDayAmount: number;
  restDayOTHours: number;
  restDayOTPay: number;
  specialHolidayHours: number;
  specialHolidayPay: number;
  specialHolidayOTHours: number;
  specialHolidayOTPay: number;
  regularHolidayHours: number;
  regularHolidayPay: number;
  regularHolidayOTHours: number;
  regularHolidayOTPay: number;
  lateHours: number;
  lateDeduction: number;
  undertimeHours: number;
  undertimeDeduction: number;
  absentHours: number;
  absentDeduction: number;
}
