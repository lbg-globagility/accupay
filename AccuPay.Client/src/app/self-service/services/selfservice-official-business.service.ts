import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';

@Injectable({
  providedIn: 'root',
})
export class SelfserviceOfficialBusinessService {
  private baseUrl: string = 'api/self-service/official-businesses';

  constructor(private httpClient: HttpClient) {}

  create(officialBusiness: OfficialBusiness): Observable<OfficialBusiness> {
    return this.httpClient.post<OfficialBusiness>(
      `${this.baseUrl}`,
      officialBusiness
    );
  }

  list(options: PageOptions): Observable<PaginatedList<OfficialBusiness>> {
    const params = options.toObject();

    return this.httpClient.get<PaginatedList<OfficialBusiness>>(
      `${this.baseUrl}`,
      { params }
    );
  }
}
