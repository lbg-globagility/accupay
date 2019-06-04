Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Loans
Imports log4net
Imports System.Collections.Concurrent

Public Class PayStubForm

    Private _logger As ILog = LogManager.GetLogger("PayrollLogger")

    Public q_employee As String = "SELECT e.RowID," &
        "e.EmployeeID 'Employee ID'," &
        "e.FirstName 'First Name'," &
        "e.MiddleName 'Middle Name'," &
        "e.LastName 'Last Name'," &
        "e.Surname," &
        "e.Nickname," &
        "e.MaritalStatus 'Marital Status'," &
        "COALESCE(e.NoOfDependents,0) 'No. Of Dependents'," &
        "DATE_FORMAT(e.Birthdate,'%m-%d-%Y') 'Birthdate'," &
        "DATE_FORMAT(e.Startdate,'" & custom_mysqldateformat & "') AS Startdate," &
        "e.JobTitle 'Job Title'," &
        "COALESCE(pos.PositionName,'') 'Position'," &
        "e.Salutation," &
        "e.TINNo 'TIN'," &
        "e.SSSNo 'SSS No.'," &
        "e.HDMFNo 'PAGIBIG No.'," &
        "e.PhilHealthNo 'PhilHealth No.'," &
        "e.WorkPhone 'Work Phone No.'," &
        "e.HomePhone 'Home Phone No.'," &
        "e.MobilePhone 'Mobile Phone No.'," &
        "e.HomeAddress 'Home address'," &
        "e.EmailAddress 'Email address'," &
        "IF(e.Gender='M','Male','Female') 'Gender'," &
        "e.EmploymentStatus 'Employment Status'," &
        "IFNULL(pf.PayFrequencyType,'') 'Pay Frequency'," &
        "e.UndertimeOverride," &
        "e.OvertimeOverride," &
        "COALESCE(pos.RowID,'pos.RowID') 'PositionID'" &
        ",IFNULL(e.PayFrequencyID,'') 'PayFrequencyID'" &
        ",e.EmployeeType" &
        ",e.LeaveBalance" &
        ",e.SickLeaveBalance" &
        ",e.MaternityLeaveBalance" &
        ",e.LeaveAllowance" &
        ",e.SickLeaveAllowance" &
        ",e.MaternityLeaveAllowance" &
        ",e.LeavePerPayPeriod" &
        ",e.SickLeavePerPayPeriod" &
        ",e.MaternityLeavePerPayPeriod" &
        ",COALESCE(fstat.RowID,3) 'fstatRowID'" &
        ",'' 'Image'" &
        ",DATE_FORMAT(e.Created,'%m-%d-%Y') 'Creation Date'," &
        "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) 'Created by'," &
        "COALESCE(DATE_FORMAT(e.LastUpd,'%m-%d-%Y'),'') 'Last Update'," &
        "(SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))  FROM user WHERE RowID=e.LastUpdBy) 'LastUpdate by'" &
        " " &
        "FROM employee e " &
        "LEFT JOIN user u ON e.CreatedBy=u.RowID " &
        "LEFT JOIN position pos ON e.PositionID=pos.RowID " &
        "LEFT JOIN payfrequency pf ON e.PayFrequencyID=pf.RowID " &
        "LEFT JOIN filingstatus fstat ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents " &
        "WHERE e.OrganizationID=" & orgztnID

    Public current_year As String = CDate(dbnow).Year

    Dim pagination As Integer = 0

    Dim employeepicture As New DataTable

    Dim viewID As Integer = Nothing

    Dim n_VeryFirstPayPeriodIDOfThisYear As Object = Nothing

    Public paypFrom As String = Nothing
    Public paypTo As String = Nothing
    Public paypRowID As String = Nothing
    Public paypPayFreqID As String = Nothing
    Public isEndOfMonth As String = 0

    Private _totalPaystubs As Integer = 0
    Private _finishedPaystubs As Integer = 0
    Private _successfulPaystubs As Integer = 0
    Private _failedPaystubs As Integer = 0

    Const max_count_per_page As Integer = 50

    Dim currentEmployeeID As String = Nothing

    Dim employee_dattab As New DataTable

    Public numofweekdays As Integer

    Public numofweekends As Integer

    Public withthirteenthmonthpay As SByte = 0

    Const max_rec_perpage As Integer = 1

    Dim IsUserPressEnterToSearch As Boolean = False

    Dim selectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim unselectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim quer_empPayFreq = ""

    Dim dtJosh As DataTable
    Dim da As New MySqlDataAdapter()

    Dim dtprintAllPaySlip As New DataTable

    Dim rptdocAll As New rptAllDecUndecPaySlip

    Private sys_ownr As New SystemOwner

    Private _results As BlockingCollection(Of PayrollGeneration.Result)

    Property VeryFirstPayPeriodIDOfThisYear As Object
        Get
            Return n_VeryFirstPayPeriodIDOfThisYear
        End Get
        Set(value As Object)
            n_VeryFirstPayPeriodIDOfThisYear = value
        End Set
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)

        SplitContainer1.SplitterWidth = 6

        Dim dgvVisibleRows = dgvpayper.Columns.Cast(Of DataGridViewColumn).Where(Function(ii) ii.Visible = True)

        Dim scrollbarwidth = 19

        Dim mincolwidth As Integer = (dgvpayper.Width - (dgvpayper.RowHeadersWidth + scrollbarwidth)) / dgvVisibleRows.Count

        For Each dgvcol In dgvVisibleRows
            dgvcol.Width = mincolwidth
            dgvcol.SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        setProperInterfaceBaseOnCurrentSystemOwner()

        For Each txtbx In Panel6.Controls.OfType(Of TextBox)
            AddHandler txtbx.TextChanged, AddressOf NumberFieldFormatter
        Next

        MyBase.OnLoad(e)

    End Sub

    Private Sub PayStub_EnabledChanged(sender As Object, e As EventArgs) Handles Me.EnabledChanged

        Dim _bool As Boolean = Me.Enabled

        ''###### CONTROL DISABLER ######

        PayrollForm.MenuStrip1.Enabled = _bool
        MDIPrimaryForm.Showmainbutton.Enabled = _bool
        ToolStrip1.Enabled = _bool

        Panel5.Enabled = _bool

        MDIPrimaryForm.systemprogressbar.Value = 0
        MDIPrimaryForm.systemprogressbar.Visible = CBool(Not _bool)

    End Sub

    Private Sub PayStub_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        viewID = VIEW_privilege("Employee Pay Slip", orgztnID)

        VIEW_payperiodofyear()

        dgvpayper.Focus()

        linkPrev.Text = "← " & (current_year - 1)
        linkNxt.Text = (current_year + 1) & " →"

        If dgvemployees.RowCount <> 0 Then
            dgvemployees.Item("EmployeeID", 0).Selected = 1
        End If

        Dim formuserprivilege = position_view_table.Select("ViewID = " & viewID)

        If formuserprivilege.Count = 0 Then

            tsbtngenpayroll.Visible = 0
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    tsbtngenpayroll.Visible = 0

                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        tsbtngenpayroll.Visible = 0
                    Else
                        tsbtngenpayroll.Visible = 1
                    End If

                End If

            Next

        End If

        cboProducts.ValueMember = "ProductID"
        cboProducts.DisplayMember = "ProductName"

        cboProducts.DataSource = New SQLQueryToDatatable("SELECT RowID AS 'ProductID', Name AS 'ProductName', Category FROM product WHERE Category IN ('Allowance Type', 'Bonus', 'Adjustment Type')" &
                                                  " AND OrganizationID='" & orgztnID & "' ORDER BY Name;").ResultTable

        dgvAdjustments.AutoGenerateColumns = False

    End Sub

    Sub VIEW_payperiodofyear(Optional param_Date As Object = Nothing)
        Dim hasValue = (MDIPrimaryForm.systemprogressbar.Value > 0)
        If hasValue Then
            For i = 0 To 5
                RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
            Next
        End If
        Dim date_param = If(param_Date = Nothing, "NULL", "'" & param_Date & "-01-01'")
        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("CALL VIEW_payperiodofyear('" & orgztnID & "'," & date_param & ",'0');")
        Dim catchdt As New DataTable : catchdt = n_SQLQueryToDatatable.ResultTable
        dgvpayper.Rows.Clear()
        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvpayper.Rows.Add(row_array)
        Next
        catchdt.Dispose()
        If hasValue Then
            AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
        End If
    End Sub

    Sub loademployee(Optional q_empsearch As String = Nothing)
        If q_empsearch = Nothing Then
        Else
            If pagination <= 0 Then
                pagination = 0
            End If
        End If

        Dim catchdt As New DataTable
        Dim param_array = New Object() {orgztnID,
                                        tsSearch.Text,
                                        pagination,
                                        GetCurrentPayFrequencyType()}

        Dim n_ReadSQLProcedureToDatatable As New _
            SQL("CALL SEARCH_employee_paystub(?og_rowid, ?unified_search_string, ?page_number, ?text_pay_freq_sched);",
                param_array)

        catchdt = n_ReadSQLProcedureToDatatable.GetFoundRows.Tables(0)

        dgvemployees.Rows.Clear()

        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvemployees.Rows.Add(row_array)
        Next

        Static x As SByte = 0

        If x = 0 Then
            x = 1

            With dgvemployees
                .Columns("RowID").Visible = False
                .Columns("UndertimeOverride").Visible = False
                .Columns("OvertimeOverride").Visible = False
                .Columns("PositionID").Visible = False
                .Columns("PayFreqID").Visible = False

                .Columns("LeaveBal").Visible = False
                .Columns("SickBal").Visible = False
                .Columns("MaternBal").Visible = False

                .Columns("LeaveAllow").Visible = False
                .Columns("SickAllow").Visible = False
                .Columns("MaternAllow").Visible = False

                .Columns("Leavepayp").Visible = False
                .Columns("Sickpayp").Visible = False
                .Columns("Maternpayp").Visible = False

                .Columns("fstatRowID").Visible = False
                .Columns("Image").Visible = False

            End With
            If dgvemployees.RowCount > 0 Then
                dgvemployees.Item("EmployeeID", 0).Selected = True
            End If

            employeepicture = New SQLQueryToDatatable("SELECT RowID,Image FROM employee WHERE Image IS NOT NULL AND OrganizationID=" & orgztnID & ";").ResultTable 'retAsDatTbl("SELECT RowID,Image FROM employee WHERE OrganizationID=" & orgztnID & ";")
        End If
    End Sub

    Private Sub dgvpayper_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpayper.CellContentClick

    End Sub

    Private Sub dgvpayper_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvpayper.SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        Static str_pay_freq_sched As String = String.Empty

        Static _year As Integer = 0

        If dgvpayper.RowCount > 0 Then
            With dgvpayper.CurrentRow

                paypPayFreqID = .Cells("Column4").Value

                paypRowID = .Cells("Column1").Value
                paypFrom = MYSQLDateFormat(CDate(.Cells("Column2").Value)) 'Format(CDate(.Cells("Column2").Value), "yyyy-MM-dd")

                paypTo = MYSQLDateFormat(CDate(.Cells("Column3").Value)) 'Format(CDate(.Cells("Column3").Value), "yyyy-MM-dd")

                isEndOfMonth = Trim(.Cells("Column14").Value)

                Dim date_diff = DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo))

                numofweekdays = 0

                For i = 0 To date_diff

                    Dim DayOfWeek = CDate(paypFrom).AddDays(i)

                    If DayOfWeek.DayOfWeek = 0 Then 'System.DayOfWeek.Sunday
                        numofweekends += 1

                    ElseIf DayOfWeek.DayOfWeek = 6 Then 'System.DayOfWeek.Saturday
                        numofweekends += 1
                    Else
                        numofweekdays += 1

                    End If

                Next

                Dim str_sched_payfreq As String = Convert.ToString(.Cells("Column12").Value)

                str_pay_freq_sched = str_sched_payfreq

                _year = current_year

                Dim select_cutoff_payfrequency =
                    tstrip.Items.OfType(Of ToolStripButton).Where(Function(tsbtn) tsbtn.Text = str_sched_payfreq)

                For Each _tsbtn In select_cutoff_payfrequency
                    PayFreq_Changed(_tsbtn, New EventArgs)

                Next

            End With
        Else

            numofweekdays = 0

            paypRowID = Nothing
            paypFrom = Nothing
            paypTo = Nothing
            isEndOfMonth = 0
            paypPayFreqID = String.Empty

            dgvemployees_SelectionChanged(sender, e)

        End If
        AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        current_year = current_year - 1

        linkPrev.Text = "← " & (current_year - 1)
        linkNxt.Text = (current_year + 1) & " →"

        VIEW_payperiodofyear(current_year)

        AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        dgvpayper_SelectionChanged(sender, e)
    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        current_year = current_year + 1

        linkNxt.Text = (current_year + 1) & " →"
        linkPrev.Text = "← " & (current_year - 1)

        VIEW_payperiodofyear(current_year)

        AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        dgvpayper_SelectionChanged(sender, e)
    End Sub

    Private Sub First_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked, Prev.LinkClicked,
                                                                                                Nxt.LinkClicked, Last.LinkClicked,
                                                                                                LinkLabel4.LinkClicked, LinkLabel3.LinkClicked,
                                                                                                LinkLabel2.LinkClicked, LinkLabel1.LinkClicked
        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "First" Or sendrname = "LinkLabel1" Then
            pagination = 0
        ElseIf sendrname = "Prev" Or sendrname = "LinkLabel2" Then

            Dim modcent = pagination Mod max_count_per_page

            If modcent = 0 Then

                pagination -= max_count_per_page
            Else

                pagination -= modcent

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sendrname = "Nxt" Or sendrname = "LinkLabel4" Then

            Dim modcent = pagination Mod max_count_per_page

            If modcent = 0 Then
                pagination += max_count_per_page
            Else
                pagination -= modcent

                pagination += max_count_per_page

            End If
        ElseIf sendrname = "Last" Or sendrname = "LinkLabel3" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / " & max_count_per_page & " FROM employee WHERE OrganizationID=" & orgztnID & ";"))
            Dim remender = lastpage Mod 1

            pagination = (lastpage - remender) * max_count_per_page
        End If

        Dim pay_freqString = String.Empty

        For Each ctrl In tstrip.Items
            If TypeOf ctrl Is ToolStripButton Then

                If DirectCast(ctrl, ToolStripButton).BackColor =
                        Color.FromArgb(194, 228, 255) Then

                    pay_freqString =
                            DirectCast(ctrl, ToolStripButton).Text

                    Exit For
                Else
                    Continue For
                End If
            Else
                Continue For
            End If

        Next

        quer_empPayFreq = " And pf.PayFrequencyType='" & pay_freqString & "' "

        loademployee(quer_empPayFreq)

        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        dgvemployees_SelectionChanged(sender, e)

        AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

    Private Sub dgvemployees_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvemployees.SelectionChanged

        btnSaveAdjustments.Enabled = False

        Dim employeetype = ""

        Static sameEmpID As String = 0

        dgvemployees.Tag = Nothing

        ResetNumberTextBox()

        If dgvemployees.RowCount > 0 Then 'And dgvemployees.CurrentRow IsNot Nothing
            With dgvemployees.CurrentRow

                employeetype = Trim(.Cells("EmployeeType").Value)
                sameEmpID = .Cells("RowID").Value

                dgvemployees.Tag = .Cells("RowID").Value

                txtFName.Text = .Cells("FirstName").Value

                Dim addtlWord = Nothing

                If .Cells("MiddleName").Value = Nothing Then
                Else

                    Dim midNameTwoWords = Split(.Cells("MiddleName").Value.ToString, " ")

                    addtlWord = " "

                    For Each s In midNameTwoWords

                        addtlWord &= (StrConv(Microsoft.VisualBasic.Left(s, 1), VbStrConv.ProperCase) & ".")

                    Next

                    txtFName.Text &= addtlWord

                End If

                txtFName.Text = txtFName.Text & " " & .Cells("LastName").Value

                txtFName.Text = txtFName.Text & If(.Cells("Surname").Value = Nothing,
                                                         "",
                                                         "-" & StrConv(.Cells("Surname").Value,
                                                                       VbStrConv.ProperCase))

                currentEmployeeID = .Cells("EmployeeID").Value

                txtEmpID.Text = "ID# " & .Cells("EmployeeID").Value &
                            If(IsDBNull(.Cells("Position")),
                                                               "",
                                                               ", " & .Cells("Position").Value) &
                            If(.Cells("EmployeeType").Value = Nothing,
                                                               "",
                                                               ", " & .Cells("EmployeeType").Value & " salary")

                pbEmpPicChk.Image = Nothing
                Try
                    Dim selemppic = employeepicture.Select("RowID=" & ValNoComma(.Cells("RowID").Value))

                    For Each drow In selemppic
                        If IsDBNull(drow("Image")) Then
                            pbEmpPicChk.Image = Nothing
                        Else
                            pbEmpPicChk.Image = ConvByteToImage(DirectCast(drow("Image"), Byte()))
                        End If
                        Exit For
                    Next
                Catch ex As Exception

                End Try

                'End If

                Gender_Label(.Cells("Gender").Value)

                txtvlallow.Text = .Cells("LeaveAllow").Value
                txtslallow.Text = .Cells("SickAllow").Value
                txtmlallow.Text = .Cells("MaternAllow").Value

                txtvlpayp.Text = .Cells("Leavepayp").Value
                txtslpayp.Text = .Cells("Sickpayp").Value
                txtmlpayp.Text = .Cells("Maternpayp").Value

                txttotabsentamt.Text = "0.00"
                txttottardiamt.Text = "0.00"
                txttotutamt.Text = "0.00"

                If ValNoComma(paypRowID) > 0 Then
                    '# ####################################################### #
                    Try
                        For Each txtbxctrl In SplitContainer1.Panel2.Controls.OfType(Of TextBox).ToList()
                            txtbxctrl.Text = "0.00"
                        Next
                    Catch ex As Exception
                        MsgBox(getErrExcptn(ex, Me.Name))
                    End Try
                    If tabEarned.SelectedIndex = 0 Then
                        TabPage1_Enter1(TabPage1, New EventArgs)
                    ElseIf tabEarned.SelectedIndex = 1 Then
                        TabPage4_Enter(TabPage4, New EventArgs)
                    End If
                Else
                    dgvpaystub.Rows.Clear()

                    txtgrosssal.Text = ""
                    txtnetsal.Text = ""
                    txttaxabsal.Text = ""

                    dgvpaystub.Rows.Clear()
                    dgvpaystubitem.Rows.Clear()
                    dgvempsal.Rows.Clear()
                    dgvetent.Rows.Clear()
                    dgvpaystubitm.Rows.Clear()
                    dgvempbon.Rows.Clear()
                    dgvLoanList.Rows.Clear()
                    dgvempallowance.Rows.Clear()
                    Try
                        For Each txtbxctrl In SplitContainer1.Panel2.Controls.OfType(Of TextBox).ToList()
                            txtbxctrl.Text = "0.00"
                        Next
                    Catch ex As Exception
                        MsgBox(getErrExcptn(ex, Me.Name))
                    End Try
                    If tabEarned.SelectedIndex = 0 Then
                        TabPage1_Enter1(TabPage1, New EventArgs)
                    ElseIf tabEarned.SelectedIndex = 1 Then
                        TabPage4_Enter(TabPage4, New EventArgs)
                    End If
                End If

            End With
        Else
            sameEmpID = -1

            currentEmployeeID = Nothing

            pbEmpPicChk.Image = Nothing
            txtFName.Text = ""
            txtEmpID.Text = ""

            txtBasicPay.Text = ""

            txttotreghrs.Text = ""
            txttotregamt.Text = ""

            txtOvertimeHours.Text = ""
            txtOvertimePay.Text = ""

            txtNightDiffHours.Text = ""
            txtNightDiffPay.Text = ""

            txtNightDiffOvertimeHours.Text = ""
            txtNightDiffOvertimePay.Text = ""

            txtHolidayHours.Text = ""
            txtHolidayPay.Text = ""

            txtRegularHours.Text = ""
            txtRegularPay.Text = ""

            lblSubtotal.Text = ""

            txtemptotallow.Text = ""
            txtTotTaxabAllow.Text = ""

            txtgrosssal.Text = ""

            txtvlbal.Text = ""
            txtslbal.Text = ""
            txtmlbal.Text = ""

            txtvlallow.Text = ""
            txtslallow.Text = ""
            txtmlallow.Text = ""

            txtvlpayp.Text = ""
            txtslpayp.Text = ""
            txtmlpayp.Text = ""

            txttotabsent.Text = ""
            txttotabsentamt.Text = ""

            txttottardi.Text = ""
            txttottardiamt.Text = ""

            txttotut.Text = ""
            txttotutamt.Text = ""

            lblsubtotmisc.Text = ""

            txtempsss.Text = ""
            txtempphh.Text = ""
            txtemphdmf.Text = ""

            txtemptotloan.Text = ""
            txtemptotbon.Text = ""

            txttaxabsal.Text = ""
            txtempwtax.Text = ""
            txtnetsal.Text = ""

            txtPaidLeave.Text = ""
            txtThirteenthMonthPay.Text = ""
            txtTotalNetPay.Text = ""

            dgvpaystub.Rows.Clear()
            dgvpaystubitem.Rows.Clear()
            dgvempsal.Rows.Clear()
            dgvetent.Rows.Clear()
            dgvpaystubitm.Rows.Clear()
            dgvempbon.Rows.Clear()
            dgvLoanList.Rows.Clear()
            dgvempallowance.Rows.Clear()

        End If

    End Sub

    Private Sub tsbtngenpayroll_Click(sender As Object, e As EventArgs) Handles tsbtngenpayroll.Click
        Me.VeryFirstPayPeriodIDOfThisYear = Nothing

        With selectPayPeriod
            .Show()
            .BringToFront()
            .dgvpaypers.Focus()
        End With
    End Sub

    Sub genpayroll(Optional PayFreqRowID As Object = Nothing)
        Dim loadTask = Task.Factory.StartNew(
            Function()
                If paypFrom = Nothing And paypTo = Nothing Then
                    Return Nothing
                End If

                Dim resources = New PayrollResources(paypRowID, CDate(paypFrom), CDate(paypTo))
                Dim resourcesTask = resources.Load()
                resourcesTask.Wait()

                Return resources
            End Function,
            0
        )

        loadTask.ContinueWith(
            AddressOf LoadingPayrollDataOnSuccess,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        loadTask.ContinueWith(
            AddressOf LoadingPayrollDataOnError,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )
    End Sub

    Private Sub LoadingPayrollDataOnSuccess(t As Task(Of PayrollResources))
        ThreadingPayrollGeneration(t.Result)
    End Sub

    Private Sub LoadingPayrollDataOnError(t As Task)
        _logger.Error("Error loading one of the payroll data.", t.Exception)
        MsgBox("Something went wrong while loading the payroll data needed for computation. Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Resources")
        Me.Enabled = True
    End Sub

    Private Sub ThreadingPayrollGeneration(resources As PayrollResources)
        _finishedPaystubs = 0

        Try
            Me.Enabled = False

            _totalPaystubs = resources.Employees.Count
            _successfulPaystubs = 0
            _failedPaystubs = 0
            _results = New BlockingCollection(Of PayrollGeneration.Result)()

            Task.Run(
                Sub()
                    Parallel.ForEach(
                        resources.Employees,
                        Sub(employee)
                            Dim generator = New PayrollGeneration(
                                employee,
                                resources,
                                Me
                            )

                            generator.DoProcess()
                        End Sub)
                End Sub)
        Catch ex As Exception
            _logger.Error("Error loading the employees", ex)
        End Try
    End Sub

    Private Sub SinglePaySlip_Click(sender As Object, e As EventArgs) Handles DeclaredToolStripMenuItem.Click,
                                                                                ActualToolStripMenuItem.Click
        Dim IsActualFlag = Convert.ToInt16(DirectCast(sender, ToolStripMenuItem).Tag)

        Dim n_PrintSinglePaySlipOfficialFormat As _
            New PrintSinglePaySlipOfficialFormat(ValNoComma(paypRowID),
                                                 IsActualFlag,
                                                 ValNoComma(dgvemployees.Tag))
    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Me.Close()
    End Sub

    Public genpayselyear As String = Nothing

    Sub btnrefresh_Click(sender As Object, e As EventArgs) Handles btnrefresh.Click
        VIEW_payperiodofyear()
    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem
        TabControlColor(TabControl1, e)
    End Sub

    Private Sub btntotallow_Click(sender As Object, e As EventArgs) Handles btntotallow.Click, btnTotalTaxabAllowance.Click
        viewtotloan.Close()
        viewtotbon.Close()

        With viewtotallow
            .Show()
            .BringToFront()
            If dgvemployees.RowCount > 0 Then

                .VIEW_allowanceperday(dgvemployees.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo,
                                        numofweekdays)

                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value
            End If
        End With
    End Sub

    Private Sub btntotbon_Click(sender As Object, e As EventArgs) Handles btntotbon.Click
        viewtotallow.Close()
        viewtotloan.Close()

        With viewtotbon
            .Show()
            .BringToFront()
            If dgvemployees.RowCount <> 0 Then
                .VIEW_employeebonus_indate(dgvemployees.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value
            End If
        End With

    End Sub

    Private Sub PayStub_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = False

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        showAuditTrail.Close()
        selectPayPeriod.Close()
        Dim open_forms = My.Application.OpenForms.Cast(Of Form).Where(Function(i) i.Name = "CrysVwr")
        If open_forms.Count > 0 Then
            Dim array_open_forms = open_forms.ToArray
            For Each frm In array_open_forms
                frm.Close()
            Next
        End If
        viewtotallow.Close()
        viewtotloan.Close()
        viewtotbon.Close()

        PayrollForm.listPayrollForm.Remove(Me.Name)
    End Sub

    Private Sub tsSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tsSearch.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        IsUserPressEnterToSearch = (e_asc = 13)
        If IsUserPressEnterToSearch Then
            Dim search_string_len = tsSearch.Text.Trim.Length
            IsUserPressEnterToSearch = (search_string_len > 0)
            tsbtnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub tsbtnSearch_Click(sender As Object, e As EventArgs) Handles tsbtnSearch.Click
        IsUserPressEnterToSearch = False

        If tsSearch.Text.Trim.Length = 0 Then
            First_LinkClicked(First, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))
        Else
            Dim dattabsearch As New DataTable

            pagination = 0

            Dim param_array = New Object() {orgztnID,
                                            tsSearch.Text,
                                            pagination,
                                            GetCurrentPayFrequencyType()}

            Dim n_ReadSQLProcedureToDatatable As New _
                SQL("CALL SEARCH_employee_paystub(?og_rowid, ?unified_search_string, ?page_number, ?text_pay_freq_sched);",
                    param_array)

            dattabsearch = n_ReadSQLProcedureToDatatable.GetFoundRows.Tables(0)

            dgvemployees.Rows.Clear()

            For Each drow As DataRow In dattabsearch.Rows
                Dim row_array = drow.ItemArray
                dgvemployees.Rows.Add(row_array)
            Next

            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            dgvemployees_SelectionChanged(sender, e)

            AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        End If
    End Sub

    Private Sub SplitContainer1_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        SplitContainer1.Panel2.Focus()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Image.Tag = 1 Then
            Button3.Image = Nothing
            Button3.Image = My.Resources.r_arrow
            Button3.Image.Tag = 0

            TabControl1.Show()
            dgvpayper.Width = 350

            dgvpayper_SelectionChanged(sender, e)
        Else
            Button3.Image = Nothing
            Button3.Image = My.Resources.l_arrow
            Button3.Image.Tag = 1

            TabControl1.Hide()
            Dim pointX As Integer = Width_resolution - (Width_resolution * 0.15)

            dgvpayper.Width = pointX
        End If
    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(viewID)
        showAuditTrail.BringToFront()
    End Sub

    Private Sub ActualToolStripMenuItem2_Click(sender As Object, e As EventArgs) _
        Handles _
        tsbtnDeclaredSummary.Click,
        tsbtnActualSummary.Click

        Dim psefr As New PayrollSummaryExcelFormatReportProvider

        Dim obj_sender = DirectCast(sender, ToolStripMenuItem)

        Dim is_actual As Boolean = (obj_sender.Name = tsbtnActualSummary.Name)

        psefr.IsActual = is_actual

        psefr.Run()

    End Sub

    Private Sub tbppayroll_Enter(sender As Object, e As EventArgs) Handles tbppayroll.Enter

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Dim payfrqncy As New AutoCompleteStringCollection

            Dim sel_query = ""

            Dim hasAnEmployee = EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE OrganizationID=" & orgztnID & " LIMIT 1);")

            If hasAnEmployee = 1 Then
                sel_query = "SELECT pp.PayFrequencyType FROM payfrequency pp INNER JOIN employee e ON e.PayFrequencyID=pp.RowID GROUP BY pp.RowID;"
            Else
                sel_query = "SELECT PayFrequencyType FROM payfrequency WHERE PayFrequencyType IN ('SEMI-MONTHLY','WEEKLY');"
            End If

            enlistTheLists(sel_query, payfrqncy)

            Dim first_sender As New ToolStripButton

            Dim indx = 0

            For Each strval In payfrqncy

                Dim new_tsbtn As New ToolStripButton

                With new_tsbtn

                    .AutoSize = False
                    .BackColor = Color.FromArgb(255, 255, 255)
                    .ImageTransparentColor = System.Drawing.Color.Magenta
                    .Margin = New System.Windows.Forms.Padding(0, 1, 0, 1)
                    .Name = String.Concat("tsbtn" & strval)
                    .Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
                    .Size = New System.Drawing.Size(110, 30)
                    .Text = strval
                    .TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                    .TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
                    .ToolTipText = strval

                End With

                tstrip.Items.Add(new_tsbtn)

                If indx = 0 Then
                    indx = 1
                    first_sender = new_tsbtn
                End If

                AddHandler new_tsbtn.Click, AddressOf PayFreq_Changed

            Next

            tstrip.PerformLayout()

            If first_sender IsNot Nothing Then
                PayFreq_Changed(first_sender, New EventArgs)
            End If

            For Each tsItem As ToolStripItem In tstrip.Items
                tsItem.PerformClick()
                Exit For
            Next

            dgvpayper_SelectionChanged(dgvpayper, New EventArgs)

            AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        End If

    End Sub

    Sub PayFreq_Changed(sender As Object, e As EventArgs)

        quer_empPayFreq = ""

        Dim senderObj As New ToolStripButton

        Static prevObj As New ToolStripButton

        Static once As SByte = 0

        senderObj = DirectCast(sender, ToolStripButton)

        If once = 0 Then
            once += 1

            prevObj = senderObj

            senderObj.BackColor = Color.FromArgb(194, 228, 255)

            senderObj.Font = selectedButtonFont

            quer_empPayFreq = " AND pf.PayFrequencyType='" & senderObj.Text & "' "

            loademployee(quer_empPayFreq)

            Exit Sub
        End If

        If prevObj.Name = Nothing Then
        Else

            If prevObj.Name <> senderObj.Name Then

                prevObj.BackColor = Color.FromArgb(255, 255, 255)

                prevObj.Font = unselectedButtonFont

                prevObj = senderObj

            End If

        End If

        senderObj.BackColor = Color.FromArgb(194, 228, 255)

        senderObj.Font = selectedButtonFont

        senderObj.Tag = 1

        Dim select_cutoff_payfrequency =
            tstrip.Items.OfType(Of ToolStripButton).Where(Function(tsbtn) tsbtn.Name <> senderObj.Name)

        For Each _tsbtn In select_cutoff_payfrequency
            _tsbtn.Tag = 0
        Next

        Dim prev_selRowIndex = -1

        If dgvemployees.RowCount <> 0 Then
            Try
                prev_selRowIndex = dgvemployees.CurrentRow.Index
            Catch ex As Exception
                prev_selRowIndex = -1
            End Try
        End If

        quer_empPayFreq = String.Empty

        tsbtnSearch_Click(sender, e)

    End Sub

    '********************************************************
    '*********             CONTEXT MENU             *********
    '********************************************************

    Private Sub cms1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cms1.Opening

        If dgvpayper.RowCount <> 0 Then
            ToolStripMenuItem1.Enabled = True
        Else
            ToolStripMenuItem1.Enabled = False

        End If

    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

        If dgvpayper.RowCount <> 0 Then

            With dgvpayper.CurrentRow

                paypRowID = .Cells("Column1").Value

                paypFrom = Format(CDate(.Cells("Column2").Value), "yyyy-MM-dd")

                paypTo = Format(CDate(.Cells("Column3").Value), "yyyy-MM-dd")

                isEndOfMonth = Trim(.Cells("Column14").Value)

                genpayselyear = Format(CDate(.Cells("Column2").Value), "yyyy")

                numofweekdays = 0

                numofweekends = 0

                Dim date_diff = DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo))

                For i = 0 To date_diff

                    Dim DayOfWeek = CDate(paypFrom).AddDays(i)

                    If DayOfWeek.DayOfWeek = 0 Then 'System.DayOfWeek.Sunday
                        numofweekends += 1

                    ElseIf DayOfWeek.DayOfWeek = 6 Then 'System.DayOfWeek.Saturday
                        numofweekends += 1
                    Else
                        numofweekdays += 1

                    End If

                Next

                withthirteenthmonthpay = 0

                If Format(CDate(.Cells("Column3").Value), "MM") = "12" Then

                    Dim prompt = MessageBox.Show("Do you want to include the calculation of Thirteenth month pay ?", "Thirteenth month pay calculation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information)

                    If prompt = Windows.Forms.DialogResult.Yes Then
                    ElseIf prompt = Windows.Forms.DialogResult.No Then
                    ElseIf prompt = Windows.Forms.DialogResult.Cancel Then
                    End If
                Else

                End If

                Dim PayFreqRowID = EXECQUER("SELECT RowID FROM payfrequency WHERE PayFrequencyType='" & Trim(.Cells("Column12").Value) & "';")

            End With
        End If
    End Sub

    Private Sub dgAdjustments_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAdjustments.CellContentClick
        If TypeOf dgvAdjustments.Columns(e.ColumnIndex) Is DataGridViewLinkColumn _
            AndAlso e.RowIndex >= 0 Then
            Dim n_ExecuteQuery As New ExecuteQuery("SELECT PartNo FROM product WHERE RowID='" & dgvAdjustments.Item("cboProducts", e.RowIndex).Value & "' LIMIT 1;")
            Dim item_name As String = n_ExecuteQuery.Result
            Dim prompt = MessageBox.Show("Are you sure you want to delete '" & item_name & "'" & If(item_name.ToLower.Contains("adjustment"), "", " adjustment") & " ?",
                                         "Delete adjustment", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If prompt = Windows.Forms.DialogResult.Yes Then
                Dim SQLTableName As String = If(dgvAdjustments.Item("IsAdjustmentActual", e.RowIndex).Value = 0, "paystubadjustment", "paystubadjustmentactual")
                Dim del_quer As String =
                    "DELETE FROM " & SQLTableName & " WHERE RowID='" & dgvAdjustments.Item("psaRowID", e.RowIndex).Value & "';"
                n_ExecuteQuery = New ExecuteQuery(del_quer)
                dgvAdjustments.Rows.Remove(dgvAdjustments.Rows(e.RowIndex))
                dgvemployees_SelectionChanged(dgvemployees, New EventArgs)
                btnSaveAdjustments.Enabled = False
            End If
        End If
    End Sub

    Sub btnSaveAdjustments_Click(sender As Object, e As EventArgs) Handles btnSaveAdjustments.Click
        dgvAdjustments.EndEdit(True)

        Dim hasError As Boolean = False
        Dim errorRow As New DataGridViewRow
        Dim lastRow As Integer = dgvAdjustments.Rows.Count

        Dim rowCount As Integer = 0

        Dim comment_columnname = "DataGridViewTextBoxColumn64"
        If Not hasError Then
            Try
                Dim SQLFunctionName As String = If(ValNoComma(tabEarned.SelectedIndex) = 0, "I_paystubadjustment", "I_paystubadjustmentactual")
                For Each dgvRow As DataGridViewRow In dgvAdjustments.Rows
                    Dim productRowID = dgvRow.Cells("cboProducts").Value
                    If productRowID IsNot Nothing And dgvRow.IsNewRow = False Then 'If Not dgvRow.Cells(0).Value Is Nothing AndAlso Not dgvRow.Cells(1).Value Is Nothing AndAlso IsNumeric(dgvRow.Cells(1).Value) Then
                        If TypeOf dgvRow.Cells("cboProducts").Value Is String Then
                            productRowID =
                            EXECQUER("SELECT RowID FROM product WHERE OrganizationID='" & orgztnID & "' AND PartNo='" & dgvRow.Cells("cboProducts").Value & "' LIMIT 1;")
                        End If
                        Dim n_ReadSQLFunction As _
                            New ReadSQLFunction(SQLFunctionName,
                                                "returnvalue",
                                                orgztnID,
                                                z_User,
                                                productRowID,
                                                ValNoComma(dgvRow.Cells("DataGridViewTextBoxColumn66").Value),
                                                dgvRow.Cells("DataGridViewTextBoxColumn64").Value,
                                                Me.currentEmployeeID,
                                                paypRowID,
                                                dgvRow.Cells("psaRowID").Value)
                        If ValNoComma(dgvRow.Cells("psaRowID").Value) = 0 And n_ReadSQLFunction.HasError = False Then
                            dgvRow.Cells("psaRowID").Value = n_ReadSQLFunction.ReturnValue
                        End If
                    Else : Continue For
                    End If
                Next
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Me.Name))
            Finally
                UpdatePaystubAdjustmentColumn()
                MsgBox("Adjustments were saved!", MsgBoxStyle.Information)
                dgvemployees_SelectionChanged(sender, e)
            End Try
        End If
    End Sub

    Private Sub btntotloan_Click(sender As Object, e As EventArgs) Handles btntotloan.Click
        viewtotallow.Close()
        viewtotbon.Close()

        With viewtotloan
            .Show()
            .BringToFront()

            If dgvemployees.RowCount <> 0 Then
                .VIEW_employeeloan_indate(dgvemployees.CurrentRow.Cells("RowID").Value, paypFrom, paypTo)
                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value
            End If
        End With
    End Sub

    Private Sub UpdatePaystubAdjustmentColumn()
        Dim _conn As New MySqlConnection
        _conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString
        Try
            If _conn.State = ConnectionState.Open Then : _conn.Close() : End If
            _conn.Open()

            cmd = New MySqlCommand("SP_UpdatePaystubAdjustment", _conn)

            With cmd
                .Parameters.Clear()
                .CommandType = CommandType.StoredProcedure

                .Parameters.AddWithValue("pa_EmployeeID", Me.currentEmployeeID)
                .Parameters.AddWithValue("pa_PayPeriodID", dgvpayper.SelectedRows(0).Cells(0).Value)
                .Parameters.AddWithValue("User_RowID", z_User)
                .Parameters.AddWithValue("Og_RowID", z_OrganizationID)
            End With

            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            _conn.Close()
            cmd.Dispose()
        End Try
    End Sub

    Private Sub UpdateAdjustmentDetails(Optional IsActual As Boolean = False)
        Try
            dtJosh = New DataTable
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            conn.Open()

            cmd = New MySqlCommand("VIEW_paystubadjustment", conn)
            With cmd
                .Parameters.Clear()
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("pa_EmployeeID", Me.currentEmployeeID)
                If dgvpayper.RowCount > 0 And dgvpayper.SelectedRows.Count > 0 Then
                    .Parameters.AddWithValue("pa_PayPeriodID", dgvpayper.SelectedRows(0).Cells(0).Value)
                Else
                    .Parameters.AddWithValue("pa_PayPeriodID", DBNull.Value)
                End If
                .Parameters.AddWithValue("pa_OrganizationID", orgztnID)
                .Parameters.AddWithValue("pa_IsActual", Convert.ToInt16(IsActual))
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(dtJosh)
            dgvAdjustments.DataSource = dtJosh
        Catch ex As Exception
            dgvAdjustments.Rows.Clear()
        Finally
            conn.Close()
            cmd.Dispose()
            da.Dispose()
        End Try
    End Sub

    Private Sub dgvAdjustments_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvAdjustments.DataError
        btnSaveAdjustments.Enabled = False
    End Sub

    Private Sub dgvAdjustments_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAdjustments.CellEndEdit
        UpdateSaveAdjustmentButtonDisable()
    End Sub

    Private Sub UpdateSaveAdjustmentButtonDisable()
        Dim hasError As Boolean = False

        btnSaveAdjustments.Enabled = Not hasError
    End Sub

    Private Sub tabEarned_DrawItem(sender As Object, e As DrawItemEventArgs) Handles tabEarned.DrawItem
        TabControlColor(tabEarned, e)
    End Sub

    Private Sub ResetNumberTextBox()
        For Each txtbx In Panel6.Controls.OfType(Of TextBox)
            txtbx.Text = "0.00"
        Next
    End Sub

    Private Sub NumberFieldFormatter(sender As Object, e As EventArgs)
        If TypeOf sender Is TextBox Then
            Dim txtboxSender = DirectCast(sender, TextBox)
            txtboxSender.Text = FormatNumber(txtboxSender.Text, 2)
        End If
    End Sub

    Private Sub TabPage1_Enter1(sender As Object, e As EventArgs) Handles TabPage1.Enter 'DECLARED
        For Each txtbxctrl In TabPage1.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag

        Dim _params = New Object() {orgztnID,
                                    EmployeeRowID,
                                    paypFrom,
                                    paypTo}

        Dim n_SQLQueryToDatatable As _
            New SQL("CALL VIEW_paystubitem_declared(?OrganizID, ?EmpRowID, ?pay_date_from, ?pay_date_to);",
                    _params)

        Dim paystubactual = n_SQLQueryToDatatable.GetFoundRows.Tables(0)

        Dim txtbxField = Panel6.Controls.OfType(Of TextBox).Where(Function(txbx) String.IsNullOrWhiteSpace(txbx.AccessibleDescription) = False)

        For Each drow As DataRow In paystubactual.Rows

            Dim psaItems = New SQL("CALL VIEW_paystubitem('" & ValNoComma(drow("RowID")) & "');").GetFoundRows.Tables(0)

            Dim strdouble = ValNoComma(drow("TrueSalary")) / ValNoComma(drow("PAYFREQUENCYDIVISOR")) 'BasicPay

            txtBasicPay.Text = FormatNumber(ValNoComma(strdouble), 2)

            txtRegularHours.Text = ValNoComma(drow("RegularHours"))

            If drow("EmployeeType").ToString = "Fixed" Then
                txtRegularPay.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then
                Dim basicPay = ValNoComma(drow("BasicPay"))
                Dim deductions = 0.0

                If drow("FirstTimeSalary").ToString = "True" Then
                    basicPay = ValNoComma(drow("RegularPay"))
                Else
                    basicPay = ValNoComma(drow("BasicPay"))
                    deductions = ValNoComma(drow("LateDeduction")) +
                    ValNoComma(drow("UndertimeDeduction")) +
                    ValNoComma(drow("Absent")) '+
                    'ValNoComma(drow("HolidayPay"))
                End If

                txtRegularPay.Text = FormatNumber(basicPay - deductions, 2)
            ElseIf drow("EmployeeType").ToString = "Daily" Then
                txtRegularPay.Text = FormatNumber(ValNoComma(drow("RegularPay")), 2)
            End If

            txtOvertimeHours.Text = ValNoComma(drow("OvertimeHours"))
            txtOvertimePay.Text = FormatNumber(ValNoComma(drow("OvertimePay")), 2)

            txtNightDiffHours.Text = ValNoComma(drow("NightDiffHours"))
            txtNightDiffPay.Text = FormatNumber(ValNoComma(drow("NightDiffPay")), 2)

            txtNightDiffOvertimeHours.Text = ValNoComma(drow("NightDiffOvertimeHours"))
            txtNightDiffOvertimePay.Text = FormatNumber(ValNoComma(drow("NightDiffOvertimePay")), 2)

            txtRestDayHours.Text = ValNoComma(drow("RestDayHours"))
            txtRestDayAmount.Text = FormatNumber(ValNoComma(drow("RestDayPay")), 2)

            txtHolidayHours.Text = 0.0
            txtHolidayPay.Text = FormatNumber(ValNoComma(drow("HolidayPay")), 2)

            Dim sumallbasic = ValNoComma(drow("RegularPay")) +
                          ValNoComma(drow("OvertimePay")) +
                          ValNoComma(drow("NightDiffPay")) +
                          ValNoComma(drow("NightDiffOvertimePay")) +
                          ValNoComma(drow("HolidayPay"))

            If drow("EmployeeType").ToString = "Fixed" Then
                lblSubtotal.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then
                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "True" Then
                    thebasicpay = ValNoComma(drow("RegularPay"))
                    lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
                Else
                    thebasicpay = ValNoComma(drow("BasicPay"))
                    thelessamounts = ValNoComma(drow("LateDeduction")) + ValNoComma(drow("UndertimeDeduction")) + ValNoComma(drow("Absent"))
                    'Dim all_regular = (thebasicpay - (thelessamounts + ValNoComma(drow("HolidayPay"))))
                    Dim all_regular = (thebasicpay - thelessamounts)
                    lblSubtotal.Text =
                    FormatNumber(all_regular + ValNoComma(drow("HolidayPay")) +
                                 ValNoComma(drow("OvertimePay")) +
                                 ValNoComma(drow("NightDiffPay")) +
                                 ValNoComma(drow("NightDiffOvertimePay")), 2)
                End If
            Else
                lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)

            End If

            'Absent
            txttotabsent.Text = FormatNumber(ValNoComma((drow("AbsentHours"))), 2)
            txttotabsentamt.Text = FormatNumber(ValNoComma((drow("Absent"))), 2)
            'Tardiness / late
            txttottardi.Text = ValNoComma(drow("LateHours"))
            txttottardiamt.Text = FormatNumber(ValNoComma((drow("LateDeduction"))), 2)
            'Undertime
            txttotut.Text = ValNoComma(drow("UndertimeHours"))
            txttotutamt.Text = FormatNumber(ValNoComma((drow("UndertimeDeduction"))), 2)

            Dim miscsubtotal = ValNoComma(drow("Absent")) + ValNoComma(drow("LateDeduction")) + ValNoComma(drow("UndertimeDeduction"))
            lblsubtotmisc.Text = FormatNumber(ValNoComma((miscsubtotal)), 2)

            'Allowance
            txtemptotallow.Text = FormatNumber(ValNoComma((drow("TotalAllowance"))), 2)

            'Taxable Allowance
            txtTotTaxabAllow.Text = FormatNumber(ValNoComma((drow("TotalTaxableAllowance"))), 2)

            txtGrandTotalAllow.Text = FormatNumber(ValNoComma(drow("TotalAllowance")) + ValNoComma(drow("TotalTaxableAllowance")), 2)

            'Bonus
            txtemptotbon.Text = FormatNumber(ValNoComma((drow("TotalBonus"))), 2)
            'Gross
            txtgrosssal.Text = FormatNumber(ValNoComma((drow("TotalGrossSalary"))), 2)

            'SSS
            txtempsss.Text = FormatNumber(ValNoComma((drow("TotalEmpSSS"))), 2)
            'PhilHealth
            txtempphh.Text = FormatNumber(ValNoComma((drow("TotalEmpPhilhealth"))), 2)
            'PAGIBIG
            txtemphdmf.Text = FormatNumber(ValNoComma((drow("TotalEmpHDMF"))), 2)

            'Taxable salary
            txttaxabsal.Text = FormatNumber(ValNoComma((drow("TotalTaxableSalary"))), 2)
            'Withholding taxS
            txtempwtax.Text = FormatNumber(ValNoComma((drow("TotalEmpWithholdingTax"))), 2)
            'Loans
            txtemptotloan.Text = FormatNumber(ValNoComma((drow("TotalLoans"))), 2)
            'Adjustments
            txtTotalAdjustments.Text = FormatNumber(ValNoComma((drow("TotalAdjustments"))), 2)

            Dim totalAgencyFee = ValNoComma(drow("TotalAgencyFee"))
            txtAgencyFee.Text = FormatNumber(totalAgencyFee, 2)

            Dim thirteenthMonthPay = ValNoComma(drow("ThirteenthMonthPay"))
            txtThirteenthMonthPay.Text = FormatNumber(thirteenthMonthPay, 2)

            Dim totalNetSalary = ValNoComma(drow("TotalNetSalary")) + totalAgencyFee
            'Net
            txtnetsal.Text = FormatNumber(totalNetSalary, 2)

            Dim totalNetPay = totalNetSalary + thirteenthMonthPay
            txtTotalNetPay.Text = FormatNumber(totalNetPay, 2)

            'LEAVE BALANCES
            txtvlbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Vacation leave'")) ' -
            txtslbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Sick leave'")) ' -
            txtmlbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Maternity/paternity leave'")) ' -
            txtPaidLeave.Text = FormatNumber(ValNoComma(drow("PaidLeaveAmount")), 2)

            For Each txtbx In txtbxField
                txtbx.Text = If(IsDBNull(drow(txtbx.AccessibleDescription)), 0.0, drow(txtbx.AccessibleDescription))
            Next

            Exit For
        Next

        paystubactual.Dispose()
        UpdateAdjustmentDetails(Convert.ToInt16(DirectCast(sender, TabPage).Tag))
    End Sub

    Private Sub TabPage1_Enter(sender As Object, e As EventArgs) Handles TabPage1.Enter 'DECLARED
        TabPage1.Text = TabPage1.Text.Trim
        TabPage1.Text = TabPage1.Text & Space(15)
        TabPage4.Text = TabPage4.Text.Trim
    End Sub

    Private Sub TabPage4_Enter1(sender As Object, e As EventArgs) Handles TabPage4.Enter 'UNDECLARED / ACTUAL
        TabPage4.Text = TabPage4.Text.Trim
        TabPage4.Text = TabPage4.Text & Space(15)
        TabPage1.Text = TabPage1.Text.Trim

        For Each txtbxctrl In TabPage4.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag

        Dim _params = New Object() {
            orgztnID,
            EmployeeRowID,
            paypFrom,
            paypTo
        }

        Dim n_SQLQueryToDatatable = New SQL("CALL VIEW_paystubitem_actual(?OrganizID, ?EmpRowID, ?pay_date_from, ?pay_date_to);", _params)

        Dim paystubactual As New DataTable

        paystubactual = n_SQLQueryToDatatable.GetFoundRows.Tables(0)

        'Dim psaItems As New DataTable

        Dim txtbxField = Panel6.Controls.OfType(Of TextBox).Where(Function(txbx) String.IsNullOrWhiteSpace(txbx.AccessibleDescription) = False)

        For Each drow As DataRow In paystubactual.Rows

            'psaItems = New SQL("CALL VIEW_paystubitemundeclared('" & ValNoComma(drow("RowID")) & "');").GetFoundRows.Tables(0)

            Dim strdouble = ValNoComma(drow("BasicPay")) 'BasicPay

            ' Basic Pay
            txtempbasicpay_U.Text = FormatNumber(ValNoComma(strdouble), 2)

            ' Regular hours
            txthrswork_U.Text = ValNoComma(drow("RegularHours"))

            ' Regular pay
            If drow("EmployeeType").ToString = "Fixed" Then

                txthrsworkamt_U.Text = FormatNumber(ValNoComma(strdouble), 2)

            ElseIf drow("EmployeeType").ToString = "Monthly" Then

                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "True" Then
                    thebasicpay = ValNoComma(drow("RegularPay"))
                Else
                    thebasicpay = ValNoComma(drow("RegularPay"))
                End If

                txthrsworkamt_U.Text = FormatNumber((thebasicpay - thelessamounts), 2)
            Else
                txthrsworkamt_U.Text = FormatNumber(ValNoComma((drow("RegularPay"))), 2)
            End If

            ' Over time
            txttotothrs_U.Text = ValNoComma(drow("OvertimeHours"))
            txttototamt_U.Text = FormatNumber(ValNoComma((drow("OvertimePay"))), 2)

            ' Night differential
            txttotnightdiffhrs_U.Text = ValNoComma(drow("NightDiffHours"))
            txttotnightdiffamt_U.Text = FormatNumber(ValNoComma((drow("NightDiffPay"))), 2)

            ' Night differential Overtime
            txttotnightdiffothrs_U.Text = ValNoComma(drow("NightDiffOvertimeHours"))
            txttotnightdiffotamt_U.Text = FormatNumber(ValNoComma((drow("NightDiffOvertimePay"))), 2)

            If drow("EmployeeType").ToString = "Fixed" Then
                lblSubtotal.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then

                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "True" Then
                    thebasicpay = ValNoComma(drow("RegularPay"))

                    lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
                Else
                    thebasicpay = ValNoComma(drow("BasicPay"))
                    thelessamounts = ValNoComma(drow("LateDeduction")) + ValNoComma(drow("UndertimeDeduction")) + ValNoComma(drow("AbsenceDeduction"))

                    Dim all_regular = (thebasicpay - thelessamounts)
                    lblSubtotal.Text = FormatNumber(
                        all_regular +
                        ValNoComma(drow("SpecialHolidayPay")) +
                        ValNoComma(drow("SpecialHolidayOTPay")) +
                        ValNoComma(drow("RegularHolidayPay")) +
                        ValNoComma(drow("RegularHolidayOTPay")) +
                        ValNoComma(drow("OvertimePay")) +
                        ValNoComma(drow("NightDiffPay")) +
                        ValNoComma(drow("NightDiffOvertimePay")),
                        2)
                End If
            Else
                lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
            End If

            'Absent
            txttotabsent_U.Text = FormatNumber(ValNoComma((drow("AbsentHours"))), 2)
            txttotabsentamt_U.Text = FormatNumber(ValNoComma((drow("AbsenceDeduction"))), 2)

            'Tardiness / late
            txttottardi_U.Text = ValNoComma(drow("LateHours"))
            txttottardiamt_U.Text = FormatNumber(ValNoComma((drow("LateDeduction"))), 2)

            'Undertime
            txttotut_U.Text = ValNoComma(drow("UndertimeHours"))
            txttotutamt_U.Text = FormatNumber(ValNoComma((drow("UndertimeDeduction"))), 2)

            Dim miscsubtotal = ValNoComma(drow("AbsenceDeduction")) + ValNoComma(drow("LateDeduction")) + ValNoComma(drow("UndertimeDeduction"))
            lblsubtotmisc.Text = FormatNumber(ValNoComma((miscsubtotal)), 2)

            'Allowance
            txtemptotallow.Text = FormatNumber(ValNoComma((drow("TotalAllowance"))), 2)

            'Taxable Allowance
            txtTotTaxabAllow.Text = FormatNumber(ValNoComma((drow("TotalTaxableAllowance"))), 2)

            txtGrandTotalAllow.Text = FormatNumber(ValNoComma(drow("TotalAllowance")) + ValNoComma(drow("TotalTaxableAllowance")), 2)

            'Bonus
            txtemptotbon.Text = FormatNumber(ValNoComma((drow("TotalBonus"))), 2)
            'Gross
            txtgrosssal.Text = FormatNumber(ValNoComma((drow("TotalGrossSalary"))), 2)

            ' SSS
            txtempsss.Text = FormatNumber(ValNoComma((drow("TotalEmpSSS"))), 2)
            ' PhilHealth
            txtempphh.Text = FormatNumber(ValNoComma((drow("TotalEmpPhilhealth"))), 2)
            ' PAGIBIG
            txtemphdmf.Text = FormatNumber(ValNoComma((drow("TotalEmpHDMF"))), 2)

            ' Taxable salary
            txttaxabsal.Text = FormatNumber(ValNoComma((drow("TotalTaxableSalary"))), 2)
            ' Withholding taxS
            txtempwtax.Text = FormatNumber(ValNoComma((drow("TotalEmpWithholdingTax"))), 2)
            ' Loans
            txtemptotloan.Text = FormatNumber(ValNoComma((drow("TotalLoans"))), 2)
            ' Adjustments
            txtTotalAdjustments.Text = FormatNumber(ValNoComma((drow("TotalAdjustments"))), 2)

            ' Agency fee
            Dim totalAgencyFee = ValNoComma(drow("TotalAgencyFee"))
            txtAgencyFee.Text = FormatNumber(totalAgencyFee, 2)

            Dim thirteenthMonthPay = ValNoComma(drow("ThirteenthMonthPay"))
            txtThirteenthMonthPay.Text = FormatNumber(thirteenthMonthPay, 2)

            Dim totalNetSalary = ValNoComma(drow("TotalNetSalary")) + totalAgencyFee
            'Net
            txtnetsal.Text = FormatNumber(totalNetSalary, 2)

            Dim totalNetPay = totalNetSalary + thirteenthMonthPay
            txtTotalNetPay.Text = FormatNumber(totalNetPay, 2)

            txtPaidLeave.Text = FormatNumber(ValNoComma(drow("LeavePay")), 2)

            For Each txtbx In txtbxField
                txtbx.Text = If(IsDBNull(drow(txtbx.AccessibleDescription)), 0.0, drow(txtbx.AccessibleDescription))
            Next

            Exit For
        Next

        paystubactual.Dispose()

        UpdateAdjustmentDetails(Convert.ToInt16(DirectCast(sender, TabPage).Tag))

    End Sub

    Private Sub TabPage4_Enter(sender As Object, e As EventArgs) 'Handles TabPage4.Enter 'UNDECLARED
        TabPage4_Enter1(TabPage4, New EventArgs)
    End Sub

    Private Sub bgwPrintAllPaySlip_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwPrintAllPaySlip.DoWork

        If dtprintAllPaySlip.Columns.Count = 0 Then

            For i = 1 To 120

                Dim n_col As New DataColumn

                n_col.ColumnName = "COL" & i

                dtprintAllPaySlip.Columns.Add(n_col)

            Next

        End If

        employee_dattab = retAsDatTbl("SELECT e.*" &
                                      ",CONCAT(UCASE(e.LastName),', ',UCASE(e.FirstName),', ',INITIALS(e.MiddleName,'.','1')) AS FullName" &
                                      " FROM employee e LEFT JOIN employeesalary esal ON e.RowID=esal.EmployeeID" &
                                      " WHERE e.OrganizationID=" & orgztnID &
                                      " AND CURDATE() BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,CURDATE())" &
                                      " GROUP BY e.RowID" &
                                      " ORDER BY e.LastName;") 'RowID DESC

        Dim n_row As DataRow

        For Each drow As DataRow In employee_dattab.Rows

            n_row = dtprintAllPaySlip.NewRow

            n_row("COL1") = drow("EmployeeID")

            n_row("COL2") = drow("FullName")

            For ii = 3 To 120

                Dim datacol_name = "COL" & ii

                n_row(datacol_name) = "0.0"

            Next

            dtprintAllPaySlip.Rows.Add(n_row)

        Next

        rptdocAll = New rptAllDecUndecPaySlip

        With rptdocAll.ReportDefinition.Sections(2)

            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("OrgName")

            objText.Text = orgNam

            objText = .ReportObjects("OrgAddress")

            Dim orgaddress = EXECQUER("SELECT CONCAT(IF(StreetAddress1 IS NULL,'',StreetAddress1)" &
                                    ",IF(StreetAddress2 IS NULL,'',CONCAT(', ',StreetAddress2))" &
                                    ",IF(Barangay IS NULL,'',CONCAT(', ',Barangay))" &
                                    ",IF(CityTown IS NULL,'',CONCAT(', ',CityTown))" &
                                    ",IF(Country IS NULL,'',CONCAT(', ',Country))" &
                                    ",IF(State IS NULL,'',CONCAT(', ',State)))" &
                                    " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" &
                                    " WHERE o.RowID=" & orgztnID & ";")

            objText.Text = orgaddress

            Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                                    ",',',COALESCE(FaxNumber,'')" &
                                    ",',',COALESCE(EmailAddress,'')" &
                                    ",',',COALESCE(TINNo,''))" &
                                    " FROM organization WHERE RowID=" & orgztnID & ";")

            Dim contactdet = Split(contactdetails, ",")
            objText = .ReportObjects("OrgContact")

            Dim contactdets As String = Nothing

            If Trim(contactdet(0).ToString) = "" Then
                contactdets = ""
            Else
                contactdets = "Contact No. " & contactdet(0).ToString
            End If

            objText.Text = contactdets
            objText = .ReportObjects("payperiod")

            Dim papy_str = "Payroll slip for the period of   " & Format(CDate(Now), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(Now), machineShortDateFormat))
            objText.Text = papy_str
        End With

        rptdocAll.SetDataSource(dtprintAllPaySlip)
    End Sub

    Private Sub bgwPrintAllPaySlip_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwPrintAllPaySlip.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MsgBox("Error: " & vbNewLine & e.Error.Message)
        ElseIf e.Cancelled Then
            MsgBox("Background work cancelled.",
                   MsgBoxStyle.Exclamation)
        Else
            Dim crvwr As New CrysVwr

            crvwr.CrystalReportViewer1.ReportSource = rptdocAll
            Dim papy_string = "Payroll slip for the period of   " & Format(CDate(Now), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(Now), machineShortDateFormat))
            crvwr.Text = papy_string
            crvwr.Refresh()
            crvwr.Show()
        End If
    End Sub

    Private Sub btndiscardchanges_Click(sender As Object, e As EventArgs) Handles btndiscardchanges.Click
        UpdateAdjustmentDetails(Convert.ToInt16(tabEarned.SelectedIndex)) 'Josh

        btnSaveAdjustments.Enabled = False
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked

        Dim n_ProdCtrlForm As New ProdCtrlForm

        With n_ProdCtrlForm

            .Status.HeaderText = "Taxable Flag"

            .PartNo.HeaderText = "Item Name"

            .NameOfCategory = "Adjustment Type"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                cboProducts.DataSource = Nothing

                cboProducts.ValueMember = "ProductID"
                cboProducts.DisplayMember = "ProductName"

                cboProducts.DataSource = New SQLQueryToDatatable("SELECT RowID AS 'ProductID', Name AS 'ProductName', Category FROM product WHERE Category IN ('Allowance Type', 'Bonus', 'Adjustment Type')" &
                                                                 " AND OrganizationID='" & orgztnID & "' ORDER BY Name;").ResultTable

            End If

        End With

    End Sub

    Private Sub Gender_Label(ByVal strGender As String)

        If strGender.Trim.Length > 0 Then

            Dim label_output As String = ""

            If strGender = "Male" Then
                label_output = "Paternity"
            Else
                label_output = "Maternity"
            End If

            Label149.Text = label_output

            Label148.Text = label_output

            Label152.Text = label_output

        End If

    End Sub

    Private Sub tsbtnDelEmpPayroll_Click(sender As Object, e As EventArgs) Handles tsbtnDelEmpPayroll.Click

        If currentEmployeeID = Nothing Then
        Else

            tsbtnDelEmpPayroll.Enabled = False

            Dim prompt = MessageBox.Show("Do you want to delete this '" & CDate(paypFrom).ToShortDateString &
                                         "' to '" & CDate(paypTo).ToShortDateString &
                                         "' payroll of employee '" & currentEmployeeID & "' ?",
                                         "Delete employee payroll",
                                         MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            If prompt = Windows.Forms.DialogResult.Yes Then

                Dim n_ExecuteQuery As New ExecuteQuery("SELECT RowID" &
                                                       " FROM paystub" &
                                                       " WHERE EmployeeID='" & dgvemployees.Tag & "'" &
                                                       " AND OrganizationID='" & orgztnID & "'" &
                                                       " AND PayPeriodID='" & paypRowID & "'" &
                                                       " LIMIT 1;")

                Dim paystubRowID As Object = Nothing

                paystubRowID = n_ExecuteQuery.Result

                If paystubRowID IsNot Nothing Then
                    n_ExecuteQuery = New ExecuteQuery("CALL DEL_specificpaystub('" & paystubRowID & "');")
                End If


            End If

            tsbtnDelEmpPayroll.Enabled = True

        End If

    End Sub

    Private Sub dgvemployees_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvemployees.RowsRemoved
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

    Private Sub PrintAllPaySlip_Click(sender As Object, e As EventArgs) Handles DeclaredToolStripMenuItem1.Click,
                                                                                ActualToolStripMenuItem1.Click
        Dim IsActualFlag = Convert.ToInt16(DirectCast(sender, ToolStripMenuItem).Tag)

        Dim n_PrintAllPaySlipOfficialFormat As _
            New PrintAllPaySlipOfficialFormat(ValNoComma(paypRowID),
                                              IsActualFlag)
    End Sub

    Private Sub IncludeThirteenthMonthButton_Click(sender As Object, e As EventArgs) Handles Incude13thMonthPayToolStripMenuItem.Click
        Dim payPeriodSelector = New PayrollSummaDateSelection()

        If payPeriodSelector.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim dateFrom = payPeriodSelector.DateFrom
        Dim dateTo = payPeriodSelector.DateTo

        Dim realse = New ReleaseThirteenthMonthPay(dateFrom, dateTo, paypRowID)
    End Sub

    Sub ProgressCounter(result As PayrollGeneration.Result)
        If result.Status = PayrollGeneration.ResultStatus.Success Then
            Interlocked.Increment(_successfulPaystubs)
        Else
            Interlocked.Increment(_failedPaystubs)
        End If

        Interlocked.Increment(_finishedPaystubs)
        _results.Add(result)

        Dim percentComplete As Integer = (_finishedPaystubs / _totalPaystubs) * 100
        MDIPrimaryForm.systemprogressbar.Value = percentComplete

        If _finishedPaystubs = _totalPaystubs Then
            Dim param_array = New Object() {orgztnID, paypRowID, z_User}

            Static strquery_recompute_13monthpay As String =
                "call recompute_thirteenthmonthpay(?organizid, ?payprowid, ?userrowid);"

            Dim n_ExecSQLProcedure = New SQL(strquery_recompute_13monthpay, param_array)
            n_ExecSQLProcedure.ExecuteQuery()

            Dim dialog = New PayrollResultDialog(_results.ToList()) With {
                .Owner = Me
            }
            dialog.ShowDialog()

            Me.Enabled = True
            dgvpayper_SelectionChanged(dgvpayper, New EventArgs)
        End If

        Console.WriteLine(String.Concat("#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@ ", percentComplete, "% complete"))
    End Sub

    Private Sub PopulateDGVEmployee(dat_tbl As DataTable)
        For Each drow As DataRow In dat_tbl.Rows
            dgvemployees.Rows.Add(drow("RowID"),
                                  drow("EmployeeID"),
                                  drow("FirstName"),
                                  drow("MiddleName"),
                                  drow("LastName"),
                                  drow("Surname"),
                                  drow("Nickname"),
                                  drow("MaritalStatus"),
                                  drow("NoOfDependents"),
                                  Format(CDate(drow("Birthdate")), machineShortDateFormat),
                                  Format(CDate(drow("StartDate")), machineShortDateFormat),
                                  drow("JobTitle"),
                                  If(IsDBNull(drow("PositionName")), "", drow("PositionName")),
                                  drow("Salutation"),
                                  drow("TINNo"),
                                  drow("SSSNo"),
                                  drow("HDMFNo"),
                                  drow("PhilHealthNo"),
                                  drow("WorkPhone"),
                                  drow("HomePhone"),
                                  drow("MobilePhone"),
                                  drow("HomeAddress"),
                                  drow("EmailAddress"),
                                  If(Trim(drow("Gender")) = "M", "Male", "Female"),
                                  drow("EmploymentStatus"),
                                  drow("PayFrequencyType"),
                                  drow("UndertimeOverride"),
                                  drow("OvertimeOverride"),
                                  If(IsDBNull(drow("PositionID")), "", drow("PositionID")),
                                  drow("PayFrequencyID"),
                                  drow("EmployeeType"),
                                  drow("LeaveBalance"),
                                  drow("SickLeaveBalance"),
                                  drow("MaternityLeaveBalance"),
                                  drow("LeaveAllowance"),
                                  drow("SickLeaveAllowance"),
                                  drow("MaternityLeaveAllowance"),
                                  drow("LeavePerPayPeriod"),
                                  drow("SickLeavePerPayPeriod"),
                                  drow("MaternityLeavePerPayPeriod"),
                                  drow("fstatRowID"),
                                  Nothing,
                                  drow("Created"),
                                  drow("CreatedBy"),
                                  If(IsDBNull(drow("LastUpd")), "", drow("LastUpd")),
                                  If(IsDBNull(drow("LastUpdBy")), "", drow("LastUpdBy")))
        Next

    End Sub

    Private Sub setProperInterfaceBaseOnCurrentSystemOwner()
        Static _bool As Boolean =
            (sys_ownr.CurrentSystemOwner = SystemOwner.Cinema2000)

        If _bool Then

            Dim str_empty As String = String.Empty

            TabPage1.Text = str_empty

            TabPage4.Text = str_empty

            AddHandler tabEarned.Selecting, AddressOf tabEarned_Selecting
        Else

        End If

    End Sub

    Private Sub tabEarned_Selecting(sender As Object, e As TabControlCancelEventArgs)

        Static _bool As Boolean =
            (sys_ownr.CurrentSystemOwner = SystemOwner.Cinema2000)

        e.Cancel = _bool

    End Sub

    Private Function GetCurrentPayFrequencyType() As String

        Dim select_cutoff_payfrequency =
                tstrip.Items.OfType(Of ToolStripButton).Where(Function(tsbtn) tsbtn.Tag = 1)

        Dim str_payfrequency_sched As String = "SEMI-MONTHLY"

        For Each _tsbtn In select_cutoff_payfrequency
            str_payfrequency_sched = _tsbtn.Text
        Next

        Return str_payfrequency_sched

    End Function

    Private Sub tsbtnCashOutUnusedLeaves_Click(sender As Object, e As EventArgs) Handles CashOutUnusedLeavesToolStripMenuItem.Click
        If paypRowID = 0 Then
            MsgBox("Please select a generated payroll.", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim payPeriodSelector = New PayrollSummaDateSelection()

        If payPeriodSelector.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim dateFromId = payPeriodSelector.PayPeriodFromID
        Dim dateToId = payPeriodSelector.PayPeriodToID

        Dim cashOut = New CashOutUnusedLeave(dateFromId, dateToId, paypRowID)
        cashOut.Execute()
    End Sub

    Private Sub tsbtnDelAllEmpPayroll_Click(sender As Object, e As EventArgs) Handles tsbtnDelAllEmpPayroll.Click
        tsbtnDelAllEmpPayroll.Enabled = False

        Dim prompt = MessageBox.Show("Do you want to delete all payrolls of employees?",
                                     "Delete all employee payroll",
                                     MessageBoxButtons.YesNoCancel,
                                     MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If prompt = Windows.Forms.DialogResult.Yes Then
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            For index = 0 To dgvemployees.Rows.Count - 1
                With dgvemployees.Rows(index)
                    dgvemployees.Tag = .Cells("RowID").Value
                    currentEmployeeID = .Cells("EmployeeID").Value
                End With

                Dim n_ExecuteQuery As New ExecuteQuery("SELECT RowID" &
                                                   " FROM paystub" &
                                                   " WHERE EmployeeID='" & dgvemployees.Tag & "'" &
                                                   " AND OrganizationID='" & orgztnID & "'" &
                                                   " AND PayPeriodID='" & paypRowID & "'" &
                                                   " LIMIT 1;")

                Dim paystubRowID As Object = Nothing

                paystubRowID = n_ExecuteQuery.Result

                If paystubRowID IsNot Nothing Then
                    n_ExecuteQuery = New ExecuteQuery("CALL DEL_specificpaystub('" & paystubRowID & "');")
                End If
            Next

            AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
            dgvemployees_SelectionChanged(sender, e)
        End If

        tsbtnDelAllEmpPayroll.Enabled = True
    End Sub
End Class

Friend Class PrintAllPaySlipOfficialFormat

    Private n_PayPeriodRowID As Object = Nothing

    Private n_IsPrintingAsActual As SByte = 0

    Sub New(PayPeriodRowID As Object,
            IsPrintingAsActual As SByte)

        n_PayPeriodRowID = PayPeriodRowID

        n_IsPrintingAsActual = IsPrintingAsActual

        DoProcess()

    End Sub

    Const customDateFormat As String = "'%c/%e/%Y'"

    Private crvwr As New CrysRepForm

    Private catchdt As New DataTable

    Private sys_ownr As New SystemOwner

    Sub DoProcess()

        Dim rptdoc As Object = Nothing

        Static current_system_owner As String = sys_ownr.CurrentSystemOwner

        Dim some_systemowners = New String() {SystemOwner.Goldwings, SystemOwner.DefaultOwner}

        If some_systemowners.Contains(current_system_owner) Then

            Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL paystub_payslip(" & orgztnID & "," & n_PayPeriodRowID & "," & n_IsPrintingAsActual & ");")

            catchdt = n_SQLQueryToDatatable.ResultTable

            rptdoc = New OfficialPaySlipFormat

            With rptdoc.ReportDefinition.Sections(2)
                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("txtOrganizName")
                objText.Text = orgNam.ToUpper

                objText = .ReportObjects("txtPayPeriod")

                If ValNoComma(n_PayPeriodRowID) > 0 Then
                    objText.Text =
                New ExecuteQuery("SELECT CONCAT(DATE_FORMAT(PayFromDate," & customDateFormat & "),' to ',DATE_FORMAT(PayToDate," & customDateFormat & ")) `Result`" &
                                 " FROM payperiod WHERE RowID=" & ValNoComma(n_PayPeriodRowID) & ";").Result
                End If
            End With

        ElseIf SystemOwner.Hyundai = current_system_owner Then

            Dim params =
                New Object() {orgztnID, n_PayPeriodRowID}

            Dim _sql As New SQL("CALL `HyundaiPayslip`(?og_rowid, ?pp_rowid, TRUE, NULL);",
                               params)

            catchdt = _sql.GetFoundRows.Tables(0)

            'rptdoc = New HyundaiPayslip
            rptdoc = New HyundaiPayslip1

            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")
            objText.Text = orgNam.ToUpper

        ElseIf SystemOwner.Cinema2000 = current_system_owner Then

            Dim params =
                New Object() {orgztnID, n_PayPeriodRowID}

            Dim str_query As String = String.Concat(
                "CALL `RPT_payslip`(?og_rowid, ?pp_rowid, TRUE, NULL);")

            Dim _sql As New SQL(str_query,
                                params)

            catchdt = _sql.GetFoundRows.Tables(0)

            rptdoc = New TwoEmpIn1PaySlip

            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("payperiod")
            objText.Text =
                Convert.ToString(
                New SQL(String.Concat("SELECT",
                                      " CONCAT('Payroll period ', DATE_FORMAT(pp.PayFromDate, '%c/%e/%Y'), ' to ', DATE_FORMAT(pp.PayToDate, '%c/%e/%Y')) `Result`",
                                      " FROM payperiod pp WHERE pp.RowID=", n_PayPeriodRowID, ";")).GetFoundRow)

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgContact")
            objText.Text = String.Empty

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgName")
            objText.Text = orgNam.ToUpper

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgAddress")
            objText.Text =
                Convert.ToString(New SQL(String.Concat("SELECT CONCAT_WS(', ',",
                                                       "IF(LENGTH(TRIM(ad.StreetAddress1)) > 0, ad.StreetAddress1, NULL)",
                                                       ",IF(LENGTH(TRIM(ad.StreetAddress2)) > 0, ad.StreetAddress2, NULL)",
                                                       ",IF(LOCATE('city', ad.Barangay) > 0, IF(LENGTH(TRIM(ad.Barangay)) > 0, ad.Barangay, NULL), CONCAT('Brgy. ', TRIM(ad.Barangay)))",
                                                       ",IF(LOCATE('city', ad.CityTown) > 0, IF(LENGTH(TRIM(ad.CityTown)) > 0, ad.CityTown, NULL), CONCAT(TRIM(ad.CityTown), ' city'))",
                                                       ",IF(LENGTH(TRIM(ad.Country)) > 0, ad.Country, NULL)",
                                                       ",IF(LENGTH(TRIM(ad.State)) > 0, ad.State, NULL)",
                                                       ",IF(LENGTH(TRIM(ad.ZipCode)) > 0, ad.ZipCode, NULL)",
                                                       ") `AddressText`",
                                                       " FROM organization og",
                                                       " INNER JOIN address ad ON ad.RowID=og.PrimaryAddressID",
                                                       " WHERE og.RowID = ", orgztnID,
                                                       ";")).GetFoundRow)

        End If

        rptdoc.SetDataSource(catchdt)

        crvwr.crysrepvwr.ReportSource = rptdoc

        crvwr.Show()

    End Sub

End Class

Friend Class PrintSinglePaySlipOfficialFormat

    Private n_PayPeriodRowID As Object = Nothing

    Private n_IsPrintingAsActual As SByte = 0

    Private n_EmployeeRowID As Object = Nothing

    Private sys_ownr As New SystemOwner

    Sub New(PayPeriodRowID As Object,
            IsPrintingAsActual As SByte,
            EmployeeRow_ID As Object)

        n_PayPeriodRowID = PayPeriodRowID

        n_IsPrintingAsActual = IsPrintingAsActual

        n_EmployeeRowID = EmployeeRow_ID

        DoProcess()

    End Sub

    Const customDateFormat As String = "'%c/%e/%Y'"

    Private crvwr As New CrysRepForm

    Private catchdt As New DataTable

    Sub DoProcess()

        Static current_system_owner As String = sys_ownr.CurrentSystemOwner

        If SystemOwner.Goldwings = current_system_owner Then

            Dim rptdoc = New OfficialPaySlipFormat

            Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL paystub_singlepayslip(" & orgztnID & "," & n_PayPeriodRowID & "," & n_IsPrintingAsActual &
                                    "," & n_EmployeeRowID & ");")

            catchdt = n_SQLQueryToDatatable.ResultTable

            With rptdoc.ReportDefinition.Sections(2)
                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("txtOrganizName")
                objText.Text = orgNam.ToUpper

                objText = .ReportObjects("txtPayPeriod")

                If ValNoComma(n_PayPeriodRowID) > 0 Then
                    objText.Text =
                    New ExecuteQuery("SELECT CONCAT(DATE_FORMAT(PayFromDate," & customDateFormat & "),' to ',DATE_FORMAT(PayToDate," & customDateFormat & ")) `Result`" &
                                     " FROM payperiod WHERE RowID=" & ValNoComma(n_PayPeriodRowID) & ";").Result
                End If
            End With

            rptdoc.SetDataSource(catchdt)

            crvwr.crysrepvwr.ReportSource = rptdoc

        ElseIf SystemOwner.Hyundai = current_system_owner Then

            Dim params =
                New Object() {orgztnID, n_PayPeriodRowID, n_EmployeeRowID}

            Dim _sql As New SQL("CALL `HyundaiPayslip`(?og_rowid, ?pp_rowid, TRUE, ?emp_rowid);",
                               params)

            catchdt = _sql.GetFoundRows.Tables(0)

            Dim rptdoc = New HyundaiPayslip

            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")
            objText.Text = orgNam.ToUpper

            rptdoc.SetDataSource(catchdt)

            crvwr.crysrepvwr.ReportSource = rptdoc

        ElseIf SystemOwner.Cinema2000 = current_system_owner Then

            Dim params =
                New Object() {orgztnID, n_PayPeriodRowID, n_EmployeeRowID}

            Dim str_query As String = String.Concat(
                "CALL `RPT_payslip`(?og_rowid, ?pp_rowid, TRUE, ?emp_rowid);")

            Dim _sql As New SQL(str_query,
                                params)

            catchdt = _sql.GetFoundRows.Tables(0)

            Dim rptdoc = New TwoEmpIn1PaySlip

            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgName")
            objText.Text = orgNam.ToUpper

            rptdoc.SetDataSource(catchdt)

            crvwr.crysrepvwr.ReportSource = rptdoc

        End If

        crvwr.Show()

    End Sub

End Class

Friend Class PrintSoloPaySlipThisPayPeriod

    Sub New(ByVal payp_From As Object, ByVal payp_To As Object, ByVal emp_RowID As Object,
            Optional Is_Actual As Int16 = 0)

        Dim rptdoc = New TwoEmpIn1PaySlip

        Dim rptdt = New DataTable

        rptdt = retAsDatTbl("CALL `RPT_solopayslip`('" & orgztnID & "', '" & payp_From & "', '" & payp_To & "', '" & emp_RowID & "', '" & Is_Actual & "');")

        Dim pay_periodstring = String.Empty

        With rptdoc.ReportDefinition.Sections(2)
            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("OrgName")

            objText.Text = orgNam

            Dim orgaddress = EXECQUER("SELECT CONCAT_WS(', ',a.StreetAddress1,a.StreetAddress2,a.Barangay,a.CityTown,a.Country,a.State) AS Result" &
                                      " FROM organization o LEFT JOIN address a ON a.RowID=o.PrimaryAddressID" &
                                      " WHERE o.RowID=" & orgztnID & ";")

            objText = .ReportObjects("OrgAddress")
            objText.Text = orgaddress

            Dim og_contact = String.Empty

            objText = .ReportObjects("OrgContact")
            objText.Text = New ExecuteQuery("SELECT CONCAT_WS(', ',c.FirstName,c.LastName,c.MainPhone,a.StreetAddress1,a.StreetAddress2,a.Barangay,a.CityTown,a.Country,a.State) AS Result" &
                                            " FROM organization o LEFT JOIN contact c ON c.RowID=o.PrimaryContactID" &
                                            " LEFT JOIN address a ON a.RowID=c.AddressID" &
                                            " WHERE o.RowID=" & orgztnID & ";").Result
            pay_periodstring = "for the period of  " & CDate(payp_From).ToShortDateString & " to " & CDate(payp_To).ToShortDateString

            objText = .ReportObjects("payperiod")
            objText.Text = pay_periodstring
        End With

        rptdoc.SetDataSource(rptdt)

        Dim crvwr As New CrysRepForm

        crvwr.crysrepvwr.ReportSource = rptdoc

        crvwr.Text = "Print payslip\" & orgNam & "\" & pay_periodstring

        crvwr.Show()

    End Sub

End Class
