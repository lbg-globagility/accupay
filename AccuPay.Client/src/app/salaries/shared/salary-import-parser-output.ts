import { SalaryImportModel } from '../shared/salary-import-model';
import { ImportParserOutput } from '../../shared/import/import-parser-output';

export class SalaryImportParserOutput
  implements ImportParserOutput<SalaryImportModel> {
  validRecords: SalaryImportModel[];
  invalidRecords: SalaryImportModel[];
  columnHeaders: string[] = [
    'Employee No',
    'Employee Name',
    'Effective From',
    'Basic Salary',
    'Allowance Salary',
    'Remarks',
  ];
}
