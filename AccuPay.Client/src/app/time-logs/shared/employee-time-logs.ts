export interface EmployeeTimeLogs {
  employeeId: number;
  employeeNo: string;
  fullName: string;
  timeLogs: {
    id: number;
    date: string;
    startTime: string;
    endTime: string;
  }[];
}
