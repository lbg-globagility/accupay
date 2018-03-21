<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmployeeBonusForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EmployeeBonusForm))
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvempbon = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.SelectionBox = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.bon_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bon_Type = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.bon_Amount = New AccuPay.DataGridViewNumberColumn()
        Me.bon_Frequency = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.bon_Start = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.bon_End = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.bon_ProdID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RemainingBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewBon = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveBon = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancelBon = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnDelBon = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnClose = New System.Windows.Forms.ToolStripButton()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.dgvpayperiod = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.ppRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ppProperDateFrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ppProperDateTo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ppPayFromDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ppPayToDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvempbon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvpayperiod, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvempbon
        '
        Me.dgvempbon.BackgroundColor = System.Drawing.Color.White
        Me.dgvempbon.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.SelectionBox, Me.bon_RowID, Me.bon_Type, Me.bon_Amount, Me.bon_Frequency, Me.bon_Start, Me.bon_End, Me.bon_ProdID, Me.RemainingBalance})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvempbon.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvempbon.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvempbon.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvempbon.Location = New System.Drawing.Point(240, 25)
        Me.dgvempbon.MultiSelect = False
        Me.dgvempbon.Name = "dgvempbon"
        Me.dgvempbon.Size = New System.Drawing.Size(540, 354)
        Me.dgvempbon.TabIndex = 3
        '
        'SelectionBox
        '
        Me.SelectionBox.HeaderText = ""
        Me.SelectionBox.Name = "SelectionBox"
        Me.SelectionBox.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.SelectionBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.SelectionBox.Width = 50
        '
        'bon_RowID
        '
        Me.bon_RowID.HeaderText = "RowID"
        Me.bon_RowID.Name = "bon_RowID"
        Me.bon_RowID.ReadOnly = True
        Me.bon_RowID.Visible = False
        '
        'bon_Type
        '
        Me.bon_Type.HeaderText = "Type"
        Me.bon_Type.Name = "bon_Type"
        Me.bon_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.bon_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.bon_Type.Width = 123
        '
        'bon_Amount
        '
        Me.bon_Amount.HeaderText = "Amount"
        Me.bon_Amount.MaxInputLength = 11
        Me.bon_Amount.Name = "bon_Amount"
        Me.bon_Amount.Width = 123
        '
        'bon_Frequency
        '
        Me.bon_Frequency.HeaderText = "Frequency"
        Me.bon_Frequency.Name = "bon_Frequency"
        Me.bon_Frequency.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.bon_Frequency.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.bon_Frequency.Width = 122
        '
        'bon_Start
        '
        '
        '
        '
        Me.bon_Start.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.bon_Start.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.bon_Start.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_Start.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.bon_Start.HeaderText = "Effective Start Date"
        Me.bon_Start.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.bon_Start.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.bon_Start.MonthCalendar.BackgroundStyle.Class = ""
        Me.bon_Start.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.bon_Start.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.bon_Start.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_Start.MonthCalendar.DisplayMonth = New Date(2016, 10, 1, 0, 0, 0, 0)
        Me.bon_Start.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.bon_Start.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.bon_Start.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.bon_Start.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_Start.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.bon_Start.Name = "bon_Start"
        Me.bon_Start.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.bon_Start.Width = 123
        '
        'bon_End
        '
        '
        '
        '
        Me.bon_End.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.bon_End.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.bon_End.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_End.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.bon_End.HeaderText = "Effective End Date"
        Me.bon_End.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.bon_End.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.bon_End.MonthCalendar.BackgroundStyle.Class = ""
        Me.bon_End.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.bon_End.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.bon_End.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_End.MonthCalendar.DisplayMonth = New Date(2016, 10, 1, 0, 0, 0, 0)
        Me.bon_End.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.bon_End.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.bon_End.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.bon_End.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.bon_End.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.bon_End.Name = "bon_End"
        Me.bon_End.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.bon_End.Width = 123
        '
        'bon_ProdID
        '
        Me.bon_ProdID.HeaderText = "ProductID"
        Me.bon_ProdID.Name = "bon_ProdID"
        Me.bon_ProdID.ReadOnly = True
        Me.bon_ProdID.Visible = False
        '
        'RemainingBalance
        '
        Me.RemainingBalance.HeaderText = "RemainingBalance"
        Me.RemainingBalance.Name = "RemainingBalance"
        Me.RemainingBalance.ReadOnly = True
        Me.RemainingBalance.Visible = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewBon, Me.tsbtnSaveBon, Me.tsbtnCancelBon, Me.ToolStripLabel1, Me.tsbtnDelBon, Me.tsbtnClose})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(780, 25)
        Me.ToolStrip1.TabIndex = 4
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnNewBon
        '
        Me.tsbtnNewBon.Image = CType(resources.GetObject("tsbtnNewBon.Image"), System.Drawing.Image)
        Me.tsbtnNewBon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewBon.Name = "tsbtnNewBon"
        Me.tsbtnNewBon.Size = New System.Drawing.Size(87, 22)
        Me.tsbtnNewBon.Text = "&New Bonus"
        '
        'tsbtnSaveBon
        '
        Me.tsbtnSaveBon.Image = CType(resources.GetObject("tsbtnSaveBon.Image"), System.Drawing.Image)
        Me.tsbtnSaveBon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveBon.Name = "tsbtnSaveBon"
        Me.tsbtnSaveBon.Size = New System.Drawing.Size(87, 22)
        Me.tsbtnSaveBon.Text = "&Save Bonus"
        '
        'tsbtnCancelBon
        '
        Me.tsbtnCancelBon.Image = CType(resources.GetObject("tsbtnCancelBon.Image"), System.Drawing.Image)
        Me.tsbtnCancelBon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancelBon.Name = "tsbtnCancelBon"
        Me.tsbtnCancelBon.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancelBon.Text = "Cancel"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.AutoSize = False
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(120, 22)
        '
        'tsbtnDelBon
        '
        Me.tsbtnDelBon.Image = CType(resources.GetObject("tsbtnDelBon.Image"), System.Drawing.Image)
        Me.tsbtnDelBon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDelBon.Name = "tsbtnDelBon"
        Me.tsbtnDelBon.Size = New System.Drawing.Size(96, 22)
        Me.tsbtnDelBon.Text = "Delete Bonus"
        '
        'tsbtnClose
        '
        Me.tsbtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnClose.Image = CType(resources.GetObject("tsbtnClose.Image"), System.Drawing.Image)
        Me.tsbtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnClose.Name = "tsbtnClose"
        Me.tsbtnClose.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnClose.Text = "Close"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 379)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(780, 35)
        Me.Panel1.TabIndex = 5
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(693, 6)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(612, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'dgvpayperiod
        '
        Me.dgvpayperiod.AllowUserToAddRows = False
        Me.dgvpayperiod.AllowUserToDeleteRows = False
        Me.dgvpayperiod.BackgroundColor = System.Drawing.Color.White
        Me.dgvpayperiod.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ppRowID, Me.ppProperDateFrom, Me.ppProperDateTo, Me.ppPayFromDate, Me.ppPayToDate, Me.Column6, Me.Column7, Me.Column8, Me.Column9, Me.Column10})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvpayperiod.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvpayperiod.Dock = System.Windows.Forms.DockStyle.Left
        Me.dgvpayperiod.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvpayperiod.Location = New System.Drawing.Point(0, 25)
        Me.dgvpayperiod.Name = "dgvpayperiod"
        Me.dgvpayperiod.ReadOnly = True
        Me.dgvpayperiod.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvpayperiod.Size = New System.Drawing.Size(240, 354)
        Me.dgvpayperiod.TabIndex = 6
        '
        'ppRowID
        '
        Me.ppRowID.HeaderText = "RowID"
        Me.ppRowID.Name = "ppRowID"
        Me.ppRowID.ReadOnly = True
        '
        'ppProperDateFrom
        '
        Me.ppProperDateFrom.HeaderText = "From Date"
        Me.ppProperDateFrom.Name = "ppProperDateFrom"
        Me.ppProperDateFrom.ReadOnly = True
        '
        'ppProperDateTo
        '
        Me.ppProperDateTo.HeaderText = "From Date"
        Me.ppProperDateTo.Name = "ppProperDateTo"
        Me.ppProperDateTo.ReadOnly = True
        '
        'ppPayFromDate
        '
        Me.ppPayFromDate.HeaderText = "Column4"
        Me.ppPayFromDate.Name = "ppPayFromDate"
        Me.ppPayFromDate.ReadOnly = True
        '
        'ppPayToDate
        '
        Me.ppPayToDate.HeaderText = "Column5"
        Me.ppPayToDate.Name = "ppPayToDate"
        Me.ppPayToDate.ReadOnly = True
        '
        'Column6
        '
        Me.Column6.HeaderText = "Column6"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'Column7
        '
        Me.Column7.HeaderText = "Column7"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        '
        'Column8
        '
        Me.Column8.HeaderText = "Column8"
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        '
        'Column9
        '
        Me.Column9.HeaderText = "Column9"
        Me.Column9.Name = "Column9"
        Me.Column9.ReadOnly = True
        '
        'Column10
        '
        Me.Column10.HeaderText = "Column10"
        Me.Column10.Name = "Column10"
        Me.Column10.ReadOnly = True
        '
        'EmployeeBonusForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(780, 414)
        Me.Controls.Add(Me.dgvempbon)
        Me.Controls.Add(Me.dgvpayperiod)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "EmployeeBonusForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        CType(Me.dgvempbon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.dgvpayperiod, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvempbon As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsbtnNewBon As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnSaveBon As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancelBon As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnDelBon As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents SelectionBox As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents bon_RowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bon_Type As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents bon_Amount As AccuPay.DataGridViewNumberColumn
    Friend WithEvents bon_Frequency As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents bon_Start As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents bon_End As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents bon_ProdID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RemainingBalance As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvpayperiod As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents ppRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ppProperDateFrom As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ppProperDateTo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ppPayFromDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ppPayToDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column10 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
