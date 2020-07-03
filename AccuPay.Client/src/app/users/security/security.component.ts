import { Component, OnInit } from '@angular/core';

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
    },
    {
      path: 'roles',
      label: 'Roles',
    },
    {
      path: 'user-access',
      label: 'User Access',
    },
  ];

  constructor() {}

  ngOnInit(): void {}
}
