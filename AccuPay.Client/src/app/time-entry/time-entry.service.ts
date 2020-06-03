import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TimeEntryService {
  private baseUrl = 'api/timeentries';

  constructor(private httpClient: HttpClient) {}

  generate(payPeriodId: number): Observable<void> {
    return this.httpClient.post<void>(
      `${this.baseUrl}/generate/${payPeriodId}`,
      {}
    );
  }
}
