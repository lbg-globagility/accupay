import { Component, OnInit } from '@angular/core';
import { ShiftService } from '../shift.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import Swal from 'sweetalert2';
import { Shift } from '../shared/shift';

@Component({
  selector: 'app-new-shift',
  templateUrl: './new-shift.component.html',
  styleUrls: ['./new-shift.component.scss']
})
export class NewShiftComponent {
  constructor(
    private shiftService: ShiftService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  onSave(shift: Shift): void {
    this.shiftService.create(shift).subscribe(
      (x) => {
        this.displaySuccess();
        this.router.navigate(['shifts', x.id]);
      },
      (err) => this.showErrorDialog(err)
    );
  }

  onCancel(): void {
    this.router.navigate(['shifts']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new shift!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }

  private showErrorDialog(err): void {
    let message: string = 'Failed to update shift.';

    if (err && err.status == 400) {
      message = err.error.Error;
    }
    this.snackBar.open(message, null, {
      duration: 2000,
      panelClass: ['mat-toolbar', 'mat-warn'],
    });
  }
}