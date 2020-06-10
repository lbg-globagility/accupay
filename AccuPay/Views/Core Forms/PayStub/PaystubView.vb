Option Strict On

Imports System.ComponentModel
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports Microsoft.Extensions.DependencyInjection

Public Class PaystubView

    Public Event Init()

    Public Event Search(term As String)

    Public Event SelectPaystub(paystub As Paystub)

    Public Event SelectPayperiod(payperiod As PayPeriod)

    Public Event ToggleActual()

    Private _adjustmentTypes As ICollection(Of String)

    Private WithEvents _adjustmentSource As BindingSource

    Sub New()

        InitializeComponent()
        Dim presenter = New PaystubPresenter(Me)
        _adjustmentSource = New BindingSource With {
            .AllowNew = True
        }

    End Sub

    Public Sub SetAdjustmentTypes(adjustmentTypes As ICollection(Of String))
        _adjustmentTypes = adjustmentTypes
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
        txtNightDiffOTPay.Text = Format(If(isActual, actual.NightDiffOvertimePay, declared.NightDiffOvertimePay))

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
        txtTotalAdjustments.Text = Format(If(isActual, actual.TotalAdjustments, declared.TotalAdjustments))
        txtTotalLoan.Text = Format(-declared.TotalLoans)

        txtNetPay.Text = Format(If(isActual, actual.NetPay, declared.NetPay))
    End Sub

    Public Sub ShowAllowanceItems(allowanceItems As ICollection(Of AllowanceItem))
        Dim allowanceItemModels = allowanceItems.
            Select(Function(a) New AllowanceItemModel() With {
                .Name = a.Allowance.Product.PartNo,
                .Amount = a.Amount
            }).
            ToList()

        dgvAllowances.DataSource = allowanceItemModels
    End Sub

    Public Sub ShowLoanTransactions(loanTransactions As ICollection(Of LoanTransaction))
        Dim loanTransactionModels = loanTransactions.
            Select(Function(l) New LoanTransactionModel() With {
                .Name = l.LoanSchedule.LoanType.PartNo,
                .Amount = l.Amount,
                .Balance = l.TotalBalance
            }).ToList()

        dgvLoanTransactions.DataSource = loanTransactionModels
    End Sub

    Public Sub ShowAdjustments(adjustments As ICollection(Of Adjustment))
        Dim adjustmentModels As New List(Of AdjustmentModel)

        If adjustments.Any() Then

            adjustmentModels = adjustments.
                                Select(Function(a) New AdjustmentModel() With {
                                    .Name = a.Product.Name,
                                    .Amount = a.Amount,
                                    .Remarks = a.Comment
                                }).
                                ToList()

        End If

        _adjustmentSource.DataSource = adjustmentModels
    End Sub

    Private Sub DgvAdjustments_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvAdjustments.EditingControlShowing
        Dim column As Integer = dgvAdjustments.CurrentCell.ColumnIndex
        Dim headerText As String = dgvAdjustments.Columns(column).HeaderText

        If (headerText.Equals("Name")) Then
            Dim tb As TextBox = DirectCast(e.Control, TextBox)

            If (tb IsNot Nothing) Then
                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                tb.AutoCompleteCustomSource = AutoCompleteLoad()
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource
            End If
        ElseIf (headerText.Equals("Amount")) Then
            Dim tb As TextBox = DirectCast(e.Control, TextBox)

            Dim row = DirectCast(dgvAdjustments.CurrentRow.DataBoundItem, AdjustmentModel)

            tb.Text = If(row?.Amount.ToString(), "0")

            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        End If
    End Sub

    Private Sub DgvAdjustments_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvAdjustments.DataError
        Dim model = DirectCast(dgvAdjustments.Rows(e.RowIndex).DataBoundItem, AdjustmentModel)
        model.Amount = 0

        e.Cancel = False
    End Sub

    Public Function AutoCompleteLoad() As AutoCompleteStringCollection
        Dim collection = New AutoCompleteStringCollection()
        collection.AddRange(_adjustmentTypes.ToArray())
        Return collection
    End Function

    Private Sub NewPayStubForm_Load() Handles Me.Load
        dgvTimeEntries.AutoGenerateColumns = False
        dgvPaystubs.AutoGenerateColumns = False
        dgvAllowances.AutoGenerateColumns = False
        dgvLoanTransactions.AutoGenerateColumns = False

        dgvAdjustments.DataSource = _adjustmentSource

        RaiseEvent Init()
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

    Private Sub OnAdjustmentAdding(sender As Object, e As AddingNewEventArgs) Handles _adjustmentSource.AddingNew
        Dim newAdjustment = New AdjustmentModel()

        e.NewObject = newAdjustment
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

    Private Async Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Dim dateFrom As Date = New Date(2017, 1, 1)
        Dim dateTo As Date = New Date(2017, 1, 15)

        Dim exporter = New ExportBankFile(dateFrom, dateTo)
        Await exporter.Extract()
    End Sub

    Private Sub DeclaredToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeclaredToolStripMenuItem.Click
        Dim report = New PayrollSummaryExcelFormatReportProvider() With {
            .IsActual = False
        }
        report.Run()
    End Sub

    Private Sub ActualToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActualToolStripMenuItem.Click
        Dim report = New PayrollSummaryExcelFormatReportProvider() With {
            .IsActual = True
        }
        report.Run()
    End Sub

    Public Class AllowanceItemModel

        Public Property Name As String

        Public Property Amount As Decimal

    End Class

    Public Class AdjustmentModel

        Public Property Name As String

        Public Property Amount As Decimal

        Public Property Remarks As String

    End Class

    Public Class LoanTransactionModel

        Public Property Name As String

        Public Property Amount As Decimal

        Public Property Balance As Decimal

    End Class

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
            Item = payPeriod
            Display = $"{GetPeriod()} - {payPeriod.PayFromDate.ToString("MM/dd/yyyy")} to {payPeriod.PayToDate.ToString("MM/dd/yyyy")}"
        End Sub

        Public Function GetPeriod() As String
            If Item.IsSemiMonthly Then
                Dim month = New Date(Item.Year, Item.Month, 1)
                Dim halfNo = String.Empty

                If Item.IsFirstHalf Then
                    halfNo = "1st Half"
                ElseIf Item.IsEndOfTheMonth Then
                    halfNo = "2nd Half"
                End If

                Return $"{month.ToString("MMM")} {halfNo}"
            ElseIf Item.IsWeekly Then
                ' Not implemented yet
                Return String.Empty
            Else
                Return String.Empty
            End If
        End Function

    End Class

End Class