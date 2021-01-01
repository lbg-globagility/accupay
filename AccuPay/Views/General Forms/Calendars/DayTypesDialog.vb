Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Repositories
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class DayTypesDialog

    Private ReadOnly _repository As DayTypeRepository

    Private _dayTypes As ICollection(Of DayType)

    Public Sub New()
        _repository = MainServiceProvider.GetRequiredService(Of DayTypeRepository)

        InitializeComponent()
        InitializeView()
    End Sub

    Private Sub InitializeView()
        DayTypesGridView.AutoGenerateColumns = False
    End Sub

    Private Async Sub DayTypesDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.CALENDAR)

        SaveButton.Visible = False
        DayTypeControl.Enabled = False

        If role.Success AndAlso role.RolePermission.Update Then

            SaveButton.Visible = True
            DayTypeControl.Enabled = True

        End If

        _dayTypes = Await _repository.GetAllAsync()
        DayTypesGridView.DataSource = _dayTypes
    End Sub

    Private Sub DayTypesGridView_SelectionChanged(sender As Object, e As EventArgs) Handles DayTypesGridView.SelectionChanged
        Dim a = DirectCast(DayTypesGridView.CurrentRow.DataBoundItem, DayType)

        DayTypeControl.DayType = a
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Const title As String = "Save Day Type"
        Await FunctionUtils.TryCatchFunctionAsync(title,
            Async Function()

                Dim dayType = DayTypeControl.DayType
                Await _repository.SaveAsync(dayType)

                myBalloon("Day type saved!", title, DetailsGroupBox, 0, -50)
            End Function)

    End Sub

    Private Sub EmployeeLeavesForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        myBalloon(, , DetailsGroupBox, , , 1)
    End Sub

End Class