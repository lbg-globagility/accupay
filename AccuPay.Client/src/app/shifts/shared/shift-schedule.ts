export interface ShiftSchedule {
  id: number;
  employeeId: number;
  date: string;
  startTime: Date;
  endTime: Date;
  breakStartTime: Date;
  breakLength: number;
  isOffset: boolean;
}
