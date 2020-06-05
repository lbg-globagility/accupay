export interface Employee {
  id: number;
  employeeNumber: number;
  fullName: string;
  regularHours: number;
  absentHours: number;
  lateHours: number;
  undertimeHours: number;
  overtimeHours: number;
  leaveHours: number;
  nightDifferentialHours: number;
  nightDifferentialOvertimeHours: number;
  specialHolidayHours: number;
  specialHolidayOTHours: number;
  regularHolidayHours: number;
  regularHolidayOTHours: number;
  restDayHours: number;
  restDayOTHours: number;
}
