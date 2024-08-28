Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services.Imports.OfficialBusiness
Imports DevComponents.AdvTree
Imports Microsoft.Extensions.DependencyInjection
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading.Tasks

Public Class OvertimeApprovalForm
    Private _presenter As OTApprovalFormPresenter
    Private _model As IList(Of OvertimeModel)
    Private _tickedIDs As IList(Of Integer)
    Private ReadOnly _overtimeRepository As IOvertimeRepository
    Public Sub New()
        InitializeComponent()
        _presenter = New OTApprovalFormPresenter(Me)
        _tickedIDs = New List(Of Integer)
        _model = New List(Of OvertimeModel)
        _overtimeRepository = MainServiceProvider.GetRequiredService(Of IOvertimeRepository)

    End Sub

    Private Async Sub ApprovalForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await _presenter.Load()
    End Sub

    Public Async Sub ShowList(list As IEnumerable(Of Overtime))
        OBDataGridView.DataSource = Nothing
        _model.Clear()
        Dim beforeThreeDays = Date.Now.AddDays(-3)
        OBDataGridView.AutoGenerateColumns = False
        list = list.Where(Function(d) d.OTStartDate >= beforeThreeDays)
        For Each detail In list
            Dim data = New OvertimeModel() With {
                        .RowID = detail.RowID,
                        .EmployeeID = detail.EmployeeID,
                        .EmployeeName = detail.Employee.FullNameWithMiddleInitial,
                        .StartDate = detail.OTStartDate,
                        .EndDate = detail.OTEndDate,
                        .StartTime = detail.OTStartTime,
                        .EndTime = detail.OTEndTime,
                        .Comment = detail.Comments,
                        .Reason = detail.Reason,
                        .Status = detail.Status,
                        .Checked = _tickedIDs.Any(Function(eID) Nullable.Equals(eID, detail.RowID))
                    }
            _model.Add(data)
        Next
        OBDataGridView.DataSource = _model
    End Sub

    Private Class OvertimeModel

        Public Property RowID As Integer
        Public Property Checked As Boolean
        Public Property EmployeeID As String
        Public Property EmployeeName As String
        Public Property StartDate As Date
        Public Property EndDate As Date
        Public Property StartTime As TimeSpan?
        Public Property EndTime As TimeSpan?
        Public Property Reason As String
        Public Property Comment As String
        Public Property Status As String

    End Class

    Private Async Sub ApproveSelectedBtn_Click(sender As Object, e As EventArgs) Handles ApproveSelectedBtn.Click

        Await _overtimeRepository.ApproveOvertimes(_tickedIDs)

        EmployeeSearchTextbox.Text = ""
        OBSelectAllCheckbox.Checked = False
        _tickedIDs.Clear()
        Await _presenter.Load()
    End Sub
    Private Async Sub DataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles OBDataGridView.CellClick
        Dim checkboxCell = OBDataGridView.Item("Checked", e.RowIndex)
        Dim rowIDCell = OBDataGridView.Item("RowID", e.RowIndex)
        Dim rowCheckBoxValue As Boolean = checkboxCell.Value
        checkboxCell.Value = Not rowCheckBoxValue
        If rowCheckBoxValue Then
            _tickedIDs.Remove(rowIDCell.Value)
        Else
            _tickedIDs.Add(rowIDCell.Value)
        End If


    End Sub

    Private Sub EmployeeSearchTextbox_TextChanged(sender As Object, e As EventArgs) Handles EmployeeSearchTextbox.TextChanged
        _presenter.FilterEmployees(EmployeeSearchTextbox.Text)
    End Sub

    Private Async Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles OBSelectAllCheckbox.CheckedChanged
        EmployeeSearchTextbox.Text = ""
        If (OBSelectAllCheckbox.Checked) Then

            _tickedIDs = _model.Select(Function(z) z.RowID).ToList
        Else
            _tickedIDs.Clear()
        End If

        Await _presenter.Load()
    End Sub

    Private Async Sub RefreshBtn_Click(sender As Object, e As EventArgs) Handles OBRefreshBtn.Click
        EmployeeSearchTextbox.Text = ""
        OBSelectAllCheckbox.Checked = False
        _tickedIDs.Clear()
        Await _presenter.Load()
    End Sub

End Class
Public Class OTApprovalFormPresenter

    Private _view As OvertimeApprovalForm

    Private _entity As IList(Of Overtime)

    Private ReadOnly _repository As IOvertimeRepository
    Public Sub New(view As OvertimeApprovalForm)
        _view = view
        _repository = MainServiceProvider.GetRequiredService(Of IOvertimeRepository)
    End Sub
    Public Async Function Load() As Task
        _entity = Await LoadList()
        _view.ShowList(_entity)

    End Function

    Private Async Function LoadList() As Task(Of IList(Of Overtime))

        Return (Await _repository.GetPendingOTWithEmployee).
            ToList()
    End Function
    Public Async Function FilterEmployees(needle As String) As Task
        Dim match =
            Function(model As Overtime) As Boolean
                Dim contains = model.Employee.FullNameWithMiddleInitialLastNameFirst.ToLower().Contains(needle)
                Dim reverseName = ($"{model.Employee.LastName} {model.Employee.FirstName}").ToLower()
                Dim containsReverseName = reverseName.Contains(needle)

                Return contains Or containsReverseName
            End Function

        Dim result = Await Task.Run(
                Function()
                    Return _entity.Where(match).ToList()
                End Function)
        _view.ShowList(result)
    End Function
End Class
