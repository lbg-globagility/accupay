export interface Leave {
  id: number;
  employeeNumber: string;
  employeeName: string;
  employeeType: string;
  leaveType: string;
  startTime: Date;
  endTime: Date;
  startDate: Date;
  endDate: Date;
  status: string;
  reason: string;
  comments: string;
}
