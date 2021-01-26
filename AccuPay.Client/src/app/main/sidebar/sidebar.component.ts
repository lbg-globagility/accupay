import { Component, OnInit } from '@angular/core';
import { cloneDeep } from 'lodash';
import { NgxPermissionsService } from 'ngx-permissions';
import { AuthService } from 'src/app/core/auth';
import { selfServiceMenu } from 'src/app/main/sidebar/self-service-menu';
import { adminMenu } from 'src/app/main/sidebar/admin-menu';
import { MenuItem } from 'src/app/main/shared';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
})
export class SidebarComponent implements OnInit {
  menuItems: MenuItem[] = [];

  constructor(
    private authService: AuthService,
    private permissionService: NgxPermissionsService
  ) {}

  ngOnInit(): void {
    const currentUser = this.authService.currentUser;
    const menuItems =
      currentUser?.type === 'Employee' ? selfServiceMenu : adminMenu;

    this.menuItems = this.filterByPermission(cloneDeep(menuItems));
  }

  isLink(menuItem: MenuItem): boolean {
    return menuItem.items == null;
  }

  toggle(menuItem: MenuItem): void {
    this.menuItems
      .filter((t) => t !== menuItem)
      .forEach((t) => (t.toggled = false));

    menuItem.toggled = !menuItem.toggled;
  }

  private filterByPermission(items: MenuItem[]): MenuItem[] {
    return items.filter((item) => {
      // If the menu has submenus, filter out those.
      if (item.items?.length > 0) {
        item.items = this.filterByPermission(item.items);
      }

      // If the menu has no permission set they should appear, otherwise
      // check if the user has the required permission.
      if (item.permission == null) {
        return true;
      } else {
        return this.permissionService.getPermission(item.permission) != null;
      }
    });
  }
}
