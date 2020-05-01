Option Strict On

Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports log4net
Imports Microsoft.EntityFrameworkCore

Public Class BpiInsurancePaymentReportProvider
    Implements ILaGlobalEmployeeReport
    Private _logger As ILog = LogManager.GetLogger("EmployeeFormAppender")

    Private _reportDocument As BpiInsuranceAmountReport

    Private _selectedDate As Date
    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim monthSelector = New selectMonth()

        Dim succeed = monthSelector.ShowDialog = DialogResult.OK
        If Not succeed Then Return False

        _selectedDate = CDate(monthSelector.MonthFirstDate)

        Try
            _reportDocument = New BpiInsuranceAmountReport

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

        Using context = New PayrollContext
            Dim periods = context.PayPeriods.Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                Where(Function(p) p.Year = _selectedDate.Year).
                Where(Function(p) p.Month = _selectedDate.Month).
                Where(Function(p) p.PayFrequencyID.Value = Data.Helpers.PayrollTools.PayFrequencySemiMonthlyId).
                ToList

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