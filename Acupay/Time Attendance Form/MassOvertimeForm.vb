Imports AccuPay.Entity
Imports AccuPay.Tools
Imports System.Data.Entity

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

    Public Sub New()
        InitializeComponent()
        _presenter = New MassOvertimePresenter(Me)
    End Sub

    Public Sub ShowEmployees(divisions As ICollection(Of Division), employees As ICollection(Of Employee))
        UserTreeView.BeginUpdate()
        UserTreeView.Nodes.Clear()

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
                    Where(Function(e) e.Position.Division.RowID = childDivision.RowID)
                For Each childEmployee In childEmployees
                    Dim employeeNode = New TreeNode() With {
                        .Name = childEmployee.Fullname,
                        .Text = childEmployee.Fullname,
                        .Tag = childEmployee
                    }

                    childNode.Nodes.Add(employeeNode)
                Next

                parentNode.Nodes.Add(childNode)
            Next

            UserTreeView.Nodes.Add(parentNode)
        Next

        UserTreeView.ExpandAll()
        UserTreeView.EndUpdate()
    End Sub

    Private Sub UserTreeView_AfterCheck(sender As Object, e As TreeViewEventArgs)
        RemoveHandler UserTreeView.AfterCheck, AddressOf UserTreeView_AfterCheck
        SetCheck(e.Node)
        SetParent(e.Node)
        _presenter.RefreshOvertime()
        AddHandler UserTreeView.AfterCheck, AddressOf UserTreeView_AfterCheck
    End Sub

    Public Function GetActiveEmployees() As IList(Of Employee)
        Dim list = New List(Of Employee)
        For Each node As TreeNode In UserTreeView.Nodes
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
        AddHandler UserTreeView.AfterCheck, AddressOf UserTreeView_AfterCheck
    End Sub

    Public Function CreateDataTable() As DataTable
        Dim dataTable = New DataTable()

        dataTable.Columns.Add("EmployeeNo", GetType(String))
        dataTable.Columns.Add("Name", GetType(String))
        dataTable.Columns.Add("Date", GetType(Date))
        dataTable.Columns.Add("OTStart", GetType(TimeSpan))
        dataTable.Columns.Add("OTEnd", GetType(TimeSpan))

        Return dataTable
    End Function

    Public Sub ShowOvertimes(overtimes As DataTable)
        OvertimeDataGridView.DataSource = overtimes
    End Sub

    Private Sub FromDatePicker_ValueChanged(sender As Object, e As EventArgs) Handles FromDatePicker.ValueChanged
        _presenter.RefreshOvertime()
    End Sub

    Private Sub ToDatePicker_ValueChanged(sender As Object, e As EventArgs) Handles ToDatePicker.ValueChanged
        _presenter.RefreshOvertime()
    End Sub

End Class

Public Class MassOvertimePresenter

    Private _view As MassOvertimeForm

    Public Sub New(view As MassOvertimeForm)
        _view = view
    End Sub

    Public Sub Load()
        Dim divisions = LoadDivisions()
        Dim employees = LoadEmployees()

        _view.ShowEmployees(divisions, employees)
    End Sub

    Private Function LoadDivisions() As ICollection(Of Division)
        Using context = New PayrollContext()
            Return context.Divisions.
                Where(Function(d) d.OrganizationID = z_OrganizationID).
                ToList()
        End Using
    End Function

    Private Function LoadEmployees() As ICollection(Of Employee)
        Using context = New PayrollContext()
            Return context.Employees.Include(Function(e) e.Position.Division).
                Where(Function(e) e.OrganizationID = z_OrganizationID).
                OrderBy(Function(e) e.LastName).
                ThenBy(Function(e) e.FirstName).
                ToList()
        End Using
    End Function

    Private Function LoadOvertimes(dateFrom As Date, dateTo As Date, employees As IList(Of Employee)) As IList(Of IGrouping(Of Integer?, Overtime))
        Dim employeeIds = employees.Select(Function(e) e.RowID).ToList()

        Using context = New PayrollContext()
            Return context.Overtimes.
                Where(Function(o) dateFrom <= o.OTStartDate And o.OTStartDate <= dateTo).
                Where(Function(o) employeeIds.Contains(o.EmployeeID)).
                GroupBy(Function(o) o.EmployeeID).
                ToList()
        End Using
    End Function

    Public Sub RefreshOvertime()
        Dim dateFrom = _view.DateFrom
        Dim dateTo = _view.DateTo
        Dim employees = _view.GetActiveEmployees()

        Dim data = _view.CreateDataTable()

        Dim overtimesByEmployee = LoadOvertimes(dateFrom, dateTo, employees)

        For Each employee In employees
            Dim overtimesOfEmployee = overtimesByEmployee.
                FirstOrDefault(Function(g) g.Key = employee.RowID)

            For Each currentDate In EachDay(dateFrom, dateTo)
                Dim overtime = overtimesOfEmployee?.
                    FirstOrDefault(Function(o) o.OTStartDate = currentDate)

                Dim newRow = data.NewRow()
                newRow.Item("EmployeeNo") = employee.EmployeeNo
                newRow.Item("Name") = employee.Fullname
                newRow.Item("Date") = currentDate
                newRow.Item("OTStart") = If(overtime?.OTStartTime, DBNull.Value)
                newRow.Item("OTEnd") = If(overtime?.OTEndTime, DBNull.Value)
                data.Rows.Add(newRow)
            Next
        Next

        _view.ShowOvertimes(data)
    End Sub

    Public Iterator Function EachDay(from As DateTime, thru As DateTime) As IEnumerable(Of DateTime)
        Dim day = from.Date
        While day.Date <= thru.Date
            Yield day
            day = day.AddDays(1)
        End While
    End Function

End Class
