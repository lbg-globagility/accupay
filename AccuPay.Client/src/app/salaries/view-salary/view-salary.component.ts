import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { Salary } from 'src/app/salaries/shared/salary';
import { SalaryService } from 'src/app/salaries/salary.service';

@Component({
  selector: 'app-view-salary',
  templateUrl: './view-salary.component.html',
  styleUrls: ['./view-salary.component.scss'],
})
export class ViewSalaryComponent implements OnInit {
  salary: Salary;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  salaryId = this.route.snapshot.paramMap.get('id');

  constructor(
    private salaryService: SalaryService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadSalary();
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Salary',
        content: 'Are you sure you want to delete this salary?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      this.salaryService.delete(this.salaryId).subscribe(() => {
        this.router.navigate(['salaries']);
        Swal.fire({
          title: 'Deleted',
          text: `The salary was successfully deleted.`,
          icon: 'success',
          showConfirmButton: true,
        });
      });
    });
  }

  private loadSalary(): void {
    this.salaryService.get(this.salaryId).subscribe((data) => {
      this.salary = data;

      this.isLoading.next(true);
    });
  }
}
