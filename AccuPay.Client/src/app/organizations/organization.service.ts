import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Organization } from 'src/app/organizations/shared/organization';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { map } from 'rxjs/operators';
import { OrganizationPageOptions } from 'src/app/organizations/shared/organization-page-options';

@Injectable({
  providedIn: 'root',
})
export class OrganizationService {
  private baseUrl = 'api/organizations';

  constructor(private httpClient: HttpClient) {}

  getAll(): Observable<Organization[]> {
    const params = { all: 'true' };

    return this.httpClient
      .get<PaginatedList<Organization>>(`${this.baseUrl}`, {
        params,
      })
      .pipe(
        map((data) => {
          return data.items;
        })
      );
  }

  list(
    options: OrganizationPageOptions
  ): Observable<PaginatedList<Organization>> {
    const params = options ? options.toObject() : null;

    return this.httpClient.get<PaginatedList<Organization>>(`${this.baseUrl}`, {
      params,
    });
  }

  getById(organizationId: number): Observable<Organization> {
    return this.httpClient.get<Organization>(
      `${this.baseUrl}/${organizationId}`
    );
  }

  create(organization: Organization): Observable<Organization> {
    return this.httpClient.post<Organization>(`${this.baseUrl}`, organization);
  }

  update(
    organizationId: number,
    organization: Organization
  ): Observable<Organization> {
    return this.httpClient.post<Organization>(
      `${this.baseUrl}/${organizationId}`,
      organization
    );
  }
}
