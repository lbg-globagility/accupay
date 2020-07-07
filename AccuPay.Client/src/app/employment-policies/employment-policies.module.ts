import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import {
  EmploymentPolicyListComponent,
  EmploymentPolicyFormComponent,
  NewEmploymentPolicyComponent,
  EditEmploymentPolicyComponent,
} from 'src/app/employment-policies/components';
import { ViewEmploymentPolicyComponent } from './components/view-employment-policy/view-employment-policy.component';

@NgModule({
  declarations: [
    EmploymentPolicyListComponent,
    NewEmploymentPolicyComponent,
    EditEmploymentPolicyComponent,
    EmploymentPolicyFormComponent,
    ViewEmploymentPolicyComponent,
  ],
  imports: [SharedModule],
})
export class EmploymentPoliciesModule {}
