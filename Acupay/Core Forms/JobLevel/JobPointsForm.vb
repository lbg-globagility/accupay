Option Strict On

Imports AccuPay.JobLevels

Public Class JobPointsForm
    Implements JobPointsView

    Public Event JobPointsForm_OnLoad() Implements JobPointsView.OnLoad

    Public Event JobPointsForm_EmployeeSelected(employee As EmployeeModel) Implements JobPointsView.EmployeeSelected

    Public Event JobPointsForm_EmployeeChanged(model As EmployeeModel) Implements JobPointsView.EmployeeChanged

    Public Event JobPointsForm_SaveEmployees() Implements JobPointsView.SaveEmployees

    Public Sub New()
        InitializeComponent()
        Dim presenter = New JobPointsPresenter(Me)
    End Sub

    Private Sub JobPointsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeComponents()
        RaiseEvent JobPointsForm_OnLoad()
    End Sub

    Private Sub InitializeComponents()
        EmployeeDataGridView.AutoGenerateColumns = False
    End Sub

    Public Sub LoadEmployees(employees As ICollection(Of EmployeeModel)) Implements JobPointsView.LoadEmployees
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

    Public Sub ShowSavedMessage() Implements JobPointsView.ShowSavedMessage
        InfoBalloon("Successfully saved.", "", EmployeeDataGridView, dispo:=1)
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        HRISForm.listHRISForm.Remove(Me.Name)
        Close()
    End Sub

End Class

Public Interface JobPointsView

    Event OnLoad()

    Event EmployeeSelected(employee As EmployeeModel)
    Event EmployeeChanged(model As EmployeeModel)
    Event SaveEmployees()

    Sub LoadEmployees(employees As ICollection(Of EmployeeModel))
    Sub ShowSavedMessage()

End Interface
