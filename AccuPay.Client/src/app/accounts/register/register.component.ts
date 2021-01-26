import Swal from 'sweetalert2';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { CustomValidators } from 'src/app/core/forms/validators';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoadingState } from 'src/app/core/states/loading-state';
import { RegistrationClaims } from 'src/app/accounts/shared/registration-claims';
import { RegistrationService } from 'src/app/accounts/services/registration.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  registered: BehaviorSubject<boolean> = new BehaviorSubject(null);

  loadingState: LoadingState = new LoadingState();

  claim: RegistrationClaims;

  usernameAvailable = false;

  token: string;

  error: string;

  form: FormGroup = this.fb.group({
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
    email: [null, [Validators.required, Validators.email]],
    password: [null, [Validators.required, Validators.minLength(8)]],
    confirmPassword: [
      null,
      [Validators.required, CustomValidators.equalTo('password')],
    ],
    token: [null],
  });

  constructor(
    private fb: FormBuilder,
    private registrationService: RegistrationService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    try {
      this.readToken();
      this.readClaims();
      this.checkExpiration();

      this.validateToServer().subscribe(
        () => this.startForm(),
        () => this.showError('Registration link has been rejected')
      );
    } catch (error) {
      if (error instanceof Error) {
        this.showError(error.message);
      } else {
        this.showError('Unexpected error');
      }
    }
  }

  private readToken() {
    this.token = this.route.snapshot.queryParamMap.get('token');

    if (this.token == null) {
      throw new Error('Registration token is missing');
    }
  }

  private readClaims() {
    try {
      this.claim = this.registrationService.mapTokenToClaims(this.token);
    } catch (error) {
      throw new Error('Registration link is invalid');
    }
  }

  private checkExpiration() {
    if (this.registrationService.isExpired(this.claim)) {
      throw new Error('Registration link has expired');
    }
  }

  private startForm() {
    this.loadingState.changeToSuccess();

    this.form.patchValue({
      firstName: this.claim.firstName,
      lastName: this.claim.lastName,
      email: this.claim.email,
    });
  }

  private validateToServer(): Observable<any> {
    return this.registrationService.verify(this.token);
  }

  checkUsernameAvailability() {
    const { userName } = this.form.value;

    this.registrationService.getUser(userName).subscribe(
      () => (this.usernameAvailable = true),
      () => (this.usernameAvailable = false)
    );
  }

  register() {
    // Trigger confirm password manually
    this.form.get('confirmPassword').updateValueAndValidity();

    if (!this.form.valid) {
      return;
    }

    const value = this.form.value;
    value.token = this.token;

    this.registered.next(true);

    this.registrationService.create(value).subscribe(() => {
      this.showSuccessDialog();
      this.router.navigate(['login']);
    });
  }

  private showSuccessDialog() {
    Swal.fire({
      title: 'Registered',
      text: 'Successfully registered!',
      icon: 'success',
      backdrop: '#01579b',
      showConfirmButton: true,
    });
  }

  private showError(text: string) {
    this.loadingState.changeToError();

    this.error = text;
  }
}
