import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject } from 'rxjs';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { OfficialBusinessService } from 'src/app/official-businesses/official-business.service';

@Component({
  selector: 'app-edit-official-business',
  templateUrl: './edit-official-business.component.html',
  styleUrls: ['./edit-official-business.component.scss'],
})
export class EditOfficialBusinessComponent implements OnInit {
  officialBusiness: OfficialBusiness;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  officialBusinessId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private officialBusinessService: OfficialBusinessService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadOfficialBusiness();
  }

  onSave(officialBusiness: OfficialBusiness): void {
    this.officialBusinessService
      .update(officialBusiness, this.officialBusinessId)
      .subscribe(
        () => {
          this.displaySuccess();
          this.router.navigate([
            'official-businesses',
            this.officialBusinessId,
          ]);
        },
        (err) => this.showErrorDialog(err)
      );
  }

  onCancel(): void {
    this.router.navigate(['official-businesses', this.officialBusinessId]);
  }

  private loadOfficialBusiness(): void {
    this.officialBusinessService
      .get(this.officialBusinessId)
      .subscribe((data) => {
        this.officialBusiness = data;

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

  private showErrorDialog(err): void {
    let message: string = 'Failed to update official business';

    if (err && err.status == 400) {
      message = err.error.Error;
    }
    this.snackBar.open(message, null, {
      duration: 2000,
      panelClass: ['mat-toolbar', 'mat-warn'],
    });
  }
}
