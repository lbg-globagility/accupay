import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from 'src/app/main/main.component';
import { PageNotFoundComponent } from 'src/app/errors/page-not-found/page-not-found.component';
import { UnderConstructionComponent } from 'src/app/errors/under-construction/under-construction.component';
import { LoginComponent } from 'src/app/accounts/login/login.component';
import { AuthGuard } from 'src/app/core/auth/auth.guard';
import { UserListComponent } from 'src/app/users/user-list/user-list.component';
import { ViewUserComponent } from 'src/app/users/view-user/view-user.component';
import { NewUserComponent } from 'src/app/users/new-user/new-user.component';
import { EditUserComponent } from 'src/app/users/edit-user/edit-user.component';
import { NewEmployeeComponent } from './employees/new-employee/new-employee.component';
import { ViewEmployeeComponent } from './employees/view-employee/view-employee.component';
import { EditEmployeeComponent } from './employees/edit-employee/edit-employee.component';
import { ViewSalaryComponent } from 'src/app/salaries/view-salary/view-salary.component';
import { NewSalaryComponent } from 'src/app/salaries/new-salary/new-salary.component';
import { EditSalaryComponent } from 'src/app/salaries/edit-salary/edit-salary.component';
import { LeaveListComponent } from 'src/app/leaves/leave-list/leave-list.component';
import { OfficialBusinessListComponent } from 'src/app/official-businesses/official-business-list/official-business-list.component';
import { OvertimeListComponent } from 'src/app/overtimes/overtime-list/overtime-list.component';
import { ViewOvertimeComponent } from 'src/app/overtimes/view-overtime/view-overtime.component';
import { NewOvertimeComponent } from 'src/app/overtimes/new-overtime/new-overtime.component';
import { EditOvertimeComponent } from 'src/app/overtimes/edit-overtime/edit-overtime.component';
import { ShiftListComponent } from 'src/app/shifts/shift-list/shift-list.component';
import { ViewShiftComponent } from 'src/app/shifts/view-shift/view-shift.component';
import { NewShiftComponent } from 'src/app/shifts/new-shift/new-shift.component';
import { EditShiftComponent } from 'src/app/shifts/edit-shift/edit-shift.component';
import { AllowanceListComponent } from 'src/app/allowances/allowance-list/allowance-list.component';
import { ViewAllowanceComponent } from 'src/app/allowances/view-allowance/view-allowance.component';
import { NewAllowanceComponent } from 'src/app/allowances/new-allowance/new-allowance.component';
import { EditAllowanceComponent } from 'src/app/allowances/edit-allowance/edit-allowance.component';
import { LoanListComponent } from 'src/app/loans/loan-list/loan-list.component';
import { ViewLoanComponent } from 'src/app/loans/view-loan/view-loan.component';
import { NewLoanComponent } from 'src/app/loans/new-loan/new-loan.component';
import { EditLoanComponent } from 'src/app/loans/edit-loan/edit-loan.component';
import { ReportsComponent } from 'src/app/reports/reports/reports.component';
import { OrganizationListComponent } from 'src/app/organizations/organization-list/organization-list.component';
import { NewOrganizationComponent } from 'src/app/organizations/new-organization/new-organization.component';
import { ViewOrganizationComponent } from 'src/app/organizations/view-organization/view-organization.component';
import { EditOrganizationComponent } from 'src/app/organizations/edit-organization/edit-organization.component';
import { RegisterComponent } from 'src/app/accounts/register/register.component';
import { NewBranchComponent } from 'src/app/branches/new-branch/new-branch.component';
import { EditBranchComponent } from 'src/app/branches/edit-branch/edit-branch.component';
import { BranchListComponent } from 'src/app/branches/branch-list/branch-list.component';
import { PositionListComponent } from './positions/position-list/position-list.component';
import { ViewPositionComponent } from 'src/app/positions/view-position/view-position.component';
import { NewPositionComponent } from 'src/app/positions/new-position/new-position.component';
import { EditPositionComponent } from 'src/app/positions/edit-position/edit-position.component';
import {
  ViewCalendarComponent,
  CalendarListComponent,
  NewCalendarComponent,
  EditCalendarComponent,
} from 'src/app/calendars/components';
import { DivisionListComponent } from 'src/app/divisions/division-list/division-list.component';
import { ViewDivisionComponent } from 'src/app/divisions/view-division/view-division.component';
import { NewDivisionComponent } from 'src/app/divisions/new-division/new-division.component';
import { EditDivisionComponent } from 'src/app/divisions/edit-division/edit-division.component';
import { PayrollComponent } from 'src/app/payroll/payroll/payroll.component';
import { ViewPayPeriodComponent } from 'src/app/payroll/view-payperiod/view-payperiod.component';
import { TimeEntryComponent } from 'src/app/time-entry/time-entry/time-entry.component';
import { TimeEntryDetailsComponent } from 'src/app/time-entry/time-entry-details/time-entry-details.component';
import { NewRoleComponent } from 'src/app/roles/new-role/new-role.component';
import { RoleListComponent } from 'src/app/roles/role-list/role-list.component';
import { EditRoleComponent } from 'src/app/roles/edit-role/edit-role.component';
import { UserRolesComponent } from 'src/app/roles/user-roles/user-roles.component';
import { LeaveBalanceComponent } from 'src/app/leaves/leave-balance/leave-balance.component';
import { TimeLogsComponent } from 'src/app/time-logs/time-logs/time-logs.component';
import { AllowanceTypeListComponent } from './allowance-types/allowance-type-list/allowance-type-list.component';
import { SalariesComponent } from 'src/app/salaries/salaries/salaries.component';
import { LeavesComponent } from 'src/app/leaves/leaves/leaves.component';
import { EmployeesComponent } from 'src/app/employees/employees/employees.component';
import { AllowancesComponent } from 'src/app/allowances/allowances/allowances.component';
import { SecurityComponent } from 'src/app/users/security/security.component';
import {
  NewClientComponent,
  ClientsComponent,
  ViewClientComponent,
  EditClientComponent,
} from 'src/app/clients/components';
import { SssReportComponent } from 'src/app/reports/sss-report/sss-report.component';
import { PhilhealthReportComponent } from './reports/philhealth-report/philhealth-report.component';
import { PagibigReportComponent } from './reports/pagibig-report/pagibig-report.component';
import { LoanReportBytypeComponent } from './reports/loan-report-bytype/loan-report-bytype.component';
import { LoanReportByemployeeComponent } from './reports/loan-report-byemployee/loan-report-byemployee.component';
import { TaxReportComponent } from './reports/tax-report/tax-report.component';
import {
  EditEmploymentPolicyComponent,
  NewEmploymentPolicyComponent,
  EmploymentPolicyListComponent,
  ViewEmploymentPolicyComponent,
} from 'src/app/employment-policies/components';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: 'clients/new',
        component: NewClientComponent,
      },
      {
        path: 'clients',
        component: ClientsComponent,
        children: [
          {
            path: ':id',
            component: ViewClientComponent,
          },
        ],
      },
      {
        path: 'clients/:id/edit',
        component: EditClientComponent,
      },
      {
        path: 'employees/new',
        component: NewEmployeeComponent,
      },
      {
        path: 'employees/:id/edit',
        component: EditEmployeeComponent,
      },
      {
        path: 'employment-policies',
        component: EmploymentPolicyListComponent,
      },
      {
        path: 'employment-policies/new',
        component: NewEmploymentPolicyComponent,
      },
      {
        path: 'employment-policies/:id',
        component: ViewEmploymentPolicyComponent,
      },
      {
        path: 'employment-policies/:id/edit',
        component: EditEmploymentPolicyComponent,
      },
      {
        path: 'users',
        component: UserListComponent,
      },
      {
        path: 'employees',
        component: EmployeesComponent,
        children: [
          {
            path: ':id',
            component: ViewEmployeeComponent,
          },
        ],
      },

      {
        path: 'salaries/new',
        component: NewSalaryComponent,
      },
      {
        path: 'salaries',
        component: SalariesComponent,
        children: [
          {
            path: ':employeeId',
            component: ViewSalaryComponent,
          },
        ],
      },
      {
        path: 'salaries/:id/edit',
        component: EditSalaryComponent,
      },
      {
        path: 'leaves',
        component: LeavesComponent,
        children: [
          {
            path: '',
            redirectTo: 'overview',
            pathMatch: 'full',
          },
          {
            path: 'overview',
            component: LeaveListComponent,
          },
          {
            path: 'balances',
            component: LeaveBalanceComponent,
          },
        ],
      },
      {
        path: 'official-businesses',
        component: OfficialBusinessListComponent,
      },
      {
        path: 'overtimes',
        component: OvertimeListComponent,
      },
      {
        path: 'overtimes/new',
        component: NewOvertimeComponent,
      },
      {
        path: 'overtimes/:id',
        component: ViewOvertimeComponent,
      },
      {
        path: 'overtimes/:id/edit',
        component: EditOvertimeComponent,
      },
      {
        path: 'time-entry',
        component: TimeEntryComponent,
      },
      {
        path: 'time-entry/:id',
        component: TimeEntryDetailsComponent,
      },
      {
        path: 'shifts',
        component: ShiftListComponent,
      },
      {
        path: 'shifts/new',
        component: NewShiftComponent,
      },
      {
        path: 'shifts/:id',
        component: ViewShiftComponent,
      },
      {
        path: 'shifts/:id/edit',
        component: EditShiftComponent,
      },
      {
        path: 'allowances',
        component: AllowancesComponent,
        children: [
          {
            path: '',
            redirectTo: 'overview',
            pathMatch: 'full',
          },
          {
            path: 'overview',
            component: AllowanceListComponent,
          },
          {
            path: 'types',
            component: AllowanceTypeListComponent,
          },
        ],
      },
      {
        path: 'allowances/new',
        component: NewAllowanceComponent,
      },
      {
        path: 'allowances/:id',
        component: ViewAllowanceComponent,
      },
      {
        path: 'allowances/:id/edit',
        component: EditAllowanceComponent,
      },
      {
        path: 'allowance-types',
        component: AllowanceTypeListComponent,
      },
      {
        path: 'loans',
        component: LoanListComponent,
      },
      {
        path: 'loans/new',
        component: NewLoanComponent,
      },
      {
        path: 'loans/:id',
        component: ViewLoanComponent,
      },
      {
        path: 'loans/:id/edit',
        component: EditLoanComponent,
      },

      {
        path: 'reports',
        component: ReportsComponent,
        children: [
          {
            path: 'sss',
            component: SssReportComponent,
          },
          {
            path: 'philhealth',
            component: PhilhealthReportComponent,
          },
          {
            path: 'pagibig',
            component: PagibigReportComponent,
          },
          {
            path: 'loanbytype',
            component: LoanReportBytypeComponent,
          },
          {
            path: 'loanbyemployee',
            component: LoanReportByemployeeComponent,
          },
          {
            path: 'tax',
            component: TaxReportComponent,
          },
        ],
      },
      {
        path: 'organizations',
        component: OrganizationListComponent,
      },
      {
        path: 'organizations/new',
        component: NewOrganizationComponent,
      },
      {
        path: 'organizations/:id',
        component: ViewOrganizationComponent,
      },
      {
        path: 'organizations/:id/edit',
        component: EditOrganizationComponent,
      },
      {
        path: 'branches',
        component: BranchListComponent,
      },
      {
        path: 'branches/new',
        component: NewBranchComponent,
      },
      {
        path: 'branches/:id/edit',
        component: EditBranchComponent,
      },
      {
        path: 'time-logs',
        component: TimeLogsComponent,
      },
      {
        path: 'divisions',
        component: DivisionListComponent,
      },
      {
        path: 'divisions/new',
        component: NewDivisionComponent,
      },
      {
        path: 'divisions/:id',
        component: ViewDivisionComponent,
      },
      {
        path: 'divisions/:id/edit',
        component: EditDivisionComponent,
      },
      {
        path: 'positions',
        component: PositionListComponent,
      },
      {
        path: 'positions/new',
        component: NewPositionComponent,
      },
      {
        path: 'positions/:id',
        component: ViewPositionComponent,
      },
      {
        path: 'positions/:id/edit',
        component: EditPositionComponent,
      },
      {
        path: 'calendars',
        component: CalendarListComponent,
      },
      {
        path: 'calendars/new',
        component: NewCalendarComponent,
      },
      {
        path: 'calendars/:id',
        component: ViewCalendarComponent,
      },
      {
        path: 'calendars/:id/edit',
        component: EditCalendarComponent,
      },
      {
        path: 'payroll',
        component: PayrollComponent,
      },
      {
        path: 'payroll/:id',
        component: ViewPayPeriodComponent,
      },
      {
        path: 'security',
        component: SecurityComponent,
        children: [
          {
            path: '',
            redirectTo: 'users',
            pathMatch: 'full',
          },
          {
            path: 'users',
            component: UserListComponent,
          },
          {
            path: 'roles',
            component: RoleListComponent,
          },
          {
            path: 'user-access',
            component: UserRolesComponent,
          },
        ],
      },
      {
        path: 'users/new',
        component: NewUserComponent,
      },
      {
        path: 'users/:id',
        component: ViewUserComponent,
      },
      {
        path: 'users/:id/edit',
        component: EditUserComponent,
      },
      {
        path: 'roles/new',
        component: NewRoleComponent,
      },
      {
        path: 'roles/:id/edit',
        component: EditRoleComponent,
      },
    ],
    canActivate: [AuthGuard],
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
