export interface Salary {
  id: number;
  employeeId: number;
  employeeNumber: string;
  employeeName: string;
  employeeType: string;
  effectiveFrom: Date;
  basicSalary: number;
  allowanceSalary: number;
  totalSalary: number;
  doPaySSSContribution: Boolean;
  autoComputePhilHealthContribution: Boolean;
  philHealthDeduction: number;
  autoComputeHDMFContribution: Boolean;
  hdmfDeduction: number;
}
