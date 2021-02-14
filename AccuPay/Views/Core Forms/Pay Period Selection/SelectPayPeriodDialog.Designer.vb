<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SelectPayPeriodDialog
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PayPeriodGridView = New System.Windows.Forms.DataGridView()
        Me.linkNxt = New System.Windows.Forms.LinkLabel()
        Me.linkPrev = New System.Windows.Forms.LinkLabel()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblpapyperiodval = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.PayPeriodStatusLabel = New System.Windows.Forms.Label()
        Me.Column15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.PayPeriodGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'PayPeriodGridView
        '
        Me.PayPeriodGridView.AllowUserToAddRows = False
        Me.PayPeriodGridView.AllowUserToDeleteRows = False
        Me.PayPeriodGridView.AllowUserToOrderColumns = True
        Me.PayPeriodGridView.AllowUserToResizeColumns = False
        Me.PayPeriodGridView.AllowUserToResizeRows = False
        Me.PayPeriodGridView.BackgroundColor = System.Drawing.Color.White
        Me.PayPeriodGridView.ColumnHeadersHeight = 38
        Me.PayPeriodGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.PayPeriodGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column15, Me.Column16})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.PayPeriodGridView.DefaultCellStyle = DataGridViewCellStyle1
        Me.PayPeriodGridView.Location = New System.Drawing.Point(122, 43)
        Me.PayPeriodGridView.MultiSelect = False
        Me.PayPeriodGridView.Name = "PayPeriodGridView"
        Me.PayPeriodGridView.ReadOnly = True
        Me.PayPeriodGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.PayPeriodGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.PayPeriodGridView.Size = New System.Drawing.Size(416, 373)
        Me.PayPeriodGridView.TabIndex = 0
        '
        'linkNxt
        '
        Me.linkNxt.AutoSize = True
        Me.linkNxt.Dock = System.Windows.Forms.DockStyle.Right
        Me.linkNxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkNxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkNxt.Location = New System.Drawing.Point(377, 0)
        Me.linkNxt.Name = "linkNxt"
        Me.linkNxt.Size = New System.Drawing.Size(39, 15)
        Me.linkNxt.TabIndex = 2
        Me.linkNxt.TabStop = True
        Me.linkNxt.Text = "Next>"
        '
        'linkPrev
        '
        Me.linkPrev.AutoSize = True
        Me.linkPrev.Dock = System.Windows.Forms.DockStyle.Left
        Me.linkPrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkPrev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkPrev.Location = New System.Drawing.Point(0, 0)
        Me.linkPrev.Name = "linkPrev"
        Me.linkPrev.Size = New System.Drawing.Size(38, 15)
        Me.linkPrev.TabIndex = 1
        Me.linkPrev.TabStop = True
        Me.linkPrev.Text = "<Prev"
        '
        'OkButton
        '
        Me.OkButton.Location = New System.Drawing.Point(383, 461)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 35)
        Me.OkButton.TabIndex = 3
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(464, 461)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 35)
        Me.CloseButton.TabIndex = 4
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(119, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 15)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Pay period"
        '
        'lblpapyperiodval
        '
        Me.lblpapyperiodval.AutoSize = True
        Me.lblpapyperiodval.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblpapyperiodval.Location = New System.Drawing.Point(178, 25)
        Me.lblpapyperiodval.Name = "lblpapyperiodval"
        Me.lblpapyperiodval.Size = New System.Drawing.Size(32, 15)
        Me.lblpapyperiodval.TabIndex = 14
        Me.lblpapyperiodval.Text = "-----"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.linkNxt)
        Me.Panel2.Controls.Add(Me.linkPrev)
        Me.Panel2.Location = New System.Drawing.Point(122, 416)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(416, 29)
        Me.Panel2.TabIndex = 283
        '
        'PayPeriodStatusLabel
        '
        Me.PayPeriodStatusLabel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.PayPeriodStatusLabel.Location = New System.Drawing.Point(452, 25)
        Me.PayPeriodStatusLabel.Name = "PayPeriodStatusLabel"
        Me.PayPeriodStatusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PayPeriodStatusLabel.Size = New System.Drawing.Size(86, 15)
        Me.PayPeriodStatusLabel.TabIndex = 284
        Me.PayPeriodStatusLabel.Text = "-----"
        Me.PayPeriodStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Column15
        '
        Me.Column15.DataPropertyName = "PayFromDate"
        Me.Column15.HeaderText = "Pay period from"
        Me.Column15.Name = "Column15"
        Me.Column15.ReadOnly = True
        Me.Column15.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column15.Width = 178
        '
        'Column16
        '
        Me.Column16.DataPropertyName = "PayToDate"
        Me.Column16.HeaderText = "Pay period to"
        Me.Column16.Name = "Column16"
        Me.Column16.ReadOnly = True
        Me.Column16.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column16.Width = 178
        '
        'SelectPayPeriodDialog
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(548, 508)
        Me.Controls.Add(Me.PayPeriodStatusLabel)
        Me.Controls.Add(Me.lblpapyperiodval)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.PayPeriodGridView)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectPayPeriodDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "List of pay period(s)"
        CType(Me.PayPeriodGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PayPeriodGridView As System.Windows.Forms.DataGridView
    Friend WithEvents linkNxt As System.Windows.Forms.LinkLabel
    Friend WithEvents linkPrev As System.Windows.Forms.LinkLabel
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblpapyperiodval As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents PayPeriodStatusLabel As Label
    Friend WithEvents Column15 As DataGridViewTextBoxColumn
    Friend WithEvents Column16 As DataGridViewTextBoxColumn
End Class
