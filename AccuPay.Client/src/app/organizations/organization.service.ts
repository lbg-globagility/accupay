import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Organization } from 'src/app/organizations/shared/organization';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';

@Injectable({
  providedIn: 'root',
})
export class OrganizationService {
  private baseUrl = 'api/organizations';

  constructor(private httpService: HttpClient) {}

  getAll(options: PageOptions): Observable<PaginatedList<Organization>> {
    const params = options ? options.toObject() : null;

    return this.httpService.get<PaginatedList<Organization>>(
      `${this.baseUrl}`,
      { params }
    );
  }
}
