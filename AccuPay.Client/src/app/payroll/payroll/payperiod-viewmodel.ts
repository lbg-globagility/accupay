import { PayPeriod } from '../shared/payperiod';

export class PayPeriodViewModel {
  isUpdating: boolean;
  payPeriod: PayPeriod;

  constructor(payPeriod: PayPeriod) {
    this.payPeriod = payPeriod;
    this.isUpdating = false;
  }
}
