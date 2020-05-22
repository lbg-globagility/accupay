import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoanListComponent } from './loan-list/loan-list.component';
import { ViewLoanComponent } from './view-loan/view-loan.component';

@NgModule({
  declarations: [LoanListComponent, ViewLoanComponent],
  imports: [SharedModule],
})
export class LoansModule {}
