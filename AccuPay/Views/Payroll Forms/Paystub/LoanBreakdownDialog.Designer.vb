<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoanBreakdownDialog
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.LoanGridView = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.LoanGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'LoanGridView
        '
        Me.LoanGridView.AllowUserToAddRows = False
        Me.LoanGridView.AllowUserToDeleteRows = False
        Me.LoanGridView.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.LoanGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.LoanGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.LoanGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column2, Me.Column6, Me.Column1, Me.Column3, Me.Column4, Me.Column5, Me.Column7})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.LoanGridView.DefaultCellStyle = DataGridViewCellStyle6
        Me.LoanGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LoanGridView.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.LoanGridView.Location = New System.Drawing.Point(0, 0)
        Me.LoanGridView.Name = "LoanGridView"
        Me.LoanGridView.ReadOnly = True
        Me.LoanGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.LoanGridView.Size = New System.Drawing.Size(808, 423)
        Me.LoanGridView.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.CloseButton)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 423)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(808, 51)
        Me.Panel2.TabIndex = 3
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(700, 9)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(96, 32)
        Me.CloseButton.TabIndex = 3
        Me.CloseButton.Text = "&Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "LoanNumber"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Loan Number"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "LoanType"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Loan Type"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "TotalAmount"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "N2"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTextBoxColumn3.HeaderText = "Loan Amount"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "DeductionAmount"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.NullValue = Nothing
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn4.HeaderText = "Deduction Amount"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "Balance"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "N2"
        DataGridViewCellStyle9.NullValue = Nothing
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn5.HeaderText = "Balance"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "DeductionSchedule"
        Me.DataGridViewTextBoxColumn6.HeaderText = "Deduction Schedule"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "PayPeriodsLeft"
        Me.DataGridViewTextBoxColumn7.HeaderText = "Pay Periods Left"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "LoanType"
        Me.Column2.HeaderText = "Loan Type"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column6
        '
        Me.Column6.DataPropertyName = "DeductionSchedule"
        Me.Column6.HeaderText = "Deduction Schedule"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "LoanNumber"
        Me.Column1.HeaderText = "Loan Number"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "TotalAmount"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column3.HeaderText = "Loan Amount"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column4
        '
        Me.Column4.DataPropertyName = "DeductionAmount"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.NullValue = Nothing
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column4.HeaderText = "Deduction Amount"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column5
        '
        Me.Column5.DataPropertyName = "Balance"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "N2"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column5.HeaderText = "Balance"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Column7
        '
        Me.Column7.DataPropertyName = "PayPeriodsLeft"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column7.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column7.HeaderText = "Pay Periods Left"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        '
        'LoanBreakdownDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(237, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(808, 474)
        Me.Controls.Add(Me.LoanGridView)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoanBreakdownDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Loan Breakdown"
        CType(Me.LoanGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LoanGridView As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Panel2 As Panel
    Friend WithEvents CloseButton As Button
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
End Class
