import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-button-spinner',
  templateUrl: './button-spinner.component.html',
  styleUrls: ['./button-spinner.component.scss'],
  host: {
    class: 'inline-block align-middle',
  },
})
export class ButtonSpinnerComponent implements OnInit {
  @Input()
  active: boolean = false;

  constructor() {}

  ngOnInit(): void {}
}
