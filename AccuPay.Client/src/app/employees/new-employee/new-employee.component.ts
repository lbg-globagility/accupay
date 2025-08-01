import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../services/employee.service';
import { Employee } from '../shared/employee';
import { Router } from '@angular/router';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-employee',
  templateUrl: './new-employee.component.html',
  styleUrls: ['./new-employee.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewEmployeeComponent implements OnInit {
  baseRoute = 'employees';

  constructor(
    private service: EmployeeService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {}

  save(employee: Employee) {
    this.service.create(employee).subscribe(
      (e) => {
        this.router.navigate([this.baseRoute, e.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create employee.')
    );
  }

  cancel() {
    this.router.navigate([this.baseRoute]);
  }
}
