import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EmploymentPolicy } from 'src/app/employment-policies/shared/employment-policy';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';

@Injectable({
  providedIn: 'root',
})
export class EmploymentPolicyService {
  private baseUrl = 'api/employment-policies';

  constructor(private httpClient: HttpClient) {}

  create(employmentPolicy: EmploymentPolicy): Observable<EmploymentPolicy> {
    return this.httpClient.post<EmploymentPolicy>(
      `${this.baseUrl}`,
      employmentPolicy
    );
  }

  list(options: PageOptions): Observable<PaginatedList<EmploymentPolicy>> {
    const params = options.toObject();

    return this.httpClient.get<PaginatedList<EmploymentPolicy>>(
      `${this.baseUrl}`,
      { params }
    );
  }
}
