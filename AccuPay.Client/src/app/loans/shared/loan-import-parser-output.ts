import { LoanImportModel } from './loan-import-model';
import { ImportParserOutput } from '../../shared/import/import-parser-output';

export class LoanImportParserOutput
  implements ImportParserOutput<LoanImportModel> {
  validRecords: LoanImportModel[];
  invalidRecords: LoanImportModel[];
  columnHeaders: string[] = [
    'Employee ID',
    'Employee Name',
    'Loan name',
    'Loan number/code',
    'Start date',
    'Total loan amount',
    'Loan balance',
    'Deduction amount',
    'Deduction frequency',
    'Comments',
    'Remarks',
  ];
}
