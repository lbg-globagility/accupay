import { ImportModel } from '../../shared/import/import-model';

export class LoanImportModel implements ImportModel {
  employeeNo: string;
  employeeName: string;
  loanName: string;
  loanNumber: string;
  startDate: Date;
  totalLoanAmount: number;
  totalLoanBalance: number;
  deductionAmount: number;
  deductionSchedule: string;
  comments: string;
  remarks: string;
}
