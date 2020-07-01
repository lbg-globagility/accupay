import { Injectable } from '@angular/core';
import { BasePdfService } from '../core/shared/services/base-pdf-service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ReportService extends BasePdfService {
  private baseUrl = 'api/reports';

  readonly sssFileName = 'sss-report.pdf';
  readonly philHealthFileName = 'philhealth-report.pdf';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  getSSSReport(month: number, year: number): Promise<any> {
    return this.getPDF(
      this.sssFileName,
      `${this.baseUrl}/sss-report/${month}/${year}`
    );
  }

  getPhilHealthReport(month: number, year: number): Promise<any> {
    return this.getPDF(
      this.philHealthFileName,
      `${this.baseUrl}/philhealth-report/${month}/${year}`
    );
  }
}
