import { Injectable } from '@angular/core';
import { Role } from 'src/app/roles/shared/role';
import { Observable } from 'rxjs';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { HttpClient } from '@angular/common/http';
import { PageOptions } from 'src/app/core/shared/page-options';
import { map } from 'rxjs/operators';
import { UserRole } from 'src/app/roles/shared/user-role';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  private baseUrl = 'api/roles';

  constructor(private httpClient: HttpClient) {}

  getById(roleId: string): Observable<Role> {
    return this.httpClient.get<Role>(`${this.baseUrl}/${roleId}`);
  }

  getAll(): Observable<Role[]> {
    return this.httpClient
      .get<PaginatedList<Role>>(`${this.baseUrl}`)
      .pipe(map((data) => data.items));
  }

  getUserRoles(): Observable<UserRole[]> {
    return this.httpClient.get<UserRole[]>(`${this.baseUrl}/user-roles`);
  }

  create(role: Role): Observable<Role> {
    return this.httpClient.post<Role>(`${this.baseUrl}`, role);
  }

  update(roleId: string, role: Role): Observable<Role> {
    return this.httpClient.put<Role>(`${this.baseUrl}/${roleId}`, role);
  }

  updateUserRoles(userRoles: UserRole[]): Observable<void> {
    return this.httpClient.put<void>(`${this.baseUrl}/user-roles`, userRoles);
  }

  list(options: PageOptions): Observable<PaginatedList<Role>> {
    const params = options.toObject();

    return this.httpClient.get<PaginatedList<Role>>(`${this.baseUrl}`, {
      params,
    });
  }
}
