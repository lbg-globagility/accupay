import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Leave } from 'src/app/leaves/shared/leave';
import { LeaveService } from 'src/app/leaves/leave.service';

@Component({
  selector: 'app-new-leave',
  templateUrl: './new-leave.component.html',
  styleUrls: ['./new-leave.component.scss'],
})
export class NewLeaveComponent {
  constructor(
    private leaveService: LeaveService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  onSave(leave: Leave): void {
    this.leaveService.create(leave).subscribe(
      (result) => {
        this.displaySuccess();
        this.router.navigate(['leaves', result.id]);
      },
      (err) => this.showErrorDialog(err)
    );
  }

  onCancel(): void {
    this.router.navigate(['leaves']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new leave!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }

  private showErrorDialog(err): void {
    let message: string = 'Failed to create leave';

    if (err && err.status == 400) {
      message = err.error.Error;
    }
    this.snackBar.open(message, null, {
      duration: 2000,
      panelClass: ['mat-toolbar', 'mat-warn'],
    });
  }
}
