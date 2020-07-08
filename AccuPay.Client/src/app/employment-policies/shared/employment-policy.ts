export interface EmploymentPolicy {
  id?: number;
  name: string;
  workDaysPerYear: number;
  gracePeriod: number;
  computeNightDiff: boolean;
  computeNightDiffOT: boolean;
  computeRestDay: boolean;
  computeRestDayOT: boolean;
  computeSpecialHoliday: boolean;
  computeRegularHoliday: boolean;
}
