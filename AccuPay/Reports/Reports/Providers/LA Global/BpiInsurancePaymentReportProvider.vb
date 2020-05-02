Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports log4net
Imports Microsoft.EntityFrameworkCore

Public Class BpiInsurancePaymentReportProvider
    Implements ILaGlobalEmployeeReport

    Private _selectedDate As Date
    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Private _reportDocument As BpiInsuranceAmountReport

    Private _payPeriodRepository As PayPeriodRepository

    Sub New()
        _reportDocument = New BpiInsuranceAmountReport()

        _payPeriodRepository = New PayPeriodRepository()
    End Sub

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim monthSelector = New selectMonth()

        Dim succeed = monthSelector.ShowDialog = DialogResult.OK
        If Not succeed Then Return False

        _selectedDate = CDate(monthSelector.MonthFirstDate)

        Try

            SetDataSource()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

    Private Async Sub SetDataSource()

        Dim bpiInsuranceProductID = (Await (New ProductRepository().
                                                GetOrCreateAdjustmentTypeAsync(ProductConstant.BPI_INSURANCE_ADJUSTMENT,
                                                organizationId:=z_OrganizationID,
                                                userId:=z_User))).RowID

        If bpiInsuranceProductID.HasValue = False Then
            Throw New Exception("Cannot get BPI Insurance data.")
        End If

        Dim periods = (Await _payPeriodRepository.GetByMonthYearAndPayPrequencyAsync(
                                    z_OrganizationID,
                                    month:=_selectedDate.Month,
                                    year:=_selectedDate.Year,
                                    payFrequencyId:=Data.Helpers.PayrollTools.PayFrequencySemiMonthlyId)).
                            ToList()

        Using context = New PayrollContext

            Dim periodIDs = periods.Select(Function(p) p.RowID.Value).ToArray()

            Dim adjustmens = context.Adjustments.
                    Include(Function(a) a.Paystub.Employee).
                    Where(Function(a) periodIDs.Contains(a.Paystub.PayPeriodID.Value)).
                    Where(Function(a) bpiInsuranceProductID.Value = a.ProductID.Value).
                    ToList()

            If Not adjustmens.Any Then
                MessageBox.Show($"No record found.", "BPI Insurance Payment Report", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim source = adjustmens.
                    GroupBy(Function(a) a.Paystub.EmployeeID).
                    Select(Function(a) ConvertToDataSource(a)).
                    OrderBy(Function(a) a.Column2).
                    ToList()

            _reportDocument.SetDataSource(source)

            Dim parameterSetter = New CrystalReportParameterValueSetter(_reportDocument)
            With parameterSetter
                .SetParameter("organizationName", z_CompanyName)

            End With

            Dim form = New LaGlobalEmployeeReportForm
            form.SetReportSource(_reportDocument)
            form.Show()

        End Using
    End Sub

    Private Function ConvertToDataSource(a As IGrouping(Of Integer?, Entity.Adjustment)) As BpiInsuranceDataSource
        Dim e = a.FirstOrDefault.Paystub.Employee
        Dim middleName = If(Not String.IsNullOrWhiteSpace(e.MiddleName), $"{Left(e.MiddleName, 1)}.", String.Empty)
        Dim nameParts = {e.LastName, e.FirstName, middleName}
        Dim fullName = String.Join(", ", nameParts.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray())

        Dim result As New BpiInsuranceDataSource With {
            .Column1 = e.EmployeeNo,
            .Column2 = fullName,
            .Column3 = a.Sum(Function(adj) adj.Amount).ToString(),
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