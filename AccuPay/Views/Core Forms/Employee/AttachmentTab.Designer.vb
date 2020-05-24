<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AttachmentTab
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AttachmentTab))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.txtEmployeeID = New System.Windows.Forms.TextBox()
        Me.txtFullname = New System.Windows.Forms.TextBox()
        Me.pbEmployee = New System.Windows.Forms.PictureBox()
        Me.ToolStrip21 = New System.Windows.Forms.ToolStrip()
        Me.btnNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton16 = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.btnUserActivity = New System.Windows.Forms.ToolStripButton()
        Me.dgvAttachments = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.c_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_FileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_FileExtension = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_ViewButton = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.pbAttachment = New System.Windows.Forms.PictureBox()
        Me.btnattadl = New System.Windows.Forms.Button()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip21.SuspendLayout()
        CType(Me.dgvAttachments, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbAttachment, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtEmployeeID
        '
        Me.txtEmployeeID.BackColor = System.Drawing.Color.White
        Me.txtEmployeeID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeID.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmployeeID.Location = New System.Drawing.Point(114, 74)
        Me.txtEmployeeID.MaxLength = 50
        Me.txtEmployeeID.Name = "txtEmployeeID"
        Me.txtEmployeeID.ReadOnly = True
        Me.txtEmployeeID.Size = New System.Drawing.Size(716, 22)
        Me.txtEmployeeID.TabIndex = 351
        '
        'txtFullname
        '
        Me.txtFullname.BackColor = System.Drawing.Color.White
        Me.txtFullname.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFullname.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtFullname.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtFullname.Location = New System.Drawing.Point(114, 34)
        Me.txtFullname.MaxLength = 250
        Me.txtFullname.Name = "txtFullname"
        Me.txtFullname.ReadOnly = True
        Me.txtFullname.Size = New System.Drawing.Size(668, 28)
        Me.txtFullname.TabIndex = 350
        '
        'pbEmployee
        '
        Me.pbEmployee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployee.Location = New System.Drawing.Point(18, 34)
        Me.pbEmployee.Name = "pbEmployee"
        Me.pbEmployee.Size = New System.Drawing.Size(88, 88)
        Me.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployee.TabIndex = 349
        Me.pbEmployee.TabStop = False
        '
        'ToolStrip21
        '
        Me.ToolStrip21.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip21.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip21.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.ToolStripButton16, Me.btnDelete, Me.btnCancel, Me.btnUserActivity})
        Me.ToolStrip21.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip21.Name = "ToolStrip21"
        Me.ToolStrip21.Size = New System.Drawing.Size(856, 25)
        Me.ToolStrip21.TabIndex = 352
        Me.ToolStrip21.Text = "ToolStrip21"
        '
        'btnNew
        '
        Me.btnNew.Image = Global.AccuPay.My.Resources.Resources._new
        Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(117, 22)
        Me.btnNew.Text = "&New Attachment"
        '
        'ToolStripButton16
        '
        Me.ToolStripButton16.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton16.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.ToolStripButton16.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton16.Name = "ToolStripButton16"
        Me.ToolStripButton16.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripButton16.Text = "Close"
        '
        'btnDelete
        '
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(122, 22)
        Me.btnDelete.Text = "&Delete Atachment"
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 22)
        Me.btnCancel.Text = "Cancel"
        '
        'btnUserActivity
        '
        Me.btnUserActivity.Image = CType(resources.GetObject("btnUserActivity.Image"), System.Drawing.Image)
        Me.btnUserActivity.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUserActivity.Name = "btnUserActivity"
        Me.btnUserActivity.Size = New System.Drawing.Size(93, 22)
        Me.btnUserActivity.Text = "User Activity"
        '
        'dgvAttachments
        '
        Me.dgvAttachments.AllowUserToAddRows = False
        Me.dgvAttachments.AllowUserToDeleteRows = False
        Me.dgvAttachments.AllowUserToOrderColumns = True
        Me.dgvAttachments.AllowUserToResizeRows = False
        Me.dgvAttachments.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvAttachments.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvAttachments.ColumnHeadersHeight = 34
        Me.dgvAttachments.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_Type, Me.c_FileName, Me.c_FileExtension, Me.c_ViewButton})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvAttachments.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvAttachments.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvAttachments.Location = New System.Drawing.Point(18, 163)
        Me.dgvAttachments.MultiSelect = False
        Me.dgvAttachments.Name = "dgvAttachments"
        Me.dgvAttachments.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvAttachments.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvAttachments.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvAttachments.Size = New System.Drawing.Size(637, 386)
        Me.dgvAttachments.TabIndex = 353
        '
        'c_Type
        '
        Me.c_Type.DataPropertyName = "Type"
        Me.c_Type.HeaderText = "Type"
        Me.c_Type.Name = "c_Type"
        Me.c_Type.ReadOnly = True
        Me.c_Type.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.c_Type.Width = 200
        '
        'c_FileName
        '
        Me.c_FileName.DataPropertyName = "FileName"
        Me.c_FileName.HeaderText = "File name"
        Me.c_FileName.MaxInputLength = 200
        Me.c_FileName.Name = "c_FileName"
        Me.c_FileName.ReadOnly = True
        Me.c_FileName.Width = 160
        '
        'c_FileExtension
        '
        Me.c_FileExtension.DataPropertyName = "FileType"
        Me.c_FileExtension.HeaderText = "File extension"
        Me.c_FileExtension.Name = "c_FileExtension"
        Me.c_FileExtension.ReadOnly = True
        Me.c_FileExtension.Width = 115
        '
        'c_ViewButton
        '
        Me.c_ViewButton.HeaderText = ""
        Me.c_ViewButton.Name = "c_ViewButton"
        Me.c_ViewButton.ReadOnly = True
        Me.c_ViewButton.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.c_ViewButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.c_ViewButton.Text = "VIEW"
        Me.c_ViewButton.UseColumnTextForButtonValue = True
        Me.c_ViewButton.Width = 120
        '
        'pbAttachment
        '
        Me.pbAttachment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbAttachment.Location = New System.Drawing.Point(661, 163)
        Me.pbAttachment.Name = "pbAttachment"
        Me.pbAttachment.Size = New System.Drawing.Size(192, 191)
        Me.pbAttachment.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbAttachment.TabIndex = 357
        Me.pbAttachment.TabStop = False
        '
        'btnattadl
        '
        Me.btnattadl.Location = New System.Drawing.Point(661, 360)
        Me.btnattadl.Name = "btnattadl"
        Me.btnattadl.Size = New System.Drawing.Size(75, 21)
        Me.btnattadl.TabIndex = 356
        Me.btnattadl.Text = "Download"
        Me.btnattadl.UseVisualStyleBackColor = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "Type"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Type"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn1.Width = 200
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "FileName"
        Me.DataGridViewTextBoxColumn2.HeaderText = "File name"
        Me.DataGridViewTextBoxColumn2.MaxInputLength = 200
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 160
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "FileType"
        Me.DataGridViewTextBoxColumn3.HeaderText = "File extension"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 160
        '
        'AttachmentTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.dgvAttachments)
        Me.Controls.Add(Me.pbAttachment)
        Me.Controls.Add(Me.btnattadl)
        Me.Controls.Add(Me.ToolStrip21)
        Me.Controls.Add(Me.txtEmployeeID)
        Me.Controls.Add(Me.txtFullname)
        Me.Controls.Add(Me.pbEmployee)
        Me.Name = "AttachmentTab"
        Me.Size = New System.Drawing.Size(856, 552)
        CType(Me.pbEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip21.ResumeLayout(False)
        Me.ToolStrip21.PerformLayout()
        CType(Me.dgvAttachments, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbAttachment, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEmployeeID As TextBox
    Public WithEvents txtFullname As TextBox
    Friend WithEvents pbEmployee As PictureBox
    Friend WithEvents ToolStrip21 As ToolStrip
    Friend WithEvents btnNew As ToolStripButton
    Friend WithEvents btnCancel As ToolStripButton
    Friend WithEvents ToolStripButton16 As ToolStripButton
    Friend WithEvents dgvAttachments As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents pbAttachment As PictureBox
    Friend WithEvents btnattadl As Button
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents btnUserActivity As ToolStripButton
    Friend WithEvents c_Type As DataGridViewTextBoxColumn
    Friend WithEvents c_FileName As DataGridViewTextBoxColumn
    Friend WithEvents c_FileExtension As DataGridViewTextBoxColumn
    Friend WithEvents c_ViewButton As DataGridViewButtonColumn
End Class
