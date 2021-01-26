import { PayrollResultDetails } from './payroll-result-details';

export interface PayrollResult {
  successes: number;
  errors: number;
  details: PayrollResultDetails[];
}
