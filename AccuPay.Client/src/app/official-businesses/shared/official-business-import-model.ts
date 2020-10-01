import { ImportModel } from '../../shared/import/import-model';

export class OfficialBusinessImportModel implements ImportModel {
  employeeNo: string;
  fullName: string;
  startDate: Date;
  startTime: Date;
  endTime: Date;
  remarks: string;
}
