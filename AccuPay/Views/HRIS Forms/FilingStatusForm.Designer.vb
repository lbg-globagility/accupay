<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FilingStatusForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FilingStatusForm))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.btnSave = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.txtFillingStatus = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvfilingstatus = New System.Windows.Forms.DataGridView()
        Me.c_MaritalStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_FilingStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpfilingdetails = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDependant = New System.Windows.Forms.TextBox()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvfilingstatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpfilingdetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnSave, Me.btnDelete, Me.btnCancel})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(337, 25)
        Me.ToolStrip1.TabIndex = 10
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
        'cmbStatus
        '
        Me.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Items.AddRange(New Object() {"Single", "Merried"})
        Me.cmbStatus.Location = New System.Drawing.Point(107, 23)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(196, 21)
        Me.cmbStatus.TabIndex = 11
        '
        'txtFillingStatus
        '
        Me.txtFillingStatus.Location = New System.Drawing.Point(107, 47)
        Me.txtFillingStatus.Name = "txtFillingStatus"
        Me.txtFillingStatus.Size = New System.Drawing.Size(196, 20)
        Me.txtFillingStatus.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Marital Status:"
        '
        'dgvfilingstatus
        '
        Me.dgvfilingstatus.AllowUserToAddRows = False
        Me.dgvfilingstatus.AllowUserToDeleteRows = False
        Me.dgvfilingstatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvfilingstatus.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_MaritalStatus, Me.c_FilingStatus, Me.c_RowID})
        Me.dgvfilingstatus.Location = New System.Drawing.Point(12, 154)
        Me.dgvfilingstatus.Name = "dgvfilingstatus"
        Me.dgvfilingstatus.Size = New System.Drawing.Size(313, 233)
        Me.dgvfilingstatus.TabIndex = 14
        '
        'c_MaritalStatus
        '
        Me.c_MaritalStatus.HeaderText = "Marital Status"
        Me.c_MaritalStatus.Name = "c_MaritalStatus"
        '
        'c_FilingStatus
        '
        Me.c_FilingStatus.HeaderText = "Filing Status"
        Me.c_FilingStatus.Name = "c_FilingStatus"
        '
        'c_RowID
        '
        Me.c_RowID.HeaderText = "RowID"
        Me.c_RowID.Name = "c_RowID"
        Me.c_RowID.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Filing Status:"
        '
        'grpfilingdetails
        '
        Me.grpfilingdetails.Controls.Add(Me.Label3)
        Me.grpfilingdetails.Controls.Add(Me.txtDependant)
        Me.grpfilingdetails.Controls.Add(Me.cmbStatus)
        Me.grpfilingdetails.Controls.Add(Me.Label2)
        Me.grpfilingdetails.Controls.Add(Me.txtFillingStatus)
        Me.grpfilingdetails.Controls.Add(Me.Label1)
        Me.grpfilingdetails.Enabled = False
        Me.grpfilingdetails.Location = New System.Drawing.Point(12, 37)
        Me.grpfilingdetails.Name = "grpfilingdetails"
        Me.grpfilingdetails.Size = New System.Drawing.Size(313, 111)
        Me.grpfilingdetails.TabIndex = 16
        Me.grpfilingdetails.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 73)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "No. of Dependant:"
        '
        'txtDependant
        '
        Me.txtDependant.Location = New System.Drawing.Point(107, 73)
        Me.txtDependant.Name = "txtDependant"
        Me.txtDependant.Size = New System.Drawing.Size(196, 20)
        Me.txtDependant.TabIndex = 16
        '
        'FilingStatusForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(337, 399)
        Me.Controls.Add(Me.grpfilingdetails)
        Me.Controls.Add(Me.dgvfilingstatus)
        Me.Controls.Add(Me.ToolStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FilingStatusForm"
        Me.Text = "FilingStatusForm"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvfilingstatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpfilingdetails.ResumeLayout(False)
        Me.grpfilingdetails.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmbStatus As System.Windows.Forms.ComboBox
    Friend WithEvents txtFillingStatus As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgvfilingstatus As System.Windows.Forms.DataGridView
    Friend WithEvents c_MaritalStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_FilingStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_RowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpfilingdetails As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDependant As System.Windows.Forms.TextBox
End Class
