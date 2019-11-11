<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PayrollGenerateForm
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
        Me.cmbPayType = New System.Windows.Forms.ComboBox()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvEmpList = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_basicpay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_absent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_late = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_undertime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_dayoffot = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_nightdiff = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvEmpList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbPayType
        '
        Me.cmbPayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPayType.FormattingEnabled = True
        Me.cmbPayType.Location = New System.Drawing.Point(12, 38)
        Me.cmbPayType.Name = "cmbPayType"
        Me.cmbPayType.Size = New System.Drawing.Size(230, 21)
        Me.cmbPayType.TabIndex = 0
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(12, 65)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerate.TabIndex = 1
        Me.btnGenerate.Text = "&Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Pay Period"
        '
        'dgvEmpList
        '
        Me.dgvEmpList.AllowUserToAddRows = False
        Me.dgvEmpList.AllowUserToDeleteRows = False
        Me.dgvEmpList.BackgroundColor = System.Drawing.SystemColors.ControlLight
        Me.dgvEmpList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEmpList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_basicpay, Me.c_absent, Me.c_late, Me.c_undertime, Me.c_dayoffot, Me.c_nightdiff})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEmpList.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvEmpList.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvEmpList.Location = New System.Drawing.Point(11, 94)
        Me.dgvEmpList.Name = "dgvEmpList"
        Me.dgvEmpList.ReadOnly = True
        Me.dgvEmpList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmpList.Size = New System.Drawing.Size(693, 377)
        Me.dgvEmpList.TabIndex = 322
        '
        'c_basicpay
        '
        Me.c_basicpay.HeaderText = "Basic Pay"
        Me.c_basicpay.Name = "c_basicpay"
        Me.c_basicpay.ReadOnly = True
        '
        'c_absent
        '
        Me.c_absent.HeaderText = "Absent"
        Me.c_absent.Name = "c_absent"
        Me.c_absent.ReadOnly = True
        '
        'c_late
        '
        Me.c_late.HeaderText = "Late"
        Me.c_late.Name = "c_late"
        Me.c_late.ReadOnly = True
        '
        'c_undertime
        '
        Me.c_undertime.HeaderText = "Under time"
        Me.c_undertime.Name = "c_undertime"
        Me.c_undertime.ReadOnly = True
        '
        'c_dayoffot
        '
        Me.c_dayoffot.HeaderText = "Day Off OT"
        Me.c_dayoffot.Name = "c_dayoffot"
        Me.c_dayoffot.ReadOnly = True
        '
        'c_nightdiff
        '
        Me.c_nightdiff.HeaderText = "Night Diff"
        Me.c_nightdiff.Name = "c_nightdiff"
        Me.c_nightdiff.ReadOnly = True
        '
        'PayrollGenerateForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(719, 483)
        Me.Controls.Add(Me.dgvEmpList)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.cmbPayType)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "PayrollGenerateForm"
        Me.Text = "Generate Payroll"
        CType(Me.dgvEmpList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbPayType As System.Windows.Forms.ComboBox
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgvEmpList As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents c_basicpay As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_absent As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_late As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_undertime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_dayoffot As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_nightdiff As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
