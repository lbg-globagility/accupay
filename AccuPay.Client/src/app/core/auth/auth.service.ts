import * as jwt_decode from 'jwt-decode';
import { Account } from 'src/app/accounts/shared/account';
import { catchError, map } from 'rxjs/operators';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, ReplaySubject, throwError } from 'rxjs';

const TOKEN_KEY = 'token';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = 'api/account';

  redirectUrl: string;

  private accountSource$ = new ReplaySubject<Account>(1);

  public account$ = this.accountSource$.asObservable();

  constructor(private httpClient: HttpClient) {}

  login(email: string, password: string): Observable<string> {
    const credentials = { email, password };

    return this.httpClient
      .post<LoginResult>('/api/account/login', credentials)
      .pipe(
        map(({ token }) => {
          const hasToken = token != null;

          if (hasToken) {
            localStorage.setItem(TOKEN_KEY, token);
          }

          this.getAccount().subscribe((account) => {
            this.accountSource$.next(account);
          });

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

  changeOrganization(organizationId: number) {
    return this.httpClient
      .post<LoginResult>(`${this.baseUrl}/change-organization`, {
        organizationId,
      })
      .pipe(
        map(({ token }) => {
          const hasToken = token != null;

          if (hasToken) {
            localStorage.setItem(TOKEN_KEY, token);
          }

          this.getAccount().subscribe((account) => {
            this.accountSource$.next(account);
          });

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

  loginWithCognito(idToken: string) {
    let credentials = null;

    try {
      credentials = jwt_decode(idToken);
    } catch (err) {
      return throwError('Invalid format');
    }

    return this.httpClient
      .post<LoginResult>('/api/account/login/cognito', credentials)
      .pipe(
        map(({ token }) => {
          const hasToken = token != null;

          if (hasToken) {
            localStorage.setItem(TOKEN_KEY, token);
          }

          this.getAccount().subscribe((account) => {
            this.accountSource$.next(account);
          });

          return null;
        }),
        catchError((err: HttpErrorResponse) => {
          if (400 <= err.status && err.status < 500) {
            return throwError('Incorrect credentials');
          } else {
            return throwError('Server error');
          }
        })
      );
  }

  logout(): void {
    this.accountSource$.next(null);

    localStorage.removeItem(TOKEN_KEY);
  }

  getAccount(): Observable<Account> {
    return this.httpClient.get<Account>('/api/account');
  }

  getClaims(): UserClaims {
    const token = this.getToken();
    const jwt = jwt_decode(token);
    const loginClaim = this.mapTokenToClaims(jwt);

    return loginClaim;
  }

  storeUrlAttempt(url: string): void {
    this.redirectUrl = url;
  }

  getPermissions(): Observable<string[]> {
    const token = this.getToken();
    const payload = jwt_decode(token);
    const permissions = this.readPermissions(payload);

    return of(permissions);
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

  private readPermissions(payload: any): string[] {
    if (payload == null) {
      return [];
    }

    const permission = payload.permission;

    if (typeof permission === 'string') {
      return [permission];
    }

    if (Array.isArray(permission)) {
      return permission;
    }

    return [];
  }

  private mapTokenToClaims(claims): UserClaims {
    return {
      userId: claims['sub'],
      expiration: claims['exp'],
    };
  }
}

export interface UserClaims {
  userId: string;
  expiration: number;
}

interface LoginResult {
  token: string;
}
