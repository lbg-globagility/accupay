import { Component, OnInit } from '@angular/core';
import { cloneDeep } from 'lodash';
import { NgxPermissionsService } from 'ngx-permissions';
import { PermissionTypes } from 'src/app/core/auth';

interface MenuItem {
  label: string;
  route?: string;
  icon?: string;
  items?: MenuItem[];
  toggled?: boolean;
  permission?: string;
}

const menuItems: MenuItem[] = [
  {
    label: 'HR',
    icon: 'supervisor_account',
    items: [
      {
        label: 'Employees',
        route: '/employees',
        permission: PermissionTypes.EmployeeRead,
      },
      {
        label: 'Salaries',
        route: '/salaries',
        icon: 'person',
        permission: PermissionTypes.SalaryRead,
      },
      {
        label: 'Allowances',
        route: '/allowances',
        icon: 'person',
        permission: PermissionTypes.AllowanceRead,
      },
      {
        label: 'Loans',
        route: '/loans',
        icon: 'person',
        permission: PermissionTypes.LoanRead,
      },
      {
        label: 'Positions',
        route: '/positions',
        permission: PermissionTypes.PositionRead,
      },
      {
        label: 'Policies',
        route: '/employment-policies',
        permission: PermissionTypes.EmploymentPolicyRead,
      },
    ],
  },
  {
    label: 'Timekeeping',
    icon: 'access_time',
    items: [
      {
        label: 'Timesheets',
        route: '/time-entry',
        permission: PermissionTypes.TimeEntryRead,
      },
      {
        label: 'Shifts',
        route: '/shifts',
        permission: PermissionTypes.ShiftRead,
      },
      {
        label: 'Time Logs',
        route: '/time-logs',
        permission: PermissionTypes.TimeLogRead,
      },
      {
        label: 'Overtimes',
        route: '/overtimes',
        permission: PermissionTypes.OvertimeRead,
      },
      {
        label: 'Leaves',
        route: '/leaves',
        permission: PermissionTypes.LeaveRead,
      },
      {
        label: 'Official Business',
        route: '/official-businesses',
        permission: PermissionTypes.OfficialBusinessRead,
      },
    ],
  },
  {
    label: 'Payroll',
    icon: 'payments',
    route: '/payroll',
    permission: PermissionTypes.PayPeriodRead,
  },
  {
    label: 'Reports',
    icon: 'insert_chart_outlined',
    route: '/reports',
  },
  {
    label: 'Security',
    icon: 'security',
    route: '/security',
  },
  {
    label: 'Settings',
    icon: 'settings',
    items: [
      {
        label: 'Organizations',
        route: '/organizations',
        permission: PermissionTypes.OrganizationRead,
      },
      {
        label: 'Branches',
        route: '/branches',
        permission: PermissionTypes.BranchRead,
      },
      {
        label: 'Calendars',
        route: '/calendars',
        permission: PermissionTypes.CalendarRead,
      },
    ],
  },
  {
    label: 'Clients',
    icon: 'business',
    route: '/clients',
    permission: PermissionTypes.ClientRead,
  },
  {
    label: 'Self-serve',
    route: '/selfserve',
    permission: PermissionTypes.SelfserveLeaveCreate,
  },
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
})
export class SidebarComponent implements OnInit {
  menuItems: MenuItem[] = [];

  constructor(private permissionService: NgxPermissionsService) {}

  ngOnInit(): void {
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
