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
