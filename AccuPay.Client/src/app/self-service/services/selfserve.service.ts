import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Leave } from 'src/app/leaves/shared/leave';
import { HttpClient } from '@angular/common/http';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { TimeEntry } from 'src/app/time-entry/shared/time-entry';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';

@Injectable({
  providedIn: 'root',
})
export class SelfserveService {
  apiRoute = 'api/SelfServiceFiling';

  constructor(protected httpClient: HttpClient) {}

  getOfficialBusinessStatusList(): Observable<string[]> {
    return this.httpClient.get<string[]>(
      `${this.apiRoute}/official-business-statuses`
    );
  }

  getEmployeeTimeEntryByPeriod(payPeriodId: number): Observable<TimeEntry[]> {
    return this.httpClient.get<TimeEntry[]>(
      `${this.apiRoute}/timeentry/${payPeriodId}`
    );
  }

  getPayperiodsByYear(year: number): Observable<PayPeriod[]> {
    if (!year) {
      year = new Date().getFullYear();
    }

    return this.httpClient.get<PayPeriod[]>(`${this.apiRoute}/year/${year}`);
  }

  getLatest(): Observable<PayPeriod> {
    return this.httpClient.get<PayPeriod>(`${this.apiRoute}/latest`);
  }
}
