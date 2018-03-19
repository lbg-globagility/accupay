<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class viewtotloan
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvLoanList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_loanno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_totloanamt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_totballeft = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_dedamt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_DedPercent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_dedsched = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_noofpayperiod = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn112 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn113 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_loantype = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvLoanList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvLoanList
        '
        Me.dgvLoanList.AllowUserToAddRows = False
        Me.dgvLoanList.AllowUserToDeleteRows = False
        Me.dgvLoanList.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvLoanList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvLoanList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLoanList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_loanno, Me.c_totloanamt, Me.c_totballeft, Me.c_dedamt, Me.c_DedPercent, Me.c_dedsched, Me.c_noofpayperiod, Me.DataGridViewTextBoxColumn112, Me.DataGridViewTextBoxColumn113, Me.c_status, Me.c_loantype})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvLoanList.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvLoanList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvLoanList.Location = New System.Drawing.Point(12, 12)
        Me.dgvLoanList.Name = "dgvLoanList"
        Me.dgvLoanList.ReadOnly = True
        Me.dgvLoanList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLoanList.Size = New System.Drawing.Size(784, 450)
        Me.dgvLoanList.TabIndex = 0
        '
        'c_loanno
        '
        Me.c_loanno.HeaderText = "Loan Number"
        Me.c_loanno.Name = "c_loanno"
        Me.c_loanno.ReadOnly = True
        '
        'c_totloanamt
        '
        Me.c_totloanamt.HeaderText = "Total Loan Amount"
        Me.c_totloanamt.Name = "c_totloanamt"
        Me.c_totloanamt.ReadOnly = True
        '
        'c_totballeft
        '
        Me.c_totballeft.HeaderText = "Total Balance Left"
        Me.c_totballeft.Name = "c_totballeft"
        Me.c_totballeft.ReadOnly = True
        '
        'c_dedamt
        '
        Me.c_dedamt.HeaderText = "Deduction Amount"
        Me.c_dedamt.Name = "c_dedamt"
        Me.c_dedamt.ReadOnly = True
        '
        'c_DedPercent
        '
        Me.c_DedPercent.HeaderText = "Deduction Percentage"
        Me.c_DedPercent.Name = "c_DedPercent"
        Me.c_DedPercent.ReadOnly = True
        '
        'c_dedsched
        '
        Me.c_dedsched.HeaderText = "Deduction Schedule"
        Me.c_dedsched.Name = "c_dedsched"
        Me.c_dedsched.ReadOnly = True
        '
        'c_noofpayperiod
        '
        Me.c_noofpayperiod.HeaderText = "No of pay period"
        Me.c_noofpayperiod.Name = "c_noofpayperiod"
        Me.c_noofpayperiod.ReadOnly = True
        '
        'DataGridViewTextBoxColumn112
        '
        Me.DataGridViewTextBoxColumn112.HeaderText = "Remarks"
        Me.DataGridViewTextBoxColumn112.Name = "DataGridViewTextBoxColumn112"
        Me.DataGridViewTextBoxColumn112.ReadOnly = True
        '
        'DataGridViewTextBoxColumn113
        '
        Me.DataGridViewTextBoxColumn113.HeaderText = "RowiD"
        Me.DataGridViewTextBoxColumn113.Name = "DataGridViewTextBoxColumn113"
        Me.DataGridViewTextBoxColumn113.ReadOnly = True
        Me.DataGridViewTextBoxColumn113.Visible = False
        '
        'c_status
        '
        Me.c_status.HeaderText = "Status"
        Me.c_status.Name = "c_status"
        Me.c_status.ReadOnly = True
        '
        'c_loantype
        '
        Me.c_loantype.HeaderText = "Loan type"
        Me.c_loantype.Name = "c_loantype"
        Me.c_loantype.ReadOnly = True
        '
        'viewtotloan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(237, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(808, 474)
        Me.Controls.Add(Me.dgvLoanList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "viewtotloan"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Loan"
        CType(Me.dgvLoanList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvLoanList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_loanno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_totloanamt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_totballeft As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_dedamt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_DedPercent As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_dedsched As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_noofpayperiod As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn112 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn113 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_status As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_loantype As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
