export interface Paystub {
  id: number;
  netPay: number;
  employee: {
    firstName: string;
    lastName: string;
  };
}
