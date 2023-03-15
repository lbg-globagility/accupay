Public Class KnownForms
    Public Shared ReadOnly PrimaryFormName As String = MDIPrimaryForm.Name
    Public Shared ReadOnly SecondaryFormNames As String() = {GeneralForm.Name, HRISForm.Name, TimeAttendForm.Name, PayrollForm.Name, FormReports.Name}
    Public Shared ReadOnly LoginFormName As String = MetroLogin.Name
    Public Shared ReadOnly CompanySwitcherFormName As String = "OrganizationListDialog"

    Public Shared ReadOnly GeneralFormNames As String() = {AgencyForm.Name, UserForm.Name, OrganizationForm.Name, UserRoleForm.Name, SSSCntrib.Name, CalendarsForm.Name}
    Public Shared ReadOnly HrisFormNames As String() = {NewEmployeeForm.Name, NewDivisionPositionForm.Name, EmployeeForm.Name, JobLevelForm.Name, JobPointsForm.Name}
    Public Shared ReadOnly TimeAndAttendanceFormNames As String() = {TimeEntrySummaryForm.Name, MassOvertimeForm.Name, ShiftForm.Name, TimeLogsForm.Name, EmployeeLeavesForm.Name, OfficialBusinessForm.Name, EmployeeOvertimeForm.Name, TripTicketForm.Name, ResetLeaveCreditsForm.Name}
    Public Shared ReadOnly PayrollFormNames As String() = {BenchmarkPayrollForm.Name, PayStubForm.Name, PaystubView.Name, EmployeeAllowanceForm.Name, EmployeeLoansForm.Name, BenchmarkPaystubForm.Name}
    Public Shared ReadOnly ReportFormNames As String() = {ReportsList.Name, CrysRepForm.Name}
End Class
