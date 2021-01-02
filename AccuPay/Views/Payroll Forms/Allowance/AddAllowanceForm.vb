Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddAllowanceForm

    Private _currentEmployee As Employee

    Private _newAllowance As New Allowance()

    Private _allowanceTypeList As List(Of Product)

    Public Property NewAllowanceTypes As List(Of Product)

    Public Property IsSaved As Boolean

    Public Property ShowBalloonSuccess As Boolean

    Private ReadOnly _productRepository As IProductRepository

    Private ReadOnly _allowanceRepository As IAllowanceRepository

    Sub New(employee As Employee)

        InitializeComponent()

        _currentEmployee = employee

        _allowanceRepository = MainServiceProvider.GetRequiredService(Of IAllowanceRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)

        Me.IsSaved = False

        Me.NewAllowanceTypes = New List(Of Product)

    End Sub

    Private Async Sub AddAllowanceForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        PopulateEmployeeData()

        LoadFrequencyList()

        Await LoadAllowanceTypes()

        ResetForm()

    End Sub

    Private Sub PopulateEmployeeData()

        txtEmployeeFirstName.Text = _currentEmployee?.FullNameWithMiddleInitial

        txtEmployeeNumber.Text = _currentEmployee?.EmployeeIdWithPositionAndEmployeeType

        pbEmployeePicture.Image = ConvByteToImage(_currentEmployee.Image)

    End Sub

    Private Sub ResetForm()
        Me._newAllowance = New Allowance With {
            .EmployeeID = _currentEmployee.RowID,
            .EffectiveStartDate = Date.Now,
            .EffectiveEndDate = Date.Now,
            .OrganizationID = z_OrganizationID
        }

        Dim firstAllowanceType = Me._allowanceTypeList.FirstOrDefault()

        If firstAllowanceType IsNot Nothing Then
            Me._newAllowance.ProductID = firstAllowanceType.RowID
            Me._newAllowance.Product = firstAllowanceType
        End If

        Me._newAllowance.AllowanceFrequency = cboallowfreq.SelectedItem.ToString()

        CreateDataBindings()
    End Sub

    Private Sub CreateDataBindings()

        cboallowtype.DataBindings.Clear()
        cboallowtype.DataBindings.Add("Text", Me._newAllowance, "Type")

        cboallowfreq.DataBindings.Clear()
        cboallowfreq.DataBindings.Add("Text", Me._newAllowance, "AllowanceFrequency")

        dtpallowstartdate.DataBindings.Clear()
        dtpallowstartdate.DataBindings.Add("Value", Me._newAllowance, "EffectiveStartDate")

        dtpallowenddate.DataBindings.Clear()
        dtpallowenddate.DataBindings.Add("Value", Me._newAllowance, "EffectiveEndDate", True, DataSourceUpdateMode.OnPropertyChanged, Nothing)

        txtallowamt.DataBindings.Clear()
        txtallowamt.DataBindings.Add("Text", Me._newAllowance, "Amount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cboallowtype_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboallowtype.SelectedValueChanged
        If Me._newAllowance IsNot Nothing Then
            Dim selectedAllowanceType = Me._allowanceTypeList.FirstOrDefault(Function(l) (l.PartNo = cboallowtype.Text))

            If selectedAllowanceType Is Nothing Then
                Me._newAllowance.ProductID = Nothing
                Me._newAllowance.Product = Nothing
            Else
                Me._newAllowance.ProductID = selectedAllowanceType.RowID
                Me._newAllowance.Product = selectedAllowanceType

            End If
        End If

    End Sub

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAddAndNew.Click, btnAddAndClose.Click

        ForceAllowanceDataBindingsCommit()

        Dim confirmMessage = ""
        Dim messageTitle = "New Allowance"

        If Me._newAllowance.ProductID Is Nothing Then

            MessageBoxHelper.ErrorMessage("Please select an allowance type.")
            Return
        End If

        If Me._newAllowance.Amount = 0 Then
            confirmMessage = "You did not enter a value for Allowance Amount. Do you want to save the new allowance?"
        End If

        If String.IsNullOrWhiteSpace(confirmMessage) = False Then

            If MessageBoxHelper.Confirm(Of Boolean) _
                (confirmMessage, messageTitle, messageBoxIcon:=MessageBoxIcon.Warning) = False Then Return

        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of IAllowanceDataService)
                Await dataService.SaveAsync(Me._newAllowance, z_User)

                Me.IsSaved = True

                If sender Is btnAddAndNew Then
                    ShowBalloonInfo("Allowance Successfully Added", "Saved")

                    ResetForm()
                Else

                    Me.ShowBalloonSuccess = True
                    Me.Close()
                End If
            End Function)

    End Sub

    Private Sub Cboallowfreq_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboallowfreq.SelectedValueChanged
        If Me._newAllowance Is Nothing Then Return

        Dim showEndDate = Not cboallowfreq.Text = Allowance.FREQUENCY_ONE_TIME

        lblEndDate.Visible = showEndDate
        dtpallowenddate.Visible = showEndDate

    End Sub

#Region "Private Functions"

    Private Sub LoadFrequencyList()

        cboallowfreq.DataSource = _allowanceRepository.GetFrequencyList()

    End Sub

    Private Async Function LoadAllowanceTypes() As Task

        Dim allowanceList = New List(Of Product)(Await _productRepository.GetAllowanceTypesAsync(z_OrganizationID))

        Me._allowanceTypeList = allowanceList.Where(Function(a) a.PartNo IsNot Nothing).
                                                Where(Function(a) a.PartNo.Trim <> String.Empty).
                                                ToList

        PopulateAllowanceTypeCombobox()

    End Function

    Private Sub PopulateAllowanceTypeCombobox()
        Dim allowanceTypes = _productRepository.ConvertToStringList(Me._allowanceTypeList)

        cboallowtype.DataSource = allowanceTypes
    End Sub

    Private Sub ForceAllowanceDataBindingsCommit()
        'Workaround. Focus other control to lose focus on current control
        pbEmployeePicture.Focus()
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, EmployeeInfoTabLayout, 400)
    End Sub

    Private Async Sub lnklbaddallowtype_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklbaddallowtype.LinkClicked

        Dim n_ProductControlForm As New ProductControlForm

        With n_ProductControlForm

            .Status.HeaderText = "Taxable Flag"

            .PartNo.HeaderText = "Allowance name"

            .NameOfCategory = ProductConstant.ALLOWANCE_TYPE_CATEGORY

            If n_ProductControlForm.ShowDialog = Windows.Forms.DialogResult.OK Then

                If .IsSaved Then

                    Dim oldSelectedAllowanceId = Me._newAllowance.ProductID

                    Await LoadAllowanceTypes()

                    Dim oldSelectedAllowance = Me._allowanceTypeList.FirstOrDefault(Function(a) Nullable.Equals(a.RowID, oldSelectedAllowanceId))

                    If oldSelectedAllowance Is Nothing Then Return

                    Dim orderedAllowanceTypeList = Me._allowanceTypeList.OrderBy(Function(p) p.PartNo).ToList

                    cboallowtype.SelectedIndex = orderedAllowanceTypeList.IndexOf(oldSelectedAllowance)

                End If

            End If

        End With

    End Sub

#End Region

End Class
