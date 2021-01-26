import { ShiftImportModel } from './shift-import-model';
import { ImportParserOutput } from '../../shared/import/import-parser-output';

export class ShiftImportParserOutput
  implements ImportParserOutput<ShiftImportModel> {
  validRecords: ShiftImportModel[];
  invalidRecords: ShiftImportModel[];
  columnHeaders: string[] = [
    'Employee ID',
    'Full Name',
    'Date',
    'Start Time',
    'End Time',
    'Break Start Time',
    'Break length',
    'Offset',
    'Remarks',
  ];
  propertyNames = Object.getOwnPropertyNames(new ShiftImportModel());
}
