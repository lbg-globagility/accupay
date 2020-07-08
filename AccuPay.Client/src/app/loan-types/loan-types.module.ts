import { NgModule } from '@angular/core';
import { LoanTypeFormComponent } from './loan-type-form/loan-type-form.component';
import { LoanTypeListComponent } from './loan-type-list/loan-type-list.component';
import { EditLoanTypeComponent } from './edit-loan-type/edit-loan-type.component';
import { NewLoanTypeComponent } from './new-loan-type/new-loan-type.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    LoanTypeFormComponent,
    LoanTypeListComponent,
    EditLoanTypeComponent,
    NewLoanTypeComponent,
  ],
  imports: [SharedModule],
})
export class LoanTypesModule {}
