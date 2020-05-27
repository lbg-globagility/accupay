import { Component, OnInit } from '@angular/core';
import { TimeLogService } from '../service/time-log.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Sort } from '@angular/material/sort';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TimeLogFilter } from '../shared/time-log-filter';

@Component({
  selector: 'app-time-log-table',
  templateUrl: './time-log-table.component.html',
  styleUrls: ['./time-log-table.component.scss'],
})
export class TimeLogTableComponent implements OnInit {
  pageIndex = 0;

  pageSize = 10;

  sort: Sort = {
    active: 'name',
    direction: '',
  };

  term: string;

  events: string[] = [];

  startDate = new Date();
  endDate = new Date();

  form: FormGroup = this.fb.group({
    startDate: [this.startDate, Validators.required],
    endDate: [this.endDate, Validators.required],
  });

  constructor(private service: TimeLogService, private fb: FormBuilder) {}

  ngOnInit(): void {}

  load() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    const filter: TimeLogFilter = {
      employeeIds: [310, 311, 313],
      startDate: this.startDate,
      endDate: this.endDate,
    };

    this.service.getList(options, filter).subscribe(async (data) => {
      await setTimeout(() => {
        // this.employees = data.items;
        // this.totalPages = data.totalPages;
        // this.totalCount = data.totalCount;
        // this.dataSource = new MatTableDataSource(this.employees);
      });
    });
  }

  startDateEvent(type: string, event: MatDatepickerInputEvent<Date>) {
    this.startDate = event.value;
  }

  endDateEvent(type: string, event: MatDatepickerInputEvent<Date>) {
    this.endDate = event.value;
  }
}
