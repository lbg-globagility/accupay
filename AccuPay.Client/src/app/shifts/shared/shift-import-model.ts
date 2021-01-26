import { ImportModel } from 'src/app/shared/import/import-model';

export class ShiftImportModel implements ImportModel {
  employeeNo: string;
  fullName: string;
  date: Date;
  timeFromDisplay: Date;
  timeToDisplay: Date;
  breakFromDisplay: Date;
  breakLength: number;
  isRestDayText: boolean;
  remarks: string;
}
