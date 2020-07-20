export interface LoanImportModel {
  employeeNo: string;
  loanName: string;
  loanNumber: string;
  startDate: Date;
  totalLoanAmount: number;
  totalLoanBalance: number;
  deductionAmount: number;
  deductionSchedule: string;
  comments: string;
}
