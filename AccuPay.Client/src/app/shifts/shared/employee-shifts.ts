export interface EmployeeShifts {
  employeeId: number;
  employeeNo: string;
  fullName: string;
  shifts: {
    id: number;
    date: string;
    startTime: string;
    endTime: string;
  }[];
}
