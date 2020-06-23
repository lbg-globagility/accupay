import { Component, OnInit } from '@angular/core';
import { cloneDeep } from 'lodash';

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
      },
      {
        label: 'Divisions',
        route: '/divisions',
      },
      {
        label: 'Positions',
        route: '/positions',
      },
      {
        label: 'Organizations',
        route: '/organizations',
      },
      {
        label: 'Branches',
        route: '/branches',
      },
      {
        label: 'Calendars',
        route: '/calendars',
        icon: 'person',
      },
    ],
  },
  {
    label: 'Timekeeping',
    icon: 'access_time',
    items: [
      {
        label: 'Time Entry',
        route: '/time-entry',
      },
      {
        label: 'Shifts',
        route: '/shifts',
      },
      {
        label: 'Time Logs',
        route: '/time-logs',
      },
      {
        label: 'Overtimes',
        route: '/overtimes',
      },
      {
        label: 'Leaves',
        route: '/leaves',
      },
      {
        label: 'Leave Balance',
        route: '/leave-balance',
      },
      {
        label: 'Official Business',
        route: '/official-businesses',
      },
    ],
  },
  {
    label: 'Payroll',
    icon: 'payments',
    items: [
      {
        label: 'Payroll',
        route: '/payroll',
      },
      {
        label: 'Salaries',
        route: '/salaries',
        icon: 'person',
      },
      {
        label: 'Allowances',
        route: '/allowances',
        icon: 'person',
      },
      {
        label: 'Allowance Types',
        route: '/allowance-types',
      },
      {
        label: 'Loans',
        route: '/loans',
        icon: 'person',
      },
    ],
  },
  {
    label: 'Reports',
    icon: 'list_alt',
    items: [
      {
        label: 'Loan Report',
        route: '/loan-report',
      },
    ],
  },
  {
    label: 'Security',
    icon: 'security',
    items: [
      {
        label: 'Users',
        route: '/users',
      },
      {
        label: 'Roles',
        route: '/roles',
      },
      {
        label: 'User Access',
        route: '/user-access',
      },
    ],
  },
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
})
export class SidebarComponent implements OnInit {
  menuItems: MenuItem[] = menuItems;

  constructor() {}

  ngOnInit(): void {
    this.menuItems = menuItems;
  }

  isLink(menuItem: MenuItem): boolean {
    return menuItem.items == null;
  }

  toggle(menuItem: MenuItem): void {
    menuItem.toggled = !menuItem.toggled;
  }
}
