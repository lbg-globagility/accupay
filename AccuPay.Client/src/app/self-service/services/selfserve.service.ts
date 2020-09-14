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

  createLeave(leave: Leave): Observable<Leave> {
    return this.httpClient.post<Leave>(`/api/self-service/leaves`, leave);
  }

  getLeaveTypes(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.apiRoute}/leave-types`);
  }

  getLeaveStatuses(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.apiRoute}/leave-statuses`);
  }

  createOvertime(overtime: Overtime): Observable<Overtime> {
    return this.httpClient.post<Overtime>(
      `${this.apiRoute}/overtime`,
      overtime
    );
  }

  getOvertimeStatuses(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.apiRoute}/overtime-statuses`);
  }

  createOfficialBusiness(ob: OfficialBusiness): Observable<OfficialBusiness> {
    return this.httpClient.post<OfficialBusiness>(
      `${this.apiRoute}/official-business`,
      ob
    );
  }

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
