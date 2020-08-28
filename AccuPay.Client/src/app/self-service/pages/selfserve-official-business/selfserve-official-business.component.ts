import { Component, OnInit, ViewChild } from '@angular/core';
import { LoadingState } from 'src/app/core/states/loading-state';
import { SelfserveService } from '../../services/selfserve.service';
import { MatDialogRef } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { SelfserveOfficialBusinessFormComponent } from '../../components/selfserve-official-business-form/selfserve-official-business-form.component';

@Component({
  selector: 'app-selfserve-official-business',
  templateUrl: './selfserve-official-business.component.html',
  styleUrls: ['./selfserve-official-business.component.scss'],
})
export class SelfserveOfficialBusinessComponent implements OnInit {
  @ViewChild(SelfserveOfficialBusinessFormComponent)
  officialBusinessForm: SelfserveOfficialBusinessFormComponent;

  officialBusiness: OfficialBusiness;

  savingState: LoadingState = new LoadingState();

  constructor(
    private service: SelfserveService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<SelfserveOfficialBusinessComponent>
  ) {}

  ngOnInit(): void {}

  onSave(): void {
    if (!this.officialBusinessForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const officialBusiness = this.officialBusinessForm.value;

    this.service.createOfficialBusiness(officialBusiness).subscribe({
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
