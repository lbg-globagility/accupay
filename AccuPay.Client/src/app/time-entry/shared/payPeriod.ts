export interface PayPeriod {
  id: number;
  cutoffStart: Date;
  cutoffEnd: Date;
  status: string;
  timeEntryCount: number;
  shiftCount: number;
  timeLogCount: number;
  leaveCount: number;
  officialBusinessCount: number;
  overtimeCount: number;
  absentCount: number;
  lateCount: number;
  undertimeCount: number;
}
