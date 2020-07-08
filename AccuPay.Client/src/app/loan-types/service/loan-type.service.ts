import { Injectable } from '@angular/core';
import { LoanType } from '../shared/loan-type';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';

@Injectable({
  providedIn: 'root',
})
export class LoanTypeService {
  apiRoute = 'api/LoanTypes';

  constructor(private httpClient: HttpClient) {}

  getList(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<LoanType>> {
    const params = options ? options.toObject() : null;
    params.term = term;

    return this.httpClient.get<PaginatedList<LoanType>>(this.apiRoute, {
      params,
    });
  }

  update(loanType: LoanType, id: number): Observable<LoanType> {
    return this.httpClient.put<LoanType>(`${this.apiRoute}/${id}`, loanType);
  }

  get(id: number): Observable<LoanType> {
    return this.httpClient.get<LoanType>(`${this.apiRoute}/${id}`);
  }

  create(loanType: LoanType): Observable<LoanType> {
    return this.httpClient.post<LoanType>(this.apiRoute, loanType);
  }

  delete(id: number) {
    return this.httpClient.delete<LoanType>(`${this.apiRoute}/${id}`);
  }
}
