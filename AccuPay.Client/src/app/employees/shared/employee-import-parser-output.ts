import { EmployeeImportModel } from './employee-import-model';

export interface EmployeeImportParserOutput {
  validRecords: EmployeeImportModel[];
  invalidRecords: EmployeeImportModel[];
}
