export interface LoanTransaction {
  id: number;
  loanNumber: string;
  loanType: string;
  totalAmount: number;
  deductionAmount: number;
  balance: number;
}
