Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddDisciplinaryAction
    Public Property IsSaved As Boolean
    Public Property ShowBalloon As Boolean

    Private _employee As Employee

    Private _newDisciplinaryAction As DisciplinaryAction

    Private _findingNames As IEnumerable(Of Product)

    Private _actions As IEnumerable(Of ListOfValue)

    Private ReadOnly _listOfValRepo As IListOfValueRepository

    Private ReadOnly _productRepo As IProductRepository

    Public Sub New(employee As Employee)

        InitializeComponent()

        _employee = employee

        _listOfValRepo = MainServiceProvider.GetRequiredService(Of IListOfValueRepository)

        _productRepo = MainServiceProvider.GetRequiredService(Of IProductRepository)
    End Sub

    Private Async Sub AddDisciplinaryAction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFullName.Text = _employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(_employee.Image)

        Await BindDataSource()
        ClearForm()
    End Sub

    Private Async Function BindDataSource() As Task
        _findingNames = Await _productRepo.GetDisciplinaryTypesAsync(z_OrganizationID)

        _actions = Await _listOfValRepo.GetEmployeeDisciplinaryPenalties()

        cboFinding.DataSource = _findingNames
        cboAction.DataSource = _actions
    End Function

    Private Async Sub AddAndCloseButton_Click(sender As Object, e As EventArgs) Handles AddAndCloseButton.Click, AddAndNewButton.Click
        pbEmployee.Focus() 'To lose focus on current control
        Dim succeed As Boolean
        Dim messageBody = ""

        If cboFinding.SelectedItem Is Nothing Then
            messageBody = "Finding Name is empty."
        ElseIf cboAction.SelectedItem Is Nothing Then
            messageBody = "Actione is empty."
        End If

        If messageBody <> "" Then
            ShowBalloonInfo(messageBody, "Invalid Input")
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("New Disciplinary Action",
            Async Function()
                Dim currentFinding As Product = CType(cboFinding.SelectedItem, Product)

                _newDisciplinaryAction = New DisciplinaryAction
                With _newDisciplinaryAction
                    .FindingID = currentFinding.RowID.Value
                    .Action = cboAction.Text
                    .DateFrom = dtpEffectiveFrom.Value
                    .DateTo = dtpEffectiveTo.Value
                    .FindingDescription = txtDescription.Text
                    .Comments = txtComments.Text
                    .OrganizationID = z_OrganizationID
                    .EmployeeID = _employee.RowID.Value
                End With

                Dim dataService = MainServiceProvider.GetRequiredService(Of IDisciplinaryActionDataService)
                Await dataService.SaveAsync(_newDisciplinaryAction, z_User)

                succeed = True
            End Function)

        If succeed Then
            IsSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Disciplinary Action successfully added.", "Saved")
                ClearForm()
            Else
                ShowBalloon = True
                Me.Close()
            End If
        End If

    End Sub

    Private Sub ClearForm()
        cboFinding.SelectedItem = Nothing
        cboAction.SelectedItem = Nothing
        dtpEffectiveFrom.Value = Today
        dtpEffectiveTo.Value = Today
        txtDescription.Clear()
        txtComments.Clear()
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 70, -74)
    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Sub Dates_ValueChanged(sender As Object, e As EventArgs) Handles dtpEffectiveFrom.ValueChanged, dtpEffectiveTo.ValueChanged
        If dtpEffectiveTo.Value < dtpEffectiveFrom.Value Then
            dtpEffectiveTo.Value = dtpEffectiveFrom.Value
        End If
    End Sub

    Private Async Sub lblAddFindingname_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddFindingname.LinkClicked

        Dim form As New NewProductDisciplinaryForm(_productRepo)
        With form
            .ShowDialog()
        End With
        Await RefreshDatasourceRetainSelection()
    End Sub

    Private Async Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked

        Dim form As New NewListOfValDisciplinaryPenaltyForm()
        With form
            .ShowDialog()
        End With
        Await RefreshDatasourceRetainSelection()
    End Sub

    Private Async Function RefreshDatasourceRetainSelection() As Task
        Dim currentFinding = cboFinding.Text
        Dim currentAction = cboAction.Text
        Await BindDataSource()
        cboFinding.Text = currentFinding
        cboAction.Text = currentAction
    End Function

End Class
