import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-loan-report',
  templateUrl: './loan-report.component.html',
  styleUrls: ['./loan-report.component.scss'],
})
export class LoanReportComponent implements OnInit {
  @Output()
  save: EventEmitter<Date> = new EventEmitter();

  form: FormGroup = this.fb.group({
    dateFrom: [null, Validators.required],
    dateTo: [null, Validators.required],
  });

  dateFrom: Date;
  dateTo: Date;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {}

  onSave(): void {
    if (!this.form.valid) {
      return;
    }

    this.dateFrom = this.form.value.dateFrom.toDate();
    this.dateTo = this.form.value.dateTo.toDate();

    if (this.dateFrom > this.dateTo) {
      return;
    }
    console.log(this.dateFrom, this.dateTo);
  }
}
