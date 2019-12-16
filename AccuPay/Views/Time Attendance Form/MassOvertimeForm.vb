Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore
Imports System.Data.Entity
Imports System.Threading.Tasks

Public Class MassOvertimeForm

    Private _presenter As MassOvertimePresenter

    Public ReadOnly Property DateFrom As Date
        Get
            Return FromDatePicker.Value
        End Get
    End Property

    Public ReadOnly Property DateTo As Date
        Get
            Return ToDatePicker.Value
        End Get
    End Property

    Public ReadOnly Property StartTime As TimeSpan?
        Get
            Return StartTimeTextBox.Value
        End Get
    End Property

    Public ReadOnly Property EndTime As TimeSpan?
        Get
            Return EndTimeTextBox.Value
        End Get
    End Property

    Public ReadOnly Property IsTimeValid As Boolean
        Get
            Return Not StartTimeTextBox.HasError And Not EndTimeTextBox.HasError
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
        OvertimeDataGridView.AutoGenerateColumns = False
        _presenter = New MassOvertimePresenter(Me)
    End Sub

    Public Sub ShowEmployees(divisions As IEnumerable(Of Division), employees As IEnumerable(Of Employee))
        EmployeeTreeView.BeginUpdate()
        EmployeeTreeView.Nodes.Clear()

        Dim parentDivisions = divisions.Where(Function(d) d.IsRoot)

        For Each parentDivision In parentDivisions
            Dim parentNode = New TreeNode() With {
                .Name = parentDivision.Name,
                .Text = parentDivision.Name,
                .Tag = parentDivision
            }

            Dim childDivisions = divisions.Where(Function(d) d.IsParent(parentDivision))
            For Each childDivision In childDivisions
                Dim childNode = New TreeNode() With {
                    .Name = childDivision.Name,
                    .Text = childDivision.Name,
                    .Tag = childDivision
                }

                Dim childEmployees = employees.
                    Where(Function(e) Nullable.Equals(e.Position?.Division.RowID, childDivision.RowID))
                For Each childEmployee In childEmployees
                    Dim employeeNode = New TreeNode() With {
                        .Name = childEmployee.FullNameWithMiddleInitialLastNameFirst,
                        .Text = childEmployee.FullNameWithMiddleInitialLastNameFirst,
                        .Tag = childEmployee
                    }

                    childNode.Nodes.Add(employeeNode)
                Next

                parentNode.Nodes.Add(childNode)
            Next

            EmployeeTreeView.Nodes.Add(parentNode)
        Next

        EmployeeTreeView.ExpandAll()
        EmployeeTreeView.EndUpdate()
    End Sub

    Private Sub EmployeeTreeView_AfterCheck(sender As Object, e As TreeViewEventArgs)
        RemoveHandler EmployeeTreeView.AfterCheck, AddressOf EmployeeTreeView_AfterCheck
        SetCheck(e.Node)
        SetParent(e.Node)
        _presenter.RefreshOvertime()
        AddHandler EmployeeTreeView.AfterCheck, AddressOf EmployeeTreeView_AfterCheck
    End Sub

    Public Function GetActiveEmployees() As IList(Of Employee)
        Dim list = New List(Of Employee)
        For Each node As TreeNode In EmployeeTreeView.Nodes
            TraverseNodes(node, list)
        Next
        Return list
    End Function

    Private Sub TraverseNodes(node As TreeNode, list As IList(Of Employee))
        If TypeOf node.Tag Is Employee And node.Checked Then
            list.Add(DirectCast(node.Tag, Employee))
        End If

        If node.GetNodeCount(False) >= 1 Then
            For Each child As TreeNode In node.Nodes
                TraverseNodes(child, list)
            Next
        End If
    End Sub

    Private Sub SetCheck(node As TreeNode)
        If node.GetNodeCount(False) >= 1 Then
            For Each childNode As TreeNode In node.Nodes
                childNode.Checked = node.Checked
                SetCheck(childNode)
            Next
        End If
    End Sub

    Private Sub SetParent(node As TreeNode)
        If node.Parent IsNot Nothing Then
            Dim areSiblingsSame = True
            For Each siblingNode As TreeNode In node.Parent.Nodes
                If siblingNode.Checked <> node.Checked Then
                    areSiblingsSame = False
                End If
            Next

            If areSiblingsSame And node.Checked Then
                node.Parent.Checked = True
            Else
                node.Parent.Checked = False
            End If

            SetParent(node.Parent)
        End If
    End Sub

    Private Sub MassOvertimeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _presenter.Load()
        AddHandler EmployeeTreeView.AfterCheck, AddressOf EmployeeTreeView_AfterCheck
        AddHandler EmployeeSearchTextBox.TextChanged, AddressOf EmployeeSearchTextBox_TextChanged
    End Sub

    Public Sub ShowOvertimes(overtimes As DataTable)
        OvertimeDataGridView.DataSource = overtimes
    End Sub

    Public Sub ShowOvertimes(overtimes As List(Of OvertimeModel))
        OvertimeDataGridView.DataSource = overtimes
    End Sub

    Private Sub FromDatePicker_ValueChanged(sender As Object, e As EventArgs) Handles FromDatePicker.ValueChanged
        _presenter.RefreshOvertime()
    End Sub

    Private Sub ToDatePicker_ValueChanged(sender As Object, e As EventArgs) Handles ToDatePicker.ValueChanged
        _presenter.RefreshOvertime()
    End Sub

    Public Sub RefreshDataGrid()
        OvertimeDataGridView.Refresh()
    End Sub

    Private Sub ApplyButton_Click(sender As Object, e As EventArgs) Handles ApplyButton.Click
        _presenter.ApplyToOvertimes()
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        SaveButton.Enabled = False
        Await _presenter.SaveOvertimes()
        _presenter.RefreshOvertime()
        SaveButton.Enabled = True
    End Sub

    Private Sub EmployeeSearchTextBox_TextChanged(sender As Object, e As EventArgs)
        _presenter.FilterEmployees(EmployeeSearchTextBox.Text)
    End Sub

