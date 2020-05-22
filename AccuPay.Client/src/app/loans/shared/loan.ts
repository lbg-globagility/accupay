export interface Loan {
  id: number;
  employeeNumber: string;
  employeeName: string;
  employeeType: string;
  loanNumber: string;
  loanType: string;
  startDate: Date;
  totalLoanAmount: number;
  deductionAmount: number;
  totalBalanceLeft: number;
  deductionPercentage: number;
  status: string;
  deductionSchedule: string;
  comments: string;
}
