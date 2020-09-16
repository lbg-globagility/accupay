<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PayStubForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PayStubForm))
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvpayper = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PayDateFrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PayDateTo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StatusColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.dgvemployees = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MiddleName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmployeeType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Position = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Division = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblGrossIncome = New System.Windows.Forms.Label()
        Me.Last = New System.Windows.Forms.LinkLabel()
        Me.Nxt = New System.Windows.Forms.LinkLabel()
        Me.Prev = New System.Windows.Forms.LinkLabel()
        Me.First = New System.Windows.Forms.LinkLabel()
        Me.txtFName = New System.Windows.Forms.TextBox()
        Me.txtEmpID = New System.Windows.Forms.TextBox()
        Me.linkPrev = New System.Windows.Forms.LinkLabel()
        Me.linkNxt = New System.Windows.Forms.LinkLabel()
        Me.btnrefresh = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tbppayroll = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tstrip = New System.Windows.Forms.ToolStrip()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel4 = New System.Windows.Forms.LinkLabel()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.pbEmpPicChk = New System.Windows.Forms.PictureBox()
        Me.lblPaidLeavePesoSign = New System.Windows.Forms.Label()
        Me.txtLeaveHours = New System.Windows.Forms.TextBox()
        Me.txtGrandTotalAllow = New System.Windows.Forms.TextBox()
        Me.Label106 = New System.Windows.Forms.Label()
        Me.Label107 = New System.Windows.Forms.Label()
        Me.Label105 = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.txtRegularHolidayHours = New System.Windows.Forms.TextBox()
        Me.txtRestDayOtHour = New System.Windows.Forms.TextBox()
        Me.Label63 = New System.Windows.Forms.Label()
        Me.Label102 = New System.Windows.Forms.Label()
        Me.txtSpecialHolidayOTPay = New System.Windows.Forms.TextBox()
        Me.txtRestDayHours = New System.Windows.Forms.TextBox()
        Me.Label97 = New System.Windows.Forms.Label()
        Me.txtRestDayOtPay = New System.Windows.Forms.TextBox()
        Me.Label76 = New System.Windows.Forms.Label()
        Me.Label96 = New System.Windows.Forms.Label()
        Me.txtRegularHolidayOTHours = New System.Windows.Forms.TextBox()
        Me.txtRestDayPay = New System.Windows.Forms.TextBox()
        Me.txtSpecialHolidayPay = New System.Windows.Forms.TextBox()
        Me.Label99 = New System.Windows.Forms.Label()
        Me.Label98 = New System.Windows.Forms.Label()
        Me.Label100 = New System.Windows.Forms.Label()
        Me.Label101 = New System.Windows.Forms.Label()
        Me.txtSpecialHolidayHours = New System.Windows.Forms.TextBox()
        Me.txtRegularHolidayOTPay = New System.Windows.Forms.TextBox()
        Me.txtSpecialHolidayOTHours = New System.Windows.Forms.TextBox()
        Me.Label64 = New System.Windows.Forms.Label()
        Me.txtRegularHolidayPay = New System.Windows.Forms.TextBox()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.Label80 = New System.Windows.Forms.Label()
        Me.Label95 = New System.Windows.Forms.Label()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.txtTotalTaxableSalary = New System.Windows.Forms.TextBox()
        Me.txtThirteenthMonthPay = New System.Windows.Forms.TextBox()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.txtAgencyFee = New System.Windows.Forms.TextBox()
        Me.txtTotalAdjustments = New System.Windows.Forms.TextBox()
        Me.txtTotalLoans = New System.Windows.Forms.TextBox()
        Me.txtWithholdingTax = New System.Windows.Forms.TextBox()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.Label85 = New System.Windows.Forms.Label()
        Me.txtTotalNetPay = New System.Windows.Forms.TextBox()
        Me.lblTotalNetPay = New System.Windows.Forms.Label()
        Me.Label87 = New System.Windows.Forms.Label()
        Me.lblAgencyFee = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtLeavePay = New System.Windows.Forms.TextBox()
        Me.LinkLabel5 = New System.Windows.Forms.LinkLabel()
        Me.btndiscardchanges = New System.Windows.Forms.Button()
        Me.tabEarned = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtAbsentHours = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtLateHours = New System.Windows.Forms.TextBox()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtUndertimeHours = New System.Windows.Forms.TextBox()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.txtAbsenceDeduction = New System.Windows.Forms.TextBox()
        Me.txtOvertimePay = New System.Windows.Forms.TextBox()
        Me.txtLateDeduction = New System.Windows.Forms.TextBox()
        Me.txtNightDiffPay = New System.Windows.Forms.TextBox()
        Me.txtUndertimeDeduction = New System.Windows.Forms.TextBox()
        Me.txtNightDiffOvertimePay = New System.Windows.Forms.TextBox()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.txtBasicRate = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txttotreghrs = New System.Windows.Forms.TextBox()
        Me.txttotregamt = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtOvertimeHours = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txtNightDiffHours = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.txtNightDiffOvertimeHours = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtRegularHours = New System.Windows.Forms.TextBox()
        Me.txtRegularPay = New System.Windows.Forms.TextBox()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label82 = New System.Windows.Forms.Label()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.Label83 = New System.Windows.Forms.Label()
        Me.txtRegularPayActual = New System.Windows.Forms.TextBox()
        Me.Label84 = New System.Windows.Forms.Label()
        Me.txtRegularHoursActual = New System.Windows.Forms.TextBox()
        Me.Label69 = New System.Windows.Forms.Label()
        Me.Label70 = New System.Windows.Forms.Label()
        Me.txtUndertimeDeductionActual = New System.Windows.Forms.TextBox()
        Me.NightDiffOvertimeHoursActual = New System.Windows.Forms.TextBox()
        Me.txtLateDeductionActual = New System.Windows.Forms.TextBox()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.txtAbsenceDeductionActual = New System.Windows.Forms.TextBox()
        Me.txtNightDiffHoursActual = New System.Windows.Forms.TextBox()
        Me.Label88 = New System.Windows.Forms.Label()
        Me.Label72 = New System.Windows.Forms.Label()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.txtOvertimeHoursActual = New System.Windows.Forms.TextBox()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.Label73 = New System.Windows.Forms.Label()
        Me.txtUndertimeHoursActual = New System.Windows.Forms.TextBox()
        Me.Label74 = New System.Windows.Forms.Label()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.txtBasicRateActual = New System.Windows.Forms.TextBox()
        Me.txtLateHoursActual = New System.Windows.Forms.TextBox()
        Me.Label75 = New System.Windows.Forms.Label()
        Me.txtAbsentHoursActual = New System.Windows.Forms.TextBox()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.txtNightDiffOvertimePayActual = New System.Windows.Forms.TextBox()
        Me.txtNightDiffPayActual = New System.Windows.Forms.TextBox()
        Me.txtOvertimePayActual = New System.Windows.Forms.TextBox()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.Label79 = New System.Windows.Forms.Label()
        Me.Label81 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label65 = New System.Windows.Forms.Label()
        Me.Label66 = New System.Windows.Forms.Label()
        Me.txtHolidayHours = New System.Windows.Forms.TextBox()
        Me.btnSaveAdjustments = New System.Windows.Forms.Button()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.dgvAdjustments = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.psaRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cboProducts = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.DataGridViewTextBoxColumn66 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn64 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column15 = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.IsAdjustmentActual = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.txtHolidayPay = New System.Windows.Forms.TextBox()
        Me.lblsubtotmisc = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.lblSubtotal = New System.Windows.Forms.TextBox()
        Me.lblPaidLeave = New System.Windows.Forms.Label()
        Me.btntotbon = New System.Windows.Forms.Button()
        Me.btntotloan = New System.Windows.Forms.Button()
        Me.btnTotalTaxabAllowance = New System.Windows.Forms.Button()
        Me.btntotallow = New System.Windows.Forms.Button()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.lblGrossIncomeDivider = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.txtTotalBonus = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtTotalTaxableAllowance = New System.Windows.Forms.TextBox()
        Me.txtTotalAllowance = New System.Windows.Forms.TextBox()
        Me.Label104 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtSssEmployeeShare = New System.Windows.Forms.TextBox()
        Me.txtPhilHealthEmployeeShare = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtHdmfEmployeeShare = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNetPay = New System.Windows.Forms.TextBox()
        Me.txtGrossPay = New System.Windows.Forms.TextBox()
        Me.Label103 = New System.Windows.Forms.Label()
        Me.lblGrossIncomePesoSign = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label94 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.GeneratePayrollToolStripMenuItem = New System.Windows.Forms.ToolStripButton()
        Me.ManagePayrollToolStripDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ManagePayslipsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ManagePrintPayslipsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ManageEmailPayslipsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrintPaySlipToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PayslipDeclaredToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PayslipActualToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrintPayrollSummaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PayrollSummaryDeclaredToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PayrollSummaryActualToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportNetPayDetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportNetPayDeclaredToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportNetPayDeclaredAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportNetPayDeclaredCashToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportNetPayDeclaredDirectDepositToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportNetPayActualToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportNetPayActualAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportNetPayActualCashToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportNetPayActualDirectDepositToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CostCenterReportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RecalculateThirteenthMonthPayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CancelPayrollToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeletePayrollToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClosePayrollToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReopenPayrollToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OthersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Include13thMonthPayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CashOutUnusedLeavesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbtnClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsSearch = New System.Windows.Forms.ToolStripTextBox()
        Me.tsbtnSearch = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel8 = New System.Windows.Forms.ToolStripLabel()
        Me.DeleteToolStripDropDownButton = New System.Windows.Forms.ToolStripButton()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ProgressTimer = New System.Windows.Forms.Timer(Me.components)
        CType(Me.dgvpayper, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvemployees, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.tbppayroll.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.pbEmpPicChk, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel6.SuspendLayout()
        Me.tabEarned.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.dgvAdjustments, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvpayper
        '
        Me.dgvpayper.AllowUserToAddRows = False
        Me.dgvpayper.AllowUserToDeleteRows = False
        Me.dgvpayper.AllowUserToOrderColumns = True
        Me.dgvpayper.AllowUserToResizeColumns = False
        Me.dgvpayper.AllowUserToResizeRows = False
        Me.dgvpayper.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvpayper.BackgroundColor = System.Drawing.Color.White
        Me.dgvpayper.ColumnHeadersHeight = 34
        Me.dgvpayper.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvpayper.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.PayDateFrom, Me.PayDateTo, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column8, Me.Column9, Me.Column10, Me.Column11, Me.Column12, Me.Column13, Me.Column14, Me.StatusColumn})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvpayper.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvpayper.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvpayper.Location = New System.Drawing.Point(10, 204)
        Me.dgvpayper.MultiSelect = False
        Me.dgvpayper.Name = "dgvpayper"
        Me.dgvpayper.ReadOnly = True
        Me.dgvpayper.RowHeadersWidth = 25
        Me.dgvpayper.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvpayper.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvpayper.Size = New System.Drawing.Size(235, 295)
        Me.dgvpayper.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = "RowID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Visible = False
        '
        'PayDateFrom
        '
        Me.PayDateFrom.HeaderText = "Pay period from"
        Me.PayDateFrom.Name = "PayDateFrom"
        Me.PayDateFrom.ReadOnly = True
        Me.PayDateFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.PayDateFrom.Width = 154
        '
        'PayDateTo
        '
        Me.PayDateTo.HeaderText = "Pay period to"
        Me.PayDateTo.Name = "PayDateTo"
        Me.PayDateTo.ReadOnly = True
        Me.PayDateTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.PayDateTo.Width = 153
        '
        'Column2
        '
        Me.Column2.HeaderText = "Column2"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Visible = False
        Me.Column2.Width = 154
        '
        'Column3
        '
        Me.Column3.HeaderText = "Column3"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Visible = False
        Me.Column3.Width = 153
        '
        'Column4
        '
        Me.Column4.HeaderText = "TotalGrossSalary"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Visible = False
        '
        'Column5
        '
        Me.Column5.HeaderText = "TotalNetSalary"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Visible = False
        '
        'Column6
        '
        Me.Column6.HeaderText = "TotalEmpSSS"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Visible = False
        '
        'Column7
        '
        Me.Column7.HeaderText = "TotalEmpWithholdingTax"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.Visible = False
        '
        'Column8
        '
        Me.Column8.HeaderText = "TotalCompSSS"
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        Me.Column8.Visible = False
        '
        'Column9
        '
        Me.Column9.HeaderText = "TotalEmpPhilhealth"
        Me.Column9.Name = "Column9"
        Me.Column9.ReadOnly = True
        Me.Column9.Visible = False
        '
        'Column10
        '
        Me.Column10.HeaderText = "TotalCompPhilhealth"
        Me.Column10.Name = "Column10"
        Me.Column10.ReadOnly = True
        Me.Column10.Visible = False
        '
        'Column11
        '
        Me.Column11.HeaderText = "TotalEmpHDMF"
        Me.Column11.Name = "Column11"
        Me.Column11.ReadOnly = True
        Me.Column11.Visible = False
        '
        'Column12
        '
        Me.Column12.HeaderText = "TotalCompHDMF"
        Me.Column12.Name = "Column12"
        Me.Column12.ReadOnly = True
        Me.Column12.Visible = False
        '
        'Column13
        '
        Me.Column13.HeaderText = "now_origin"
        Me.Column13.Name = "Column13"
        Me.Column13.ReadOnly = True
        Me.Column13.Visible = False
        '
        'Column14
        '
        Me.Column14.HeaderText = "End of month"
        Me.Column14.Name = "Column14"
        Me.Column14.ReadOnly = True
        Me.Column14.Visible = False
        '
        'StatusColumn
        '
        Me.StatusColumn.HeaderText = "Status"
        Me.StatusColumn.Name = "StatusColumn"
        Me.StatusColumn.ReadOnly = True
        Me.StatusColumn.Visible = False
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.Label25.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label25.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label25.Location = New System.Drawing.Point(0, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(1229, 21)
        Me.Label25.TabIndex = 139
        Me.Label25.Text = "PAYROLL"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvemployees
        '
        Me.dgvemployees.AllowUserToAddRows = False
        Me.dgvemployees.AllowUserToDeleteRows = False
        Me.dgvemployees.AllowUserToOrderColumns = True
        Me.dgvemployees.AllowUserToResizeColumns = False
        Me.dgvemployees.AllowUserToResizeRows = False
        Me.dgvemployees.BackgroundColor = System.Drawing.Color.White
        Me.dgvemployees.ColumnHeadersHeight = 34
        Me.dgvemployees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvemployees.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.RowID, Me.EmployeeID, Me.LastName, Me.FirstName, Me.MiddleName, Me.EmployeeType, Me.Position, Me.Division})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvemployees.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvemployees.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvemployees.Location = New System.Drawing.Point(103, 104)
        Me.dgvemployees.MultiSelect = False
        Me.dgvemployees.Name = "dgvemployees"
        Me.dgvemployees.ReadOnly = True
        Me.dgvemployees.RowHeadersWidth = 25
        Me.dgvemployees.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvemployees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvemployees.Size = New System.Drawing.Size(765, 300)
        Me.dgvemployees.TabIndex = 174
        '
        'RowID
        '
        Me.RowID.HeaderText = "RowID"
        Me.RowID.Name = "RowID"
        Me.RowID.ReadOnly = True
        '
        'EmployeeID
        '
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmployeeID.DefaultCellStyle = DataGridViewCellStyle2
        Me.EmployeeID.HeaderText = "Employee ID"
        Me.EmployeeID.Name = "EmployeeID"
        Me.EmployeeID.ReadOnly = True
        '
        'LastName
        '
        Me.LastName.HeaderText = "Last Name"
        Me.LastName.Name = "LastName"
        Me.LastName.ReadOnly = True
        '
        'FirstName
        '
        Me.FirstName.HeaderText = "First Name"
        Me.FirstName.Name = "FirstName"
        Me.FirstName.ReadOnly = True
        '
        'MiddleName
        '
        Me.MiddleName.HeaderText = "Middle Name"
        Me.MiddleName.Name = "MiddleName"
        Me.MiddleName.ReadOnly = True
        '
        'EmployeeType
        '
        Me.EmployeeType.HeaderText = "Employee Type"
        Me.EmployeeType.Name = "EmployeeType"
        Me.EmployeeType.ReadOnly = True
        '
        'Position
        '
        Me.Position.HeaderText = "Position"
        Me.Position.Name = "Position"
        Me.Position.ReadOnly = True
        '
        'Division
        '
        Me.Division.HeaderText = "Division"
        Me.Division.Name = "Division"
        Me.Division.ReadOnly = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(389, 591)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 24)
        Me.Label2.TabIndex = 171
        Me.Label2.Text = "Net pay :"
        '
        'lblGrossIncome
        '
        Me.lblGrossIncome.AutoSize = True
        Me.lblGrossIncome.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrossIncome.Location = New System.Drawing.Point(14, 583)
        Me.lblGrossIncome.Name = "lblGrossIncome"
        Me.lblGrossIncome.Size = New System.Drawing.Size(91, 13)
        Me.lblGrossIncome.TabIndex = 170
        Me.lblGrossIncome.Text = "Gross income :"
        '
        'Last
        '
        Me.Last.AutoSize = True
        Me.Last.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Last.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Last.Location = New System.Drawing.Point(243, 407)
        Me.Last.Name = "Last"
        Me.Last.Size = New System.Drawing.Size(44, 15)
        Me.Last.TabIndex = 169
        Me.Last.TabStop = True
        Me.Last.Text = "Last>>"
        '
        'Nxt
        '
        Me.Nxt.AutoSize = True
        Me.Nxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Nxt.Location = New System.Drawing.Point(198, 407)
        Me.Nxt.Name = "Nxt"
        Me.Nxt.Size = New System.Drawing.Size(39, 15)
        Me.Nxt.TabIndex = 168
        Me.Nxt.TabStop = True
        Me.Nxt.Text = "Next>"
        '
        'Prev
        '
        Me.Prev.AutoSize = True
        Me.Prev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Prev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Prev.Location = New System.Drawing.Point(153, 407)
        Me.Prev.Name = "Prev"
        Me.Prev.Size = New System.Drawing.Size(38, 15)
        Me.Prev.TabIndex = 167
        Me.Prev.TabStop = True
        Me.Prev.Text = "<Prev"
        '
        'First
        '
        Me.First.AutoSize = True
        Me.First.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.First.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.First.Location = New System.Drawing.Point(103, 407)
        Me.First.Name = "First"
        Me.First.Size = New System.Drawing.Size(44, 15)
        Me.First.TabIndex = 166
        Me.First.TabStop = True
        Me.First.Text = "<<First"
        '
        'txtFName
        '
        Me.txtFName.BackColor = System.Drawing.Color.White
        Me.txtFName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFName.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFName.Location = New System.Drawing.Point(98, 17)
        Me.txtFName.MaxLength = 250
        Me.txtFName.Name = "txtFName"
        Me.txtFName.ReadOnly = True
        Me.txtFName.Size = New System.Drawing.Size(516, 28)
        Me.txtFName.TabIndex = 163
        '
        'txtEmpID
        '
        Me.txtEmpID.BackColor = System.Drawing.Color.White
        Me.txtEmpID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmpID.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmpID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmpID.Location = New System.Drawing.Point(98, 44)
        Me.txtEmpID.MaxLength = 50
        Me.txtEmpID.Multiline = True
        Me.txtEmpID.Name = "txtEmpID"
        Me.txtEmpID.ReadOnly = True
        Me.txtEmpID.Size = New System.Drawing.Size(800, 24)
        Me.txtEmpID.TabIndex = 162
        '
        'linkPrev
        '
        Me.linkPrev.AutoSize = True
        Me.linkPrev.Dock = System.Windows.Forms.DockStyle.Left
        Me.linkPrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkPrev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkPrev.Location = New System.Drawing.Point(0, 0)
        Me.linkPrev.Name = "linkPrev"
        Me.linkPrev.Size = New System.Drawing.Size(38, 15)
        Me.linkPrev.TabIndex = 141
        Me.linkPrev.TabStop = True
        Me.linkPrev.Text = "<Prev"
        '
        'linkNxt
        '
        Me.linkNxt.AutoSize = True
        Me.linkNxt.Dock = System.Windows.Forms.DockStyle.Right
        Me.linkNxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkNxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkNxt.Location = New System.Drawing.Point(196, 0)
        Me.linkNxt.Name = "linkNxt"
        Me.linkNxt.Size = New System.Drawing.Size(39, 15)
        Me.linkNxt.TabIndex = 142
        Me.linkNxt.TabStop = True
        Me.linkNxt.Text = "Next>"
        '
        'btnrefresh
        '
        Me.btnrefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnrefresh.Location = New System.Drawing.Point(170, 175)
        Me.btnrefresh.Name = "btnrefresh"
        Me.btnrefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnrefresh.TabIndex = 143
        Me.btnrefresh.Text = "&Refresh"
        Me.btnrefresh.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TabControl1.Controls.Add(Me.tbppayroll)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.TabControl1.ItemSize = New System.Drawing.Size(62, 25)
        Me.TabControl1.Location = New System.Drawing.Point(254, 21)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(975, 526)
        Me.TabControl1.TabIndex = 0
        '
        'tbppayroll
        '
        Me.tbppayroll.Controls.Add(Me.Panel1)
        Me.tbppayroll.Controls.Add(Me.ToolStrip1)
        Me.tbppayroll.Location = New System.Drawing.Point(4, 4)
        Me.tbppayroll.Name = "tbppayroll"
        Me.tbppayroll.Padding = New System.Windows.Forms.Padding(3)
        Me.tbppayroll.Size = New System.Drawing.Size(967, 493)
        Me.tbppayroll.TabIndex = 0
        Me.tbppayroll.Text = "PAYROLL               "
        Me.tbppayroll.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.SplitContainer1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(961, 462)
        Me.Panel1.TabIndex = 176
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.Controls.Add(Me.tstrip)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label61)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LinkLabel1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LinkLabel2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LinkLabel3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LinkLabel4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label59)
        Me.SplitContainer1.Panel1.Controls.Add(Me.dgvemployees)
        Me.SplitContainer1.Panel1.Controls.Add(Me.pbEmpPicChk)
        Me.SplitContainer1.Panel1.Controls.Add(Me.First)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtFName)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtEmpID)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Prev)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Last)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Nxt)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblPaidLeavePesoSign)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtLeaveHours)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtGrandTotalAllow)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label106)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label107)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label105)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel6)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label95)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label67)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtTotalTaxableSalary)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtThirteenthMonthPay)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label53)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtAgencyFee)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtTotalAdjustments)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtTotalLoans)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtWithholdingTax)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label93)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label85)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtTotalNetPay)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblTotalNetPay)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label87)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblAgencyFee)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label14)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label32)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label6)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtLeavePay)
        Me.SplitContainer1.Panel2.Controls.Add(Me.LinkLabel5)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btndiscardchanges)
        Me.SplitContainer1.Panel2.Controls.Add(Me.tabEarned)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label18)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label65)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label66)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtHolidayHours)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnSaveAdjustments)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label62)
        Me.SplitContainer1.Panel2.Controls.Add(Me.dgvAdjustments)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label37)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtHolidayPay)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblsubtotmisc)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label5)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label48)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label47)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label46)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label45)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label44)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label43)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label41)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label40)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblSubtotal)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblPaidLeave)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btntotbon)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btntotloan)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnTotalTaxabAllowance)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btntotallow)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label35)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label34)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label33)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label26)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblGrossIncomeDivider)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label21)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label29)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtTotalBonus)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label13)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtTotalTaxableAllowance)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtTotalAllowance)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label104)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label12)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label7)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtSssEmployeeShare)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtPhilHealthEmployeeShare)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label9)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtHdmfEmployeeShare)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label10)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtNetPay)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtGrossPay)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblGrossIncome)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label103)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblGrossIncomePesoSign)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label38)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label36)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label94)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label42)
        Me.SplitContainer1.Size = New System.Drawing.Size(961, 462)
        Me.SplitContainer1.SplitterDistance = 242
        Me.SplitContainer1.TabIndex = 0
        '
        'tstrip
        '
        Me.tstrip.AutoSize = False
        Me.tstrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.tstrip.CanOverflow = False
        Me.tstrip.Dock = System.Windows.Forms.DockStyle.None
        Me.tstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tstrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tstrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.tstrip.Location = New System.Drawing.Point(3, 104)
        Me.tstrip.Name = "tstrip"
        Me.tstrip.Size = New System.Drawing.Size(99, 300)
        Me.tstrip.TabIndex = 280
        Me.tstrip.Text = "ToolStrip1"
        '
        'Label61
        '
        Me.Label61.AutoSize = True
        Me.Label61.ForeColor = System.Drawing.Color.White
        Me.Label61.Location = New System.Drawing.Point(878, 422)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(37, 13)
        Me.Label61.TabIndex = 279
        Me.Label61.Text = "_____"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel1.Location = New System.Drawing.Point(103, 86)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(44, 15)
        Me.LinkLabel1.TabIndex = 275
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "<<First"
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel2.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel2.Location = New System.Drawing.Point(153, 86)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(38, 15)
        Me.LinkLabel2.TabIndex = 276
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "<Prev"
        '
        'LinkLabel3
        '
        Me.LinkLabel3.AutoSize = True
        Me.LinkLabel3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel3.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel3.Location = New System.Drawing.Point(243, 86)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(44, 15)
        Me.LinkLabel3.TabIndex = 278
        Me.LinkLabel3.TabStop = True
        Me.LinkLabel3.Text = "Last>>"
        '
        'LinkLabel4
        '
        Me.LinkLabel4.AutoSize = True
        Me.LinkLabel4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel4.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel4.Location = New System.Drawing.Point(198, 86)
        Me.LinkLabel4.Name = "LinkLabel4"
        Me.LinkLabel4.Size = New System.Drawing.Size(39, 15)
        Me.LinkLabel4.TabIndex = 277
        Me.LinkLabel4.TabStop = True
        Me.LinkLabel4.Text = "Next>"
        '
        'Label59
        '
        Me.Label59.AutoSize = True
        Me.Label59.ForeColor = System.Drawing.Color.White
        Me.Label59.Location = New System.Drawing.Point(435, 422)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(37, 13)
        Me.Label59.TabIndex = 274
        Me.Label59.Text = "_____"
        '
        'pbEmpPicChk
        '
        Me.pbEmpPicChk.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmpPicChk.Location = New System.Drawing.Point(3, 3)
        Me.pbEmpPicChk.Name = "pbEmpPicChk"
        Me.pbEmpPicChk.Size = New System.Drawing.Size(89, 77)
        Me.pbEmpPicChk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmpPicChk.TabIndex = 164
        Me.pbEmpPicChk.TabStop = False
        '
        'lblPaidLeavePesoSign
        '
        Me.lblPaidLeavePesoSign.AutoSize = True
        Me.lblPaidLeavePesoSign.Location = New System.Drawing.Point(232, 378)
        Me.lblPaidLeavePesoSign.Name = "lblPaidLeavePesoSign"
        Me.lblPaidLeavePesoSign.Size = New System.Drawing.Size(14, 13)
        Me.lblPaidLeavePesoSign.TabIndex = 541
        Me.lblPaidLeavePesoSign.Text = "₱"
        '
        'txtLeaveHours
        '
        Me.txtLeaveHours.BackColor = System.Drawing.SystemColors.Window
        Me.txtLeaveHours.Location = New System.Drawing.Point(125, 375)
        Me.txtLeaveHours.Name = "txtLeaveHours"
        Me.txtLeaveHours.ReadOnly = True
        Me.txtLeaveHours.Size = New System.Drawing.Size(100, 20)
        Me.txtLeaveHours.TabIndex = 540
        Me.txtLeaveHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtGrandTotalAllow
        '
        Me.txtGrandTotalAllow.BackColor = System.Drawing.Color.White
        Me.txtGrandTotalAllow.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtGrandTotalAllow.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtGrandTotalAllow.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtGrandTotalAllow.Location = New System.Drawing.Point(253, 495)
        Me.txtGrandTotalAllow.Name = "txtGrandTotalAllow"
        Me.txtGrandTotalAllow.ReadOnly = True
        Me.txtGrandTotalAllow.ShortcutsEnabled = False
        Me.txtGrandTotalAllow.Size = New System.Drawing.Size(100, 16)
        Me.txtGrandTotalAllow.TabIndex = 536
        Me.txtGrandTotalAllow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGrandTotalAllow.Visible = False
        '
        'Label106
        '
        Me.Label106.AutoSize = True
        Me.Label106.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label106.Location = New System.Drawing.Point(14, 496)
        Me.Label106.Name = "Label106"
        Me.Label106.Size = New System.Drawing.Size(144, 13)
        Me.Label106.TabIndex = 537
        Me.Label106.Text = "Grand Total Allowance :"
        Me.Label106.Visible = False
        '
        'Label107
        '
        Me.Label107.AutoSize = True
        Me.Label107.Location = New System.Drawing.Point(233, 496)
        Me.Label107.Name = "Label107"
        Me.Label107.Size = New System.Drawing.Size(14, 13)
        Me.Label107.TabIndex = 539
        Me.Label107.Text = "₱"
        Me.Label107.Visible = False
        '
        'Label105
        '
        Me.Label105.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label105.Location = New System.Drawing.Point(15, 478)
        Me.Label105.Name = "Label105"
        Me.Label105.Size = New System.Drawing.Size(365, 8)
        Me.Label105.TabIndex = 535
        Me.Label105.Text = "---------------------------------------------------------------------------------" &
    "----------------------"
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.txtRegularHolidayHours)
        Me.Panel6.Controls.Add(Me.txtRestDayOtHour)
        Me.Panel6.Controls.Add(Me.Label63)
        Me.Panel6.Controls.Add(Me.Label102)
        Me.Panel6.Controls.Add(Me.txtSpecialHolidayOTPay)
        Me.Panel6.Controls.Add(Me.txtRestDayHours)
        Me.Panel6.Controls.Add(Me.Label97)
        Me.Panel6.Controls.Add(Me.txtRestDayOtPay)
        Me.Panel6.Controls.Add(Me.Label76)
        Me.Panel6.Controls.Add(Me.Label96)
        Me.Panel6.Controls.Add(Me.txtRegularHolidayOTHours)
        Me.Panel6.Controls.Add(Me.txtRestDayPay)
        Me.Panel6.Controls.Add(Me.txtSpecialHolidayPay)
        Me.Panel6.Controls.Add(Me.Label99)
        Me.Panel6.Controls.Add(Me.Label98)
        Me.Panel6.Controls.Add(Me.Label100)
        Me.Panel6.Controls.Add(Me.Label101)
        Me.Panel6.Controls.Add(Me.txtSpecialHolidayHours)
        Me.Panel6.Controls.Add(Me.txtRegularHolidayOTPay)
        Me.Panel6.Controls.Add(Me.txtSpecialHolidayOTHours)
        Me.Panel6.Controls.Add(Me.Label64)
        Me.Panel6.Controls.Add(Me.txtRegularHolidayPay)
        Me.Panel6.Controls.Add(Me.Label60)
        Me.Panel6.Controls.Add(Me.Label80)
        Me.Panel6.Location = New System.Drawing.Point(8, 219)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(359, 152)
        Me.Panel6.TabIndex = 534
        '
        'txtRegularHolidayHours
        '
        Me.txtRegularHolidayHours.AccessibleDescription = "RegularHolidayHours"
        Me.txtRegularHolidayHours.BackColor = System.Drawing.Color.White
        Me.txtRegularHolidayHours.Location = New System.Drawing.Point(117, 9)
        Me.txtRegularHolidayHours.Name = "txtRegularHolidayHours"
        Me.txtRegularHolidayHours.ReadOnly = True
        Me.txtRegularHolidayHours.ShortcutsEnabled = False
        Me.txtRegularHolidayHours.Size = New System.Drawing.Size(100, 20)
        Me.txtRegularHolidayHours.TabIndex = 307
        Me.txtRegularHolidayHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRestDayOtHour
        '
        Me.txtRestDayOtHour.AccessibleDescription = "RestDayOTHours"
        Me.txtRestDayOtHour.Location = New System.Drawing.Point(117, 124)
        Me.txtRestDayOtHour.Name = "txtRestDayOtHour"
        Me.txtRestDayOtHour.Size = New System.Drawing.Size(100, 20)
        Me.txtRestDayOtHour.TabIndex = 533
        Me.txtRestDayOtHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label63
        '
        Me.Label63.AutoSize = True
        Me.Label63.Location = New System.Drawing.Point(6, 39)
        Me.Label63.Name = "Label63"
        Me.Label63.Size = New System.Drawing.Size(81, 13)
        Me.Label63.TabIndex = 311
        Me.Label63.Text = "Reg holiday ot :"
        '
        'Label102
        '
        Me.Label102.AutoSize = True
        Me.Label102.Location = New System.Drawing.Point(6, 128)
        Me.Label102.Name = "Label102"
        Me.Label102.Size = New System.Drawing.Size(67, 13)
        Me.Label102.TabIndex = 532
        Me.Label102.Text = "Rest day ot :"
        '
        'txtSpecialHolidayOTPay
        '
        Me.txtSpecialHolidayOTPay.AccessibleDescription = "SpecialHolidayOTPay"
        Me.txtSpecialHolidayOTPay.BackColor = System.Drawing.Color.White
        Me.txtSpecialHolidayOTPay.Location = New System.Drawing.Point(243, 78)
        Me.txtSpecialHolidayOTPay.Name = "txtSpecialHolidayOTPay"
        Me.txtSpecialHolidayOTPay.ReadOnly = True
        Me.txtSpecialHolidayOTPay.ShortcutsEnabled = False
        Me.txtSpecialHolidayOTPay.Size = New System.Drawing.Size(100, 20)
        Me.txtSpecialHolidayOTPay.TabIndex = 303
        Me.txtSpecialHolidayOTPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRestDayHours
        '
        Me.txtRestDayHours.AccessibleDescription = "RestDayHours"
        Me.txtRestDayHours.Location = New System.Drawing.Point(117, 101)
        Me.txtRestDayHours.Name = "txtRestDayHours"
        Me.txtRestDayHours.Size = New System.Drawing.Size(100, 20)
        Me.txtRestDayHours.TabIndex = 533
        Me.txtRestDayHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label97
        '
        Me.Label97.AutoSize = True
        Me.Label97.Location = New System.Drawing.Point(6, 62)
        Me.Label97.Name = "Label97"
        Me.Label97.Size = New System.Drawing.Size(74, 13)
        Me.Label97.TabIndex = 311
        Me.Label97.Text = "Spec holiday :"
        '
        'txtRestDayOtPay
        '
        Me.txtRestDayOtPay.AccessibleDescription = "RestDayOTPay"
        Me.txtRestDayOtPay.Location = New System.Drawing.Point(243, 124)
        Me.txtRestDayOtPay.Name = "txtRestDayOtPay"
        Me.txtRestDayOtPay.Size = New System.Drawing.Size(100, 20)
        Me.txtRestDayOtPay.TabIndex = 531
        Me.txtRestDayOtPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label76
        '
        Me.Label76.AutoSize = True
        Me.Label76.Location = New System.Drawing.Point(6, 16)
        Me.Label76.Name = "Label76"
        Me.Label76.Size = New System.Drawing.Size(69, 13)
        Me.Label76.TabIndex = 306
        Me.Label76.Text = "Reg holiday :"
        '
        'Label96
        '
        Me.Label96.AutoSize = True
        Me.Label96.Location = New System.Drawing.Point(6, 105)
        Me.Label96.Name = "Label96"
        Me.Label96.Size = New System.Drawing.Size(55, 13)
        Me.Label96.TabIndex = 532
        Me.Label96.Text = "Rest day :"
        '
        'txtRegularHolidayOTHours
        '
        Me.txtRegularHolidayOTHours.AccessibleDescription = "RegularHolidayOTHours"
        Me.txtRegularHolidayOTHours.BackColor = System.Drawing.Color.White
        Me.txtRegularHolidayOTHours.Location = New System.Drawing.Point(117, 32)
        Me.txtRegularHolidayOTHours.Name = "txtRegularHolidayOTHours"
        Me.txtRegularHolidayOTHours.ReadOnly = True
        Me.txtRegularHolidayOTHours.ShortcutsEnabled = False
        Me.txtRegularHolidayOTHours.Size = New System.Drawing.Size(100, 20)
        Me.txtRegularHolidayOTHours.TabIndex = 312
        Me.txtRegularHolidayOTHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRestDayPay
        '
        Me.txtRestDayPay.AccessibleDescription = "RestDayPay"
        Me.txtRestDayPay.Location = New System.Drawing.Point(243, 101)
        Me.txtRestDayPay.Name = "txtRestDayPay"
        Me.txtRestDayPay.Size = New System.Drawing.Size(100, 20)
        Me.txtRestDayPay.TabIndex = 531
        Me.txtRestDayPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSpecialHolidayPay
        '
        Me.txtSpecialHolidayPay.AccessibleDescription = "SpecialHolidayPay"
        Me.txtSpecialHolidayPay.BackColor = System.Drawing.Color.White
        Me.txtSpecialHolidayPay.Location = New System.Drawing.Point(243, 55)
        Me.txtSpecialHolidayPay.Name = "txtSpecialHolidayPay"
        Me.txtSpecialHolidayPay.ReadOnly = True
        Me.txtSpecialHolidayPay.ShortcutsEnabled = False
        Me.txtSpecialHolidayPay.Size = New System.Drawing.Size(100, 20)
        Me.txtSpecialHolidayPay.TabIndex = 303
        Me.txtSpecialHolidayPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label99
        '
        Me.Label99.AutoSize = True
        Me.Label99.Location = New System.Drawing.Point(6, 85)
        Me.Label99.Name = "Label99"
        Me.Label99.Size = New System.Drawing.Size(83, 13)
        Me.Label99.TabIndex = 311
        Me.Label99.Text = "Spec holiday ot:"
        '
        'Label98
        '
        Me.Label98.AutoSize = True
        Me.Label98.Location = New System.Drawing.Point(223, 81)
        Me.Label98.Name = "Label98"
        Me.Label98.Size = New System.Drawing.Size(14, 13)
        Me.Label98.TabIndex = 301
        Me.Label98.Text = "₱"
        '
        'Label100
        '
        Me.Label100.AutoSize = True
        Me.Label100.Location = New System.Drawing.Point(223, 104)
        Me.Label100.Name = "Label100"
        Me.Label100.Size = New System.Drawing.Size(14, 13)
        Me.Label100.TabIndex = 301
        Me.Label100.Text = "₱"
        '
        'Label101
        '
        Me.Label101.AutoSize = True
        Me.Label101.Location = New System.Drawing.Point(223, 127)
        Me.Label101.Name = "Label101"
        Me.Label101.Size = New System.Drawing.Size(14, 13)
        Me.Label101.TabIndex = 301
        Me.Label101.Text = "₱"
        '
        'txtSpecialHolidayHours
        '
        Me.txtSpecialHolidayHours.AccessibleDescription = "SpecialHolidayHours"
        Me.txtSpecialHolidayHours.BackColor = System.Drawing.Color.White
        Me.txtSpecialHolidayHours.Location = New System.Drawing.Point(117, 55)
        Me.txtSpecialHolidayHours.Name = "txtSpecialHolidayHours"
        Me.txtSpecialHolidayHours.ReadOnly = True
        Me.txtSpecialHolidayHours.ShortcutsEnabled = False
        Me.txtSpecialHolidayHours.Size = New System.Drawing.Size(100, 20)
        Me.txtSpecialHolidayHours.TabIndex = 312
        Me.txtSpecialHolidayHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRegularHolidayOTPay
        '
        Me.txtRegularHolidayOTPay.AccessibleDescription = "RegularHolidayOTPay"
        Me.txtRegularHolidayOTPay.BackColor = System.Drawing.Color.White
        Me.txtRegularHolidayOTPay.Location = New System.Drawing.Point(243, 32)
        Me.txtRegularHolidayOTPay.Name = "txtRegularHolidayOTPay"
        Me.txtRegularHolidayOTPay.ReadOnly = True
        Me.txtRegularHolidayOTPay.ShortcutsEnabled = False
        Me.txtRegularHolidayOTPay.Size = New System.Drawing.Size(100, 20)
        Me.txtRegularHolidayOTPay.TabIndex = 303
        Me.txtRegularHolidayOTPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSpecialHolidayOTHours
        '
        Me.txtSpecialHolidayOTHours.AccessibleDescription = "SpecialHolidayOTHours"
        Me.txtSpecialHolidayOTHours.BackColor = System.Drawing.Color.White
        Me.txtSpecialHolidayOTHours.Location = New System.Drawing.Point(117, 78)
        Me.txtSpecialHolidayOTHours.Name = "txtSpecialHolidayOTHours"
        Me.txtSpecialHolidayOTHours.ReadOnly = True
        Me.txtSpecialHolidayOTHours.ShortcutsEnabled = False
        Me.txtSpecialHolidayOTHours.Size = New System.Drawing.Size(100, 20)
        Me.txtSpecialHolidayOTHours.TabIndex = 312
        Me.txtSpecialHolidayOTHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label64
        '
        Me.Label64.AutoSize = True
        Me.Label64.Location = New System.Drawing.Point(223, 58)
        Me.Label64.Name = "Label64"
        Me.Label64.Size = New System.Drawing.Size(14, 13)
        Me.Label64.TabIndex = 301
        Me.Label64.Text = "₱"
        '
        'txtRegularHolidayPay
        '
        Me.txtRegularHolidayPay.AccessibleDescription = "RegularHolidayPay"
        Me.txtRegularHolidayPay.BackColor = System.Drawing.Color.White
        Me.txtRegularHolidayPay.Location = New System.Drawing.Point(243, 9)
        Me.txtRegularHolidayPay.Name = "txtRegularHolidayPay"
        Me.txtRegularHolidayPay.ReadOnly = True
        Me.txtRegularHolidayPay.ShortcutsEnabled = False
        Me.txtRegularHolidayPay.Size = New System.Drawing.Size(100, 20)
        Me.txtRegularHolidayPay.TabIndex = 317
        Me.txtRegularHolidayPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.Location = New System.Drawing.Point(223, 35)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(14, 13)
        Me.Label60.TabIndex = 301
        Me.Label60.Text = "₱"
        '
        'Label80
        '
        Me.Label80.AutoSize = True
        Me.Label80.Location = New System.Drawing.Point(223, 12)
        Me.Label80.Name = "Label80"
        Me.Label80.Size = New System.Drawing.Size(14, 13)
        Me.Label80.TabIndex = 302
        Me.Label80.Text = "₱"
        '
        'Label95
        '
        Me.Label95.AutoSize = True
        Me.Label95.Location = New System.Drawing.Point(610, 566)
        Me.Label95.Name = "Label95"
        Me.Label95.Size = New System.Drawing.Size(14, 13)
        Me.Label95.TabIndex = 530
        Me.Label95.Text = "₱"
        '
        'Label67
        '
        Me.Label67.AutoSize = True
        Me.Label67.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label67.Location = New System.Drawing.Point(389, 528)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(116, 13)
        Me.Label67.TabIndex = 509
        Me.Label67.Text = "Total Adjustments :"
        '
        'txtTotalTaxableSalary
        '
        Me.txtTotalTaxableSalary.BackColor = System.Drawing.Color.White
        Me.txtTotalTaxableSalary.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalTaxableSalary.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtTotalTaxableSalary.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtTotalTaxableSalary.Location = New System.Drawing.Point(630, 376)
        Me.txtTotalTaxableSalary.Name = "txtTotalTaxableSalary"
        Me.txtTotalTaxableSalary.ReadOnly = True
        Me.txtTotalTaxableSalary.ShortcutsEnabled = False
        Me.txtTotalTaxableSalary.Size = New System.Drawing.Size(100, 16)
        Me.txtTotalTaxableSalary.TabIndex = 178
        Me.txtTotalTaxableSalary.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtThirteenthMonthPay
        '
        Me.txtThirteenthMonthPay.BackColor = System.Drawing.Color.White
        Me.txtThirteenthMonthPay.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtThirteenthMonthPay.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtThirteenthMonthPay.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtThirteenthMonthPay.Location = New System.Drawing.Point(631, 566)
        Me.txtThirteenthMonthPay.Name = "txtThirteenthMonthPay"
        Me.txtThirteenthMonthPay.ReadOnly = True
        Me.txtThirteenthMonthPay.ShortcutsEnabled = False
        Me.txtThirteenthMonthPay.Size = New System.Drawing.Size(100, 16)
        Me.txtThirteenthMonthPay.TabIndex = 528
        Me.txtThirteenthMonthPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.Location = New System.Drawing.Point(231, 198)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(14, 13)
        Me.Label53.TabIndex = 176
        Me.Label53.Text = "₱"
        Me.Label53.Visible = False
        '
        'txtAgencyFee
        '
        Me.txtAgencyFee.BackColor = System.Drawing.Color.White
        Me.txtAgencyFee.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtAgencyFee.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtAgencyFee.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtAgencyFee.Location = New System.Drawing.Point(632, 490)
        Me.txtAgencyFee.Name = "txtAgencyFee"
        Me.txtAgencyFee.ReadOnly = True
        Me.txtAgencyFee.ShortcutsEnabled = False
        Me.txtAgencyFee.Size = New System.Drawing.Size(100, 16)
        Me.txtAgencyFee.TabIndex = 521
        Me.txtAgencyFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalAdjustments
        '
        Me.txtTotalAdjustments.BackColor = System.Drawing.Color.White
        Me.txtTotalAdjustments.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalAdjustments.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtTotalAdjustments.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtTotalAdjustments.Location = New System.Drawing.Point(632, 528)
        Me.txtTotalAdjustments.Name = "txtTotalAdjustments"
        Me.txtTotalAdjustments.ReadOnly = True
        Me.txtTotalAdjustments.ShortcutsEnabled = False
        Me.txtTotalAdjustments.Size = New System.Drawing.Size(100, 16)
        Me.txtTotalAdjustments.TabIndex = 510
        Me.txtTotalAdjustments.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalLoans
        '
        Me.txtTotalLoans.BackColor = System.Drawing.Color.White
        Me.txtTotalLoans.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalLoans.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtTotalLoans.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtTotalLoans.Location = New System.Drawing.Point(630, 452)
        Me.txtTotalLoans.Name = "txtTotalLoans"
        Me.txtTotalLoans.ReadOnly = True
        Me.txtTotalLoans.ShortcutsEnabled = False
        Me.txtTotalLoans.Size = New System.Drawing.Size(100, 16)
        Me.txtTotalLoans.TabIndex = 195
        Me.txtTotalLoans.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtWithholdingTax
        '
        Me.txtWithholdingTax.BackColor = System.Drawing.Color.White
        Me.txtWithholdingTax.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtWithholdingTax.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWithholdingTax.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtWithholdingTax.Location = New System.Drawing.Point(630, 414)
        Me.txtWithholdingTax.Name = "txtWithholdingTax"
        Me.txtWithholdingTax.ReadOnly = True
        Me.txtWithholdingTax.ShortcutsEnabled = False
        Me.txtWithholdingTax.Size = New System.Drawing.Size(100, 16)
        Me.txtWithholdingTax.TabIndex = 182
        Me.txtWithholdingTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label93
        '
        Me.Label93.AutoSize = True
        Me.Label93.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label93.Location = New System.Drawing.Point(387, 566)
        Me.Label93.Name = "Label93"
        Me.Label93.Size = New System.Drawing.Size(133, 13)
        Me.Label93.TabIndex = 527
        Me.Label93.Text = "Thirteenth Month Pay:"
        '
        'Label85
        '
        Me.Label85.AutoSize = True
        Me.Label85.Location = New System.Drawing.Point(610, 620)
        Me.Label85.Name = "Label85"
        Me.Label85.Size = New System.Drawing.Size(14, 13)
        Me.Label85.TabIndex = 526
        Me.Label85.Text = "₱"
        '
        'txtTotalNetPay
        '
        Me.txtTotalNetPay.BackColor = System.Drawing.Color.White
        Me.txtTotalNetPay.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalNetPay.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalNetPay.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtTotalNetPay.Location = New System.Drawing.Point(632, 620)
        Me.txtTotalNetPay.Name = "txtTotalNetPay"
        Me.txtTotalNetPay.ReadOnly = True
        Me.txtTotalNetPay.ShortcutsEnabled = False
        Me.txtTotalNetPay.Size = New System.Drawing.Size(100, 16)
        Me.txtTotalNetPay.TabIndex = 525
        Me.txtTotalNetPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTotalNetPay
        '
        Me.lblTotalNetPay.AutoSize = True
        Me.lblTotalNetPay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalNetPay.Location = New System.Drawing.Point(389, 620)
        Me.lblTotalNetPay.Name = "lblTotalNetPay"
        Me.lblTotalNetPay.Size = New System.Drawing.Size(88, 13)
        Me.lblTotalNetPay.TabIndex = 523
        Me.lblTotalNetPay.Text = "Total Net pay:"
        '
        'Label87
        '
        Me.Label87.AutoSize = True
        Me.Label87.Location = New System.Drawing.Point(610, 490)
        Me.Label87.Name = "Label87"
        Me.Label87.Size = New System.Drawing.Size(14, 13)
        Me.Label87.TabIndex = 522
        Me.Label87.Text = "₱"
        '
        'lblAgencyFee
        '
        Me.lblAgencyFee.AutoSize = True
        Me.lblAgencyFee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgencyFee.Location = New System.Drawing.Point(389, 490)
        Me.lblAgencyFee.Name = "lblAgencyFee"
        Me.lblAgencyFee.Size = New System.Drawing.Size(82, 13)
        Me.lblAgencyFee.TabIndex = 520
        Me.lblAgencyFee.Text = "Agency Fee :"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(387, 452)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(76, 13)
        Me.Label14.TabIndex = 194
        Me.Label14.Text = "Total Loan :"
        '
        'Label32
        '
        Me.Label32.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(390, 435)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(367, 13)
        Me.Label32.TabIndex = 242
        Me.Label32.Text = "---------------------------------------------------------------------------------" &
    "----------------------"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(387, 414)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(103, 13)
        Me.Label6.TabIndex = 180
        Me.Label6.Text = "Withholding tax :"
        '
        'txtLeavePay
        '
        Me.txtLeavePay.BackColor = System.Drawing.SystemColors.Window
        Me.txtLeavePay.Location = New System.Drawing.Point(251, 375)
        Me.txtLeavePay.Name = "txtLeavePay"
        Me.txtLeavePay.ReadOnly = True
        Me.txtLeavePay.ShortcutsEnabled = False
        Me.txtLeavePay.Size = New System.Drawing.Size(100, 20)
        Me.txtLeavePay.TabIndex = 519
        Me.txtLeavePay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LinkLabel5
        '
        Me.LinkLabel5.ActiveLinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel5.AutoSize = True
        Me.LinkLabel5.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel5.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LinkLabel5.Location = New System.Drawing.Point(1252, 3)
        Me.LinkLabel5.Name = "LinkLabel5"
        Me.LinkLabel5.Size = New System.Drawing.Size(121, 13)
        Me.LinkLabel5.TabIndex = 515
        Me.LinkLabel5.TabStop = True
        Me.LinkLabel5.Text = "Add an Item to the list"
        Me.LinkLabel5.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'btndiscardchanges
        '
        Me.btndiscardchanges.Image = CType(resources.GetObject("btndiscardchanges.Image"), System.Drawing.Image)
        Me.btndiscardchanges.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btndiscardchanges.Location = New System.Drawing.Point(881, 179)
        Me.btndiscardchanges.Name = "btndiscardchanges"
        Me.btndiscardchanges.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.btndiscardchanges.Size = New System.Drawing.Size(118, 34)
        Me.btndiscardchanges.TabIndex = 514
        Me.btndiscardchanges.Text = "Discard changes"
        Me.btndiscardchanges.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btndiscardchanges.UseVisualStyleBackColor = True
        '
        'tabEarned
        '
        Me.tabEarned.Controls.Add(Me.TabPage1)
        Me.tabEarned.Controls.Add(Me.TabPage4)
        Me.tabEarned.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.tabEarned.ItemSize = New System.Drawing.Size(62, 25)
        Me.tabEarned.Location = New System.Drawing.Point(3, 3)
        Me.tabEarned.Name = "tabEarned"
        Me.tabEarned.SelectedIndex = 0
        Me.tabEarned.Size = New System.Drawing.Size(734, 207)
        Me.tabEarned.TabIndex = 513
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Panel2)
        Me.TabPage1.Controls.Add(Me.txtAbsentHours)
        Me.TabPage1.Controls.Add(Me.Label17)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.txtLateHours)
        Me.TabPage1.Controls.Add(Me.Label52)
        Me.TabPage1.Controls.Add(Me.Label16)
        Me.TabPage1.Controls.Add(Me.txtUndertimeHours)
        Me.TabPage1.Controls.Add(Me.Label54)
        Me.TabPage1.Controls.Add(Me.Label15)
        Me.TabPage1.Controls.Add(Me.Label55)
        Me.TabPage1.Controls.Add(Me.Label27)
        Me.TabPage1.Controls.Add(Me.Label56)
        Me.TabPage1.Controls.Add(Me.Label28)
        Me.TabPage1.Controls.Add(Me.Label57)
        Me.TabPage1.Controls.Add(Me.txtAbsenceDeduction)
        Me.TabPage1.Controls.Add(Me.txtOvertimePay)
        Me.TabPage1.Controls.Add(Me.txtLateDeduction)
        Me.TabPage1.Controls.Add(Me.txtNightDiffPay)
        Me.TabPage1.Controls.Add(Me.txtUndertimeDeduction)
        Me.TabPage1.Controls.Add(Me.txtNightDiffOvertimePay)
        Me.TabPage1.Controls.Add(Me.Label51)
        Me.TabPage1.Controls.Add(Me.Label50)
        Me.TabPage1.Controls.Add(Me.Label49)
        Me.TabPage1.Controls.Add(Me.Label58)
        Me.TabPage1.Controls.Add(Me.txtBasicRate)
        Me.TabPage1.Controls.Add(Me.Label8)
        Me.TabPage1.Controls.Add(Me.txttotreghrs)
        Me.TabPage1.Controls.Add(Me.txttotregamt)
        Me.TabPage1.Controls.Add(Me.Label11)
        Me.TabPage1.Controls.Add(Me.txtOvertimeHours)
        Me.TabPage1.Controls.Add(Me.Label22)
        Me.TabPage1.Controls.Add(Me.txtNightDiffHours)
        Me.TabPage1.Controls.Add(Me.Label23)
        Me.TabPage1.Controls.Add(Me.txtNightDiffOvertimeHours)
        Me.TabPage1.Controls.Add(Me.Label20)
        Me.TabPage1.Controls.Add(Me.Label19)
        Me.TabPage1.Controls.Add(Me.txtRegularHours)
        Me.TabPage1.Controls.Add(Me.txtRegularPay)
        Me.TabPage1.Location = New System.Drawing.Point(4, 29)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(726, 174)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Tag = "0"
        Me.TabPage1.Text = "DECLARED               "
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Black
        Me.Panel2.Location = New System.Drawing.Point(373, 1)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1, 174)
        Me.Panel2.TabIndex = 515
        '
        'txtAbsentHours
        '
        Me.txtAbsentHours.BackColor = System.Drawing.Color.White
        Me.txtAbsentHours.Location = New System.Drawing.Point(496, 72)
        Me.txtAbsentHours.Name = "txtAbsentHours"
        Me.txtAbsentHours.ReadOnly = True
        Me.txtAbsentHours.ShortcutsEnabled = False
        Me.txtAbsentHours.Size = New System.Drawing.Size(100, 20)
        Me.txtAbsentHours.TabIndex = 203
        Me.txtAbsentHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(380, 79)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(46, 13)
        Me.Label17.TabIndex = 202
        Me.Label17.Text = "Absent :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(5, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 13)
        Me.Label4.TabIndex = 208
        Me.Label4.Text = "Basic rate :"
        '
        'txtLateHours
        '
        Me.txtLateHours.BackColor = System.Drawing.Color.White
        Me.txtLateHours.Location = New System.Drawing.Point(496, 98)
        Me.txtLateHours.Name = "txtLateHours"
        Me.txtLateHours.ReadOnly = True
        Me.txtLateHours.ShortcutsEnabled = False
        Me.txtLateHours.Size = New System.Drawing.Size(100, 20)
        Me.txtLateHours.TabIndex = 204
        Me.txtLateHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Location = New System.Drawing.Point(224, 75)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(14, 13)
        Me.Label52.TabIndex = 176
        Me.Label52.Text = "₱"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(380, 105)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(59, 13)
        Me.Label16.TabIndex = 205
        Me.Label16.Text = "Tardiness :"
        '
        'txtUndertimeHours
        '
        Me.txtUndertimeHours.BackColor = System.Drawing.Color.White
        Me.txtUndertimeHours.Location = New System.Drawing.Point(496, 125)
        Me.txtUndertimeHours.Name = "txtUndertimeHours"
        Me.txtUndertimeHours.ReadOnly = True
        Me.txtUndertimeHours.ShortcutsEnabled = False
        Me.txtUndertimeHours.Size = New System.Drawing.Size(100, 20)
        Me.txtUndertimeHours.TabIndex = 206
        Me.txtUndertimeHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Location = New System.Drawing.Point(224, 153)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(14, 13)
        Me.Label54.TabIndex = 176
        Me.Label54.Text = "₱"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(380, 132)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(61, 13)
        Me.Label15.TabIndex = 207
        Me.Label15.Text = "Undertime :"
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(224, 127)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(14, 13)
        Me.Label55.TabIndex = 176
        Me.Label55.Text = "₱"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(493, 56)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(71, 13)
        Me.Label27.TabIndex = 228
        Me.Label27.Text = "Total hours"
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(224, 101)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(14, 13)
        Me.Label56.TabIndex = 176
        Me.Label56.Text = "₱"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(620, 56)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(81, 13)
        Me.Label28.TabIndex = 229
        Me.Label28.Text = "Total amount"
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(244, 26)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(14, 13)
        Me.Label57.TabIndex = 176
        Me.Label57.Text = "₱"
        Me.Label57.Visible = False
        '
        'txtAbsenceDeduction
        '
        Me.txtAbsenceDeduction.BackColor = System.Drawing.Color.White
        Me.txtAbsenceDeduction.Location = New System.Drawing.Point(623, 72)
        Me.txtAbsenceDeduction.Name = "txtAbsenceDeduction"
        Me.txtAbsenceDeduction.ReadOnly = True
        Me.txtAbsenceDeduction.ShortcutsEnabled = False
        Me.txtAbsenceDeduction.Size = New System.Drawing.Size(100, 20)
        Me.txtAbsenceDeduction.TabIndex = 235
        Me.txtAbsenceDeduction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtOvertimePay
        '
        Me.txtOvertimePay.BackColor = System.Drawing.Color.White
        Me.txtOvertimePay.Location = New System.Drawing.Point(244, 98)
        Me.txtOvertimePay.Name = "txtOvertimePay"
        Me.txtOvertimePay.ReadOnly = True
        Me.txtOvertimePay.ShortcutsEnabled = False
        Me.txtOvertimePay.Size = New System.Drawing.Size(100, 20)
        Me.txtOvertimePay.TabIndex = 196
        Me.txtOvertimePay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtLateDeduction
        '
        Me.txtLateDeduction.BackColor = System.Drawing.Color.White
        Me.txtLateDeduction.Location = New System.Drawing.Point(623, 98)
        Me.txtLateDeduction.Name = "txtLateDeduction"
        Me.txtLateDeduction.ReadOnly = True
        Me.txtLateDeduction.ShortcutsEnabled = False
        Me.txtLateDeduction.Size = New System.Drawing.Size(100, 20)
        Me.txtLateDeduction.TabIndex = 236
        Me.txtLateDeduction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNightDiffPay
        '
        Me.txtNightDiffPay.BackColor = System.Drawing.Color.White
        Me.txtNightDiffPay.Location = New System.Drawing.Point(244, 124)
        Me.txtNightDiffPay.Name = "txtNightDiffPay"
        Me.txtNightDiffPay.ReadOnly = True
        Me.txtNightDiffPay.ShortcutsEnabled = False
        Me.txtNightDiffPay.Size = New System.Drawing.Size(100, 20)
        Me.txtNightDiffPay.TabIndex = 197
        Me.txtNightDiffPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtUndertimeDeduction
        '
        Me.txtUndertimeDeduction.BackColor = System.Drawing.Color.White
        Me.txtUndertimeDeduction.Location = New System.Drawing.Point(623, 125)
        Me.txtUndertimeDeduction.Name = "txtUndertimeDeduction"
        Me.txtUndertimeDeduction.ReadOnly = True
        Me.txtUndertimeDeduction.ShortcutsEnabled = False
        Me.txtUndertimeDeduction.Size = New System.Drawing.Size(100, 20)
        Me.txtUndertimeDeduction.TabIndex = 237
        Me.txtUndertimeDeduction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNightDiffOvertimePay
        '
        Me.txtNightDiffOvertimePay.BackColor = System.Drawing.Color.White
        Me.txtNightDiffOvertimePay.Location = New System.Drawing.Point(244, 150)
        Me.txtNightDiffOvertimePay.Name = "txtNightDiffOvertimePay"
        Me.txtNightDiffOvertimePay.ReadOnly = True
        Me.txtNightDiffOvertimePay.ShortcutsEnabled = False
        Me.txtNightDiffOvertimePay.Size = New System.Drawing.Size(100, 20)
        Me.txtNightDiffOvertimePay.TabIndex = 198
        Me.txtNightDiffOvertimePay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Location = New System.Drawing.Point(603, 75)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(14, 13)
        Me.Label51.TabIndex = 271
        Me.Label51.Text = "₱"
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Location = New System.Drawing.Point(603, 101)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(14, 13)
        Me.Label50.TabIndex = 270
        Me.Label50.Text = "₱"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Location = New System.Drawing.Point(603, 128)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(14, 13)
        Me.Label49.TabIndex = 269
        Me.Label49.Text = "₱"
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(5, 26)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(14, 13)
        Me.Label58.TabIndex = 272
        Me.Label58.Text = "₱"
        '
        'txtBasicRate
        '
        Me.txtBasicRate.Location = New System.Drawing.Point(23, 23)
        Me.txtBasicRate.Name = "txtBasicRate"
        Me.txtBasicRate.ShortcutsEnabled = False
        Me.txtBasicRate.Size = New System.Drawing.Size(100, 20)
        Me.txtBasicRate.TabIndex = 209
        Me.txtBasicRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 79)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(50, 13)
        Me.Label8.TabIndex = 210
        Me.Label8.Text = "Regular :"
        '
        'txttotreghrs
        '
        Me.txttotreghrs.BackColor = System.Drawing.Color.White
        Me.txttotreghrs.Location = New System.Drawing.Point(138, 23)
        Me.txttotreghrs.Name = "txttotreghrs"
        Me.txttotreghrs.ReadOnly = True
        Me.txttotreghrs.ShortcutsEnabled = False
        Me.txttotreghrs.Size = New System.Drawing.Size(100, 20)
        Me.txttotreghrs.TabIndex = 211
        Me.txttotreghrs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txttotreghrs.Visible = False
        '
        'txttotregamt
        '
        Me.txttotregamt.BackColor = System.Drawing.Color.White
        Me.txttotregamt.Location = New System.Drawing.Point(264, 23)
        Me.txttotregamt.Name = "txttotregamt"
        Me.txttotregamt.ReadOnly = True
        Me.txttotregamt.ShortcutsEnabled = False
        Me.txttotregamt.Size = New System.Drawing.Size(100, 20)
        Me.txttotregamt.TabIndex = 212
        Me.txttotregamt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txttotregamt.Visible = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(7, 105)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(55, 13)
        Me.Label11.TabIndex = 213
        Me.Label11.Text = "Overtime :"
        '
        'txtOvertimeHours
        '
        Me.txtOvertimeHours.BackColor = System.Drawing.Color.White
        Me.txtOvertimeHours.Location = New System.Drawing.Point(118, 98)
        Me.txtOvertimeHours.Name = "txtOvertimeHours"
        Me.txtOvertimeHours.ReadOnly = True
        Me.txtOvertimeHours.ShortcutsEnabled = False
        Me.txtOvertimeHours.Size = New System.Drawing.Size(100, 20)
        Me.txtOvertimeHours.TabIndex = 214
        Me.txtOvertimeHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(7, 131)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(89, 13)
        Me.Label22.TabIndex = 216
        Me.Label22.Text = "Night differential :"
        '
        'txtNightDiffHours
        '
        Me.txtNightDiffHours.BackColor = System.Drawing.Color.White
        Me.txtNightDiffHours.Location = New System.Drawing.Point(118, 124)
        Me.txtNightDiffHours.Name = "txtNightDiffHours"
        Me.txtNightDiffHours.ReadOnly = True
        Me.txtNightDiffHours.ShortcutsEnabled = False
        Me.txtNightDiffHours.Size = New System.Drawing.Size(100, 20)
        Me.txtNightDiffHours.TabIndex = 217
        Me.txtNightDiffHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(7, 157)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(107, 13)
        Me.Label23.TabIndex = 219
        Me.Label23.Text = "Night differential OT :"
        '
        'txtNightDiffOvertimeHours
        '
        Me.txtNightDiffOvertimeHours.BackColor = System.Drawing.Color.White
        Me.txtNightDiffOvertimeHours.Location = New System.Drawing.Point(118, 150)
        Me.txtNightDiffOvertimeHours.Name = "txtNightDiffOvertimeHours"
        Me.txtNightDiffOvertimeHours.ReadOnly = True
        Me.txtNightDiffOvertimeHours.ShortcutsEnabled = False
        Me.txtNightDiffOvertimeHours.Size = New System.Drawing.Size(100, 20)
        Me.txtNightDiffOvertimeHours.TabIndex = 220
        Me.txtNightDiffOvertimeHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(115, 56)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(71, 13)
        Me.Label20.TabIndex = 233
        Me.Label20.Text = "Total hours"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(241, 56)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(81, 13)
        Me.Label19.TabIndex = 234
        Me.Label19.Text = "Total amount"
        '
        'txtRegularHours
        '
        Me.txtRegularHours.BackColor = System.Drawing.Color.White
        Me.txtRegularHours.Location = New System.Drawing.Point(118, 72)
        Me.txtRegularHours.Name = "txtRegularHours"
        Me.txtRegularHours.ReadOnly = True
        Me.txtRegularHours.ShortcutsEnabled = False
        Me.txtRegularHours.Size = New System.Drawing.Size(100, 20)
        Me.txtRegularHours.TabIndex = 252
        Me.txtRegularHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRegularPay
        '
        Me.txtRegularPay.BackColor = System.Drawing.Color.White
        Me.txtRegularPay.Location = New System.Drawing.Point(244, 72)
        Me.txtRegularPay.Name = "txtRegularPay"
        Me.txtRegularPay.ReadOnly = True
        Me.txtRegularPay.ShortcutsEnabled = False
        Me.txtRegularPay.Size = New System.Drawing.Size(100, 20)
        Me.txtRegularPay.TabIndex = 253
        Me.txtRegularPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.Panel3)
        Me.TabPage4.Controls.Add(Me.Label82)
        Me.TabPage4.Controls.Add(Me.Label68)
        Me.TabPage4.Controls.Add(Me.Label83)
        Me.TabPage4.Controls.Add(Me.txtRegularPayActual)
        Me.TabPage4.Controls.Add(Me.Label84)
        Me.TabPage4.Controls.Add(Me.txtRegularHoursActual)
        Me.TabPage4.Controls.Add(Me.Label69)
        Me.TabPage4.Controls.Add(Me.Label70)
        Me.TabPage4.Controls.Add(Me.txtUndertimeDeductionActual)
        Me.TabPage4.Controls.Add(Me.NightDiffOvertimeHoursActual)
        Me.TabPage4.Controls.Add(Me.txtLateDeductionActual)
        Me.TabPage4.Controls.Add(Me.Label71)
        Me.TabPage4.Controls.Add(Me.txtAbsenceDeductionActual)
        Me.TabPage4.Controls.Add(Me.txtNightDiffHoursActual)
        Me.TabPage4.Controls.Add(Me.Label88)
        Me.TabPage4.Controls.Add(Me.Label72)
        Me.TabPage4.Controls.Add(Me.Label89)
        Me.TabPage4.Controls.Add(Me.txtOvertimeHoursActual)
        Me.TabPage4.Controls.Add(Me.Label90)
        Me.TabPage4.Controls.Add(Me.Label73)
        Me.TabPage4.Controls.Add(Me.txtUndertimeHoursActual)
        Me.TabPage4.Controls.Add(Me.Label74)
        Me.TabPage4.Controls.Add(Me.Label91)
        Me.TabPage4.Controls.Add(Me.txtBasicRateActual)
        Me.TabPage4.Controls.Add(Me.txtLateHoursActual)
        Me.TabPage4.Controls.Add(Me.Label75)
        Me.TabPage4.Controls.Add(Me.txtAbsentHoursActual)
        Me.TabPage4.Controls.Add(Me.Label92)
        Me.TabPage4.Controls.Add(Me.txtNightDiffOvertimePayActual)
        Me.TabPage4.Controls.Add(Me.txtNightDiffPayActual)
        Me.TabPage4.Controls.Add(Me.txtOvertimePayActual)
        Me.TabPage4.Controls.Add(Me.Label77)
        Me.TabPage4.Controls.Add(Me.Label78)
        Me.TabPage4.Controls.Add(Me.Label79)
        Me.TabPage4.Controls.Add(Me.Label81)
        Me.TabPage4.Location = New System.Drawing.Point(4, 29)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(726, 174)
        Me.TabPage4.TabIndex = 1
        Me.TabPage4.Tag = "1"
        Me.TabPage4.Text = "ACTUAL"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Black
        Me.Panel3.Location = New System.Drawing.Point(373, 1)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1, 174)
        Me.Panel3.TabIndex = 516
        '
        'Label82
        '
        Me.Label82.AutoSize = True
        Me.Label82.Location = New System.Drawing.Point(603, 75)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(14, 13)
        Me.Label82.TabIndex = 288
        Me.Label82.Text = "₱"
        '
        'Label68
        '
        Me.Label68.AutoSize = True
        Me.Label68.Location = New System.Drawing.Point(5, 26)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(14, 13)
        Me.Label68.TabIndex = 322
        Me.Label68.Text = "₱"
        '
        'Label83
        '
        Me.Label83.AutoSize = True
        Me.Label83.Location = New System.Drawing.Point(603, 101)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(14, 13)
        Me.Label83.TabIndex = 287
        Me.Label83.Text = "₱"
        '
        'txtRegularPayActual
        '
        Me.txtRegularPayActual.BackColor = System.Drawing.Color.White
        Me.txtRegularPayActual.Location = New System.Drawing.Point(244, 72)
        Me.txtRegularPayActual.Name = "txtRegularPayActual"
        Me.txtRegularPayActual.ReadOnly = True
        Me.txtRegularPayActual.ShortcutsEnabled = False
        Me.txtRegularPayActual.Size = New System.Drawing.Size(100, 20)
        Me.txtRegularPayActual.TabIndex = 321
        Me.txtRegularPayActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label84
        '
        Me.Label84.AutoSize = True
        Me.Label84.Location = New System.Drawing.Point(603, 128)
        Me.Label84.Name = "Label84"
        Me.Label84.Size = New System.Drawing.Size(14, 13)
        Me.Label84.TabIndex = 286
        Me.Label84.Text = "₱"
        '
        'txtRegularHoursActual
        '
        Me.txtRegularHoursActual.BackColor = System.Drawing.Color.White
        Me.txtRegularHoursActual.Location = New System.Drawing.Point(118, 72)
        Me.txtRegularHoursActual.Name = "txtRegularHoursActual"
        Me.txtRegularHoursActual.ReadOnly = True
        Me.txtRegularHoursActual.ShortcutsEnabled = False
        Me.txtRegularHoursActual.Size = New System.Drawing.Size(100, 20)
        Me.txtRegularHoursActual.TabIndex = 320
        Me.txtRegularHoursActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label69
        '
        Me.Label69.AutoSize = True
        Me.Label69.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label69.Location = New System.Drawing.Point(241, 56)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(81, 13)
        Me.Label69.TabIndex = 319
        Me.Label69.Text = "Total amount"
        '
        'Label70
        '
        Me.Label70.AutoSize = True
        Me.Label70.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label70.Location = New System.Drawing.Point(115, 56)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(71, 13)
        Me.Label70.TabIndex = 318
        Me.Label70.Text = "Total hours"
        '
        'txtUndertimeDeductionActual
        '
        Me.txtUndertimeDeductionActual.BackColor = System.Drawing.Color.White
        Me.txtUndertimeDeductionActual.Location = New System.Drawing.Point(623, 125)
        Me.txtUndertimeDeductionActual.Name = "txtUndertimeDeductionActual"
        Me.txtUndertimeDeductionActual.ReadOnly = True
        Me.txtUndertimeDeductionActual.ShortcutsEnabled = False
        Me.txtUndertimeDeductionActual.Size = New System.Drawing.Size(100, 20)
        Me.txtUndertimeDeductionActual.TabIndex = 282
        Me.txtUndertimeDeductionActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'NightDiffOvertimeHoursActual
        '
        Me.NightDiffOvertimeHoursActual.BackColor = System.Drawing.Color.White
        Me.NightDiffOvertimeHoursActual.Location = New System.Drawing.Point(118, 150)
        Me.NightDiffOvertimeHoursActual.Name = "NightDiffOvertimeHoursActual"
        Me.NightDiffOvertimeHoursActual.ReadOnly = True
        Me.NightDiffOvertimeHoursActual.ShortcutsEnabled = False
        Me.NightDiffOvertimeHoursActual.Size = New System.Drawing.Size(100, 20)
        Me.NightDiffOvertimeHoursActual.TabIndex = 316
        Me.NightDiffOvertimeHoursActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtLateDeductionActual
        '
        Me.txtLateDeductionActual.BackColor = System.Drawing.Color.White
        Me.txtLateDeductionActual.Location = New System.Drawing.Point(623, 98)
        Me.txtLateDeductionActual.Name = "txtLateDeductionActual"
        Me.txtLateDeductionActual.ReadOnly = True
        Me.txtLateDeductionActual.ShortcutsEnabled = False
        Me.txtLateDeductionActual.Size = New System.Drawing.Size(100, 20)
        Me.txtLateDeductionActual.TabIndex = 281
        Me.txtLateDeductionActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label71
        '
        Me.Label71.AutoSize = True
        Me.Label71.Location = New System.Drawing.Point(7, 157)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(107, 13)
        Me.Label71.TabIndex = 315
        Me.Label71.Text = "Night differential OT :"
        '
        'txtAbsenceDeductionActual
        '
        Me.txtAbsenceDeductionActual.BackColor = System.Drawing.Color.White
        Me.txtAbsenceDeductionActual.Location = New System.Drawing.Point(623, 72)
        Me.txtAbsenceDeductionActual.Name = "txtAbsenceDeductionActual"
        Me.txtAbsenceDeductionActual.ReadOnly = True
        Me.txtAbsenceDeductionActual.ShortcutsEnabled = False
        Me.txtAbsenceDeductionActual.Size = New System.Drawing.Size(100, 20)
        Me.txtAbsenceDeductionActual.TabIndex = 280
        Me.txtAbsenceDeductionActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNightDiffHoursActual
        '
        Me.txtNightDiffHoursActual.BackColor = System.Drawing.Color.White
        Me.txtNightDiffHoursActual.Location = New System.Drawing.Point(118, 124)
        Me.txtNightDiffHoursActual.Name = "txtNightDiffHoursActual"
        Me.txtNightDiffHoursActual.ReadOnly = True
        Me.txtNightDiffHoursActual.ShortcutsEnabled = False
        Me.txtNightDiffHoursActual.Size = New System.Drawing.Size(100, 20)
        Me.txtNightDiffHoursActual.TabIndex = 314
        Me.txtNightDiffHoursActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label88
        '
        Me.Label88.AutoSize = True
        Me.Label88.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label88.Location = New System.Drawing.Point(620, 56)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(81, 13)
        Me.Label88.TabIndex = 279
        Me.Label88.Text = "Total amount"
        '
        'Label72
        '
        Me.Label72.AutoSize = True
        Me.Label72.Location = New System.Drawing.Point(7, 131)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(89, 13)
        Me.Label72.TabIndex = 313
        Me.Label72.Text = "Night differential :"
        '
        'Label89
        '
        Me.Label89.AutoSize = True
        Me.Label89.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label89.Location = New System.Drawing.Point(493, 56)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(71, 13)
        Me.Label89.TabIndex = 278
        Me.Label89.Text = "Total hours"
        '
        'txtOvertimeHoursActual
        '
        Me.txtOvertimeHoursActual.BackColor = System.Drawing.Color.White
        Me.txtOvertimeHoursActual.Location = New System.Drawing.Point(118, 98)
        Me.txtOvertimeHoursActual.Name = "txtOvertimeHoursActual"
        Me.txtOvertimeHoursActual.ReadOnly = True
        Me.txtOvertimeHoursActual.ShortcutsEnabled = False
        Me.txtOvertimeHoursActual.Size = New System.Drawing.Size(100, 20)
        Me.txtOvertimeHoursActual.TabIndex = 312
        Me.txtOvertimeHoursActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label90
        '
        Me.Label90.AutoSize = True
        Me.Label90.Location = New System.Drawing.Point(380, 132)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(61, 13)
        Me.Label90.TabIndex = 277
        Me.Label90.Text = "Undertime :"
        '
        'Label73
        '
        Me.Label73.AutoSize = True
        Me.Label73.Location = New System.Drawing.Point(7, 105)
        Me.Label73.Name = "Label73"
        Me.Label73.Size = New System.Drawing.Size(55, 13)
        Me.Label73.TabIndex = 311
        Me.Label73.Text = "Overtime :"
        '
        'txtUndertimeHoursActual
        '
        Me.txtUndertimeHoursActual.BackColor = System.Drawing.Color.White
        Me.txtUndertimeHoursActual.Location = New System.Drawing.Point(496, 125)
        Me.txtUndertimeHoursActual.Name = "txtUndertimeHoursActual"
        Me.txtUndertimeHoursActual.ReadOnly = True
        Me.txtUndertimeHoursActual.ShortcutsEnabled = False
        Me.txtUndertimeHoursActual.Size = New System.Drawing.Size(100, 20)
        Me.txtUndertimeHoursActual.TabIndex = 276
        Me.txtUndertimeHoursActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label74
        '
        Me.Label74.AutoSize = True
        Me.Label74.Location = New System.Drawing.Point(7, 79)
        Me.Label74.Name = "Label74"
        Me.Label74.Size = New System.Drawing.Size(50, 13)
        Me.Label74.TabIndex = 310
        Me.Label74.Text = "Regular :"
        '
        'Label91
        '
        Me.Label91.AutoSize = True
        Me.Label91.Location = New System.Drawing.Point(380, 105)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(59, 13)
        Me.Label91.TabIndex = 275
        Me.Label91.Text = "Tardiness :"
        '
        'txtBasicRateActual
        '
        Me.txtBasicRateActual.Location = New System.Drawing.Point(23, 23)
        Me.txtBasicRateActual.Name = "txtBasicRateActual"
        Me.txtBasicRateActual.ShortcutsEnabled = False
        Me.txtBasicRateActual.Size = New System.Drawing.Size(100, 20)
        Me.txtBasicRateActual.TabIndex = 309
        Me.txtBasicRateActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtLateHoursActual
        '
        Me.txtLateHoursActual.BackColor = System.Drawing.Color.White
        Me.txtLateHoursActual.Location = New System.Drawing.Point(496, 98)
        Me.txtLateHoursActual.Name = "txtLateHoursActual"
        Me.txtLateHoursActual.ReadOnly = True
        Me.txtLateHoursActual.ShortcutsEnabled = False
        Me.txtLateHoursActual.Size = New System.Drawing.Size(100, 20)
        Me.txtLateHoursActual.TabIndex = 274
        Me.txtLateHoursActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label75
        '
        Me.Label75.AutoSize = True
        Me.Label75.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label75.Location = New System.Drawing.Point(5, 7)
        Me.Label75.Name = "Label75"
        Me.Label75.Size = New System.Drawing.Size(72, 13)
        Me.Label75.TabIndex = 308
        Me.Label75.Text = "Basic rate :"
        '
        'txtAbsentHoursActual
        '
        Me.txtAbsentHoursActual.BackColor = System.Drawing.Color.White
        Me.txtAbsentHoursActual.Location = New System.Drawing.Point(496, 72)
        Me.txtAbsentHoursActual.Name = "txtAbsentHoursActual"
        Me.txtAbsentHoursActual.ReadOnly = True
        Me.txtAbsentHoursActual.ShortcutsEnabled = False
        Me.txtAbsentHoursActual.Size = New System.Drawing.Size(100, 20)
        Me.txtAbsentHoursActual.TabIndex = 273
        Me.txtAbsentHoursActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label92
        '
        Me.Label92.AutoSize = True
        Me.Label92.Location = New System.Drawing.Point(380, 79)
        Me.Label92.Name = "Label92"
        Me.Label92.Size = New System.Drawing.Size(46, 13)
        Me.Label92.TabIndex = 272
        Me.Label92.Text = "Absent :"
        '
        'txtNightDiffOvertimePayActual
        '
        Me.txtNightDiffOvertimePayActual.BackColor = System.Drawing.Color.White
        Me.txtNightDiffOvertimePayActual.Location = New System.Drawing.Point(244, 150)
        Me.txtNightDiffOvertimePayActual.Name = "txtNightDiffOvertimePayActual"
        Me.txtNightDiffOvertimePayActual.ReadOnly = True
        Me.txtNightDiffOvertimePayActual.ShortcutsEnabled = False
        Me.txtNightDiffOvertimePayActual.Size = New System.Drawing.Size(100, 20)
        Me.txtNightDiffOvertimePayActual.TabIndex = 305
        Me.txtNightDiffOvertimePayActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNightDiffPayActual
        '
        Me.txtNightDiffPayActual.BackColor = System.Drawing.Color.White
        Me.txtNightDiffPayActual.Location = New System.Drawing.Point(244, 124)
        Me.txtNightDiffPayActual.Name = "txtNightDiffPayActual"
        Me.txtNightDiffPayActual.ReadOnly = True
        Me.txtNightDiffPayActual.ShortcutsEnabled = False
        Me.txtNightDiffPayActual.Size = New System.Drawing.Size(100, 20)
        Me.txtNightDiffPayActual.TabIndex = 304
        Me.txtNightDiffPayActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtOvertimePayActual
        '
        Me.txtOvertimePayActual.BackColor = System.Drawing.Color.White
        Me.txtOvertimePayActual.Location = New System.Drawing.Point(244, 98)
        Me.txtOvertimePayActual.Name = "txtOvertimePayActual"
        Me.txtOvertimePayActual.ReadOnly = True
        Me.txtOvertimePayActual.ShortcutsEnabled = False
        Me.txtOvertimePayActual.Size = New System.Drawing.Size(100, 20)
        Me.txtOvertimePayActual.TabIndex = 303
        Me.txtOvertimePayActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Location = New System.Drawing.Point(224, 101)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(14, 13)
        Me.Label77.TabIndex = 301
        Me.Label77.Text = "₱"
        '
        'Label78
        '
        Me.Label78.AutoSize = True
        Me.Label78.Location = New System.Drawing.Point(224, 127)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(14, 13)
        Me.Label78.TabIndex = 300
        Me.Label78.Text = "₱"
        '
        'Label79
        '
        Me.Label79.AutoSize = True
        Me.Label79.Location = New System.Drawing.Point(224, 153)
        Me.Label79.Name = "Label79"
        Me.Label79.Size = New System.Drawing.Size(14, 13)
        Me.Label79.TabIndex = 299
        Me.Label79.Text = "₱"
        '
        'Label81
        '
        Me.Label81.AutoSize = True
        Me.Label81.Location = New System.Drawing.Point(224, 75)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(14, 13)
        Me.Label81.TabIndex = 298
        Me.Label81.Text = "₱"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(12, 202)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(68, 13)
        Me.Label18.TabIndex = 200
        Me.Label18.Text = "Holiday pay :"
        Me.Label18.Visible = False
        '
        'Label65
        '
        Me.Label65.AutoSize = True
        Me.Label65.Location = New System.Drawing.Point(610, 528)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(14, 13)
        Me.Label65.TabIndex = 512
        Me.Label65.Text = "₱"
        '
        'Label66
        '
        Me.Label66.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label66.Location = New System.Drawing.Point(392, 511)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(367, 13)
        Me.Label66.TabIndex = 511
        Me.Label66.Text = "---------------------------------------------------------------------------------" &
    "----------------------"
        '
        'txtHolidayHours
        '
        Me.txtHolidayHours.BackColor = System.Drawing.Color.White
        Me.txtHolidayHours.Location = New System.Drawing.Point(125, 195)
        Me.txtHolidayHours.Name = "txtHolidayHours"
        Me.txtHolidayHours.ReadOnly = True
        Me.txtHolidayHours.ShortcutsEnabled = False
        Me.txtHolidayHours.Size = New System.Drawing.Size(100, 20)
        Me.txtHolidayHours.TabIndex = 201
        Me.txtHolidayHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtHolidayHours.Visible = False
        '
        'btnSaveAdjustments
        '
        Me.btnSaveAdjustments.Enabled = False
        Me.btnSaveAdjustments.Image = Global.AccuPay.My.Resources.Resources.Add
        Me.btnSaveAdjustments.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveAdjustments.Location = New System.Drawing.Point(746, 179)
        Me.btnSaveAdjustments.Name = "btnSaveAdjustments"
        Me.btnSaveAdjustments.Size = New System.Drawing.Size(129, 34)
        Me.btnSaveAdjustments.TabIndex = 508
        Me.btnSaveAdjustments.Text = "Save Adjustments"
        Me.btnSaveAdjustments.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveAdjustments.UseVisualStyleBackColor = True
        '
        'Label62
        '
        Me.Label62.AutoSize = True
        Me.Label62.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label62.Location = New System.Drawing.Point(743, 3)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(75, 13)
        Me.Label62.TabIndex = 507
        Me.Label62.Text = "Adjustments"
        '
        'dgvAdjustments
        '
        Me.dgvAdjustments.AllowUserToOrderColumns = True
        Me.dgvAdjustments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvAdjustments.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvAdjustments.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvAdjustments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAdjustments.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.psaRowID, Me.cboProducts, Me.DataGridViewTextBoxColumn66, Me.DataGridViewTextBoxColumn64, Me.Column15, Me.IsAdjustmentActual})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvAdjustments.DefaultCellStyle = DataGridViewCellStyle7
        Me.dgvAdjustments.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvAdjustments.Location = New System.Drawing.Point(746, 19)
        Me.dgvAdjustments.MultiSelect = False
        Me.dgvAdjustments.Name = "dgvAdjustments"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvAdjustments.RowHeadersDefaultCellStyle = DataGridViewCellStyle8
        Me.dgvAdjustments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAdjustments.Size = New System.Drawing.Size(626, 154)
        Me.dgvAdjustments.TabIndex = 506
        '
        'psaRowID
        '
        Me.psaRowID.DataPropertyName = "RowID"
        Me.psaRowID.HeaderText = "RowID"
        Me.psaRowID.Name = "psaRowID"
        Me.psaRowID.Visible = False
        '
        'cboProducts
        '
        Me.cboProducts.DataPropertyName = "ProductID"
        Me.cboProducts.FillWeight = 30.0!
        Me.cboProducts.HeaderText = "Item"
        Me.cboProducts.Name = "cboProducts"
        Me.cboProducts.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cboProducts.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'DataGridViewTextBoxColumn66
        '
        Me.DataGridViewTextBoxColumn66.DataPropertyName = "PayAmount"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn66.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumn66.FillWeight = 15.0!
        Me.DataGridViewTextBoxColumn66.HeaderText = "Amount"
        Me.DataGridViewTextBoxColumn66.Name = "DataGridViewTextBoxColumn66"
        '
        'DataGridViewTextBoxColumn64
        '
        Me.DataGridViewTextBoxColumn64.DataPropertyName = "Comment"
        Me.DataGridViewTextBoxColumn64.FillWeight = 45.0!
        Me.DataGridViewTextBoxColumn64.HeaderText = "Comment"
        Me.DataGridViewTextBoxColumn64.Name = "DataGridViewTextBoxColumn64"
        '
        'Column15
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column15.DefaultCellStyle = DataGridViewCellStyle6
        Me.Column15.FillWeight = 10.0!
        Me.Column15.HeaderText = ""
        Me.Column15.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.Column15.LinkColor = System.Drawing.Color.Red
        Me.Column15.Name = "Column15"
        Me.Column15.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column15.Text = "Delete"
        Me.Column15.UseColumnTextForLinkValue = True
        '
        'IsAdjustmentActual
        '
        Me.IsAdjustmentActual.DataPropertyName = "IsActual"
        Me.IsAdjustmentActual.HeaderText = "IsAdjustmentActual"
        Me.IsAdjustmentActual.Name = "IsAdjustmentActual"
        Me.IsAdjustmentActual.Visible = False
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(387, 224)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(66, 13)
        Me.Label37.TabIndex = 254
        Me.Label37.Text = "Sub total :"
        Me.Label37.Visible = False
        '
        'txtHolidayPay
        '
        Me.txtHolidayPay.BackColor = System.Drawing.Color.White
        Me.txtHolidayPay.Location = New System.Drawing.Point(251, 195)
        Me.txtHolidayPay.Name = "txtHolidayPay"
        Me.txtHolidayPay.ReadOnly = True
        Me.txtHolidayPay.ShortcutsEnabled = False
        Me.txtHolidayPay.Size = New System.Drawing.Size(100, 20)
        Me.txtHolidayPay.TabIndex = 232
        Me.txtHolidayPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtHolidayPay.Visible = False
        '
        'lblsubtotmisc
        '
        Me.lblsubtotmisc.BackColor = System.Drawing.Color.White
        Me.lblsubtotmisc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblsubtotmisc.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblsubtotmisc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lblsubtotmisc.Location = New System.Drawing.Point(630, 215)
        Me.lblsubtotmisc.Name = "lblsubtotmisc"
        Me.lblsubtotmisc.ReadOnly = True
        Me.lblsubtotmisc.ShortcutsEnabled = False
        Me.lblsubtotmisc.Size = New System.Drawing.Size(100, 16)
        Me.lblsubtotmisc.TabIndex = 257
        Me.lblsubtotmisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.lblsubtotmisc.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(1348, 671)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 273
        Me.Label5.Text = "_____"
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Location = New System.Drawing.Point(610, 216)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(14, 13)
        Me.Label48.TabIndex = 268
        Me.Label48.Text = "₱"
        Me.Label48.Visible = False
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Location = New System.Drawing.Point(610, 266)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(14, 13)
        Me.Label47.TabIndex = 267
        Me.Label47.Text = "₱"
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Location = New System.Drawing.Point(610, 292)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(14, 13)
        Me.Label46.TabIndex = 267
        Me.Label46.Text = "₱"
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Location = New System.Drawing.Point(610, 318)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(14, 13)
        Me.Label45.TabIndex = 266
        Me.Label45.Text = "₱"
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Location = New System.Drawing.Point(610, 452)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(14, 13)
        Me.Label44.TabIndex = 265
        Me.Label44.Text = "₱"
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(233, 532)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(14, 13)
        Me.Label43.TabIndex = 264
        Me.Label43.Text = "₱"
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(610, 414)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(14, 13)
        Me.Label41.TabIndex = 262
        Me.Label41.Text = "₱"
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(610, 376)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(14, 13)
        Me.Label40.TabIndex = 261
        Me.Label40.Text = "₱"
        '
        'lblSubtotal
        '
        Me.lblSubtotal.BackColor = System.Drawing.Color.White
        Me.lblSubtotal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblSubtotal.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblSubtotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lblSubtotal.Location = New System.Drawing.Point(253, 396)
        Me.lblSubtotal.Name = "lblSubtotal"
        Me.lblSubtotal.ReadOnly = True
        Me.lblSubtotal.ShortcutsEnabled = False
        Me.lblSubtotal.Size = New System.Drawing.Size(100, 16)
        Me.lblSubtotal.TabIndex = 256
        Me.lblSubtotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.lblSubtotal.Visible = False
        '
        'lblPaidLeave
        '
        Me.lblPaidLeave.AutoSize = True
        Me.lblPaidLeave.Location = New System.Drawing.Point(14, 375)
        Me.lblPaidLeave.Name = "lblPaidLeave"
        Me.lblPaidLeave.Size = New System.Drawing.Size(63, 13)
        Me.lblPaidLeave.TabIndex = 251
        Me.lblPaidLeave.Text = "Paid leave :"
        '
        'btntotbon
        '
        Me.btntotbon.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btntotbon.Location = New System.Drawing.Point(360, 527)
        Me.btntotbon.Name = "btntotbon"
        Me.btntotbon.Size = New System.Drawing.Size(21, 22)
        Me.btntotbon.TabIndex = 248
        Me.btntotbon.Text = "..."
        Me.btntotbon.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btntotbon.UseVisualStyleBackColor = True
        Me.btntotbon.Visible = False
        '
        'btntotloan
        '
        Me.btntotloan.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btntotloan.Location = New System.Drawing.Point(736, 449)
        Me.btntotloan.Name = "btntotloan"
        Me.btntotloan.Size = New System.Drawing.Size(21, 22)
        Me.btntotloan.TabIndex = 247
        Me.btntotloan.Text = "..."
        Me.btntotloan.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btntotloan.UseVisualStyleBackColor = True
        '
        'btnTotalTaxabAllowance
        '
        Me.btnTotalTaxabAllowance.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnTotalTaxabAllowance.Location = New System.Drawing.Point(360, 457)
        Me.btnTotalTaxabAllowance.Name = "btnTotalTaxabAllowance"
        Me.btnTotalTaxabAllowance.Size = New System.Drawing.Size(21, 22)
        Me.btnTotalTaxabAllowance.TabIndex = 246
        Me.btnTotalTaxabAllowance.Text = "..."
        Me.btnTotalTaxabAllowance.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnTotalTaxabAllowance.UseVisualStyleBackColor = True
        Me.btnTotalTaxabAllowance.Visible = False
        '
        'btntotallow
        '
        Me.btntotallow.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btntotallow.Location = New System.Drawing.Point(360, 431)
        Me.btntotallow.Name = "btntotallow"
        Me.btntotallow.Size = New System.Drawing.Size(21, 22)
        Me.btntotallow.TabIndex = 246
        Me.btntotallow.Text = "..."
        Me.btntotallow.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btntotallow.UseVisualStyleBackColor = True
        Me.btntotallow.Visible = False
        '
        'Label35
        '
        Me.Label35.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(390, 473)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(367, 13)
        Me.Label35.TabIndex = 245
        Me.Label35.Text = "---------------------------------------------------------------------------------" &
    "----------------------"
        '
        'Label34
        '
        Me.Label34.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.Location = New System.Drawing.Point(390, 397)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(367, 13)
        Me.Label34.TabIndex = 244
        Me.Label34.Text = "---------------------------------------------------------------------------------" &
    "----------------------"
        '
        'Label33
        '
        Me.Label33.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(390, 346)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(367, 13)
        Me.Label33.TabIndex = 243
        Me.Label33.Text = "---------------------------------------------------------------------------------" &
    "-----------------------------------------"
        '
        'Label26
        '
        Me.Label26.Font = New System.Drawing.Font("Segoe UI", 6.75!)
        Me.Label26.Location = New System.Drawing.Point(390, 240)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(367, 13)
        Me.Label26.TabIndex = 241
        Me.Label26.Text = "---------------------------------------------------------------------------------" &
    "----------------------"
        '
        'lblGrossIncomeDivider
        '
        Me.lblGrossIncomeDivider.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrossIncomeDivider.Location = New System.Drawing.Point(15, 549)
        Me.lblGrossIncomeDivider.Name = "lblGrossIncomeDivider"
        Me.lblGrossIncomeDivider.Size = New System.Drawing.Size(365, 13)
        Me.lblGrossIncomeDivider.TabIndex = 240
        Me.lblGrossIncomeDivider.Text = "---------------------------------------------------------------------------------" &
    "-----------------------------------------"
        '
        'Label21
        '
        Me.Label21.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(15, 418)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(365, 13)
        Me.Label21.TabIndex = 239
        Me.Label21.Text = "---------------------------------------------------------------------------------" &
    "----------------------"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(14, 405)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(66, 13)
        Me.Label29.TabIndex = 230
        Me.Label29.Text = "Sub total :"
        Me.Label29.Visible = False
        '
        'txtTotalBonus
        '
        Me.txtTotalBonus.BackColor = System.Drawing.Color.White
        Me.txtTotalBonus.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalBonus.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtTotalBonus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtTotalBonus.Location = New System.Drawing.Point(253, 531)
        Me.txtTotalBonus.Name = "txtTotalBonus"
        Me.txtTotalBonus.ReadOnly = True
        Me.txtTotalBonus.ShortcutsEnabled = False
        Me.txtTotalBonus.Size = New System.Drawing.Size(100, 16)
        Me.txtTotalBonus.TabIndex = 196
        Me.txtTotalBonus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(14, 532)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(83, 13)
        Me.Label13.TabIndex = 197
        Me.Label13.Text = "Total Bonus :"
        '
        'txtTotalTaxableAllowance
        '
        Me.txtTotalTaxableAllowance.BackColor = System.Drawing.Color.White
        Me.txtTotalTaxableAllowance.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalTaxableAllowance.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtTotalTaxableAllowance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtTotalTaxableAllowance.Location = New System.Drawing.Point(253, 461)
        Me.txtTotalTaxableAllowance.Name = "txtTotalTaxableAllowance"
        Me.txtTotalTaxableAllowance.ReadOnly = True
        Me.txtTotalTaxableAllowance.ShortcutsEnabled = False
        Me.txtTotalTaxableAllowance.Size = New System.Drawing.Size(100, 16)
        Me.txtTotalTaxableAllowance.TabIndex = 198
        Me.txtTotalTaxableAllowance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalAllowance
        '
        Me.txtTotalAllowance.BackColor = System.Drawing.Color.White
        Me.txtTotalAllowance.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalAllowance.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtTotalAllowance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtTotalAllowance.Location = New System.Drawing.Point(253, 435)
        Me.txtTotalAllowance.Name = "txtTotalAllowance"
        Me.txtTotalAllowance.ReadOnly = True
        Me.txtTotalAllowance.ShortcutsEnabled = False
        Me.txtTotalAllowance.Size = New System.Drawing.Size(100, 16)
        Me.txtTotalAllowance.TabIndex = 198
        Me.txtTotalAllowance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label104
        '
        Me.Label104.AutoSize = True
        Me.Label104.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label104.Location = New System.Drawing.Point(14, 462)
        Me.Label104.Name = "Label104"
        Me.Label104.Size = New System.Drawing.Size(130, 13)
        Me.Label104.TabIndex = 199
        Me.Label104.Text = "Total Taxable Allowance :"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(14, 436)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(147, 13)
        Me.Label12.TabIndex = 199
        Me.Label12.Text = "Total non-taxable Allowance :"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(387, 270)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 13)
        Me.Label7.TabIndex = 181
        Me.Label7.Text = "Employee SSS :"
        '
        'txtSssEmployeeShare
        '
        Me.txtSssEmployeeShare.BackColor = System.Drawing.Color.White
        Me.txtSssEmployeeShare.Location = New System.Drawing.Point(630, 263)
        Me.txtSssEmployeeShare.Name = "txtSssEmployeeShare"
        Me.txtSssEmployeeShare.ReadOnly = True
        Me.txtSssEmployeeShare.ShortcutsEnabled = False
        Me.txtSssEmployeeShare.Size = New System.Drawing.Size(100, 20)
        Me.txtSssEmployeeShare.TabIndex = 183
        Me.txtSssEmployeeShare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPhilHealthEmployeeShare
        '
        Me.txtPhilHealthEmployeeShare.BackColor = System.Drawing.Color.White
        Me.txtPhilHealthEmployeeShare.Location = New System.Drawing.Point(630, 289)
        Me.txtPhilHealthEmployeeShare.Name = "txtPhilHealthEmployeeShare"
        Me.txtPhilHealthEmployeeShare.ReadOnly = True
        Me.txtPhilHealthEmployeeShare.ShortcutsEnabled = False
        Me.txtPhilHealthEmployeeShare.Size = New System.Drawing.Size(100, 20)
        Me.txtPhilHealthEmployeeShare.TabIndex = 184
        Me.txtPhilHealthEmployeeShare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(387, 296)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(110, 13)
        Me.Label9.TabIndex = 186
        Me.Label9.Text = "Employee PhilHealth :"
        '
        'txtHdmfEmployeeShare
        '
        Me.txtHdmfEmployeeShare.BackColor = System.Drawing.Color.White
        Me.txtHdmfEmployeeShare.Location = New System.Drawing.Point(630, 315)
        Me.txtHdmfEmployeeShare.Name = "txtHdmfEmployeeShare"
        Me.txtHdmfEmployeeShare.ReadOnly = True
        Me.txtHdmfEmployeeShare.ShortcutsEnabled = False
        Me.txtHdmfEmployeeShare.Size = New System.Drawing.Size(100, 20)
        Me.txtHdmfEmployeeShare.TabIndex = 190
        Me.txtHdmfEmployeeShare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(387, 322)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(105, 13)
        Me.Label10.TabIndex = 191
        Me.Label10.Text = "Employee PAGIBIG :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(387, 376)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 13)
        Me.Label3.TabIndex = 179
        Me.Label3.Text = "Taxable income :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtNetPay
        '
        Me.txtNetPay.BackColor = System.Drawing.Color.White
        Me.txtNetPay.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtNetPay.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!)
        Me.txtNetPay.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtNetPay.Location = New System.Drawing.Point(631, 591)
        Me.txtNetPay.Name = "txtNetPay"
        Me.txtNetPay.ReadOnly = True
        Me.txtNetPay.ShortcutsEnabled = False
        Me.txtNetPay.Size = New System.Drawing.Size(127, 22)
        Me.txtNetPay.TabIndex = 177
        Me.txtNetPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtGrossPay
        '
        Me.txtGrossPay.BackColor = System.Drawing.Color.White
        Me.txtGrossPay.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtGrossPay.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtGrossPay.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtGrossPay.Location = New System.Drawing.Point(253, 573)
        Me.txtGrossPay.Name = "txtGrossPay"
        Me.txtGrossPay.ReadOnly = True
        Me.txtGrossPay.ShortcutsEnabled = False
        Me.txtGrossPay.Size = New System.Drawing.Size(100, 16)
        Me.txtGrossPay.TabIndex = 176
        Me.txtGrossPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label103
        '
        Me.Label103.AutoSize = True
        Me.Label103.Location = New System.Drawing.Point(232, 462)
        Me.Label103.Name = "Label103"
        Me.Label103.Size = New System.Drawing.Size(14, 13)
        Me.Label103.TabIndex = 259
        Me.Label103.Text = "₱"
        '
        'lblGrossIncomePesoSign
        '
        Me.lblGrossIncomePesoSign.AutoSize = True
        Me.lblGrossIncomePesoSign.Location = New System.Drawing.Point(233, 576)
        Me.lblGrossIncomePesoSign.Name = "lblGrossIncomePesoSign"
        Me.lblGrossIncomePesoSign.Size = New System.Drawing.Size(14, 13)
        Me.lblGrossIncomePesoSign.TabIndex = 260
        Me.lblGrossIncomePesoSign.Text = "₱"
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(232, 436)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(14, 13)
        Me.Label38.TabIndex = 259
        Me.Label38.Text = "₱"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(232, 397)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(14, 13)
        Me.Label36.TabIndex = 176
        Me.Label36.Text = "₱"
        Me.Label36.Visible = False
        '
        'Label94
        '
        Me.Label94.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label94.Location = New System.Drawing.Point(392, 549)
        Me.Label94.Name = "Label94"
        Me.Label94.Size = New System.Drawing.Size(367, 13)
        Me.Label94.TabIndex = 529
        Me.Label94.Text = "---------------------------------------------------------------------------------" &
    "----------------------"
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(609, 591)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(23, 24)
        Me.Label42.TabIndex = 263
        Me.Label42.Text = "₱"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GeneratePayrollToolStripMenuItem, Me.ManagePayrollToolStripDropDownButton, Me.tsbtnClose, Me.ToolStripSeparator1, Me.tsSearch, Me.tsbtnSearch, Me.ToolStripLabel8, Me.DeleteToolStripDropDownButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(961, 25)
        Me.ToolStrip1.TabIndex = 175
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'GeneratePayrollToolStripMenuItem
        '
        Me.GeneratePayrollToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.cash_register001
        Me.GeneratePayrollToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.GeneratePayrollToolStripMenuItem.Name = "GeneratePayrollToolStripMenuItem"
        Me.GeneratePayrollToolStripMenuItem.Size = New System.Drawing.Size(113, 22)
        Me.GeneratePayrollToolStripMenuItem.Text = "Generate Payroll"
        '
        'ManagePayrollToolStripDropDownButton
        '
        Me.ManagePayrollToolStripDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ManagePayslipsToolStripMenuItem, Me.PrintPaySlipToolStripMenuItem, Me.PrintPayrollSummaryToolStripMenuItem, Me.ExportNetPayDetailsToolStripMenuItem, Me.CostCenterReportToolStripMenuItem, Me.RecalculateThirteenthMonthPayToolStripMenuItem, Me.CancelPayrollToolStripMenuItem, Me.DeletePayrollToolStripMenuItem, Me.ClosePayrollToolStripMenuItem, Me.ReopenPayrollToolStripMenuItem, Me.OthersToolStripMenuItem})
        Me.ManagePayrollToolStripDropDownButton.Image = Global.AccuPay.My.Resources.Resources.checked_list0
        Me.ManagePayrollToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ManagePayrollToolStripDropDownButton.Name = "ManagePayrollToolStripDropDownButton"
        Me.ManagePayrollToolStripDropDownButton.Size = New System.Drawing.Size(118, 22)
        Me.ManagePayrollToolStripDropDownButton.Text = "Manage Payroll"
        '
        'ManagePayslipsToolStripMenuItem
        '
        Me.ManagePayslipsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ManagePrintPayslipsToolStripMenuItem, Me.ManageEmailPayslipsToolStripMenuItem})
        Me.ManagePayslipsToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.payroll
        Me.ManagePayslipsToolStripMenuItem.Name = "ManagePayslipsToolStripMenuItem"
        Me.ManagePayslipsToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.ManagePayslipsToolStripMenuItem.Text = "Manage Payslips"
        '
        'ManagePrintPayslipsToolStripMenuItem
        '
        Me.ManagePrintPayslipsToolStripMenuItem.Name = "ManagePrintPayslipsToolStripMenuItem"
        Me.ManagePrintPayslipsToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ManagePrintPayslipsToolStripMenuItem.Text = "Print Payslips"
        '
        'ManageEmailPayslipsToolStripMenuItem
        '
        Me.ManageEmailPayslipsToolStripMenuItem.Name = "ManageEmailPayslipsToolStripMenuItem"
        Me.ManageEmailPayslipsToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ManageEmailPayslipsToolStripMenuItem.Text = "Email Payslips"
        '
        'PrintPaySlipToolStripMenuItem
        '
        Me.PrintPaySlipToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PayslipDeclaredToolStripMenuItem, Me.PayslipActualToolStripMenuItem})
        Me.PrintPaySlipToolStripMenuItem.Image = CType(resources.GetObject("PrintPaySlipToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PrintPaySlipToolStripMenuItem.Name = "PrintPaySlipToolStripMenuItem"
        Me.PrintPaySlipToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.PrintPaySlipToolStripMenuItem.Text = "Print Payslips"
        '
        'PayslipDeclaredToolStripMenuItem
        '
        Me.PayslipDeclaredToolStripMenuItem.Name = "PayslipDeclaredToolStripMenuItem"
        Me.PayslipDeclaredToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.PayslipDeclaredToolStripMenuItem.Tag = "0"
        Me.PayslipDeclaredToolStripMenuItem.Text = "Declared"
        '
        'PayslipActualToolStripMenuItem
        '
        Me.PayslipActualToolStripMenuItem.Name = "PayslipActualToolStripMenuItem"
        Me.PayslipActualToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.PayslipActualToolStripMenuItem.Tag = "1"
        Me.PayslipActualToolStripMenuItem.Text = "Actual"
        '
        'PrintPayrollSummaryToolStripMenuItem
        '
        Me.PrintPayrollSummaryToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PayrollSummaryDeclaredToolStripMenuItem, Me.PayrollSummaryActualToolStripMenuItem})
        Me.PrintPayrollSummaryToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.timeattendance
        Me.PrintPayrollSummaryToolStripMenuItem.Name = "PrintPayrollSummaryToolStripMenuItem"
        Me.PrintPayrollSummaryToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.PrintPayrollSummaryToolStripMenuItem.Text = "Print Payroll Summary"
        '
        'PayrollSummaryDeclaredToolStripMenuItem
        '
        Me.PayrollSummaryDeclaredToolStripMenuItem.Name = "PayrollSummaryDeclaredToolStripMenuItem"
        Me.PayrollSummaryDeclaredToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.PayrollSummaryDeclaredToolStripMenuItem.Tag = "0"
        Me.PayrollSummaryDeclaredToolStripMenuItem.Text = "Declared"
        '
        'PayrollSummaryActualToolStripMenuItem
        '
        Me.PayrollSummaryActualToolStripMenuItem.Name = "PayrollSummaryActualToolStripMenuItem"
        Me.PayrollSummaryActualToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.PayrollSummaryActualToolStripMenuItem.Tag = "1"
        Me.PayrollSummaryActualToolStripMenuItem.Text = "Actual"
        '
        'ExportNetPayDetailsToolStripMenuItem
        '
        Me.ExportNetPayDetailsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportNetPayDeclaredToolStripMenuItem, Me.ExportNetPayActualToolStripMenuItem})
        Me.ExportNetPayDetailsToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.userid
        Me.ExportNetPayDetailsToolStripMenuItem.Name = "ExportNetPayDetailsToolStripMenuItem"
        Me.ExportNetPayDetailsToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.ExportNetPayDetailsToolStripMenuItem.Text = "Export Net Pay Details"
        '
        'ExportNetPayDeclaredToolStripMenuItem
        '
        Me.ExportNetPayDeclaredToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportNetPayDeclaredAllToolStripMenuItem, Me.ExportNetPayDeclaredCashToolStripMenuItem, Me.ExportNetPayDeclaredDirectDepositToolStripMenuItem})
        Me.ExportNetPayDeclaredToolStripMenuItem.Name = "ExportNetPayDeclaredToolStripMenuItem"
        Me.ExportNetPayDeclaredToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.ExportNetPayDeclaredToolStripMenuItem.Text = "Declared"
        '
        'ExportNetPayDeclaredAllToolStripMenuItem
        '
        Me.ExportNetPayDeclaredAllToolStripMenuItem.Name = "ExportNetPayDeclaredAllToolStripMenuItem"
        Me.ExportNetPayDeclaredAllToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ExportNetPayDeclaredAllToolStripMenuItem.Text = "All"
        '
        'ExportNetPayDeclaredCashToolStripMenuItem
        '
        Me.ExportNetPayDeclaredCashToolStripMenuItem.Name = "ExportNetPayDeclaredCashToolStripMenuItem"
        Me.ExportNetPayDeclaredCashToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ExportNetPayDeclaredCashToolStripMenuItem.Text = "Cash"
        '
        'ExportNetPayDeclaredDirectDepositToolStripMenuItem
        '
        Me.ExportNetPayDeclaredDirectDepositToolStripMenuItem.Name = "ExportNetPayDeclaredDirectDepositToolStripMenuItem"
        Me.ExportNetPayDeclaredDirectDepositToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ExportNetPayDeclaredDirectDepositToolStripMenuItem.Text = "Direct Deposit"
        '
        'ExportNetPayActualToolStripMenuItem
        '
        Me.ExportNetPayActualToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportNetPayActualAllToolStripMenuItem, Me.ExportNetPayActualCashToolStripMenuItem, Me.ExportNetPayActualDirectDepositToolStripMenuItem})
        Me.ExportNetPayActualToolStripMenuItem.Name = "ExportNetPayActualToolStripMenuItem"
        Me.ExportNetPayActualToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.ExportNetPayActualToolStripMenuItem.Text = "Actual"
        '
        'ExportNetPayActualAllToolStripMenuItem
        '
        Me.ExportNetPayActualAllToolStripMenuItem.Name = "ExportNetPayActualAllToolStripMenuItem"
        Me.ExportNetPayActualAllToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ExportNetPayActualAllToolStripMenuItem.Text = "All"
        '
        'ExportNetPayActualCashToolStripMenuItem
        '
        Me.ExportNetPayActualCashToolStripMenuItem.Name = "ExportNetPayActualCashToolStripMenuItem"
        Me.ExportNetPayActualCashToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ExportNetPayActualCashToolStripMenuItem.Text = "Cash"
        '
        'ExportNetPayActualDirectDepositToolStripMenuItem
        '
        Me.ExportNetPayActualDirectDepositToolStripMenuItem.Name = "ExportNetPayActualDirectDepositToolStripMenuItem"
        Me.ExportNetPayActualDirectDepositToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ExportNetPayActualDirectDepositToolStripMenuItem.Text = "Direct Deposit"
        '
        'CostCenterReportToolStripMenuItem
        '
        Me.CostCenterReportToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.Document
        Me.CostCenterReportToolStripMenuItem.Name = "CostCenterReportToolStripMenuItem"
        Me.CostCenterReportToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.CostCenterReportToolStripMenuItem.Text = "Cost Center Report"
        '
        'RecalculateThirteenthMonthPayToolStripMenuItem
        '
        Me.RecalculateThirteenthMonthPayToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.cash_register
        Me.RecalculateThirteenthMonthPayToolStripMenuItem.Name = "RecalculateThirteenthMonthPayToolStripMenuItem"
        Me.RecalculateThirteenthMonthPayToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.RecalculateThirteenthMonthPayToolStripMenuItem.Text = "Recalculate 13th Month Pay"
        '
        'CancelPayrollToolStripMenuItem
        '
        Me.CancelPayrollToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.deleteuser
        Me.CancelPayrollToolStripMenuItem.Name = "CancelPayrollToolStripMenuItem"
        Me.CancelPayrollToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.CancelPayrollToolStripMenuItem.Text = "Cancel Payroll"
        Me.CancelPayrollToolStripMenuItem.ToolTipText = "Delete all paystubs and reset the pay period to ""PENDING"""
        '
        'DeletePayrollToolStripMenuItem
        '
        Me.DeletePayrollToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.DeletePayrollToolStripMenuItem.Name = "DeletePayrollToolStripMenuItem"
        Me.DeletePayrollToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.DeletePayrollToolStripMenuItem.Text = "Delete Payroll"
        Me.DeletePayrollToolStripMenuItem.ToolTipText = "Delete all paystubs"
        '
        'ClosePayrollToolStripMenuItem
        '
        Me.ClosePayrollToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.l_arrow
        Me.ClosePayrollToolStripMenuItem.Name = "ClosePayrollToolStripMenuItem"
        Me.ClosePayrollToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.ClosePayrollToolStripMenuItem.Text = "Close Payroll"
        Me.ClosePayrollToolStripMenuItem.Visible = False
        '
        'ReopenPayrollToolStripMenuItem
        '
        Me.ReopenPayrollToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.r_arrow
        Me.ReopenPayrollToolStripMenuItem.Name = "ReopenPayrollToolStripMenuItem"
        Me.ReopenPayrollToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.ReopenPayrollToolStripMenuItem.Text = "Reopen Payroll"
        Me.ReopenPayrollToolStripMenuItem.Visible = False
        '
        'OthersToolStripMenuItem
        '
        Me.OthersToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Include13thMonthPayToolStripMenuItem, Me.CashOutUnusedLeavesToolStripMenuItem})
        Me.OthersToolStripMenuItem.Image = Global.AccuPay.My.Resources.Resources.Documents_icon_64
        Me.OthersToolStripMenuItem.Name = "OthersToolStripMenuItem"
        Me.OthersToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.OthersToolStripMenuItem.Text = "Others"
        Me.OthersToolStripMenuItem.Visible = False
        '
        'Include13thMonthPayToolStripMenuItem
        '
        Me.Include13thMonthPayToolStripMenuItem.Name = "Include13thMonthPayToolStripMenuItem"
        Me.Include13thMonthPayToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.Include13thMonthPayToolStripMenuItem.Text = "Include 13th Month Pay"
        Me.Include13thMonthPayToolStripMenuItem.Visible = False
        '
        'CashOutUnusedLeavesToolStripMenuItem
        '
        Me.CashOutUnusedLeavesToolStripMenuItem.Name = "CashOutUnusedLeavesToolStripMenuItem"
        Me.CashOutUnusedLeavesToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.CashOutUnusedLeavesToolStripMenuItem.Text = "Cash Out Unused Leaves"
        Me.CashOutUnusedLeavesToolStripMenuItem.Visible = False
        '
        'tsbtnClose
        '
        Me.tsbtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnClose.Name = "tsbtnClose"
        Me.tsbtnClose.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnClose.Text = "Close"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsSearch
        '
        Me.tsSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tsSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.tsSearch.Name = "tsSearch"
        Me.tsSearch.Size = New System.Drawing.Size(145, 25)
        '
        'tsbtnSearch
        '
        Me.tsbtnSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnSearch.Image = Global.AccuPay.My.Resources.Resources.magnifier_zoom
        Me.tsbtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSearch.Name = "tsbtnSearch"
        Me.tsbtnSearch.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnSearch.Text = "Search Employee"
        '
        'ToolStripLabel8
        '
        Me.ToolStripLabel8.AutoSize = False
        Me.ToolStripLabel8.Name = "ToolStripLabel8"
        Me.ToolStripLabel8.Size = New System.Drawing.Size(66, 22)
        '
        'DeleteToolStripDropDownButton
        '
        Me.DeleteToolStripDropDownButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.DeleteToolStripDropDownButton.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.DeleteToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteToolStripDropDownButton.Name = "DeleteToolStripDropDownButton"
        Me.DeleteToolStripDropDownButton.Size = New System.Drawing.Size(105, 22)
        Me.DeleteToolStripDropDownButton.Text = "Delete Paystub"
        Me.DeleteToolStripDropDownButton.ToolTipText = "Deletes only the selected" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "payroll of an employee"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Icon_169.ico")
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.Button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Image = Global.AccuPay.My.Resources.Resources.r_arrow
        Me.Button3.Location = New System.Drawing.Point(213, 11)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(32, 23)
        Me.Button3.TabIndex = 177
        Me.Button3.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.Button3.UseVisualStyleBackColor = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.Panel5)
        Me.Panel4.Controls.Add(Me.dgvpayper)
        Me.Panel4.Controls.Add(Me.Button3)
        Me.Panel4.Controls.Add(Me.btnrefresh)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel4.Location = New System.Drawing.Point(0, 21)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(254, 526)
        Me.Panel4.TabIndex = 179
        '
        'Panel5
        '
        Me.Panel5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel5.Controls.Add(Me.linkPrev)
        Me.Panel5.Controls.Add(Me.linkNxt)
        Me.Panel5.Location = New System.Drawing.Point(10, 499)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(235, 20)
        Me.Panel5.TabIndex = 179
        '
        'Timer1
        '
        Me.Timer1.Interval = 2500
        '
        'ProgressTimer
        '
        '
        'PayStubForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(185, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1229, 547)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Label25)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "PayStubForm"
        Me.Text = "paystub"
        CType(Me.dgvpayper, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvemployees, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.tbppayroll.ResumeLayout(False)
        Me.tbppayroll.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.pbEmpPicChk, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.tabEarned.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        CType(Me.dgvAdjustments, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvpayper As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents linkPrev As System.Windows.Forms.LinkLabel
    Friend WithEvents linkNxt As System.Windows.Forms.LinkLabel
    Friend WithEvents pbEmpPicChk As System.Windows.Forms.PictureBox
    Friend WithEvents txtFName As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpID As System.Windows.Forms.TextBox
    Friend WithEvents Last As System.Windows.Forms.LinkLabel
    Friend WithEvents Nxt As System.Windows.Forms.LinkLabel
    Friend WithEvents Prev As System.Windows.Forms.LinkLabel
    Friend WithEvents First As System.Windows.Forms.LinkLabel
    Friend WithEvents btnrefresh As System.Windows.Forms.Button
    Friend WithEvents lblGrossIncome As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dgvemployees As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tbppayroll As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents txtGrossPay As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalTaxableSalary As System.Windows.Forms.TextBox
    Friend WithEvents txtNetPay As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtPhilHealthEmployeeShare As System.Windows.Forms.TextBox
    Friend WithEvents txtSssEmployeeShare As System.Windows.Forms.TextBox
    Friend WithEvents txtWithholdingTax As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtHdmfEmployeeShare As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtTotalAllowance As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtTotalBonus As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalLoans As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtUndertimeHours As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtLateHours As System.Windows.Forms.TextBox
    Friend WithEvents txtAbsentHours As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtHolidayHours As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtNightDiffOvertimePay As System.Windows.Forms.TextBox
    Friend WithEvents txtNightDiffPay As System.Windows.Forms.TextBox
    Friend WithEvents txtOvertimePay As System.Windows.Forms.TextBox
    Friend WithEvents txtBasicRate As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txttotreghrs As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txttotregamt As System.Windows.Forms.TextBox
    Friend WithEvents txtOvertimeHours As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtNightDiffOvertimeHours As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents txtNightDiffHours As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents txtHolidayPay As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtLateDeduction As System.Windows.Forms.TextBox
    Friend WithEvents txtAbsenceDeduction As System.Windows.Forms.TextBox
    Friend WithEvents txtUndertimeDeduction As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents lblGrossIncomeDivider As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents btntotallow As System.Windows.Forms.Button
    Friend WithEvents btntotloan As System.Windows.Forms.Button
    Friend WithEvents btntotbon As System.Windows.Forms.Button
    Friend WithEvents txtRegularPay As System.Windows.Forms.TextBox
    Friend WithEvents txtRegularHours As System.Windows.Forms.TextBox
    Friend WithEvents lblPaidLeave As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents lblSubtotal As System.Windows.Forms.TextBox
    Friend WithEvents lblsubtotmisc As System.Windows.Forms.TextBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents lblGrossIncomePesoSign As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents tsSearch As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtnSearch As System.Windows.Forms.ToolStripButton
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel3 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel4 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents tstrip As System.Windows.Forms.ToolStrip
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents dgvAdjustments As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents btnSaveAdjustments As System.Windows.Forms.Button
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents txtTotalAdjustments As System.Windows.Forms.TextBox
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents tabEarned As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents txtRegularPayActual As System.Windows.Forms.TextBox
    Friend WithEvents txtRegularHoursActual As System.Windows.Forms.TextBox
    Friend WithEvents Label69 As System.Windows.Forms.Label
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents txtRegularHolidayPay As System.Windows.Forms.TextBox
    Friend WithEvents NightDiffOvertimeHoursActual As System.Windows.Forms.TextBox
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents txtNightDiffHoursActual As System.Windows.Forms.TextBox
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents txtOvertimeHoursActual As System.Windows.Forms.TextBox
    Friend WithEvents Label73 As System.Windows.Forms.Label
    Friend WithEvents Label74 As System.Windows.Forms.Label
    Friend WithEvents txtBasicRateActual As System.Windows.Forms.TextBox
    Friend WithEvents Label75 As System.Windows.Forms.Label
    Friend WithEvents txtRegularHolidayHours As System.Windows.Forms.TextBox
    Friend WithEvents Label76 As System.Windows.Forms.Label
    Friend WithEvents txtNightDiffOvertimePayActual As System.Windows.Forms.TextBox
    Friend WithEvents txtNightDiffPayActual As System.Windows.Forms.TextBox
    Friend WithEvents txtOvertimePayActual As System.Windows.Forms.TextBox
    Friend WithEvents Label77 As System.Windows.Forms.Label
    Friend WithEvents Label78 As System.Windows.Forms.Label
    Friend WithEvents Label79 As System.Windows.Forms.Label
    Friend WithEvents Label80 As System.Windows.Forms.Label
    Friend WithEvents Label81 As System.Windows.Forms.Label
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Friend WithEvents Label83 As System.Windows.Forms.Label
    Friend WithEvents Label84 As System.Windows.Forms.Label
    Friend WithEvents txtUndertimeDeductionActual As System.Windows.Forms.TextBox
    Friend WithEvents txtLateDeductionActual As System.Windows.Forms.TextBox
    Friend WithEvents txtAbsenceDeductionActual As System.Windows.Forms.TextBox
    Friend WithEvents Label88 As System.Windows.Forms.Label
    Friend WithEvents Label89 As System.Windows.Forms.Label
    Friend WithEvents Label90 As System.Windows.Forms.Label
    Friend WithEvents txtUndertimeHoursActual As System.Windows.Forms.TextBox
    Friend WithEvents Label91 As System.Windows.Forms.Label
    Friend WithEvents txtLateHoursActual As System.Windows.Forms.TextBox
    Friend WithEvents txtAbsentHoursActual As System.Windows.Forms.TextBox
    Friend WithEvents Label92 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btndiscardchanges As System.Windows.Forms.Button
    Friend WithEvents LinkLabel5 As System.Windows.Forms.LinkLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents ToolStripLabel8 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents DeleteToolStripDropDownButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents psaRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cboProducts As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn66 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn64 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column15 As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents IsAdjustmentActual As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents txtLeavePay As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lblAgencyFee As System.Windows.Forms.Label
    Friend WithEvents txtAgencyFee As System.Windows.Forms.TextBox
    Friend WithEvents Label87 As System.Windows.Forms.Label
    Friend WithEvents lblTotalNetPay As System.Windows.Forms.Label
    Friend WithEvents txtTotalNetPay As System.Windows.Forms.TextBox
    Friend WithEvents Label85 As System.Windows.Forms.Label
    Friend WithEvents Label93 As System.Windows.Forms.Label
    Friend WithEvents txtThirteenthMonthPay As System.Windows.Forms.TextBox
    Friend WithEvents Label94 As System.Windows.Forms.Label
    Friend WithEvents Label95 As System.Windows.Forms.Label
    Friend WithEvents Label96 As Label
    Friend WithEvents txtRestDayPay As TextBox
    Friend WithEvents txtRestDayHours As TextBox
    Friend WithEvents txtRestDayOtHour As TextBox
    Friend WithEvents Label102 As Label
    Friend WithEvents txtRestDayOtPay As TextBox
    Friend WithEvents Label60 As Label
    Friend WithEvents Label64 As Label
    Friend WithEvents txtSpecialHolidayOTHours As TextBox
    Friend WithEvents txtRegularHolidayOTPay As TextBox
    Friend WithEvents txtSpecialHolidayHours As TextBox
    Friend WithEvents Label101 As Label
    Friend WithEvents Label100 As Label
    Friend WithEvents Label98 As Label
    Friend WithEvents Label99 As Label
    Friend WithEvents txtSpecialHolidayPay As TextBox
    Friend WithEvents txtRegularHolidayOTHours As TextBox
    Friend WithEvents Label97 As Label
    Friend WithEvents txtSpecialHolidayOTPay As TextBox
    Friend WithEvents Label63 As Label
    Friend WithEvents Panel6 As Panel
    Friend WithEvents btnTotalTaxabAllowance As Button
    Friend WithEvents txtTotalTaxableAllowance As TextBox
    Friend WithEvents Label104 As Label
    Friend WithEvents Label103 As Label
    Friend WithEvents Label105 As Label
    Friend WithEvents txtGrandTotalAllow As TextBox
    Friend WithEvents Label106 As Label
    Friend WithEvents Label107 As Label
    Friend WithEvents ManagePayrollToolStripDropDownButton As ToolStripDropDownButton
    Friend WithEvents PrintPaySlipToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PayslipDeclaredToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PayslipActualToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PrintPayrollSummaryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PayrollSummaryDeclaredToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PayrollSummaryActualToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ClosePayrollToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReopenPayrollToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GeneratePayrollToolStripMenuItem As ToolStripButton
    Friend WithEvents OthersToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Include13thMonthPayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CashOutUnusedLeavesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeletePayrollToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ManagePayslipsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ManagePrintPayslipsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ManageEmailPayslipsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CostCenterReportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ProgressTimer As Timer
    Friend WithEvents RecalculateThirteenthMonthPayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lblPaidLeavePesoSign As Label
    Friend WithEvents txtLeaveHours As TextBox
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents PayDateFrom As DataGridViewTextBoxColumn
    Friend WithEvents PayDateTo As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
    Friend WithEvents Column8 As DataGridViewTextBoxColumn
    Friend WithEvents Column9 As DataGridViewTextBoxColumn
    Friend WithEvents Column10 As DataGridViewTextBoxColumn
    Friend WithEvents Column11 As DataGridViewTextBoxColumn
    Friend WithEvents Column12 As DataGridViewTextBoxColumn
    Friend WithEvents Column13 As DataGridViewTextBoxColumn
    Friend WithEvents Column14 As DataGridViewTextBoxColumn
    Friend WithEvents StatusColumn As DataGridViewTextBoxColumn
    Friend WithEvents CancelPayrollToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RowID As DataGridViewTextBoxColumn
    Friend WithEvents EmployeeID As DataGridViewTextBoxColumn
    Friend WithEvents LastName As DataGridViewTextBoxColumn
    Friend WithEvents FirstName As DataGridViewTextBoxColumn
    Friend WithEvents MiddleName As DataGridViewTextBoxColumn
    Friend WithEvents EmployeeType As DataGridViewTextBoxColumn
    Friend WithEvents Position As DataGridViewTextBoxColumn
    Friend WithEvents Division As DataGridViewTextBoxColumn
    Friend WithEvents ExportNetPayDetailsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportNetPayDeclaredToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportNetPayDeclaredAllToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportNetPayDeclaredCashToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportNetPayDeclaredDirectDepositToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportNetPayActualToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportNetPayActualAllToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportNetPayActualCashToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportNetPayActualDirectDepositToolStripMenuItem As ToolStripMenuItem
End Class