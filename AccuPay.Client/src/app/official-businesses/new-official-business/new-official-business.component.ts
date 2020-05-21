import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { OfficialBusinessService } from 'src/app/official-businesses/official-business.service';

@Component({
  selector: 'app-new-official-business',
  templateUrl: './new-official-business.component.html',
  styleUrls: ['./new-official-business.component.scss'],
})
export class NewOfficialBusinessComponent {
  constructor(
    private officialBusinessService: OfficialBusinessService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  onSave(officialBusiness: OfficialBusiness): void {
    this.officialBusinessService.create(officialBusiness).subscribe(
      (x) => {
        this.displaySuccess();
        this.router.navigate(['official-businesses', x.id]);
      },
      (err) => this.showErrorDialog(err)
    );
  }

  onCancel(): void {
    this.router.navigate(['official-businesses']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new official business!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }

  private showErrorDialog(err): void {
    let message: string = 'Failed to update official business.';

    if (err && err.status == 400) {
      message = err.error.Error;
    }
    this.snackBar.open(message, null, {
      duration: 2000,
      panelClass: ['mat-toolbar', 'mat-warn'],
    });
  }
}
