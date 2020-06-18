import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoanTransaction } from '../shared/loan-transaction';

@Injectable({
  providedIn: 'root',
})
export class PaystubService {
  private baseUrl = 'api/paystubs';

  constructor(private httpClient: HttpClient) {}

  getLoanTransactions(paystubId: Number): Observable<LoanTransaction[]> {
    return this.httpClient.get<LoanTransaction[]>(
      `${this.baseUrl}/${paystubId}/transactions`
    );
  }
}
