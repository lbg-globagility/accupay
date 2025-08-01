import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MainModule } from 'src/app/main/main.module';
import { AccountsModule } from 'src/app/accounts/accounts.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgxPermissionsModule } from 'ngx-permissions';
import { AllowancesModule } from 'src/app/allowances/allowances.module';
import { EmployeesModule } from './employees/employees.module';
import { LeavesModule } from 'src/app/leaves/leaves.module';
import { LoansModule } from 'src/app/loans/loans.module';
import { OfficialBusinessesModule } from 'src/app/official-businesses/official-businesses.module';
import { OvertimesModule } from 'src/app/overtimes/overtimes.module';
import { ShiftsModule } from 'src/app/shifts/shifts.module';
import { SalariesModule } from 'src/app/salaries/salaries.module';
import { UsersModule } from 'src/app/users/users.module';
import { OrganizationsModule } from 'src/app/organizations/organizations.module';
import { AuthInterceptor } from 'src/app/core/auth/auth.interceptor';
import { BranchesModule } from 'src/app/branches/branches.module';
import { TimeLogsModule } from './time-logs/time-logs.module';
import { PositionsModule } from './positions/positions.module';
import { CalendarsModule } from 'src/app/calendars/calendars.module';
import { PayrollModule } from 'src/app/payroll/payroll.module';
import { DivisionsModule } from './divisions/divisions.module';
import { TimeEntryModule } from './time-entry/time-entry.module';
import { RolesModule } from 'src/app/roles/roles.module';
import { AllowanceTypesModule } from './allowance-types/allowance-types.module';
import { ReportsModule } from './reports/reports.module';
import { ClientsModule } from 'src/app/clients/clients.module';
import { LoanTypesModule } from './loan-types/loan-types.module';
import { EmploymentPoliciesModule } from 'src/app/employment-policies/employment-policies.module';
import { AccountService } from 'src/app/accounts/services/account.service';
import { ErrorsModule } from 'src/app/errors/errors.module';
import { catchError } from 'rxjs/operators';
import { SelfServiceModule } from './self-service/self-service.module';
import { AuthService } from 'src/app/core/auth';
import { forkJoin } from 'rxjs';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgxPermissionsModule.forRoot(),
    AccountsModule,
    MainModule,
    AllowancesModule,
    ClientsModule,
    EmployeesModule,
    ErrorsModule,
    LeavesModule,
    LoansModule,
    OfficialBusinessesModule,
    OvertimesModule,
    ShiftsModule,
    SalariesModule,
    UsersModule,
    OrganizationsModule,
    BranchesModule,
    CalendarsModule,
    TimeLogsModule,
    PositionsModule,
    PayrollModule,
    DivisionsModule,
    TimeEntryModule,
    EmploymentPoliciesModule,
    RolesModule,
    AllowanceTypesModule,
    ReportsModule,
    LoanTypesModule,
    SelfServiceModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: APP_INITIALIZER,
      useFactory: (as: AuthService) => () => {
        return forkJoin([as.getCurrentRole(), as.getAccount()])
          .pipe(catchError(() => Promise.resolve(false)))
          .toPromise();
      },
      deps: [AuthService],
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
