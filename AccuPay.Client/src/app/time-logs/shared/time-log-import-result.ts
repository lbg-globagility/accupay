import { TimeLog } from './time-log';
import { TimeLogImportDetails } from './time-log-import-details';

export interface TimeLogImportResult {
  generatedTimeLogs: TimeLog[];
  invalidRecords: TimeLogImportDetails[];
  // validRecords: TimeLogImportDetails[];
}
