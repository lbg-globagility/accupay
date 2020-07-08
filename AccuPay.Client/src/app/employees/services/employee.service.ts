import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Employee } from '../shared/employee';
import { HttpClient } from '@angular/common/http';
import { BasePdfService } from 'src/app/core/shared/services/base-pdf-service';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService extends BasePdfService {
  apiRoute = 'api/employees';

  readonly employeeTemplateFileName = 'accupay-employeelist-template.xlsx';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  list2(
    options: PageOptions,
    searchTerm: string = ''
  ): Observable<PaginatedList<Employee>> {
    const params = options ? options.toObject() : null;
    if (searchTerm != null) {
      params.term = searchTerm;
    }

    return this.httpClient.get<PaginatedList<Employee>>(this.apiRoute, {
      params,
    });
  }

  list(options: PageOptions): Observable<PaginatedList<Employee>> {
    const params = options ? options.toObject() : null;

    return this.httpClient.get<PaginatedList<Employee>>(this.apiRoute, {
      params,
    });
  }

  getAll(): Observable<Employee[]> {
    return this.httpClient
      .get<PaginatedList<Employee>>(`${this.apiRoute}?all=true`)
      .pipe(
        map((data) => {
          return data.items;
        })
      );
  }

  getById(id: number): Observable<Employee> {
    return this.httpClient.get<Employee>(`${this.apiRoute}/${id}`);
  }

  update(id: number, employee: Employee) {
    return this.httpClient.put(`${this.apiRoute}/${id}`, employee);
  }

  create(employee: Employee): Observable<Employee> {
    return this.httpClient.post<Employee>(this.apiRoute, employee);
  }

  getEmployeeTemplate(): Promise<any> {
    return this.getFile(
      this.employeeTemplateFileName,
      `${this.apiRoute}/accupay-employeelist-template`
    );
  }
}
