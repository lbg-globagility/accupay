import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Overtime } from 'src/app/overtimes/shared/overtime';

@Injectable({
  providedIn: 'root'
})
export class OvertimeService {
  baseUrl = 'api/overtimes';

  constructor(private httpClient: HttpClient) { }

  getAll(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<Overtime>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Overtime>>(`${this.baseUrl}`, {
      params,
    });
  }
}
