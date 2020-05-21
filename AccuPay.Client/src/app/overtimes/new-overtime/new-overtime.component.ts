import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';

@Component({
  selector: 'app-new-overtime',
  templateUrl: './new-overtime.component.html',
  styleUrls: ['./new-overtime.component.scss'],
})
export class NewOvertimeComponent {
  constructor(
    private overtimeService: OvertimeService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  onSave(overtime: Overtime): void {
    this.overtimeService.create(overtime).subscribe(
      (x) => {
        this.displaySuccess();
        this.router.navigate(['overtimes', x.id]);
      },
      (err) => this.showErrorDialog(err)
    );
  }

  onCancel(): void {
    this.router.navigate(['overtimes']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new overtime!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }

  private showErrorDialog(err): void {
    let message: string = 'Failed to update overtime.';

    if (err && err.status == 400) {
      message = err.error.Error;
    }
    this.snackBar.open(message, null, {
      duration: 2000,
      panelClass: ['mat-toolbar', 'mat-warn'],
    });
  }
}
