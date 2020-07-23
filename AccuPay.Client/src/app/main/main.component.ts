import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/accounts/services/account.service';
import { NgxPermissionsService } from 'ngx-permissions';
import { Role } from 'src/app/roles/shared/role';
import { LoadingState } from '../core/states/loading-state';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent implements OnInit {
  state: LoadingState = new LoadingState();

  constructor(
    private accountService: AccountService,
    private permissionsService: NgxPermissionsService
  ) {}

  ngOnInit(): void {
    this.loadCurrentRole();
  }

  private loadCurrentRole(): void {
    this.accountService.getCurrentRole().subscribe((role) => {
      if (role == null) {
        return;
      }

      this.addPermissions(role);
      this.state.changeToSuccess();
    });
  }

  private addPermissions(role: Role): void {
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
