Imports System.ComponentModel
Imports System.Configuration
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Repositories
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports Indigo
Imports Microsoft.Extensions.DependencyInjection
Imports MySql.Data.MySqlClient

Public Class MDIPrimaryForm

    Dim DefaultFontStyle = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))

    Dim ExemptedForms As New List(Of String)

    Dim ClosingForm As Form = Nothing 'New

    Private versionNo As String

    Private str_pending_leave As String =
        String.Concat("SELECT e.EmployeeID",
                      ", CONCAT_WS(', ', e.LastName, e.FirstName) `FullName`",
                      ", CONCAT_WS(' to ', CONCAT(TIME_FORMAT(elv.LeaveStartTime, '%l:%i'), LEFT(TIME_FORMAT(elv.LeaveEndTime, '%p'), 1))",
                      "                  , CONCAT(TIME_FORMAT(elv.LeaveEndTime, '%l:%i'), LEFT(TIME_FORMAT(elv.LeaveEndTime, '%p'), 1))) `LeaveTime`",
                      ", DATE_FORMAT(elv.LeaveStartDate, '%c/%e/%Y') `LeaveStartDate`",
                      ", (DATEDIFF(elv.LeaveEndDate, elv.LeaveStartDate) + 1) `LeaveDays`",
                      " FROM employeeleave elv",
                      " INNER JOIN employee e ON e.RowID=elv.EmployeeID",
                      " WHERE elv.OrganizationID=?og_rowid",
                      " And elv.`Status`=?lv_status",
                      " ORDER BY elv.Created DESC;")

    Private if_sysowner_is_benchmark As Boolean
    Private if_sysowner_is_cinema2k As Boolean
    Private if_sysowner_is_hyundai As Boolean

    Private ReadOnly _policyHelper As IPolicyHelper

    Private ReadOnly _systemOwnerService As SystemOwnerService

    Private ReadOnly _userRepository As IAspNetUserRepository

    Private ReadOnly _paystubEmailRepository As IPaystubEmailRepository

    Sub New()

        InitializeComponent()

        _policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        _userRepository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)

        _paystubEmailRepository = MainServiceProvider.GetRequiredService(Of IPaystubEmailRepository)

        Dim currentSystemOwner = _systemOwnerService.GetCurrentSystemOwner()
        if_sysowner_is_benchmark = currentSystemOwner = SystemOwnerService.Benchmark
        if_sysowner_is_cinema2k = currentSystemOwner = SystemOwnerService.Cinema2000
        if_sysowner_is_hyundai = currentSystemOwner = SystemOwnerService.Hyundai

        PrepareFormForBenchmark()
    End Sub

    Protected Overrides Async Sub OnLoad(e As EventArgs)
        With ExemptedForms
            .Add("MDIPrimaryForm")
            .Add("MetroLogin")
            .Add("FormReports")
            .Add("GeneralForm")
            .Add("HomeForm")
            .Add("HRISForm")
            .Add("PayrollForm")
            .Add("TimeAttendForm")
        End With

        SplitContainer1.SplitterWidth = 6

        SplitContainer2.SplitterWidth = 6

        Panel2.Font = DefaultFontStyle

        Panel3.Font = DefaultFontStyle

        Panel12.Font = DefaultFontStyle

        Panel11.Font = DefaultFontStyle

        Panel13.Font = DefaultFontStyle

        Panel14.Font = DefaultFontStyle

        Panel6.Font = DefaultFontStyle

        Panel5.Font = DefaultFontStyle

        Panel7.Font = DefaultFontStyle

        CollapsibleGroupBox6.Visible = if_sysowner_is_hyundai
        Panel15.Font = DefaultFontStyle

        setProperDashBoardAccordingToSystemOwner()

        Await RunLeaveAccrual()

        Panel1.Focus()
        MyBase.OnLoad(e)
        Await RestrictDashboardByPermission()
        MetroLogin.Hide()
    End Sub

    Public listofGroup As New AutoCompleteStringCollection

    Public Sub ChangeForm(ByVal Formname As Form)
        Try
            Application.DoEvents()
            Dim FName As String = Formname.Name
            Formname.TopLevel = False

            If listofGroup.Contains(FName) Then
                Formname.Show()
                Formname.BringToFront()
            Else
                Me.Panel1.Controls.Add(Formname)
                listofGroup.Add(Formname.Name)

                Formname.Show()
                Formname.BringToFront()
                Formname.Dock = DockStyle.Fill

            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblTime.Text = TimeOfDay
    End Sub

    Private Async Function RunLeaveAccrual() As Task

        Dim listOfValueService = MainServiceProvider.GetRequiredService(Of ListOfValueService)
        Dim collection = Await listOfValueService.CreateAsync("LeavePolicy")

        If collection.GetBoolean("LeavePolicy.AutomaticAccrual") Then
            Dim unused = Task.Run(
                Async Function()

                    Dim service = MainServiceProvider.GetRequiredService(Of LeaveAccrualService)
                    Await service.CheckAccruals(z_OrganizationID, z_User)
                End Function)
        End If
    End Function

    Private Async Sub MDIPrimaryForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        LockTime()
        EmailStatusTimer.Stop()

        If e.Cancel = False Then

            Dim prompt = MessageBox.Show("Do you want to log out ?", "Confirming log out", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

            If prompt = MsgBoxResult.Yes Then

                USER_ROLE = Nothing

                e.Cancel = False

                ''Close all forms that remains open

                Dim listofExtraFrm As Form()

                Dim listofExtraForm As New List(Of String)

                listofExtraForm.Add("CrysVwr")
                listofExtraForm.Add("leavtyp")
                listofExtraForm.Add("LoanType")
                listofExtraForm.Add("newEmpStat")
                listofExtraForm.Add("newEmpType")
                listofExtraForm.Add("newPostion")
                listofExtraForm.Add("newProdAllowa")
                listofExtraForm.Add("newProdBonus")
                listofExtraForm.Add("SelectFromEmployee")
                listofExtraForm.Add("selectPayPeriod")
                listofExtraForm.Add("viewtotallow")
                listofExtraForm.Add("viewtotbon")
                listofExtraForm.Add("viewtotloan")
                listofExtraForm.Add("FindingForm")

                listofExtraForm.Add("AddListOfValueForm")
                listofExtraForm.Add("AddPostionForm")

                listofExtraForm.Add("GeneralForm")
                listofExtraForm.Add("HRISForm")
                listofExtraForm.Add("PayrollForm")
                listofExtraForm.Add("TimeAttendForm")

                ReDim listofExtraFrm(My.Application.OpenForms.Count - 1)

                Dim itemindex = 0

                Dim open_forms = My.Application.OpenForms

                For Each f As Form In open_forms

                    Dim frmName = f.Name

                    If ExemptedForms.Contains(frmName) Then
                        Continue For
                    Else

                        If listofExtraForm.Contains(frmName) Then
                            Continue For
                        Else
                            If frmName.Trim.Length > 0 Then
                                listofExtraFrm(itemindex) = f
                                itemindex += 1

                            End If

                        End If

                    End If
                Next

                Dim openform_count = listofExtraFrm.GetUpperBound(0)

                For ii = 0 To openform_count

                    If listofExtraFrm(ii) Is Nothing Then
                        Continue For
                    Else

                        ClosingForm = listofExtraFrm(ii)

                        ClosingForm.Close()

                    End If

                Next

                If openform_count >= 5 Then
                    Thread.Sleep(1175)
                End If

                With MetroLogin

                    .Show()

                    .UserNameTextBox.Clear()

                    .PasswordTextBox.Clear()

                    .UserNameTextBox.Focus()

                    .PhotoImages.Image = Nothing

                    .OrganizationComboBox.SelectedIndex = -1

                    Await .CheckAppVersion()

                    Await .ReloadOrganizationAsync()

                    If Debugger.IsAttached Then
                        .AssignDefaultCredentials()
                    End If
                End With

            ElseIf prompt = MsgBoxResult.No Then
                e.Cancel = True
            Else
                e.Cancel = True
            End If

            Dim e_CloseReason = New String() {1, 2, 3, 4, 5, 6}
        End If
    End Sub

    Private Sub MDIPrimaryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            PrepareForm()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
        End Try
    End Sub

    Private Sub PrepareForm()
        If dbnow = Nothing Then
            dbnow = EXECQUER(CURDATE_MDY)
        End If
        TimeAndAttendanceToolStripButton.Text = "Time &&" & vbNewLine & "Attendance"
        TimeAndAttendanceToolStripButton.ToolTipText = "Time & Attendance"
        '123, 24
        lblTime.Text = TimeOfDay
        lblUser.Text = userFirstName &
                       If(userLastName = Nothing, "", " " & userLastName)
        lblPosition.Text = z_postName
        SelectHomeToolStrip()
        PictureBox1.Image = ImageList1.Images(1)
        LoadVersionNo()

        If _policyHelper.UseEmailPayslip Then

            EmailServiceStatusToolStripLabel.Visible = True
            AddHandler EmailStatusTimer.Tick, AddressOf EmailStatusTimer_Tick

            EmailStatusTimer.Start()
        End If

    End Sub

    Private Sub PrepareFormForBenchmark()
        If if_sysowner_is_benchmark Then
            HomeToolStripButton.Visible = False
            TimeAndAttendanceToolStripButton.Visible = False
        End If
    End Sub

    Private Async Function RestrictByUserLevel() As Task

        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
        End If

        If Not _policyHelper.UseUserLevel Then Return

        If user.UserLevel = UserLevel.Four OrElse user.UserLevel = UserLevel.Five Then

            GeneralToolStripButton.Visible = False
            PayrollToolStripButton.Visible = False
            ReportsToolStripButton.Visible = False

            LoanBalanceCollapsibleGroupBox.Visible = False
            NegativePayslipsCollapsibleGroupBox.Visible = False
            PendingOfficialBusinessCollapsibleGroupBox.Visible = False

            If user.UserLevel = UserLevel.Five Then

                TimeAndAttendanceToolStripButton.Visible = False

            End If

        End If

    End Function

    Private Sub LoadVersionNo()
        Dim appSettings = ConfigurationManager.AppSettings
        Me.versionNo = appSettings.Get("payroll.version")

        If versionNo IsNot Nothing Then
            lblVersionValue.Text = Me.versionNo
        Else
            lblVersionValue.Text = "Version no is missing."
        End If
    End Sub

    'Trebuchet MS
    'Segoe UI

    Dim selectedButtonFont = New Font("Trebuchet MS", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType(0, Byte))

    Dim unselectedButtonFont = New Font("Trebuchet MS", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))

    Dim isHome As SByte = 0

    Private Sub HomeToolStripButton_Click(sender As Object, e As EventArgs) Handles HomeToolStripButton.Click
        SelectHomeToolStrip()

    End Sub

    Private Sub SelectHomeToolStrip()
        isHome = 1

        UnlockTime()

        GeneralForm.Hide()
        HRISForm.Hide()
        PayrollForm.Hide()
        BenchmarkPayrollForm.Hide()
        TimeAttendForm.Hide()

        FormReports.Hide()

        HomeToolStripButton.BackColor = Color.FromArgb(255, 255, 255)

        GeneralToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        HrisToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        TimeAndAttendanceToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        PayrollToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        ReportsToolStripButton.BackColor = Color.FromArgb(194, 228, 255)

        HomeToolStripButton.Font = selectedButtonFont

        GeneralToolStripButton.Font = unselectedButtonFont
        HrisToolStripButton.Font = unselectedButtonFont
        TimeAndAttendanceToolStripButton.Font = unselectedButtonFont
        PayrollToolStripButton.Font = unselectedButtonFont
        ReportsToolStripButton.Font = unselectedButtonFont

        Static once As SByte = 0
        If once = 0 Then
            once = 1
            Me.Text = orgNam
        End If

    End Sub

    Private Async Sub GeneralToolStripButton_Click(sender As Object, e As EventArgs) Handles GeneralToolStripButton.Click

        isHome = 0

        LockTime()

        ChangeForm(GeneralForm)

        HRISForm.Hide()
        PayrollForm.Hide()
        TimeAttendForm.Hide()

        FormReports.Hide()

        HomeToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        HrisToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        TimeAndAttendanceToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        PayrollToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        ReportsToolStripButton.BackColor = Color.FromArgb(194, 228, 255)

        GeneralToolStripButton.BackColor = Color.FromArgb(255, 255, 255)

        GeneralToolStripButton.Font = selectedButtonFont

        HomeToolStripButton.Font = unselectedButtonFont
        HrisToolStripButton.Font = unselectedButtonFont
        TimeAndAttendanceToolStripButton.Font = unselectedButtonFont
        PayrollToolStripButton.Font = unselectedButtonFont
        ReportsToolStripButton.Font = unselectedButtonFont

        Await refresh_previousForm(0)
    End Sub

    Private Async Sub TimeAndAttendanceToolStripButton_Click(sender As Object, e As EventArgs) Handles TimeAndAttendanceToolStripButton.Click

        isHome = 0

        LockTime()

        ChangeForm(TimeAttendForm)

        GeneralForm.Hide()
        HRISForm.Hide()
        PayrollForm.Hide()

        FormReports.Hide()

        HomeToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        GeneralToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        HrisToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        PayrollToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        ReportsToolStripButton.BackColor = Color.FromArgb(194, 228, 255)

        TimeAndAttendanceToolStripButton.BackColor = Color.FromArgb(255, 255, 255)

        TimeAndAttendanceToolStripButton.Font = selectedButtonFont

        HomeToolStripButton.Font = unselectedButtonFont
        GeneralToolStripButton.Font = unselectedButtonFont
        HrisToolStripButton.Font = unselectedButtonFont
        PayrollToolStripButton.Font = unselectedButtonFont
        ReportsToolStripButton.Font = unselectedButtonFont

        Await refresh_previousForm(2)
    End Sub

    Dim theemployeetable As New DataTable

    Private Async Function refresh_previousForm(Optional groupindex As Object = 0) As Task

        If previousForm IsNot Nothing Then

            If groupindex = 1 Then 'HRIS

                If previousForm.Name = "Employee" Then

                    With EmployeeForm

                        Select Case .tabIndx

                            Case .GetEmployeeProfileTabPageIndex
                                If .listofEditDepen.Count = 0 Then
                                    Await EmployeeForm.SearchEmployee()
                                Else

                                End If

                        End Select

                    End With

                End If

            ElseIf groupindex = 3 Then 'Payroll
                If previousForm.Name = "Paystub" Then
                    Await PayStubForm.VIEW_payperiodofyear()
                End If
            End If

        End If
    End Function

    Private Async Sub PayrollToolStripButton_Click(sender As Object, e As EventArgs) Handles PayrollToolStripButton.Click

        isHome = 0

        LockTime()

        ChangeForm(PayrollForm)

        GeneralForm.Hide()
        HRISForm.Hide()
        TimeAttendForm.Hide()

        FormReports.Hide()

        HomeToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        GeneralToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        HrisToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        TimeAndAttendanceToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        ReportsToolStripButton.BackColor = Color.FromArgb(194, 228, 255)

        PayrollToolStripButton.BackColor = Color.FromArgb(255, 255, 255)

        PayrollToolStripButton.Font = selectedButtonFont

        HomeToolStripButton.Font = unselectedButtonFont
        GeneralToolStripButton.Font = unselectedButtonFont
        HrisToolStripButton.Font = unselectedButtonFont
        TimeAndAttendanceToolStripButton.Font = unselectedButtonFont
        ReportsToolStripButton.Font = unselectedButtonFont

        Await refresh_previousForm(3)

    End Sub

    Private Async Sub tsbtnHRIS_Click(sender As Object, e As EventArgs) Handles HrisToolStripButton.Click

        isHome = 0

        LockTime()

        ChangeForm(HRISForm)

        GeneralForm.Hide()
        PayrollForm.Hide()
        TimeAttendForm.Hide()

        FormReports.Hide()

        HomeToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        GeneralToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        TimeAndAttendanceToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        PayrollToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        ReportsToolStripButton.BackColor = Color.FromArgb(194, 228, 255)

        HrisToolStripButton.BackColor = Color.FromArgb(255, 255, 255)

        HrisToolStripButton.Font = selectedButtonFont

        HomeToolStripButton.Font = unselectedButtonFont
        GeneralToolStripButton.Font = unselectedButtonFont
        TimeAndAttendanceToolStripButton.Font = unselectedButtonFont
        PayrollToolStripButton.Font = unselectedButtonFont
        ReportsToolStripButton.Font = unselectedButtonFont

        Await refresh_previousForm(1)
    End Sub

    'Toggling pin status

    Private Sub Pin_UnPin(sender As Object, e As EventArgs)

        Static once As SByte = 0

        If once = 0 Then
            once = 1
        End If

        ImageList1.Images(0).Tag = 1
        ImageList1.Images(1).Tag = 2
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

        Static once As SByte = 0

        If once = 0 Then
            once = 1
            PictureBox1.Image.Tag = 1
        End If

        ImageList1.Images(0).Tag = 1
        ImageList1.Images(1).Tag = 2

        If PictureBox1.Image.Tag = 1 Then 'Hide toolstrip
            PictureBox1.Image = ImageList1.Images(0)
            PictureBox1.Image.Tag = 0

            Showmainbutton.Dock = DockStyle.None
        Else '                             'Show toolstrip
            PictureBox1.Image = ImageList1.Images(1)
            PictureBox1.Image.Tag = 1

            Showmainbutton.Dock = DockStyle.Left

        End If

    End Sub

    Private Sub ToolStripButton5_Click_1(sender As Object, e As EventArgs) Handles ReportsToolStripButton.Click
        isHome = 0

        LockTime()

        ChangeForm(FormReports)

        GeneralForm.Hide()
        HRISForm.Hide()
        TimeAttendForm.Hide()

        HomeToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        GeneralToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        HrisToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        TimeAndAttendanceToolStripButton.BackColor = Color.FromArgb(194, 228, 255)
        PayrollToolStripButton.BackColor = Color.FromArgb(194, 228, 255)

        ReportsToolStripButton.BackColor = Color.FromArgb(255, 255, 255)

        ReportsToolStripButton.Font = selectedButtonFont

        HomeToolStripButton.Font = unselectedButtonFont
        GeneralToolStripButton.Font = unselectedButtonFont
        HrisToolStripButton.Font = unselectedButtonFont
        TimeAndAttendanceToolStripButton.Font = unselectedButtonFont
        PayrollToolStripButton.Font = unselectedButtonFont
    End Sub

    Sub LockTime()
        Timer2.Stop()
        bgDashBoardReloader.CancelAsync()
        Timer2.Enabled = False
    End Sub

    Sub UnlockTime()

        Timer2.Enabled = True
        Timer2.Start()

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Timer2_Tick(Timer2, New EventArgs)

        End If

    End Sub

    Private Sub Timer2_Tick(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Timer2.Tick

        TimeTick += 1

        If TimeTick = 1 Then ' the timer now succeeds 60 seconds

            TimeTick = 0

            LockTime()

            If bgDashBoardReloader.IsBusy = False Then

                bgDashBoardReloader.RunWorkerAsync()

            End If

        End If

    End Sub

    Dim n_bgwAge21Dependents = Nothing

    Dim n_bgwBDayCelebrant = Nothing

    Dim n_bgwOBPending = Nothing

    Dim n_bgwOTPending = Nothing

    Dim n_bgwLoanBalances = Nothing

    Dim n_bgwNegaPaySlips = Nothing

    Dim n_bgwForRegularization = Nothing

    Dim dt_pend_leave As New DataTable

    Private Sub bgDashBoardReloader_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgDashBoardReloader.DoWork

        If CType(sender, BackgroundWorker).CancellationPending Then

            e.Cancel = True
            Return

        End If

        Dim params(0, 1) As Object

        params(0, 0) = "OrganizID"

        params(0, 1) = orgztnID

        n_bgwAge21Dependents = New DashBoardDataExtractor(params, "DBoard_Age21Dependents")

        n_bgwAge21Dependents = n_bgwAge21Dependents.getDataTable

        n_bgwBDayCelebrant = New DashBoardDataExtractor(params, "DBoard_BirthdayCelebrantThisMonth")

        n_bgwBDayCelebrant = n_bgwBDayCelebrant.getDataTable

        n_bgwOBPending = New DashBoardDataExtractor(params, "DBoard_OBPending")

        n_bgwOBPending = n_bgwOBPending.getDataTable

        n_bgwOTPending = New DashBoardDataExtractor(params, "DBoard_OTPending")

        n_bgwOTPending = n_bgwOTPending.getDataTable

        n_bgwLoanBalances = New DashBoardDataExtractor(params, "DBoard_LoanBalances")

        n_bgwLoanBalances = n_bgwLoanBalances.getDataTable

        n_bgwNegaPaySlips = New DashBoardDataExtractor(params, "DBoard_NegativePaySlips")

        n_bgwNegaPaySlips = n_bgwNegaPaySlips.getDataTable

        n_bgwForRegularization = New DashBoardDataExtractor(params, "DBoard_ForRegularization")

        n_bgwForRegularization = n_bgwForRegularization.getDataTable

        dgvfrequentabsent.Tag = New SQLQueryToDatatable("CALL `FREQUENT_absent`('" & orgztnID & "');").ResultTable

        dgvfrequentleave.Tag = New SQLQueryToDatatable("CALL `FREQUENT_leave`('" & orgztnID & "');").ResultTable

        'dgvfrequentleave

        If if_sysowner_is_hyundai Then

            Dim pend_leave As New SQL(str_pending_leave,
                New Object() {orgztnID, Core.Entities.Leave.StatusPending})

            dt_pend_leave = pend_leave.GetFoundRows.Tables(0)

        End If

        If CType(sender, BackgroundWorker).CancellationPending Then

            e.Cancel = True
            Return

        End If

    End Sub

    Private Sub bgDashBoardReloader_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bgDashBoardReloader.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            Console.WriteLine("bgDashBoardReloader ERROR : " & e.Error.Message)
            Return
        ElseIf e.Cancelled Then
            Console.WriteLine("bgDashBoardReloader CANCELLED")
            Return
        End If

        UnlockTime()

        Static once As SByte = 0

        Dim dattbl = InstantiateDatatable(n_bgwAge21Dependents)

        PopulateDGVwithDatTbl(dgvAge21Depen,
                              dattbl)

        dattbl = InstantiateDatatable(n_bgwBDayCelebrant)

        PopulateDGVwithDatTbl(dgvBDayCeleb,
                              dattbl)

        dattbl = InstantiateDatatable(n_bgwLoanBalances)

        PopulateDGVwithDatTbl(dgvLoanBalance,
                              dattbl)

        dattbl = InstantiateDatatable(n_bgwOBPending)

        PopulateDGVwithDatTbl(dgvOBPending,
                              dattbl)

        dattbl = InstantiateDatatable(n_bgwOTPending)

        PopulateDGVwithDatTbl(dgvOTPending,
                              dattbl)

        'dgvnegaPaySlip, n_bgwNegaPaySlips
        dattbl = InstantiateDatatable(n_bgwNegaPaySlips)

        PopulateDGVwithDatTbl(dgvnegaPaySlip,
                              dattbl)

        dattbl = InstantiateDatatable(n_bgwForRegularization)

        PopulateDGVwithDatTbl(dgvRegularization,
                              dattbl)

        Dim new_dt As New DataTable
        new_dt = DirectCast(dgvfrequentabsent.Tag, DataTable)
        PopulateDGVwithDatTbl(dgvfrequentabsent,
                              new_dt)

        Dim n_dt As New DataTable
        n_dt = DirectCast(dgvfrequentleave.Tag, DataTable)
        PopulateDGVwithDatTbl(dgvfrequentleave,
                              n_dt)

        If if_sysowner_is_hyundai Then
            dgvpendingleave.Rows.Clear()
            For Each drow As DataRow In dt_pend_leave.Rows
                dgvpendingleave.Rows.Add(drow.ItemArray)
            Next
            dgvpendingleave.Enabled = True
        End If

        dgvAge21Depen.Enabled = True
        dgvBDayCeleb.Enabled = True
        dgvLoanBalance.Enabled = True
        dgvOBPending.Enabled = True
        dgvOTPending.Enabled = True
        dgvnegaPaySlip.Enabled = True
        dgvRegularization.Enabled = True
        dgvfrequentabsent.Enabled = True
        dgvfrequentleave.Enabled = True

        If once = 0 Then
            once = 1
            dgvAge21Depen.Enabled = True
            dgvBDayCeleb.Enabled = True

        End If
    End Sub

    Private Async Function RestrictDashboardByPermission() As Task

        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
        End If

        If _policyHelper.UseUserLevel Then

            Await RestrictByUserLevel()
        Else

            RestrictByRole()

        End If

    End Function

    Private Sub RestrictByRole()

        'General
        Dim userPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.USER).FirstOrDefault()
        Dim organizationPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.ORGANIZATION).FirstOrDefault()
        Dim branchPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.BRANCH).FirstOrDefault()
        Dim rolePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.ROLE).FirstOrDefault()
        Dim shiftPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.SHIFT).FirstOrDefault()
        Dim calendarPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.CALENDAR).FirstOrDefault()
        Dim agencyPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.AGENCY).FirstOrDefault()

        If Not CheckReadPermission(userPermission) AndAlso
            Not CheckReadPermission(organizationPermission) AndAlso
            Not CheckReadPermission(branchPermission) AndAlso
            Not CheckReadPermission(rolePermission) AndAlso
            Not CheckReadPermission(shiftPermission) AndAlso
            Not CheckReadPermission(calendarPermission) Then

            GeneralToolStripButton.Visible = False

            If _policyHelper.UseAgency AndAlso CheckReadPermission(agencyPermission) Then
                GeneralToolStripButton.Visible = True
            End If

        End If

        'HRIS
        Dim employeePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.EMPLOYEE).FirstOrDefault()
        Dim salaryPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.SALARY).FirstOrDefault()
        Dim divisionPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.DIVISION).FirstOrDefault()
        Dim positionPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.POSITION).FirstOrDefault()

        If Not CheckReadPermission(employeePermission) AndAlso
            Not CheckReadPermission(salaryPermission) AndAlso
            Not CheckReadPermission(divisionPermission) AndAlso
            Not CheckReadPermission(positionPermission) Then

            HrisToolStripButton.Visible = False
        End If

        'Time & Attendance
        Dim leavePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.LEAVE).FirstOrDefault()
        Dim officialBusinessPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.OFFICIALBUSINESS).FirstOrDefault()
        Dim overtimePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.OVERTIME).FirstOrDefault()
        'shiftPermission
        Dim timeLogPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.TIMELOG).FirstOrDefault()
        Dim timeEntryPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.TIMEENTRY).FirstOrDefault()

        If Not CheckReadPermission(leavePermission) AndAlso
            Not CheckReadPermission(officialBusinessPermission) AndAlso
            Not CheckReadPermission(overtimePermission) AndAlso
            Not CheckReadPermission(shiftPermission) AndAlso
            Not CheckReadPermission(timeLogPermission) AndAlso
            Not CheckReadPermission(timeEntryPermission) Then

            TimeAndAttendanceToolStripButton.Visible = False
        End If

        'Payroll
        Dim allowancePermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.ALLOWANCE).FirstOrDefault()
        Dim loanPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.LOAN).FirstOrDefault()
        Dim payPeriodPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.PAYPERIOD).FirstOrDefault()

        If Not CheckReadPermission(allowancePermission) AndAlso
            Not CheckReadPermission(loanPermission) AndAlso
            Not CheckReadPermission(payPeriodPermission) Then

            PayrollToolStripButton.Visible = False
        End If

        ShowOrHideDashboardWidgets(
            loanPermission:=loanPermission,
            timeLogPermission:=timeLogPermission,
            payPeriodPermission:=payPeriodPermission,
            employeePermission:=employeePermission,
            officialBusinessPermission:=officialBusinessPermission,
            overtimePermission:=overtimePermission,
            leavePermission:=leavePermission)
    End Sub

    Private Shared Function CheckReadPermission(userPermission As RolePermission) As Boolean
        Return userPermission IsNot Nothing AndAlso userPermission.Read
    End Function

    Private Sub ShowOrHideDashboardWidgets(
        loanPermission As RolePermission,
        timeLogPermission As RolePermission,
        payPeriodPermission As RolePermission,
        employeePermission As RolePermission,
        officialBusinessPermission As RolePermission,
        overtimePermission As RolePermission,
        leavePermission As RolePermission)

        'TODO: stop querying for dashboard data if user has no read permission for that

        If Not CheckReadPermission(loanPermission) Then
            LoanBalanceCollapsibleGroupBox.Visible = False
        End If

        If Not CheckReadPermission(timeLogPermission) Then
            CollapsibleGroupBox3.Visible = False
        End If

        If Not CheckReadPermission(payPeriodPermission) Then
            NegativePayslipsCollapsibleGroupBox.Visible = False
        End If

        If Not CheckReadPermission(employeePermission) Then
            BirthdayCollapsibleGroupBox.Visible = False
            UnqualifiedCollapsibleGroupBox.Visible = False
            CollapsibleGroupBox5.Visible = False
        End If

        If Not CheckReadPermission(officialBusinessPermission) Then
            PendingOfficialBusinessCollapsibleGroupBox.Visible = False
        End If

        If Not CheckReadPermission(overtimePermission) Then
            PendingOvertimeCollapsibleGroupBox.Visible = False
        End If

        If Not CheckReadPermission(leavePermission) Then
            CollapsibleGroupBox4.Visible = False
            CollapsibleGroupBox6.Visible = False
        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = Keys.F5 And isHome = 1 Then
            Timer2_Tick(Timer2, New EventArgs)
            Return True
        ElseIf keyData = Keys.F1 Then
            PictureBox1_Click(PictureBox1, New EventArgs)
            Return True
        ElseIf keyData = Keys.Oem5 Then
            Static thrice As Integer = -1
            thrice += 1

            If thrice = 5 Then
                thrice = 0
            End If

            Return False
        Else
            Return MyBase.ProcessCmdKey(msg, keyData)
        End If
    End Function

    Private Sub MDIPrimaryForm_TextChanged(sender As Object, e As EventArgs) Handles Me.TextChanged
        CenterMe()
    End Sub

    Private Sub MDIPrimaryForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        CenterMe()

        Width_resolution = Me.Width

        Height_resolution = Me.Height
    End Sub

    Private Const strtoConcat = " "

    Private Sub CenterMe()
        Me.Refresh()
    End Sub

    Protected Overrides Sub OnActivated(e As EventArgs)
        MyBase.OnActivated(e)
    End Sub

    Protected Overrides Sub OnDeactivate(e As EventArgs)
        MyBase.OnDeactivate(e)
    End Sub

    Dim bgwork_errormsg As String = String.Empty

    Private Sub setProperDashBoardAccordingToSystemOwner()
        If if_sysowner_is_cinema2k Then
            setVisiblePropertyDashBoardBaseOnCinema2K(Panel8)
            setVisiblePropertyDashBoardBaseOnCinema2K(Panel9)
            setVisiblePropertyDashBoardBaseOnCinema2K(Panel10)
        End If
    End Sub

    Private Sub setVisiblePropertyDashBoardBaseOnCinema2K(pnl As Panel)

        Dim _list =
            pnl.Controls.OfType(Of CollapsibleGroupBox)()

        For Each collapgpbox In _list
            Dim _bool As Boolean =
                (collapgpbox.AccessibleDescription = SystemOwnerService.Cinema2000)

            collapgpbox.Visible = _bool

        Next

    End Sub

    Private Async Sub EmailStatusTimer_Tick(sender As Object, e As EventArgs)
        Try
            EmailServiceStatusToolStripLabel.Enabled = True

            Dim onQueue = Await _paystubEmailRepository.GetAllOnQueueAsync()
            Dim queueCount = onQueue.Count()

            Dim connectionString = ConnectionStringRegistry.GetCurrent()
            Dim service = New WSMService(connectionString.ServerName, StringConfig.AccupayEmailServiceName)
            Dim status = Await service.GetStatus()

            Dim isOnline = status = ServiceProcess.ServiceControllerStatus.Running

            UpdateEmailStatusToolStripLabel(isOnline, queueCount)
        Catch ex As Exception
            EmailServiceStatusToolStripLabel.Text = $"Cannot access Email Service status."
            EmailServiceStatusToolStripLabel.ForeColor = Color.Black
        End Try
    End Sub

    Private Sub UpdateEmailStatusToolStripLabel(isOnline As Boolean, queueCount As Integer)

        Dim foreColor = Color.DarkGreen
        Dim status = "Online"

        If Not isOnline Then

            foreColor = Color.DarkRed
            status = "OFFLINE"
        End If

        Dim queueDescription = String.Empty

        If queueCount > 0 Then
            EmailServiceStatusToolStripLabel.ForeColor = Color.DarkOrange
            queueDescription = $"{queueCount} email{If(queueCount > 1, "s", "")} on queue."
        End If

        EmailServiceStatusToolStripLabel.Text = $"Email Service is {status}. {queueDescription}"
        EmailServiceStatusToolStripLabel.ForeColor = foreColor
    End Sub

    Private Sub EmailServiceStatusToolStripLabel_Click(sender As Object, e As EventArgs) Handles EmailServiceStatusToolStripLabel.Click

        Dim form As New EmailDashboardForm()
        form.ShowDialog()

    End Sub

