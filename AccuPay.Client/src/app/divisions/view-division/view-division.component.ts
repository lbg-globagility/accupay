import { Component, OnInit } from '@angular/core';
import { Division } from '../shared/division';
import { BehaviorSubject } from 'rxjs';
import { DivisionService } from '../division.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-view-division',
  templateUrl: './view-division.component.html',
  styleUrls: ['./view-division.component.scss'],
})
export class ViewDivisionComponent implements OnInit {
  division: Division;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  divisionId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private divisionService: DivisionService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadDivision();
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Division',
        content: 'Are you sure you want to delete this division?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

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
        (err) => this.errorHandler.badRequest(err, 'Failed to delete division.')
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
