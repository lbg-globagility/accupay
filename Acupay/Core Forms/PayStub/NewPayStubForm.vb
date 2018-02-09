Option Strict On

Imports System.Data.Entity
Imports AccuPay.Entity

Public Class NewPayStubForm

    Private _dateFrom As Date = New Date(2017, 1, 1)
    Private _dateTo As Date = New Date(2017, 1, 15)

    Public Sub NewPayStubForm_Load() Handles Me.Load
        dgvTimeEntries.AutoGenerateColumns = False
        dgvPaystubs.AutoGenerateColumns = False

        Using context = New PayrollContext()
            Dim payStubs = context.Paystubs.
                Include(Function(p) p.Employee).
                Where(Function(p) p.PayFromdate = _dateFrom).
                Where(Function(p) p.PayToDate = _dateTo).
                Where(Function(p) CBool(p.OrganizationID = z_OrganizationID)).
                OrderBy(Function(p) p.Employee.LastName).
                ThenBy(Function(p) p.Employee.FirstName).
                ToList()

            Dim paystubModels = payStubs.Select(Function(p) New PayStubModel(p)).ToList()

            dgvPaystubs.DataSource = paystubModels
        End Using
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
        Dim paystubModel = DirectCast(dgvPaystubs.CurrentRow.DataBoundItem, PayStubModel)

        If paystubModel Is Nothing Then
            Return
        End If

        Dim paystub = paystubModel.Paystub

        Dim timeEntries As IList(Of TimeEntry) = Nothing
        Using context = New PayrollContext()
            Dim query = context.TimeEntries.
                Where(Function(t) _dateFrom <= t.EntryDate And t.EntryDate <= _dateTo).
                Where(Function(t) Nullable.Equals(t.EmployeeID, paystub.EmployeeID))

            timeEntries = query.ToList()
        End Using

        dgvTimeEntries.DataSource = timeEntries

        DisplayPaystub(paystub)
    End Sub

    Private Sub DisplayPaystub(paystub As Paystub)
        txtRegularHours.Text = Format(paystub.RegularHours)
        txtRegularPay.Text = Format(paystub.RegularPay)

        txtOvertimeHours.Text = Format(paystub.OvertimeHours)
        txtOvertimePay.Text = Format(paystub.OvertimePay)

        txtNightDiffHours.Text = Format(paystub.NightDiffHours)
        txtNightDiffPay.Text = Format(paystub.NightDiffPay)

        txtNightDiffOTHours.Text = Format(paystub.NightDiffOvertimeHours)
        txtNightDiffOTPay.Text = Format(paystub.NightDiffOvertimePay)

        txtRestDayHours.Text = Format(paystub.RestDayHours)
        txtRestDayPay.Text = Format(paystub.RestDayPay)

        txtRestDayOTHours.Text = Format(paystub.RestDayOTHours)
        txtRestDayOTPay.Text = Format(paystub.RestDayOTPay)

        txtSpecialHolidayHours.Text = Format(paystub.SpecialHolidayHours)
        txtRegularHolidayHours.Text = Format(paystub.RegularHolidayHours)
        txtRegularHolidayPay.Text = Format(paystub.HolidayPay)

        txtLeaveHours.Text = Format(paystub.LeaveHours)
        txtLeavePay.Text = Format(paystub.LeavePay)

        txtLateHours.Text = Format(paystub.LateHours)
        txtLateAmount.Text = Format(-paystub.LateDeduction)

        txtUndertimeHours.Text = Format(paystub.UndertimeHours)
        txtUndertimeAmount.Text = Format(-paystub.UndertimeDeduction)

        txtAbsentHours.Text = Format(paystub.AbsentHours)
        txtAbsentDeduction.Text = Format(-paystub.AbsenceDeduction)

        txtTotalEarnings.Text = Format(paystub.WorkPay)

        txtTotalAllowance.Text = Format(paystub.TotalAllowance)
        txtGrossPay.Text = Format(paystub.GrossPay)

        txtSss.Text = Format(-paystub.SssEmployeeShare)
        txtPhilHealth.Text = Format(-paystub.PhilHealthEmployeeShare)
        txtPagIbig.Text = Format(-paystub.HdmfEmployeeShare)

        txtTaxable.Text = Format(paystub.TaxableIncome)
        txtWithholdingTax.Text = Format(-paystub.WithholdingTax)
        txtTotalLoan.Text = Format(-paystub.TotalLoans)

        txtNetPay.Text = Format(paystub.NetPay)
    End Sub

    Private Function Format(value As Decimal) As String
        Return String.Format("{0:#,###,##0.00;(#,###,##0.00);""-""}", value)
    End Function

    Private Class PayStubModel

        Private _paystub As Paystub

        Public ReadOnly Property Paystub As Paystub
            Get
                Return _paystub
            End Get
        End Property

        Public Sub New(paystub As Paystub)
            _paystub = paystub
        End Sub

        Public ReadOnly Property EmployeeName As String
            Get
                Return $"{_paystub.Employee.LastName}, {_paystub.Employee.FirstName}"
            End Get
        End Property

    End Class

End Class