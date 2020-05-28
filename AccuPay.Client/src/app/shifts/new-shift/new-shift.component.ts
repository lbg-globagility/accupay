import { Component } from '@angular/core';
import { ShiftService } from '../shift.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';
import { Shift } from '../shared/shift';

@Component({
  selector: 'app-new-shift',
  templateUrl: './new-shift.component.html',
  styleUrls: ['./new-shift.component.scss'],
})
export class NewShiftComponent {
  constructor(
    private shiftService: ShiftService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  onSave(shift: Shift): void {
    this.shiftService.create(shift).subscribe(
      (x) => {
        this.displaySuccess();
        this.router.navigate(['shifts', x.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to update shift.')
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
}
