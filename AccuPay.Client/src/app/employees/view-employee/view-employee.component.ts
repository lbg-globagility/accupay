import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { EmployeeService } from '../services/employee.service';
import { BehaviorSubject } from 'rxjs';
import { Employee } from '../shared/employee';

@Component({
  selector: 'app-view-employee',
  templateUrl: './view-employee.component.html',
  styleUrls: ['./view-employee.component.scss'],
  host: {
    class: 'block',
  },
})
export class ViewEmployeeComponent implements OnInit {
  id: number;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);
  employee: Employee;

  constructor(
    private route: ActivatedRoute,
    private service: EmployeeService,
    private router: Router
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

  routeToEditPage() {
    this.router.navigate(['employees', this.id, 'edit']);
  }
}
