import { Component, OnInit, Input } from '@angular/core';
import { Organization } from 'src/app/accounts/shared/organization';
import { AccountService } from 'src/app/accounts/services/account.service';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { TopbarAccountService } from '../../service/topbar-account.service';
import { OrganizationFormService } from '../organization-form.service';
import Swal from 'sweetalert2';
import { CustomValidators } from 'src/app/core/forms/validators';

@Component({
  selector: 'app-organization-acknowledgement-thresholds',
  templateUrl: './organization-acknowledgement-thresholds.component.html',
  styleUrls: ['./organization-acknowledgement-thresholds.component.scss']
})
export class OrganizationAcknowledgementThresholdsComponent implements OnInit {
  @Input()
  organization: Organization;

  group: FormGroup;

  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private topbarAccountService: TopbarAccountService
  ) {}

  ngOnInit() {
    this.group = this.fb.group({
      items: this.fb.array([
        this.fb.group({
          classification: ['Class I'],
          first: [],
          second: [
            '',
            [Validators.required, CustomValidators.moreThan('first', 'second')]
          ]
        }),
        this.fb.group({
          classification: ['Class II'],
          first: [],
          second: [
            '',
            [Validators.required, CustomValidators.moreThan('first', 'second')]
          ]
        }),
        this.fb.group({
          classification: ['Class III'],
          first: [],
          second: [
            '',
            [Validators.required, CustomValidators.moreThan('first', 'second')]
          ]
        })
      ])
    });

    this.acknowledgmentThresholdArray.patchValue(
      this.organization.acknowledgmentThresholds
    );
  }

  get acknowledgmentThresholdArray(): FormArray {
    return this.group.get('items') as FormArray;
  }

  saveAcknowledgements() {
    this.accountService
      .updateOrganizationAcknowledgments(this.group.value)
      .subscribe(() => {
        this.topbarAccountService.setData();
        setTimeout(() => {
          this.showSuccessDialog();
        });
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
