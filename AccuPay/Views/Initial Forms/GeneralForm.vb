Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utils

Public Class GeneralForm

    Public listGeneralForm As New List(Of String)

    Dim sys_ownr As New SystemOwnerService()

    Private _payRateCalculationBasis As PayRateCalculationBasis

    Private _userRepository As UserRepository

    Sub ChangeForm(ByVal Formname As Form, Optional ViewName As String = Nothing)

        reloadViewPrivilege()

        Dim view_ID = ValNoComma(VIEW_privilege(ViewName, orgztnID))

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If PayrollTools.CheckIfUsingUserLevel() = True OrElse formuserprivilege.Count > 0 Then

            For Each drow In formuserprivilege
                'If drow("ReadOnly").ToString = "Y" Then
                If drow("AllowedToAccess").ToString = "N" Then

                    'ChangeForm(Formname)
                    'previousForm = Formname

                    'Exit For
                    Exit Sub
                Else
                    If drow("Creates").ToString = "Y" _
                        Or drow("Updates").ToString = "Y" _
                        Or drow("Deleting").ToString = "Y" _
                        Or drow("ReadOnly").ToString = "Y" Then
                        'And drow("Updates").ToString = "Y" Then

                        'ChangeForm(Formname)
                        'previousForm = Formname
                        Exit For
                    Else
                        Exit Sub
                    End If

                End If

            Next
        Else
            Exit Sub
        End If

        Try
            Application.DoEvents()
            Dim FName As String = Formname.Name
            Formname.KeyPreview = True
            Formname.TopLevel = False
            Formname.Enabled = True
            If listGeneralForm.Contains(FName) Then
                Formname.Show()
                Formname.BringToFront()
                Formname.Focus()
            Else
                PanelGeneral.Controls.Add(Formname)
                listGeneralForm.Add(Formname.Name)

                Formname.Show()
                Formname.BringToFront()
                Formname.Focus()
                'Formname.Location = New Point((PanelGeneral.Width / 2) - (Formname.Width / 2), (PanelGeneral.Height / 2) - (Formname.Height / 2))
                'Formname.Anchor = AnchorStyles.Top And AnchorStyles.Bottom And AnchorStyles.Right And AnchorStyles.Left
                'Formname.WindowState = FormWindowState.Maximized
                Formname.Dock = DockStyle.Fill
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            Dim listOfForms = PanelGeneral.Controls.Cast(Of Form).Where(Function(i) i.Name <> Formname.Name)
            For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
                'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
                pb.Enabled = False
            Next
        End Try

    End Sub

    Private Sub GeneralForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        For Each objctrl As Control In PanelGeneral.Controls
            If TypeOf objctrl Is Form Then
                DirectCast(objctrl, Form).Close()

            End If
        Next

    End Sub

    Private Async Sub GeneralForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _userRepository = New UserRepository()

        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then
            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
        End If

        Dim settings = ListOfValueCollection.Create()

        If settings.GetEnum("Pay rate.CalculationBasis",
                   PayRateCalculationBasis.Organization) = PayRateCalculationBasis.Branch Then

            CalendarsToolStripMenuItem.Visible = True
            PayRateToolStripMenuItem.Visible = False
        Else

            CalendarsToolStripMenuItem.Visible = False
            PayRateToolStripMenuItem.Visible = True

        End If

        If settings.GetBoolean("Employee Policy.ShowBranch", False) = False Then
            BranchToolStripMenuItem.Visible = False
        End If

        If settings.GetBoolean("User Policy.UseUserLevel", False) = False Then
            Return
        Else
            UserPrivilegeToolStripMenuItem.Visible = False
        End If

        If user.UserLevel = UserLevel.Two OrElse user.UserLevel = UserLevel.Three Then

            UserToolStripMenuItem.Visible = False
            OrganizationToolStripMenuItem.Visible = False
            ListOfValueToolStripMenuItem.Visible = False

        End If

    End Sub

    Private Sub UserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserToolStripMenuItem.Click

        ChangeForm(UsersForm, "Users")
        previousForm = UsersForm

        'If FormLeft.Contains("Users") Then
        '    FormLeft.Remove("Users")

        '    FormLeft.Add("Users")
        'Else
        '    FormLeft.Add("Users")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.text = "Welcome"
        'Else
        '    MDIPrimaryForm.text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

    End Sub

    Private Sub ListOfValueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListOfValueToolStripMenuItem.Click

        ChangeForm(ListOfValueForm, "List of value")
        previousForm = ListOfValueForm

        'If FormLeft.Contains("List of value") Then
        '    FormLeft.Remove("List of value")

        '    FormLeft.Add("List of value")
        'Else
        '    FormLeft.Add("List of value")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.text = "Welcome"
        'Else
        '    MDIPrimaryForm.text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

    End Sub

    Private Sub OrganizationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrganizationToolStripMenuItem.Click

        ChangeForm(OrganizationForm, "Organization")
        previousForm = OrganizationForm

        'If FormLeft.Contains("Organization") Then
        '    FormLeft.Remove("Organization")

        '    FormLeft.Add("Organization")
        'Else
        '    FormLeft.Add("Organization")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.text = "Welcome"
        'Else
        '    MDIPrimaryForm.text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

    End Sub

    Private Sub SupplierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserPrivilegeToolStripMenuItem.Click

        'ChangeForm(UserPrivilegeForm)
        ChangeForm(userprivil, "User Privilege")

        previousForm = userprivil

        'If FormLeft.Contains("User Privilege") Then
        '    FormLeft.Remove("User Privilege")

        '    FormLeft.Add("User Privilege")
        'Else
        '    FormLeft.Add("User Privilege")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.text = "Welcome"
        'Else
        '    MDIPrimaryForm.text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

    End Sub

    Private Sub PhilHealthTableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PhilHealthTableToolStripMenuItem.Click

        ChangeForm(PhiHealth, "PhilHealth Contribution Table")
        previousForm = PhiHealth

        'If FormLeft.Contains("PhilHealth Contribution Table") Then
        '    FormLeft.Remove("PhilHealth Contribution Table")

        '    FormLeft.Add("PhilHealth Contribution Table")
        'Else
        '    FormLeft.Add("PhilHealth Contribution Table")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.text = "Welcome"
        'Else
        '    MDIPrimaryForm.text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

    End Sub

    Private Sub SSSTableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SSSTableToolStripMenuItem.Click

        ChangeForm(SSSCntrib, "SSS Contribution Table")
        previousForm = SSSCntrib

        'If FormLeft.Contains("SSS Contribution Table") Then
        '    FormLeft.Remove("SSS Contribution Table")

        '    FormLeft.Add("SSS Contribution Table")
        'Else
        '    FormLeft.Add("SSS Contribution Table")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.text = "Welcome"
        'Else
        '    MDIPrimaryForm.text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

    End Sub

    Private Sub WithholdingTaxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WithholdingTaxToolStripMenuItem.Click

        ChangeForm(Revised_Withholding_Tax_Tables, "Withholding Tax Table")
        previousForm = Revised_Withholding_Tax_Tables

        'If FormLeft.Contains("Withholding tax table") Then
        '    FormLeft.Remove("Withholding tax table")

        '    FormLeft.Add("Withholding tax table")
        'Else
        '    FormLeft.Add("Withholding tax table")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.text = "Welcome"
        'Else
        '    MDIPrimaryForm.text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

    End Sub

    Private Sub DutyShiftingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DutyShiftingToolStripMenuItem.Click

        ChangeForm(ShiftEntryForm, "Duty shifting")
        previousForm = ShiftEntryForm

        'If FormLeft.Contains("Duty shifting") Then
        '    FormLeft.Remove("Duty shifting")

        '    FormLeft.Add("Duty shifting")
        'Else
        '    FormLeft.Add("Duty shifting")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.text = "Welcome"
        'Else
        '    MDIPrimaryForm.text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

    End Sub

    Private Sub PayRateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PayRateToolStripMenuItem.Click

        ChangeForm(PayRateForm, "Pay rate")
        previousForm = PayRateForm

        'If FormLeft.Contains("Pay rate") Then
        '    FormLeft.Remove("Pay rate")

        '    FormLeft.Add("Pay rate")
        'Else
        '    FormLeft.Add("Pay rate")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.text = "Welcome"
        'Else
        '    MDIPrimaryForm.text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

    End Sub

    Private Sub CalendarsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CalendarsToolStripMenuItem.Click
        ChangeForm(CalendarsForm, "Calendars")
        previousForm = CalendarsForm
    End Sub

    Sub reloadViewPrivilege()

        Dim hasPositionViewUpdate = EXECQUER("SELECT EXISTS(SELECT" &
                                             " RowID" &
                                             " FROM position_view" &
                                             " WHERE OrganizationID='" & orgztnID & "'" &
                                             " AND (DATE_FORMAT(Created,@@date_format) = CURDATE()" &
                                             " OR DATE_FORMAT(LastUpd,@@date_format) = CURDATE()));")

        If hasPositionViewUpdate = "1" Then

            position_view_table = retAsDatTbl("SELECT *" &
                                              " FROM position_view" &
                                              " WHERE PositionID=(SELECT PositionID FROM user WHERE RowID=" & z_User & ")" &
                                              " AND OrganizationID='" & orgztnID & "';")

        End If

    End Sub

    Private Sub AgencyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AgencyToolStripMenuItem.Click

        Dim n_UserAccessRights As New UserAccessRights(AgencyForm.ViewIdentification)

        'If n_UserAccessRights.ResultValue(AccessRightName.HasReadOnly) Then
        '    'Agency
        ChangeForm(AgencyForm, "Agency")
        previousForm = AgencyForm

        'End If

    End Sub

    Private Sub BranchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BranchToolStripMenuItem.Click

        Dim form As New AddBranchForm
        form.ShowDialog()

    End Sub

    Private Sub PanelGeneral_ControlRemoved(sender As Object, e As ControlEventArgs) Handles PanelGeneral.ControlRemoved
        Dim listOfForms = PanelGeneral.Controls.Cast(Of Form)()
        For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
            'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
            pb.Enabled = True
            Exit For
        Next
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim ownr() As String =
            Split(AgencyToolStripMenuItem.AccessibleDescription, ";")

        AgencyToolStripMenuItem.Visible =
            ownr.Contains(sys_ownr.GetCurrentSystemOwner())

        MyBase.OnLoad(e)

    End Sub

End Class