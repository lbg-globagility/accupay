import Swal from 'sweetalert2';
import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { OvertimeFormComponent } from 'src/app/overtimes/overtime-form/overtime-form.component';
import { LoadingState } from 'src/app/core/states/loading-state';

@Component({
  selector: 'app-edit-overtime',
  templateUrl: './edit-overtime.component.html',
  styleUrls: ['./edit-overtime.component.scss'],
})
export class EditOvertimeComponent implements OnInit {
  @ViewChild(OvertimeFormComponent)
  overtimeForm: OvertimeFormComponent;

  overtime: Overtime;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  savingState: LoadingState = new LoadingState();

  overtimeId: number;

  constructor(
    private overtimeService: OvertimeService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<EditOvertimeComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.overtimeId = data.overtimeId;
  }

  ngOnInit(): void {
    this.loadOvertime();
  }

  onSave(): void {
    if (!this.overtimeForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const leave = this.overtimeForm.value;

    this.overtimeService.update(leave, this.overtimeId).subscribe({
      next: () => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.dialog.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to update overtime.');
      },
    });
  }

  private loadOvertime(): void {
    this.overtimeService.get(this.overtimeId).subscribe((data) => {
      this.overtime = data;
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
