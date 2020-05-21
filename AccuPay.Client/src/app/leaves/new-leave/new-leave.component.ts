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
      (s) => {
        this.displaySuccess();
        this.router.navigate(['leaves', s.id]);
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
    this.snackBar.open('Failed to create the leave', null, {
      duration: 2000,
    });
  }
}
