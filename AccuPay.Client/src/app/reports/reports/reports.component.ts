import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss'],
  host: {
    class: 'h-full',
  },
})
export class ReportsComponent implements OnInit {
  modelChanged: Subject<void>;

  selectedReports: any;

  selectedReport: string;

  constructor(private router: Router) {
    this.modelChanged = new Subject();
    this.modelChanged.pipe(auditTime(Constants.ThrottleTime)).subscribe();
  }

  ngOnInit(): void {}

  selectReport(): void {
    this.selectedReport =
      this.selectedReports.length > 0 ? this.selectedReports[0] : null;

    this.router.navigate(['reports', this.selectedReport]);
  }
}
