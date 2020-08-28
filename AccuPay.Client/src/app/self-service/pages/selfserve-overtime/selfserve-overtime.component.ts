import { Component, OnInit, ViewChild } from '@angular/core';
import { SelfserveService } from '../../services/selfserve.service';
import { MatDialogRef } from '@angular/material/dialog';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { LoadingState } from 'src/app/core/states/loading-state';
import { SelfserveOvertimeFormComponent } from '../../components/selfserve-overtime-form/selfserve-overtime-form.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-selfserve-overtime',
  templateUrl: './selfserve-overtime.component.html',
  styleUrls: ['./selfserve-overtime.component.scss'],
})
export class SelfserveOvertimeComponent implements OnInit {
  @ViewChild(SelfserveOvertimeFormComponent)
  overtimeForm: SelfserveOvertimeFormComponent;

  overtime: Overtime;

  savingState: LoadingState = new LoadingState();

  constructor(
    private service: SelfserveService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<SelfserveOvertimeComponent>
  ) {}

  ngOnInit(): void {}

  onSave(): void {
    if (!this.overtimeForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const overtime = this.overtimeForm.value;

    this.service.createOvertime(overtime).subscribe({
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
