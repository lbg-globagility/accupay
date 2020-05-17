import { Component, OnInit } from '@angular/core';
import { Organization } from 'src/app/accounts/shared/organization';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],
})
export class SidenavComponent implements OnInit {
  organization: Organization;

  imageUrl: string;

  moment: string;

  constructor() {}

  ngOnInit(): void {}
}
