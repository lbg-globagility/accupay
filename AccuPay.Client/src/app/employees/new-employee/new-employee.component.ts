import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../services/employee.service';
import { Employee } from '../shared/employee';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-employee',
  templateUrl: './new-employee.component.html',
  styleUrls: ['./new-employee.component.scss'],
})
export class NewEmployeeComponent implements OnInit {
  baseRoute = 'employees';

  constructor(private service: EmployeeService, private router: Router) {}

  ngOnInit(): void {}

  save(employee: Employee) {
    this.service.create(employee).subscribe((id) => {
      this.router.navigate([this.baseRoute, id]);
    });
  }

  cancel() {
    this.router.navigate([this.baseRoute]);
  }
}
