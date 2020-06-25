import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { Client } from 'src/app/clients/shared/client';
import { PageOptions } from 'src/app/core/shared/page-options';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  private baseUrl = 'api/clients';

  constructor(private httpClient: HttpClient) {}

  list(options: PageOptions): Observable<PaginatedList<Client>> {
    const params = options.toObject();
    return this.httpClient.get<PaginatedList<Client>>(`${this.baseUrl}`, {
      params,
    });
  }

  getById(clientId: number): Observable<Client> {
    return this.httpClient.get<Client>(`${this.baseUrl}/${clientId}`);
  }

  create(client: Client): Observable<Client> {
    return this.httpClient.post<Client>(`${this.baseUrl}`, client);
  }

  update(clientId: number, client: Client): Observable<Client> {
    return this.httpClient.put<Client>(`${this.baseUrl}/${clientId}`, client);
  }
}
