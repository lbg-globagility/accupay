Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Repositories
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.Utils

Public Class AddDivisionForm

    Private _divisionRepository As New DivisionRepository

    Private _positionRepository As New PositionRepository

    Private _payFrequencyRepository As New PayFrequencyRepository

    Private _parentDivisions As List(Of Division)

    Private _newDivision As Division

    Private _positions As New List(Of Position)

    Private _payFrequencies As New List(Of Data.Entities.PayFrequency)

    Private _divisionTypes As List(Of String)

    Private _deductionSchedules As List(Of String)

    Public Property IsSaved As Boolean

    Public Property ShowBalloonSuccess As Boolean

    Public Property LastDivisionAdded As Division

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Me.IsSaved = False

        Me.LastDivisionAdded = New Division()

    End Sub

    Private Async Sub AddPositionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Await LoadDivisionList()

        Await LoadPositions()

        Await LoadPayFrequencies()

        GetDivisionTypes()

        GetDeductionSchedules()

        ResetForm()

    End Sub

    Private Async Function LoadDivisionList() As Task

        Dim divisions = Await _divisionRepository.GetAllParentsAsync()

        _parentDivisions = divisions.OrderBy(Function(d) d.Name).ToList

    End Function

    Private Async Function LoadPositions() As Task

        Dim positions = Await _positionRepository.GetAllAsync()

        _positions = positions.OrderBy(Function(p) p.Name).ToList

    End Function

    Private Async Function LoadPayFrequencies() As Task

        Dim payFrequencies = Await _payFrequencyRepository.GetAllAsync()

        _payFrequencies = payFrequencies.
                                Where(Function(p) p.RowID.Value = PayFrequencyType.SemiMonthly OrElse
                                    p.RowID.Value = PayFrequencyType.Weekly).ToList

    End Function

    Private Sub GetDivisionTypes()

        _divisionTypes = _divisionRepository.GetDivisionTypeList

    End Sub

    Private Sub GetDeductionSchedules()

        _deductionSchedules = ContributionSchedule.GetList()

    End Sub

    Private Sub ResetForm()

        Me._newDivision = Division.CreateEmptyDivision()

        DivisionUserControl1.SetDivision(Me._newDivision,
                                         _parentDivisions,
                                         Me._positions,
                                        Me._divisionTypes,
                                        Me._payFrequencies,
                                        Me._deductionSchedules)

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Async Sub AddDivisionButtonClicked(sender As Object, e As EventArgs) Handles btnAddAndNew.Click, btnAddAndClose.Click

        'removing focus to DivisionUserControl1 updates the databind
        DivisionUserControl1.Focus()

        Dim messageTitle = "Save Division"

        If ValidateDivision(messageTitle) = False Then

            Return

        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                          Async Function()
                              Await SaveDivision(messageTitle, sender)
                          End Function)

    End Sub

    Private Async Function SaveDivision(messageTitle As String, sender As Object) As Task

        Me.LastDivisionAdded = Await _divisionRepository.SaveAsync(Me._newDivision)

        Dim repo As New UserActivityRepository
        repo.RecordAdd(z_User, "Division", Me._newDivision.RowID.Value, z_OrganizationID)

        Me.IsSaved = True

        If sender Is btnAddAndNew Then

            ShowBalloonInfo($"Division: {Me._newDivision.Name} was successfully added.", "New Division", 0, -50)

            ResetForm()
        Else

            Me.ShowBalloonSuccess = True
            Me.Close()
        End If

    End Function

    Private Function ValidateDivision(messageTitle As String) As Boolean

        Dim xPosition As Integer = 0
        Dim yPosition As Integer = -80

        If Me._newDivision Is Nothing Then

            MessageBoxHelper.Warning("No division to be added!", messageTitle)
            Return False
        End If

        If String.IsNullOrWhiteSpace(Me._newDivision.Name) Then

            DivisionUserControl1.ShowError("Name", "Please enter a name", xPosition, yPosition)
            Return False
        End If

        If Me._newDivision.ParentDivisionID Is Nothing Then

            DivisionUserControl1.ShowError("ParentDivisionID", "Please select a parent division", xPosition, yPosition)
            Return False

        ElseIf Me._newDivision.GracePeriod Is Nothing OrElse Me._newDivision.WorkDaysPerYear < 0 Then

            DivisionUserControl1.ShowError("WorkDaysPerYear", "Please enter a valid Grace period in minutes", xPosition, yPosition)
            Return False

        ElseIf Me._newDivision.WorkDaysPerYear Is Nothing OrElse Me._newDivision.WorkDaysPerYear < 0 Then

            DivisionUserControl1.ShowError("WorkDaysPerYear", "Please enter a valid Number of days work per year", xPosition, yPosition)
            Return False

        End If

        Return True

    End Function

    Private Sub ShowBalloonInfo(content As String, title As String, Optional x As Integer = 0, Optional y As Integer = 0)
        myBalloon(content, title, DivisionUserControl1, x, y)
    End Sub

End Class