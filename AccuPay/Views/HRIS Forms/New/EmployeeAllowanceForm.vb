Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Repository
Imports AccuPay.SimplifiedEntities
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports MySql.Data.MySqlClient
Imports Simplified = AccuPay.SimplifiedEntities.GridView

Public Class EmployeeAllowanceForm
    Private _employeeRepository As New EmployeeRepository

    Private _allowanceTypeList As List(Of Product)

    Private _employees As New List(Of Simplified.Employee)
    Private _currentAllowance As New Allowance

    Private _currentAllowances As New List(Of Allowance)

    Private _unchangedAllowances As New List(Of Allowance)

    Private threadArrayList As New List(Of Thread)

    Private Async Sub EmployeeAllowancesForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        InitializeComponentSettings()

        cbShowAll_CheckedChanged(sender, e)

        ResetAllowanceForm()

        Await LoadAllowanceTypes()
        Await LoadEmployees()

    End Sub

    Private Async Sub searchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
        Dim searchValue = searchTextBox.Text.ToLower()

        Await LoadEmployees(searchValue)

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Async Sub employeesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles dgvEmp.SelectionChanged

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

    Private Sub lnlAddAllowanceType_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklbaddallowtype.LinkClicked

        Dim form As New AddAllowanceTypeForm()
        form.ShowDialog()

        If form.IsSaved Then

            Me._allowanceTypeList.Add(form.NewAllowanceType)

            PopulateAllowanceTypeCombobox()

            If Me._currentAllowance IsNot Nothing Then

                Me._currentAllowance.ProductID = form.NewAllowanceType.RowID
                'Me._currentAllowance.Type = form.NewAllowanceType.PartNo

                Dim orderedAllowanceTypeList = Me._allowanceTypeList.OrderBy(Function(p) p.PartNo).ToList

                cboallowtype.SelectedIndex = orderedAllowanceTypeList.IndexOf(form.NewAllowanceType)

            End If

            ShowBalloonInfo("Allowance Type Successfully Added", "Saved")
        End If

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

        Try

            Using context As New PayrollContext
                For Each allowance In changedAllowances
                    If allowance.RowID Is Nothing Then
                        allowance.Created = Date.Now
                        allowance.CreatedBy = z_User

                        context.Allowances.Add(allowance)
                    Else
                        allowance.LastUpdBy = z_User

                        context.Entry(allowance).State = EntityState.Modified
                    End If

                    Await context.SaveChangesAsync()
                Next
            End Using

            ShowBalloonInfo($"{changedAllowances.Count} Allowance(s) Successfully Updated.", messageTitle)

            Dim currentEmployee = GetSelectedEmployee()

            If currentEmployee Is Nothing Then Return

            Await LoadAllowances(currentEmployee)

        Catch ex As ArgumentException

            Dim errorMessage = "One of the updated allowances has an error:" & Environment.NewLine & ex.Message

            MessageBoxHelper.ErrorMessage(errorMessage, messageTitle)

        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

        End Try
    End Sub

    Private Sub combobox_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboallowfreq.SelectedValueChanged, cboallowtype.SelectedValueChanged
        If sender Is cboallowtype AndAlso Me._currentAllowance IsNot Nothing Then
            Dim selectedAllowanceType = Me._allowanceTypeList.FirstOrDefault(Function(l) (l.PartNo = cboallowtype.Text))

            If selectedAllowanceType Is Nothing Then
                Me._currentAllowance.ProductID = Nothing
            Else
                Me._currentAllowance.ProductID = selectedAllowanceType.RowID
                Me._currentAllowance.Product.PartNo = selectedAllowanceType.PartNo
                AllowanceBindingSource_CurrentItemChanged(Nothing, Nothing) 'Force refresh to show PartNo update
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

        Dim currentAllowance

        Using context = New PayrollContext()

            currentAllowance = Await context.Allowances.FirstOrDefaultAsync(Function(l) Nullable.Equals(l.RowID, _currentAllowance.RowID))

        End Using

        If currentAllowance Is Nothing Then

            MessageBoxHelper.Warning("Allowance not found in database! Please close this form then open again.")

            Return
        End If

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete this?", "Confirm Deletion") = False Then

            Return
        End If

        Try

            Using context = New PayrollContext()

                context.Remove(currentAllowance)

                Await context.SaveChangesAsync()

            End Using

            Await LoadAllowances(currentEmployee)

            ShowBalloonInfo("Successfully Deleted.", messageTitle)

        Catch ex As ArgumentException

            MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)

        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

        End Try

    End Sub

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

