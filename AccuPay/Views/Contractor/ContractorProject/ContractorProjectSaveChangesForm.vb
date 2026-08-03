Option Strict On
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports SergeUtils

Public Class ContractorProjectSaveChangesForm
    Private ReadOnly _userId As Integer
    Private ReadOnly _contractorProject As ContractorProject
    Private ReadOnly _origincontractorProject As ContractorProject

    Public Sub New(userId As Integer, contractorProject As ContractorProject)
        _userId = userId
        _contractorProject = contractorProject

        _origincontractorProject = contractorProject.CloneFrom(userId, contractorProject)

        InitializeComponent()

    End Sub

    Private Async Sub ContractorProjectSaveChangesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gridEmployees.AutoGenerateColumns = False

        ClearDataBindings(Panel1)

        InitDataBindings()

        If String.IsNullOrEmpty(_contractorProject.ContractorName) Then
            Dim contractorDataService = GetRequiredService(Of IContractorDataService)()
            Dim contractor = (Await contractorDataService.GetAllAsync()).FirstOrDefault(Function(c) Integer.Equals(c.RowID, _contractorProject.ContractorId))
            txContractorName.Text = contractor.Name

        Else
            txContractorName.Text = _contractorProject.ContractorName

        End If

        AddHandler dtpEnd.CheckedChanged, AddressOf dtpEnd_CheckedChanged

        InitSelectAllColumnHeader()

        InitButtonsClearSearchBox()

    End Sub

    Private Sub InitSelectAllColumnHeader()
        Dim cbHeaderIsVerified = New DatagridViewCheckBoxHeaderCell("[All]")
        Column1.HeaderCell = cbHeaderIsVerified
        RemoveHandler cbHeaderIsVerified.OnCheckBoxClicked, AddressOf cbHeaderIsSelectAll_CheckBoxClicked
        AddHandler cbHeaderIsVerified.OnCheckBoxClicked, AddressOf cbHeaderIsSelectAll_CheckBoxClicked
    End Sub

    Private Sub cbHeaderIsSelectAll_CheckBoxClicked(state As Boolean)
        Dim data = gridEmployees.Rows.OfType(Of DataGridViewRow).
            ToList()

        data.ForEach(Sub(t)
                         t.Cells(Column1.Name).Value = state
                     End Sub)

        gridEmployees.EndEdit()
        gridEmployees.Refresh()

    End Sub

    Private Sub InitDataBindings()
        Dim isUntouchable = False
        Dim updateMode = If(isUntouchable, DataSourceUpdateMode.Never, DataSourceUpdateMode.OnPropertyChanged)
        Const PropertyName As String = "Text"

        Dim nameBinding = New Binding(PropertyName, _contractorProject, NameOf(_contractorProject.Name), False, updateMode)
        TextBox1.DataBindings.Add(nameBinding)
        AddHandler nameBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)
                If Not e.DesiredType Is GetType(String) Then Return

                Dim value = CStr(e.Value)

                btnSave.Enabled = Not _origincontractorProject.Name = value AndAlso Not String.IsNullOrEmpty(value)

            End Sub

        Dim descriptionBinding = New Binding(PropertyName, _contractorProject, NameOf(_contractorProject.Description), False, updateMode)
        txDescription.DataBindings.Add(descriptionBinding)
        AddHandler descriptionBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)
                If Not e.DesiredType Is GetType(String) Then Return

                Dim value = CStr(e.Value)

                btnSave.Enabled = Not _origincontractorProject.Description = value

            End Sub

        Dim nullDateValue = New Date?() 'DBNull.Value

        Dim beginDateBinding = New Binding("Value",
            _contractorProject,
            NameOf(_contractorProject.BeginDate),
            formattingEnabled:=True,
            dataSourceUpdateMode:=updateMode,
            nullValue:=nullDateValue)
        dtpBegin.DataBindings.Add(beginDateBinding)
        AddHandler beginDateBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)

                Dim value = CType(e.Value, Date?)

                btnSave.Enabled = Not Date.Equals(_origincontractorProject.BeginDate, value)

            End Sub

        Dim endDateBinding = New Binding("Value",
            _contractorProject,
            NameOf(_contractorProject.EndDate),
            formattingEnabled:=True,
            dataSourceUpdateMode:=updateMode,
            nullValue:=nullDateValue)
        dtpEnd.DataBindings.Add(endDateBinding)
        AddHandler endDateBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)

                Dim value = CType(If(dtpEnd.Checked, e.Value, endDateBinding.NullValue), Date?)

                btnSave.Enabled = Not Date.Equals(_origincontractorProject.EndDate, If(value Is Nothing, New Date?(), value))

                dtpBegin.Focus()

            End Sub

        AddHandler endDateBinding.Format,
            Sub(sender As Object, e As ConvertEventArgs)
                Dim dtp = CType(CType(sender, Binding).Control, NullableDateTimePicker)

                dtp.Checked = Not (Date.Equals(e.Value, New Date?()) OrElse e.Value Is Nothing OrElse IsDBNull(e.Value))

            End Sub

    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        btnSave.Enabled = False

        Await FunctionUtils.TryCatchFunctionAsync("Contractor Project Save Changes",
            Async Function()
                Dim contractorProjectDataService = GetRequiredService(Of IContractorProjectDataService)()

                If _contractorProject.IsNewEntity Then Await contractorProjectDataService.SaveAsync(_contractorProject, _userId)

                If Not _contractorProject.IsNewEntity Then Await contractorProjectDataService.SaveManyAsync(currentlyLoggedInUserId:=_userId, updated:=New List(Of ContractorProject) From {_contractorProject})


                DialogResult = DialogResult.OK

                btnSave.Enabled = True

            End Function)

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

    End Sub

    Private Sub dtpEnd_CheckedChanged(sender As Object, e As EventArgs)
        Dim dp = DirectCast(sender, NullableDateTimePicker)

        dp.FocusToNext()

    End Sub

    Private Async Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        Select Case TabControl1.SelectedIndex
            Case TabPage1.TabIndex

            Case TabPage2.TabIndex
                Await LoadEmployeesAsync()

            Case Else

        End Select

    End Sub

    Private Async Function LoadEmployeesAsync() As Task
        Dim employees = Await GetEmployeesAsync()

        gridEmployees.DataSource = employees.
            Select(Function(t)
                       Dim projectEmployee = _contractorProject.FindEmployeeById(t.RowID.Value)

                       Return New ProjectEmployeeModel(t, isExists:=If(projectEmployee Is Nothing, False, projectEmployee.RowID.HasValue))
                   End Function).
            OrderBy(Function(t) t.CustomDisplayText).
            ToList()

    End Function

    Private Async Function GetEmployeesAsync() As Task(Of List(Of Employee))
        Dim employeeDataService = GetRequiredService(Of IEmployeeDataService)()
        Return (Await employeeDataService.GetAllActiveEmployeesWithOrganizationAsync()).ToList()
    End Function

    Private Sub InitButtonsClearSearchBox()
        Dim size = Function(tb As TextBox) As Size
                       Return New Size(width:=tb.ClientSize.Height, height:=tb.ClientSize.Height)
                   End Function

        Dim point = Function(tb As TextBox, btn As Button) As Point
                        Return New Point(x:=tb.ClientSize.Width - (btn.Size.Width - 1), y:=0)
                    End Function

        Dim _font = New Font(Font.FontFamily, 7.5!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))

        With btnClearSearch1
            .Size = size(TextBoxSearch1)
            .Location = point(TextBoxSearch1, btnClearSearch1)
            .Font = _font
            .Cursor = Cursors.Default
        End With

        TextBoxSearch1.Controls.Clear()
        TextBoxSearch1.Controls.Add(btnClearSearch1)

    End Sub

    Private Sub TextBoxSearch1_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch1.TextChanged

    End Sub

    Private Sub TextBoxSearch1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxSearch1.KeyDown
        If Not e.KeyCode = Keys.Enter Then Return

    End Sub

    Private Sub TextBoxSearch1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxSearch1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True

        End If

    End Sub

End Class
