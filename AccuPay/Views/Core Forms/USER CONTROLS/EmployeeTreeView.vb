Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class EmployeeTreeView

#Region "VariableDeclarations"

    Private _presenter As EmployeeTreeViewPresenter

    Public Event OrganizationIDChanged(s As Object, e As EventArgs)

    Public Event FiltersEmployee(s As Object, e As EventArgs)

    Public Event EmployeeTicked(s As Object, e As EventArgs)

    Private tickedEmployees As IList(Of Employee)

    Private tickedEmployeeIDs As IList(Of Integer)

    Private _organizationID As Integer

    Public ReadOnly Property OrganizationID As Integer
        Get
            Return _organizationID
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()

        InitializeComponent()

        _presenter = New EmployeeTreeViewPresenter(Me)
        tickedEmployees = New List(Of Employee)
        tickedEmployeeIDs = New List(Of Integer)
    End Sub

#End Region

#Region "Methods"

    Public Async Function SwitchOrganization(organizationId As Integer) As Task
        _organizationID = organizationId
        Await WhenOrganizationIdChanged(organizationId)
    End Function

    Private Async Function WhenOrganizationIdChanged(organizationId As Integer) As Task
        If DesignMode Then
            Return
        End If

        RaiseEvent OrganizationIDChanged(Me, New EventArgs)
        Await _presenter.Load(organizationId)
    End Function

    Private Sub EmployeeTreeView_AfterCheck(sender As Object, e As TreeViewEventArgs)
        RemoveHandler AccuPayEmployeeTreeView.AfterCheck, AddressOf EmployeeTreeView_AfterCheck
        SetCheck(e.Node)
        SetParent(e.Node)

        CollectTickedEmployees(e.Node)

        AddHandler AccuPayEmployeeTreeView.AfterCheck, AddressOf EmployeeTreeView_AfterCheck
    End Sub

    Private Sub CollectTickedEmployees(tickedNode As TreeNode)
        _presenter.TraverseNodes(tickedNode, tickedEmployees)
        tickedEmployeeIDs = tickedEmployees.Select(Function(e) e.RowID.Value).ToList

        RaiseEvent EmployeeTicked(Me, New EventArgs)
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

    Public Sub ShowEmployees(divisions As IEnumerable(Of Division), employees As IEnumerable(Of Employee))
        AccuPayEmployeeTreeView.BeginUpdate()
        AccuPayEmployeeTreeView.Nodes.Clear()

        Dim rootNode = New TreeNode With {
            .Name = "$root",
            .Text = "All"
        }

        Dim parentDivisions = divisions.Where(Function(d) d.IsRoot)

        For Each parentDivision In parentDivisions
            Dim parentNode = New TreeNode() With {
                .Name = parentDivision.Name,
                .Text = parentDivision.Name,
                .Tag = parentDivision
            }

            Dim childDivisions = divisions.Where(Function(d) d.IsParent(parentDivision))

            If childDivisions.Any() Then
                parentNode.Text = $"{parentNode.Text} ({childDivisions.Count()})"
            End If

            For Each childDivision In childDivisions
                Dim childNode = New TreeNode() With {
                    .Name = childDivision.Name,
                    .Text = childDivision.Name,
                    .Tag = childDivision
                }

                Dim childEmployees = employees.
                    Where(Function(e) Nullable.Equals(e.Position?.Division?.RowID, childDivision.RowID))

                If Not childEmployees.Any() Then
                    Continue For
                End If

                childNode.Text = $"{childNode.Text} ({childEmployees.Count()})"

                For Each childEmployee In childEmployees
                    Dim shouldSetChecked = tickedEmployeeIDs.Any(Function(eID) Nullable.Equals(eID, childEmployee.RowID))

                    Dim employeeNode = New TreeNode() With {
                        .Name = childEmployee.FullNameWithMiddleInitialLastNameFirst,
                        .Text = $"{childEmployee.FullNameWithMiddleInitialLastNameFirst} #{childEmployee.EmployeeNo}",
                        .Tag = childEmployee,
                        .Checked = shouldSetChecked
                    }

                    childNode.Nodes.Add(employeeNode)
                Next

                parentNode.Nodes.Add(childNode)
            Next

            If parentNode.Nodes.Count < 1 Then
                Continue For
            End If

            rootNode.Nodes.Add(parentNode)
        Next

        AccuPayEmployeeTreeView.Nodes.Add(rootNode)

        AccuPayEmployeeTreeView.ExpandAll()
        AccuPayEmployeeTreeView.EndUpdate()
        AccuPayEmployeeTreeView.Nodes(0).EnsureVisible()
    End Sub

#End Region

#Region "Functions"

    Public Function GetTickedEmployees() As IList(Of Employee)
        Return tickedEmployees
    End Function

#End Region

