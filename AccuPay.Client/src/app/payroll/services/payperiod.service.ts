import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Paystub } from 'src/app/payroll/shared/paystub';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PayrollResult } from '../shared/payroll-result';

@Injectable({
  providedIn: 'root',
})
export class PayPeriodService {
  private baseUrl = 'api/payperiods';

  constructor(private httpClient: HttpClient) {}

  start(cutoffStart: Date, cutoffEnd: Date): Observable<void> {
    return this.httpClient.post<void>(`${this.baseUrl}`, {
      cutoffStart,
      cutoffEnd,
    });
  }

  getLatest(): Observable<PayPeriod> {
    return this.httpClient.get<PayPeriod>(`${this.baseUrl}/latest`);
  }

  getById(payPeriodId: number): Observable<PayPeriod> {
    return this.httpClient.get<PayPeriod>(`${this.baseUrl}/${payPeriodId}`);
  }

  getPaystubs(payPeriodId: number): Observable<Paystub[]> {
    return this.httpClient.get<Paystub[]>(
      `${this.baseUrl}/${payPeriodId}/paystubs`
    );
  }

  calculate(payPeriodId: number): Observable<PayrollResult> {
    return this.httpClient.post<PayrollResult>(
      `${this.baseUrl}/${payPeriodId}/calculate`,
      {}
    );
  }

  GetList(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<PayPeriod>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<PayPeriod>>(`${this.baseUrl}`, {
      params,
    });
  }
}
