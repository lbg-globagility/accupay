import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import {
  EmploymentPolicyListComponent,
  EmploymentPolicyFormComponent,
  NewEmploymentPolicyComponent,
  EditEmploymentPolicyComponent,
} from 'src/app/employment-policies/components';

@NgModule({
  declarations: [
    EmploymentPolicyListComponent,
    NewEmploymentPolicyComponent,
    EditEmploymentPolicyComponent,
    EmploymentPolicyFormComponent,
  ],
  imports: [SharedModule],
})
export class EmploymentPoliciesModule {}
