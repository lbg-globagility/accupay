Option Strict On

Imports System.Data.Entity
Imports AccuPay.Entity

Public Class NewPayStubForm

    Private _dateFrom As Date = New Date(2017, 12, 21)
    Private _dateTo As Date = New Date(2018, 1, 5)

    Public Sub New()
        InitializeComponent()
        txtRegularHours.AutoSize = False
    End Sub

    Public Sub NewPayStubForm_Load() Handles Me.Load
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
        Dim column As DataGridViewColumn = dgvPaystubs.Columns(e.ColumnIndex)
        If (column.DataPropertyName.Contains(".")) Then
            Dim data As Object = dgvPaystubs.Rows(e.RowIndex).DataBoundItem
            Dim properties As String() = column.DataPropertyName.Split("."c)

            Dim i As Integer = 0
            While i < properties.Length And data IsNot Nothing
                data = data.GetType().GetProperty(properties(i)).GetValue(data)
                i += 1
            End While

            dgvPaystubs.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = data
        End If
    End Sub

    Private Sub dgvPaystubs_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPaystubs.SelectionChanged
        Dim paystubModel = DirectCast(dgvPaystubs.CurrentRow.DataBoundItem, PayStubModel)

        If paystubModel Is Nothing Then
            Return
        End If

        Dim paystub = paystubModel.Paystub

        txtRegularHours.Text = CStr(paystub.RegularHours)
        txtRegularPay.Text = CStr(paystub.RegularPay)

        txtOvertimeHours.Text = CStr(paystub.OvertimeHours)
        txtOvertimePay.Text = CStr(paystub.OvertimePay)

        txtNightDiffHours.Text = CStr(paystub.NightDiffHours)
        txtNightDiffPay.Text = CStr(paystub.NightDiffPay)

        txtNightDiffOTHours.Text = CStr(paystub.NightDiffOvertimeHours)
        txtNightDiffOTPay.Text = CStr(paystub.NightDiffOvertimePay)

        txtRestDayHours.Text = CStr(paystub.RestDayHours)
        txtRestDayPay.Text = CStr(paystub.RestDayPay)

        txtLeavePay.Text = CStr(paystub.LeavePay)
        txtHolidayPay.Text = CStr(paystub.HolidayPay)
        txtTotalPay.Text = CStr(paystub.WorkPay)

        txtLateHours.Text = CStr(paystub.LateHours)
        txtLateAmount.Text = CStr(paystub.LateDeduction)

        txtUndertimeHours.Text = CStr(paystub.UndertimeHours)
        txtUndertimeAmount.Text = CStr(paystub.UndertimeDeduction)

        txtAbsentDeduction.Text = CStr(paystub.AbsenceDeduction)
    End Sub

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