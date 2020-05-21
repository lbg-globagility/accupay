import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { AllowanceType } from 'src/app/allowances/shared/allowance-type';
import { Allowance } from 'src/app/allowances/shared/allowance';

@Injectable({
  providedIn: 'root',
})
export class AllowanceService {
  baseUrl = 'api/allowances';

  constructor(private httpClient: HttpClient) {}

  getAll(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<Allowance>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Allowance>>(`${this.baseUrl}`, {
      params,
    });
  }

  get(id: number): Observable<Allowance> {
    return this.httpClient.get<Allowance>(`${this.baseUrl}/${id}`);
  }

  create(allowance: Allowance): Observable<Allowance> {
    return this.httpClient.post<Allowance>(`${this.baseUrl}`, allowance);
  }

  update(allowance: Allowance, id: number): Observable<Allowance> {
    return this.httpClient.put<Allowance>(`${this.baseUrl}/${id}`, allowance);
  }

  delete(id: number): Observable<Allowance> {
    return this.httpClient.delete<Allowance>(`${this.baseUrl}/${id}`);
  }

  getAllowanceTypes(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/allowancetypes`);
  }

  GetFrequencyList(): Observable<AllowanceType[]> {
    return this.httpClient.get<AllowanceType[]>(
      `${this.baseUrl}/frequencylist`
    );
  }
}
