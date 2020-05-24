import { Component, OnInit } from '@angular/core';
import { OrganizationService } from 'src/app/organizations/organization.service';
import { Organization } from 'src/app/organizations/shared/organization';
import { Router } from '@angular/router';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-organization',
  templateUrl: './new-organization.component.html',
  styleUrls: ['./new-organization.component.scss'],
})
export class NewOrganizationComponent implements OnInit {
  constructor(
    private organizationService: OrganizationService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {}

  onSave(organization: Organization) {
    this.organizationService.create(organization).subscribe(
      (o) => {
        this.router.navigate(['organizations', o.id]);
      },
      (err) =>
        this.errorHandler.badRequest(err, 'Failed to create organization.')
    );
  }

  onCancel() {
    this.router.navigate(['organizations']);
  }
}
