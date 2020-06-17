export interface TimeLogImportDetails {
  employeeNumber: string;
  employeeName: string;
  dateAndTime: Date;
  errorMessage: string;
  lineContent: string;
  lineNumber: number;
  type: string;
}
