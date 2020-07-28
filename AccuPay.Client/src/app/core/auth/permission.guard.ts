import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  CanActivateChild,
  Router,
} from '@angular/router';
import { NgxPermissionsService } from 'ngx-permissions';

@Injectable({
  providedIn: 'root',
})
/**
 * Protects routes from unauthorized access by checking the permissions
 * of the current user
 */
export class PermissionGuard implements CanActivateChild {
  constructor(
    private permissionService: NgxPermissionsService,
    private router: Router
  ) {}

  async canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    _: RouterStateSnapshot
  ): Promise<boolean | UrlTree> {
    const permission = childRoute.data.permission;

    // If the route has no permission defined, then make the route accessible
    // to everyone.
    if (permission == null) {
      return Promise.resolve(true);
    }

    const hasPermission = await this.permissionService.hasPermission(
      permission
    );

    // If the user doesn't have the permission, route them to the "not authorized" page.
    if (!hasPermission) {
      return this.router.createUrlTree(['not-authorized']);
    } else {
      return hasPermission;
    }
  }
}
