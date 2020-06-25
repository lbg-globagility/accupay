import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClientService } from 'src/app/clients/services/client.service';
import { Client } from 'src/app/clients/shared/client';
import { OrganizationService } from 'src/app/organizations/organization.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { OrganizationPageOptions } from 'src/app/organizations/shared/organization-page-options';
import { Organization } from 'src/app/organizations/shared/organization';

@Component({
  selector: 'app-view-client',
  templateUrl: './view-client.component.html',
  styleUrls: ['./view-client.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class ViewClientComponent implements OnInit {
  private clientId: number;

  client: Client;

  organizations: Organization[] = [];

  readonly displayedColumns: string[] = ['name'];

  constructor(
    private clientService: ClientService,
    private organizationService: OrganizationService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.clientId = +paramMap.get('id');
      this.loadClient();
      this.loadOrganizations();
    });
  }

  private loadClient() {
    this.clientService
      .getById(this.clientId)
      .subscribe((client) => (this.client = client));
  }

  private loadOrganizations() {
    const options = new OrganizationPageOptions(0, 25, this.clientId);

    this.organizationService.list(options).subscribe((data) => {
      this.organizations = data.items;
    });
  }
}
