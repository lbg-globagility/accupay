import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MainModule } from 'src/app/main/main.module';
import { AccountsModule } from 'src/app/accounts/accounts.module';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AccountsModule,
    MainModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
