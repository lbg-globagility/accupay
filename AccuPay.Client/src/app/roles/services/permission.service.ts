import { Injectable } from '@angular/core';
import { Permission } from 'src/app/roles/shared/permission';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PermissionService {
  private baseUrl = 'api/permissions';

  constructor(private httpClient: HttpClient) {}

  getAll(): Observable<Permission[]> {
    return this.httpClient.get<Permission[]>(`${this.baseUrl}`);
  }
}
