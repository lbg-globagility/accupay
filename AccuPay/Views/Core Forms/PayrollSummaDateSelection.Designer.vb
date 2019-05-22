<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PayrollSummaDateSelection
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvpayperiod = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Column1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.linkNxt = New System.Windows.Forms.LinkLabel()
        Me.linkPrev = New System.Windows.Forms.LinkLabel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DateFromLabel = New System.Windows.Forms.Label()
        Me.DateToLabel = New System.Windows.Forms.Label()
        Me.cboStringParameter = New System.Windows.Forms.ComboBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label360 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.SemiMonthlyTab = New System.Windows.Forms.TabPage()
        Me.WeeklyTab = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.panelSalarySwitch = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.cboxLoanType = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkHideEmptyColumns = New System.Windows.Forms.CheckBox()
        CType(Me.dgvpayperiod, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.panelSalarySwitch.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvpayperiod
        '
        Me.dgvpayperiod.AllowUserToAddRows = False
        Me.dgvpayperiod.AllowUserToDeleteRows = False
        Me.dgvpayperiod.AllowUserToOrderColumns = True
        Me.dgvpayperiod.AllowUserToResizeColumns = False
        Me.dgvpayperiod.AllowUserToResizeRows = False
        Me.dgvpayperiod.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvpayperiod.BackgroundColor = System.Drawing.Color.White
        Me.dgvpayperiod.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvpayperiod.ColumnHeadersHeight = 34
        Me.dgvpayperiod.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvpayperiod.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column6, Me.Column7, Me.Column4, Me.Column5})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvpayperiod.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvpayperiod.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvpayperiod.Location = New System.Drawing.Point(0, 39)
        Me.dgvpayperiod.MultiSelect = False
        Me.dgvpayperiod.Name = "dgvpayperiod"
        Me.dgvpayperiod.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvpayperiod.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvpayperiod.Size = New System.Drawing.Size(438, 345)
        Me.dgvpayperiod.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = ""
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 28
        '
        'Column2
        '
        Me.Column2.HeaderText = "Column2"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column2.Visible = False
        Me.Column2.Width = 175
        '
        'Column3
        '
        Me.Column3.HeaderText = "Column3"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column3.Visible = False
        Me.Column3.Width = 175
        '
        'Column6
        '
        Me.Column6.HeaderText = "Date from"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column6.Width = 175
        '
        'Column7
        '
        Me.Column7.HeaderText = "Date to"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column7.Width = 175
        '
        'Column4
        '
        Me.Column4.HeaderText = "Column4"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column4.Visible = False
        '
        'Column5
        '
        Me.Column5.HeaderText = "Column5"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column5.Visible = False
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(359, 7)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 35)
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(278, 7)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 35)
        Me.btnOK.TabIndex = 8
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'linkNxt
        '
        Me.linkNxt.AutoSize = True
        Me.linkNxt.Dock = System.Windows.Forms.DockStyle.Right
        Me.linkNxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkNxt.Location = New System.Drawing.Point(399, 0)
        Me.linkNxt.Name = "linkNxt"
        Me.linkNxt.Size = New System.Drawing.Size(39, 15)
        Me.linkNxt.TabIndex = 6
        Me.linkNxt.TabStop = True
        Me.linkNxt.Text = "Next>"
        '
        'linkPrev
        '
        Me.linkPrev.AutoSize = True
        Me.linkPrev.Dock = System.Windows.Forms.DockStyle.Left
        Me.linkPrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkPrev.Location = New System.Drawing.Point(0, 0)
        Me.linkPrev.Name = "linkPrev"
        Me.linkPrev.Size = New System.Drawing.Size(38, 15)
        Me.linkPrev.TabIndex = 5
        Me.linkPrev.TabStop = True
        Me.linkPrev.Text = "<Prev"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(36, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "From :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(22, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "To :"
        '
        'DateFromLabel
        '
        Me.DateFromLabel.AutoSize = True
        Me.DateFromLabel.Location = New System.Drawing.Point(54, 7)
        Me.DateFromLabel.Name = "DateFromLabel"
        Me.DateFromLabel.Size = New System.Drawing.Size(65, 13)
        Me.DateFromLabel.TabIndex = 11
        Me.DateFromLabel.Text = "<DateFrom>"
        '
        'DateToLabel
        '
        Me.DateToLabel.AutoSize = True
        Me.DateToLabel.Location = New System.Drawing.Point(54, 29)
        Me.DateToLabel.Name = "DateToLabel"
        Me.DateToLabel.Size = New System.Drawing.Size(55, 13)
        Me.DateToLabel.TabIndex = 12
        Me.DateToLabel.Text = "<DateTo>"
        '
        'cboStringParameter
        '
        Me.cboStringParameter.FormattingEnabled = True
        Me.cboStringParameter.Location = New System.Drawing.Point(177, 3)
        Me.cboStringParameter.Name = "cboStringParameter"
        Me.cboStringParameter.Size = New System.Drawing.Size(176, 21)
        Me.cboStringParameter.TabIndex = 7
        Me.cboStringParameter.Visible = False
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.White
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Location = New System.Drawing.Point(47, 11)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(100, 13)
        Me.TextBox1.TabIndex = 13
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextBox1.Visible = False
        '
        'Label360
        '
        Me.Label360.AutoSize = True
        Me.Label360.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label360.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label360.Location = New System.Drawing.Point(153, 3)
        Me.Label360.Name = "Label360"
        Me.Label360.Size = New System.Drawing.Size(18, 24)
        Me.Label360.TabIndex = 520
        Me.Label360.Text = "*"
        Me.Label360.Visible = False
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(0, 570)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(438, 20)
        Me.Label5.TabIndex = 521
        Me.Label5.Text = "Note ! Asterisk is a required field"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label5.Visible = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.SemiMonthlyTab)
        Me.TabControl1.Controls.Add(Me.WeeklyTab)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabControl1.Font = New System.Drawing.Font("Segoe UI Semibold", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(438, 39)
        Me.TabControl1.TabIndex = 522
        '
        'SemiMonthlyTab
        '
        Me.SemiMonthlyTab.Location = New System.Drawing.Point(4, 39)
        Me.SemiMonthlyTab.Name = "SemiMonthlyTab"
        Me.SemiMonthlyTab.Padding = New System.Windows.Forms.Padding(3)
        Me.SemiMonthlyTab.Size = New System.Drawing.Size(430, 0)
        Me.SemiMonthlyTab.TabIndex = 0
        Me.SemiMonthlyTab.Text = "SEMI-MONTHLY"
        Me.SemiMonthlyTab.UseVisualStyleBackColor = True
        '
        'WeeklyTab
        '
        Me.WeeklyTab.Location = New System.Drawing.Point(4, 39)
        Me.WeeklyTab.Name = "WeeklyTab"
        Me.WeeklyTab.Padding = New System.Windows.Forms.Padding(3)
        Me.WeeklyTab.Size = New System.Drawing.Size(430, 0)
        Me.WeeklyTab.TabIndex = 1
        Me.WeeklyTab.Text = "WEEKLY"
        Me.WeeklyTab.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.linkNxt)
        Me.Panel1.Controls.Add(Me.linkPrev)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 390)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(438, 15)
        Me.Panel1.TabIndex = 523
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.DateFromLabel)
        Me.Panel2.Controls.Add(Me.DateToLabel)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.btnOK)
        Me.Panel2.Controls.Add(Me.btnClose)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 524)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(438, 46)
        Me.Panel2.TabIndex = 524
        '
        'panelSalarySwitch
        '
        Me.panelSalarySwitch.Controls.Add(Me.Label3)
        Me.panelSalarySwitch.Controls.Add(Me.RadioButton2)
        Me.panelSalarySwitch.Controls.Add(Me.RadioButton1)
        Me.panelSalarySwitch.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelSalarySwitch.Location = New System.Drawing.Point(0, 494)
        Me.panelSalarySwitch.Name = "panelSalarySwitch"
        Me.panelSalarySwitch.Size = New System.Drawing.Size(438, 30)
        Me.panelSalarySwitch.TabIndex = 525
        Me.panelSalarySwitch.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(251, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(16, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "or"
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(278, 7)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(55, 17)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.Text = "Actual"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(177, 7)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(68, 17)
        Me.RadioButton1.TabIndex = 0
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Declared"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.chkHideEmptyColumns)
        Me.Panel3.Controls.Add(Me.cboStringParameter)
        Me.Panel3.Controls.Add(Me.Label360)
        Me.Panel3.Controls.Add(Me.TextBox1)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 434)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(438, 60)
        Me.Panel3.TabIndex = 526
        Me.Panel3.Visible = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.dgvpayperiod)
        Me.Panel4.Controls.Add(Me.TabControl1)
        Me.Panel4.Controls.Add(Me.Panel1)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(438, 405)
        Me.Panel4.TabIndex = 527
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.cboxLoanType)
        Me.Panel5.Controls.Add(Me.Label4)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel5.Location = New System.Drawing.Point(0, 405)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(438, 29)
        Me.Panel5.TabIndex = 528
        Me.Panel5.Visible = False
        '
        'cboxLoanType
        '
        Me.cboxLoanType.DisplayMember = "PartNo"
        Me.cboxLoanType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboxLoanType.FormattingEnabled = True
        Me.cboxLoanType.Location = New System.Drawing.Point(121, 4)
        Me.cboxLoanType.Name = "cboxLoanType"
        Me.cboxLoanType.Size = New System.Drawing.Size(241, 21)
        Me.cboxLoanType.TabIndex = 1
        Me.cboxLoanType.ValueMember = "RowID"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(57, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(58, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Loan Type"
        '
        'chkHideEmptyColumns
        '
        Me.chkHideEmptyColumns.AutoSize = True
        Me.chkHideEmptyColumns.Checked = True
        Me.chkHideEmptyColumns.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHideEmptyColumns.Location = New System.Drawing.Point(177, 30)
        Me.chkHideEmptyColumns.Name = "chkHideEmptyColumns"
        Me.chkHideEmptyColumns.Size = New System.Drawing.Size(121, 17)
        Me.chkHideEmptyColumns.TabIndex = 521
        Me.chkHideEmptyColumns.Text = "Hide empty columns"
        Me.chkHideEmptyColumns.UseVisualStyleBackColor = True
        '
        'PayrollSummaDateSelection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(438, 590)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.panelSalarySwitch)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label5)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PayrollSummaDateSelection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.dgvpayperiod, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.panelSalarySwitch.ResumeLayout(False)
        Me.panelSalarySwitch.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvpayperiod As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents linkNxt As System.Windows.Forms.LinkLabel
    Friend WithEvents linkPrev As System.Windows.Forms.LinkLabel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DateFromLabel As System.Windows.Forms.Label
    Friend WithEvents DateToLabel As System.Windows.Forms.Label
    Friend WithEvents cboStringParameter As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label360 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents SemiMonthlyTab As System.Windows.Forms.TabPage
    Friend WithEvents WeeklyTab As System.Windows.Forms.TabPage
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents panelSalarySwitch As Panel
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Panel5 As Panel
    Friend WithEvents cboxLoanType As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents chkHideEmptyColumns As CheckBox
End Class
