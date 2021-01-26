export class PayPeriod {
  public id: number;
  public cutoffStart: Date;
  public cutoffEnd: Date;
  public status: string;

  public month: number;
  public year: number;
  public isFirstHalf: boolean;
}
