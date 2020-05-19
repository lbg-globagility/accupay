import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Allowance } from 'src/app/allowances/shared/allowance';

@Injectable({
  providedIn: 'root'
})
export class AllowanceService {
  baseUrl = 'api/allowances';

  constructor(private httpClient: HttpClient) { }

  getAll(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<Allowance>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Allowance>>(`${this.baseUrl}`, {
      params,
    });
  }
}
