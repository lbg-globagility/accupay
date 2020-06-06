import Swal from 'sweetalert2';
import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { OfficialBusinessService } from 'src/app/official-businesses/official-business.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MatDialogRef } from '@angular/material/dialog';
import { OfficialBusinessFormComponent } from '../official-business-form/official-business-form.component';
import { LoadingState } from 'src/app/core/states/loading-state';

@Component({
  selector: 'app-new-official-business',
  templateUrl: './new-official-business.component.html',
  styleUrls: ['./new-official-business.component.scss'],
})
export class NewOfficialBusinessComponent {
  @ViewChild(OfficialBusinessFormComponent)
  officialBusinessForm: OfficialBusinessFormComponent;

  savingState: LoadingState = new LoadingState();

  constructor(
    private officialBusinessService: OfficialBusinessService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<NewOfficialBusinessComponent>
  ) {}

  onSave(officialBusiness: OfficialBusiness): void {
    if (!this.officialBusinessForm.form.valid) {
      return;
    }

    this.savingState.changeToLoading();

    this.officialBusinessService.create(officialBusiness).subscribe(
      (x) => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.dialog.close(true);
      },
      (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(
          err,
          'Failed to create official business.'
        );
      }
    );
  }

  onCancel(): void {
    this.dialog.close();
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new official business!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
