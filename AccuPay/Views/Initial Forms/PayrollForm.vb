Imports AccuPay.Data.Services
Imports Microsoft.Extensions.DependencyInjection

Public Class PayrollForm

    Public listPayrollForm As New List(Of String)

    Private if_sysowner_is_benchmark As Boolean

    Dim _systemOwnerService As SystemOwnerService

    Private _policyHelper As PolicyHelper

    Sub New(systemOwnerService As SystemOwnerService, policyHelper As PolicyHelper)

        InitializeComponent()

        _policyHelper = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)
    End Sub

    Private Sub ChangeForm(ByVal Formname As Form, Optional ViewName As String = Nothing)

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
        End Try

    End Sub

    Sub PayrollToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PayrollToolStripMenuItem.Click
        'ChangeForm(PayrollGenerateForm)

        Using MainServiceProvider

            If if_sysowner_is_benchmark Then

                Dim form = MainServiceProvider.GetRequiredService(Of BenchmarkPayrollForm)()
                ChangeForm(form, "Benchmark - Payroll Form")
                previousForm = form
            Else

                Dim form = MainServiceProvider.GetRequiredService(Of PayStubForm)()
                form.SetParentForms(Me, Me.MDIPrimaryForm)
                ChangeForm(form, "Employee Pay Slip")
                previousForm = form

            End If

        End Using

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

        If Not Debugger.IsAttached Then
            PaystubExperimentalToolStripMenuItem.Visible = False
            WithholdingTaxToolStripMenuItem.Visible = False
        End If
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

        Dim showBonusForm As Boolean =
            (_systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Goldwings)

        ' no AccuPay clients are using bonus and other features are outdated and might be buggy
        ' just like deleting Paystub should also delete it's bonuses
        showBonusForm = False

        BonusToolStripMenuItem.Visible = showBonusForm

        if_sysowner_is_benchmark = _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark

        If if_sysowner_is_benchmark Then

            BenchmarkPaystubToolStripMenuItem.Visible = True

            BonusToolStripMenuItem.Visible = False
            WithholdingTaxToolStripMenuItem.Visible = False
            PaystubExperimentalToolStripMenuItem.Visible = False
            AllowanceToolStripMenuItem.Visible = False
        Else
            BenchmarkPaystubToolStripMenuItem.Visible = False
        End If

    End Sub

    Private Sub PaystubExperimentalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PaystubExperimentalToolStripMenuItem.Click
        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of PaystubView)()

            ChangeForm(form, "Employee Pay Slip")

            previousForm = form

        End Using
    End Sub

    Private Sub AllowanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllowanceToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeAllowanceForm)()

            ChangeForm(form, "Employee Allowance")

            previousForm = form

        End Using
    End Sub

    Private Sub LoanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoanToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of EmployeeLoansForm)()

            ChangeForm(form, "Employee Loan Schedule")

            previousForm = form

        End Using
    End Sub

    Private Sub BenchmarkPaystubToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BenchmarkPaystubToolStripMenuItem.Click

        Using MainServiceProvider
            Dim form = MainServiceProvider.GetRequiredService(Of BenchmarkPaystubForm)()

            ChangeForm(form, "Benchmark - Paystub")

            previousForm = form

        End Using
    End Sub

End Class