Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class ActiveEmployeeChecklistReportProvider
    Implements ILaGlobalEmployeeReport

    Private _reportDocument As ActiveEmployeeChecklistReport

    Private recordFound As Boolean

    Private _startDate As Date

    Private _endDate As Date

    Private _periodID As Integer

    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim succeed = False
        Dim form As New SelectPayPeriodSimple

        succeed = form.ShowDialog = DialogResult.OK
        If Not succeed Then Return False

        _periodID = form.RowID

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

    Private Function ActiveCheck(e As Employee) As Boolean
        If Not e.TerminationDate.HasValue Then
            Return True
        End If

        Dim employeeStartDate = e.StartDate
        Dim terminationDate = e.TerminationDate.Value.Date

        If employeeStartDate <= _startDate And terminationDate >= _endDate Then
            Return True
        ElseIf _startDate <= employeeStartDate And _endDate >= employeeStartDate Then
            Return True
        ElseIf _startDate <= terminationDate And _endDate >= terminationDate Then
            Return True
        End If

        Return False
    End Function

    Private Async Sub SetDataSource()
        Using context = New PayrollContext
            Dim records = Await context.Employees.
                Where(Function(e) e.OrganizationID.Value = z_OrganizationID).
                Where(Function(e) e.IsActive).
                ToListAsync()

            Dim employees = records.Where(Function(e) ActiveCheck(e)).ToList()

            recordFound = records.Any()
            If Not recordFound Then
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

        End Using

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