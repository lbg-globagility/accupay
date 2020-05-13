import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss'],
})
export class TopbarComponent {
  @Output()
  toggleMenu = new EventEmitter<void>();

  logout(): void {}

  onToggleMenu(): void {
    this.toggleMenu.emit();
  }
}
