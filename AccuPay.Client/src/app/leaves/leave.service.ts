import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Leave } from 'src/app/leaves/shared/leave';

@Injectable({
  providedIn: 'root',
})
export class LeaveService {
  baseUrl = 'api/leaves';

  constructor(private httpClient: HttpClient) {}

  getAll(options: PageOptions, term = ''): Observable<PaginatedList<Leave>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Leave>>(`${this.baseUrl}`, {
      params,
    });
  }

  get(id: number): Observable<Leave> {
    return this.httpClient.get<Leave>(`${this.baseUrl}/${id}`);
  }

  create(leave: Leave): Observable<Leave> {
    return this.httpClient.post<Leave>(`${this.baseUrl}`, leave);
  }

  update(leave: Leave, id: number): Observable<Leave> {
    return this.httpClient.put<Leave>(`${this.baseUrl}/${id}`, leave);
  }

  delete(id: number): Observable<Leave> {
    return this.httpClient.delete<Leave>(`${this.baseUrl}/${id}`);
  }

  getLeaveTypes(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/types`);
  }

  getStatusList(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/statuslist`);
  }
}
