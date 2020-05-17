import { AuthService } from 'src/app/core/auth/auth.service';
import { BehaviorSubject } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-timeout',
  templateUrl: './timeout.component.html',
  styleUrls: ['./timeout.component.scss']
})
export class TimeoutComponent implements OnInit {
  email: string;

  loggingIn: BehaviorSubject<boolean> = new BehaviorSubject(null);

  loginError: string;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  form: FormGroup = this.fb.group({
    password: [null, Validators.required]
  });

  ngOnInit() {
    this.getEmailAddress();
  }

  getEmailAddress(): void {
    this.authService.getAccount().subscribe(account => {
      this.email = account.email;
    });
  }

  login() {
    if (!this.form.valid) {
      return;
    }

    const { password } = this.form.value;

    this.loginError = null;
    this.loggingIn.next(true);

    this.authService.login(this.email, password).subscribe(
      () => {
        if (this.authService.redirectUrl != null) {
          const redirectUrl = decodeURI(this.authService.redirectUrl);

          this.authService.redirectUrl = null;

          this.router.navigateByUrl(redirectUrl);
        } else {
          this.router.navigate(['dashboard']);
        }
      },
      error => {
        this.loginError = error;
        this.loggingIn.next(false);
      }
    );
  }

  goToLogin() {
    return this.router.navigate(['login']);
  }
}
