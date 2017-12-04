Public Class PayrollForm

    Public listPayrollForm As New List(Of String)

    Private sys_ownr As New SystemOwner

    Private Sub ChangeForm(ByVal Formname As Form, Optional ViewName As String = Nothing)

        reloadViewPrivilege()

        Dim view_ID = ValNoComma(VIEW_privilege(ViewName, orgztnID))

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count > 0 Then

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
            If listPayrollForm.Contains(FName) Then
                'Formname.Show()
                'Formname.BringToFront()
                'Formname.Focus()
            Else
                PanelPayroll.Controls.Add(Formname)
                listPayrollForm.Add(Formname.Name)
                Formname.Refresh()
                'Formname.Location = New Point((PanelPayroll.Width / 2) - (Formname.Width / 2), (PanelPayroll.Height / 2) - (Formname.Height / 2))
                'Formname.Anchor = AnchorStyles.Top And AnchorStyles.Bottom And AnchorStyles.Right And AnchorStyles.Left
                'Formname.WindowState = FormWindowState.Maximized
                Formname.Dock = DockStyle.Fill
            End If
            Formname.Show()
            Formname.BringToFront()
            Formname.Focus()

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            Dim listOfForms = PanelPayroll.Controls.Cast(Of Form).Where(Function(i) i.Name <> Formname.Name)
            For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
                'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
                pb.Enabled = False
            Next
        End Try

    End Sub

    Sub PayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PayrollToolStripMenuItem.Click
        'ChangeForm(PayrollGenerateForm)
        ChangeForm(PayStubForm, "Employee Pay Slip")
        previousForm = PayStubForm
    End Sub

    Private Sub BonusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BonusToolStripMenuItem.Click
        ChangeForm(BonusGenerator, "Employee Pay Slip")
        previousForm = BonusGenerator

    End Sub

    Private Sub PayrollForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        For Each objctrl As Control In PanelPayroll.Controls
            If TypeOf objctrl Is Form Then
                DirectCast(objctrl, Form).Close()

            End If
        Next

    End Sub

    Private Sub PayrollForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        setProperInterfaceBaseOnCurrentSystemOwner()
    End Sub

    Sub reloadViewPrivilege()

        Dim hasPositionViewUpdate = EXECQUER("SELECT EXISTS(SELECT" & _
                                             " RowID" & _
                                             " FROM position_view" & _
                                             " WHERE OrganizationID='" & orgztnID & "'" &
                                             " AND (DATE_FORMAT(Created,@@date_format) = CURDATE()" & _
                                             " OR DATE_FORMAT(LastUpd,@@date_format) = CURDATE()));")

        If hasPositionViewUpdate = "1" Then

            position_view_table = retAsDatTbl("SELECT *" & _
                                              " FROM position_view" & _
                                              " WHERE PositionID=(SELECT PositionID FROM user WHERE RowID=" & z_User & ")" & _
                                              " AND OrganizationID='" & orgztnID & "';")

        End If

    End Sub

    Private Sub PanelPayroll_ControlRemoved(sender As Object, e As ControlEventArgs) Handles PanelPayroll.ControlRemoved
        Dim listOfForms = PanelPayroll.Controls.Cast(Of Form)()
        For Each pb As Form In listOfForms 'PanelTimeAttend.Controls.OfType(Of Form)() 'KeyPreview'Enabled
            'If Formname.Name = pb.Name Then : Continue For : Else : pb.Enabled = False : End If
            pb.Enabled = True
            Exit For
        Next
    End Sub

    Private Sub PanelPayroll_Paint(sender As Object, e As PaintEventArgs) Handles PanelPayroll.Paint

    End Sub

    Private Sub WithholdingTaxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WithholdingTaxToolStripMenuItem.Click
        ChangeForm(WithholdingTax, "Employee Pay Slip")
        previousForm = WithholdingTax
    End Sub

    Private Sub setProperInterfaceBaseOnCurrentSystemOwner()

        Dim _bool As Boolean =
            (sys_ownr.CurrentSystemOwner = SystemOwner.Cinema2000)

        If _bool Then
            BonusToolStripMenuItem.Visible = (Not _bool)

        Else

        End If

    End Sub

End Class