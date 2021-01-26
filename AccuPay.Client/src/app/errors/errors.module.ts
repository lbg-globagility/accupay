import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { PageNotFoundComponent } from 'src/app/errors/page-not-found/page-not-found.component';
import { UnderConstructionComponent } from './under-construction/under-construction.component';
import { NotAuthorizedComponent } from './not-authorized/not-authorized.component';

@NgModule({
  declarations: [
    PageNotFoundComponent,
    UnderConstructionComponent,
    NotAuthorizedComponent,
  ],
  imports: [SharedModule],
})
export class ErrorsModule {}
