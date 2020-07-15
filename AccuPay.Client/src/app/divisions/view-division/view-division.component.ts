import { Component, OnInit } from '@angular/core';
import { Division } from '../shared/division';
import { BehaviorSubject } from 'rxjs';
import { DivisionService } from '../division.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-view-division',
  templateUrl: './view-division.component.html',
  styleUrls: ['./view-division.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class ViewDivisionComponent implements OnInit {
  divisionId: number = null;

  division: Division;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  constructor(
    private divisionService: DivisionService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.divisionId = Number(paramMap.get('id'));
      this.loadDivision();
    });
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Division',
        content: 'Are you sure you want to delete this division?',
      },
    });

    dialogRef
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => {
        this.divisionService.delete(this.divisionId).subscribe(
          () => {
            this.router.navigate(['divisions']);
            Swal.fire({
              title: 'Deleted',
              text: `The division was successfully deleted.`,
              icon: 'success',
              showConfirmButton: true,
            });
          },
          (err) =>
            this.errorHandler.badRequest(err, 'Failed to delete division.')
        );
      });
  }

  private loadDivision(): void {
    this.divisionService.get(this.divisionId).subscribe((data) => {
      this.division = data;

      this.isLoading.next(true);
    });
  }
}
