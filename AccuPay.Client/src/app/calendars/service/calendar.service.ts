import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Calendar } from 'src/app/calendars/shared/calendar';
import { CalendarDay } from 'src/app/calendars/shared/calendar-day';
import { DayType } from 'src/app/calendars/shared/day-type';

@Injectable({
  providedIn: 'root',
})
export class CalendarService {
  private baseUrl = 'api/calendars';

  constructor(private httpClient: HttpClient) {}

  getById(id: number): Observable<Calendar> {
    return this.httpClient.get<Calendar>(`${this.baseUrl}/${id}`);
  }

  getDays(id: number, year: number): Observable<CalendarDay[]> {
    const params = { year: year.toString() };

    return this.httpClient.get<CalendarDay[]>(`${this.baseUrl}/${id}/days`, {
      params,
    });
  }

  getDayTypes(): Observable<DayType[]> {
    return this.httpClient.get<DayType[]>(`${this.baseUrl}/day-types`);
  }

  create(calendar: Calendar): Observable<Calendar> {
    return this.httpClient.post<Calendar>(`${this.baseUrl}`, calendar);
  }

  update(id: number, calendar: Calendar): Observable<Calendar> {
    return this.httpClient.put<Calendar>(`${this.baseUrl}/${id}`, calendar);
  }

  updateDays(
    calendarId: number,
    calendarDays: CalendarDay[]
  ): Observable<void> {
    return this.httpClient.put<void>(
      `${this.baseUrl}/${calendarId}/days`,
      calendarDays
    );
  }

  list(): Observable<Calendar[]> {
    return this.httpClient.get<Calendar[]>(`${this.baseUrl}`);
  }
}
