import Swal from 'sweetalert2';
import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { OfficialBusinessService } from 'src/app/official-businesses/official-business.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LoadingState } from 'src/app/core/states/loading-state';
import { OfficialBusinessFormComponent } from '../official-business-form/official-business-form.component';

@Component({
  selector: 'app-edit-official-business',
  templateUrl: './edit-official-business.component.html',
  styleUrls: ['./edit-official-business.component.scss'],
})
export class EditOfficialBusinessComponent implements OnInit {
  @ViewChild(OfficialBusinessFormComponent)
  officialBusinessForm: OfficialBusinessFormComponent;

  officialBusiness: OfficialBusiness;

  officialBusinessId: number;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  savingState: LoadingState = new LoadingState();

  constructor(
    private officialBusinessService: OfficialBusinessService,
    private route: ActivatedRoute,
    private router: Router,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<EditOfficialBusinessComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.officialBusinessId = data.officialBusinessId;
  }

  ngOnInit(): void {
    this.loadOfficialBusiness();
  }

  onSave(officialBusiness: OfficialBusiness): void {
    if (!this.officialBusinessForm.form.valid) {
      return;
    }

    this.savingState.changeToLoading();

    this.officialBusinessService
      .update(officialBusiness, this.officialBusinessId)
      .subscribe(
        () => {
          this.savingState.changeToSuccess();
          this.displaySuccess();
          this.dialog.close(true);
        },
        (err) => {
          this.savingState.changeToError();
          this.errorHandler.badRequest(
            err,
            'Failed to update official business.'
          );
        }
      );
  }

  onCancel(): void {
    this.dialog.close();
  }

  private loadOfficialBusiness(): void {
    this.officialBusinessService
      .get(this.officialBusinessId)
      .subscribe((data) => {
        this.officialBusiness = data;

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
