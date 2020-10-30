Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports Microsoft.Extensions.DependencyInjection

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

    Private Async Sub MassOvertimeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await _presenter.Load()
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

    Private Async Sub EmployeeSearchTextBox_TextChanged(sender As Object, e As EventArgs)
        Await _presenter.FilterEmployees(EmployeeSearchTextBox.Text)
    End Sub

End Class

Public Class MassOvertimePresenter

    Private _employees As IList(Of Employee)

    Private _divisions As IList(Of Division)

    Private _view As MassOvertimeForm

    Private _models As List(Of OvertimeModel)

    Private _divisionService As DivisionDataService

    Private _employeeRepository As EmployeeRepository
    Private featureChecker As FeatureListChecker

    Public Sub New(view As MassOvertimeForm)
        _view = view

        _divisionService = MainServiceProvider.GetRequiredService(Of DivisionDataService)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        featureChecker = FeatureListChecker.Instance
    End Sub

    Public Async Function Load() As Task
        _divisions = LoadDivisions()
        _employees = Await LoadEmployees()

        _view.ShowEmployees(_divisions, _employees)
    End Function

    Private Function LoadDivisions() As IList(Of Division)

        Return _divisionService.GetAll(z_OrganizationID).ToList()
    End Function

    Private Async Function LoadEmployees() As Task(Of IList(Of Employee))

        Return (Await _employeeRepository.
            GetAllWithDivisionAndPositionAsync(z_OrganizationID)).
            ToList()
    End Function

    Private Function LoadOvertimes(dateFrom As Date, dateTo As Date, employees As IList(Of Employee)) As IList(Of IGrouping(Of Integer?, Overtime))
        Dim employeeIds = employees.Select(Function(e) e.RowID.Value).ToList()

        Dim repository = MainServiceProvider.GetRequiredService(Of OvertimeRepository)
        Dim overtimes = repository.GetByEmployeeIDsAndDatePeriod(
            z_OrganizationID,
            employeeIds,
            New TimePeriod(dateFrom, dateTo))

        Return overtimes.
            GroupBy(Function(o) o.EmployeeID).
            ToList()
    End Function

    Public Async Function FilterEmployees(needle As String) As Task
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
    End Function

    Public Sub RefreshOvertime()
        Dim dateFrom = _view.DateFrom
        Dim dateTo = _view.DateTo
        Dim employees = _view.GetActiveEmployees()

        Dim overtimesByEmployee = LoadOvertimes(dateFrom.Date, dateTo.Date, employees)

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
        Dim changedOvertimes = _models.
            Where(Function(ot) ot.IsNew Or ot.IsUpdate).
            Select(Function(ot) ConverToOvertime(ot)).
            ToList()

        Await FunctionUtils.TryCatchFunctionAsync("Saving Mass Overtimes",
        Async Function()
            If changedOvertimes.Any Then

                changedOvertimes.ForEach(Function(o) o.LastUpdBy = z_User)
                Dim service = MainServiceProvider.GetRequiredService(Of OvertimeDataService)

                service.CheckMassOvertimeFeature(featureChecker.HasAccess(Feature.MassOvertime))
                Await service.SaveManyAsync(changedOvertimes)

            End If

            Dim deletableOvertimeIDs = _models.
                Where(Function(ot) ot.IsDelete).
                Where(Function(ot) ot.Overtime.RowID IsNot Nothing).
                Select(Function(ot) ot.Overtime.RowID.Value).
                ToList()

            If deletableOvertimeIDs.Any Then

                Dim service = MainServiceProvider.GetRequiredService(Of OvertimeDataService)
                Await service.DeleteManyAsync(deletableOvertimeIDs, z_OrganizationID, z_User)

            End If
        End Function)
    End Function

    Private Shared Function ConverToOvertime(model As OvertimeModel) As Overtime
        Dim convertedOvertime = New Overtime() With {
            .EmployeeID = model.EmployeeID,
            .OrganizationID = z_OrganizationID,
            .LastUpd = Date.Now,
            .LastUpdBy = z_User,
            .OTStartDate = model.Date,
            .OTEndDate = model.Date,
            .Status = Overtime.StatusApproved,
            .OTStartTime = model.StartTime,
            .OTEndTime = model.EndTime
        }

        If model.IsUpdate Then
            convertedOvertime.RowID = model.Overtime.RowID
            convertedOvertime.Created = model.Overtime.Created
            convertedOvertime.CreatedBy = model.Overtime.CreatedBy
        End If
        If model.IsNew Then
            convertedOvertime.Created = Date.Now
            convertedOvertime.CreatedBy = z_User
        End If

        Return convertedOvertime
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
