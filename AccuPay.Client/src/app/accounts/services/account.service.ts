import { Account } from '../shared/account';
import { FileProgress } from 'src/app/files/shared/file-progress';
import {
  HttpClient,
  HttpEvent,
  HttpEventType,
  HttpRequest,
  HttpResponse
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, share } from 'rxjs/operators';
import { Observable, ReplaySubject } from 'rxjs';
import { Organization } from 'src/app/accounts/shared/organization';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private api = 'api/account';

  constructor(private httpClient: HttpClient) {}

  get(): Observable<Account> {
    return this.httpClient.get<Account>(this.api);
  }

  getOrganization(): Observable<Organization> {
    return this.httpClient.get<Organization>(`${this.api}/organization`);
  }

  updateOrganization(organization: Organization): Observable<Organization> {
    return this.httpClient.post<Organization>(
      `${this.api}/organization`,
      organization
    );
  }

  update(account: Account) {
    return this.httpClient.post(`${this.api}/edit`, account);
  }

  upload(file: File, id: string) {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('id', id);

    const request = new HttpRequest('POST', `/api/account/image`, formData, {
      reportProgress: true
    });

    return this.httpClient
      .request(request)
      .pipe(map(this.handleProgress), share());
  }

  uploadOrganization(file: File, id: string) {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('id', id);

    const request = new HttpRequest(
      'POST',
      `/api/account/organization/image`,
      formData,
      {
        reportProgress: true
      }
    );

    return this.httpClient
      .request(request)
      .pipe(map(this.handleProgressOrganization), share());
  }

  updateOrganizationDomains(
    organization: Organization
  ): Observable<Organization> {
    return this.httpClient.post<Organization>(
      `${this.api}/organization/domains`,
      organization
    );
  }

  updateOrganizationAcknowledgments(
    organization: Organization
  ): Observable<Organization> {
    return this.httpClient.post<Organization>(
      `${this.api}/organization/acknowledgment-thresholds`,
      organization
    );
  }

  updateOrganizationCompletionThresholds(
    organization: Organization
  ): Observable<Organization> {
    return this.httpClient.post<Organization>(
      `${this.api}/organization/completion-thresholds`,
      organization
    );
  }

  private handleProgress(
    event: HttpEvent<any> | HttpResponse<any>
  ): FileProgress<Account> {
    switch (event.type) {
      case HttpEventType.Sent:
        return new FileProgress('Sending', 0);
      case HttpEventType.UploadProgress:
        return new FileProgress(
          'In Progress',
          Math.round((100 * event.loaded) / event.total)
        );
      case HttpEventType.Response:
        const response = <HttpResponse<Account>>event;

        return new FileProgress('Success', 100, response.body);
      default:
        return new FileProgress('Receiving', 100);
    }
  }

  private handleProgressOrganization(
    event: HttpEvent<any> | HttpResponse<any>
  ): FileProgress<Organization> {
    switch (event.type) {
      case HttpEventType.Sent:
        return new FileProgress('Sending', 0);
      case HttpEventType.UploadProgress:
        return new FileProgress(
          'In Progress',
          Math.round((100 * event.loaded) / event.total)
        );
      case HttpEventType.Response:
        const response = <HttpResponse<Organization>>event;

        return new FileProgress('Success', 100, response.body);
      default:
        return new FileProgress('Receiving', 100);
    }
  }
}
