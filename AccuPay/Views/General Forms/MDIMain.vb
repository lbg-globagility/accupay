Imports System.Windows.Forms

Public Class MDIMain
    Public Sub ChangeDisplayForm(ByVal Formname As Form, ByVal Ima As Image)
        Try
            Application.DoEvents()
            Dim NewToolStripButton As New ToolStripButton
            Dim FName As String = Formname.Name
            Formname.Icon = Icon
            Dim Duplicate As Boolean = False
            If Not Duplicate And Not FName = LoginForm.Name Then
                NewToolStripButton.Image = Ima
                NewToolStripButton.Text = Mid(Formname.Text, 1, 5)
                NewToolStripButton.ToolTipText = Formname.Text
                NewToolStripButton.TextImageRelation = TextImageRelation.ImageAboveText
                AddHandler Formname.FormClosed, AddressOf NewToolStripButton.Dispose
                AddHandler NewToolStripButton.Click, AddressOf Formname.BringToFront

                ' AddHandler NewToolStripButton.Click, AddressOf Showmainbutton_Click

                Showmainbutton.Items.Add(NewToolStripButton)
                Formname.WindowState = FormWindowState.Maximized
                Formname.TopLevel = False
                Panel1.Controls.Add(Formname)
                Formname.Show()
            End If
            Z_Mouseleaver = True
            Formname.BringToFront()
            '    Formname.BringToFront()
            Formname.Location = New Point((Panel1.Width / 2) - (Formname.Width / 2), (Panel1.Height / 2) - (Formname.Height / 2))
        Catch ex As Exception
            Exit Sub

        End Try


    End Sub

    'Public Sub ChangeDisplayForm(ByVal Formname As Form, ByVal Ima As Image)
    '    Application.DoEvents()
    '    Dim NewToolStripButton As New ToolStripButton
    '    Dim FName As String = Formname.Name
    '    Formname.Icon = Icon  'MDImain form's icon
    '    Dim Duplicate As Boolean = False
    '    'For Each k As Form In Panel1.Controls
    '    '    If k.Name = FName Then
    '    '        Duplicate = True
    '    '        k.WindowState = FormWindowState.Normal
    '    '        k.BringToFront()
    '    '    End If
    '    'Next
    '    If Not Duplicate And Not FName = LoginForm.Name Then
    '        NewToolStripButton.Image = Ima
    '        NewToolStripButton.Text = Mid(Formname.Text, 1, 3)
    '        NewToolStripButton.ToolTipText = Formname.Text
    '        NewToolStripButton.TextImageRelation = TextImageRelation.ImageAboveText
    '        AddHandler Formname.FormClosed, AddressOf NewToolStripButton.Dispose
    '        AddHandler NewToolStripButton.Click, AddressOf Formname.BringToFront
    '        Showmainbutton.Items.Add(NewToolStripButton)
    '        Formname.WindowState = FormWindowState.Normal

    '        Formname.TopLevel = False
    '        Panel1.Controls.Add(Formname)
    '        Formname.Show()

    '        'ElseIf FName = LoginForm.Name Then
    '        '    If IsDate("1/30/2010") Then
    '        '        Formname.Location = New Point((Panel1.Width / 2) - (Formname.Width / 2), (Panel1.Height / 2) - (Formname.Height / 2))
    '        '    Else
    '        '        MsgBox("Date format should be MM/DD/YYYY." & vbNewLine & "Date should be:MM/dd/yyyy order!" & vbNewLine & "Change your date format!")
    '        '        Exit Sub
    '        '    End If
    '        '    Formname.TopLevel = False
    '        '    Panel1.Controls.Add(Formname)
    '        '    Formname.Show()
    '    End If
    '    Z_Mouseleaver = True
    '    Mainbuttons.Hide()

    '    Formname.BringToFront()
    '    Formname.Location = New Point((Panel1.Width / 2) - (Formname.Width / 2), (Panel1.Height / 2) - (Formname.Height / 2))
    'End Sub


    Private Sub EmployeeSalaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmployeeSalaryToolStripMenuItem.Click
        ChangeDisplayForm(EmpSalaryForm, EmployeeSalaryToolStripMenuItem.Image)
    End Sub

    Private Sub OrganizationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrganizationToolStripMenuItem.Click
        ChangeDisplayForm(OrganizationForm, OrganizationToolStripMenuItem.Image)
    End Sub

    Private Sub MDIMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        LoginForm.Show()



    End Sub

    Private Sub ListOfValueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListOfValueToolStripMenuItem.Click
        ChangeDisplayForm(ListOfValueForm, ListOfValueToolStripMenuItem.Image)
    End Sub

    Private Sub AuditTrailToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AuditTrailToolStripMenuItem.Click
        ChangeDisplayForm(AuditTrailForm, AuditTrailToolStripMenuItem.Image)
    End Sub

    Private Sub UserAccountToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserAccountToolStripMenuItem.Click
        ChangeDisplayForm(UsersForm, UserAccountToolStripMenuItem.Image)
    End Sub

    Private Sub EmployeePreviousEmployersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmployeePreviousEmployersToolStripMenuItem.Click
        ChangeDisplayForm(EmployeePrevEmployerForm, EmployeePreviousEmployersToolStripMenuItem.Image)
    End Sub

    Private Sub PFF053FormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PFF053FormToolStripMenuItem.Click
        ChangeDisplayForm(PFF_053Form, PFF053FormToolStripMenuItem.Image)
    End Sub
End Class
