import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Calendar } from 'src/app/calendars/shared/calendar';

@Injectable({
  providedIn: 'root',
})
export class CalendarService {
  private baseUrl = 'api/calendars';

  constructor(private httpClient: HttpClient) {}

  getById(id: number): Observable<Calendar> {
    return this.httpClient.get<Calendar>(`${this.baseUrl}/${id}`);
  }

  create(calendar: Calendar): Observable<Calendar> {
    return this.httpClient.post<Calendar>(`${this.baseUrl}`, calendar);
  }

  update(id: number, calendar: Calendar): Observable<Calendar> {
    return this.httpClient.put<Calendar>(`${this.baseUrl}/${id}`, calendar);
  }

  list(): Observable<Calendar[]> {
    return this.httpClient.get<Calendar[]>(`${this.baseUrl}`);
  }
}
