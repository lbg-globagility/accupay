import { PermissionTypes } from 'src/app/core/auth';
import { MenuItem } from 'src/app/main/shared';

export const adminMenu: MenuItem[] = [
  {
    label: 'HR',
    icon: 'supervisor_account',
    items: [
      {
        label: 'Employees',
        route: '/employees',
        permission: PermissionTypes.EmployeeRead,
      },
      {
        label: 'Salaries',
        route: '/salaries',
        icon: 'person',
        permission: PermissionTypes.SalaryRead,
      },
      {
        label: 'Allowances',
        route: '/allowances',
        icon: 'person',
        permission: PermissionTypes.AllowanceRead,
      },
      {
        label: 'Loans',
        route: '/loans',
        icon: 'person',
        permission: PermissionTypes.LoanRead,
      },
      {
        label: 'Positions',
        route: '/positions',
        permission: PermissionTypes.PositionRead,
      },
      {
        label: 'Policies',
        route: '/employment-policies',
        permission: PermissionTypes.EmploymentPolicyRead,
      },
    ],
  },
  {
    label: 'Timekeeping',
    icon: 'access_time',
    items: [
      {
        label: 'Timesheets',
        route: '/time-entry',
        permission: PermissionTypes.TimeEntryRead,
      },
      {
        label: 'Shifts',
        route: '/shifts',
        permission: PermissionTypes.ShiftRead,
      },
      {
        label: 'Time Logs',
        route: '/time-logs',
        permission: PermissionTypes.TimeLogRead,
      },
      {
        label: 'Overtimes',
        route: '/overtimes',
        permission: PermissionTypes.OvertimeRead,
      },
      {
        label: 'Leaves',
        route: '/leaves',
        permission: PermissionTypes.LeaveRead,
      },
      {
        label: 'Official Business',
        route: '/official-businesses',
        permission: PermissionTypes.OfficialBusinessRead,
      },
    ],
  },
  {
    label: 'Payroll',
    icon: 'payments',
    route: '/payroll',
    permission: PermissionTypes.PayPeriodRead,
  },
  {
    label: 'Reports',
    icon: 'insert_chart_outlined',
    route: '/reports',
  },
  {
    label: 'Security',
    icon: 'security',
    route: '/security',
  },
  {
    label: 'Settings',
    icon: 'settings',
    items: [
      {
        label: 'Organizations',
        route: '/organizations',
        permission: PermissionTypes.OrganizationRead,
      },
      {
        label: 'Branches',
        route: '/branches',
        permission: PermissionTypes.BranchRead,
      },
      {
        label: 'Calendars',
        route: '/calendars',
        permission: PermissionTypes.CalendarRead,
      },
    ],
  },
  {
    label: 'Clients',
    icon: 'business',
    route: '/clients',
    permission: PermissionTypes.ClientRead,
  },
];
