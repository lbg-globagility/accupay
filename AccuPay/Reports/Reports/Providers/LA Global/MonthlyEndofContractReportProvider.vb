Imports AccuPay.Entity
Imports AccuPay.Repository

Public Class MonthlyEndofContractReportProvider
    Implements ILaGlobalEmployeeReport

    Private _reportDocument As MonthlyEndOfContractReport

    Private recordFound As Boolean

    Private _startDate As Date

    Private _endDate As Date

    Private employeeRepo As New EmployeeRepository()

    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim monthSelector = New selectMonth()

        Dim succeed = monthSelector.ShowDialog = DialogResult.OK
        If Not succeed Then Return False

        _startDate = CDate(monthSelector.MonthFirstDate)
        _endDate = CDate(monthSelector.MonthLastDate)

        Try
            _reportDocument = New MonthlyEndOfContractReport

            SetDataSource()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

    Private Async Sub SetDataSource()
        Dim fetchAll = Await employeeRepo.GetAllAsync()
        Dim employees = fetchAll.
            Where(Function(e) e.OrganizationID.Value = z_OrganizationID).
            Where(Function(e) e.IsActive).
            Where(Function(e) e.DateRegularized.HasValue).
            Where(Function(e) e.DateRegularized.Value.Date >= _startDate AndAlso e.DateRegularized.Value.Date <= _endDate).
            ToList()

        recordFound = employees.Any()
        If Not recordFound Then
            MessageBox.Show($"No record found.", "Active Employee Checklist Report", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Return
        End If

        Dim descendingOrder = employees.OrderByDescending(Function(e) e.DateRegularized.Value.Date).ToList()

        Dim source = descendingOrder.
            Select(Function(e) New ActiveEmployeeChecklistDateSource With {
            .Column1 = e.EmployeeNo,
            .Column2 = $"{e.LastName}, {e.FirstName}{If(Not String.IsNullOrWhiteSpace(e.MiddleName), $", {Left(e.MiddleName, 1)}.", String.Empty)}",
            .Column3 = e.Branch?.Name,
            .Column4 = e.StartDate.ToShortDateString(),
            .Column5 = If(e.DateRegularized.HasValue, e.DateRegularized.Value.Date.ToShortDateString(), String.Empty)}).
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

        'Hire Date
        Public Property Column4 As String

        'Date Regualarized
        Public Property Column5 As String

    End Class

End Class