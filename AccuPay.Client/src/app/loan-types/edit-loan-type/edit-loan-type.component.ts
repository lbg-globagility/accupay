import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { LoanType } from '../shared/loan-type';
import { LoanTypeFormComponent } from '../loan-type-form/loan-type-form.component';
import { LoadingState } from 'src/app/core/states/loading-state';
import { BehaviorSubject } from 'rxjs';
import { LoanTypeService } from '../service/loan-type.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-edit-loan-type',
  templateUrl: './edit-loan-type.component.html',
  styleUrls: ['./edit-loan-type.component.scss'],
})
export class EditLoanTypeComponent implements OnInit {
  @ViewChild(LoanTypeFormComponent)
  loanTypeForm: LoanTypeFormComponent;

  savingState: LoadingState = new LoadingState();

  loanType: LoanType;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  id: number;

  constructor(
    private service: LoanTypeService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<EditLoanTypeComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.id = data.loanTypeId;
  }

  ngOnInit(): void {
    this.loadLoanType();
  }

  private loadLoanType(): void {
    this.service.get(this.id).subscribe((data) => {
      this.loanType = data;

      this.isLoading.next(true);
    });
  }

  onSave() {
    if (!this.loanTypeForm.form.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const loanType = this.loanTypeForm.value;

    this.service.update(loanType, this.id).subscribe({
      next: (result) => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.dialog.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to edit loan type.');
      },
    });
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully updated a loan type!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
