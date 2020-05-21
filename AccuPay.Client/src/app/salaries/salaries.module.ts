import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SalaryListComponent } from './salary-list/salary-list.component';
import { ViewSalaryComponent } from './view-salary/view-salary.component';
import { ConfirmationDialogComponent } from '../shared/components/confirmation-dialog/confirmation-dialog.component';
import { EditSalaryComponent } from './edit-salary/edit-salary.component';
import { SalaryFormComponent } from './salary-form/salary-form.component';
import { NewSalaryComponent } from './new-salary/new-salary.component';

@NgModule({
  declarations: [
    SalaryListComponent,
    ViewSalaryComponent,
    ConfirmationDialogComponent,
    EditSalaryComponent,
    SalaryFormComponent,
    NewSalaryComponent,
  ],
  imports: [SharedModule],
})
export class SalariesModule {}