#Region "EventHandlers"

    Private Async Sub EmployeeTreeView_Load(sender As Object, e As EventArgs) Handles Me.Load
        If DesignMode Then
            Return
        End If

        Await _presenter.Load(z_OrganizationID)
        AddHandler AccuPayEmployeeTreeView.AfterCheck, AddressOf EmployeeTreeView_AfterCheck
    End Sub

    Private Async Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        If _presenter Is Nothing Then Return

        Await _presenter.FilterEmployees(TextBox1.Text, chkActive.Checked)

        RaiseEvent FiltersEmployee(e, New EventArgs)
    End Sub

    Private Async Sub ChkActiveOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkActive.CheckedChanged

        If _presenter Is Nothing Then Return

        Await _presenter.FilterEmployees(TextBox1.Text, chkActive.Checked)

        RaiseEvent FiltersEmployee(e, New EventArgs)
    End Sub

#End Region

    Private Class EmployeeTreeViewPresenter

        Private _employees As IList(Of Employee)

        Private _divisions As IList(Of Division)

        Private _view As EmployeeTreeView

        Private _organizationId As Integer

        Private ReadOnly _divisionRepository As DivisionRepository

        Private ReadOnly _employeeRepository As EmployeeRepository

        Public Sub New(view As EmployeeTreeView)
            _view = view
            _organizationId = view.OrganizationID

            If MainServiceProvider IsNot Nothing Then

                _divisionRepository = MainServiceProvider.GetRequiredService(Of DivisionRepository)

                _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
            End If

        End Sub

        Public Async Function Load(organizationId As Integer) As Task
            _organizationId = organizationId
            Await Reload()
        End Function

        Private Async Function Reload() As Task
            _divisions = Await LoadDivisions()
            _employees = Await LoadEmployees()

            Dim initialEmployees = _employees.Where(Function(e) e.IsActive).ToList()

            _view.ShowEmployees(_divisions, initialEmployees)
        End Function

        Private Async Function LoadDivisions() As Task(Of IList(Of Division))

            Return (Await _divisionRepository.
                GetAllAsync(_organizationId)).
                OrderBy(Function(d) d.Name).
                ToList()
        End Function

        Private Async Function LoadEmployees() As Task(Of IList(Of Employee))

            Return (Await _employeeRepository.
                GetAllWithDivisionAndPositionAsync(_organizationId)).
                OrderBy(Function(e) e.LastName).
                ThenBy(Function(e) e.FirstName).
                ToList()

        End Function

        Public Async Function FilterEmployees(needle As String, isActiveOnly As Boolean) As Task
            Dim match =
                Function(employee As Employee) As Boolean
                    needle = needle.ToLower
                    Dim contains = employee.FullNameWithMiddleInitialLastNameFirst.ToLower().Contains(needle)
                    Dim reverseName = ($"{employee.LastName} {employee.FirstName}").ToLower()
                    Dim containsReverseName = reverseName.Contains(needle)
                    Dim hasThisKindOfEmployeeNo = employee.EmployeeNo IsNot Nothing AndAlso employee.EmployeeNo.Contains(needle)

                    Return contains Or containsReverseName Or hasThisKindOfEmployeeNo
                End Function

            Dim filterActive =
                Function(employee As Employee)
                    If isActiveOnly Then
                        Return True
                    Else
                        Return employee.IsActive
                    End If
                End Function

            Dim employees = Await Task.Run(
                Function()
                    Return _employees.Where(match).Where(filterActive).ToList()
                End Function)

            _view.ShowEmployees(_divisions, employees)
        End Function

        Public Function GetActiveEmployees() As IList(Of Employee)
            Dim list = New List(Of Employee)
            For Each node As TreeNode In _view.AccuPayEmployeeTreeView.Nodes
                TraverseNodes(node, list)
            Next
            Return list
        End Function

        Public Sub TraverseNodes(node As TreeNode, list As IList(Of Employee))
            Dim isEmployee = TypeOf node.Tag Is Employee
            Dim isSatisfy = isEmployee And node.Checked

            If isSatisfy Then
                EmployeeListRemover(DirectCast(node.Tag, Employee), list)

                list.Add(DirectCast(node.Tag, Employee))
            Else
                If isEmployee Then
                    EmployeeListRemover(DirectCast(node.Tag, Employee), list)
                End If
            End If

            If node.GetNodeCount(False) >= 1 Then
                For Each child As TreeNode In node.Nodes
                    TraverseNodes(child, list)
                Next
            End If
        End Sub

        Private Sub EmployeeListRemover(employee As Employee, list As IList(Of Employee))
            Dim isExists = list.Any(Function(e) Nullable.Equals(e.RowID, employee.RowID))
            If isExists Then
                list.Remove(employee)
            End If
        End Sub

    End Class

End Class
