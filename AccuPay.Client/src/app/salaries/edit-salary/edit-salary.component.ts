import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Salary } from 'src/app/salaries/shared/salary';
import { SalaryService } from 'src/app/salaries/salary.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-edit-salary',
  templateUrl: './edit-salary.component.html',
  styleUrls: ['./edit-salary.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class EditSalaryComponent implements OnInit {
  salary: Salary;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  salaryId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private salaryService: SalaryService,
    private route: ActivatedRoute,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadSalary();
  }

  onSave(salary: Salary): void {
    this.salaryService.update(salary, this.salaryId).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['salaries', this.salary.employeeId]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to update salary.')
    );
  }

  onCancel(): void {
    this.router.navigate(['salaries', this.salary.employeeId]);
  }

  private loadSalary(): void {
    this.salaryService.get(this.salaryId).subscribe((data) => {
      this.salary = data;

      this.isLoading.next(true);
    });
  }

  private displaySuccess(): void {
    Swal.fire({
      title: 'Success',
      text: 'Successfully updated!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
