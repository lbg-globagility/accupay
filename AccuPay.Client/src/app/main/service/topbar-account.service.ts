import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { TopbarAccount } from '../shared/topbar-account';

@Injectable({
  providedIn: 'root'
})
export class TopbarAccountService {
  apiUrl = 'api/account';

  private data = new Subject<any>();

  currentData$ = this.data.asObservable();

  constructor(private httpClient: HttpClient) {}

  get(): Observable<TopbarAccount> {
    return this.httpClient.get<TopbarAccount>(this.apiUrl);
  }

  getName(): Observable<TopbarAccountName> {
    return this.httpClient.get<TopbarAccountName>(this.apiUrl);
  }

  setData() {
    return this.data.next();
  }
}

interface TopbarAccountName {
  firstName: string;
  lastName: string;
}
