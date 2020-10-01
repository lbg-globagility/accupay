import { Component, OnInit, ViewChild } from '@angular/core';
import { LoadingState } from 'src/app/core/states/loading-state';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MatDialogRef } from '@angular/material/dialog';
import { SelfserviceLeaveFormComponent } from 'src/app/self-service/leaves/components/selfservice-leave-form/selfservice-leave-form.component';
import { SelfServiceLeaveService } from 'src/app/self-service/services';

@Component({
  selector: 'app-selfservice-new-leave',
  templateUrl: './selfservice-new-leave.component.html',
  styleUrls: ['./selfservice-new-leave.component.scss'],
})
export class SelfserviceNewLeaveComponent implements OnInit {
  @ViewChild(SelfserviceLeaveFormComponent)
  leaveForm: SelfserviceLeaveFormComponent;

  savingState: LoadingState = new LoadingState();

  constructor(
    private service: SelfServiceLeaveService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<SelfserviceNewLeaveComponent>
  ) {}

  ngOnInit(): void {}

  onSave(): void {
    if (!this.leaveForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const leave = this.leaveForm.value;

    this.service.create(leave).subscribe({
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
