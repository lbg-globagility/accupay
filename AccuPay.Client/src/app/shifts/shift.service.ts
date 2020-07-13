import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Shift } from 'src/app/shifts/shared/shift';
import { BasePdfService } from '../core/shared/services/base-pdf-service';
import { ShiftsByEmployeePageOptions } from './shared/shifts-by-employee-page-option';

@Injectable({
  providedIn: 'root',
})
export class ShiftService extends BasePdfService {
  baseUrl = 'api/shifts';

  readonly shiftTemplateFileName = 'accupay-shiftschedule-template';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  getAll(options: PageOptions, term = ''): Observable<PaginatedList<Shift>> {
    const params = options ? options.toObject() : null;
    params.term = term;
    return this.httpClient.get<PaginatedList<Shift>>(`${this.baseUrl}`, {
      params,
    });
  }

  get(id: number): Observable<Shift> {
    return this.httpClient.get<Shift>(`${this.baseUrl}/${id}`);
  }

  create(shift: Shift): Observable<Shift> {
    return this.httpClient.post<Shift>(`${this.baseUrl}`, shift);
  }

  update(shift: Shift, id: number): Observable<Shift> {
    return this.httpClient.put<Shift>(`${this.baseUrl}/${id}`, shift);
  }

  delete(id: number): Observable<Shift> {
    return this.httpClient.delete<Shift>(`${this.baseUrl}/${id}`);
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

  listByEmployee(
    options: ShiftsByEmployeePageOptions
  ): Observable<PaginatedList<Shift>> {
    const params = options ? options.toObject() : null;

    return this.httpClient.get<PaginatedList<Shift>>(
      `${this.baseUrl}/employees`,
      {
        params,
      }
    );
  }

  updateMany(shifts: any[]): Observable<void> {
    return this.httpClient.put<void>(`${this.baseUrl}`, shifts);
  }
}
