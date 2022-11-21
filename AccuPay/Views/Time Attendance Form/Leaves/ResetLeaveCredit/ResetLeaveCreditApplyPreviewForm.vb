Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services.Imports.ResetLeaveCredits
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class ResetLeaveCreditApplyPreviewForm
    Private ReadOnly _dataSource As List(Of ResetLeaveCreditItemModel)

    Public Sub New(dataSource As List(Of ResetLeaveCreditItemModel))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _dataSource = New List(Of ResetLeaveCreditItemModel)
        _dataSource = dataSource.
            Where(Function(t) t.IsSelected AndAlso Not t.IsApplied).
            ToList()

        DataGridViewX1.AutoGenerateColumns = False
    End Sub

    Private Sub ResetLeaveCreditApplyPreviewForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim hasAny = _dataSource.Any()

        btnApply.Enabled = hasAny
        btnApply.Text = $"{btnApply.Text} ({_dataSource.Count()})"

        If Not hasAny Then Return

        DataGridViewX1.DataSource = _dataSource
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub

    Private Async Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Dim prompt = MessageBox.Show(
            $"Are you sure you want to apply the selected {_dataSource.Count()} leave credits for the following employee(s)?",
            "Apply Leave Credits",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If Not prompt = Windows.Forms.DialogResult.Yes Then Return

        Dim messageTitle = "Apply ResetLeaveCredits"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of IResetLeaveCreditDataService)

                Await dataService.ApplyLeaveCredits(organizationId:=z_OrganizationID, userId:=z_User, resetLeaveCreditItems:=_dataSource)

                MessageBox.Show("Leave credits applied successfully!", "Apply Leave Credits")

                DialogResult = DialogResult.OK
            End Function)
    End Sub
End Class
