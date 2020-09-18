import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { LoadingState } from 'src/app/core/states/loading-state';
import Swal from 'sweetalert2';
import { SelfserviceOfficialBusinessService } from 'src/app/self-service/services/selfservice-official-business.service';
import { SelfserviceOfficialBusinessFormComponent } from 'src/app/self-service/official-businesses/components/selfservice-official-business-form/selfservice-official-business-form.component';

@Component({
  selector: 'app-selfservice-new-official-business',
  templateUrl: './selfservice-new-official-business.component.html',
  styleUrls: ['./selfservice-new-official-business.component.scss'],
})
export class SelfserviceNewOfficialBusinessComponent implements OnInit {
  @ViewChild(SelfserviceOfficialBusinessFormComponent)
  officialBusinessForm: SelfserviceOfficialBusinessFormComponent;

  savingState: LoadingState = new LoadingState();

  constructor(
    private service: SelfserviceOfficialBusinessService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<SelfserviceOfficialBusinessFormComponent>
  ) {}

  ngOnInit(): void {}

  onSave(): void {
    if (!this.officialBusinessForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const officialBusiness = this.officialBusinessForm.value;

    this.service.create(officialBusiness).subscribe({
      next: (result) => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.dialog.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(
          err,
          'Failed to create official business.'
        );
      },
    });
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
