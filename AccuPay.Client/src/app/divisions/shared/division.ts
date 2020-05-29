export interface Division {
  id: number;
  name: string;
  parentId: number;
  parentName: string;
  divisionType: string;
  workDaysPerYear: number;
  automaticOvertimeFiling: boolean;
  philHealthDeductionSchedule: string;
  sssDeductionSchedule: string;
  pagIBIGDeductionSchedule: string;
  withholdingTaxSchedule: string;
}
