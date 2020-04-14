Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class BpiInsurancePaymentReportProvider
    Implements ILaGlobalEmployeeReport

    Private reportDocument As BpiInsuranceAmountReport

    Private _selectedDate As Date
    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim monthSelector = New selectMonth()

        Dim succeed = monthSelector.ShowDialog = DialogResult.OK
        If Not succeed Then Return False

        _selectedDate = CDate(monthSelector.MonthFirstDate)

        Try
            reportDocument = New BpiInsuranceAmountReport

            SetDataSource()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

    Private Async Sub SetDataSource()
        Using context = New PayrollContext
            Dim periods = Await context.PayPeriods.Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                Where(Function(p) p.Year = _selectedDate.Year).
                Where(Function(p) p.Month = _selectedDate.Month).
                Where(Function(p) p.PayFrequencyID = 1).
                ToListAsync()

            Dim periodIDs = periods.Select(Function(p) p.RowID.Value).ToArray()

            Dim firstPeriod = periods.FirstOrDefault
            Dim payrollResource = New PayrollResources(firstPeriod.RowID.Value, firstPeriod.PayFromDate, firstPeriod.PayToDate)

            Dim bpiInsuranceProductID = payrollResource.BpiInsuranceProduct.RowID.Value

            Dim adjustmens = Await context.Adjustments.
                Include(Function(a) a.Paystub.Employee).
                Where(Function(a) periodIDs.Contains(a.Paystub.PayPeriodID.Value)).
                Where(Function(a) bpiInsuranceProductID = a.ProductID.Value).
                ToListAsync()

            Dim source = adjustmens.
                GroupBy(Function(a) a.Paystub.EmployeeID).
                Select(Function(a) ConvertToDataSource(a)).
                OrderBy(Function(a) a.Column2).
                ToList()

            Dim parameterSetter = New CrystalReportParameterValueSetter(reportDocument)
            With parameterSetter
                .SetParameter("organizationName", z_CompanyName)

            End With

            reportDocument.SetDataSource(source)
        End Using

        Dim form = New LaGlobalEmployeeReportForm
        form.SetReportSource(reportDocument)
        form.Show()

    End Sub

    Private Function ConvertToDataSource(a As IGrouping(Of Integer?, Adjustment)) As BpiInsuranceDataSource
        Dim e = a.FirstOrDefault.Paystub.Employee
        Dim middleName = If(Not String.IsNullOrWhiteSpace(e.MiddleName), $"{Left(e.MiddleName, 1)}.", String.Empty)
        Dim nameParts = {e.LastName, e.FirstName, middleName}
        Dim fullName = String.Join(", ", nameParts.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray())

        Dim result As New BpiInsuranceDataSource With {
            .Column1 = e.EmployeeNo,
            .Column2 = fullName,
            .Column3 = a.Sum(Function(adj) adj.Amount),
            .Column4 = _selectedDate.ToShortDateString()}

        Return result
    End Function

    Private Class BpiInsuranceDataSource

        'Employee ID
        Public Property Column1 As String

        'Employee Fullname
        Public Property Column2 As String

        'Payment/Amount
        Public Property Column3 As String

        'Selected Month - in a value of date
        Public Property Column4 As String

    End Class

End Class