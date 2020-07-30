import { ImportModel } from 'src/app/shared/import/import-model';

export class AllowanceImportModel implements ImportModel {
  // employeeId: number;
  employeeNo: string;
  // productId: number;
  // allowanceTypeId: number;
  allowanceName: string;
  effectiveStartDate: Date;
  allowanceFrequency: string;
  effectiveEndDate: Date;
  amount: number;
  remarks: string;
}
