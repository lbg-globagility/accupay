export interface Allowance {
  id: number;
  employeeId: number;
  employeeNumber: string;
  employeeType: string;
  employeeName: string;
  allowanceTypeId: number;
  allowanceType: string;
  frequency: string;
  startDate: Date;
  endDate: Date;
  amount: number;
}
