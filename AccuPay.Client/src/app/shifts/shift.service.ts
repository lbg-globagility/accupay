import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Shift } from 'src/app/shifts/shared/shift';
import { BasePdfService } from '../core/shared/services/base-pdf-service';
import { ShiftsByEmployeePageOptions } from './shared/shifts-by-employee-page-option';
import { EmployeeShifts } from './shared/employee-shifts';

@Injectable({
  providedIn: 'root',
})
export class ShiftService extends BasePdfService {
  baseUrl = 'api/shifts';

  readonly shiftTemplateFileName = 'accupay-shiftschedule-template';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  listByEmployee(
    options: ShiftsByEmployeePageOptions
  ): Observable<PaginatedList<EmployeeShifts>> {
    const params = options ? options.toObject() : null;

    return this.httpClient.get<PaginatedList<EmployeeShifts>>(
      `${this.baseUrl}/employees`,
      {
        params,
      }
    );
  }

  batchApply(shifts: any[]): Observable<void> {
    return this.httpClient.put<void>(`${this.baseUrl}`, shifts);
  }

  import(file: File): Observable<Shift> {
    const formData = new FormData();
    formData.append('file', file);

    return this.httpClient.post<Shift>(`${this.baseUrl}/import`, formData);
  }

  getShifScheduleTemplate(): Promise<any> {
    return this.getFile(
      this.shiftTemplateFileName,
      `${this.baseUrl}/accupay-shiftschedule-template`
    );
  }
}
