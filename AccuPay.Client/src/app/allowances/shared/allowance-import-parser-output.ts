import { AllowanceImportModel } from './allowance-import-model';

export interface AllowanceImportParserOutput {
  validRecords: AllowanceImportModel[];
  invalidRecords: AllowanceImportModel[];
}
