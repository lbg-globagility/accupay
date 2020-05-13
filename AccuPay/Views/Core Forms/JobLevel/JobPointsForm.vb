Option Strict On

Imports AccuPay.JobLevels
Imports Microsoft.Extensions.DependencyInjection

Public Class JobPointsForm
    Implements IJobPointsView

    Public Event JobPointsForm_OnLoad() Implements IJobPointsView.OnLoad

    Public Event JobPointsForm_EmployeeSelected(employee As EmployeeModel) Implements IJobPointsView.EmployeeSelected

    Public Event JobPointsForm_EmployeeChanged(model As EmployeeModel) Implements IJobPointsView.EmployeeChanged

    Public Event JobPointsForm_SaveEmployees() Implements IJobPointsView.SaveEmployees

    Public Sub New()
        InitializeComponent()

        Using MainServiceProvider

            Dim presenter = MainServiceProvider.GetRequiredService(Of JobPointsPresenter)()

        End Using

        'Dim presenter = New JobPointsPresenter(Me)
    End Sub

    Private Sub JobPointsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeComponents()
        RaiseEvent JobPointsForm_OnLoad()
    End Sub

    Private Sub InitializeComponents()
        EmployeeDataGridView.AutoGenerateColumns = False
    End Sub

    Public Sub LoadEmployees(employees As ICollection(Of EmployeeModel)) Implements IJobPointsView.LoadEmployees
        EmployeeDataGridView.DataSource = employees
    End Sub

    Private Sub EmployeeDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles EmployeeDataGridView.SelectionChanged
        Dim employeeModel = DirectCast(EmployeeDataGridView.CurrentRow.DataBoundItem, EmployeeModel)

        If employeeModel Is Nothing Then
            Return
        End If

        RaiseEvent JobPointsForm_EmployeeSelected(employeeModel)
    End Sub

    Private Sub EmployeeDataGridView_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles EmployeeDataGridView.CellValueChanged
        If e.RowIndex < 0 Then
            Return
        End If

        Dim model = DirectCast(EmployeeDataGridView.Rows(e.RowIndex).DataBoundItem, EmployeeModel)

        If model Is Nothing Then
            Return
        End If

        RaiseEvent JobPointsForm_EmployeeChanged(model)
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        RaiseEvent JobPointsForm_SaveEmployees()
    End Sub

    Public Sub ShowSavedMessage() Implements IJobPointsView.ShowSavedMessage
        InfoBalloon("Successfully saved.", "", EmployeeDataGridView, dispo:=1)
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        HRISForm.listHRISForm.Remove(Me.Name)
        Close()
    End Sub

End Class

Public Interface IJobPointsView

    Event OnLoad()

    Event EmployeeSelected(employee As EmployeeModel)

    Event EmployeeChanged(model As EmployeeModel)

    Event SaveEmployees()

    Sub LoadEmployees(employees As ICollection(Of EmployeeModel))

    Sub ShowSavedMessage()

End Interface