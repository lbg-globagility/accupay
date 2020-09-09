import * as jwt_decode from 'jwt-decode';
import { Account } from 'src/app/accounts/shared/account';
import { catchError, map, tap, flatMap } from 'rxjs/operators';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { Role } from 'src/app/roles/shared/role';
import { NgxPermissionsService } from 'ngx-permissions';

const TOKEN_KEY = 'token';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = 'api/account';

  private redirectUrl: string;

  private currentUser$ = new BehaviorSubject<Account>(null);

  constructor(
    private httpClient: HttpClient,
    private permissionService: NgxPermissionsService
  ) {}

  get currentUser(): Account {
    return this.currentUser$.getValue();
  }

  login(email: string, password: string): Observable<any> {
    const credentials = { email, password };

    return this.httpClient
      .post<LoginResult>(`${this.baseUrl}/login`, credentials)
      .pipe(
        map(({ token }) => {
          if (token != null) {
            localStorage.setItem(TOKEN_KEY, token);
          }
        }),
        flatMap(() => this.getAccount()),
        flatMap(() => this.getCurrentRole()),
        catchError((response: HttpErrorResponse) => {
          if (400 <= response.status && response.status < 500) {
            return throwError('BadCredentials');
          } else {
            return throwError('Server error');
          }
        })
      );
  }

  changeOrganization(organizationId: number) {
    return this.httpClient
      .post<LoginResult>(`${this.baseUrl}/change-organization`, {
        organizationId,
      })
      .pipe(
        map(({ token }) => {
          if (token != null) {
            localStorage.setItem(TOKEN_KEY, token);
          }

          this.getAccount().subscribe();

          return null;
        }),
        catchError((response: HttpErrorResponse) => {
          if (400 <= response.status && response.status < 500) {
            return throwError('BadCredentials');
          } else {
            return throwError('Server error');
          }
        })
      );
  }

  logout(): void {
    this.currentUser$.next(null);

    localStorage.removeItem(TOKEN_KEY);
  }

  getAccount(): Observable<Account> {
    return this.httpClient
      .get<Account>(`${this.baseUrl}`)
      .pipe(tap((account) => this.currentUser$.next(account)));
  }

  getCurrentRole(): Observable<Role> {
    return this.httpClient
      .get<Role>(`${this.baseUrl}/current-role`)
      .pipe(tap((role) => this.loadPermissions(role)));
  }

  private getClaims(): UserClaims {
    const token = this.getToken();
    const jwt = jwt_decode(token);
    const loginClaim = this.mapTokenToClaims(jwt);

    return loginClaim;
  }

  hasAttemptedUrl(): boolean {
    return this.redirectUrl != null;
  }

  storeUrlAttempt(url: string): void {
    this.redirectUrl = url;
  }

  popUrlAttempt(): string {
    const redirectUrl = this.redirectUrl;
    this.redirectUrl = null;
    return redirectUrl;
  }

  isTokenValid(): boolean {
    const token = this.getToken();

    // If there's no token to check, then session is automatically invalid
    if (token == null) {
      return false;
    }

    const jwt = jwt_decode(token);
    const loginClaim = this.mapTokenToClaims(jwt);

    const currentTime = Date.now() / 1000;

    // If current time has exceeded the token's expiration, then the session is invalid
    return currentTime < loginClaim.expiration;
  }

  isAuthenticated(): boolean {
    return this.getToken() != null;
  }

  getToken(): string {
    return localStorage.getItem(TOKEN_KEY);
  }

  private mapTokenToClaims(claims): UserClaims {
    return {
      userId: claims['sub'],
      expiration: claims['exp'],
      employeeId: claims['employeeId'],
    };
  }

  private loadPermissions(role: Role): void {
    const permissions: string[] = [];

    for (const rolePermission of role.rolePermissions) {
      if (rolePermission.read) {
        permissions.push(`${rolePermission.permissionName}:read`);
      }

      if (rolePermission.create) {
        permissions.push(`${rolePermission.permissionName}:create`);
      }

      if (rolePermission.update) {
        permissions.push(`${rolePermission.permissionName}:update`);
      }

      if (rolePermission.delete) {
        permissions.push(`${rolePermission.permissionName}:delete`);
      }
    }

    this.permissionService.loadPermissions(permissions);
  }
}

export interface UserClaims {
  userId: string;
  employeeId: number;
  expiration: number;
}

interface LoginResult {
  token: string;
}
