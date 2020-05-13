import Swal from 'sweetalert2';
import { AccountService } from 'src/app/accounts/services/account.service';
import { Component, OnInit } from '@angular/core';
import { FileProgress } from 'src/app/files/shared/file-progress';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { Organization } from 'src/app/accounts/shared/organization';
import { OrganizationFormService } from 'src/app/main/organization/organization-form.service';
import { TopbarAccountService } from '../service/topbar-account.service';

export class FileUpload {
  progress: Observable<FileProgress<Organization>>;

  isFailed() {
    // return this.progress.status === 'Failed';
  }

  isSuccess() {
    // return this.progress.status === 'Success';
  }
}

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss'],
  providers: [OrganizationFormService]
})
export class OrganizationComponent implements OnInit {
  organization: Organization;

  progress: Observable<FileProgress<Organization>>;

  hide = true;

  currentProgress: FileProgress<Organization> = new FileProgress(
    'Not Started',
    0
  );

  get form(): FormGroup {
    return this.formService.form;
  }

  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private topbarAccountService: TopbarAccountService,
    private formService: OrganizationFormService
  ) {}

  ngOnInit(): void {
    this.accountService.getOrganization().subscribe(organization => {
      this.organization = organization;

      this.formService.patchValue(organization);

      for (const domain of this.organization.domains) {
        this.domains.push(this.createDomain(domain));
      }
    });
  }

  isInProgress() {
    return !(
      this.currentProgress.isFailed() || this.currentProgress.isSuccess()
    );
  }

  get domains(): FormArray {
    return this.form.get('domains') as FormArray;
  }

  saveInfo(image: File): void {
    const organizationId = this.organization.id as any;
    const progress = this.accountService.uploadOrganization(
      image,
      organizationId
    );

    progress.subscribe(
      response => {
        if (response.isSuccess()) {
          const { body: account } = response;

          this.form.get('id').setValue(organizationId);
          this.form.get('image').setValue(account.image);
        }

        this.hide = false;
        this.currentProgress = response;
      },
      err => {},
      () => {
        if (this.currentProgress.isSuccess()) {
          this.accountService
            .updateOrganization(this.formService.value)
            .subscribe(() => {
              this.topbarAccountService.setData();
              setTimeout(() => {
                this.showSuccessDialog();
              });
            });
        }
      }
    );
  }

  saveDomains() {
    this.accountService
      .updateOrganizationDomains(this.formService.value)
      .subscribe(() => {
        this.topbarAccountService.setData();
        setTimeout(() => {
          this.showSuccessDialog();
        });
      });
  }

  saveAcknowledgements() {
    this.accountService
      .updateOrganizationAcknowledgments(this.formService.value)
      .subscribe(() => {
        this.topbarAccountService.setData();
        setTimeout(() => {
          this.showSuccessDialog();
        });
      });
  }

  saveCompletionThresholds() {
    this.accountService
      .updateOrganizationCompletionThresholds(this.formService.value)
      .subscribe(() => {
        this.topbarAccountService.setData();
        setTimeout(() => {
          this.showSuccessDialog();
        });
      });
  }

  private createDomain(domain: any) {
    return this.fb.group({
      id: [domain.id],
      name: [domain.name]
    });
  }

  private showSuccessDialog() {
    Swal.fire({
      title: 'Complete',
      text: 'Successfully saved!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false
    });
  }
}
