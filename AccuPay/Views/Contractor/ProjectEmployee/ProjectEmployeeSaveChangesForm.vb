Option Strict On
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports Microsoft.EntityFrameworkCore.Internal

Public Class ProjectEmployeeSaveChangesForm
    Private ReadOnly _userId As Integer
    Private ReadOnly _projectEmployee As ProjectEmployee
    Private ReadOnly _originprojectEmployee As ProjectEmployee

    Public Sub New(userId As Integer, projectEmployee As ProjectEmployee)
        _userId = userId
        _projectEmployee = projectEmployee

        _originprojectEmployee = ProjectEmployee.CloneFrom(userId, projectEmployee)

        InitializeComponent()

    End Sub

    Private Async Sub ProjectEmployeeSaveChangesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClearDataBindings(Panel1)

        Await LoadEmployeesAsync()
        cbxEmployee_DropDown(cbxEmployee, New EventArgs())

        InitDataBindings()

        If String.IsNullOrEmpty(_projectEmployee.ProjectName) Then
            Dim contractorProjectDataService = GetRequiredService(Of IContractorProjectDataService)()
            Dim contractorProject = (Await contractorProjectDataService.GetAllAsync()).FirstOrDefault(Function(c) Integer.Equals(c.RowID, _projectEmployee.ProjectId))
            txProjectName.Text = contractorProject.Name

        Else
            txProjectName.Text = _projectEmployee.ProjectName
            dtpBegin.MinDate = _projectEmployee.Project.BeginDate.GetValueOrDefault(DateTime.MinValue)

        End If

        AddHandler dtpEnd.CheckedChanged, AddressOf dtpEnd_CheckedChanged

    End Sub

    Private Sub InitDataBindings()
        Dim isUntouchable = False
        Dim updateMode = If(isUntouchable, DataSourceUpdateMode.Never, DataSourceUpdateMode.OnPropertyChanged)
        Const PropertyName As String = "Text"

        Dim employeeBinding = New Binding("SelectedValue", _projectEmployee, NameOf(_projectEmployee.EmployeeId), True, updateMode)
        cbxEmployee.DataBindings.Add(employeeBinding)
        AddHandler employeeBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)

                Dim value = CInt(e.Value)

                btnSave.Enabled = Not If(_originprojectEmployee.EmployeeId, 0) = value

            End Sub

        Dim nullDateValue = New Date?() 'DBNull.Value

        Dim beginDateBinding = New Binding("Value",
            _projectEmployee,
            NameOf(_projectEmployee.BeginDate),
            formattingEnabled:=True,
            dataSourceUpdateMode:=updateMode,
            nullValue:=nullDateValue)
        dtpBegin.DataBindings.Add(beginDateBinding)
        AddHandler beginDateBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)

                Dim value = CType(e.Value, Date?)

                btnSave.Enabled = Not Date.Equals(_originprojectEmployee.BeginDate, value)

            End Sub

        Dim endDateBinding = New Binding("Value",
            _projectEmployee,
            NameOf(_projectEmployee.EndDate),
            formattingEnabled:=True,
            dataSourceUpdateMode:=updateMode,
            nullValue:=nullDateValue)
        dtpEnd.DataBindings.Add(endDateBinding)
        AddHandler endDateBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)

                Dim value = CType(If(dtpEnd.Checked, e.Value, endDateBinding.NullValue), Date?)

                btnSave.Enabled = Not Date.Equals(_originprojectEmployee.EndDate, If(value Is Nothing, New Date?(), value))

                dtpBegin.Focus()

            End Sub

        AddHandler endDateBinding.Format,
            Sub(sender As Object, e As ConvertEventArgs)
                Dim dtp = CType(CType(sender, Binding).Control, NullableDatePicker)

                dtp.Checked = Not (Date.Equals(e.Value, New Date?()) OrElse e.Value Is Nothing OrElse IsDBNull(e.Value))

            End Sub

    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        btnSave.Enabled = False

        Await FunctionUtils.TryCatchFunctionAsync("Project Employee Save Changes",
            Async Function()
                Dim projectEmployeeDataService = GetRequiredService(Of IProjectEmployeeDataService)()

                If _projectEmployee.IsNewEntity Then Await projectEmployeeDataService.SaveAsync(_projectEmployee, _userId)

                If Not _projectEmployee.IsNewEntity Then Await projectEmployeeDataService.SaveManyAsync(currentlyLoggedInUserId:=_userId, updated:=New List(Of ProjectEmployee) From {_projectEmployee})


                DialogResult = DialogResult.OK

                btnSave.Enabled = True

            End Function)

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

    End Sub

    Private Sub cbxEmployee_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles cbxEmployee.SelectedIndexChanged

    End Sub

    Private Sub cbxEmployee_SelectedIndexChanged0(sender As Object, e As EventArgs)

    End Sub

    Private Sub cbxEmployee_DropDown(sender As Object, e As EventArgs)

        Dim employeeList = CType(cbxEmployee.DataSource, List(Of Employee))

        If Not employeeList.Any() Then Return

        Static font As Font = cbxEmployee.Font
        Dim grp As Graphics = cbxEmployee.CreateGraphics()

        Dim vertScrollBarWidth As Integer = If(cbxEmployee.Items.Count > cbxEmployee.MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0)

        Dim longestWord = employeeList.
            OrderByDescending(Function(emp) emp.CustomDisplayText.Length).
            Select(Function(emp) emp.CustomDisplayText).
            FirstOrDefault()

        Dim width = CInt(grp.MeasureString(longestWord, font).Width) + vertScrollBarWidth

        cbxEmployee.DropDownWidth = width + 8

    End Sub

    Private Async Function LoadEmployeesAsync() As Task
        Dim employeeDataService = GetRequiredService(Of IEmployeeDataService)()
        Dim employees = Await employeeDataService.GetAllActiveEmployeesWithOrganizationAsync()

        With cbxEmployee
            Dim emp = employees.FirstOrDefault()
            .ValueMember = NameOf(emp.RowID)
            .DisplayMember = NameOf(emp.CustomDisplayText)
            .BindingContext = New BindingContext()
            .DataSource = employees.
                OrderBy(Function(t) t.CustomDisplayText).
                ToList()

        End With

    End Function

    Private Sub dtpEnd_CheckedChanged(sender As Object, e As EventArgs)
        Dim dp = DirectCast(sender, NullableDatePicker)

        dp.FocusToNext()

    End Sub

End Class
