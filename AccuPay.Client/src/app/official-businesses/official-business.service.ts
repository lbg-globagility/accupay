import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { Moment } from 'moment';

@Injectable({
  providedIn: 'root',
})
export class OfficialBusinessService {
  baseUrl = 'api/officialbusinesses';

  constructor(private httpClient: HttpClient) {}

  getAll(
    options: PageOptions,
    term = '',
    dateFrom: Moment = null,
    dateTo: Moment = null
  ): Observable<PaginatedList<OfficialBusiness>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    if (dateFrom) {
      params.dateFrom = dateFrom.toISOString();
    }
    if (dateTo) {
      params.dateTo = dateTo.toISOString();
    }

    return this.httpClient.get<PaginatedList<OfficialBusiness>>(
      `${this.baseUrl}`,
      {
        params,
      }
    );
  }

  get(id: number): Observable<OfficialBusiness> {
    return this.httpClient.get<OfficialBusiness>(`${this.baseUrl}/${id}`);
  }

  create(officialBusiness: OfficialBusiness): Observable<OfficialBusiness> {
    return this.httpClient.post<OfficialBusiness>(
      `${this.baseUrl}`,
      officialBusiness
    );
  }

  update(
    officialBusiness: OfficialBusiness,
    id: number
  ): Observable<OfficialBusiness> {
    return this.httpClient.put<OfficialBusiness>(
      `${this.baseUrl}/${id}`,
      officialBusiness
    );
  }

  delete(id: number): Observable<OfficialBusiness> {
    return this.httpClient.delete<OfficialBusiness>(`${this.baseUrl}/${id}`);
  }

  getStatusList(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/statuslist`);
  }
}
