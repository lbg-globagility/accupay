export interface Shift {
  id: number;
  employeeId: number;
  employeeNumber: string;
  employeeName: string;
  employeeType: string;
  date: Date;
  startTime: Date;
  endTime: Date;
  breakStartTime: Date;
  breakLength: number;
  isOffset: boolean;
}
