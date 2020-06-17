import { Component, OnInit, ViewChild } from '@angular/core';
import { AllowanceTypeFormComponent } from '../allowance-type-form/allowance-type-form.component';
import { LoadingState } from 'src/app/core/states/loading-state';
import { AllowanceTypeService } from '../service/allowance-type.service';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MatDialogRef } from '@angular/material/dialog';
import { AllowanceType } from '../shared/allowance-type';

@Component({
  selector: 'app-new-allowance-type',
  templateUrl: './new-allowance-type.component.html',
  styleUrls: ['./new-allowance-type.component.scss'],
})
export class NewAllowanceTypeComponent implements OnInit {
  @ViewChild(AllowanceTypeFormComponent)
  allowanceTypeForm: AllowanceTypeFormComponent;

  savingState: LoadingState = new LoadingState();

  newAllowanceType: AllowanceType;

  constructor(
    private service: AllowanceTypeService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<NewAllowanceTypeComponent>
  ) {}

  ngOnInit(): void {
    this.newAllowanceType = {
      id: 0,
      dispalyString: '',
      frequency: '',
      is13thMonthPay: false,
      isFixed: false,
      isTaxable: false,
      name: '',
    };
  }

  onSave() {
    if (!this.allowanceTypeForm.form.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const allowanceType = this.allowanceTypeForm.value;

    this.service.create(allowanceType).subscribe({
      next: (result) => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.dialog.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to create allowance type.');
      },
    });
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new allowance type!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
