import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SalaryListComponent } from './salary-list/salary-list.component';
import { ViewSalaryComponent } from './view-salary/view-salary.component';



@NgModule({
  declarations: [SalaryListComponent, ViewSalaryComponent],
  imports: [SharedModule]
})
export class SalariesModule { }
