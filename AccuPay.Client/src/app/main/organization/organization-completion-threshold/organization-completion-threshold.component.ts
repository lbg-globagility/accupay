import { Component, OnInit, Input } from '@angular/core';
import { Organization } from 'src/app/accounts/shared/organization';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { AccountService } from 'src/app/accounts/services/account.service';
import { TopbarAccountService } from '../../service/topbar-account.service';
import { CustomValidators } from 'src/app/core/forms/validators';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-organization-completion-threshold',
  templateUrl: './organization-completion-threshold.component.html',
  styleUrls: ['./organization-completion-threshold.component.scss']
})
export class OrganizationCompletionThresholdComponent implements OnInit {
  @Input()
  organization: Organization;

  group: FormGroup;
  formService: any;

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

    this.inProgressThresholdArray.patchValue(
      this.organization.inProgressThresholds
    );
  }

  get inProgressThresholdArray(): FormArray {
    return this.group.get('items') as FormArray;
  }

  saveCompletionThresholds() {
    this.accountService
      .updateOrganizationCompletionThresholds(this.group.value)
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
