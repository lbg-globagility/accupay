import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PayrollService {
  private baseUrl = 'api/payroll';

  constructor(private httpClient: HttpClient) {}

  start(cutoffStart: Date, cutoffEnd: Date): Observable<void> {
    return this.httpClient.post<void>(`${this.baseUrl}/payroll`, {
      cutoffStart,
      cutoffEnd,
    });
  }

  getLatest() {}

  getById() {}

  list() {}
}
