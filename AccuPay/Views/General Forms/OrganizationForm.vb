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
Imports Microsoft.Extensions.DependencyInjection

Public Class OrganizationForm

    Private Const EmptyIndex As Integer = -1
    Private CurrentYear As Integer = Date.Now.Year

    Private _currentRolePermission As RolePermission
    Private _currentOrganization As Organization
    Private _organizations As List(Of Organization)
    Private ReadOnly _addressRepository As AddressRepository
    Private ReadOnly _organizationRepository As OrganizationRepository
    Private ReadOnly _policy As PolicyHelper

    Sub New()

        InitializeComponent()

        _addressRepository = MainServiceProvider.GetRequiredService(Of AddressRepository)
        _organizationRepository = MainServiceProvider.GetRequiredService(Of OrganizationRepository)
        _policy = MainServiceProvider.GetRequiredService(Of PolicyHelper)

    End Sub

    Private Async Sub OrganizationForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        AddressComboBox.DisplayMember = "FullAddress"
        AddressComboBox.ValueMember = "RowID"
        Await FillAddress()

        OrganizationGridView.AutoGenerateColumns = False
        Await FillOrganizationList()

        FillOrganizationData()

        FirstPayPeriodGroupBox.Visible = _policy.HasDifferentPayPeriodDates
        SetMaxAndMinDateOfFirstPayPeriod()

        Await RestrictByRole()

        AddHandler OrganizationGridView.SelectionChanged, AddressOf OrganizationGridView_SelectionChanged
        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

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

    Private Sub ClearTextBoxes()

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

    End Sub

    Async Function FillAddress() As Task
        AddressComboBox.DataSource = (Await _addressRepository.GetAllAsync()).
            OrderBy(Function(a) a.FullAddress).
            ToList()
    End Function

    Private Async Function FillOrganizationList() As Task
        Dim list = Await _organizationRepository.List(OrganizationPageOptions.AllData, Z_Client)

        _organizations = list.organizations.
            OrderBy(Function(o) o.Name).
            ToList()

        OrganizationGridView.DataSource = _organizations
    End Function

    Private Sub FillOrganizationData()

        ClearTextBoxes()

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

            chkTimeLogsOnlyRequirement.Checked = _currentOrganization.IsTimeLogsOnlyAttendanceRequirement
        End If

    End Sub

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

    Private Sub NewButton_Click(sender As Object, e As EventArgs) Handles NewButton.Click
        ClearTextBoxes()
        SaveButton.Enabled = True
        OrganizationGridView.Enabled = False
        NewButton.Enabled = False

        _currentOrganization = Organization.NewOrganization(z_User, Z_Client)

        txtcompanyName.Focus()

        FirstPayPeriodGroupBox.Enabled = True

    End Sub

    Private Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click

        OrganizationGridView.Enabled = True
        NewButton.Enabled = True

        FillOrganizationData()
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

        _currentOrganization.IsTimeLogsOnlyAttendanceRequirement = chkTimeLogsOnlyRequirement.Checked
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        PhotoImages.Focus()

        If _currentOrganization Is Nothing Then

            MessageBoxHelper.Warning("No selected organization.")
            Return

        End If

        ApplyChanges()

        Dim isNew As Boolean = Not _currentOrganization.RowID.HasValue OrElse _currentOrganization.RowID.Value <= 0

        If isNew Then

            If _currentRolePermission.Create = False Then
                MessageBoxHelper.DefaultUnauthorizedActionMessage()
                Return
            End If
        Else

            If _currentRolePermission.Update = False Then
                MessageBoxHelper.DefaultUnauthorizedActionMessage()
                Return
            End If

        End If

        Await SaveOrganzation(isNew)

    End Sub

    Private Async Function SaveOrganzation(isNew As Boolean) As Task
        Await FunctionUtils.TryCatchFunctionAsync("Save Organization",
            Async Function()

                Dim dataService = MainServiceProvider.GetRequiredService(Of ListOfValueDataService)
                Dim firstHalf = New TimePeriod(FirstHalfStartDate.Value.Date, FirstHalfEndDate.Value.Date)
                Dim endOfTheMonth = New TimePeriod(EndOfTheMonthStartDate.Value.Date, EndOfTheMonthEndDate.Value.Date)

                If isNew AndAlso _policy.HasDifferentPayPeriodDates Then
                    dataService.ValidateDefaultPayPeriodData(firstHalf, endOfTheMonth)
                End If

                Dim organizationDataService = MainServiceProvider.GetRequiredService(Of OrganizationDataService)
                Await organizationDataService.SaveAsync(_currentOrganization)

                If isNew AndAlso _policy.HasDifferentPayPeriodDates Then
                    Await dataService.CreateOrUpdateDefaultPayPeriod(
                        organizationId:=_currentOrganization.RowID.Value,
                        currentlyLoggedInUserId:=z_User,
                        firstHalf:=firstHalf,
                        endOfTheMonth:=endOfTheMonth)

                    Await _policy.Refresh()
                End If

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

    Private Sub OrganizationGridView_SelectionChanged(sender As Object, e As EventArgs)

        FillOrganizationData()
    End Sub

    Private Function GetSelectedOrganization() As Organization

        If OrganizationGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(OrganizationGridView.CurrentRow.DataBoundItem, Organization).CloneJson()
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub addAddressLink1_LinkClicked_1(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles addAddressLink1.LinkClicked
        'address.ShowDialog()

        Dim n_AddressClass As New AddressClass

        n_AddressClass.IsAddNew = True

        If n_AddressClass.ShowDialog("") = Windows.Forms.DialogResult.OK Then

            Dim full_address = String.Empty

            With n_AddressClass

                If .StreetAddress1 = Nothing Then
                    full_address = Nothing
                Else
                    full_address = .StreetAddress1 & ","
                End If

                If .StreetAddress2 <> Nothing Then
                    full_address &= .StreetAddress2 & ","
                End If

                If .Barangay <> Nothing Then
                    full_address &= .Barangay & ","
                End If

                If .City <> Nothing Then
                    full_address &= .City & ","
                End If

                If .State <> Nothing Then
                    full_address &= "," & .State & ","
                End If

                If .Country <> Nothing Then
                    full_address &= .Country & ","
                End If

                If .ZipCode <> Nothing Then
                    full_address &= .ZipCode
                End If

            End With

            Dim addressstringlength = full_address.Length

            Dim LastCharIsComma = String.Empty

            Try
                LastCharIsComma =
                full_address.Substring((addressstringlength - 1), 1)
            Catch ex As Exception
                LastCharIsComma = String.Empty
            End Try

            If LastCharIsComma.Trim = "," Then
                full_address = full_address.Substring(0, (addressstringlength - 1))

            End If

            full_address = full_address.Replace(",,", ",")

            If AddressComboBox.Items.Contains(full_address) = False Then
                AddressComboBox.Items.Add(full_address)

            End If

            AddressComboBox.Text = full_address

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