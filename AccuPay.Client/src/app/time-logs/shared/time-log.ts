export interface TimeLog {
  id: number;
  employeeId: number;
  employeeNumber: string;
  employeeName: string;
  employeeType: string;
  date: Date;
  startTime: Date;
  endTime: Date;
  branchId: number;
  branchName: string;
}
