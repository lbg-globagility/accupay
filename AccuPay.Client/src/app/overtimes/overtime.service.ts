import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { Moment } from 'moment';
import { BasePdfService } from '../core/shared/services/base-pdf-service';

@Injectable({
  providedIn: 'root',
})
export class OvertimeService extends BasePdfService {
  baseUrl = 'api/overtimes';

  readonly OTTemplateileName = 'accupay-overtime-template.xlsx';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  getAll(
    options: PageOptions,
    term = '',
    dateFrom?: Moment,
    dateTo?: Moment
  ): Observable<PaginatedList<Overtime>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    if (dateFrom) {
      params.dateFrom = dateFrom.toISOString();
    }
    if (dateTo) {
      params.dateTo = dateTo.toISOString();
    }

    return this.httpClient.get<PaginatedList<Overtime>>(`${this.baseUrl}`, {
      params,
    });
  }

  get(id: number): Observable<Overtime> {
    return this.httpClient.get<Overtime>(`${this.baseUrl}/${id}`);
  }

  create(overtime: Overtime): Observable<Overtime> {
    return this.httpClient.post<Overtime>(`${this.baseUrl}`, overtime);
  }

  update(overtime: Overtime, id: number): Observable<Overtime> {
    return this.httpClient.put<Overtime>(`${this.baseUrl}/${id}`, overtime);
  }

  delete(id: number): Observable<Overtime> {
    return this.httpClient.delete<Overtime>(`${this.baseUrl}/${id}`);
  }

  getStatusList(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/statuslist`);
  }

  getOvertimeTemplate(): Promise<any> {
    return this.getFile(
      this.OTTemplateileName,
      `${this.baseUrl}/accupay-overtime-template`
    );
  }
}
