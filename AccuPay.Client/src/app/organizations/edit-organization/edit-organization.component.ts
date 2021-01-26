import { Component, OnInit } from '@angular/core';
import { OrganizationService } from 'src/app/organizations/organization.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Organization } from 'src/app/organizations/shared/organization';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-edit-organization',
  templateUrl: './edit-organization.component.html',
  styleUrls: ['./edit-organization.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class EditOrganizationComponent implements OnInit {
  private organizationId: number = +this.route.snapshot.paramMap.get('id');

  organization: Organization;

  constructor(
    private organizationService: OrganizationService,
    private router: Router,
    private route: ActivatedRoute,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.organizationService
      .getById(this.organizationId)
      .subscribe((o) => (this.organization = o));
  }

  onSave(organization: Organization) {
    this.organizationService
      .update(this.organizationId, organization)
      .subscribe(
        () => {
          this.router.navigate(['organizations', this.organizationId]);
        },
        (err) =>
          this.errorHandler.badRequest(err, 'Failed to update organization.')
      );
  }

  onCancel() {
    this.router.navigate(['organizations', this.organizationId]);
  }
}
