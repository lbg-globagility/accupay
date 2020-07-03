import { Component, OnInit, Output } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-select-payperiod-range',
  templateUrl: './select-payperiod-range.component.html',
  styleUrls: ['./select-payperiod-range.component.scss'],
})
export class SelectPayperiodRangeComponent implements OnInit {
  @Output()
  form: FormGroup = this.fb.group({});

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {}
}
