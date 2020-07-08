import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { EmploymentPolicy } from 'src/app/employment-policies/shared';

@Injectable({
  providedIn: 'root',
})
export class EmploymentPolicyService {
  private baseUrl = 'api/employment-policies';

  constructor(private httpClient: HttpClient) {}

  getById(employmentPolicyId: number): Observable<EmploymentPolicy> {
    return this.httpClient.get<EmploymentPolicy>(
      `${this.baseUrl}/${employmentPolicyId}`
    );
  }

  create(employmentPolicy: EmploymentPolicy): Observable<EmploymentPolicy> {
    return this.httpClient.post<EmploymentPolicy>(
      `${this.baseUrl}`,
      employmentPolicy
    );
  }

  update(
    employmentPolicyId: number,
    employmentPolicy: EmploymentPolicy
  ): Observable<EmploymentPolicy> {
    return this.httpClient.put<EmploymentPolicy>(
      `${this.baseUrl}/${employmentPolicyId}`,
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
