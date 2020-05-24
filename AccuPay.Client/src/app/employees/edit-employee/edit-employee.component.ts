import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../services/employee.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Employee } from '../shared/employee';
import { BehaviorSubject } from 'rxjs';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-edit-employee',
  templateUrl: './edit-employee.component.html',
  styleUrls: ['./edit-employee.component.scss'],
})
export class EditEmployeeComponent implements OnInit {
  private id: number;

  employee: Employee;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  constructor(
    private route: ActivatedRoute,
    private service: EmployeeService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((param) => {
      this.id = +param.get('id');

      this.service.getById(this.id).subscribe((employee) => {
        this.employee = employee;

        this.isLoading.next(true);
      });
    });
  }

  save(employee: Employee) {
    this.service.update(this.id, employee).subscribe(
      () => {
        this.router.navigate(['employees', this.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to update employee.')
    );
  }

  cancel() {
    this.router.navigate(['employees', this.id]);
  }
}
