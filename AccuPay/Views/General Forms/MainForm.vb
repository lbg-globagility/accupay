Imports MySql.Data.MySqlClient

Public Class MainForm
    Public Sub ChangeForm(ByVal Formname As Form)
        Try
            Application.DoEvents()
            Dim FName As String = Formname.Name
            Formname.WindowState = FormWindowState.Maximized
            Formname.TopLevel = False
            Panel1.Controls.Add(Formname)
            Formname.Show()
            Formname.BringToFront()
            Formname.Location = New Point((Panel1.Width / 2) - (Formname.Width / 2), (Panel1.Height / 2) - (Formname.Height / 2))
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Public Sub loadform(ByVal frmname As Form)
        Try
            Panel1.AutoScroll = True

            'AddHandler frmname.LocationChanged, AddressOf Me.Form2_LocationChanged
            'AddHandler frmname.SizeChanged, AddressOf Me.Form2_SizeChanged
            frmname.TopLevel = False
            Panel1.Controls.Add(frmname)
            If frmname.Size.Width > Panel1.Size.Width Or frmname.Size.Height > Panel1.Size.Height Then
                Panel1.SetAutoScrollMargin(frmname.Size.Width - Panel1.Size.Width, frmname.Size.Height - Panel1.Size.Height)
            End If
            frmname.Show()
            frmname.BringToFront()
        Catch ex As Exception

        End Try
    End Sub




    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        LoginForm.UsernameTextBox.Clear()
        LoginForm.PasswordTextBox.Clear()

        LoginForm.Show()
    End Sub



    Private Sub btnEmpSalary_Click(sender As Object, e As EventArgs) Handles hbtnEmpSalary.Click
        EmpSalaryForm.lbllinkbasicpay.Text = ""
        ChangeForm(EmpSalaryForm)
    End Sub


    Private Sub btnPrevEmployer_Click(sender As Object, e As EventArgs) Handles hbtnPrevEmployer.Click
        ChangeForm(EmployeePrevEmployerForm)
    End Sub



    Private Sub btnUsersform_Click(sender As Object, e As EventArgs) Handles btnUsersform.Click
        'Dim f As UsersForm = New UsersForm()
        'f.MdiParent = Me
        'f.Show()
        ChangeForm(UsersForm)
    End Sub

    Private Sub btnListOfval_Click(sender As Object, e As EventArgs) Handles btnListOfval.Click
        ChangeForm(ListOfValueForm)
    End Sub

    Private Sub btnOrganization_Click(sender As Object, e As EventArgs) Handles btnOrganization.Click
        ChangeForm(OrganizationForm)
    End Sub

    Private Sub btnEducationbGround_Click(sender As Object, e As EventArgs) Handles hbtnEducationbGround.Click
        ChangeForm(EducationalBackgroundForm)
    End Sub

    Private Sub btnUserPrevilige_Click(sender As Object, e As EventArgs) Handles btnUserPrevilige.Click
        ChangeForm(UserPrivilegeForm)
    End Sub

    Private Sub hbtnUsers_Click(sender As Object, e As EventArgs) Handles hbtnUsers.Click
        ChangeForm(UsersForm)
    End Sub

    Private Sub hbtnListofval_Click(sender As Object, e As EventArgs) Handles hbtnListofval.Click
        ChangeForm(ListOfValueForm)
    End Sub

    Private Sub hbtnorganization_Click(sender As Object, e As EventArgs) Handles hbtnorganization.Click
        ChangeForm(OrganizationForm)
    End Sub

    Private Sub hbtnexit_Click(sender As Object, e As EventArgs) Handles hbtnexit.Click
        LoginForm.Show()
        Me.Close()

    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load


        Dim position As String = getStringItem("Select p.PositionName From User u inner join Position p on u.PositionID = p.RowID " & _
                                             "Where u.RowID = '" & z_User & "' And u.OrganizationID = " & z_OrganizationID & "")
        Dim getposition As String = position

        lblUser.Text = LoginForm.UsernameTextBox.Text
        lblPosition.Text = getposition
        lblTime.Text = TimeOfDay
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblTime.Text = TimeOfDay
    End Sub

    Private Sub btnDivision_Click(sender As Object, e As EventArgs) Handles hbtnDivision.Click
        ChangeForm(DivisionForm)
    End Sub

    Private Sub btntax_Click(sender As Object, e As EventArgs) Handles btntax.Click
        ChangeForm(Revised_Withholding_Tax_Tables)
    End Sub

    Private Sub btnLoadSched_Click(sender As Object, e As EventArgs) Handles btnLoadSched.Click
        ChangeForm(LoanScheduleForm)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ChangeForm(EmployeeLoanHistoryForm)
    End Sub

    Private Sub btnbackup_Click(sender As Object, e As EventArgs) Handles btnbackup.Click
        Try

            Dim ConString As String = System.IO.File.ReadAllText("C:\PayrollSystem\ConnectionStringPayroll.txt")
            Dim BackupFile As String
            Dim fileSaver As SaveFileDialog = New SaveFileDialog()
            fileSaver.Filter = "SQL files | *.sql"

            If fileSaver.ShowDialog() = Windows.Forms.DialogResult.OK Then
                BackupFile = fileSaver.FileName

                Using sConnection As New MySqlConnection(ConString)
                    Using sqlCommand As New MySqlCommand()
                        Using sqlBackup As New MySqlBackup(sqlCommand)
                            sqlCommand.Connection = sConnection
                            sConnection.Open()
                            sqlBackup.ExportToFile(BackupFile)
                            MessageBox.Show("MySQL database backup has been created.", "MySQL Backup", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            sConnection.Close()
                        End Using
                    End Using
                End Using
            Else
                MessageBox.Show("No backup file was created.", "MySQL Restore", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If


        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            connection.Close()

        End Try
    End Sub

    Private Sub btnShiftEntry_Click(sender As Object, e As EventArgs) Handles btnShiftEntry.Click
        ChangeForm(EmployeeShiftEntryForm)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ChangeForm(EmployeePromotionForm)
    End Sub

    Private Sub btnDisciplinaryaction_Click(sender As Object, e As EventArgs) Handles btnDisciplinaryaction.Click
        ChangeForm(EmpDisciplinaryActionForm)
    End Sub

    Private Sub btnTimeENtry_Click(sender As Object, e As EventArgs) Handles btnTimeENtry.Click
        ChangeForm(AttendanceTimeEntryForm)
    End Sub
End Class