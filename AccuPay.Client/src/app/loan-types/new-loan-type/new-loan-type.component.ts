import { Component, OnInit, ViewChild } from '@angular/core';
import { LoanTypeFormComponent } from '../loan-type-form/loan-type-form.component';
import { LoanType } from '../shared/loan-type';
import { LoanTypeService } from '../service/loan-type.service';
import { LoadingState } from 'src/app/core/states/loading-state';
import { MatDialogRef } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-loan-type',
  templateUrl: './new-loan-type.component.html',
  styleUrls: ['./new-loan-type.component.scss'],
})
export class NewLoanTypeComponent implements OnInit {
  @ViewChild(LoanTypeFormComponent)
  loanTypeForm: LoanTypeFormComponent;

  savingState: LoadingState = new LoadingState();

  newLoanType: LoanType;

  constructor(
    private service: LoanTypeService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<NewLoanTypeComponent>
  ) {}

  ngOnInit(): void {
    this.newLoanType = {
      id: 0,
      name: '',
    };
  }

  onSave() {
    if (!this.loanTypeForm.form.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const loanType = this.loanTypeForm.value;

    this.service.create(loanType).subscribe({
      next: (result) => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.dialog.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to create loan type.');
      },
    });
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new loan type!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
