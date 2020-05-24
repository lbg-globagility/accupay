import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { AllowanceListComponent } from './allowance-list/allowance-list.component';
import { ViewAllowanceComponent } from './view-allowance/view-allowance.component';
import { NewAllowanceComponent } from './new-allowance/new-allowance.component';
import { EditAllowanceComponent } from './edit-allowance/edit-allowance.component';
import { AllowanceFormComponent } from './allowance-form/allowance-form.component';

@NgModule({
  declarations: [AllowanceListComponent, ViewAllowanceComponent, NewAllowanceComponent, EditAllowanceComponent, AllowanceFormComponent],
  imports: [SharedModule],
})
export class AllowancesModule {}
