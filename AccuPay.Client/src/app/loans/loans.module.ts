import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoanListComponent } from './loan-list/loan-list.component';
import { ViewLoanComponent } from './view-loan/view-loan.component';
import { NewLoanComponent } from './new-loan/new-loan.component';
import { EditLoanComponent } from './edit-loan/edit-loan.component';
import { LoanFormComponent } from './loan-form/loan-form.component';
import { LoanHistoryComponent } from './loan-history/loan-history.component';
import { LoansComponent } from './loans/loans.component';
import { ViewLoanDialogComponent } from './view-loan-dialog/view-loan-dialog.component';

@NgModule({
  declarations: [
    LoanListComponent,
    ViewLoanComponent,
    NewLoanComponent,
    EditLoanComponent,
    LoanFormComponent,
    LoanHistoryComponent,
    LoansComponent,
    ViewLoanDialogComponent,
  ],
  imports: [SharedModule],
})
export class LoansModule {}
