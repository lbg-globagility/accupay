Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports DevComponents.AdvTree
Imports Microsoft.Extensions.DependencyInjection
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading.Tasks

Public Class LeaveApprovalForm
    Private _presenter As LeaveApprovalFormPresenter
    Private _leaveModel As IList(Of LeaveModel)
    Private _selectAll As Boolean
    Private _tickedLeaveIDs As IList(Of Integer)
    Public Sub New()
        InitializeComponent()
        _presenter = New LeaveApprovalFormPresenter(Me)
        _tickedLeaveIDs = New List(Of Integer)
        _leaveModel = New List(Of LeaveModel)
        _selectAll = False

    End Sub

    Private Async Sub LeaveApprovalForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await _presenter.Load()
        'AddHandler LeavesDataGridView.CellClick, AddressOf LeavesDataGridView_SelectionChanged
    End Sub

    Public Async Sub ShowLeaves(leaves As IEnumerable(Of Leave))
        LeavesDataGridView.DataSource = Nothing
        _leaveModel.Clear()
        Dim beforeThreeDays = Date.Now.AddDays(-3)
        LeavesDataGridView.AutoGenerateColumns = False
        leaves = leaves.Where(Function(d) d.StartDate >= beforeThreeDays)
        For Each detail In leaves
            Dim leaveNode = New LeaveModel() With {
                        .RowID = detail.RowID,
                        .EmployeeID = detail.EmployeeID,
                        .EmployeeName = detail.Employee.FullNameWithMiddleInitial,
                        .StartDate = detail.StartDate,
                        .EndDate = detail.EndDate,
                        .StartTime = detail.StartTime,
                        .EndTime = detail.EndTime,
                        .LeaveType = detail.LeaveType,
                        .Comment = detail.Comments,
                        .Reason = detail.Reason,
                        .Status = detail.Status,
                        .Checked = _tickedLeaveIDs.Any(Function(eID) Nullable.Equals(eID, detail.RowID))
                    }
            _leaveModel.Add(leaveNode)
        Next
        LeavesDataGridView.DataSource = _leaveModel
    End Sub

    Private Class LeaveModel

        Public Property RowID As Integer
        Public Property Checked As Boolean
        Public Property EmployeeID As String
        Public Property EmployeeName As String
        Public Property LeaveType As String
        Public Property StartDate As Date
        Public Property EndDate As Date
        Public Property StartTime As TimeSpan?
        Public Property EndTime As TimeSpan?
        Public Property Reason As String
        Public Property Comment As String
        Public Property Status As String

    End Class

    Private Sub ApproveSelectedBtn_Click(sender As Object, e As EventArgs) Handles ApproveSelectedBtn.Click
        Dim asas = _tickedLeaveIDs


        'ApproveLeave(tickedLeaveIDs)
    End Sub
    Private Async Sub LeavesDataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles LeavesDataGridView.CellClick
        Dim checkboxCell = LeavesDataGridView.Item("Checked", e.RowIndex)
        Dim rowIDCell = LeavesDataGridView.Item("RowID", e.RowIndex)
        Dim rowCheckBoxValue As Boolean = checkboxCell.Value
        checkboxCell.Value = Not rowCheckBoxValue
        If rowCheckBoxValue Then
            _tickedLeaveIDs.Remove(rowIDCell.Value)
        Else
            _tickedLeaveIDs.Add(rowIDCell.Value)
        End If


    End Sub

    Private Sub EmployeeSearchTextbox_TextChanged(sender As Object, e As EventArgs) Handles EmployeeSearchTextbox.TextChanged
        _presenter.FilterEmployees(EmployeeSearchTextbox.Text)
    End Sub

    Private Async Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles LeaveSelectAllCheckbox.CheckedChanged
        _selectAll = LeaveSelectAllCheckbox.Checked
        EmployeeSearchTextbox.Text = ""
        If (LeaveSelectAllCheckbox.Checked) Then

            _tickedLeaveIDs = _leaveModel.Select(Function(z) z.RowID).ToList
        Else
            _tickedLeaveIDs.Clear()
        End If

        Await _presenter.Load()
    End Sub

    Private Async Sub LeaveRefreshBtn_Click(sender As Object, e As EventArgs) Handles LeaveRefreshBtn.Click
        EmployeeSearchTextbox.Text = ""
        LeaveSelectAllCheckbox.Checked = False
        _tickedLeaveIDs.Clear()
        Await _presenter.Load()
    End Sub

End Class
Public Class LeaveApprovalFormPresenter

    Private _view As LeaveApprovalForm

    Private _leaves As IList(Of Leave)

    Private ReadOnly _leaveRepository As ILeaveRepository
    Public Sub New(view As LeaveApprovalForm)
        _view = view
        _leaveRepository = MainServiceProvider.GetRequiredService(Of ILeaveRepository)
    End Sub
    Public Async Function Load() As Task
        _leaves = Await LoadLeaves()
        _view.ShowLeaves(_leaves)

    End Function

    Private Async Function LoadLeaves() As Task(Of IList(Of Leave))

        Return (Await _leaveRepository.
            GetLeaveWithEmployee()).
            ToList()
    End Function
    Public Async Function FilterEmployees(needle As String) As Task
        Dim match =
            Function(leave As Leave) As Boolean
                Dim contains = leave.Employee.FullNameWithMiddleInitialLastNameFirst.ToLower().Contains(needle)
                Dim reverseName = ($"{leave.Employee.LastName} {leave.Employee.FirstName}").ToLower()
                Dim containsReverseName = reverseName.Contains(needle)

                Return contains Or containsReverseName
            End Function

        Dim leaves = Await Task.Run(
                Function()
                    Return _leaves.Where(match).ToList()
                End Function)
        _view.ShowLeaves(leaves)
    End Function
End Class
