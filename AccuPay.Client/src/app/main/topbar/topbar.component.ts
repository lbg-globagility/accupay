import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/auth/auth.service';
import { Router } from '@angular/router';
import { OrganizationService } from 'src/app/organizations/organization.service';
import { Organization } from 'src/app/organizations/shared/organization';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss'],
})
export class TopbarComponent implements OnInit {
  @Output()
  toggleMenu = new EventEmitter<void>();

  organizations: Organization[];

  public constructor(
    private authService: AuthService,
    private organizationService: OrganizationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadOrganizations();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['login']);
  }

  loadOrganizations() {
    this.organizationService
      .getAll()
      .subscribe((organizations) => (this.organizations = organizations));
  }

  onToggleMenu(): void {
    this.toggleMenu.emit();
  }

  changeOrganization(organization: Organization) {
    this.authService.changeOrganization(organization.id).subscribe(() => {
      window.location.reload();
    });
  }
}
