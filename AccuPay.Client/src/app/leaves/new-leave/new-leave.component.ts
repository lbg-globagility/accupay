import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Leave } from 'src/app/leaves/shared/leave';
import { LeaveService } from 'src/app/leaves/leave.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-leave',
  templateUrl: './new-leave.component.html',
  styleUrls: ['./new-leave.component.scss'],
})
export class NewLeaveComponent {
  constructor(
    private leaveService: LeaveService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  onSave(leave: Leave): void {
    this.leaveService.create(leave).subscribe(
      (result) => {
        this.displaySuccess();
        this.router.navigate(['leaves', result.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create leave.')
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
}
