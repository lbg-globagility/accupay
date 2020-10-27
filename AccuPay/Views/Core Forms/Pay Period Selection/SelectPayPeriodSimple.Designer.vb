<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SelectPayPeriodSimple
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.gridPeriods = New System.Windows.Forms.DataGridView()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.labelCurrentYear = New System.Windows.Forms.Label()
        Me.linkNextYear = New System.Windows.Forms.LinkLabel()
        Me.linkPreviousYear = New System.Windows.Forms.LinkLabel()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        CType(Me.gridPeriods, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Location = New System.Drawing.Point(256, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 406)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(424, 35)
        Me.Panel1.TabIndex = 1
        '
        'Button2
        '
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(337, 6)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'gridPeriods
        '
        Me.gridPeriods.AllowUserToAddRows = False
        Me.gridPeriods.AllowUserToDeleteRows = False
        Me.gridPeriods.AllowUserToResizeColumns = False
        Me.gridPeriods.AllowUserToResizeRows = False
        Me.gridPeriods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridPeriods.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column2, Me.Column3})
        Me.gridPeriods.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridPeriods.Location = New System.Drawing.Point(0, 0)
        Me.gridPeriods.MultiSelect = False
        Me.gridPeriods.Name = "gridPeriods"
        Me.gridPeriods.ReadOnly = True
        Me.gridPeriods.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridPeriods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridPeriods.Size = New System.Drawing.Size(424, 377)
        Me.gridPeriods.TabIndex = 0
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "Month"
        Me.Column2.HeaderText = "Month"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column2.Width = 181
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "Period"
        Me.Column3.HeaderText = "Period"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column3.Width = 181
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.labelCurrentYear)
        Me.Panel2.Controls.Add(Me.linkNextYear)
        Me.Panel2.Controls.Add(Me.linkPreviousYear)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 377)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(424, 29)
        Me.Panel2.TabIndex = 2
        '
        'labelCurrentYear
        '
        Me.labelCurrentYear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.labelCurrentYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelCurrentYear.Location = New System.Drawing.Point(38, 0)
        Me.labelCurrentYear.Name = "labelCurrentYear"
        Me.labelCurrentYear.Size = New System.Drawing.Size(347, 29)
        Me.labelCurrentYear.TabIndex = 4
        Me.labelCurrentYear.Text = "Label1"
        Me.labelCurrentYear.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'linkNextYear
        '
        Me.linkNextYear.AutoSize = True
        Me.linkNextYear.Dock = System.Windows.Forms.DockStyle.Right
        Me.linkNextYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkNextYear.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkNextYear.Location = New System.Drawing.Point(385, 0)
        Me.linkNextYear.Name = "linkNextYear"
        Me.linkNextYear.Size = New System.Drawing.Size(39, 15)
        Me.linkNextYear.TabIndex = 3
        Me.linkNextYear.TabStop = True
        Me.linkNextYear.Text = "Next>"
        '
        'linkPreviousYear
        '
        Me.linkPreviousYear.AutoSize = True
        Me.linkPreviousYear.Dock = System.Windows.Forms.DockStyle.Left
        Me.linkPreviousYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkPreviousYear.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkPreviousYear.Location = New System.Drawing.Point(0, 0)
        Me.linkPreviousYear.Name = "linkPreviousYear"
        Me.linkPreviousYear.Size = New System.Drawing.Size(38, 15)
        Me.linkPreviousYear.TabIndex = 2
        Me.linkPreviousYear.TabStop = True
        Me.linkPreviousYear.Text = "<Prev"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "RowID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Column1"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "Month"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Month"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn2.Width = 183
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "Period"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Period"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn3.Width = 183
        '
        'SelectPayPeriodSimple
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(424, 441)
        Me.Controls.Add(Me.gridPeriods)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectPayPeriodSimple"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        CType(Me.gridPeriods, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Button2 As Button
    Friend WithEvents gridPeriods As DataGridView
    Friend WithEvents Panel2 As Panel
    Friend WithEvents linkNextYear As LinkLabel
    Friend WithEvents linkPreviousYear As LinkLabel
    Friend WithEvents labelCurrentYear As Label
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
End Class
