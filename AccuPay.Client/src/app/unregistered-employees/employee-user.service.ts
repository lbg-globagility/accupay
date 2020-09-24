import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedList } from '../core/shared/paginated-list';
import { Employee } from '../employees/shared/employee';
import { EmployeePageOptions } from '../employees/shared/employee-page-options';

@Injectable({
  providedIn: 'root',
})
export class EmployeeUserService {
  apiRoute = 'api/employeeusers';

  constructor(private httpClient: HttpClient) {}

  getEmployees(
    options: EmployeePageOptions
  ): Observable<PaginatedList<Employee>> {
    const params = options ? options.toObject() : null;

    return this.httpClient.get<PaginatedList<Employee>>(
      this.apiRoute + '/employees',
      {
        params,
      }
    );
  }
}
