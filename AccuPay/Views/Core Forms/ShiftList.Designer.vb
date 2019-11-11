<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShiftList
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvcalendar = New System.Windows.Forms.DataGridView()
        Me.shRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.shTimeFrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.shTimeTo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.dgvcalendar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvcalendar
        '
        Me.dgvcalendar.AllowUserToAddRows = False
        Me.dgvcalendar.AllowUserToDeleteRows = False
        Me.dgvcalendar.AllowUserToResizeColumns = False
        Me.dgvcalendar.AllowUserToResizeRows = False
        Me.dgvcalendar.BackgroundColor = System.Drawing.Color.White
        Me.dgvcalendar.ColumnHeadersHeight = 25
        Me.dgvcalendar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvcalendar.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.shRowID, Me.shTimeFrom, Me.shTimeTo})
        Me.dgvcalendar.Dock = System.Windows.Forms.DockStyle.Top
        Me.dgvcalendar.Location = New System.Drawing.Point(0, 0)
        Me.dgvcalendar.MultiSelect = False
        Me.dgvcalendar.Name = "dgvcalendar"
        Me.dgvcalendar.ReadOnly = True
        Me.dgvcalendar.RowHeadersWidth = 28
        Me.dgvcalendar.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvcalendar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvcalendar.Size = New System.Drawing.Size(183, 171)
        Me.dgvcalendar.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.dgvcalendar, "Choose and double click")
        '
        'shRowID
        '
        DataGridViewCellStyle19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        DataGridViewCellStyle19.ForeColor = System.Drawing.Color.DimGray
        Me.shRowID.DefaultCellStyle = DataGridViewCellStyle19
        Me.shRowID.HeaderText = "RowID"
        Me.shRowID.Name = "shRowID"
        Me.shRowID.ReadOnly = True
        Me.shRowID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.shRowID.Visible = False
        Me.shRowID.Width = 95
        '
        'shTimeFrom
        '
        DataGridViewCellStyle20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        DataGridViewCellStyle20.ForeColor = System.Drawing.Color.DimGray
        Me.shTimeFrom.DefaultCellStyle = DataGridViewCellStyle20
        Me.shTimeFrom.HeaderText = ""
        Me.shTimeFrom.Name = "shTimeFrom"
        Me.shTimeFrom.ReadOnly = True
        Me.shTimeFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.shTimeFrom.Width = 68
        '
        'shTimeTo
        '
        DataGridViewCellStyle21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        DataGridViewCellStyle21.ForeColor = System.Drawing.Color.DimGray
        Me.shTimeTo.DefaultCellStyle = DataGridViewCellStyle21
        Me.shTimeTo.HeaderText = ""
        Me.shTimeTo.Name = "shTimeTo"
        Me.shTimeTo.ReadOnly = True
        Me.shTimeTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.shTimeTo.Width = 68
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(94, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(0, 171)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(183, 20)
        Me.Label5.TabIndex = 523
        Me.Label5.Text = "Press (Esc)escape to close"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ShiftList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(183, 191)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.dgvcalendar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShiftList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "ShiftList"
        CType(Me.dgvcalendar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvcalendar As System.Windows.Forms.DataGridView
    Friend WithEvents shRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents shTimeFrom As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents shTimeTo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
