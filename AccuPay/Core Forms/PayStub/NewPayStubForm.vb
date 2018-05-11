Option Strict On

Imports System.Data.Entity
Imports AccuPay.Entity

Public Class NewPayStubForm

    Private _isActual As Boolean = False

    Private _dateFrom As Date = New Date(2017, 1, 1)
    Private _dateTo As Date = New Date(2017, 1, 15)

    Public Sub NewPayStubForm_Load() Handles Me.Load
        dgvTimeEntries.AutoGenerateColumns = False
        dgvPaystubs.AutoGenerateColumns = False

        Using context = New PayrollContext()
            Dim query =
                (From p In context.Paystubs
                 Join pa In context.PaystubActuals
                    On p.PayPeriodID Equals pa.PayPeriodID And
                     p.EmployeeID Equals pa.EmployeeID
                 Where p.PayFromdate = _dateFrom And
                     p.PayToDate = _dateTo And
                     p.OrganizationID = z_OrganizationID
                 Order By p.Employee.LastName,
                     p.Employee.FirstName
                 Select New With {.Paystub = p, .PaystubActual = pa, .Employee = p.Employee}
                )

            Dim paystubs = query.ToList()

            Dim paystubModels = paystubs.
                Select(Function(p) New PayStubModel(p.Employee, p.Paystub, p.PaystubActual)).
                ToList()

            dgvPaystubs.DataSource = paystubModels
        End Using

        LoadPayperiods()
    End Sub

    Private Sub LoadPayperiods()
        Dim payPeriods As IList(Of PayPeriod) = Nothing

        Using context = New PayrollContext()
            Dim query =
                (From p In context.PayPeriods
                 Join ps In context.Paystubs On p.RowID Equals ps.PayPeriodID
                 Where p.OrganizationID = z_OrganizationID
                 Group By p.RowID Into x = Group
                 Select x.Distinct().FirstOrDefault().p)

            payPeriods = query.ToList()
        End Using

        cboPayPeriods.DataSource = payPeriods.Select(
            Function(t) New With {
                .Display = t.RowID,
                .Item = t
            }
        ).ToList()
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
        Dim paystubActual = paystubModel.PaystubActual

        Dim timeEntries As IList(Of TimeEntry) = Nothing
        Using context = New PayrollContext()
            Dim query = context.TimeEntries.
                Where(Function(t) _dateFrom <= t.Date And t.Date <= _dateTo).
                Where(Function(t) Nullable.Equals(t.EmployeeID, paystub.EmployeeID))

            timeEntries = query.ToList()
        End Using

        dgvTimeEntries.DataSource = timeEntries

        DisplayPaystub(paystub, paystubActual)
    End Sub

    Private Sub DisplayPaystub(declared As Paystub, actual As PaystubActual)
        txtRegularHours.Text = Format(declared.RegularHours)
        txtRegularPay.Text = Format(declared.RegularPay)

        txtOvertimeHours.Text = Format(declared.OvertimeHours)
        txtOvertimePay.Text = Format(If(_isActual, actual.OvertimePay, declared.OvertimePay))

        txtNightDiffHours.Text = Format(declared.NightDiffHours)
        txtNightDiffPay.Text = Format(If(_isActual, actual.NightDiffPay, declared.NightDiffPay))

        txtNightDiffOTHours.Text = Format(declared.NightDiffOvertimeHours)
        txtNightDiffOTPay.Text = Format(If(_isActual, actual.NightDiffOvertimePay, declared.NightDiffOvertimePay))

        txtRestDayHours.Text = Format(declared.RestDayHours)
        txtRestDayPay.Text = Format(If(_isActual, actual.RestDayPay, declared.RestDayPay))

        txtRestDayOTHours.Text = Format(declared.RestDayOTHours)
        txtRestDayOTPay.Text = Format(If(_isActual, actual.RestDayOTPay, declared.RestDayOTPay))

        txtSpecialHolidayHours.Text = Format(declared.SpecialHolidayHours)
        txtSpecialHolidayPay.Text = Format(If(_isActual, actual.SpecialHolidayPay, declared.SpecialHolidayPay))

        txtRegularHolidayHours.Text = Format(declared.RegularHolidayHours)
        txtRegularHolidayPay.Text = Format(If(_isActual, actual.RegularHolidayPay, declared.RegularHolidayPay))

        txtLeaveHours.Text = Format(declared.LeaveHours)
        txtLeavePay.Text = Format(declared.LeavePay)

        txtLateHours.Text = Format(declared.LateHours)
        txtLateAmount.Text = Format(-declared.LateDeduction)

        txtUndertimeHours.Text = Format(declared.UndertimeHours)
        txtUndertimeAmount.Text = Format(-declared.UndertimeDeduction)

        txtAbsentHours.Text = Format(declared.AbsentHours)
        txtAbsentDeduction.Text = Format(-declared.AbsenceDeduction)

        txtTotalEarnings.Text = Format(declared.TotalEarnings)

        txtTotalAllowance.Text = Format(declared.TotalAllowance)
        txtGrossPay.Text = Format(If(_isActual, actual.GrossPay, declared.GrossPay))

        txtSss.Text = Format(-declared.SssEmployeeShare)
        txtPhilHealth.Text = Format(-declared.PhilHealthEmployeeShare)
        txtPagIbig.Text = Format(-declared.HdmfEmployeeShare)

        txtTaxable.Text = Format(declared.TaxableIncome)
        txtWithholdingTax.Text = Format(-declared.WithholdingTax)
        txtTotalLoan.Text = Format(-declared.TotalLoans)

        txtNetPay.Text = Format(If(_isActual, actual.GrossPay, declared.NetPay))
    End Sub

    Private Function Format(value As Decimal) As String
        Return String.Format("{0:#,###,##0.00;(#,###,##0.00);""-""}", value)
    End Function

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

End Class
