import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class SelfServiceOvertimeService {
  private baseUrl: string = 'api/self-service/overtimes';

  constructor(private httpClient: HttpClient) {}

  list(): Observable<PaginatedList<Overtime>> {
    return this.httpClient.get<PaginatedList<Overtime>>(`${this.baseUrl}`);
  }

  create(overtime: Overtime): Observable<Overtime> {
    return this.httpClient.post<Overtime>(`${this.baseUrl}`, overtime);
  }
}
