import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PageOptions } from '../core/shared/page-options';
import { PaginatedList } from '../core/shared/paginated-list';
import { Employee } from 'src/app/time-entry/shared/employee';
import { PayPeriod } from './shared/payPeriod';

@Injectable({
  providedIn: 'root',
})
export class TimeEntryService {
  private baseUrl = 'api/timeentries';

  constructor(private httpClient: HttpClient) {}

  generate(payPeriodId: number): Observable<void> {
    return this.httpClient.post<void>(
      `${this.baseUrl}/${payPeriodId}/generate`,
      {}
    );
  }

  getEmployees(
    payPeriodId: number,
    options: PageOptions,
    term: string = ''
  ): Observable<PaginatedList<Employee>> {
    const params = options ? options.toObject() : null;
    params.term = term;

    return this.httpClient.get<PaginatedList<Employee>>(
      `${this.baseUrl}/${payPeriodId}/employees`,
      {
        params,
      }
    );
  }

  getDetails(payPeriodId: number): Observable<PayPeriod> {
    return this.httpClient.get<PayPeriod>(`${this.baseUrl}/${payPeriodId}`);
  }
}
