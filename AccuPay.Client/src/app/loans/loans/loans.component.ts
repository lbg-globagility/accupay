import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-loans',
  templateUrl: './loans.component.html',
  styleUrls: ['./loans.component.scss'],
})
export class LoansComponent implements OnInit {
  readonly navLinks = [
    {
      path: 'overview',
      label: 'Loans',
    },
    {
      path: 'types',
      label: 'Types',
    },
  ];

  constructor() {}

  ngOnInit(): void {}
}
