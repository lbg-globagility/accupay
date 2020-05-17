import { EditMyAccountComponent } from 'src/app/accounts/edit-my-account/edit-my-account.component';
import { MyAccountComponent } from 'src/app/accounts/my-account/my-account.component';
import { NgModule } from '@angular/core';
import { OauthComponent } from './oauth/oauth.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { TimeoutComponent } from './timeout/timeout.component';

@NgModule({
  declarations: [
    EditMyAccountComponent,
    MyAccountComponent,
    TimeoutComponent,
    OauthComponent
  ],
  imports: [SharedModule]
})
export class AccountsModule {}
