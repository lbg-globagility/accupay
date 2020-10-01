import { AuthService } from 'src/app/core/auth/auth.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { NgxPermissionsService } from 'ngx-permissions';
import { catchError, map } from 'rxjs/operators';
import { Role } from 'src/app/roles/shared/role';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loggingIn: BehaviorSubject<boolean> = new BehaviorSubject(null);

  loginError: string;

  form: FormGroup = this.fb.group({
    email: [],
    password: [],
  });

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private accountService: AccountService,
    private permissionService: NgxPermissionsService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  forgotPassword(): void {
    this.router.navigate(['account', 'forgot-password']);
  }

  login(): void {
    if (!this.form.valid) {
      return;
    }

    const { email, password } = this.form.value;

    this.loginError = null;
    this.loggingIn.next(true);

    this.authService.login(email, password).subscribe(
      () => {
        if (this.authService.hasAttemptedUrl()) {
          const attemptedUrl = this.authService.popUrlAttempt();
          this.router.navigateByUrl(attemptedUrl);
        } else {
          const currentUser = this.authService.currentUser;

          if (currentUser.type === 'Admin') {
            this.router.navigate(['']);
          } else {
            this.router.navigate(['self-service']);
          }
        }
      },
      (error) => {
        this.loginError = error;
        this.loggingIn.next(false);
      }
    );
  }
}
