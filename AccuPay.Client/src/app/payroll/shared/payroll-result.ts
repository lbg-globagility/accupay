import { PayrollResultDetails } from './payroll-result-details';

export interface PayrollResult {
  successes: number;
  failures: number;
  details: PayrollResultDetails[];
}
