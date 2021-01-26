import { OfficialBusinessImportModel } from './official-business-import-model';
import { ImportParserOutput } from '../../shared/import/import-parser-output';

export class OfficialBusinessImportParserOutput
  implements ImportParserOutput<OfficialBusinessImportModel> {
  validRecords: OfficialBusinessImportModel[];
  invalidRecords: OfficialBusinessImportModel[];
  columnHeaders: string[] = [
    'Employee ID',
    'Employee Name',
    'Start Date',
    'Start Time',
    'End Time',
    'Remarks',
  ];
}
