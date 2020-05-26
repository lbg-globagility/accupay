import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Branch } from 'src/app/branches/shared/branch';
import { Observable } from 'rxjs';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { PageOptions } from 'src/app/core/shared/page-options';

@Injectable({
  providedIn: 'root',
})
export class BranchService {
  private baseUrl = 'api/branches';

  constructor(private httpClient: HttpClient) {}

  getById(id: number): Observable<Branch> {
    return this.httpClient.get<Branch>(`${this.baseUrl}/${id}`);
  }

  create(branch: Branch): Observable<Branch> {
    return this.httpClient.post<Branch>(`${this.baseUrl}`, branch);
  }

  update(id: number, branch: Branch): Observable<Branch> {
    return this.httpClient.put<Branch>(`${this.baseUrl}/${id}`, branch);
  }

  list(options: PageOptions): Observable<PaginatedList<Branch>> {
    const params = options.toObject();
    return this.httpClient.get<PaginatedList<Branch>>(`${this.baseUrl}`, {
      params,
    });
  }
}
