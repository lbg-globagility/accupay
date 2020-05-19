import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from 'src/app/core/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss'],
})
export class TopbarComponent {
  @Output()
  toggleMenu = new EventEmitter<void>();

  public constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  logout(): void {
    this.authService.logout();
    this.router.navigate(['login']);
  }

  onToggleMenu(): void {
    this.toggleMenu.emit();
  }
}
