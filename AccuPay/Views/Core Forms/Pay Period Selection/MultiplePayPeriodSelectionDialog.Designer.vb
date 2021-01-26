<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MultiplePayPeriodSelectionDialog
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PayperiodsGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.IsCheckedColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DateFromColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateToColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.linkNxt = New System.Windows.Forms.LinkLabel()
        Me.linkPrev = New System.Windows.Forms.LinkLabel()
        Me.DateFromLabel = New System.Windows.Forms.Label()
        Me.DateToLabel = New System.Windows.Forms.Label()
        Me.DateFromTextLabel = New System.Windows.Forms.Label()
        Me.DateToTextLabel = New System.Windows.Forms.Label()
        Me.SalaryDistributionComboBox = New System.Windows.Forms.ComboBox()
        Me.SalaryDistributionRequiredFieldLabel = New System.Windows.Forms.Label()
        Me.ReminderLabel = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.SemiMonthlyTab = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.DeclaredOrActualPanel = New System.Windows.Forms.Panel()
        Me.OrLabel = New System.Windows.Forms.Label()
        Me.ActualRadioButton = New System.Windows.Forms.RadioButton()
        Me.DeclaredRadioButton = New System.Windows.Forms.RadioButton()
        Me.PayrollSummaryPanel = New System.Windows.Forms.Panel()
        Me.SalaryDistributionLabel = New System.Windows.Forms.Label()
        Me.chkHideEmptyColumns = New System.Windows.Forms.CheckBox()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.LoanTypePanel = New System.Windows.Forms.Panel()
        Me.LoanTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.LoanTypeLabel = New System.Windows.Forms.Label()
        CType(Me.PayperiodsGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.DeclaredOrActualPanel.SuspendLayout()
        Me.PayrollSummaryPanel.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.LoanTypePanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'PayperiodsGridView
        '
        Me.PayperiodsGridView.AllowUserToAddRows = False
        Me.PayperiodsGridView.AllowUserToDeleteRows = False
        Me.PayperiodsGridView.AllowUserToOrderColumns = True
        Me.PayperiodsGridView.AllowUserToResizeColumns = False
        Me.PayperiodsGridView.AllowUserToResizeRows = False
        Me.PayperiodsGridView.BackgroundColor = System.Drawing.Color.White
        Me.PayperiodsGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.PayperiodsGridView.ColumnHeadersHeight = 34
        Me.PayperiodsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.PayperiodsGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IsCheckedColumn, Me.DateFromColumn, Me.DateToColumn})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.PayperiodsGridView.DefaultCellStyle = DataGridViewCellStyle1
        Me.PayperiodsGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PayperiodsGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.PayperiodsGridView.Location = New System.Drawing.Point(0, 39)
        Me.PayperiodsGridView.MultiSelect = False
        Me.PayperiodsGridView.Name = "PayperiodsGridView"
        Me.PayperiodsGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.PayperiodsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.PayperiodsGridView.Size = New System.Drawing.Size(438, 521)
        Me.PayperiodsGridView.TabIndex = 0
        '
        'IsCheckedColumn
        '
        Me.IsCheckedColumn.DataPropertyName = "IsChecked"
        Me.IsCheckedColumn.HeaderText = ""
        Me.IsCheckedColumn.Name = "IsCheckedColumn"
        Me.IsCheckedColumn.Width = 28
        '
        'DateFromColumn
        '
        Me.DateFromColumn.DataPropertyName = "DateFrom"
        Me.DateFromColumn.HeaderText = "Date from"
        Me.DateFromColumn.Name = "DateFromColumn"
        Me.DateFromColumn.ReadOnly = True
        Me.DateFromColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DateFromColumn.Width = 175
        '
        'DateToColumn
        '
        Me.DateToColumn.DataPropertyName = "DateTo"
        Me.DateToColumn.HeaderText = "Date to"
        Me.DateToColumn.Name = "DateToColumn"
        Me.DateToColumn.ReadOnly = True
        Me.DateToColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DateToColumn.Width = 175
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(359, 17)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 35)
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(278, 17)
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
        'DateFromLabel
        '
        Me.DateFromLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DateFromLabel.AutoSize = True
        Me.DateFromLabel.Location = New System.Drawing.Point(12, 17)
        Me.DateFromLabel.Name = "DateFromLabel"
        Me.DateFromLabel.Size = New System.Drawing.Size(36, 13)
        Me.DateFromLabel.TabIndex = 9
        Me.DateFromLabel.Text = "From :"
        '
        'DateToLabel
        '
        Me.DateToLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DateToLabel.AutoSize = True
        Me.DateToLabel.Location = New System.Drawing.Point(22, 39)
        Me.DateToLabel.Name = "DateToLabel"
        Me.DateToLabel.Size = New System.Drawing.Size(26, 13)
        Me.DateToLabel.TabIndex = 10
        Me.DateToLabel.Text = "To :"
        '
        'DateFromTextLabel
        '
        Me.DateFromTextLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DateFromTextLabel.AutoSize = True
        Me.DateFromTextLabel.Location = New System.Drawing.Point(54, 17)
        Me.DateFromTextLabel.Name = "DateFromTextLabel"
        Me.DateFromTextLabel.Size = New System.Drawing.Size(65, 13)
        Me.DateFromTextLabel.TabIndex = 11
        Me.DateFromTextLabel.Text = "<DateFrom>"
        '
        'DateToTextLabel
        '
        Me.DateToTextLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DateToTextLabel.AutoSize = True
        Me.DateToTextLabel.Location = New System.Drawing.Point(54, 39)
        Me.DateToTextLabel.Name = "DateToTextLabel"
        Me.DateToTextLabel.Size = New System.Drawing.Size(55, 13)
        Me.DateToTextLabel.TabIndex = 12
        Me.DateToTextLabel.Text = "<DateTo>"
        '
        'SalaryDistributionComboBox
        '
        Me.SalaryDistributionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.SalaryDistributionComboBox.FormattingEnabled = True
        Me.SalaryDistributionComboBox.Location = New System.Drawing.Point(177, 3)
        Me.SalaryDistributionComboBox.Name = "SalaryDistributionComboBox"
        Me.SalaryDistributionComboBox.Size = New System.Drawing.Size(176, 21)
        Me.SalaryDistributionComboBox.TabIndex = 7
        '
        'SalaryDistributionRequiredFieldLabel
        '
        Me.SalaryDistributionRequiredFieldLabel.AutoSize = True
        Me.SalaryDistributionRequiredFieldLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.SalaryDistributionRequiredFieldLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.SalaryDistributionRequiredFieldLabel.Location = New System.Drawing.Point(153, 3)
        Me.SalaryDistributionRequiredFieldLabel.Name = "SalaryDistributionRequiredFieldLabel"
        Me.SalaryDistributionRequiredFieldLabel.Size = New System.Drawing.Size(18, 24)
        Me.SalaryDistributionRequiredFieldLabel.TabIndex = 520
        Me.SalaryDistributionRequiredFieldLabel.Text = "*"
        '
        'ReminderLabel
        '
        Me.ReminderLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.ReminderLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ReminderLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ReminderLabel.ForeColor = System.Drawing.Color.White
        Me.ReminderLabel.Location = New System.Drawing.Point(0, 741)
        Me.ReminderLabel.Name = "ReminderLabel"
        Me.ReminderLabel.Size = New System.Drawing.Size(438, 20)
        Me.ReminderLabel.TabIndex = 521
        Me.ReminderLabel.Text = "Note ! Asterisk is a required field"
        Me.ReminderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ReminderLabel.Visible = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.SemiMonthlyTab)
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
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.linkNxt)
        Me.Panel1.Controls.Add(Me.linkPrev)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 560)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(438, 15)
        Me.Panel1.TabIndex = 523
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.DateFromTextLabel)
        Me.Panel2.Controls.Add(Me.DateToTextLabel)
        Me.Panel2.Controls.Add(Me.DateFromLabel)
        Me.Panel2.Controls.Add(Me.DateToLabel)
        Me.Panel2.Controls.Add(Me.btnOK)
        Me.Panel2.Controls.Add(Me.btnClose)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 685)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(438, 56)
        Me.Panel2.TabIndex = 524
        '
        'DeclaredOrActualPanel
        '
        Me.DeclaredOrActualPanel.Controls.Add(Me.OrLabel)
        Me.DeclaredOrActualPanel.Controls.Add(Me.ActualRadioButton)
        Me.DeclaredOrActualPanel.Controls.Add(Me.DeclaredRadioButton)
        Me.DeclaredOrActualPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DeclaredOrActualPanel.Location = New System.Drawing.Point(0, 655)
        Me.DeclaredOrActualPanel.Name = "DeclaredOrActualPanel"
        Me.DeclaredOrActualPanel.Size = New System.Drawing.Size(438, 30)
        Me.DeclaredOrActualPanel.TabIndex = 525
        Me.DeclaredOrActualPanel.Visible = False
        '
        'OrLabel
        '
        Me.OrLabel.AutoSize = True
        Me.OrLabel.Location = New System.Drawing.Point(251, 9)
        Me.OrLabel.Name = "OrLabel"
        Me.OrLabel.Size = New System.Drawing.Size(16, 13)
        Me.OrLabel.TabIndex = 2
        Me.OrLabel.Text = "or"
        '
        'ActualRadioButton
        '
        Me.ActualRadioButton.AutoSize = True
        Me.ActualRadioButton.Location = New System.Drawing.Point(278, 7)
        Me.ActualRadioButton.Name = "ActualRadioButton"
        Me.ActualRadioButton.Size = New System.Drawing.Size(55, 17)
        Me.ActualRadioButton.TabIndex = 1
        Me.ActualRadioButton.Text = "Actual"
        Me.ActualRadioButton.UseVisualStyleBackColor = True
        '
        'DeclaredRadioButton
        '
        Me.DeclaredRadioButton.AutoSize = True
        Me.DeclaredRadioButton.Checked = True
        Me.DeclaredRadioButton.Location = New System.Drawing.Point(177, 7)
        Me.DeclaredRadioButton.Name = "DeclaredRadioButton"
        Me.DeclaredRadioButton.Size = New System.Drawing.Size(68, 17)
        Me.DeclaredRadioButton.TabIndex = 0
        Me.DeclaredRadioButton.TabStop = True
        Me.DeclaredRadioButton.Text = "Declared"
        Me.DeclaredRadioButton.UseVisualStyleBackColor = True
        '
        'PayrollSummaryPanel
        '
        Me.PayrollSummaryPanel.Controls.Add(Me.SalaryDistributionLabel)
        Me.PayrollSummaryPanel.Controls.Add(Me.chkHideEmptyColumns)
        Me.PayrollSummaryPanel.Controls.Add(Me.SalaryDistributionComboBox)
        Me.PayrollSummaryPanel.Controls.Add(Me.SalaryDistributionRequiredFieldLabel)
        Me.PayrollSummaryPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PayrollSummaryPanel.Location = New System.Drawing.Point(0, 605)
        Me.PayrollSummaryPanel.Name = "PayrollSummaryPanel"
        Me.PayrollSummaryPanel.Size = New System.Drawing.Size(438, 50)
        Me.PayrollSummaryPanel.TabIndex = 526
        Me.PayrollSummaryPanel.Visible = False
        '
        'SalaryDistributionLabel
        '
        Me.SalaryDistributionLabel.AutoSize = True
        Me.SalaryDistributionLabel.Location = New System.Drawing.Point(47, 11)
        Me.SalaryDistributionLabel.Name = "SalaryDistributionLabel"
        Me.SalaryDistributionLabel.Size = New System.Drawing.Size(91, 13)
        Me.SalaryDistributionLabel.TabIndex = 2
        Me.SalaryDistributionLabel.Text = "Salary Distribution"
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
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.PayperiodsGridView)
        Me.Panel4.Controls.Add(Me.TabControl1)
        Me.Panel4.Controls.Add(Me.Panel1)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(438, 575)
        Me.Panel4.TabIndex = 527
        '
        'LoanTypePanel
        '
        Me.LoanTypePanel.Controls.Add(Me.LoanTypeComboBox)
        Me.LoanTypePanel.Controls.Add(Me.LoanTypeLabel)
        Me.LoanTypePanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LoanTypePanel.Location = New System.Drawing.Point(0, 575)
        Me.LoanTypePanel.Name = "LoanTypePanel"
        Me.LoanTypePanel.Size = New System.Drawing.Size(438, 30)
        Me.LoanTypePanel.TabIndex = 528
        Me.LoanTypePanel.Visible = False
        '
        'LoanTypeComboBox
        '
        Me.LoanTypeComboBox.DisplayMember = "PartNo"
        Me.LoanTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LoanTypeComboBox.FormattingEnabled = True
        Me.LoanTypeComboBox.Location = New System.Drawing.Point(121, 4)
        Me.LoanTypeComboBox.Name = "LoanTypeComboBox"
        Me.LoanTypeComboBox.Size = New System.Drawing.Size(241, 21)
        Me.LoanTypeComboBox.TabIndex = 1
        Me.LoanTypeComboBox.ValueMember = "RowID"
        '
        'LoanTypeLabel
        '
        Me.LoanTypeLabel.AutoSize = True
        Me.LoanTypeLabel.Location = New System.Drawing.Point(57, 7)
        Me.LoanTypeLabel.Name = "LoanTypeLabel"
        Me.LoanTypeLabel.Size = New System.Drawing.Size(58, 13)
        Me.LoanTypeLabel.TabIndex = 0
        Me.LoanTypeLabel.Text = "Loan Type"
        '
        'MultiplePayPeriodSelectionDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(438, 761)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.LoanTypePanel)
        Me.Controls.Add(Me.PayrollSummaryPanel)
        Me.Controls.Add(Me.DeclaredOrActualPanel)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ReminderLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MultiplePayPeriodSelectionDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.PayperiodsGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.DeclaredOrActualPanel.ResumeLayout(False)
        Me.DeclaredOrActualPanel.PerformLayout()
        Me.PayrollSummaryPanel.ResumeLayout(False)
        Me.PayrollSummaryPanel.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.LoanTypePanel.ResumeLayout(False)
        Me.LoanTypePanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PayperiodsGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents linkNxt As System.Windows.Forms.LinkLabel
    Friend WithEvents linkPrev As System.Windows.Forms.LinkLabel
    Friend WithEvents DateFromLabel As System.Windows.Forms.Label
    Friend WithEvents DateToLabel As System.Windows.Forms.Label
    Friend WithEvents DateFromTextLabel As System.Windows.Forms.Label
    Friend WithEvents DateToTextLabel As System.Windows.Forms.Label
    Friend WithEvents SalaryDistributionComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents SalaryDistributionRequiredFieldLabel As System.Windows.Forms.Label
    Friend WithEvents ReminderLabel As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents SemiMonthlyTab As System.Windows.Forms.TabPage
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents DeclaredOrActualPanel As Panel
    Friend WithEvents DeclaredRadioButton As RadioButton
    Friend WithEvents ActualRadioButton As RadioButton
    Friend WithEvents OrLabel As Label
    Friend WithEvents PayrollSummaryPanel As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents LoanTypePanel As Panel
    Friend WithEvents LoanTypeComboBox As ComboBox
    Friend WithEvents LoanTypeLabel As Label
    Friend WithEvents chkHideEmptyColumns As CheckBox
    Friend WithEvents IsCheckedColumn As DataGridViewCheckBoxColumn
    Friend WithEvents DateFromColumn As DataGridViewTextBoxColumn
    Friend WithEvents DateToColumn As DataGridViewTextBoxColumn
    Friend WithEvents SalaryDistributionLabel As Label
End Class
