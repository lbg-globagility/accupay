import { SalaryImportModel } from '../shared/salary-import-model';

export interface SalaryImportParserOutput {
  validRecords: SalaryImportModel[];
  invalidRecords: SalaryImportModel[];
}
