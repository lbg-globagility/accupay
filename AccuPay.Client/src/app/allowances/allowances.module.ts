import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { AllowanceListComponent } from './allowance-list/allowance-list.component';
import { ViewAllowanceComponent } from './view-allowance/view-allowance.component';

@NgModule({
  declarations: [AllowanceListComponent, ViewAllowanceComponent],
  imports: [SharedModule],
})
export class AllowancesModule {}
