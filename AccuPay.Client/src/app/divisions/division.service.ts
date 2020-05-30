import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Division } from 'src/app/divisions/shared/division';
import { PageOptions } from '../core/shared/page-options';

@Injectable({
  providedIn: 'root',
})
export class DivisionService {
  baseUrl = 'api/divisions';

  constructor(private httpClient: HttpClient) {}

  getList(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<Division>> {
    const params = options ? options.toObject() : null;
    params.term = term;

    return this.httpClient.get<PaginatedList<Division>>(this.baseUrl, {
      params,
    });
  }

  getAll(): Observable<Division[]> {
    return this.httpClient.get<PaginatedList<Division>>(this.baseUrl).pipe(
      map((data) => {
        return data.items;
      })
    );
  }

  getAllParents(): Observable<Division[]> {
    return this.httpClient.get<Division[]>(`${this.baseUrl}/parents`);
  }

  getDivisionTypes(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/types`);
  }

  getDeductionSchedules(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/schedules`);
  }

  get(id: number): Observable<Division> {
    return this.httpClient.get<Division>(`${this.baseUrl}/${id}`);
  }

  create(leave: Division): Observable<Division> {
    return this.httpClient.post<Division>(`${this.baseUrl}`, leave);
  }

  update(leave: Division, id: number): Observable<Division> {
    return this.httpClient.put<Division>(`${this.baseUrl}/${id}`, leave);
  }

  delete(id: number): Observable<Division> {
    return this.httpClient.delete<Division>(`${this.baseUrl}/${id}`);
  }
}
