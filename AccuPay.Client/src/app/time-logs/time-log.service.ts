import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { TimeLog } from 'src/app/time-logs/shared/time-log';
import { EmployeeTimeLogs } from 'src/app/time-logs/shared/employee-time-logs';
import { TimeLogImportResult } from './shared/time-log-import-result';
import { TimeLogsByEmployeePageOptions } from 'src/app/time-logs/shared/timelogs-by-employee-page-options';

@Injectable({
  providedIn: 'root',
})
export class TimeLogService {
  baseUrl = 'api/timelogs';

  constructor(private httpClient: HttpClient) {}

  listByEmployee(
    options: TimeLogsByEmployeePageOptions
  ): Observable<PaginatedList<EmployeeTimeLogs>> {
    const params = options ? options.toObject() : null;

    return this.httpClient.get<PaginatedList<EmployeeTimeLogs>>(
      `${this.baseUrl}/employees`,
      {
        params,
      }
    );
  }

  batchApply(timeLogs: any[]): Observable<void> {
    return this.httpClient.post<void>(`${this.baseUrl}`, timeLogs);
  }

  import(file: File): Observable<TimeLogImportResult> {
    const formData = new FormData();
    formData.append('file', file);

    return this.httpClient.post<TimeLogImportResult>(
      `${this.baseUrl}/import`,
      formData
    );
  }
}
