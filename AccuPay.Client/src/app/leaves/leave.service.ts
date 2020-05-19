import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Leave } from 'src/app/leaves/shared/leave';

@Injectable({
  providedIn: 'root'
})
export class LeaveService {
  baseUrl = 'api/leaves';

  constructor(private httpClient: HttpClient) { }

  getAll(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<Leave>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Leave>>(`${this.baseUrl}`, {
      params,
    });
  }
}
