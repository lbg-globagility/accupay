import { Component, OnInit } from '@angular/core';
import { ClientService } from 'src/app/clients/services/client.service';
import { PageEvent } from '@angular/material/paginator';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Client } from 'src/app/clients/shared/client';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss'],
  host: {
    class: 'block h-full overflow-hidden',
  },
})
export class ClientsComponent implements OnInit {
  pageIndex: number = 0;

  pageSize: number = 25;

  clients: Client[] = [];

  totalCount: number = 0;

  selectedClients: Client[] = [];

  constructor(private clientService: ClientService, private router: Router) {}

  ngOnInit(): void {
    this.loadClients();
  }

  page(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadClients();
  }

  selectionChange() {
    const client =
      this.selectedClients.length > 0 ? this.selectedClients[0] : null;

    this.router.navigate(['clients', client.id]);
  }

  loadClients() {
    const options = new PageOptions(this.pageIndex, this.pageSize);
    this.clientService.list(options).subscribe((data) => {
      this.clients = data.items;
      this.totalCount = data.totalCount;

      const firstClient = this.clients[0];
      this.selectedClients = [firstClient];
      this.router.navigate(['clients', firstClient.id]);
    });
  }
}
