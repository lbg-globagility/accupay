Option Strict On

Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class PaystubView

    Public Event Init()

    Public Event Search(term As String)

    Public Event SelectPaystub(paystub As Paystub)

    Public Event SelectPayperiod(payperiod As PayPeriod)

    Public Event ToggleActual()

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Dim presenter = New PaystubPresenter(Me)
    End Sub

    Public Sub NewPayStubForm_Load() Handles Me.Load
        dgvTimeEntries.AutoGenerateColumns = False
        dgvPaystubs.AutoGenerateColumns = False

        RaiseEvent Init()
    End Sub

    Public Sub ShowPaystubs(paystubs As IList(Of Paystub))
        Dim paystubModels = paystubs.
            Select(Function(p) New PayStubModel(p.Employee, p, Nothing)).
            ToList()

        dgvPaystubs.DataSource = paystubModels
    End Sub

    Public Sub ShowSalary(employee As Employee, salary As Salary, isActual As Boolean)
        If salary Is Nothing Then
            Return
        End If

        Dim amount = If(isActual, salary.TotalSalary, salary.BasicSalary)

        Dim dailyRate = 0D
        If employee.IsMonthly Or employee.IsFixed Then
            dailyRate = amount / (employee.WorkDaysPerYear / 12)
        Else
            dailyRate = amount
        End If

        Dim hourlyRate = dailyRate / 8

        txtSalary.Text = Format(amount)
        txtDailyRate.Text = Format(dailyRate)
        txtHourlyRate.Text = Format(hourlyRate)
    End Sub

    Public Sub ShowPayperiods(payperiods As IList(Of PayPeriod))
        cboPayPeriods.DataSource = payperiods.
            Select(Function(t) New PayPeriodModel(t)).
            OrderByDescending(Function(t) t.Item.PayFromDate).
            ToList()
    End Sub

    Public Sub ShowTimeEntries(timeEntries As IList(Of TimeEntry))
        dgvTimeEntries.DataSource = timeEntries
    End Sub

    Private Sub PayStubDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvPaystubs.CellFormatting
        Dim dataGridView = DirectCast(sender, DataGridView)

        Dim column As DataGridViewColumn = dataGridView.Columns(e.ColumnIndex)
        If (column.DataPropertyName.Contains(".")) Then
            Dim data As Object = dataGridView.Rows(e.RowIndex).DataBoundItem
            Dim properties As String() = column.DataPropertyName.Split("."c)

            Dim i As Integer = 0
            While i < properties.Length And data IsNot Nothing
                data = data.GetType().GetProperty(properties(i)).GetValue(data)
                i += 1
            End While

            dataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = data
        End If
    End Sub

    Private Sub dgvPaystubs_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPaystubs.SelectionChanged
        Try
            Dim paystubModel = DirectCast(dgvPaystubs.CurrentRow.DataBoundItem, PayStubModel)

            If paystubModel Is Nothing Then
                Return
            End If

            Dim paystub = paystubModel.Paystub

            RaiseEvent SelectPaystub(paystub)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ShowPaystub(declared As Paystub, actual As PaystubActual, isActual As Boolean)
        txtBasicHours.Text = Format(declared.BasicHours)
        txtBasicPay.Text = Format(declared.BasicPay)

        txtRegularHours.Text = Format(declared.RegularHours)
        txtRegularPay.Text = Format(declared.RegularPay)

        txtOvertimeHours.Text = Format(declared.OvertimeHours)
        txtOvertimePay.Text = Format(If(isActual, actual.OvertimePay, declared.OvertimePay))

        txtNightDiffHours.Text = Format(declared.NightDiffHours)
        txtNightDiffPay.Text = Format(If(isActual, actual.NightDiffPay, declared.NightDiffPay))

        txtNightDiffOTHours.Text = Format(declared.NightDiffOvertimeHours)
        txtNightDiffOTPay.Text = Format(If(isActual, actual.NightDiffOTPay, declared.NightDiffOvertimePay))

        txtRestDayHours.Text = Format(declared.RestDayHours)
        txtRestDayPay.Text = Format(If(isActual, actual.RestDayPay, declared.RestDayPay))

        txtRestDayOTHours.Text = Format(declared.RestDayOTHours)
        txtRestDayOTPay.Text = Format(If(isActual, actual.RestDayOTPay, declared.RestDayOTPay))

        txtSpecialHolidayHours.Text = Format(declared.SpecialHolidayHours)
        txtSpecialHolidayPay.Text = Format(If(isActual, actual.SpecialHolidayPay, declared.SpecialHolidayPay))

        txtRegularHolidayHours.Text = Format(declared.RegularHolidayHours)
        txtRegularHolidayPay.Text = Format(If(isActual, actual.RegularHolidayPay, declared.RegularHolidayPay))

        txtLeaveHours.Text = Format(declared.LeaveHours)
        txtLeavePay.Text = Format(declared.LeavePay)

        txtLateHours.Text = Format(declared.LateHours)
        txtLateAmount.Text = Format(-If(isActual, actual.LateDeduction, declared.LateDeduction))

        txtUndertimeHours.Text = Format(declared.UndertimeHours)
        txtUndertimeAmount.Text = Format(-If(isActual, actual.UndertimeDeduction, declared.UndertimeDeduction))

        txtAbsentHours.Text = Format(declared.AbsentHours)
        txtAbsentDeduction.Text = Format(-If(isActual, actual.AbsenceDeduction, declared.AbsenceDeduction))

        txtDeductionHours.Text = Format(declared.LateHours + declared.UndertimeHours + declared.AbsentHours)

        Dim deductedAmount = If(
            isActual,
            actual.LateDeduction + actual.UndertimeDeduction + actual.AbsenceDeduction,
            declared.LateDeduction + declared.UndertimeDeduction + declared.AbsenceDeduction)
        txtDeductionAmount.Text = Format(-deductedAmount)

        txtTotalEarnings.Text = Format(declared.TotalEarnings)

        txtTotalAllowance.Text = Format(declared.TotalAllowance)
        txtGrossPay.Text = Format(If(isActual, actual.GrossPay, declared.GrossPay))

        txtSss.Text = Format(-declared.SssEmployeeShare)
        txtPhilHealth.Text = Format(-declared.PhilHealthEmployeeShare)
        txtPagIbig.Text = Format(-declared.HdmfEmployeeShare)

        txtTaxable.Text = Format(declared.TaxableIncome)
        txtWithholdingTax.Text = Format(-declared.WithholdingTax)
        txtTotalLoan.Text = Format(-declared.TotalLoans)

        txtNetPay.Text = Format(If(isActual, actual.NetPay, declared.NetPay))
    End Sub

    Public Sub ShowAdjustments(adjustments As IList(Of Adjustment))
        dgvAdjustments.DataSource = adjustments
    End Sub

    Private Function Format(value As Decimal) As String
        Return String.Format("{0:#,###,##0.00;(#,###,##0.00);""-""}", value)
    End Function

    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        RaiseEvent Search(TextBoxSearch.Text.ToLower())
    End Sub

    Private Sub cboPayPeriods_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPayPeriods.SelectedIndexChanged
        Dim payPeriodModel = DirectCast(cboPayPeriods.SelectedItem, PayPeriodModel)

        If payPeriodModel Is Nothing Then
            Return
        End If

        RaiseEvent SelectPayperiod(payPeriodModel.Item)
    End Sub

    Private Sub btnActualToggle_Click(sender As Object, e As EventArgs) Handles btnActualToggle.Click
        RaiseEvent ToggleActual()
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Dim dateFrom As Date = New Date(2017, 1, 1)
        Dim dateTo As Date = New Date(2017, 1, 15)

        Dim exporter = New ExportBankFile(dateFrom, dateTo)
        exporter.Extract()
    End Sub

    Private Class PayStubModel

        Public ReadOnly Property Employee As Employee

        Public ReadOnly Property Paystub As Paystub

        Public ReadOnly Property PaystubActual As PaystubActual

        Public Sub New(employee As Employee, paystub As Paystub, paystubActual As PaystubActual)
            Me.Employee = employee
            Me.Paystub = paystub
            Me.PaystubActual = paystubActual
        End Sub

    End Class

    Public Class PayPeriodModel

        Public Property Display As String

        Public Property Item As PayPeriod

        Public Sub New(payPeriod As PayPeriod)
            Display = $"{payPeriod.PayFromDate.ToString("MM/dd/yyyy")} {payPeriod.PayToDate.ToString("MM/dd/yyyy")}"
            Item = payPeriod
        End Sub

    End Class

End Class
