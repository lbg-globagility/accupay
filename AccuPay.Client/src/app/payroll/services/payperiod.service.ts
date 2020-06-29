import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Paystub } from 'src/app/payroll/shared/paystub';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PayrollResult } from '../shared/payroll-result';
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root',
})
export class PayPeriodService {
  private baseUrl = 'api/payperiods';

  readonly payslipFileName = 'payslip.pdf';

  constructor(private httpClient: HttpClient) {}

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

  getPaystubs(
    payPeriodId: number,
    options: PageOptions,
    term: string = ''
  ): Observable<PaginatedList<Paystub>> {
    const params = options ? options.toObject() : null;
    params.term = term;

    return this.httpClient.get<PaginatedList<Paystub>>(
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

  getDocument(payPeriodId: number): Promise<any> {
    return new Promise((resolve, reject) => {
      this.httpClient
        .get(`${this.baseUrl}/${payPeriodId}/payslips`, {
          responseType: 'blob',
        })
        .subscribe(
          (blob) => {
            saveAs(blob, this.payslipFileName);
            resolve();
          },
          (error) => {
            console.log('Error downloading the file.');
            reject(error);
          }
        );
    });
  }
}
