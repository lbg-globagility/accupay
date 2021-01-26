import { ImportModel } from 'src/app/shared/import/import-model';

export class AllowanceImportModel implements ImportModel {
  employeeNo: string;
  fullName: string;
  allowanceName: string;
  effectiveStartDate: Date;
  allowanceFrequency: string;
  effectiveEndDate: Date;
  amount: number;
  remarks: string;
}
