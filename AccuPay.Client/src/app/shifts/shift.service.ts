import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Shift } from 'src/app/shifts/shared/shift';

@Injectable({
  providedIn: 'root',
})
export class ShiftService {
  baseUrl = 'api/shifts';

  constructor(private httpClient: HttpClient) {}

  getAll(options: PageOptions, term = ''): Observable<PaginatedList<Shift>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Shift>>(`${this.baseUrl}`, {
      params,
    });
  }

  get(id: number): Observable<Shift> {
    return this.httpClient.get<Shift>(`${this.baseUrl}/${id}`);
  }

  create(shift: Shift): Observable<Shift> {
    return this.httpClient.post<Shift>(`${this.baseUrl}`, shift);
  }

  update(shift: Shift, id: number): Observable<Shift> {
    return this.httpClient.put<Shift>(`${this.baseUrl}/${id}`, shift);
  }

  delete(id: number): Observable<Shift> {
    return this.httpClient.delete<Shift>(`${this.baseUrl}/${id}`);
  }
}
