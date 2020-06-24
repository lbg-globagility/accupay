import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-report-form',
  templateUrl: './report-form.component.html',
  styleUrls: ['./report-form.component.scss'],
})
export class ReportFormComponent implements OnInit {
  reportType: any = this.route.snapshot.paramMap.get('report');

  @Output()
  save: EventEmitter<Date> = new EventEmitter();

  form: FormGroup = this.fb.group({
    dateFrom: [null, Validators.required],
    dateTo: [null, Validators.required],
  });

  dateFrom: Date;
  dateTo: Date;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.reportType = String(paramMap.get('report'));
    });
  }

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
