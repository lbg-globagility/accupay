import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Observable } from 'rxjs';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Employee } from 'src/app/employees/shared/employee';
import { TimeLogFilter } from '../shared/time-log-filter';
import { TimeLog } from '../shared/timelog';

@Injectable({
  providedIn: 'root',
})
export class TimeLogService {
  apiRoute = 'api/TimeLogs';

  constructor(private httpClient: HttpClient) {}

  getList(options: PageOptions, body: TimeLogFilter) {
    const params = options ? options.toObject() : null;

    return this.httpClient.post<PaginatedList<TimeLog>>(
      `${this.apiRoute}/filter`,
      body,
      {
        params,
      }
    );
  }
}
