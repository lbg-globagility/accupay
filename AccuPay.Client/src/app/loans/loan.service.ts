import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Loan } from 'src/app/loans/shared/loan';

@Injectable({
  providedIn: 'root'
})
export class LoanService {
  baseUrl = 'api/loans';

  constructor(private httpClient: HttpClient) { }

  getAll(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<Loan>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Loan>>(`${this.baseUrl}`, {
      params,
    });
  }
}
