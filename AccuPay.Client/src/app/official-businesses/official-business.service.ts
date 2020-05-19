import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';

@Injectable({
  providedIn: 'root'
})
export class OfficialBusinessService {
  baseUrl = 'api/officialbusinesses';

  constructor(private httpClient: HttpClient) { }

  getAll(
    options: PageOptions,
    term = ''
  ): Observable<PaginatedList<OfficialBusiness>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<OfficialBusiness>>(`${this.baseUrl}`, {
      params,
    });
  }
}
