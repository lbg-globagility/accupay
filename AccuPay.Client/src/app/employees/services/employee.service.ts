import { Injectable } from '@angular/core';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Observable } from 'rxjs';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Employee } from '../shared/employee';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  apiRoute = 'api/employees';

  constructor(private httpClient: HttpClient) {}

  getList(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<Employee>> {
    const params = options ? options.toObject() : null;
    params.term = term;

    return this.httpClient.get<PaginatedList<Employee>>(this.apiRoute, {
      params,
    });
  }

  getById(id: number): Observable<Employee> {
    return this.httpClient.get<Employee>(`${this.apiRoute}/${id}`);
  }

  update(id: number, employee: Employee) {
    return this.httpClient.put(`${this.apiRoute}/${id}`, employee);
  }

  create(employee: Employee): Observable<number> {
    return this.httpClient.post<number>(this.apiRoute, employee);
  }
}
