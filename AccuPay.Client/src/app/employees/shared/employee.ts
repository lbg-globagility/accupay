export interface Employee {
  id: number;
  payFrequencyID: string;
  firstName: string;
  middleName: string;
  lastName: string;
  employeeNo: string;
  tin: string;
  sssNo: string;
  pagIbigNo: string;
  philHealthNo: string;
  employmentStatus: string;
  emailAddress: string;
  workPhone: string;
  landlineNo: string;
  mobileNo: string;
  address: string;
  gender: string;
  employeeType: string;
  maritalStatus: string;
  birthdate: Date;
  startDate: string;
  terminationDate: string;
  noOfDependents: string;
  newEmployeeFlag: string;
  alphalistExempted: string;
  dayOfRest: string;
  atmNo: string;
  bankName: string;
  dateRegularized: string;
  dateEvaluated: string;
  revealInPayroll: string;
  agencyID: string;
  advancementPoints: string;
  bpiInsurance: string;
  fullName: string;
  regularizationDate: Date;
  employmentPolicy: {
    id: number;
    name: string;
  };
  position: {
    id: number;
    name: string;
  };
}
