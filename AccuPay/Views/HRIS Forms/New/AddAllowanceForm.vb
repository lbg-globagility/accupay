Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports Simplified = AccuPay.SimplifiedEntities.GridView

Public Class AddAllowanceForm
    Private _currentEmployee As Simplified.Employee

    Private _newAllowance As New Allowance

    Private _allowanceTypeList As List(Of Product)

    Public Property IsSaved As Boolean

    Public Property ShowBalloonSuccess As Boolean

    Public Property NewAllowanceTypes As List(Of Product)

    Sub New(employee As Simplified.Employee)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _currentEmployee = employee

        Me.IsSaved = False

        Me.NewAllowanceTypes = New List(Of Product)

    End Sub

    Private Async Sub AddAllowanceForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        PopulateEmployeeData()

        Await LoadAllowanceTypes()

        ResetForm()

    End Sub

    Private Sub ResetForm()
        Me._newAllowance = New Allowance
        'Me._newAllowance.Product = New Product
        Me._newAllowance.EmployeeID = _currentEmployee.RowID
        Me._newAllowance.EffectiveStartDate = Date.Now
        Me._newAllowance.EffectiveEndDate = Date.Now

        CreateDataBindings()
    End Sub

    Private Sub PopulateEmployeeData()

        txtEmployeeFirstName.Text = _currentEmployee?.FullNameWithMiddleNameInitial

        txtEmployeeNumber.Text = _currentEmployee?.EmployeeNo

        pbEmployeePicture.Image = ConvByteToImage(_currentEmployee.Image)

    End Sub

    Private Sub CreateDataBindings()

        cboAllowType.DataBindings.Clear()
        cboAllowType.DataBindings.Add("Text", Me._newAllowance, "Type")

        cboAllowFreq.DataBindings.Clear()
        cboAllowFreq.DataBindings.Add("Text", Me._newAllowance, "AllowanceFrequency")

        dtpallowstartdate.DataBindings.Clear()
        dtpallowstartdate.DataBindings.Add("Value", Me._newAllowance, "EffectiveStartDate")

        dtpallowenddate.DataBindings.Clear()
        dtpallowenddate.DataBindings.Add("Value", Me._newAllowance, "EffectiveEndDate")

        txtPeriodicAllowanceAmount.DataBindings.Clear()
        txtPeriodicAllowanceAmount.DataBindings.Add("Text", Me._newAllowance, "Amount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cboAllowType_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboAllowType.SelectedValueChanged
        If sender Is cboAllowType AndAlso Me._newAllowance IsNot Nothing Then
            Dim selectedAllowType = Me._allowanceTypeList.FirstOrDefault(Function(l) l.PartNo = cboAllowType.Text)

            If selectedAllowType Is Nothing Then

                Me._newAllowance.ProductID = Nothing

            Else

                Me._newAllowance.ProductID = selectedAllowType.RowID

            End If
        End If
    End Sub

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAddAndNew.Click, btnAddAndClose.Click

        ForceAllowanceGridViewCommit()

        Dim confirmMessage = ""

        If Me._newAllowance.Amount = 0 Then
            confirmMessage = "You did not enter a value for Allowance Amount. Do you want to save the new allowance?"
        End If

        If String.IsNullOrWhiteSpace(confirmMessage) = False Then

            If MessageBoxHelper.Confirm(Of Boolean)(confirmMessage, "New Allowance") = False Then Return

        End If

        If Not dtpallowenddate.Checked Then
            _newAllowance.EffectiveEndDate = Nothing
        End If

        Using context As New PayrollContext
            If _newAllowance.RowID Is Nothing Then
                _newAllowance.OrganizationID = z_OrganizationID
                _newAllowance.Created = Date.Now
                _newAllowance.CreatedBy = z_User
                context.Allowances.Add(_newAllowance)
            Else
                Dim oldAllowance = Await context.Allowances.FirstOrDefaultAsync(Function(l) Nullable.Equals(l.RowID, _newAllowance.RowID))

                _newAllowance.LastUpd = Date.Now
                _newAllowance.LastUpdBy = z_User

                context.Allowances.Attach(_newAllowance)
                context.Entry(_newAllowance).State = EntityState.Modified
            End If

            Await context.SaveChangesAsync()
        End Using

        Me.IsSaved = True

        If sender Is btnAddAndNew Then
            ShowBalloonInfo("Allowance Successfully Added", "Saved")

            ResetForm()

        Else

            Me.ShowBalloonSuccess = True
            Me.Close()
        End If

    End Sub

    Private Sub lnlAddAllowanceType_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnlAddAllowanceType.LinkClicked
        Dim form As New AddAllowanceTypeForm()
        form.ShowDialog()

        If form.IsSaved Then

            Me._allowanceTypeList.Add(form.NewAllowanceType)

            Me.NewAllowanceTypes.Add(form.NewAllowanceType)

            PopulateAllowanceTypeCombobox()

            If Me._newAllowance IsNot Nothing Then
                Me._newAllowance.ProductID = form.NewAllowanceType.RowID
                'Me._newAllowance.Product.PartNo = form.NewAllowanceType.PartNo

                Dim orderedAllowanceTypeList = Me._allowanceTypeList.OrderBy(Function(p) p.PartNo).ToList

                cboAllowType.SelectedIndex = orderedAllowanceTypeList.IndexOf(form.NewAllowanceType)

            End If

            ShowBalloonInfo("Allowance Type Successfully Added", "Saved")
        End If
    End Sub

    Private Sub cbo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboAllowFreq.KeyPress, cboAllowType.KeyPress

        e.Handled = True

    End Sub

    Private Sub cboallowfreq_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboAllowFreq.SelectedValueChanged  ', cboallowfreq.SelectedIndexChanged

        dtpallowstartdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]

        Select Case cboAllowFreq.SelectedIndex

            Case 0 To 1 'Daily & Monthly
                dtpallowstartdate.Enabled = 1
                dtpallowenddate.Enabled = 1

                lblreqstartdate.Visible = 1

            Case 2 'One time
                dtpallowstartdate.Enabled = 1
                dtpallowenddate.Enabled = 0

                lblreqstartdate.Visible = 1
            Case 3 To 4 'Semi-monthly & Weekly
                dtpallowstartdate.Enabled = 1
                dtpallowenddate.Enabled = 1

                lblreqstartdate.Visible = 1

            Case Else 'Nothing
                dtpallowstartdate.Enabled = 0
                dtpallowenddate.Enabled = 0

                lblreqstartdate.Visible = 0

        End Select

        _newAllowance.AllowanceFrequency = cboAllowFreq.Text
    End Sub

    Private amountBeforeTextChange As Decimal

    Private Sub txtAllowanceAmount_Enter(sender As Object, e As EventArgs) Handles txtPeriodicAllowanceAmount.Enter

        If Me._newAllowance Is Nothing Then Return

        amountBeforeTextChange = Me._newAllowance.Amount

    End Sub

#Region "Private Functions"

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
                Dim ex = New Exception("GetOrCreate: Category not found.")
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

        cboAllowType.DataSource = allowanceTypes
        cboAllowType.Text = ""
    End Sub

    Private Sub ForceAllowanceGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        pbEmployeePicture.Focus()
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, EmployeeInfoTabLayout, 400)
    End Sub
#End Region
End Class