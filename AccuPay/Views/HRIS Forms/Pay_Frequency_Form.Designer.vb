<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Pay_Frequency_Form
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Pay_Frequency_Form))
        Me.dgvPayrollList = New System.Windows.Forms.DataGridView()
        Me.c_PayFrequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_StartDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbPayType = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpPayroll = New System.Windows.Forms.GroupBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        CType(Me.dgvPayrollList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpPayroll.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvPayrollList
        '
        Me.dgvPayrollList.AllowUserToAddRows = False
        Me.dgvPayrollList.AllowUserToDeleteRows = False
        Me.dgvPayrollList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvPayrollList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPayrollList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_PayFrequency, Me.c_StartDate, Me.c_RowID})
        Me.dgvPayrollList.Location = New System.Drawing.Point(12, 135)
        Me.dgvPayrollList.Name = "dgvPayrollList"
        Me.dgvPayrollList.ReadOnly = True
        Me.dgvPayrollList.Size = New System.Drawing.Size(321, 330)
        Me.dgvPayrollList.TabIndex = 0
        '
        'c_PayFrequency
        '
        Me.c_PayFrequency.HeaderText = "Payroll Type"
        Me.c_PayFrequency.Name = "c_PayFrequency"
        Me.c_PayFrequency.ReadOnly = True
        Me.c_PayFrequency.Width = 120
        '
        'c_StartDate
        '
        Me.c_StartDate.HeaderText = "Start Date"
        Me.c_StartDate.Name = "c_StartDate"
        Me.c_StartDate.ReadOnly = True
        Me.c_StartDate.Width = 120
        '
        'c_RowID
        '
        Me.c_RowID.HeaderText = "RowID"
        Me.c_RowID.Name = "c_RowID"
        Me.c_RowID.ReadOnly = True
        Me.c_RowID.Visible = False
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(86, 26)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(178, 20)
        Me.dtpStartDate.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Start Date:"
        '
        'cmbPayType
        '
        Me.cmbPayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPayType.FormattingEnabled = True
        Me.cmbPayType.Items.AddRange(New Object() {"DAILY", "WEEKLY", "SEMI-MONTHLY", "MONTHLY"})
        Me.cmbPayType.Location = New System.Drawing.Point(86, 52)
        Me.cmbPayType.Name = "cmbPayType"
        Me.cmbPayType.Size = New System.Drawing.Size(178, 21)
        Me.cmbPayType.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Payroll Type:"
        '
        'grpPayroll
        '
        Me.grpPayroll.Controls.Add(Me.dtpStartDate)
        Me.grpPayroll.Controls.Add(Me.Label2)
        Me.grpPayroll.Controls.Add(Me.Label1)
        Me.grpPayroll.Controls.Add(Me.cmbPayType)
        Me.grpPayroll.Enabled = False
        Me.grpPayroll.Location = New System.Drawing.Point(12, 39)
        Me.grpPayroll.Name = "grpPayroll"
        Me.grpPayroll.Size = New System.Drawing.Size(321, 90)
        Me.grpPayroll.TabIndex = 8
        Me.grpPayroll.TabStop = False
        Me.grpPayroll.Text = "Payroll Type Details"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.btnDelete, Me.btnCancel})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(350, 25)
        Me.ToolStrip1.TabIndex = 9
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnNew
        '
        Me.btnNew.Image = CType(resources.GetObject("btnNew.Image"), System.Drawing.Image)
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(51, 22)
        Me.btnNew.Text = "&New"
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(51, 22)
        Me.btnSave.Text = "&Save"
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(60, 22)
        Me.btnDelete.Text = "&Delete"
        '
        'btnCancel
        '
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "&Cancel"
        '
        'Pay_Frequency_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(350, 477)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.grpPayroll)
        Me.Controls.Add(Me.dgvPayrollList)
        Me.Name = "Pay_Frequency_Form"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Payroll Period"
        CType(Me.dgvPayrollList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpPayroll.ResumeLayout(False)
        Me.grpPayroll.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvPayrollList As System.Windows.Forms.DataGridView
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbPayType As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpPayroll As System.Windows.Forms.GroupBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents c_PayFrequency As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_StartDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_RowID As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
