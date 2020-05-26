import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { TimeoutComponent } from './timeout/timeout.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

@NgModule({
  declarations: [TimeoutComponent, LoginComponent, RegisterComponent],
  imports: [SharedModule],
})
export class AccountsModule {}
