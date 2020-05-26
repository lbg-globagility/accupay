import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { NewBranchComponent } from './new-branch/new-branch.component';
import { BranchFormComponent } from './branch-form/branch-form.component';

@NgModule({
  declarations: [NewBranchComponent, BranchFormComponent],
  imports: [SharedModule],
})
export class BranchesModule {}
