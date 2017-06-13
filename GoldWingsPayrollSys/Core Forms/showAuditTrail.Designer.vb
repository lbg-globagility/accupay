<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class showAuditTrail
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
        Me.dgvaudit = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ViewID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ChangeRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Createds = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CreatedBy = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ViewName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FieldChange = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OldValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NewValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ActionPerformed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Last = New System.Windows.Forms.LinkLabel()
        Me.Nxt = New System.Windows.Forms.LinkLabel()
        Me.Prev = New System.Windows.Forms.LinkLabel()
        Me.First = New System.Windows.Forms.LinkLabel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        CType(Me.dgvaudit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvaudit
        '
        Me.dgvaudit.AllowUserToAddRows = False
        Me.dgvaudit.AllowUserToDeleteRows = False
        Me.dgvaudit.AllowUserToOrderColumns = True
        Me.dgvaudit.BackgroundColor = System.Drawing.Color.White
        Me.dgvaudit.ColumnHeadersHeight = 35
        Me.dgvaudit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvaudit.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.RowID, Me.ViewID, Me.ChangeRowID, Me.Createds, Me.CreatedBy, Me.ViewName, Me.FieldChange, Me.OldValue, Me.NewValue, Me.ActionPerformed})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvaudit.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvaudit.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvaudit.Location = New System.Drawing.Point(12, 12)
        Me.dgvaudit.MultiSelect = False
        Me.dgvaudit.Name = "dgvaudit"
        Me.dgvaudit.ReadOnly = True
        Me.dgvaudit.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvaudit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvaudit.Size = New System.Drawing.Size(1165, 436)
        Me.dgvaudit.TabIndex = 0
        '
        'RowID
        '
        Me.RowID.HeaderText = "RowID"
        Me.RowID.Name = "RowID"
        Me.RowID.ReadOnly = True
        Me.RowID.Visible = False
        Me.RowID.Width = 125
        '
        'ViewID
        '
        Me.ViewID.HeaderText = "ViewID"
        Me.ViewID.Name = "ViewID"
        Me.ViewID.ReadOnly = True
        Me.ViewID.Visible = False
        Me.ViewID.Width = 124
        '
        'ChangeRowID
        '
        Me.ChangeRowID.HeaderText = "ChangeRowID"
        Me.ChangeRowID.Name = "ChangeRowID"
        Me.ChangeRowID.ReadOnly = True
        Me.ChangeRowID.Visible = False
        Me.ChangeRowID.Width = 125
        '
        'Createds
        '
        Me.Createds.HeaderText = "Created"
        Me.Createds.Name = "Createds"
        Me.Createds.ReadOnly = True
        Me.Createds.Width = 160
        '
        'CreatedBy
        '
        Me.CreatedBy.HeaderText = "Created by"
        Me.CreatedBy.Name = "CreatedBy"
        Me.CreatedBy.ReadOnly = True
        Me.CreatedBy.Width = 161
        '
        'ViewName
        '
        Me.ViewName.HeaderText = "View name"
        Me.ViewName.Name = "ViewName"
        Me.ViewName.ReadOnly = True
        Me.ViewName.Width = 160
        '
        'FieldChange
        '
        Me.FieldChange.HeaderText = "Field changed"
        Me.FieldChange.Name = "FieldChange"
        Me.FieldChange.ReadOnly = True
        Me.FieldChange.Width = 160
        '
        'OldValue
        '
        Me.OldValue.HeaderText = "Previous value"
        Me.OldValue.Name = "OldValue"
        Me.OldValue.ReadOnly = True
        Me.OldValue.Width = 160
        '
        'NewValue
        '
        Me.NewValue.HeaderText = "New value"
        Me.NewValue.Name = "NewValue"
        Me.NewValue.ReadOnly = True
        Me.NewValue.Width = 161
        '
        'ActionPerformed
        '
        Me.ActionPerformed.HeaderText = "Action performed"
        Me.ActionPerformed.Name = "ActionPerformed"
        Me.ActionPerformed.ReadOnly = True
        Me.ActionPerformed.Width = 160
        '
        'Last
        '
        Me.Last.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Last.AutoSize = True
        Me.Last.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Last.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Last.Location = New System.Drawing.Point(728, 0)
        Me.Last.Name = "Last"
        Me.Last.Size = New System.Drawing.Size(44, 15)
        Me.Last.TabIndex = 154
        Me.Last.TabStop = True
        Me.Last.Text = "Last>>"
        '
        'Nxt
        '
        Me.Nxt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Nxt.AutoSize = True
        Me.Nxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Nxt.Location = New System.Drawing.Point(683, 0)
        Me.Nxt.Name = "Nxt"
        Me.Nxt.Size = New System.Drawing.Size(39, 15)
        Me.Nxt.TabIndex = 153
        Me.Nxt.TabStop = True
        Me.Nxt.Text = "Next>"
        '
        'Prev
        '
        Me.Prev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Prev.AutoSize = True
        Me.Prev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Prev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Prev.Location = New System.Drawing.Point(469, 0)
        Me.Prev.Name = "Prev"
        Me.Prev.Size = New System.Drawing.Size(38, 15)
        Me.Prev.TabIndex = 152
        Me.Prev.TabStop = True
        Me.Prev.Text = "<Prev"
        '
        'First
        '
        Me.First.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.First.AutoSize = True
        Me.First.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.First.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.First.Location = New System.Drawing.Point(419, 0)
        Me.First.Name = "First"
        Me.First.Size = New System.Drawing.Size(44, 15)
        Me.First.TabIndex = 151
        Me.First.TabStop = True
        Me.First.Text = "<<First"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Last)
        Me.Panel1.Controls.Add(Me.Nxt)
        Me.Panel1.Controls.Add(Me.First)
        Me.Panel1.Controls.Add(Me.Prev)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 454)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1189, 24)
        Me.Panel1.TabIndex = 155
        '
        'showAuditTrail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1189, 478)
        Me.Controls.Add(Me.dgvaudit)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "showAuditTrail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Audit trail"
        CType(Me.dgvaudit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvaudit As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents RowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ViewID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ChangeRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Createds As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CreatedBy As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ViewName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FieldChange As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OldValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActionPerformed As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Last As System.Windows.Forms.LinkLabel
    Friend WithEvents Nxt As System.Windows.Forms.LinkLabel
    Friend WithEvents Prev As System.Windows.Forms.LinkLabel
    Friend WithEvents First As System.Windows.Forms.LinkLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
End Class
