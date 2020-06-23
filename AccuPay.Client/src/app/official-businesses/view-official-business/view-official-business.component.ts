import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { OfficialBusinessService } from 'src/app/official-businesses/official-business.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-view-official-business',
  templateUrl: './view-official-business.component.html',
  styleUrls: ['./view-official-business.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class ViewOfficialBusinessComponent implements OnInit {
  officialBusiness: OfficialBusiness;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  officialBusinessId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private officialBusinessService: OfficialBusinessService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadOfficialBusiness();
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Official Business',
        content: 'Are you sure you want to delete this official business?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      this.officialBusinessService.delete(this.officialBusinessId).subscribe(
        () => {
          this.router.navigate(['official-businesses']);
          Swal.fire({
            title: 'Deleted',
            text: `The official business was successfully deleted.`,
            icon: 'success',
            showConfirmButton: true,
          });
        },
        (err) =>
          this.errorHandler.badRequest(
            err,
            'Failed to delete official business.'
          )
      );
    });
  }

  private loadOfficialBusiness(): void {
    this.officialBusinessService
      .get(this.officialBusinessId)
      .subscribe((data) => {
        this.officialBusiness = data;

        this.isLoading.next(true);
      });
  }
}
