import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmploymentPolicyService } from 'src/app/employment-policies/services/employment-policy.service';
import { EmploymentPolicy } from 'src/app/employment-policies/shared';
import { EmploymentPolicyFormComponent } from 'src/app/employment-policies/components/employment-policy-form/employment-policy-form.component';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-edit-employment-policy',
  templateUrl: './edit-employment-policy.component.html',
  styleUrls: ['./edit-employment-policy.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class EditEmploymentPolicyComponent implements OnInit {
  @ViewChild(EmploymentPolicyFormComponent)
  form: EmploymentPolicyFormComponent;

  employmentPolicyId: number = +this.route.snapshot.paramMap.get('id');

  employmentPolicy: EmploymentPolicy;

  constructor(
    private employmentPolicyService: EmploymentPolicyService,
    private errorHandler: ErrorHandler,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadEmploymentPolicy();
  }

  save() {
    if (!this.form.valid) {
      return;
    }

    const value = this.form.value;
    this.employmentPolicyService
      .update(this.employmentPolicyId, value)
      .subscribe({
        next: () => {
          this.displaySuccess();
          this.router.navigate([
            'employment-policies',
            this.employmentPolicyId,
          ]);
        },
        error: (err) =>
          this.errorHandler.badRequest(
            err,
            'Failed to update employment policy'
          ),
      });
  }

  cancel() {
    this.router.navigate(['employment-policies']);
  }

  private loadEmploymentPolicy() {
    this.employmentPolicyService
      .getById(this.employmentPolicyId)
      .subscribe(
        (employmentPolicy) => (this.employmentPolicy = employmentPolicy)
      );
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      // text: 'Updated employment policy',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
