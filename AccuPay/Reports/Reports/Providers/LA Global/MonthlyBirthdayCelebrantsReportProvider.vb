Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class MonthlyBirthdayCelebrantsReportProvider
    Implements ILaGlobalEmployeeReport

    Private _reportDocument As MonthlyBirthdayCelebrantsReport

    Private _selectedDate As Date

    Private recordFound As Boolean

    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim monthSelector = New selectMonth()

        Dim succeed = monthSelector.ShowDialog = DialogResult.OK
        If Not succeed Then Return False

        _selectedDate = CDate(monthSelector.MonthFirstDate)

        Try
            _reportDocument = New MonthlyBirthdayCelebrantsReport

            SetDataSource()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

    Private Async Sub SetDataSource()

        Dim fetchAll As New List(Of Employee)

        Dim employeeBuilder = MainServiceProvider.GetRequiredService(Of EmployeeQueryBuilder)

        fetchAll = Await employeeBuilder.
                        IsActive().
                        IncludeBranch().
                        ToListAsync(z_OrganizationID)

        Dim employees = fetchAll.
                Where(Function(e) _selectedDate.Month = e.BirthDate.Month).
                ToList()

        recordFound = employees.Any()
        If Not recordFound Then
            Dim monthName = Format(_selectedDate, "MMMM")
            MessageBox.Show($"No {monthName} birthday celebrant.", "Monthly Birthday Celebrants", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Return
        End If

        Dim ascendingOrder = employees.OrderBy(Function(e) e.BirthDate).ToList()

        Dim source = ascendingOrder.
            Select(Function(e) New MonthlyBirthdayCelebrantDateSource With {
            .Column1 = e.EmployeeNo,
            .Column2 = $"{e.LastName}, {e.FirstName}{If(Not String.IsNullOrWhiteSpace(e.MiddleName), $" {Left(e.MiddleName, 1)}.", String.Empty)}",
            .Column3 = e.Branch?.Name,
            .Column4 = e.BirthDate.ToShortDateString(),
            .Column5 = _selectedDate.ToShortDateString()}).
            ToList()

        _reportDocument.SetDataSource(source)

        Dim parameterSetter = New CrystalReportParameterValueSetter(_reportDocument)
        parameterSetter.SetParameter("organizationName", z_CompanyName)

        Dim form = New LaGlobalEmployeeReportForm
        form.SetReportSource(_reportDocument)
        form.Show()

    End Sub

    Private Class MonthlyBirthdayCelebrantDateSource

        'Employee ID
        Public Property Column1 As String

        'Employee Fullname
        Public Property Column2 As String

        'Branch
        Public Property Column3 As String

        'Birthdate
        Public Property Column4 As String

        'Selected Month - in a value of date
        Public Property Column5 As String

    End Class

End Class