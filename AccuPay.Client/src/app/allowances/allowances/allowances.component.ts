import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-allowances',
  templateUrl: './allowances.component.html',
  styleUrls: ['./allowances.component.scss'],
})
export class AllowancesComponent implements OnInit {
  readonly navLinks = [
    {
      path: 'overview',
      label: 'Allowances',
    },
    {
      path: 'types',
      label: 'Types',
    },
  ];

  constructor() {}

  ngOnInit(): void {}
}
