import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { AllowanceType } from 'src/app/allowances/shared/allowance-type';
import { Allowance } from 'src/app/allowances/shared/allowance';
import { BasePdfService } from '../core/shared/services/base-pdf-service';
import { AllowanceImportParserOutput } from './shared/allowance-import-parser-output';

@Injectable({
  providedIn: 'root',
})
export class AllowanceService extends BasePdfService {
  baseUrl = 'api/allowances';

  readonly allowanceTemplateFileName = 'accupay-allowance-template.xlsx';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

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

  getAllowanceTypes(): Observable<AllowanceType[]> {
    return this.httpClient.get<AllowanceType[]>(`${this.baseUrl}/types`);
  }

  GetFrequencyList(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}/frequencylist`);
  }

  getAllowanceTemplate(): Promise<any> {
    return this.getFile(
      this.allowanceTemplateFileName,
      `${this.baseUrl}/accupay-allowance-template`
    );
  }

  import(file: File): Observable<AllowanceImportParserOutput> {
    const formData = new FormData();
    formData.append('file', file);

    return this.httpClient.post<AllowanceImportParserOutput>(
      `${this.baseUrl}/import`,
      formData
    );
  }
}
