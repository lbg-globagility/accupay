Option Strict On

Imports System.Data.Entity
Imports AccuPay.Entity

Public Class NewPayStubForm

    Private _dateFrom As Date = New Date(2017, 7, 11)
    Private _dateTo As Date = New Date(2017, 7, 25)

    Public Sub NewPayStubForm_Load() Handles Me.Load
        PayStubDataGridView.AutoGenerateColumns = False

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

            PayStubDataGridView.DataSource = paystubModels
        End Using
    End Sub

    Private Sub PayStubDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles PayStubDataGridView.CellFormatting
        Dim column As DataGridViewColumn = PayStubDataGridView.Columns(e.ColumnIndex)
        If (column.DataPropertyName.Contains(".")) Then
            Dim data As Object = PayStubDataGridView.Rows(e.RowIndex).DataBoundItem
            Dim properties As String() = column.DataPropertyName.Split("."c)

            Dim i As Integer = 0
            While i < properties.Length And data IsNot Nothing
                data = data.GetType().GetProperty(properties(i)).GetValue(data)
                i += 1
            End While

            PayStubDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = data
        End If
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