End Class

Public Class MassOvertimePresenter

    Private _employees As IList(Of Employee)

    Private _divisions As IList(Of Division)

    Private _view As MassOvertimeForm

    Private _models As List(Of OvertimeModel)

    Public Sub New(view As MassOvertimeForm)
        _view = view
    End Sub

    Public Sub Load()
        _divisions = LoadDivisions()
        _employees = LoadEmployees()

        _view.ShowEmployees(_divisions, _employees)
    End Sub

    Private Function LoadDivisions() As IList(Of Division)
        Using context = New PayrollContext()
            Return context.Divisions.
                Where(Function(d) Nullable.Equals(d.OrganizationID, z_OrganizationID)).
                OrderBy(Function(d) d.Name).
                ToList()
        End Using
    End Function

    Private Function LoadEmployees() As IList(Of Employee)
        Using context = New PayrollContext()
            Return context.Employees.Include(Function(e) e.Position.Division).
                Where(Function(e) Nullable.Equals(e.OrganizationID, z_OrganizationID)).
                OrderBy(Function(e) e.LastName).
                ThenBy(Function(e) e.FirstName).
                ToList()
        End Using
    End Function

    Private Function LoadOvertimes(dateFrom As Date, dateTo As Date, employees As IList(Of Employee)) As IList(Of IGrouping(Of Integer?, Overtime))
        Dim employeeIds = employees.Select(Function(e) e.RowID).ToList()

        Using context = New PayrollContext()
            Return context.Overtimes.
                Where(Function(o) employeeIds.Contains(o.EmployeeID)).
                Where(Function(o) dateFrom.Date <= o.OTStartDate).
                Where(Function(o) o.OTStartDate <= dateTo.Date).
                GroupBy(Function(o) o.EmployeeID).
                ToList()
        End Using
    End Function

    Public Async Sub FilterEmployees(needle As String)
        Dim match =
            Function(employee As Employee) As Boolean
                Dim contains = employee.FullNameWithMiddleInitialLastNameFirst.ToLower().Contains(needle)
                Dim reverseName = ($"{employee.LastName} {employee.FirstName}").ToLower()
                Dim containsReverseName = reverseName.Contains(needle)

                Return contains Or containsReverseName
            End Function

        Dim employees = Await Task.Run(
            Function()
                Return _employees.Where(match).ToList()
            End Function)

        _view.ShowEmployees(_divisions, employees)
    End Sub

    Public Sub RefreshOvertime()
        Dim dateFrom = _view.DateFrom
        Dim dateTo = _view.DateTo
        Dim employees = _view.GetActiveEmployees()

        Dim overtimesByEmployee = LoadOvertimes(dateFrom, dateTo, employees)

        _models = New List(Of OvertimeModel)

        For Each employee In employees
            Dim overtimesOfEmployee = overtimesByEmployee.
                FirstOrDefault(Function(g) Nullable.Equals(g.Key, employee.RowID))

            For Each currentDate In Calendar.EachDay(dateFrom, dateTo)
                Dim overtime = overtimesOfEmployee?.
                    FirstOrDefault(Function(o) o.OTStartDate = currentDate)

                Dim model = New OvertimeModel() With {
                    .EmployeeID = employee.RowID,
                    .EmployeeNo = employee.EmployeeNo,
                    .Name = employee.FullNameWithMiddleInitialLastNameFirst,
                    .Date = currentDate,
                    .StartTime = overtime?.OTStartTime,
                    .EndTime = overtime?.OTEndTime,
                    .Overtime = overtime
                }

                _models.Add(model)
            Next
        Next

        _view.ShowOvertimes(_models)
    End Sub

    Public Sub ApplyToOvertimes()
        If Not _view.IsTimeValid Then
            Return
        End If

        For Each model In _models
            model.StartTime = _view.StartTime
            model.EndTime = _view.EndTime
        Next

        _view.RefreshDataGrid()
    End Sub

    Public Async Function SaveOvertimes() As Task
        Using context = New PayrollContext()
            For Each model In _models
                Dim overtime = model.Overtime

                If model.IsNew Then
                    overtime = New Overtime() With {
                        .EmployeeID = model.EmployeeID,
                        .OrganizationID = z_OrganizationID,
                        .CreatedBy = z_User,
                        .OTStartDate = model.Date,
                        .OTEndDate = model.Date,
                        .Status = Overtime.StatusApproved
                    }
                ElseIf model.IsUpdate Then
                    overtime.LastUpdBy = z_User
                End If

                If model.IsNew Or model.IsUpdate Then
                    overtime.OTStartTime = model.StartTime
                    overtime.OTEndTime = model.EndTime

                    If model.IsNew Then
                        context.Overtimes.Add(overtime)
                    Else
                        context.Entry(overtime).State = EntityState.Modified
                    End If
                ElseIf model.IsDelete Then
                    context.Overtimes.Remove(overtime)
                End If
            Next

            Await context.SaveChangesAsync()
        End Using
    End Function

End Class

Public Class OvertimeModel

    Public Property EmployeeID As Integer?

    Public Property EmployeeNo As String

    Public Property Name As String

    Public Property [Date] As Date

    Public Property StartTime As TimeSpan?

    Public Property EndTime As TimeSpan?

    Public Property Overtime As Overtime

    Public ReadOnly Property HasValue As Boolean
        Get
            Return StartTime IsNot Nothing Or EndTime IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property IsNew As Boolean
        Get
            Return Overtime Is Nothing And HasValue
        End Get
    End Property

    Public ReadOnly Property IsDelete As Boolean
        Get
            Return Overtime IsNot Nothing And (Not HasValue)
        End Get
    End Property

    Public ReadOnly Property IsUpdate As Boolean
        Get
            Return Overtime IsNot Nothing And HasValue
        End Get
    End Property

End Class
