import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Observable } from 'rxjs';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { AllowanceType } from 'src/app/allowances/shared/allowance-type';

@Injectable({
  providedIn: 'root',
})
export class AllowanceTypeService {
  constructor(private httpClient: HttpClient) {}

  getList(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<AllowanceType>> {
    const params = options ? options.toObject() : null;
    params.term = term;

    return this.httpClient.get<PaginatedList<AllowanceType>>(
      `api/allowancetypes`,
      {
        params,
      }
    );
  }

  get(id: number): Observable<AllowanceType> {
    return this.httpClient.get<AllowanceType>(`api/allowancetypes/${id}`);
  }

  create(allowanceType: AllowanceType): Observable<AllowanceType> {
    return this.httpClient.post<AllowanceType>(
      `api/allowancetypes`,
      allowanceType
    );
  }

  update(allowanceType: AllowanceType, id: number): Observable<AllowanceType> {
    return this.httpClient.put<AllowanceType>(
      `api/allowancetypes/${id}`,
      allowanceType
    );
  }

  delete(id: number): Observable<AllowanceType> {
    return this.httpClient.delete<AllowanceType>(`api/allowancetypes/${id}`);
  }
}
