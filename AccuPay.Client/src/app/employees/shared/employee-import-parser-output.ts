import { EmployeeImportModel } from './employee-import-model';
import { ImportParserOutput } from '../../shared/import/import-parser-output';

export class EmployeeImportParserOutput
  implements ImportParserOutput<EmployeeImportModel> {
  validRecords: EmployeeImportModel[];
  invalidRecords: EmployeeImportModel[];
  columnHeaders: string[] = [
    'Employee ID',
    'Last name',
    'First name',
    'Middle name',
    'Birth date',
    'Gender(M/F)',
    'Nickname',
    'Marital Status',
    'Salutation',
    'Address',
    'Contact No.',
    'Job position',
    'TIN',
    'SSS No.',
    'PhilHealth No.',
    'PAGIBIG No.',
    'Date employed',
    'Employee Type',
    'Employment status',
    'VL allowance per year',
    'SL allowance per year',
    'Works days per year',
    'Branch',
    'Current VL balance',
    'Current SL balance',
    'ATM No./Account No.',
    'Remarks',
  ];
}
