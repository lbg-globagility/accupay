import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Leave } from 'src/app/leaves/shared/leave';
import { Observable } from 'rxjs';
import { PaginatedList } from 'src/app/core/shared/paginated-list';

@Injectable({
  providedIn: 'root',
})
export class SelfServiceLeaveService {
  private baseUrl: string = 'api/self-service/leaves';

  constructor(private httpClient: HttpClient) {}

  create(leave: Leave): Observable<Leave> {
    return this.httpClient.post<Leave>(`${this.baseUrl}`, leave);
  }

  list(): Observable<PaginatedList<Leave>> {
    return this.httpClient.get<PaginatedList<Leave>>(`${this.baseUrl}`);
  }
}
