import Swal from 'sweetalert2';
import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Leave } from 'src/app/leaves/shared/leave';
import { LeaveService } from 'src/app/leaves/leave.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LeaveFormComponent } from 'src/app/leaves/leave-form/leave-form.component';
import { LoadingState } from 'src/app/core/states/loading-state';

@Component({
  selector: 'app-edit-leave',
  templateUrl: './edit-leave.component.html',
  styleUrls: ['./edit-leave.component.scss'],
})
export class EditLeaveComponent implements OnInit {
  @ViewChild(LeaveFormComponent)
  leaveForm: LeaveFormComponent;

  leave: Leave;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  savingState: LoadingState = new LoadingState();

  leaveId: number;

  constructor(
    private leaveService: LeaveService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<EditLeaveComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.leaveId = data.leaveId;
  }

  ngOnInit(): void {
    this.loadLeave();
  }

  onSave(): void {
    if (!this.leaveForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const leave = this.leaveForm.value;

    this.leaveService.update(leave, this.leaveId).subscribe({
      next: () => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.dialog.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to update leave.');
      },
    });
  }

  private loadLeave(): void {
    this.leaveService.get(this.leaveId).subscribe((data) => {
      this.leave = data;
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
}
