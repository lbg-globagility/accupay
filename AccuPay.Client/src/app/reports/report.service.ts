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
  readonly pagIBIGFileName = 'pagibig-report.pdf';
  readonly loanByTypeFileName = 'loan-by-type-report.pdf';

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

  getPagIBIGReport(month: number, year: number) {
    return this.getPDF(
      this.pagIBIGFileName,
      `${this.baseUrl}/pagibig-report/${month}/${year}`
    );
  }
  getLoanByTypeReport(
    monthFrom: number,
    dayFrom: number,
    yearFrom: number,
    monthTo: number,
    dayTo: number,
    yearTo: number,
    isPerPage: any
  ): Promise<any> {
    return this.getPDF(
      this.pagIBIGFileName,
      `${this.baseUrl}/loanbytype-report/${monthFrom}/${dayFrom}/${yearFrom}/${monthTo}/${dayTo}/${yearTo}/${isPerPage}`
    );
  }
}
