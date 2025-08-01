import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { Paystub } from 'src/app/payroll/shared/paystub';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PayrollResult } from '../shared/payroll-result';
import { BasePdfService } from 'src/app/core/shared/services/base-pdf-service';
import { PaginatedList } from 'src/app/core/shared/paginated-list';

@Injectable({
  providedIn: 'root',
})
export class PayPeriodService extends BasePdfService {
  private baseUrl = 'api/payperiods';

  readonly payslipFileName = 'payslip.pdf';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  getList(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<PayPeriod>> {
    const params = options ? options.toObject() : null;
    params.term = term;

    return this.httpClient.get<PaginatedList<PayPeriod>>(this.baseUrl, {
      params,
    });
  }

  getYearlyPayPeriods(year: number): Observable<PayPeriod[]> {
    if (!year) {
      year = new Date().getFullYear();
    }

    return this.httpClient.get<PayPeriod[]>(`${this.baseUrl}/year/${year}`);
  }

  start(
    month: number,
    year: number,
    isFirstHalf: boolean
  ): Observable<PayPeriod> {
    return this.httpClient.post<PayPeriod>(`${this.baseUrl}`, {
      month,
      year,
      isFirstHalf,
    });
  }

  close(payPeriodId: number): Observable<PayPeriod> {
    return this.httpClient.put<PayPeriod>(
      `${this.baseUrl}/${payPeriodId}/close`,
      {}
    );
  }

  reopen(payPeriodId: number): Observable<PayPeriod> {
    return this.httpClient.put<PayPeriod>(
      `${this.baseUrl}/${payPeriodId}/reopen`,
      {}
    );
  }

  delete(payPeriodId: number): Observable<PayPeriod> {
    return this.httpClient.put<PayPeriod>(
      `${this.baseUrl}/${payPeriodId}/delete`,
      {}
    );
  }

  cancel(payPeriodId: number): Observable<PayPeriod> {
    return this.httpClient.put<PayPeriod>(
      `${this.baseUrl}/${payPeriodId}/cancel`,
      {}
    );
  }

  getLatest(): Observable<PayPeriod> {
    return this.httpClient.get<PayPeriod>(`${this.baseUrl}/latest`);
  }

  getById(payPeriodId: number): Observable<PayPeriod> {
    return this.httpClient.get<PayPeriod>(`${this.baseUrl}/${payPeriodId}`);
  }

  getPaystubs(
    payPeriodId: number,
    options: PageOptions,
    term: string = ''
  ): Observable<Paystub[]> {
    const params = options ? options.toObject() : null;
    params.term = term;

    return this.httpClient.get<Paystub[]>(
      `${this.baseUrl}/${payPeriodId}/paystubs`,
      {
        params,
      }
    );
  }

  calculate(payPeriodId: number): Observable<PayrollResult> {
    return this.httpClient.post<PayrollResult>(
      `${this.baseUrl}/${payPeriodId}/calculate`,
      {}
    );
  }

  getPayslipPDF(payPeriodId: number): Promise<any> {
    return this.getFile(
      this.payslipFileName,
      `${this.baseUrl}/${payPeriodId}/payslips`
    );
  }
}
