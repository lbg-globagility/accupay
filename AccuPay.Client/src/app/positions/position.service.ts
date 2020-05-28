import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Position } from 'src/app/positions/shared/position';

@Injectable({
  providedIn: 'root',
})
export class PositionService {
  baseUrl = 'api/positions';

  constructor(private httpClient: HttpClient) {}

  getAll(options: PageOptions, term = ''): Observable<PaginatedList<Position>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Position>>(`${this.baseUrl}`, {
      params,
    });
  }

  get(id: number): Observable<Position> {
    return this.httpClient.get<Position>(`${this.baseUrl}/${id}`);
  }

  delete(id: number): Observable<Position> {
    return this.httpClient.delete<Position>(`${this.baseUrl}/${id}`);
  }
}
