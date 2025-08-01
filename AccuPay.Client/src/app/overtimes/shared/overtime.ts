export interface Overtime {
  id: number;
  employeeId: number;
  employeeNumber: string;
  employeeName: string;
  employeeType: string;
  startTime: Date;
  endTime: Date;
  startDate: Date;
  endDate: Date;
  status: string;
  reason: string;
  comments: string;
}
