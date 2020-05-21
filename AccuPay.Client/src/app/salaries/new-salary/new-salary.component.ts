import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Salary } from 'src/app/salaries/shared/salary';
import { SalaryService } from 'src/app/salaries/salary.service';

@Component({
  selector: 'app-new-salary',
  templateUrl: './new-salary.component.html',
  styleUrls: ['./new-salary.component.scss'],
})
export class NewSalaryComponent {
  constructor(
    private salaryService: SalaryService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  onSave(salary: Salary): void {
    this.salaryService.create(salary).subscribe(
      (s) => {
        this.displaySuccess();
        this.router.navigate(['salaries', s.id]);
      },
      (err) => this.showErrorDialog()
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

  private showErrorDialog(): void {
    this.snackBar.open('Failed to create the salary', null, {
      duration: 2000,
    });
  }
}
