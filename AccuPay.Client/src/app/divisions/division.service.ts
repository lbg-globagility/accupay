import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Division } from 'src/app/divisions/shared/division';

@Injectable({
  providedIn: 'root',
})
export class DivisionService {
  baseUrl = 'api/divisions';

  constructor(private httpClient: HttpClient) {}

  getAll(): Observable<Division[]> {
    return this.httpClient.get<PaginatedList<Division>>(this.baseUrl).pipe(
      map((data) => {
        return data.items;
      })
    );
  }
}
