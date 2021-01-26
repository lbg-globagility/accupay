import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Position } from 'src/app/positions/shared/position';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class PositionService {
  baseUrl = 'api/positions';

  constructor(private httpClient: HttpClient) {}

  list(options: PageOptions, term = ''): Observable<PaginatedList<Position>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Position>>(`${this.baseUrl}`, {
      params,
    });
  }

  getAll(): Observable<Position[]> {
    return this.httpClient
      .get<PaginatedList<Position>>(`${this.baseUrl}?all=true`)
      .pipe(map((data) => data.items));
  }

  get(id: number): Observable<Position> {
    return this.httpClient.get<Position>(`${this.baseUrl}/${id}`);
  }

  create(overtime: Position): Observable<Position> {
    return this.httpClient.post<Position>(`${this.baseUrl}`, overtime);
  }

  update(overtime: Position, id: number): Observable<Position> {
    return this.httpClient.put<Position>(`${this.baseUrl}/${id}`, overtime);
  }

  delete(id: number): Observable<Position> {
    return this.httpClient.delete<Position>(`${this.baseUrl}/${id}`);
  }
}
