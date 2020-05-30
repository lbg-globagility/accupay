import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { TimeLog } from 'src/app/time-logs/shared/time-log';

@Injectable({
  providedIn: 'root',
})
export class TimeLogService {
  baseUrl = 'api/timelogs';

  constructor(private httpClient: HttpClient) {}

  getAll(options: PageOptions, term = ''): Observable<PaginatedList<TimeLog>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<TimeLog>>(`${this.baseUrl}`, {
      params,
    });
  }

  import(file: File): Observable<TimeLog> {
    const formData = new FormData();
    formData.append('file', file);

    return this.httpClient.post<TimeLog>(`${this.baseUrl}/import`, formData);
  }
}
