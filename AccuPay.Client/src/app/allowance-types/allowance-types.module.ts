import { NgModule } from '@angular/core';
import { AllowanceTypeFormComponent } from './allowance-type-form/allowance-type-form.component';
import { NewAllowanceTypeComponent } from './new-allowance-type/new-allowance-type.component';
import { EditAllowanceTypeComponent } from './edit-allowance-type/edit-allowance-type.component';
import { AllowanceTypeListComponent } from './allowance-type-list/allowance-type-list.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    AllowanceTypeFormComponent,
    NewAllowanceTypeComponent,
    EditAllowanceTypeComponent,
    AllowanceTypeListComponent,
  ],
  imports: [SharedModule],
})
export class AllowanceTypesModule {}
