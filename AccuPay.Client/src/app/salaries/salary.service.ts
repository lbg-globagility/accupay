import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Salary } from 'src/app/salaries/shared/salary';
import { BasePdfService } from '../core/shared/services/base-pdf-service';

@Injectable({
  providedIn: 'root',
})
export class SalaryService extends BasePdfService {
  baseUrl = 'api/salaries';

  readonly salaryTemplateFileName = 'accupay-salary-template.xlsx';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  list(
    options: PageOptions,
    term: string = '',
    employeeId?: number
  ): Observable<PaginatedList<Salary>> {
    const params = options ? options.toObject() : null;
    if (term) {
      params.term = term;
    }
    if (params) {
      params.employeeId = `${employeeId}`;
    }
    return this.httpClient.get<PaginatedList<Salary>>(`${this.baseUrl}`, {
      params,
    });
  }

  get(id: number): Observable<Salary> {
    return this.httpClient.get<Salary>(`${this.baseUrl}/${id}`);
  }

  getLatest(employeeId: number): Observable<Salary> {
    return this.httpClient.get<Salary>(
      `${this.baseUrl}/latest?employeeId=${employeeId}`
    );
  }

  create(salary: Salary): Observable<Salary> {
    return this.httpClient.post<Salary>(`${this.baseUrl}`, salary);
  }

  update(salary: Salary, id: number): Observable<Salary> {
    return this.httpClient.put<Salary>(`${this.baseUrl}/${id}`, salary);
  }

  delete(id: number): Observable<Salary> {
    return this.httpClient.delete<Salary>(`${this.baseUrl}/${id}`);
  }

  getSalaryTemplate(): Promise<any> {
    return this.getPDF(
      this.salaryTemplateFileName,
      `${this.baseUrl}/accupay-salary-template`
    );
  }
}
