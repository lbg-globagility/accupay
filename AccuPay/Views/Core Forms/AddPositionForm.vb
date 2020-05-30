Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utils
Imports Microsoft.Extensions.DependencyInjection

Public Class AddPositionForm

    Private Const FormEntityName As String = "Position"

    Private _divisions As List(Of Division)

    Private _jobLevels As List(Of JobLevel)

    Private _newPosition As Position

    Public Property IsSaved As Boolean

    Public Property ShowBalloonSuccess As Boolean

    Public Property LastPositionAdded As Position

    Private _divisionService As DivisionDataService

    Private _userActivityRepository As UserActivityRepository

    Sub New()

        InitializeComponent()

        _divisionService = MainServiceProvider.GetRequiredService(Of DivisionDataService)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

        Me.IsSaved = False

        Me.LastPositionAdded = New Position()

    End Sub

    Private Async Sub AddPositionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Await LoadDivisionList()

        ResetForm()

    End Sub

    Private Async Function LoadDivisionList() As Task

        Dim divisions = Await _divisionService.GetAllAsync(z_OrganizationID)

        _divisions = divisions.OrderBy(Function(d) d.Name).ToList

    End Function

    Private Sub ResetForm()

        Me._newPosition = New Position
        Me._newPosition.OrganizationID = z_OrganizationID
        Me._newPosition.CreatedBy = z_User

        Dim allChildDivisions = _divisions.Where(Function(d) d.IsRoot = False).ToList

        PositionUserControl1.SetPosition(Me._newPosition, allChildDivisions, _jobLevels)

    End Sub

    Private Function ValidatePosition(messageTitle As String) As Boolean

        Dim xPosition As Integer = 0
        Dim yPosition As Integer = -80

        If Me._newPosition Is Nothing Then

            MessageBoxHelper.Warning("No position to be added!", messageTitle)
            Return False

        ElseIf Me._newPosition.DivisionID Is Nothing Then

            PositionUserControl1.ShowError("DivisionID", "Please select a division", xPosition, yPosition)
            Return False

        ElseIf String.IsNullOrWhiteSpace(Me._newPosition.Name) Then

            PositionUserControl1.ShowError("Name", "Please enter a name", xPosition, yPosition)
            Return False

        End If

        Return True

    End Function

    Private Async Sub AddPositionButtonClicked(sender As Object, e As EventArgs) Handles btnAddAndNew.Click, btnAddAndClose.Click

        Dim messageTitle = "Save Position"

        PositionUserControl1.Focus()

        If ValidatePosition(messageTitle) = False Then

            Return

        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                          Async Function()
                              Await SavePosition(sender)
                          End Function)

    End Sub

    Private Async Function SavePosition(sender As Object) As Task

        Dim positionService = MainServiceProvider.GetRequiredService(Of PositionDataService)
        Await positionService.SaveAsync(Me._newPosition)

        Me.LastPositionAdded = Me._newPosition

        _userActivityRepository.RecordAdd(z_User, FormEntityName, Me._newPosition.RowID.Value, z_OrganizationID)

        Me.IsSaved = True

        If sender Is btnAddAndNew Then

            ShowBalloonInfo($"Position: {Me._newPosition.Name} was successfully added.", "New Position", 0, -80)

            ResetForm()
        Else

            Me.ShowBalloonSuccess = True
            Me.Close()
        End If

    End Function

    Private Sub ShowBalloonInfo(content As String, title As String, Optional x As Integer = 0, Optional y As Integer = 0)
        myBalloon(content, title, PositionUserControl1, x, y)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class