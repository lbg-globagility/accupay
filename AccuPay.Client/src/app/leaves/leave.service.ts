import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Leave } from 'src/app/leaves/shared/leave';
import { Moment } from 'moment';
import { LeaveTransaction } from './shared/leave-transaction';
import { LeaveBalance } from './shared/leave-balance';
import { BasePdfService } from '../core/shared/services/base-pdf-service';

@Injectable({
  providedIn: 'root',
})
export class LeaveService extends BasePdfService {
  baseUrl = 'api/leaves';

  readonly leaveTemplateFileName = 'accupay-leave-template.xlsx';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  getAll(
    options: PageOptions,
    term = '',
    dateFrom?: Moment,
    dateTo?: Moment
  ): Observable<PaginatedList<Leave>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    if (dateFrom) {
      params.dateFrom = dateFrom?.toISOString();
    }
    if (dateTo) {
      params.dateTo = dateTo?.toISOString();
    }

    return this.httpClient.get<PaginatedList<Leave>>(`${this.baseUrl}`, {
      params,
    });
  }

  getBalance(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<LeaveBalance>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<LeaveBalance>>(
      `${this.baseUrl}/ledger`,
      {
        params,
      }
    );
  }

  getLedger(
    options: PageOptions,
    id: number,
    type = ''
  ): Observable<PaginatedList<LeaveTransaction>> {
    const params = options ? options.toObject() : null;
    params.type = type;
    return this.httpClient.get<PaginatedList<LeaveTransaction>>(
      `${this.baseUrl}/ledger/${id}`,
      {
        params,
      }
    );
  }

  get(id: number): Observable<Leave> {
    return this.httpClient.get<Leave>(`${this.baseUrl}/${id}`);
  }

  create(leave: Leave): Observable<Leave> {
    return this.httpClient.post<Leave>(`${this.baseUrl}`, leave);
  }

  update(leave: Leave, id: number): Observable<Leave> {
    return this.httpClient.put<Leave>(`${this.baseUrl}/${id}`, leave);
  }

  delete(id: number): Observable<Leave> {
    return this.httpClient.delete<Leave>(`${this.baseUrl}/${id}`);
  }

  getLeaveTypes(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/types`);
  }

  getStatusList(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/statuslist`);
  }

  getLeaveTemplate(): Promise<any> {
    return this.getPDF(
      this.leaveTemplateFileName,
      `${this.baseUrl}/accupay-leave-template`
    );
  }
}
