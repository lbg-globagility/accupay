import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { TimeoutComponent } from './timeout/timeout.component';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [TimeoutComponent, LoginComponent],
  imports: [SharedModule],
})
export class AccountsModule {}
