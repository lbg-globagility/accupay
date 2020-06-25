import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { ViewSalaryComponent } from './view-salary/view-salary.component';
import { ConfirmationDialogComponent } from '../shared/components/confirmation-dialog/confirmation-dialog.component';
import { EditSalaryComponent } from './edit-salary/edit-salary.component';
import { SalaryFormComponent } from './salary-form/salary-form.component';
import { NewSalaryComponent } from './new-salary/new-salary.component';
import { SalariesComponent } from './salaries/salaries.component';

@NgModule({
  declarations: [
    ViewSalaryComponent,
    ConfirmationDialogComponent,
    EditSalaryComponent,
    SalaryFormComponent,
    NewSalaryComponent,
    SalariesComponent,
  ],
  imports: [SharedModule],
})
export class SalariesModule {}
