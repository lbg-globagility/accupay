import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { AllowanceListComponent } from './allowance-list/allowance-list.component';



@NgModule({
  declarations: [AllowanceListComponent],
  imports: [SharedModule]
})
export class AllowancesModule { }
