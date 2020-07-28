import { Component, OnInit } from '@angular/core';
import { PermissionTypes } from 'src/app/core/auth';

@Component({
  selector: 'app-security',
  templateUrl: './security.component.html',
  styleUrls: ['./security.component.scss'],
})
export class SecurityComponent implements OnInit {
  readonly navLinks = [
    {
      path: 'users',
      label: 'Users',
      permission: PermissionTypes.UserRead,
    },
    {
      path: 'roles',
      label: 'Roles',
      permission: PermissionTypes.RoleRead,
    },
    {
      path: 'user-access',
      label: 'User Access',
      permission: PermissionTypes.RoleRead,
    },
  ];

  constructor() {}

  ngOnInit(): void {}
}
