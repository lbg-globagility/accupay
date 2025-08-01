Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddBonusForm

    Public Property IsSaved As Boolean

    Public Property ShowBalloon As Boolean

    Private _products As IEnumerable(Of Product)

    Private _frequencies As New List(Of String)

    Private _employee As Employee

    Private _newBonus As New Bonus()

    Private ReadOnly _productRepo As IProductRepository

    Public Sub New(employee As Employee)

        InitializeComponent()

        _employee = employee

        _productRepo = MainServiceProvider.GetRequiredService(Of IProductRepository)

    End Sub

    Private Async Sub AddBonusForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EmployeeNameTextBox.Text = _employee.FullNameWithMiddleInitialLastNameFirst
        EmployeeNumberTextbox.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        EmployeePictureBox.Image = ConvByteToImage(_employee.Image)

        _products = Await _productRepo.GetBonusTypesAsync(z_OrganizationID)

        Dim bonusRepo = MainServiceProvider.GetRequiredService(Of IBonusRepository)
        _frequencies = bonusRepo.GetFrequencyList()

        BindDataSource()
        ClearForm()

    End Sub

    Private Sub BindDataSource()
        cbobonfreq.DataSource = _frequencies
        cbobontype.DisplayMember = "Name"
        cbobontype.DataSource = _products
    End Sub

    Private Sub ClearForm()
        cbobontype.SelectedItem = Nothing
        cbobonfreq.SelectedItem = Nothing
        txtbonamt.Text = ""
        dtpbonstartdate.Value = Today
        dtpbonenddate.Value = Today
    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Sub cbobonfreq_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbobonfreq.SelectedIndexChanged
        If cbobonfreq.SelectedItem Is Nothing Then Return

        If cbobonfreq.SelectedItem.ToString = Bonus.FREQUENCY_ONE_TIME Then
            dtpbonenddate.Enabled = False
            dtpbonenddate.Value = dtpbonstartdate.Value
        Else
            dtpbonenddate.Enabled = True
        End If
    End Sub

    Private Sub dtpbonstartdate_ValueChanged(sender As Object, e As EventArgs) Handles dtpbonstartdate.ValueChanged
        If Not dtpbonenddate.Enabled Or dtpbonenddate.Value < dtpbonstartdate.Value Then
            dtpbonenddate.Value = dtpbonstartdate.Value
        End If
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles AddAndCloseButton.Click, AddAndNewButton.Click
        EmployeePictureBox.Focus() 'To lose focus on current control
        Dim succeed As Boolean
        Dim messageTitle = ""
        If cbobontype.SelectedItem Is Nothing Then
            messageTitle = "Invalid Bonus Type"
        ElseIf cbobonfreq.SelectedItem Is Nothing Then
            messageTitle = "Invalid Bonus Frequency"
        ElseIf txtbonamt.Text = "" OrElse IsNumeric(txtbonamt.Text) = False Then
            messageTitle = "Invalid Amount"
        End If

        If messageTitle <> "" Then
            ShowBalloonInfo("Error Input.", messageTitle)
            Return
        End If

        Dim product = _products.
            Where(Function(x) x.Name = cbobontype.Text).
            FirstOrDefault()

        _newBonus = New Bonus
        Await FunctionUtils.TryCatchFunctionAsync("New Bonus",
        Async Function()
            With _newBonus
                .ProductID = product.RowID
                .AllowanceFrequency = cbobonfreq.SelectedItem.ToString
                .BonusAmount = CType(txtbonamt.Text, Decimal?)
                .EffectiveStartDate = dtpbonstartdate.Value
                .EffectiveEndDate = dtpbonenddate.Value
                .OrganizationID = z_OrganizationID
                .EmployeeID = _employee.RowID
                .TaxableFlag = product.Status
            End With

            Dim dataService = MainServiceProvider.GetRequiredService(Of IBonusDataService)
            Await dataService.SaveAsync(_newBonus, z_User)

            succeed = True
        End Function)

        If succeed Then
            IsSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Bonus successfully added.", "Saved")
                ClearForm()
            Else
                ShowBalloon = True
                Me.Close()
            End If
        End If

    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, EmployeePictureBox, 67, -67)
    End Sub

    Private Sub dtpbonenddate_ValueChanged(sender As Object, e As EventArgs) Handles dtpbonenddate.ValueChanged
        If dtpbonenddate.Value < dtpbonstartdate.Value Then
            dtpbonenddate.Value = dtpbonstartdate.Value
        End If
    End Sub

End Class
