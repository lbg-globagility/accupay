Imports AccuPay.Data.Enums
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class GeneralForm

    Public listGeneralForm As New List(Of String)

    Dim _systemOwnerService As SystemOwnerService

    Private _policyHelper As PolicyHelper

    Private _userRepository As UserRepository

    Sub New()

        InitializeComponent()

        _policyHelper = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        _userRepository = MainServiceProvider.GetRequiredService(Of UserRepository)

    End Sub

    Sub ChangeForm(ByVal Formname As Form, Optional ViewName As String = Nothing)

        reloadViewPrivilege()

        Dim view_ID = ValNoComma(VIEW_privilege(ViewName, orgztnID))

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If _policyHelper.UseUserLevel OrElse formuserprivilege.Count > 0 Then

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

        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then
            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
        End If

        If _policyHelper.PayRateCalculationBasis = PayRateCalculationBasis.Branch Then

            CalendarsToolStripMenuItem.Visible = True
            PayRateToolStripMenuItem.Visible = False
        Else

            CalendarsToolStripMenuItem.Visible = False
            PayRateToolStripMenuItem.Visible = True

        End If

        If _policyHelper.ShowBranch = False Then
            BranchToolStripMenuItem.Visible = False
        End If

        If _policyHelper.UseUserLevel = False Then
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

    End Sub

    Private Sub OrganizationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrganizationToolStripMenuItem.Click

        ChangeForm(OrganizationForm, "Organization")
        previousForm = OrganizationForm

    End Sub

    Private Sub SupplierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserPrivilegeToolStripMenuItem.Click

        'ChangeForm(UserPrivilegeForm)
        ChangeForm(userprivil, "User Privilege")

        previousForm = userprivil

    End Sub

    Private Sub PhilHealthTableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PhilHealthTableToolStripMenuItem.Click

        ChangeForm(PhiHealth, "PhilHealth Contribution Table")
        previousForm = PhiHealth

    End Sub

    Private Sub SSSTableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SSSTableToolStripMenuItem.Click

        ChangeForm(SSSCntrib, "SSS Contribution Table")
        previousForm = SSSCntrib

    End Sub

    Private Sub WithholdingTaxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WithholdingTaxToolStripMenuItem.Click

        ChangeForm(Revised_Withholding_Tax_Tables, "Withholding Tax Table")
        previousForm = Revised_Withholding_Tax_Tables
    End Sub

    Private Sub DutyShiftingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DutyShiftingToolStripMenuItem.Click

        ChangeForm(ShiftEntryForm, "Duty shifting")
        previousForm = ShiftEntryForm

    End Sub

    Private Sub PayRateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PayRateToolStripMenuItem.Click

        ChangeForm(PayRateForm, "Pay rate")
        previousForm = PayRateForm

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
            ownr.Contains(_systemOwnerService.GetCurrentSystemOwner())

        MyBase.OnLoad(e)

    End Sub

End Class