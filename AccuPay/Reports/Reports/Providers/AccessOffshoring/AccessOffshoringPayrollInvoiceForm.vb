Option Strict On
Imports SergeUtils

Public Class AccessOffshoringPayrollInvoiceForm

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub AccessOffshoringPayrollInvoiceForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gridContractors.AutoGenerateColumns = False
        gridPeriods.AutoGenerateColumns = False
        gridEmployees.AutoGenerateColumns = False

        InitSelectAllColumnHeader()

    End Sub

    Private Sub InitSelectAllColumnHeader()
        Dim cbHeaderIsVerified = New DatagridViewCheckBoxHeaderCell("[All]")
        Column1.HeaderCell = cbHeaderIsVerified
        RemoveHandler cbHeaderIsVerified.OnCheckBoxClicked, AddressOf cbHeaderIsSelectAll_CheckBoxClicked
        AddHandler cbHeaderIsVerified.OnCheckBoxClicked, AddressOf cbHeaderIsSelectAll_CheckBoxClicked
    End Sub

    Private Sub gridPeriods_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridPeriods.CellContentClick

    End Sub

    Private Sub gridPeriods_SelectionChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub cbHeaderIsSelectAll_CheckBoxClicked(state As Boolean)
        Dim data = gridEmployees.Rows.OfType(Of DataGridViewRow).
            ToList()

        data.ForEach(Sub(t)
                         t.Cells(Column1.Name).Value = state
                     End Sub)

        gridEmployees.EndEdit()
        gridEmployees.Refresh()

    End Sub


End Class
