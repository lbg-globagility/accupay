import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { User } from 'src/app/users/shared/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = 'api/users';

  constructor(private httpClient: HttpClient) {}

  get(id: string): Observable<User> {
    return this.httpClient.get<User>(`${this.baseUrl}/${id}`);
  }

  getAll(): Observable<PaginatedList<User>> {
    return this.httpClient.get<PaginatedList<User>>(`${this.baseUrl}?all=true`);
  }

  getByOrganization(
    options: PageOptions,
    term = '',
    organizationName = ''
  ): Observable<PaginatedList<User>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    params.organizationName = organizationName;

    return this.httpClient.get<PaginatedList<User>>(`${this.baseUrl}`, {
      params,
    });
  }

  update(user: User, id: string): Observable<any> {
    return this.httpClient.post<any>(`${this.baseUrl}/${id}`, user);
  }

  create(user: User, addDomain: boolean): Observable<User> {
    return this.httpClient.post<User>(`${this.baseUrl}`, user, {
      params: {
        addDomain: `${addDomain}`,
      },
    });
  }
}
