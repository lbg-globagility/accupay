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
import { ShiftListComponent } from 'src/app/shifts/shift-list/shift-list.component';
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
import { PayrollSummaryComponent } from './reports/payroll-summary/payroll-summary.component';
import { PagibigReportComponent } from './reports/pagibig-report/pagibig-report.component';
import { PhilhealthReportComponent } from './reports/philhealth-report/philhealth-report.component';
import { SssReportComponent } from 'src/app/reports/sss-report/sss-report.component';
import { TaxReportComponent } from './reports/tax-report/tax-report.component';
import { ThirteenthMonthReportComponent } from './reports/thirteenth-month-report/thirteenth-month-report.component';
import { LoanReportBytypeComponent } from './reports/loan-report-bytype/loan-report-bytype.component';
import { LoanReportByemployeeComponent } from './reports/loan-report-byemployee/loan-report-byemployee.component';
import { LoanTypeListComponent } from './loan-types/loan-type-list/loan-type-list.component';
import {
  EditEmploymentPolicyComponent,
  NewEmploymentPolicyComponent,
  EmploymentPolicyListComponent,
  ViewEmploymentPolicyComponent,
} from 'src/app/employment-policies/components';
import { LoansComponent } from './loans/loans/loans.component';
import { PositionsComponent } from 'src/app/positions/components/positions/positions.component';
import { PermissionGuard, PermissionTypes } from 'src/app/core/auth';
import { NotAuthorizedComponent } from 'src/app/errors/not-authorized/not-authorized.component';
import { SelfServeComponent } from './self-service/self-serve/self-serve.component';
import { SelfServiceTimesheetsComponent } from 'src/app/self-service/components/self-service-timesheets/self-service-timesheets.component';
import { SelfserviceLeavesComponent } from 'src/app/self-service/leaves/components';
import { SelfserviceOvertimesComponent } from 'src/app/self-service/overtimes/components';
import { SelfserviceOfficialBusinessesComponent } from 'src/app/self-service/official-businesses/components';
import { SelfserviceDashboardComponent } from 'src/app/self-service/dashboard/components/selfservice-dashboard/selfservice-dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: 'clients',
        children: [
          {
            path: 'new',
            component: NewClientComponent,
          },
          {
            path: '',
            component: ClientsComponent,
            children: [
              {
                path: ':id',
                component: ViewClientComponent,
              },
            ],
          },
          {
            path: ':id/edit',
            component: EditClientComponent,
          },
        ],
      },
      {
        path: 'employees',
        data: { permission: PermissionTypes.EmployeeRead },
        children: [
          {
            path: 'new',
            component: NewEmployeeComponent,
            data: { permission: PermissionTypes.EmployeeCreate },
          },
          {
            path: '',
            component: EmployeesComponent,
            children: [
              {
                path: ':id',
                component: ViewEmployeeComponent,
              },
            ],
          },
          {
            path: ':id/edit',
            component: EditEmployeeComponent,
            data: { permission: PermissionTypes.EmployeeUpdate },
          },
        ],
      },
      {
        path: 'employment-policies',
        data: { permission: PermissionTypes.EmploymentPolicyRead },
        children: [
          {
            path: 'new',
            component: NewEmploymentPolicyComponent,
            data: { permission: PermissionTypes.EmploymentPolicyCreate },
          },
          {
            path: '',
            component: EmploymentPolicyListComponent,
          },
          {
            path: ':id',
            component: ViewEmploymentPolicyComponent,
          },
          {
            path: ':id/edit',
            component: EditEmploymentPolicyComponent,
            data: { permission: PermissionTypes.EmploymentPolicyUpdate },
          },
        ],
      },
      {
        path: 'salaries',
        data: { permission: PermissionTypes.SalaryRead },
        children: [
          {
            path: 'new',
            component: NewSalaryComponent,
            data: { permission: PermissionTypes.SalaryCreate },
          },
          {
            path: '',
            component: SalariesComponent,
            children: [
              {
                path: ':employeeId',
                component: ViewSalaryComponent,
              },
            ],
          },
          {
            path: ':id/edit',
            component: EditSalaryComponent,
            data: { permission: PermissionTypes.SalaryUpdate },
          },
        ],
      },
      {
        path: 'leaves',
        component: LeavesComponent,
        data: { permission: PermissionTypes.LeaveRead },
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
        data: { permission: PermissionTypes.OfficialBusinessRead },
      },
      {
        path: 'overtimes',
        component: OvertimeListComponent,
        data: { permission: PermissionTypes.OvertimeRead },
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
        data: { permission: PermissionTypes.ShiftRead },
      },
      {
        path: 'allowances',
        data: { permission: PermissionTypes.AllowanceRead },
        children: [
          {
            path: 'new',
            component: NewAllowanceComponent,
            data: { permission: PermissionTypes.AllowanceCreate },
          },
          {
            path: '',
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
            path: ':id',
            component: ViewAllowanceComponent,
          },
          {
            path: ':id/edit',
            component: EditAllowanceComponent,
            data: { permission: PermissionTypes.AllowanceUpdate },
          },
        ],
      },
      {
        path: 'loans',
        data: { permission: PermissionTypes.LoanRead },
        children: [
          {
            path: 'new',
            component: NewLoanComponent,
            data: { permission: PermissionTypes.LoanCreate },
          },
          {
            path: '',
            component: LoansComponent,
            children: [
              {
                path: '',
                redirectTo: 'overview',
                pathMatch: 'full',
              },
              {
                path: 'overview',
                component: LoanListComponent,
              },
              {
                path: 'types',
                component: LoanTypeListComponent,
              },
            ],
          },
          {
            path: ':id',
            component: ViewLoanComponent,
          },
          {
            path: ':id/edit',
            component: EditLoanComponent,
            data: { permission: PermissionTypes.LoanUpdate },
          },
        ],
      },
      {
        path: 'reports',
        component: ReportsComponent,
        children: [
          {
            path: 'payroll-summary',
            component: PayrollSummaryComponent,
          },
          {
            path: 'pagibig',
            component: PagibigReportComponent,
          },
          {
            path: 'philhealth',
            component: PhilhealthReportComponent,
          },
          {
            path: 'sss',
            component: SssReportComponent,
          },
          {
            path: 'tax',
            component: TaxReportComponent,
          },
          {
            path: '13th-month',
            component: ThirteenthMonthReportComponent,
          },
          {
            path: 'loans-by-type',
            component: LoanReportBytypeComponent,
          },
          {
            path: 'loans-by-employee',
            component: LoanReportByemployeeComponent,
          },
        ],
      },
      {
        path: 'organizations',
        data: { permission: PermissionTypes.OrganizationRead },
        children: [
          {
            path: '',
            component: OrganizationListComponent,
          },
          {
            path: 'new',
            component: NewOrganizationComponent,
            data: { permission: PermissionTypes.OrganizationCreate },
          },
          {
            path: ':id',
            component: ViewOrganizationComponent,
          },
          {
            path: ':id/edit',
            component: EditOrganizationComponent,
            data: { permission: PermissionTypes.OrganizationUpdate },
          },
        ],
      },
      {
        path: 'branches',
        data: { permission: PermissionTypes.BranchRead },
        children: [
          {
            path: '',
            component: BranchListComponent,
          },
          {
            path: 'new',
            component: NewBranchComponent,
            data: { permission: PermissionTypes.BranchCreate },
          },
          {
            path: ':id/edit',
            component: EditBranchComponent,
            data: { permission: PermissionTypes.BranchUpdate },
          },
        ],
      },
      {
        path: 'calendars',
        data: { permission: PermissionTypes.CalendarRead },
        children: [
          {
            path: '',
            component: CalendarListComponent,
          },
          {
            path: 'new',
            component: NewCalendarComponent,
            data: { permission: PermissionTypes.CalendarCreate },
          },
          {
            path: ':id',
            component: ViewCalendarComponent,
          },
          {
            path: ':id/edit',
            component: EditCalendarComponent,
            data: { permission: PermissionTypes.CalendarUpdate },
          },
        ],
      },
      {
        path: 'time-logs',
        component: TimeLogsComponent,
      },
      {
        path: 'positions',
        data: { permission: PermissionTypes.PositionRead },
        children: [
          {
            path: 'new',
            component: NewPositionComponent,
            data: { permission: PermissionTypes.PositionCreate },
          },
          {
            path: '',
            component: PositionsComponent,
            children: [
              {
                path: 'divisions/:id',
                component: ViewDivisionComponent,
              },
              {
                path: ':id',
                component: ViewPositionComponent,
              },
            ],
          },
          {
            path: ':id/edit',
            component: EditPositionComponent,
            data: { permission: PermissionTypes.PositionUpdate },
          },
        ],
      },
      {
        path: 'divisions/new',
        component: NewDivisionComponent,
        data: { permission: PermissionTypes.DivisionCreate },
      },
      {
        path: 'divisions/:id/edit',
        component: EditDivisionComponent,
        data: { permission: PermissionTypes.DivisionUpdate },
      },
      {
        path: 'payroll',
        data: { permission: PermissionTypes.PayPeriodRead },
        children: [
          {
            path: '',
            component: PayrollComponent,
          },
          {
            path: ':id',
            component: ViewPayPeriodComponent,
          },
        ],
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
            data: { permission: PermissionTypes.UserRead },
          },
          {
            path: 'roles',
            component: RoleListComponent,
            data: { permission: PermissionTypes.RoleRead },
          },
          {
            path: 'user-access',
            component: UserRolesComponent,
            data: { permission: PermissionTypes.RoleRead },
          },
        ],
      },
      {
        path: 'users',
        data: { permission: PermissionTypes.UserRead },
        children: [
          {
            path: 'new',
            component: NewUserComponent,
            data: { permission: PermissionTypes.UserCreate },
          },
          {
            path: ':id',
            component: ViewUserComponent,
          },
          {
            path: ':id/edit',
            component: EditUserComponent,
            data: { permission: PermissionTypes.UserUpdate },
          },
        ],
      },
      {
        path: 'roles',
        children: [
          {
            path: 'new',
            component: NewRoleComponent,
            data: { permission: PermissionTypes.RoleCreate },
          },
          {
            path: ':id/edit',
            component: EditRoleComponent,
            data: { permission: PermissionTypes.RoleUpdate },
          },
        ],
      },
    ],
    canActivate: [AuthGuard],
    canActivateChild: [PermissionGuard],
  },
  {
    path: 'self-service',
    component: MainComponent,
    children: [
      {
        path: '',
        component: SelfserviceDashboardComponent,
      },
      {
        path: 'leaves',
        component: SelfserviceLeavesComponent,
      },
      {
        path: 'overtimes',
        component: SelfserviceOvertimesComponent,
      },
      {
        path: 'official-businesses',
        component: SelfserviceOfficialBusinessesComponent,
      },
      {
        path: 'timesheets',
        component: SelfServiceTimesheetsComponent,
      },
    ],
  },
  {
    path: 'not-authorized',
    component: NotAuthorizedComponent,
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
