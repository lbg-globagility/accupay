<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BankFileTextFormatForm
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.dtpPayrollDate = New System.Windows.Forms.DateTimePicker()
        Me.gridPayroll = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Column1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lnkSelectPeriod = New System.Windows.Forms.LinkLabel()
        Me.numCompanyCode = New System.Windows.Forms.NumericUpDown()
        Me.numBatchNo = New System.Windows.Forms.NumericUpDown()
        Me.numPresentingOffice = New System.Windows.Forms.NumericUpDown()
        Me.numCeilingAmount = New System.Windows.Forms.NumericUpDown()
        Me.numFundingAccountNo = New System.Windows.Forms.NumericUpDown()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.DataGridViewCheckBoxColumn1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.gridPayroll, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.numCompanyCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numBatchNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numPresentingOffice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numCeilingAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numFundingAccountNo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "* Company Code"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "* Funding Account No.:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "* Ceiling Amount:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(311, 26)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "* Payroll Date:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(311, 54)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(107, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "* Presenting Office:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(311, 82)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 13)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "* Batch No.:"
        '
        'dtpPayrollDate
        '
        Me.dtpPayrollDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpPayrollDate.Location = New System.Drawing.Point(424, 18)
        Me.dtpPayrollDate.Name = "dtpPayrollDate"
        Me.dtpPayrollDate.Size = New System.Drawing.Size(101, 22)
        Me.dtpPayrollDate.TabIndex = 3
        '
        'gridPayroll
        '
        Me.gridPayroll.AllowUserToAddRows = False
        Me.gridPayroll.AllowUserToDeleteRows = False
        Me.gridPayroll.AllowUserToResizeRows = False
        Me.gridPayroll.BackgroundColor = System.Drawing.Color.White
        Me.gridPayroll.ColumnHeadersHeight = 34
        Me.gridPayroll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridPayroll.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridPayroll.DefaultCellStyle = DataGridViewCellStyle2
        Me.gridPayroll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridPayroll.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.gridPayroll.Location = New System.Drawing.Point(0, 0)
        Me.gridPayroll.MultiSelect = False
        Me.gridPayroll.Name = "gridPayroll"
        Me.gridPayroll.RowHeadersWidth = 25
        Me.gridPayroll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridPayroll.Size = New System.Drawing.Size(636, 262)
        Me.gridPayroll.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "IsSelected"
        Me.Column1.HeaderText = ""
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 24
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "AccountNumber"
        Me.Column2.HeaderText = "Account Number"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "LastName"
        Me.Column3.HeaderText = "Last Name"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column4
        '
        Me.Column4.DataPropertyName = "FirstName"
        Me.Column4.HeaderText = "First Name"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column5
        '
        Me.Column5.DataPropertyName = "Amount"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle1.Format = "N2"
        DataGridViewCellStyle1.NullValue = Nothing
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle1
        Me.Column5.HeaderText = "Amount"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lnkSelectPeriod)
        Me.Panel1.Controls.Add(Me.numCompanyCode)
        Me.Panel1.Controls.Add(Me.numBatchNo)
        Me.Panel1.Controls.Add(Me.numPresentingOffice)
        Me.Panel1.Controls.Add(Me.numCeilingAmount)
        Me.Panel1.Controls.Add(Me.numFundingAccountNo)
        Me.Panel1.Controls.Add(Me.dtpPayrollDate)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(636, 126)
        Me.Panel1.TabIndex = 2
        '
        'lnkSelectPeriod
        '
        Me.lnkSelectPeriod.AutoSize = True
        Me.lnkSelectPeriod.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkSelectPeriod.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnkSelectPeriod.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkSelectPeriod.Location = New System.Drawing.Point(531, 27)
        Me.lnkSelectPeriod.Name = "lnkSelectPeriod"
        Me.lnkSelectPeriod.Size = New System.Drawing.Size(83, 13)
        Me.lnkSelectPeriod.TabIndex = 277
        Me.lnkSelectPeriod.TabStop = True
        Me.lnkSelectPeriod.Text = "Change Period"
        '
        'numCompanyCode
        '
        Me.numCompanyCode.Location = New System.Drawing.Point(139, 18)
        Me.numCompanyCode.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        Me.numCompanyCode.Name = "numCompanyCode"
        Me.numCompanyCode.Size = New System.Drawing.Size(166, 22)
        Me.numCompanyCode.TabIndex = 0
        Me.numCompanyCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'numBatchNo
        '
        Me.numBatchNo.Location = New System.Drawing.Point(424, 72)
        Me.numBatchNo.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.numBatchNo.Name = "numBatchNo"
        Me.numBatchNo.Size = New System.Drawing.Size(166, 22)
        Me.numBatchNo.TabIndex = 5
        Me.numBatchNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'numPresentingOffice
        '
        Me.numPresentingOffice.Location = New System.Drawing.Point(424, 44)
        Me.numPresentingOffice.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.numPresentingOffice.Name = "numPresentingOffice"
        Me.numPresentingOffice.Size = New System.Drawing.Size(166, 22)
        Me.numPresentingOffice.TabIndex = 4
        Me.numPresentingOffice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'numCeilingAmount
        '
        Me.numCeilingAmount.Location = New System.Drawing.Point(139, 72)
        Me.numCeilingAmount.Maximum = New Decimal(New Integer() {1410065407, 2, 0, 0})
        Me.numCeilingAmount.Name = "numCeilingAmount"
        Me.numCeilingAmount.ReadOnly = True
        Me.numCeilingAmount.Size = New System.Drawing.Size(166, 22)
        Me.numCeilingAmount.TabIndex = 2
        Me.numCeilingAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numCeilingAmount.ThousandsSeparator = True
        '
        'numFundingAccountNo
        '
        Me.numFundingAccountNo.Location = New System.Drawing.Point(139, 44)
        Me.numFundingAccountNo.Maximum = New Decimal(New Integer() {1410065407, 2, 0, 0})
        Me.numFundingAccountNo.Name = "numFundingAccountNo"
        Me.numFundingAccountNo.Size = New System.Drawing.Size(166, 22)
        Me.numFundingAccountNo.TabIndex = 1
        Me.numFundingAccountNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridPayroll)
        Me.Panel2.Controls.Add(Me.Panel4)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 126)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(636, 286)
        Me.Panel2.TabIndex = 9
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.chkSelectAll)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(0, 262)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(636, 24)
        Me.Panel4.TabIndex = 9
        '
        'chkSelectAll
        '
        Me.chkSelectAll.Dock = System.Windows.Forms.DockStyle.Left
        Me.chkSelectAll.Location = New System.Drawing.Point(0, 0)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(378, 24)
        Me.chkSelectAll.TabIndex = 8
        Me.chkSelectAll.Text = "Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.btnClose)
        Me.Panel3.Controls.Add(Me.btnExport)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 412)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(636, 38)
        Me.Panel3.TabIndex = 10
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(549, 7)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Location = New System.Drawing.Point(468, 7)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(75, 23)
        Me.btnExport.TabIndex = 0
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'DataGridViewCheckBoxColumn1
        '
        Me.DataGridViewCheckBoxColumn1.DataPropertyName = "IsSelected"
        Me.DataGridViewCheckBoxColumn1.HeaderText = "Column1"
        Me.DataGridViewCheckBoxColumn1.Name = "DataGridViewCheckBoxColumn1"
        Me.DataGridViewCheckBoxColumn1.Width = 24
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "AccountNumber"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Column2"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "LastName"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Column3"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "FirstName"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Column4"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "Amount"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Format = "N2"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewTextBoxColumn4.HeaderText = "Amount"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "DataHash"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Column6"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "RandomData"
        Me.DataGridViewTextBoxColumn6.HeaderText = "Column7"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        '
        'BankFileTextFormatForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(636, 450)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.Name = "BankFileTextFormatForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.gridPayroll, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.numCompanyCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numBatchNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numPresentingOffice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numCeilingAmount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numFundingAccountNo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents dtpPayrollDate As DateTimePicker
    Friend WithEvents gridPayroll As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents chkSelectAll As CheckBox
    Friend WithEvents btnClose As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents numFundingAccountNo As NumericUpDown
    Friend WithEvents DataGridViewCheckBoxColumn1 As DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents numCeilingAmount As NumericUpDown
    Friend WithEvents numBatchNo As NumericUpDown
    Friend WithEvents numPresentingOffice As NumericUpDown
    Friend WithEvents numCompanyCode As NumericUpDown
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents lnkSelectPeriod As LinkLabel
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewCheckBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents Panel4 As Panel
End Class