#Region "Private Functions"

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, lblFormTitle, 400)
    End Sub

    Private Sub InitializeComponentSettings()
        dgvempallowance.AutoGenerateColumns = False
        dgvEmp.AutoGenerateColumns = False
    End Sub

    Private Async Function LoadEmployees(Optional searchValue As String = "") As Task
        Dim filteredEmployees As New List(Of Simplified.Employee)

        If String.IsNullOrEmpty(searchValue) Then
            dgvEmp.DataSource = Me._employees
        Else
            dgvEmp.DataSource =
                Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        End If
    End Function

    Private Async Function LoadAllowances(currentEmployee As Simplified.Employee) As Task
        If currentEmployee Is Nothing Then Return

        Using context = New PayrollContext()
            Me._currentAllowances = Await context.Allowances.
                        Include(Function(p) p.Product).
                        Where(Function(a) Nullable.Equals(a.EmployeeID, currentEmployee.RowID)).
                        ToListAsync
        End Using

        If Me._currentAllowances.Count = 0 Then
            Me._currentAllowances.Add(Nothing)
        End If

        Me._unchangedAllowances = Me._currentAllowances.CloneListJson()

        AllowancesBindingSource.DataSource = Me._currentAllowances

        dgvempallowance.DataSource = AllowancesBindingSource

    End Function

    Private Async Function LoadAllowanceTypes() As Task
        Dim categoryName = ProductConstant.ALLOWANCE_TYPE_CATEGORY

        Using context = New PayrollContext()

            Dim category = Await context.Categories.
                                Where(Function(c) Nullable.Equals(c.OrganizationID, z_OrganizationID)).
                                Where(Function(c) c.CategoryName = categoryName).
                                FirstOrDefaultAsync


            If category Is Nothing Then
                'get the existing category with same name to use as CategoryID
                Dim existingCategoryProduct = Await context.Categories.
                                Where(Function(c) c.CategoryName = categoryName).
                                FirstOrDefaultAsync

                Dim existingCategoryProductId = existingCategoryProduct?.RowID


                category = New Category
                category.CategoryID = existingCategoryProductId
                category.CategoryName = categoryName
                category.OrganizationID = z_OrganizationID
                category.CatalogID = Nothing
                category.LastUpd = Date.Now

                context.Categories.Add(category)
                context.SaveChanges()

                'if there is no existing category with same name,
                'use the newly added category's RowID as its CategoryID

                If existingCategoryProductId Is Nothing Then

                    Try
                        category.CategoryID = category.RowID
                        Await context.SaveChangesAsync()

                    Catch ex As Exception
                        'if for some reason hindi na update, we can't let that row
                        'to have no CategoryID so dapat i-delete rin yung added category
                        context.Categories.Remove(category)
                        context.SaveChanges()

                        Throw ex
                    End Try

                End If
            End If

            If category Is Nothing Then
                Dim ex = New Exception("ProductRepository->GetOrCreate: Category not found.")
                Throw ex
            End If

            Me._allowanceTypeList = Await context.Products.
                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                Where(Function(p) Nullable.Equals(p.CategoryID, category.RowID)).
                ToListAsync
        End Using

        PopulateAllowanceTypeCombobox()
    End Function

    Private Sub PopulateAllowanceTypeCombobox()
        Dim stringList As List(Of String)
        stringList = New List(Of String)

        For Each product In Me._allowanceTypeList

            Select Case "PartNo"
                Case "Name"
                    stringList.Add(product.PartNo)

                Case Else
                    stringList.Add(product.Name)
            End Select
        Next

        Dim allowanceTypes = stringList.OrderBy(Function(s) s).ToList

        cboallowtype.DataSource = allowanceTypes
    End Sub

    Private Function GetSelectedEmployee() As Simplified.Employee
        If dgvEmp.CurrentRow Is Nothing Then Return Nothing

        Return CType(dgvEmp.CurrentRow.DataBoundItem, Simplified.Employee)
    End Function

    Private Sub PopulateAllowanceForm(allowance As Allowance)
        Me._currentAllowance = allowance

        Dim originalAllowance = Me._unchangedAllowances.
            FirstOrDefault(Function(l) Nullable.Equals(l.RowID, Me._currentAllowance.RowID))

        txtallowamt.DataBindings.Clear()
        txtallowamt.DataBindings.Add("Text", Me._currentAllowance, "Amount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        dtpallowstartdate.DataBindings.Clear()
        dtpallowstartdate.DataBindings.Add("Value", Me._currentAllowance, "EffectiveStartDate", True, DataSourceUpdateMode.OnPropertyChanged, Nothing)

        dtpallowenddate.DataBindings.Clear()
        dtpallowenddate.DataBindings.Add("Value", Me._currentAllowance, "EffectiveEndDate", True, DataSourceUpdateMode.OnPropertyChanged, Nothing)

        cboallowtype.DataBindings.Clear()
        cboallowtype.DataBindings.Add("Text", Me._currentAllowance, "Type", True, DataSourceUpdateMode.OnPropertyChanged, Nothing)

        cboallowfreq.DataBindings.Clear()
        cboallowfreq.DataBindings.Add("Text", Me._currentAllowance, "AllowanceFrequency", True, DataSourceUpdateMode.OnPropertyChanged, Nothing)

    End Sub

    Private Sub ResetAllowanceForm()

        Me._currentAllowance = Nothing

        cboallowtype.DataBindings.Clear()
        cboallowtype.SelectedIndex = -1

        cboallowfreq.DataBindings.Clear()
        cboallowfreq.SelectedIndex = -1

        dtpallowstartdate.DataBindings.Clear()
        dtpallowstartdate.ResetText()

        dtpallowenddate.DataBindings.Clear()
        dtpallowenddate.ResetText()

        txtallowamt.DataBindings.Clear()
        txtallowamt.Clear()

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
            newAllowance.OrganizationID <> oldAllowance.OrganizationID OrElse
            newAllowance.AllowanceFrequency <> oldAllowance.AllowanceFrequency OrElse
            newAllowance.Product.PartNo <> oldAllowance.Product.PartNo OrElse
            newAllowance.Type <> oldAllowance.Type OrElse
            newAllowance.Amount <> oldAllowance.Amount OrElse
            newAllowance.EffectiveStartDate <> oldAllowance.EffectiveStartDate OrElse
            newAllowance.EffectiveEndDate <> oldAllowance.EffectiveEndDate Then

            hasChanged = True
        End If

        Return hasChanged

    End Function

#End Region

#Region "DateTimePickers"
    Dim empallow_daterangehasrecord

    Private Sub dtpallowstartdate_ValueChanged(sender As Object, e As EventArgs) Handles dtpallowstartdate.ValueChanged
        empallow_daterangehasrecord = 0
        Dim date_range = DateDiff(DateInterval.Day, CDate(dtpallowstartdate.Value), CDate(dtpallowenddate.Value))

        If date_range < 0 And cboallowfreq.SelectedIndex <> 2 And cboallowfreq.SelectedIndex <> -1 Then
            empallow_daterangehasrecord = 1
            WarnBalloon("please supply a valid date range.", "invalid date range", dtpallowstartdate, dtpallowstartdate.Width - 16, -69)

            dtpallowenddate.Value = dtpallowstartdate.Value
        End If
    End Sub

    Private Sub dtpallowenddate_ValueChanged(sender As Object, e As EventArgs) Handles dtpallowenddate.ValueChanged
        cbDtpOverlay.Checked = True
        'If Not dtpallowenddate.Checked And _currentAllowance IsNot Nothing Then
        '    _currentAllowance.EffectiveEndDate = Nothing
        'End If

        empallow_daterangehasrecord = 0
        Dim date_range = DateDiff(DateInterval.Day, CDate(dtpallowstartdate.Value), CDate(dtpallowenddate.Value))

        If date_range < 0 And cboallowfreq.SelectedIndex <> 2 And cboallowfreq.SelectedIndex <> -1 Then
            empallow_daterangehasrecord = 1
            WarnBalloon("Please supply a valid date range.", "Invalid date range", dtpallowenddate, dtpallowenddate.Width - 16, -69)

            dtpallowstartdate.Value = dtpallowenddate.Value
        End If
    End Sub

    Private Sub cbDtpOverlay_CheckedChanged(sender As Object, e As EventArgs) Handles cbDtpOverlay.CheckedChanged
        dtpallowenddate.Checked = cbDtpOverlay.Checked

        If Not cbDtpOverlay.Checked And _currentAllowance IsNot Nothing Then
            _currentAllowance.EffectiveEndDate = Nothing
        End If
    End Sub
#End Region
    Dim activeEmpSorted As List(Of Simplified.Employee)
    Dim allEmpSorted As List(Of GridView.Employee)

    Dim gotAllEmp As Boolean = False
    Dim gotActiveEmp As Boolean = False

    Private Async Sub cbShowAll_CheckedChanged(sender As Object, e As EventArgs) Handles cbShowAll.CheckedChanged
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
                                      emp.EmploymentStatus <> "Terminated" And
                                      emp.EmploymentStatus <> "Resigned")

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

        Await LoadEmployees()
    End Sub

End Class