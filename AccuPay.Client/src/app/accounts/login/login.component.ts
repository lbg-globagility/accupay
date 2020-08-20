import { AuthService } from 'src/app/core/auth/auth.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { NgxPermissionsService } from 'ngx-permissions';
import { catchError, map } from 'rxjs/operators';

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
        this.loadRole().subscribe(() => {
          if (this.authService.redirectUrl != null) {
            const attemptedUrl = this.authService.redirectUrl;
            this.authService.redirectUrl = null;
            this.router.navigateByUrl(attemptedUrl);
          } else {
            this.router.navigate(['']);
          }
        });
      },
      (error) => {
        this.loginError = error;
        this.loggingIn.next(false);
      }
    );
  }

  private loadRole(): Observable<void> {
    return this.accountService.getCurrentRole().pipe(
      map((role) => this.accountService.getPermissions(role)),
      map((permissions) => this.permissionService.loadPermissions(permissions))
    );
  }
}
