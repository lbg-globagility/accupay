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
    label: 'Dashboard',
    route: '/dashboard',
    icon: 'dashboard',
    permission: 'dashboard.ViewDashboard',
  },
  {
    label: 'Recalls',
    icon: 'drafts',
    items: [
      {
        label: 'List',
        route: '/recalls',
      },
      {
        label: 'New',
        route: '/recalls/new',
        permission: 'recalls.CreateRecall',
      },
    ],
  },
  {
    label: 'Suppliers',
    route: '/suppliers',
    icon: 'local_shipping',
    permission: 'suppliers.ListSuppliers',
  },
  {
    label: 'Providers',
    route: '/providers',
    icon: 'local_hospital',
    permission: 'providers.ListProviders',
  },
  {
    label: 'Workflow Groups',
    route: '/workflow-groups',
    icon: 'supervised_user_circle',
    permission: 'recalls.CreateWorkflowGroups',
  },
  {
    label: 'Users',
    route: '/users',
    icon: 'person',
    permission: 'users.ListUsers',
  },
  {
    label: 'Roles',
    route: '/roles',
    icon: 'verified_user',
    permission: 'users.ListRoles',
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
