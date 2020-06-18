import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoanTransaction } from '../shared/loan-transaction';
import { Adjustment } from '../shared/adjustment';

@Injectable({
  providedIn: 'root',
})
export class PaystubService {
  private baseUrl = 'api/paystubs';

  constructor(private httpClient: HttpClient) {}

  GetAdjustments(paystubId: Number): Observable<Adjustment[]> {
    return this.httpClient.get<Adjustment[]>(
      `${this.baseUrl}/${paystubId}/adjustments`
    );
  }

  getLoanTransactions(paystubId: Number): Observable<LoanTransaction[]> {
    return this.httpClient.get<LoanTransaction[]>(
      `${this.baseUrl}/${paystubId}/loans`
    );
  }
}
