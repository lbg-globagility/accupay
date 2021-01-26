import Swal from 'sweetalert2';
import { Component, ViewChild } from '@angular/core';
import { LeaveService } from 'src/app/leaves/leave.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MatDialogRef } from '@angular/material/dialog';
import { LoadingState } from 'src/app/core/states/loading-state';
import { LeaveFormComponent } from 'src/app/leaves/leave-form/leave-form.component';

@Component({
  selector: 'app-new-leave',
  templateUrl: './new-leave.component.html',
  styleUrls: ['./new-leave.component.scss'],
})
export class NewLeaveComponent {
  @ViewChild(LeaveFormComponent)
  leaveForm: LeaveFormComponent;

  savingState: LoadingState = new LoadingState();

  constructor(
    private leaveService: LeaveService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<NewLeaveComponent>
  ) {}

  onSave(): void {
    if (!this.leaveForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const leave = this.leaveForm.value;

    this.leaveService.create(leave).subscribe({
      next: (result) => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.dialog.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to create leave.');
      },
    });
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
