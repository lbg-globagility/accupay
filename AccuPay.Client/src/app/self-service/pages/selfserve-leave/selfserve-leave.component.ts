import { Component, OnInit, ViewChild } from '@angular/core';
import { Leave } from 'src/app/leaves/shared/leave';
import { LoadingState } from 'src/app/core/states/loading-state';
import { SelfserveService } from '../../services/selfserve.service';
import { SelfserveLeaveFormComponent } from '../../components/selfserve-leave-form/selfserve-leave-form.component';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-selfserve-leave',
  templateUrl: './selfserve-leave.component.html',
  styleUrls: ['./selfserve-leave.component.scss'],
})
export class SelfserveLeaveComponent implements OnInit {
  @ViewChild(SelfserveLeaveFormComponent)
  leaveForm: SelfserveLeaveFormComponent;

  leave: Leave = {
    comments: null,
    employeeId: null,
    employeeName: null,
    employeeNumber: null,
    employeeType: null,
    endDate: null,
    endTime: null,
    id: null,
    leaveType: null,
    reason: null,
    startDate: null,
    startTime: null,
    status: 'Pending',
  };

  savingState: LoadingState = new LoadingState();

  constructor(
    private service: SelfserveService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<SelfserveLeaveComponent>
  ) {}

  ngOnInit(): void {}

  onSave(): void {
    if (!this.leaveForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const leave = this.leaveForm.value;

    this.service.createLeave(leave).subscribe({
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
      text: 'Request has been sent for approval',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
