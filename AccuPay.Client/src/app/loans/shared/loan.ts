export interface Loan {
    id: number;
    employeeNumber: string;
    employeeName: string;
    loanType: string;
    deductionSchedule: string;
    startDate: Date;
    totalLoanAmount: number;
    totalBalanceLeft: number;
  }
  