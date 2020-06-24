import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Salary } from 'src/app/salaries/shared/salary';
import { SalaryService } from 'src/app/salaries/salary.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-salary',
  templateUrl: './new-salary.component.html',
  styleUrls: ['./new-salary.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewSalaryComponent {
  constructor(
    private salaryService: SalaryService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  onSave(salary: Salary): void {
    this.salaryService.create(salary).subscribe(
      (s) => {
        this.displaySuccess();
        this.router.navigate(['salaries', s.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create salary.')
    );
  }

  onCancel(): void {
    this.router.navigate(['salaries']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new salary!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
