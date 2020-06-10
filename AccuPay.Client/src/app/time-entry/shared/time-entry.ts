export interface TimeEntry {
  id: number;
  date: Date;
  regularHours: number;
  overtimeHours: number;
  nightDiffHours: number;
  nightDiffOTHours: number;
  lateHours: number;
  undertimeHours: number;
  absentHours: number;
}
