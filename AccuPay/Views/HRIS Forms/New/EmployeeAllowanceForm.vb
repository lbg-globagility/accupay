Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class EmployeeAllowanceForm

    Private Const FormEntityName As String = "Allowance"

    Private _currentAllowance As Allowance

    Private _employees As List(Of Employee)

    Private _allEmployees As List(Of Employee)

    Private _allowanceTypeList As List(Of Product)

    Private _currentAllowances As List(Of Allowance)

    Private _changedAllowances As List(Of Allowance)

    Private _employeeRepository As EmployeeRepository

    Private _productRepository As ProductRepository

    Private _userActivityRepository As UserActivityRepository

    Private _textBoxDelayedAction As DelayedAction(Of Boolean)

    Sub New()

        InitializeComponent()

        _employees = New List(Of Employee)

        _allEmployees = New List(Of Employee)

        _currentAllowances = New List(Of Allowance)

        _changedAllowances = New List(Of Allowance)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

        _textBoxDelayedAction = New DelayedAction(Of Boolean)

    End Sub

    Private Async Sub EmployeeAllowancesForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        InitializeComponentSettings()

        LoadFrequencyList()
        Await LoadAllowanceTypes()

        Await LoadEmployees()
        Await ShowEmployeeList()

        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs)

        _textBoxDelayedAction.ProcessAsync(Async Function()
                                               Await FilterEmployees(SearchTextBox.Text.ToLower())

                                               Return True
                                           End Function)

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Async Sub EmployeesDataGridView_SelectionChanged(sender As Object, e As EventArgs)

        Await ShowEmployeeAllowances()

    End Sub

    Private Async Function ShowEmployeeAllowances() As Task

        ResetForm()

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        EmployeeNameTextBox.Text = currentEmployee.FullNameWithMiddleInitial
        EmployeeNumberTextBox.Text = currentEmployee.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(currentEmployee.Image)

        Await LoadAllowances(currentEmployee)

    End Function

    Private Sub AllowanceGridView_SelectionChanged(sender As Object, e As EventArgs)

        ShowAllowanceDetails()

    End Sub

    Private Sub ShowAllowanceDetails()

        If AllowanceGridView.CurrentRow Is Nothing Then Return

        Dim currentAllowance As Allowance = GetSelectedAllowance()

        Dim currentEmployee = GetSelectedEmployee()
        If currentAllowance IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentAllowance.EmployeeID, currentEmployee.RowID) Then

            PopulateAllowanceForm(currentAllowance)
        End If
    End Sub

    Private Async Sub lnlAddAllowanceType_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklbaddallowtype.LinkClicked

        Dim n_ProductControlForm As New ProductControlForm

        With n_ProductControlForm

            .Status.HeaderText = "Taxable Flag"

            .PartNo.HeaderText = "Allowance name"

            .NameOfCategory = ProductConstant.ALLOWANCE_TYPE_CATEGORY

            If n_ProductControlForm.ShowDialog = Windows.Forms.DialogResult.OK Then

                If .IsSaved Then

                    Dim oldSelectedAllowanceId = Me._currentAllowance.ProductID

                    Await LoadAllowanceTypes()

                    Dim oldSelectedAllowance = Me._allowanceTypeList.FirstOrDefault(Function(a) Nullable.Equals(a.RowID, oldSelectedAllowanceId))

                    If oldSelectedAllowance Is Nothing Then Return

                    Dim orderedAllowanceTypeList = Me._allowanceTypeList.OrderBy(Function(p) p.PartNo).ToList

                    cboallowtype.SelectedIndex = orderedAllowanceTypeList.IndexOf(oldSelectedAllowance)

                End If

            End If

        End With

    End Sub

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        ForceAllowanceGridViewCommit()

        Dim changedAllowances As New List(Of Allowance)

        Dim messageTitle = "Update Allowances"

        For Each allowance In Me._currentAllowances
            If CheckIfAllowanceIsChanged(allowance) Then
                allowance.LastUpdBy = z_User
                changedAllowances.Add(allowance)
            End If
        Next

        If changedAllowances.Count < 1 Then

            MessageBoxHelper.Warning("No changed allowances!", messageTitle)
            Return

        ElseIf changedAllowances.Count > 1 AndAlso MessageBoxHelper.Confirm(Of Boolean) _
            ($"You are about to update multiple allowances. Do you want to proceed?", "Confirm Multiple Updates") = False Then

            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                                        Async Function()

                                            Dim service = MainServiceProvider.GetRequiredService(Of AllowanceDataService)
                                            Await service.SaveManyAsync(changedAllowances)

                                            For Each item In changedAllowances
                                                RecordUpdate(item)
                                            Next

                                            ShowBalloonInfo($"{changedAllowances.Count} Allowance(s) Successfully Updated.", messageTitle)

                                            Dim currentEmployee = GetSelectedEmployee()

                                            If currentEmployee IsNot Nothing Then

                                                Await LoadAllowances(currentEmployee)

                                            End If

                                        End Function)
    End Sub

    Private Sub cboallowtype_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboallowtype.SelectedValueChanged
        If Me._currentAllowance IsNot Nothing Then
            Dim selectedAllowanceType = Me._allowanceTypeList.FirstOrDefault(Function(l) (l.PartNo = cboallowtype.Text))

            If selectedAllowanceType Is Nothing Then
                Me._currentAllowance.ProductID = Nothing
            Else
                Me._currentAllowance.ProductID = selectedAllowanceType.RowID
                Me._currentAllowance.Product = selectedAllowanceType.CloneJson()

                'force commit to gridview
                'ForceAllowanceGridViewCommit()
                'cboallowtype.Focus()
            End If
        End If

    End Sub

    Private Async Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click

        Dim employee As Employee = GetSelectedEmployee()

        If employee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")

            Return
        End If

        Dim form As New AddAllowanceForm(employee)
        form.ShowDialog()

        If form.IsSaved Then

            If form.NewAllowanceTypes.Count > 0 Then

                For Each allowanceType In form.NewAllowanceTypes
                    Me._allowanceTypeList.Add(allowanceType)
                Next

                PopulateAllowanceTypeCombobox()
            End If

            Await LoadAllowances(employee)

            If form.ShowBalloonSuccess Then
                ShowBalloonInfo("Allowance Successfully Added", "Saved")
            End If
        End If

    End Sub

    Private Sub AllowanceBindingSource_CurrentItemChanged(sender As Object, e As EventArgs) Handles AllowancesBindingSource.CurrentItemChanged

        Dim currentRow = AllowanceGridView.CurrentRow

        If currentRow Is Nothing Then Return

        If Me._currentAllowance Is Nothing Then Return

        Dim hasChanged = CheckIfAllowanceIsChanged(Me._currentAllowance)

        If hasChanged Then
            currentRow.DefaultCellStyle.BackColor = Color.Yellow
            currentRow.DefaultCellStyle.SelectionForeColor = Color.Red
        Else
            currentRow.DefaultCellStyle.BackColor = Color.White
            currentRow.DefaultCellStyle.SelectionForeColor = Color.Black

        End If
    End Sub

    Private Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        If Me._currentAllowances Is Nothing Then
            MessageBoxHelper.Warning("No changed allowances!")
            Return
        End If

        Me._currentAllowances = Me._changedAllowances.CloneListJson()

        PopulateAllowanceGridView()
    End Sub

    Private Sub PopulateAllowanceGridView()

        RemoveHandler AllowanceGridView.SelectionChanged, AddressOf AllowanceGridView_SelectionChanged

        AllowancesBindingSource.DataSource = Me._currentAllowances

        AllowanceGridView.DataSource = AllowancesBindingSource

        ShowAllowanceDetails()

        AddHandler AllowanceGridView.SelectionChanged, AddressOf AllowanceGridView_SelectionChanged

    End Sub

    Private Async Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        Const messageTitle As String = "Delete Allowance"

        If Me._currentAllowance?.RowID Is Nothing Then
            MessageBoxHelper.Warning("No allowance selected!")

            Return
        End If

        Dim service = MainServiceProvider.GetRequiredService(Of AllowanceDataService)

        Dim currentAllowance = Await service.GetByIdAsync(Me._currentAllowance.RowID.Value)

        If currentAllowance Is Nothing Then

            MessageBoxHelper.Warning("Allowance not found in database! Please close this form then open again.")

            Return
        End If

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete this?", "Confirm Deletion") = False Then

            Return
        End If

        Dim allowanceIsAlreadyUsed = Await service.CheckIfAlreadyUsedAsync(Me._currentAllowance.RowID.Value)

        If allowanceIsAlreadyUsed Then

            If MessageBoxHelper.Confirm(Of Boolean) _
        ("This allowance has already been used. Deleting this might affect previous cutoffs. We suggest changing the End Date instead. Do you want to proceed deletion?", "Confirm Deletion",
            messageBoxIcon:=MessageBoxIcon.Warning) = False Then

                Return
            End If

        End If

        Await DeleteAllowance(currentEmployee, messageTitle)

    End Sub

    Private Async Function DeleteAllowance(currentEmployee As Employee, messageTitle As String) As Task

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                                            Async Function()
                                                Dim service = MainServiceProvider.GetRequiredService(Of AllowanceDataService)
                                                Await service.DeleteAsync(Me._currentAllowance.RowID.Value)

                                                _userActivityRepository.RecordDelete(z_User,
                                                                                     FormEntityName,
                                                                                     CInt(Me._currentAllowance.RowID),
                                                                                     z_OrganizationID)

                                                Await LoadAllowances(currentEmployee)

                                                ShowBalloonInfo("Successfully Deleted.", messageTitle)

                                            End Function)
    End Function

    Private Async Sub ImportToolStripButton_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click

        Using form = New ImportAllowanceForm()
            form.ShowDialog()

            If form.IsSaved Then

                Await LoadAllowanceTypes()

                Dim currentEmployee = GetSelectedEmployee()

                If currentEmployee IsNot Nothing Then
                    Await LoadAllowances(currentEmployee)
                End If

                ShowBalloonInfo("Allowances Successfully Imported", "Import Allowances")

            End If

        End Using
    End Sub

    Private Sub EmployeeAllowancesForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        PayrollForm.listPayrollForm.Remove(Me.Name)
        myBalloon(, , lblFormTitle, , , 1)

    End Sub

    Private Async Sub ShowAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowAllCheckBox.CheckedChanged

        RemoveHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

        SearchTextBox.Clear()
        Await ShowEmployeeList()

        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, lblFormTitle, 400)
    End Sub

    Private Sub InitializeComponentSettings()
        AllowanceGridView.AutoGenerateColumns = False
        employeesDataGridView.AutoGenerateColumns = False
    End Sub

    Private Sub LoadFrequencyList()

        Dim service = MainServiceProvider.GetRequiredService(Of AllowanceDataService)
        cboallowfreq.DataSource = service.GetFrequencyList()

    End Sub

    Private Async Function FilterEmployees(Optional searchValue As String = "") As Task
        Dim filteredEmployees As New List(Of Employee)

        RemoveHandler employeesDataGridView.SelectionChanged, AddressOf EmployeesDataGridView_SelectionChanged

        If String.IsNullOrEmpty(searchValue) Then
            employeesDataGridView.DataSource = Me._employees
        Else
            employeesDataGridView.DataSource =
                Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        End If

        Await ShowEmployeeAllowances()

        AddHandler employeesDataGridView.SelectionChanged, AddressOf EmployeesDataGridView_SelectionChanged
    End Function

    Private Async Function LoadAllowances(currentEmployee As Employee) As Task
        If currentEmployee?.RowID Is Nothing Then Return

        Dim service = MainServiceProvider.GetRequiredService(Of AllowanceDataService)
        Dim allowances = (Await service.GetByEmployeeWithProductAsync(currentEmployee.RowID.Value)).
                                OrderByDescending(Function(a) a.EffectiveStartDate).
                                ThenBy(Function(a) a.Type).
                                ToList

        'only get the items with effective end dates first
        Me._currentAllowances = allowances.Where(Function(a) a.EffectiveEndDate IsNot Nothing).ToList

        'then add the items with no effective end dates at the beginning
        Me._currentAllowances.InsertRange(0, allowances.
                                Where(Function(a) a.EffectiveEndDate Is Nothing).
                                OrderByDescending(Function(a) a.EffectiveStartDate).
                                ToList)

        Me._changedAllowances = Me._currentAllowances.CloneListJson()

        PopulateAllowanceGridView()

    End Function

    Private Async Function LoadAllowanceTypes() As Task

        Dim allowanceList = New List(Of Product)(Await _productRepository.GetAllowanceTypesAsync(z_OrganizationID))

        ' TODO: decide on either deleting the allowance type on delete on ProductControlForm,
        'or if we stil use ActiveData to soft delete the allowance, what should happen on the form
        'if the selected allowance has a soft deleted allowance type.

        Me._allowanceTypeList = allowanceList.Where(Function(a) a.PartNo IsNot Nothing).
                                                Where(Function(a) a.PartNo.Trim <> String.Empty).
                                                ToList

        PopulateAllowanceTypeCombobox()

    End Function

    Private Sub PopulateAllowanceTypeCombobox()
        Dim allowanceTypes = _productRepository.ConvertToStringList(Me._allowanceTypeList)

        cboallowtype.DataSource = allowanceTypes
    End Sub

    Private Function GetSelectedEmployee() As Employee
        If employeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(employeesDataGridView.CurrentRow.DataBoundItem, Employee)
    End Function

    Private Sub PopulateAllowanceForm(allowance As Allowance)
        Me._currentAllowance = allowance

        txtallowamt.DataBindings.Clear()
        txtallowamt.DataBindings.Add("Text", Me._currentAllowance, "Amount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        dtpallowstartdate.DataBindings.Clear()
        dtpallowstartdate.DataBindings.Add("Value", Me._currentAllowance, "EffectiveStartDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        dtpallowenddate.DataBindings.Clear()
        dtpallowenddate.DataBindings.Add("Value", Me._currentAllowance, "EffectiveEndDate", True, DataSourceUpdateMode.OnPropertyChanged, Nothing)

        cboallowtype.DataBindings.Clear()
        cboallowtype.DataBindings.Add("Text", Me._currentAllowance, "Type")

        cboallowfreq.DataBindings.Clear()
        cboallowfreq.DataBindings.Add("Text", Me._currentAllowance, "AllowanceFrequency")

        DetailsTabLayout.Enabled = True

    End Sub

    Private Sub ResetForm()

        'employee details

        EmployeeNameTextBox.Text = String.Empty
        EmployeeNumberTextBox.Text = String.Empty

        EmployeePictureBox.Image = Nothing

        'allowance grid view
        RemoveHandler AllowanceGridView.SelectionChanged, AddressOf AllowanceGridView_SelectionChanged

        AllowanceGridView.DataSource = Nothing

        AddHandler AllowanceGridView.SelectionChanged, AddressOf AllowanceGridView_SelectionChanged

        'allowance details
        Me._currentAllowance = Nothing

        DetailsTabLayout.Enabled = False

        cboallowtype.SelectedIndex = -1
        cboallowtype.DataBindings.Clear()

        cboallowfreq.SelectedIndex = -1
        cboallowfreq.DataBindings.Clear()

        dtpallowstartdate.ResetText()
        dtpallowstartdate.DataBindings.Clear()

        dtpallowenddate.ResetText()
        dtpallowenddate.DataBindings.Clear()

        txtallowamt.Clear()
        txtallowamt.DataBindings.Clear()

    End Sub

    Private Function GetSelectedAllowance() As Allowance
        Return CType(AllowanceGridView.CurrentRow.DataBoundItem, Allowance)
    End Function

    Private Sub ForceAllowanceGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        EmployeeInfoTabLayout.Focus()
    End Sub

    Private Function CheckIfAllowanceIsChanged(newAllowance As Allowance) As Boolean
        If _currentAllowance Is Nothing Then Return False

        Dim oldAllowance =
            Me._changedAllowances.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newAllowance.RowID))

        If oldAllowance Is Nothing Then Return False

        Dim hasChanged = False

        If _
            Nullable.Equals(newAllowance.ProductID, oldAllowance.ProductID) = False OrElse
            Nullable.Equals(newAllowance.OrganizationID, oldAllowance.OrganizationID) = False OrElse
            newAllowance.AllowanceFrequency <> oldAllowance.AllowanceFrequency OrElse
            newAllowance.Product?.PartNo <> oldAllowance.Product?.PartNo OrElse
            newAllowance.Type <> oldAllowance.Type OrElse
            newAllowance.Amount <> oldAllowance.Amount OrElse
            newAllowance.EffectiveStartDate <> oldAllowance.EffectiveStartDate OrElse
            Nullable.Equals(newAllowance.EffectiveEndDate, oldAllowance.EffectiveEndDate) = False Then

            hasChanged = True
        End If

        Return hasChanged

    End Function

    Private Function RecordUpdate(newAllowance As Allowance) As Boolean

        Dim oldAllowance =
            Me._changedAllowances.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newAllowance.RowID))

        If oldAllowance Is Nothing Then Return False

        Dim changes = New List(Of UserActivityItem)

        Dim entityName = FormEntityName.ToLower()

        If newAllowance.Type <> oldAllowance.Type Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldAllowance.RowID),
                        .Description = $"Updated {entityName} type from '{oldAllowance.Type}' to '{newAllowance.Type}'."
                        })
        End If
        If newAllowance.AllowanceFrequency <> oldAllowance.AllowanceFrequency Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldAllowance.RowID),
                        .Description = $"Updated {entityName} frequency from '{oldAllowance.AllowanceFrequency}' to '{newAllowance.AllowanceFrequency}'."
                        })
        End If
        If newAllowance.EffectiveStartDate <> oldAllowance.EffectiveStartDate Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldAllowance.RowID),
                        .Description = $"Updated {entityName} start date from '{oldAllowance.EffectiveStartDate.ToShortDateString}' to '{newAllowance.EffectiveStartDate.ToShortDateString}'."
                        })
        End If
        If newAllowance.EffectiveEndDate.ToString <> oldAllowance.EffectiveEndDate.ToString Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldAllowance.RowID),
                        .Description = $"Updated {entityName} end date from '{oldAllowance.EffectiveEndDate?.ToShortDateString}' to '{newAllowance.EffectiveEndDate?.ToShortDateString}'."
                        })
        End If
        If newAllowance.Amount <> oldAllowance.Amount Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldAllowance.RowID),
                        .Description = $"Updated {entityName} amount from '{oldAllowance.Amount.ToString}' to '{newAllowance.Amount.ToString}'."
                        })
        End If

        _userActivityRepository.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)

        Return True
    End Function

    Private Async Function LoadEmployees() As Task

        Me._allEmployees = (Await _employeeRepository.GetAllWithPositionAsync(z_OrganizationID)).
                            OrderBy(Function(e) e.LastName).
                            ToList

    End Function

    Private Async Function ShowEmployeeList() As Task

        If ShowAllCheckBox.Checked Then

            Me._employees = Me._allEmployees
        Else

            Me._employees = Me._allEmployees.Where(Function(e) e.IsActive).ToList

        End If

        Await FilterEmployees()
    End Function

    Private Sub UserActivityToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityToolStripButton.Click
        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Sub Cboallowfreq_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboallowfreq.SelectedValueChanged
        If _currentAllowance Is Nothing Then Return

        Dim showEndDate = Not cboallowfreq.Text = Allowance.FREQUENCY_ONE_TIME

        lblEndDate.Visible = showEndDate
        dtpallowenddate.Visible = showEndDate

    End Sub

End Class