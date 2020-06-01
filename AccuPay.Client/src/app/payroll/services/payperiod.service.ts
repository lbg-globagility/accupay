import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Payperiod } from 'src/app/payroll/shared/payperiod';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Paystub } from 'src/app/payroll/shared/paystub';

@Injectable({
  providedIn: 'root',
})
export class PayperiodService {
  private baseUrl = 'api/payperiods';

  constructor(private httpClient: HttpClient) {}

  start(cutoffStart: Date, cutoffEnd: Date): Observable<void> {
    return this.httpClient.post<void>(`${this.baseUrl}`, {
      cutoffStart,
      cutoffEnd,
    });
  }

  getLatest(): Observable<Payperiod> {
    return this.httpClient.get<Payperiod>(`${this.baseUrl}/latest`);
  }

  getById(payperiodId: number): Observable<Payperiod> {
    return this.httpClient.get<Payperiod>(`${this.baseUrl}/${payperiodId}`);
  }

  getPaystubs(payperiodId: number): Observable<Paystub> {
    return this.httpClient.get<Paystub[]>(
      `${this.baseUrl}/${payperiodId}/paystubs`
    );
  }

  list(): Observable<PaginatedList<Payperiod>> {
    return this.httpClient.get<PaginatedList<Payperiod>>(`${this.baseUrl}`);
  }
}
