import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { Salary } from 'src/app/salaries/shared/salary';
import { SalaryService } from 'src/app/salaries/salary.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { PageOptions } from 'src/app/core/shared/page-options';

@Component({
  selector: 'app-view-salary',
  templateUrl: './view-salary.component.html',
  styleUrls: ['./view-salary.component.scss'],
})
export class ViewSalaryComponent implements OnInit {
  employeeId: number = Number(this.route.snapshot.paramMap.get('employeeId'));

  latestSalary: Salary;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  salaryId = Number(this.route.snapshot.paramMap.get('id'));

  salaries: Salary[] = [];

  displayedColumns = [
    'effectiveFrom',
    'basicAmount',
    'allowanceAmount',
    'totalAmount',
  ];

  constructor(
    private salaryService: SalaryService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.employeeId = Number(paramMap.get('employeeId'));
      this.loadSalaries();
      this.loadSalary();
    });
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

      this.salaryService.delete(this.salaryId).subscribe(
        () => {
          this.router.navigate(['salaries']);
          Swal.fire({
            title: 'Deleted',
            text: `The salary was successfully deleted.`,
            icon: 'success',
            showConfirmButton: true,
          });
        },
        (err) => this.errorHandler.badRequest(err, 'Failed to delete salary.')
      );
    });
  }

  private loadSalaries(): void {
    const options = new PageOptions(0, 25);
    this.salaryService
      .list(options, null, this.employeeId)
      .subscribe((data) => {
        this.salaries = data.items;
      });
  }

  private loadSalary(): void {
    this.salaryService.getLatest(this.employeeId).subscribe((salary) => {
      this.latestSalary = salary;

      this.isLoading.next(true);
    });
  }
}
