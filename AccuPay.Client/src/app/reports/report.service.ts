import { Injectable } from '@angular/core';
import { BasePdfService } from '../core/shared/services/base-pdf-service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ReportService extends BasePdfService {
  private baseUrl = 'api/reports';

  readonly payrollSummaryFileName = 'payroll-summary.xlsx';
  readonly pagIBIGFileName = 'pagibig-report.pdf';
  readonly philHealthFileName = 'philhealth-report.pdf';
  readonly sssFileName = 'sss-report.pdf';
  readonly taxFileName = 'tax-report.pdf';
  readonly thirteenthMonthFileName = 'thirteenth-month-report.pdf';
  readonly loanByTypeFileName = 'loan-by-type-report.pdf';
  readonly loanByEmployeeFileName = 'loan-by-employee-report.pdf';

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  getPayrollSummary(
    payPeriodFromId: number,
    payPeriodToId: number,
    keepInOneSheet: boolean = true,
    hideEmptyColumns: boolean = true,
    salaryDistributionType: string = 'All',
    isActual: boolean = false
  ) {
    var params = `?keepInOneSheet=${keepInOneSheet}&hideEmptyColumns=${hideEmptyColumns}&salaryDistributionType=${salaryDistributionType}&isActual=${isActual}`;
    // var params = '';

    return this.getFile(
      this.payrollSummaryFileName,
      `${this.baseUrl}/payroll-summary/${payPeriodFromId}/${payPeriodToId}` +
        params
    );
  }

  getPagIBIGReport(month: number, year: number) {
    return this.getFile(
      this.pagIBIGFileName,
      `${this.baseUrl}/pagibig-report/${month}/${year}`
    );
  }

  getPhilHealthReport(month: number, year: number): Promise<any> {
    return this.getFile(
      this.philHealthFileName,
      `${this.baseUrl}/philhealth-report/${month}/${year}`
    );
  }

  getSSSReport(month: number, year: number): Promise<any> {
    return this.getFile(
      this.sssFileName,
      `${this.baseUrl}/sss-report/${month}/${year}`
    );
  }

  getTaxReport(month: number, year: number) {
    return this.getFile(
      this.taxFileName,
      `${this.baseUrl}/tax-report/${month}/${year}`
    );
  }

  get13thMonthReport(dateFrom: string, dateTo: string) {
    return this.getFile(
      this.thirteenthMonthFileName,
      `${this.baseUrl}/tax-report/${dateFrom}/${dateTo}`
    );
  }

  getLoanByTypeReport(
    dateFrom: string,
    dateTo: string,
    isPerPage: boolean
  ): Promise<any> {
    return this.getFile(
      this.loanByTypeFileName,
      `${this.baseUrl}/loanbytype-report/${dateFrom}/${dateTo}/${isPerPage}`
    );
  }

  getLoanByEmployeeReport(dateFrom: string, dateTo: string): Promise<any> {
    return this.getFile(
      this.loanByEmployeeFileName,
      `${this.baseUrl}/loanbyemployee-report/${dateFrom}/${dateTo}`
    );
  }
}
