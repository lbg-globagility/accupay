import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Salary } from 'src/app/salaries/shared/salary';

@Injectable({
  providedIn: 'root'
})
export class SalaryService {
  baseUrl = 'api/salaries';

  constructor(private httpClient: HttpClient) { }

  getAll(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<Salary>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Salary>>(`${this.baseUrl}`, {
      params,
    });
  }
}
