import { AllowanceImportModel } from './allowance-import-model';
import { ImportParserOutput } from '../../shared/import/import-parser-output';

export class AllowanceImportParserOutput
  implements ImportParserOutput<AllowanceImportModel> {
  validRecords: AllowanceImportModel[];
  invalidRecords: AllowanceImportModel[];
  columnHeaders: string[] = [
    'EmployeeID',
    'Employee Name',
    'Name of allowance',
    'Effective start date',
    'Effective end date',
    'Allowance frequency',
    'Allowance amount',
    'Remarks',
  ];
  propertyNames = Object.getOwnPropertyNames(new AllowanceImportModel());
}
