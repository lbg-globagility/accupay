import { Component, OnInit } from '@angular/core';
import { LoadingState } from 'src/app/core/states/loading-state';
import { Organization } from 'src/app/organizations/shared/organization';
import { OrganizationService } from 'src/app/organizations/organization.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-view-organization',
  templateUrl: './view-organization.component.html',
  styleUrls: ['./view-organization.component.scss'],
})
export class ViewOrganizationComponent implements OnInit {
  state: LoadingState = new LoadingState();

  organization: Organization;

  organizationId = +this.route.snapshot.paramMap.get('id');

  constructor(
    private organizationService: OrganizationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.organizationService
      .getById(this.organizationId)
      .subscribe((organization) => {
        this.organization = organization;
        this.state.changeToSuccess();
      });
  }
}