End Class

Public Class DashBoardDataExtractor

    Dim datatab As New DataTable

    Sub New(Optional ParamsCollection As Array = Nothing,
            Optional ProcedureName As String = Nothing)

        datatab = callProcAsDatTbl(ParamsCollection,
                                   ProcedureName)

    End Sub

    Public ReadOnly Property getDataTable As DataTable

        Get
            Return datatab

        End Get

    End Property

    Function callProcAsDatTbl(Optional ParamsCollection As Array = Nothing,
                                      Optional ProcedureName As String = Nothing) As Object

        Dim returnvalue = Nothing

        Dim mysqlda As New MySqlDataAdapter()

        Dim new_conn As New MySqlConnection

        new_conn.ConnectionString = db_connectinstring

        Try

            If new_conn.State = ConnectionState.Open Then : new_conn.Close() : End If

            new_conn.Open()

            Dim ds As New DataSet()

            With mysqlda

                .SelectCommand = New MySqlCommand(ProcedureName, new_conn)
                .SelectCommand.CommandTimeout = 999999
                .SelectCommand.CommandType = CommandType.StoredProcedure

                .SelectCommand.Parameters.Clear()

                For e = 0 To ParamsCollection.GetUpperBound(0) ' - 1

                    Dim paramName As String = ParamsCollection(e, 0)

                    Dim paramVal As Object = ParamsCollection(e, 1)

                    .SelectCommand.Parameters.AddWithValue(paramName, paramVal)

                Next

                .Fill(ds, "Table0")

            End With

            Dim dt As DataTable = ds.Tables("Table0")

            returnvalue = dt

            hasERR = 0
        Catch ex As Exception
            hasERR = 1

            MsgBox(getErrExcptn(ex, ProcedureName), MsgBoxStyle.Critical)

            returnvalue = Nothing
        Finally

            mysqlda.Dispose()

        End Try

        Return returnvalue

    End Function

End Class
