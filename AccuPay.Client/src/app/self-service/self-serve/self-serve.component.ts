import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SelfserveTimeEntryComponent } from '../pages/selfserve-time-entry/selfserve-time-entry.component';
import { cloneDeep } from 'lodash';
import { NgxPermissionsService } from 'ngx-permissions';
import { SelfserviceNewLeaveComponent } from 'src/app/self-service/leaves/components';
import { SelfserviceNewOvertimeComponent } from 'src/app/self-service/overtimes/components';
import { SelfserviceNewOfficialBusinessComponent } from 'src/app/self-service/official-businesses/components';

enum SelfserveCommand {
  FileLeave = 0,
  FileOveretime = 1,
  FileOfficialBusiness = 2,
  ViewTimeEntry = 3,
}

interface MenuItem {
  label: string;
  icon?: string;
  permission?: string;
  command: SelfserveCommand;
  items?: MenuItem[];
}

const menuItems: MenuItem[] = [
  {
    label: 'File a Leave',
    icon: '',
    // permission: PermissionTypes.SelfserveLeaveCreate,
    command: SelfserveCommand.FileLeave,
  },
  {
    label: 'File an Overtime',
    icon: '',
    // permission: PermissionTypes.SelfserveLeaveCreate,
    command: SelfserveCommand.FileOveretime,
  },
  {
    label: 'File an Official business',
    icon: '',
    // permission: PermissionTypes.SelfserveLeaveCreate,
    command: SelfserveCommand.FileOfficialBusiness,
  },
  {
    label: 'View Timesheet',
    icon: '',
    // permission: PermissionTypes.SelfserveLeaveRead,
    command: SelfserveCommand.ViewTimeEntry,
  },
];

@Component({
  selector: 'app-self-serve',
  templateUrl: './self-serve.component.html',
  styleUrls: ['./self-serve.component.scss'],
})
export class SelfServeComponent implements OnInit {
  menuItems: MenuItem[];

  constructor(
    private permissionService: NgxPermissionsService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.menuItems = this.filterByPermission(cloneDeep(menuItems));
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

  executeCommand(command: SelfserveCommand) {
    switch (command) {
      case 0:
        this.createLeave();
        return;
      case 1:
        this.createOvertime();
        return;
      case 2:
        this.createOfficialBusiness();
        return;
      case 3:
        this.viewMyTimeEntry();
        return;
    }
  }

  createLeave() {
    this.dialog
      .open(SelfserviceNewLeaveComponent)
      .afterClosed()
      .subscribe(() => {});
  }

  createOvertime() {
    this.dialog
      .open(SelfserviceNewOvertimeComponent)
      .afterClosed()
      .subscribe(() => {});
  }

  createOfficialBusiness() {
    this.dialog
      .open(SelfserviceNewOfficialBusinessComponent)
      .afterClosed()
      .subscribe(() => {});
  }

  viewMyTimeEntry() {
    this.dialog
      .open(SelfserveTimeEntryComponent)
      .afterClosed()
      .subscribe(() => {});
  }

  logout(): void {}
}
