import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/accounts/services/account.service';
import { NgxPermissionsService } from 'ngx-permissions';
import { Role } from 'src/app/roles/shared/role';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent implements OnInit {
  constructor(
    private accountService: AccountService,
    private permissionsService: NgxPermissionsService
  ) {}

  ngOnInit() {
    this.accountService.getCurrentRole().subscribe((role) => {
      if (role == null) {
        return;
      }

      this.addPermissions(role);
    });
  }

  private addPermissions(role: Role) {
    for (const rolePermission of role.rolePermissions) {
      if (rolePermission.read) {
        this.permissionsService.addPermission(
          `${rolePermission.permissionName}:read`
        );
      }

      if (rolePermission.create) {
        this.permissionsService.addPermission(
          `${rolePermission.permissionName}:create`
        );
      }

      if (rolePermission.update) {
        this.permissionsService.addPermission(
          `${rolePermission.permissionName}:update`
        );
      }

      if (rolePermission.delete) {
        this.permissionsService.addPermission(
          `${rolePermission.permissionName}:delete`
        );
      }
    }
  }
}
