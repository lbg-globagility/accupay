import Swal from 'sweetalert2';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/core/auth/auth.service';
import { Component, OnInit } from '@angular/core';
import { LoadingState } from 'src/app/core/states/loading-state';

@Component({
  selector: 'app-oauth',
  templateUrl: './oauth.component.html',
  styleUrls: ['./oauth.component.scss']
})
export class OauthComponent implements OnInit {
  isLoading: LoadingState = new LoadingState();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    const token = this.readToken();

    if (token != null) {
      this.authService
        .loginWithCognito(token)
        .subscribe(() => this.letThrough(), err => this.showError(err));
    } else {
      this.showError('We did not receive a token from your portal');
    }
  }

  readToken() {
    const fragment = new URLSearchParams(this.route.snapshot.fragment);
    if (fragment.has('id_token')) {
      return fragment.get('id_token');
    }

    return null;
  }

  letThrough() {
    this.router.navigate(['/']);
  }

  showError(errorText: string) {
    this.router.navigate(['login']);

    Swal.fire({
      title: 'Error',
      text: errorText,
      showConfirmButton: true,
      icon: 'error'
    });
  }
}
