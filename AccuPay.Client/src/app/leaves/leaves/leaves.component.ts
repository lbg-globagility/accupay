import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-leaves',
  templateUrl: './leaves.component.html',
  styleUrls: ['./leaves.component.scss'],
})
export class LeavesComponent implements OnInit {
  readonly navLinks = [
    {
      path: 'overview',
      label: 'Leaves',
    },
    {
      path: 'balances',
      label: 'Balances',
    },
  ];

  constructor() {}

  ngOnInit(): void {}
}
