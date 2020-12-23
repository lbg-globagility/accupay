Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.DependencyInjection

Public Class OrganizationForm

    Private Const EmptyIndex As Integer = -1
    Private CurrentYear As Integer = Date.Now.Year

    Private _currentRolePermission As RolePermission
    Private _currentOrganization As Organization
    Private _organizations As List(Of Organization)
    Private ReadOnly _addressRepository As AddressRepository
    Private ReadOnly _organizationRepository As OrganizationRepository
    Private ReadOnly _policy As IPolicyHelper

    Sub New()

        InitializeComponent()

        _addressRepository = MainServiceProvider.GetRequiredService(Of AddressRepository)
        _organizationRepository = MainServiceProvider.GetRequiredService(Of OrganizationRepository)
        _policy = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

    End Sub

    Private Async Sub OrganizationForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        AddressComboBox.DisplayMember = "FullAddress"
        AddressComboBox.ValueMember = "RowID"
        Await FillAddress()

        OrganizationGridView.AutoGenerateColumns = False
        Await FillOrganizationList()

        FirstPayPeriodGroupBox.Visible = _policy.HasDifferentPayPeriodDates
        SetMaxAndMinDateOfFirstPayPeriod()

        Await RestrictByRole()

        AddHandler OrganizationGridView.SelectionChanged, AddressOf OrganizationGridView_SelectionChanged
        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

        chkTimeLogsOnlyRequirement.Visible = _policy.PaidAsLongAsHasTimeLog
    End Sub

    Private Async Function RestrictByRole() As Task
        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.ORGANIZATION)

        NewButton.Visible = False
        SaveButton.Visible = False
        CancelToolStripButton.Visible = False

        If role.Success Then
            _currentRolePermission = role.RolePermission

            If _currentRolePermission.Create Then
                NewButton.Visible = True

            End If

            If _currentRolePermission.Update OrElse _currentRolePermission.Create Then
                SaveButton.Visible = True
                CancelToolStripButton.Visible = True
            End If
        Else
            SplitContainer1.Panel1.Enabled = False

        End If
    End Function

    Private Async Function ClearTextBoxes() As Task

        txtcompanyName.Clear()
        txttradeName.Clear()
        AddressComboBox.SelectedIndex = EmptyIndex
        txtcompMainPhoneTxt.Clear()
        txtcompFaxNumTxt.Clear()
        txtcompEmailTxt.Clear()
        IsAgencyCheckBox.Checked = False
        txtRDO.Clear()
        txtZIP.Clear()
        txtcompUrl.Clear()
        txtcompAltPhoneTxt.Clear()
        txtorgTinNumTxt.Clear()
        txtcompAltEmailTxt.Clear()
        PhotoImages.Image = Nothing

        FirstPayPeriodGroupBox.Enabled = False

        Dim currentDate = Date.Now.Date
        nightdiffshiftfrom.Value = currentDate.Add(Organization.DefaultNightDifferentialTimeFrom)
        nightdiffshiftto.Value = currentDate.Add(Organization.DefaultNightDifferentialTimeTo)

        If _policy.HasDifferentPayPeriodDates Then

            FillFirstPayPeriodData()

        End If

        Await OrganizationUserRolesControl.SetOrganization(Nothing, isReadOnly:=False)

    End Function

    Async Function FillAddress() As Task
        AddressComboBox.DataSource = (Await _addressRepository.GetAllAsync()).
            OrderBy(Function(a) a.FullAddress).
            ToList()
    End Function

    Private Async Function FillOrganizationList() As Task

        RemoveHandler OrganizationGridView.SelectionChanged, AddressOf OrganizationGridView_SelectionChanged

        Dim list = Await _organizationRepository.List(OrganizationPageOptions.AllData, Z_Client)

        Dim organizations = list.organizations.
            OrderBy(Function(o) o.Name).
            ToList()

        Dim userRepository = MainServiceProvider.GetRequiredService(Of AspNetUserRepository)
        Dim userRoles = Await userRepository.GetUserRolesAsync(z_User)

        Dim allowedOrganizations = userRoles.
            GroupBy(Function(o) o.OrganizationId).
            Select(Function(o) o.Key).
            ToArray()

        _organizations = organizations.
        Where(Function(o) allowedOrganizations.Contains(o.RowID.Value)).
        ToList()

        OrganizationGridView.DataSource = _organizations

        Await FillOrganizationData()

        AddHandler OrganizationGridView.SelectionChanged, AddressOf OrganizationGridView_SelectionChanged

    End Function

    Private Async Function FillOrganizationData() As Task

        Await ClearTextBoxes()

        _currentOrganization = GetSelectedOrganization()

        If _currentOrganization IsNot Nothing Then

            txtcompanyName.Text = _currentOrganization.Name
            txttradeName.Text = _currentOrganization.TradeName

            If _currentOrganization.PrimaryAddressId Is Nothing Then

                AddressComboBox.SelectedIndex = -1
            Else

                AddressComboBox.SelectedValue = _currentOrganization.PrimaryAddressId

            End If

            txtcompMainPhoneTxt.Text = _currentOrganization.MainPhone
            txtcompFaxNumTxt.Text = _currentOrganization.FaxNumber
            txtcompEmailTxt.Text = _currentOrganization.EmailAddress
            IsAgencyCheckBox.Checked = _currentOrganization.IsAgency
            txtRDO.Text = _currentOrganization.RDOCode
            txtZIP.Text = _currentOrganization.ZIPCode
            txtcompUrl.Text = _currentOrganization.URL
            txtcompAltPhoneTxt.Text = _currentOrganization.AltPhone
            txtorgTinNumTxt.Text = _currentOrganization.Tinno
            txtcompAltEmailTxt.Text = _currentOrganization.AltEmailAddress
            PhotoImages.Image = ConvByteToImage(_currentOrganization.Image)

            Dim currentDate = Date.Now.Date

            nightdiffshiftfrom.Value = currentDate.Add(_currentOrganization.NightDifferentialTimeFrom)
            nightdiffshiftto.Value = currentDate.Add(_currentOrganization.NightDifferentialTimeTo)

            If _policy.HasDifferentPayPeriodDates Then

                FillFirstPayPeriodData(_currentOrganization.RowID.Value)

            End If

            chkTimeLogsOnlyRequirement.Checked = _currentOrganization.PaidAsLongAsHasTimeLog

            Await OrganizationUserRolesControl.SetOrganization(_currentOrganization.RowID, isReadOnly:=False)
        End If

    End Function

    Private Sub FillFirstPayPeriodData(Optional organizationId As Integer? = Nothing)
        Dim firstHalf = _policy.DefaultFirstHalfDaysSpan(organizationId)
        Dim endOfTheMonth = _policy.DefaultEndOfTheMonthDaysSpan(organizationId)

        Dim januaryMonth As Integer = 1

        FirstHalfStartDate.Value = firstHalf.From.GetDate(januaryMonth, CurrentYear)
        FirstHalfEndDate.Value = firstHalf.To.GetDate(januaryMonth, CurrentYear)
        EndOfTheMonthStartDate.Value = endOfTheMonth.From.GetDate(januaryMonth, CurrentYear)
        EndOfTheMonthEndDate.Value = endOfTheMonth.To.GetDate(januaryMonth, CurrentYear)
    End Sub

    Private Sub SetMaxAndMinDateOfFirstPayPeriod()
        Dim januaryMonth As Integer = 1
        Dim decemberMonth As Integer = 12
        Dim daysInJanuary As Integer = Date.DaysInMonth(CurrentYear, januaryMonth)

        FirstHalfStartDate.MinDate = New Date(CurrentYear - 1, decemberMonth, 1)
        FirstHalfStartDate.MaxDate = New Date(CurrentYear, januaryMonth, daysInJanuary)

        FirstHalfEndDate.MinDate = New Date(CurrentYear - 1, decemberMonth, 1)
        FirstHalfEndDate.MaxDate = New Date(CurrentYear, januaryMonth, daysInJanuary)

        EndOfTheMonthStartDate.MinDate = New Date(CurrentYear - 1, decemberMonth, 1)
        EndOfTheMonthStartDate.MaxDate = New Date(CurrentYear, januaryMonth, daysInJanuary)

        EndOfTheMonthEndDate.MinDate = New Date(CurrentYear - 1, decemberMonth, 1)
        EndOfTheMonthEndDate.MaxDate = New Date(CurrentYear, januaryMonth, daysInJanuary)
    End Sub

    Private Async Sub NewButton_Click(sender As Object, e As EventArgs) Handles NewButton.Click

        Await ClearTextBoxes()
        SaveButton.Enabled = True
        OrganizationGridView.Enabled = False
        NewButton.Enabled = False

        _currentOrganization = Organization.NewOrganization(Z_Client)

        txtcompanyName.Focus()

        FirstPayPeriodGroupBox.Enabled = True

    End Sub

    Private Async Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click

        OrganizationGridView.Enabled = True
        NewButton.Enabled = True

        Await FillOrganizationData()
    End Sub

    Private Sub ApplyChanges()

        _currentOrganization.Name = txtcompanyName.Text
        _currentOrganization.TradeName = txttradeName.Text

        If AddressComboBox.SelectedIndex = EmptyIndex Then

            _currentOrganization.PrimaryAddressId = Nothing
        Else

            _currentOrganization.PrimaryAddressId = CType(AddressComboBox.SelectedValue, Integer)

        End If

        _currentOrganization.MainPhone = txtcompMainPhoneTxt.Text
        _currentOrganization.FaxNumber = txtcompFaxNumTxt.Text
        _currentOrganization.EmailAddress = txtcompEmailTxt.Text
        _currentOrganization.IsAgency = IsAgencyCheckBox.Checked
        _currentOrganization.RDOCode = txtRDO.Text
        _currentOrganization.ZIPCode = txtZIP.Text
        _currentOrganization.URL = txtcompUrl.Text
        _currentOrganization.AltPhone = txtcompAltPhoneTxt.Text
        _currentOrganization.Tinno = txtorgTinNumTxt.Text
        _currentOrganization.AltEmailAddress = txtcompAltEmailTxt.Text

        _currentOrganization.NightDifferentialTimeFrom = nightdiffshiftfrom.Value.TimeOfDay
        _currentOrganization.NightDifferentialTimeTo = nightdiffshiftto.Value.TimeOfDay

        _currentOrganization.PaidAsLongAsHasTimeLog = chkTimeLogsOnlyRequirement.Checked
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        PhotoImages.Focus()

        Const messageTitle = "Save Organization"

        If _currentOrganization Is Nothing Then

            MessageBoxHelper.Warning("No selected organization.", messageTitle)
            Return

        End If

        ApplyChanges()

        Dim isNew As Boolean = Not _currentOrganization.RowID.HasValue OrElse _currentOrganization.RowID.Value <= 0

        If isNew Then

            If _currentRolePermission.Create = False Then
                MessageBoxHelper.DefaultUnauthorizedActionMessage(messageTitle)
                Return
            End If
        Else

            If _currentRolePermission.Update = False Then
                MessageBoxHelper.DefaultUnauthorizedActionMessage(messageTitle)
                Return
            End If

        End If

        Dim userRoles = OrganizationUserRolesControl.GetUserRoles(allowNullUserId:=isNew)

        If Not userRoles.Any(Function(r) r.RoleId.HasValue AndAlso r.RoleId.Value > 0) Then
            MessageBoxHelper.Warning("Should have at least one user with access to this organization.", messageTitle)
            Return
        End If

        Await SaveOrganzation(isNew, userRoles, messageTitle)

    End Sub

    Private Async Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

        If _currentOrganization?.RowID Is Nothing Then
            MessageBoxHelper.Warning("No organization selected!")
            Return
        End If

        Const messageTitle As String = "Delete Organization"

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete organization: {_currentOrganization.Name}?", "Confirm Deletion") = False Then

            Return
        End If

        DeleteButton.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of OrganizationDataService)
                Await dataService.DeleteAsync(
                    id:=_currentOrganization.RowID.Value,
                    currentlyLoggedInUserId:=z_User)

                myBalloon("Successfully Deleted", "Deleted", lblSaveMsg, , -100)

                Await FillOrganizationList()

            End Function,
            dbUpdateCallBack:=
            Sub(dbu As DbUpdateException)

                For Each result In dbu.Entries
                    Console.WriteLine($"Type: {result.Entity.GetType().Name} was part of the problem. | Error Message: {dbu.Message} | Inner Exception: {dbu.InnerException?.Message}")
                Next

                MessageBoxHelper.ErrorMessage(
                    $"Organization: {_currentOrganization.Name} cannot be deleted because it already has transactions in the system. You can try renaming the organization instead.",
                    messageTitle)

            End Sub)

        DeleteButton.Enabled = True
        Me.Cursor = Cursors.Default

    End Sub

    Private Async Function SaveOrganzation(isNew As Boolean, userRoles As List(Of UserRoleIdData), messageTitle As String) As Task

        If userRoles.Any(Function(r) r.RoleId.HasValue AndAlso r.RoleId.Value > 0) Then

        End If

        SaveButton.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()

                Dim dataService = MainServiceProvider.GetRequiredService(Of ListOfValueDataService)
                Dim firstHalf = New TimePeriod(FirstHalfStartDate.Value.Date, FirstHalfEndDate.Value.Date)
                Dim endOfTheMonth = New TimePeriod(EndOfTheMonthStartDate.Value.Date, EndOfTheMonthEndDate.Value.Date)

                If isNew AndAlso _policy.HasDifferentPayPeriodDates Then
                    dataService.ValidateDefaultPayPeriodData(firstHalf, endOfTheMonth)

                End If

                Dim organizationService = MainServiceProvider.GetRequiredService(Of OrganizationDataService)
                Dim roleService = MainServiceProvider.GetRequiredService(Of RoleDataService)

                Await organizationService.SaveAsync(_currentOrganization, z_User)

                If isNew Then

                    For Each userRole In userRoles

                        userRole.OrganizationId = _currentOrganization.RowID.Value

                    Next

                    If _policy.HasDifferentPayPeriodDates Then
                        Await dataService.CreateOrUpdateDefaultPayPeriod(
                            organizationId:=_currentOrganization.RowID.Value,
                            currentlyLoggedInUserId:=z_User,
                            firstHalf:=firstHalf,
                            endOfTheMonth:=endOfTheMonth)

                        Await _policy.Refresh()
                    End If

                End If

                Await roleService.UpdateUserRolesAsync(userRoles, Z_Client)

                If _currentOrganization.RowID = z_OrganizationID Then
                    MDIPrimaryForm.Text = _currentOrganization.Name
                    orgNam = MDIPrimaryForm.Text
                End If

                OrganizationGridView.Enabled = True
                myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)

                Await FillOrganizationList()

                If NewButton.Enabled = False Then
                    NewButton.Enabled = True

                End If

            End Function)

        SaveButton.Enabled = True
        Me.Cursor = Cursors.Default

    End Function

    Private Sub txtcompFaxNumTxt_TextChanged(sender As Object, e As EventArgs) Handles txtcompFaxNumTxt.TextChanged
        TextboxTestNumeric(CType(sender, TextBox), 30, 2)
    End Sub

    Private Sub txtcompMainPhoneTxt_TextChanged(sender As Object, e As EventArgs) Handles txtcompMainPhoneTxt.TextChanged
        TextboxTestNumeric(CType(sender, TextBox), 30, 2)
    End Sub

    Private Sub txtorgTinNumTxt_TextChanged(sender As Object, e As EventArgs) Handles txtorgTinNumTxt.TextChanged
        TextboxTestNumeric(CType(sender, TextBox), 30, 2)
    End Sub

    Private Sub OrganizationForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            hintInfo.Dispose()
        Catch ex As Exception

        End Try

        myBalloon(, , lblSaveMsg, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

    Private Sub browseBtn_Click(sender As Object, e As EventArgs) Handles BrowseImageButton.Click

        If _currentOrganization Is Nothing Then

            MessageBoxHelper.Warning("No selected organization.")
            Return
        End If

        Try
            Dim fileOpener As OpenFileDialog = New OpenFileDialog()
            fileOpener.Filter = "Image files | *.jpg"
            'fileOpener.Filter = "JPEG(*.jpg)|*.jpg|JPEG(*.jpeg)|*.jpg|PNG(*.PNG)|*.png|Bitmap(*.BMP)|*.bmp"
            If fileOpener.ShowDialog() = Windows.Forms.DialogResult.OK Then

                _currentOrganization.Image = convertFileToByte(fileOpener.FileName)
                PhotoImages.Image = Image.FromFile(fileOpener.FileName)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Private Async Sub OrganizationGridView_SelectionChanged(sender As Object, e As EventArgs)

        Await FillOrganizationData()
    End Sub

    Private Function GetSelectedOrganization() As Organization

        If OrganizationGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(OrganizationGridView.CurrentRow.DataBoundItem, Organization).CloneJson()
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub

    Private Async Sub addAddressLink1_LinkClicked_1(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles addAddressLink1.LinkClicked
        Dim n_AddressClass As New AddressClass

        n_AddressClass.IsAddNew = True

        If n_AddressClass.ShowDialog("") = Windows.Forms.DialogResult.OK Then

            Dim originalAddressId = _currentOrganization.PrimaryAddressId

            Await FillAddress()

            Dim address = CType(AddressComboBox.DataSource, List(Of Address))

            If originalAddressId.HasValue AndAlso address.Any(Function(a) a.RowID.Value = originalAddressId.Value) Then

                AddressComboBox.SelectedValue = originalAddressId
            Else

                AddressComboBox.SelectedIndex = EmptyIndex
            End If

        End If

    End Sub

    Private Sub txtRDO_KeyDown(sender As Object, e As KeyEventArgs) Handles txtRDO.KeyDown
        If e.KeyCode = Keys.Up Then
            txtRDO.Text = $"{Val(txtRDO.Text) + 1}"
            txtRDO.SelectionStart = txtRDO.TextLength
        ElseIf e.KeyCode = Keys.Down Then
            txtRDO.Text = $"{Val(txtRDO.Text) - 1}"
            txtRDO.SelectionStart = txtRDO.TextLength
        ElseIf e.Control AndAlso e.KeyCode = Keys.C Then
            txtRDO.Copy()
        End If
    End Sub

    Private Sub txtRDO_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRDO.KeyPress
        e.Handled = TrapNumKey(Asc(e.KeyChar).ToString())
    End Sub

    Private Sub txtZIP_KeyDown(sender As Object, e As KeyEventArgs) Handles txtZIP.KeyDown
        If e.KeyCode = Keys.Up Then
            txtZIP.Text = $"{Val(txtZIP.Text) + 1}"
            txtZIP.SelectionStart = txtZIP.TextLength
        ElseIf e.KeyCode = Keys.Down Then
            txtZIP.Text = $"{Val(txtZIP.Text) - 1}"
            txtZIP.SelectionStart = txtZIP.TextLength
        ElseIf e.Control AndAlso e.KeyCode = Keys.C Then
            txtZIP.Copy()
        End If

    End Sub

    Private Sub txtZIP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtZIP.KeyPress
        e.Handled = TrapNumKey(Asc(e.KeyChar).ToString())
    End Sub

    Private Sub RemoveImageLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles RemoveImageLink.LinkClicked

        If _currentOrganization Is Nothing Then

            MessageBoxHelper.Warning("No selected organization.")
            Return
        End If

        _currentOrganization.Image = Nothing
        PhotoImages.Image = Nothing
    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs)

        RemoveHandler OrganizationGridView.SelectionChanged, AddressOf OrganizationGridView_SelectionChanged

        OrganizationGridView.DataSource = _organizations.
            Where(Function(o) o.Name.ToLower().Contains(SearchTextBox.Text.ToLower)).
            ToList()

        AddHandler OrganizationGridView.SelectionChanged, AddressOf OrganizationGridView_SelectionChanged

    End Sub

End Class
