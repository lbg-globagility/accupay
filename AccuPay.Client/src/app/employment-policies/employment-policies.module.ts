import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { EmploymentPolicyListComponent } from './employment-policy-list/employment-policy-list.component';
import { NewEmploymentPolicyComponent } from './new-employment-policy/new-employment-policy.component';

@NgModule({
  declarations: [EmploymentPolicyListComponent, NewEmploymentPolicyComponent],
  imports: [SharedModule],
})
export class EmploymentPoliciesModule {}
