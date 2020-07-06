import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { EmploymentPolicyService } from 'src/app/employment-policies/services/employment-policy.service';
import { EmploymentPolicyFormComponent } from 'src/app/employment-policies/components/employment-policy-form/employment-policy-form.component';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-new-employment-policy',
  templateUrl: './new-employment-policy.component.html',
  styleUrls: ['./new-employment-policy.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewEmploymentPolicyComponent implements OnInit {
  @ViewChild(EmploymentPolicyFormComponent)
  form: EmploymentPolicyFormComponent;

  constructor(
    private employmentPolicyService: EmploymentPolicyService,
    private errorHandler: ErrorHandler,
    private router: Router
  ) {}

  ngOnInit(): void {}

  save(): void {
    if (!this.form.valid) {
      return;
    }

    const value = this.form.value;

    this.employmentPolicyService.create(value).subscribe({
      next: () => {
        this.displaySuccess();
        this.router.navigate(['employment-policies']);
      },
      error: (err) => {
        this.errorHandler.badRequest(err, 'Failed to create employment policy');
      },
    });
  }

  cancel(): void {
    this.router.navigate(['employment-policies']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new leave!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
