import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SalaryListComponent } from './salary-list/salary-list.component';
import { ViewSalaryComponent } from './view-salary/view-salary.component';
import { DeleteSalaryConfirmationComponent } from './components/delete-salary-confirmation/delete-salary-confirmation.component';
import { EditSalaryComponent } from './edit-salary/edit-salary.component';
import { SalaryFormComponent } from './components/salary-form/salary-form.component';



@NgModule({
  declarations: [SalaryListComponent, ViewSalaryComponent, DeleteSalaryConfirmationComponent, EditSalaryComponent, SalaryFormComponent],
  imports: [SharedModule]
})
export class SalariesModule { }
