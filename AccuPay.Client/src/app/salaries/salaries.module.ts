import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SalaryListComponent } from './salary-list/salary-list.component';



@NgModule({
  declarations: [SalaryListComponent],
  imports: [SharedModule]
})
export class SalariesModule { }
