import { LoanImportModel } from './loan-import-model';

export interface LoanImportParserOutput {
  validRecords: LoanImportModel[];
  invalidRecords: LoanImportModel[];
}
