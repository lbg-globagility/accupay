import Swal from 'sweetalert2';
import { Component, ViewChild } from '@angular/core';
import { OvertimeService } from 'src/app/overtimes/overtime.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { OvertimeFormComponent } from '../overtime-form/overtime-form.component';
import { LoadingState } from 'src/app/core/states/loading-state';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-new-overtime',
  templateUrl: './new-overtime.component.html',
  styleUrls: ['./new-overtime.component.scss'],
})
export class NewOvertimeComponent {
  @ViewChild(OvertimeFormComponent)
  overtimeForm: OvertimeFormComponent;

  savingState: LoadingState = new LoadingState();

  constructor(
    private overtimeService: OvertimeService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<NewOvertimeComponent>
  ) {}

  onSave(): void {
    if (!this.overtimeForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const overtime = this.overtimeForm.value;

    this.overtimeService.create(overtime).subscribe({
      next: (result) => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.dialog.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to create overtime.');
      },
    });
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
}
