import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/auth/auth.service';
import { Router } from '@angular/router';
import { OrganizationService } from 'src/app/organizations/organization.service';
import { Organization } from 'src/app/organizations/shared/organization';
import { AccountService } from 'src/app/accounts/services/account.service';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss'],
})
export class TopbarComponent implements OnInit {
  @Output()
  toggleMenu = new EventEmitter<void>();

  currentOrganization: Organization;

  organizations: Organization[];

  public constructor(
    private authService: AuthService,
    private accountService: AccountService,
    private organizationService: OrganizationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.getCurrentUserOrganization();
    this.loadOrganizations();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['login']);
  }

  loadOrganizations() {
    this.organizationService.getAll().subscribe((organizations) => {
      this.organizations = organizations;
    });
  }

  getCurrentUserOrganization() {
    this.accountService.getOrganization().subscribe((organization) => {
      this.currentOrganization = organization;
    });
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
