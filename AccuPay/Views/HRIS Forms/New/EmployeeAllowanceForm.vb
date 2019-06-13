Option Strict On

Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Repository
Imports AccuPay.SimplifiedEntities
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports Simplified = AccuPay.SimplifiedEntities.GridView

Public Class EmployeeAllowanceForm

    Private _employeeRepository As New EmployeeRepository

    Private _productRepository As New ProductRepository

    Private _allowanceRepository As New AllowanceRepository

    Private _allowanceTypeList As List(Of Product)

    Private _employees As New List(Of Simplified.Employee)
    Private _currentAllowance As Allowance

    Private _currentAllowances As New List(Of Allowance)

    Private _unchangedAllowances As New List(Of Allowance)

    Private threadArrayList As New List(Of Thread)

    Private Async Sub EmployeeAllowancesForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        InitializeComponentSettings()

        LoadFrequencyList()
        Await LoadAllowanceTypes()

        Await ShowEmployeeList()

        ResetAllowanceForm()

    End Sub

    Private Async Sub searchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
        Dim searchValue = searchTextBox.Text.ToLower()

        Await FilterEmployees(searchValue)

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Async Sub employeesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles employeesDataGridView.SelectionChanged

        ResetAllowanceForm()

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        txtEmployeeFirstName.Text = currentEmployee.FullNameWithMiddleNameInitial
        txtEmployeeNumber.Text = currentEmployee.EmployeeNo

        pbEmpPicAllow.Image = ConvByteToImage(currentEmployee.Image)

        Await LoadAllowances(currentEmployee)

    End Sub

    Private Sub dgvempallowance_SelectionChanged(sender As Object, e As EventArgs) Handles dgvempallowance.SelectionChanged
        ResetAllowanceForm()

        If dgvempallowance.CurrentRow Is Nothing Then Return

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

    Private Async Sub tsbtnSaveAllowance_Click(sender As Object, e As EventArgs) Handles tsbtnSaveAllowance.Click
        ForceAllowanceGridViewCommit()

        Dim changedAllowances As New List(Of Allowance)

        Dim messageTitle = "Update Allowances"

        For Each allowance In Me._currentAllowances
            If CheckIfAllowanceIsChanged(allowance) Then
                changedAllowances.Add(allowance)
            End If
        Next

        If changedAllowances.Count < 1 Then

            MessageBoxHelper.Warning("No unchanged allowances!", messageTitle)
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                                        Async Function()
                                            Await _allowanceRepository.SaveManyAsync(changedAllowances)

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
                Me._currentAllowance.Product.PartNo = selectedAllowanceType.PartNo

                'force commit to gridview
                'ForceAllowanceGridViewCommit()
                'cboallowtype.Focus()
            End If
        End If

    End Sub

    Private Async Sub tsbtnNewAllowance_Click(sender As Object, e As EventArgs) Handles tsbtnNewAllowance.Click

        Dim employee As Simplified.Employee = GetSelectedEmployee()

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

        Dim currentRow = dgvempallowance.CurrentRow

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

    Private Sub tsbtnCancelAllowance_Click(sender As Object, e As EventArgs) Handles tsbtnCancelAllowance.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        If Me._currentAllowances Is Nothing Then
            MessageBoxHelper.Warning("No allowances!")
            Return
        End If

        Me._currentAllowances = Me._unchangedAllowances.CloneListJson()

        AllowancesBindingSource.DataSource = Me._currentAllowances

        dgvempallowance.DataSource = AllowancesBindingSource

    End Sub

    Private Async Sub tsbtnDeleteAllowanceButton_Click(sender As Object, e As EventArgs) Handles DeleteAllowanceButton.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        Const messageTitle As String = "Delete Allowance"

        If Me._currentAllowance Is Nothing OrElse
            Me._currentAllowance.RowID Is Nothing Then
            MessageBoxHelper.Warning("No allowance selected!")

            Return
        End If

        Dim currentAllowance = Await _allowanceRepository.GetByIdAsync(Me._currentAllowance.RowID)

        If currentAllowance Is Nothing Then

            MessageBoxHelper.Warning("Allowance not found in database! Please close this form then open again.")

            Return
        End If

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete this?", "Confirm Deletion") = False Then

            Return
        End If

        Dim allowanceIsAlreadyUsed = Await _allowanceRepository.CheckIfAlreadyUsed(Me._currentAllowance.RowID)

        If allowanceIsAlreadyUsed Then

            If MessageBoxHelper.Confirm(Of Boolean) _
        ("This allowance has already been used. Deleting this might affect previous cutoffs. We suggest changing the End Date instead. Do you want to proceed deletion?", "Confirm Deletion",
            messageBoxIcon:=MessageBoxIcon.Warning) = False Then

                Return
            End If

        End If

        Await DeleteAllowance(currentEmployee, messageTitle)

    End Sub

    Private Async Function DeleteAllowance(currentEmployee As Simplified.Employee, messageTitle As String) As Task

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                                            Async Function()
                                                Await _allowanceRepository.DeleteAsync(Me._currentAllowance.RowID)

                                                Await LoadAllowances(currentEmployee)

                                                ShowBalloonInfo("Successfully Deleted.", messageTitle)

                                            End Function)
    End Function

    Private filepath As String

    Private Async Sub tsbtnImportAllowances_Click(sender As Object, e As EventArgs) Handles tsbtnImportAllowance.Click

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

        HRISForm.listHRISForm.Remove(Me.Name)

    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, lblFormTitle, 400)
    End Sub

    Private Sub InitializeComponentSettings()
        dgvempallowance.AutoGenerateColumns = False
        employeesDataGridView.AutoGenerateColumns = False
    End Sub

    Private Sub LoadFrequencyList()

        cboallowfreq.DataSource = _allowanceRepository.GetStatusList()

    End Sub

    Private Async Function FilterEmployees(Optional searchValue As String = "") As Task
        Dim filteredEmployees As New List(Of Simplified.Employee)

        If String.IsNullOrEmpty(searchValue) Then
            employeesDataGridView.DataSource = Me._employees
        Else
            employeesDataGridView.DataSource =
                Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        End If
    End Function

    Private Async Function LoadAllowances(currentEmployee As Simplified.Employee) As Task
        If currentEmployee Is Nothing Then Return

        Me._currentAllowances = (Await _allowanceRepository.GetByEmployeeIncludesProductAsync(currentEmployee.RowID)).ToList

        Me._unchangedAllowances = Me._currentAllowances.CloneListJson()

        AllowancesBindingSource.DataSource = Me._currentAllowances

        dgvempallowance.DataSource = AllowancesBindingSource

    End Function

    Private Async Function LoadAllowanceTypes() As Task

        Dim allowanceList = New List(Of Product)(Await _productRepository.GetAllowanceTypes())

        Me._allowanceTypeList = allowanceList.Where(Function(a) a.PartNo IsNot Nothing).
                                                Where(Function(a) a.PartNo.Trim <> String.Empty).
                                                ToList

        PopulateAllowanceTypeCombobox()

    End Function

    Private Sub PopulateAllowanceTypeCombobox()
        Dim allowanceTypes = _productRepository.ConvertToStringList(Me._allowanceTypeList)

        cboallowtype.DataSource = allowanceTypes
    End Sub

    Private Function GetSelectedEmployee() As Simplified.Employee
        If employeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(employeesDataGridView.CurrentRow.DataBoundItem, Simplified.Employee)
    End Function

    Private Sub PopulateAllowanceForm(allowance As Allowance)
        Me._currentAllowance = allowance

        Dim originalAllowance = Me._unchangedAllowances.
            FirstOrDefault(Function(l) Nullable.Equals(l.RowID, Me._currentAllowance.RowID))

        txtallowamt.DataBindings.Clear()
        txtallowamt.DataBindings.Add("Text", Me._currentAllowance, "Amount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        dtpallowstartdate.DataBindings.Clear()
        dtpallowstartdate.DataBindings.Add("Value", Me._currentAllowance, "EffectiveStartDate") 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        NullableDatePicker1.DataBindings.Clear()
        NullableDatePicker1.DataBindings.Add("Value", Me._currentAllowance, "EffectiveEndDate", True, DataSourceUpdateMode.OnPropertyChanged, Nothing) 'No DataSourceUpdateMode.OnPropertyChanged because it resets to current date

        cboallowtype.DataBindings.Clear()
        cboallowtype.DataBindings.Add("Text", Me._currentAllowance, "Type")

        cboallowfreq.DataBindings.Clear()
        cboallowfreq.DataBindings.Add("Text", Me._currentAllowance, "AllowanceFrequency")

        AllowanceDetailsTabLayout.Enabled = True

    End Sub

    Private Sub ResetAllowanceForm()

        Me._currentAllowance = Nothing

        AllowanceDetailsTabLayout.Enabled = False

        cboallowtype.SelectedIndex = -1
        cboallowtype.DataBindings.Clear()

        cboallowfreq.SelectedIndex = -1
        cboallowfreq.DataBindings.Clear()

        dtpallowstartdate.ResetText()
        dtpallowstartdate.DataBindings.Clear()

        NullableDatePicker1.ResetText()
        NullableDatePicker1.DataBindings.Clear()

        txtallowamt.Clear()
        txtallowamt.DataBindings.Clear()

    End Sub

    Private Function GetSelectedAllowance() As Allowance
        Return CType(dgvempallowance.CurrentRow.DataBoundItem, Allowance)
    End Function

    Private Sub ForceAllowanceGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        EmployeeInfoTabLayout.Focus()
    End Sub

    Private Function CheckIfAllowanceIsChanged(newAllowance As Allowance) As Boolean
        If _currentAllowance Is Nothing Then Return False

        Dim oldAllowance =
            Me._unchangedAllowances.
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

    Dim activeEmpSorted As List(Of Simplified.Employee)
    Dim allEmpSorted As List(Of GridView.Employee)

    Dim gotAllEmp As Boolean = False
    Dim gotActiveEmp As Boolean = False

    Private Async Sub cbShowAll_CheckedChanged(sender As Object, e As EventArgs) Handles cbShowAll.CheckedChanged
        Await ShowEmployeeList()
    End Sub

    Private Async Function ShowEmployeeList() As Task
        If cbShowAll.Checked Then
            If Not gotAllEmp Then
                Dim allEmp = Await _employeeRepository.GetAllAsync(Of Simplified.Employee)()
                allEmpSorted = CType(allEmp, List(Of Simplified.Employee)).
                    OrderBy(Function(emp) emp.LastName).
                    ToList()
                gotAllEmp = True
            End If
            Me._employees = allEmpSorted
        Else
            If Not gotActiveEmp Then
                Using context As New PayrollContext
                    Dim list = context.Employees.
                            Where(Function(emp) Nullable.Equals(emp.OrganizationID, z_OrganizationID) And
                                      emp.IsActive)

                    Dim activeEmp = Await list.
                                Select(Function(emp) New GridView.Employee With {
                                    .RowID = emp.RowID,
                                    .EmployeeNo = emp.EmployeeNo,
                                    .FirstName = emp.FirstName,
                                    .MiddleName = emp.MiddleName,
                                    .LastName = emp.LastName,
                                    .Image = emp.Image
                                }).
                                ToListAsync
                    activeEmpSorted = activeEmp.
                        OrderBy(Function(emp) emp.LastName).
                        ToList()
                End Using

                gotActiveEmp = True
            End If
            Me._employees = activeEmpSorted
        End If

        Await FilterEmployees()
    End Function

End Class