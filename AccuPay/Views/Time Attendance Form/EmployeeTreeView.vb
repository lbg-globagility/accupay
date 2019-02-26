Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore
Imports System.Collections.ObjectModel
Imports System.Data.Entity
Imports System.Threading.Tasks

Public Class EmployeeTreeView

    Private _presenter As EmployeeTreeViewPresenter

    Public Event OrganizationIDChanged(s As Object, e As EventArgs)

    Public Event FiltersEmployee(s As Object, e As EventArgs)

    Public Property OrganizationID As Integer
        Get

        End Get
        Set(value As Integer)
            WhenOrganizationIdChanged(value)
        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _presenter = New EmployeeTreeViewPresenter(Me)
    End Sub

    Private Sub EmployeeTreeView_Load(sender As Object, e As EventArgs) Handles Me.Load
        If DesignMode Then
            Return
        End If

        _presenter.Load()
        AddHandler AccuPayEmployeeTreeView.AfterCheck, AddressOf EmployeeTreeView_AfterCheck
    End Sub

    Private Sub WhenOrganizationIdChanged(organizationId As Integer)
        If DesignMode Then
            Return
        End If

        RaiseEvent OrganizationIDChanged(Me, New EventArgs)
        _presenter.Load(organizationId)
    End Sub

    Private Sub EmployeeTreeView_AfterCheck(sender As Object, e As TreeViewEventArgs)
        RemoveHandler AccuPayEmployeeTreeView.AfterCheck, AddressOf EmployeeTreeView_AfterCheck
        SetCheck(e.Node)
        SetParent(e.Node)
        AddHandler AccuPayEmployeeTreeView.AfterCheck, AddressOf EmployeeTreeView_AfterCheck
    End Sub

    Public Function GetActiveEmployees() As IList(Of Employee)
        Return _presenter.GetActiveEmployees
    End Function

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

    Public Sub ShowEmployees(divisions As IEnumerable(Of Division), employees As IEnumerable(Of Employee))
        AccuPayEmployeeTreeView.BeginUpdate()
        AccuPayEmployeeTreeView.Nodes.Clear()

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
                        .Name = childEmployee.Fullname,
                        .Text = childEmployee.Fullname,
                        .Tag = childEmployee
                    }

                    childNode.Nodes.Add(employeeNode)
                Next

                parentNode.Nodes.Add(childNode)
            Next

            AccuPayEmployeeTreeView.Nodes.Add(parentNode)
        Next

        AccuPayEmployeeTreeView.ExpandAll()
        AccuPayEmployeeTreeView.EndUpdate()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        _presenter.FilterEmployees(TextBox1.Text)

        RaiseEvent FiltersEmployee(e, New EventArgs)
    End Sub

    Private Class EmployeeTreeViewPresenter

        Private _employees As IList(Of Employee)

        Private _divisions As IList(Of Division)

        Private _view As EmployeeTreeView

        Private _models As List(Of OvertimeModel)

        Private _organizationId As Integer

        Public Sub New(view As EmployeeTreeView)
            _view = view
            _organizationId = view.OrganizationID
        End Sub

        Public Sub Load()
            Reload()
        End Sub

        Public Sub Load(organizationId As Integer)
            _organizationId = organizationId
            Reload()
        End Sub

        Private Sub Reload()
            _divisions = LoadDivisions()
            _employees = LoadEmployees()

            _view.ShowEmployees(_divisions, _employees)
        End Sub

        Private Function LoadDivisions() As IList(Of Division)
            Using context = New PayrollContext()
                Return context.Divisions.
                    Where(Function(d) Nullable.Equals(d.OrganizationID, _organizationId)).
                    ToList()
            End Using
        End Function

        Private Function LoadEmployees() As IList(Of Employee)
            Using context = New PayrollContext()
                Return context.Employees.Include(Function(e) e.Position.Division).
                    Where(Function(e) Nullable.Equals(e.OrganizationID, _organizationId)).
                    OrderBy(Function(e) e.LastName).
                    ThenBy(Function(e) e.FirstName).
                    ToList()
            End Using
        End Function

        Public Async Sub FilterEmployees(needle As String)
            Dim match =
                Function(employee As Employee) As Boolean
                    Dim contains = employee.Fullname.ToLower().Contains(needle)
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

        Public Function GetActiveEmployees() As IList(Of Employee)
            Dim list = New List(Of Employee)
            For Each node As TreeNode In _view.AccuPayEmployeeTreeView.Nodes
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

    End Class

End Class
