import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  CanActivateChild,
} from '@angular/router';
import { Observable } from 'rxjs';
import { NgxPermissionsService } from 'ngx-permissions';

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements CanActivate, CanActivateChild {
  constructor(private permissionService: NgxPermissionsService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | UrlTree {
    return this.checkPermission(route, state);
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | UrlTree {
    return this.checkPermission(childRoute, state);
  }

  private checkPermission(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | UrlTree {
    const permission = route.data.permission;

    if (permission != null) {
      return this.permissionService.hasPermission(permission);
    } else {
      return true;
    }
  }
}
