import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { Position } from 'src/app/positions/shared/position';
import { PositionService } from 'src/app/positions/position.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { filter } from 'rxjs/operators';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { EmployeePageOptions } from 'src/app/employees/shared/employee-page-options';
import { Employee } from 'src/app/employees/shared/employee';

@Component({
  selector: 'app-view-position',
  templateUrl: './view-position.component.html',
  styleUrls: ['./view-position.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class ViewPositionComponent implements OnInit {
  position: Position;

  employees: Employee[] = [];

  readonly displayedColumns: string[] = ['employee'];

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  positionId: number = null;

  constructor(
    private positionService: PositionService,
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.positionId = Number(paramMap.get('id'));
      this.loadPosition();
      this.loadEmployees();
    });
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Position',
        content: 'Are you sure you want to delete this position?',
      },
    });

    dialogRef
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => {
        this.positionService.delete(this.positionId).subscribe(
          () => {
            this.router.navigate(['positions']);
            Swal.fire({
              title: 'Deleted',
              text: `The position was successfully deleted.`,
              icon: 'success',
              showConfirmButton: true,
            });
          },
          (err) =>
            this.errorHandler.badRequest(err, 'Failed to delete position.')
        );
      });
  }

  private loadPosition(): void {
    this.positionService.get(this.positionId).subscribe((data) => {
      this.position = data;

      this.isLoading.next(true);
    });
  }

  private loadEmployees(): void {
    const options = new EmployeePageOptions(
      0,
      1000,
      null,
      null,
      this.positionId
    );

    this.employeeService.list(options).subscribe((data) => {
      this.employees = data.items;
    });
  }
}
