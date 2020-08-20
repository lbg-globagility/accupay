import { ImportParserOutput } from '../../shared/import/import-parser-output';
import { OvertimeImportModel } from './overetime-import-model';

export class OvertimeImportParserOutput
  implements ImportParserOutput<OvertimeImportModel> {
  validRecords: OvertimeImportModel[];
  invalidRecords: OvertimeImportModel[];
  columnHeaders: string[] = [
    'EmployeeID',
    'Full Name',
    'Effective start date',
    'Effective Start Time',
    'Effective End Time',
    'Remarks',
  ];
  propertyNames = Object.getOwnPropertyNames(new OvertimeImportModel());
}
