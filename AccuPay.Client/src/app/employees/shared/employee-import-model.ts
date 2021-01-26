import { ImportModel } from '../../shared/import/import-model';

export class EmployeeImportModel implements ImportModel {
  employeeNo: string;
  firstName: string;
  middleName: string;
  lastName: string;
  nickname: string;
  tin: string;
  sssNo: string;
  pagIbigNo: string;
  philHealthNo: string;
  employmentStatus: string;
  mobileNo: string;
  address: string;
  gender: string;
  employeeType: string;
  maritalStatus: string;
  salutation: string;
  birthdate: Date;
  startDate: Date;
  atmNo: string;
  workDaysPerYear: number;
  jobPosition: string;
  vacationLeaveAllowance: number;
  sickLeaveAllowance: number;
  branch: string;
  currentVacationLeaveBalance: number;
  currentSickLeaveBalance: number;
  remarks: string;
}
