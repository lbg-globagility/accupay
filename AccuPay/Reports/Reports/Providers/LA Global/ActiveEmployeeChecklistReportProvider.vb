Option Strict On

Imports AccuPay.Data
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories

Public Class ActiveEmployeeChecklistReportProvider
    Implements ILaGlobalEmployeeReport

    Private _reportDocument As ActiveEmployeeChecklistReport

    Private _startDate As Date

    Private _endDate As Date

    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Private ReadOnly _context As PayrollContext

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Sub New(context As PayrollContext, payPeriodRepository As PayPeriodRepository)

        _context = context

        _payPeriodRepository = payPeriodRepository
    End Sub

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim succeed = False
        Dim form As New SelectPayPeriodSimple(_payPeriodRepository)

        succeed = form.ShowDialog = DialogResult.OK
        If Not succeed Then Return False

        _startDate = form.PayFromDate
        _endDate = form.PayToDate

        Try
            _reportDocument = New ActiveEmployeeChecklistReport

            SetDataSource()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

    Private Sub SetDataSource()

        Dim employees As New List(Of Employee)

        Using employeeBuilder = New EmployeeRepository.EmployeeBuilder(_context)

            employees = employeeBuilder.
                            IsActive().
                            IncludeBranch().
                            ToList(z_OrganizationID)
        End Using

        If Not employees.Any Then
            MessageBox.Show($"No record found.", "Active Employee Checklist Report", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Return
        End If

        Dim ascendingOrder = employees.OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst).ToList()

        Dim source = ascendingOrder.
                Select(Function(e) New ActiveEmployeeChecklistDateSource With {
                .Column1 = e.EmployeeNo,
                .Column2 = $"{e.LastName}, {e.FirstName}{If(Not String.IsNullOrWhiteSpace(e.MiddleName), $", {Left(e.MiddleName, 1)}.", String.Empty)}",
                .Column3 = e.Branch?.Name,
                .Column4 = String.Empty}).
                ToList()

        _reportDocument.SetDataSource(source)

        Dim parameterSetter = New CrystalReportParameterValueSetter(_reportDocument)
        Dim parameters = New Dictionary(Of String, Object) From {
                {"fromDate", _startDate}, {"toDate", _endDate}, {"organizationName", z_CompanyName}}
        parameterSetter.SetParameters(parameters)

        Dim form = New LaGlobalEmployeeReportForm
        form.SetReportSource(_reportDocument)
        form.Show()

    End Sub

    Private Class ActiveEmployeeChecklistDateSource

        'Employee ID
        Public Property Column1 As String

        'Employee Fullname
        Public Property Column2 As String

        'Branch
        Public Property Column3 As String

        'Birthdate
        Public Property Column4 As String

    End Class

End Class