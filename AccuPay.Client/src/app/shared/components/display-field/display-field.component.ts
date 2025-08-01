import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-display-field',
  templateUrl: './display-field.component.html',
  styleUrls: ['./display-field.component.scss'],
})
export class DisplayFieldComponent implements OnInit {
  @Input()
  label: string;

  constructor() {}

  ngOnInit(): void {}
}
