import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { AllowanceTypeService } from '../service/allowance-type.service';
import { BehaviorSubject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AllowanceType } from 'src/app/allowances/shared/allowance-type';
import { AllowanceTypeFormComponent } from '../allowance-type-form/allowance-type-form.component';
import { LoadingState } from 'src/app/core/states/loading-state';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-edit-allowance-type',
  templateUrl: './edit-allowance-type.component.html',
  styleUrls: ['./edit-allowance-type.component.scss'],
})
export class EditAllowanceTypeComponent implements OnInit {
  @ViewChild(AllowanceTypeFormComponent)
  allowanceTypeForm: AllowanceTypeFormComponent;

  savingState: LoadingState = new LoadingState();

  allowanceType: AllowanceType;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  id: number;

  constructor(
    private service: AllowanceTypeService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<EditAllowanceTypeComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.id = data.allowanceTypeId;
  }

  ngOnInit(): void {
    this.loadAllowanceType();
  }

  private loadAllowanceType(): void {
    this.service.get(this.id).subscribe((data) => {
      this.allowanceType = data;

      this.isLoading.next(true);
    });
  }

  onSave() {
    if (!this.allowanceTypeForm.form.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const allowanceType = this.allowanceTypeForm.value;

    this.service.update(allowanceType, this.id).subscribe({
      next: (result) => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.dialog.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to edit allowance type.');
      },
    });
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully updated an allowance type!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
