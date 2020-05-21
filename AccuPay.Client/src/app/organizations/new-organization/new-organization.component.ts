import { Component, OnInit } from '@angular/core';
import { OrganizationService } from 'src/app/organizations/organization.service';
import { Organization } from 'src/app/organizations/shared/organization';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-organization',
  templateUrl: './new-organization.component.html',
  styleUrls: ['./new-organization.component.scss'],
})
export class NewOrganizationComponent implements OnInit {
  constructor(
    private organizationService: OrganizationService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  onSave(organization: Organization) {
    this.organizationService.create(organization).subscribe((o) => {
      this.router.navigate(['organizations', o.id]);
    });
  }

  onCancel() {
    this.router.navigate(['organizations']);
  }
}
