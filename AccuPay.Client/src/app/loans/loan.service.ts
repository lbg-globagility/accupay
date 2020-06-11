import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { SelectItem } from 'src/app/core/shared/select-item';
import { Loan } from 'src/app/loans/shared/loan';
import { LoanHistory } from './shared/loan-history';

@Injectable({
  providedIn: 'root',
})
export class LoanService {
  baseUrl = 'api/loans';

  constructor(private httpClient: HttpClient) {}

  getAll(options: PageOptions, term = ''): Observable<PaginatedList<Loan>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Loan>>(`${this.baseUrl}`, {
      params,
    });
  }

  get(id: number): Observable<Loan> {
    return this.httpClient.get<Loan>(`${this.baseUrl}/${id}`);
  }

  create(loan: Loan): Observable<Loan> {
    return this.httpClient.post<Loan>(`${this.baseUrl}`, loan);
  }

  update(loan: Loan, id: number): Observable<Loan> {
    return this.httpClient.put<Loan>(`${this.baseUrl}/${id}`, loan);
  }

  delete(id: number): Observable<Loan> {
    return this.httpClient.delete<Loan>(`${this.baseUrl}/${id}`);
  }

  getLoanTypes(): Observable<SelectItem[]> {
    return this.httpClient.get<SelectItem[]>(`${this.baseUrl}/types`);
  }

  getStatusList(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/statuslist`);
  }

  getDeductionSchedules(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/deductionsechedules`);
  }

  getHistory(
    options: PageOptions,
    id: number
  ): Observable<PaginatedList<LoanHistory>> {
    const params = options ? options.toObject() : null;
    return this.httpClient.get<PaginatedList<LoanHistory>>(
      `${this.baseUrl}/history/${id}`,
      {
        params,
      }
    );
  }
}
