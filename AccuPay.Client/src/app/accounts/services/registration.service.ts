import * as jwt_decode from 'jwt-decode';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegistrationClaims } from 'src/app/accounts/shared/registration-claims';

@Injectable({
  providedIn: 'root',
})
export class RegistrationService {
  apiUrl = 'api/account';

  constructor(private httpClient: HttpClient) {}

  mapTokenToClaims(token: string): RegistrationClaims {
    try {
      const payload = jwt_decode(token);

      const claims: RegistrationClaims = {
        userId: payload['sub'],
        email: payload['email'],
        firstName: payload['given_name'],
        lastName: payload['family_name'],
        role:
          payload[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ],
        companyName: payload['company_name'],
        expiration: payload['exp'],
      };

      return claims;
    } catch (error) {
      throw new Error('Invalid token');
    }
  }

  isExpired(claims: RegistrationClaims): boolean {
    const expirationTime = claims.expiration;
    const currentTime = Number(Math.floor(Date.now() / 1000));

    return expirationTime < currentTime;
  }

  getUser(userName: string) {
    return this.httpClient.get(`${this.apiUrl}/availability/${userName}`);
  }

  /**
   * Verify if the registration token is valid or not.
   *
   * @param token the registration token
   */
  verify(token: string) {
    return this.httpClient.get(`${this.apiUrl}/verify?token=${token}`);
  }

  create(account: Account) {
    return this.httpClient.post(`${this.apiUrl}/register`, account);
  }
}
