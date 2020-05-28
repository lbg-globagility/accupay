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
    label: 'Employees',
    route: '/employees',
    icon: 'person',
  },
  {
    label: 'Users',
    route: '/users',
    icon: 'person',
  },
  {
    label: 'Salaries',
    route: '/salaries',
    icon: 'person',
  },
  {
    label: 'Leaves',
    route: '/leaves',
    icon: 'person',
  },
  {
    label: 'Official Business',
    route: '/official-businesses',
    icon: 'person',
  },
  {
    label: 'Overtimes',
    route: '/overtimes',
    icon: 'person',
  },
  {
    label: 'Shifts',
    route: '/shifts',
    icon: 'person',
  },
  {
    label: 'Allowances',
    route: '/allowances',
    icon: 'person',
  },
  {
    label: 'Loans',
    route: '/loans',
    icon: 'person',
  },
  {
    label: 'Organizations',
    route: '/organizations',
    icon: 'person',
  },
  {
    label: 'Branches',
    route: '/branches',
    icon: 'person',
  },
  {
    label: 'Positions',
    route: '/positions',
    icon: 'person',
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
