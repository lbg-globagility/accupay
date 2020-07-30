import { ImportModel } from 'src/app/shared/import/import-model';

export class SalaryImportModel implements ImportModel {
  employeeNo: string;
  effectiveFrom: Date;
  basicSalary: number;
  allowanceSalary: number;
  remarks: string;
}
