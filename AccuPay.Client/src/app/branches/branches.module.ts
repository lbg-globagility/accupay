import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { NewBranchComponent } from './new-branch/new-branch.component';
import { BranchFormComponent } from './branch-form/branch-form.component';
import { EditBranchComponent } from './edit-branch/edit-branch.component';
import { BranchListComponent } from './branch-list/branch-list.component';

@NgModule({
  declarations: [NewBranchComponent, BranchFormComponent, EditBranchComponent, BranchListComponent],
  imports: [SharedModule],
})
export class BranchesModule {}
