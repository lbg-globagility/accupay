import { Injectable, ElementRef, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Employee } from '../shared/employee';
import { HttpClient } from '@angular/common/http';
import { BasePdfService } from 'src/app/core/shared/services/base-pdf-service';
import { EmployeeImportParserOutput } from '../shared/employee-import-parser-output';
import { EmployeePageOptions } from 'src/app/employees/shared/employee-page-options';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService extends BasePdfService {
  apiRoute = 'api/employees';

  readonly employeeTemplateFileName = 'accupay-employeelist-template.xlsx';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  list(options: EmployeePageOptions): Observable<PaginatedList<Employee>> {
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

  getEmploymentStatuses(): Observable<string[]> {
    return this.httpClient.get<string[]>(
      `${this.apiRoute}/employment-statuses`
    );
  }

  import(file: File): Observable<EmployeeImportParserOutput> {
    const formData = new FormData();
    formData.append('file', file);

    return this.httpClient.post<EmployeeImportParserOutput>(
      `${this.apiRoute}/import`,
      formData
    );
  }
}
