import { Account } from 'src/app/accounts/shared/account';

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Organization } from 'src/app/accounts/shared/organization';
import { Role } from 'src/app/roles/shared/role';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private api = 'api/account';

  constructor(private httpClient: HttpClient) {}

  get(): Observable<Account> {
    return this.httpClient.get<Account>(this.api);
  }

  getOrganization(): Observable<Organization> {
    return this.httpClient.get<Organization>(`${this.api}/organization`);
  }

  getCurrentRole(): Observable<Role> {
    return this.httpClient.get<Role>(`${this.api}/current-role`);
  }

  getPermissions(role: Role): string[] {
    const permissions: string[] = [];

    for (const rolePermission of role.rolePermissions) {
      if (rolePermission.read) {
        permissions.push(`${rolePermission.permissionName}:read`);
      }

      if (rolePermission.create) {
        permissions.push(`${rolePermission.permissionName}:create`);
      }

      if (rolePermission.update) {
        permissions.push(`${rolePermission.permissionName}:update`);
      }

      if (rolePermission.delete) {
        permissions.push(`${rolePermission.permissionName}:delete`);
      }
    }

    return permissions;
  }

  updateOrganization(organization: Organization): Observable<Organization> {
    return this.httpClient.post<Organization>(
      `${this.api}/organization`,
      organization
    );
  }

  update(account: Account) {
    return this.httpClient.post(`${this.api}/edit`, account);
  }
}
