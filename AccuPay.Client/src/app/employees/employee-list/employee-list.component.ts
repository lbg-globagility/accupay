import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../services/employee.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Employee } from '../shared/employee';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent implements OnInit {
  totalPages: number;
  totalCount: number;
  term: string;
  list: Employee[];

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {
    this.load();
  }

  load() {
    const options = new PageOptions();

    this.employeeService.getList(options, this.term).subscribe(async (data) => {
      await setTimeout(() => {
        this.list = data.items;

        this.totalPages = data.totalPages;
        this.totalCount = data.totalCount;
      });
    });
  }
}
