import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoanListComponent } from './loan-list/loan-list.component';



@NgModule({
  declarations: [LoanListComponent],
  imports: [SharedModule]
})
export class LoansModule { }
