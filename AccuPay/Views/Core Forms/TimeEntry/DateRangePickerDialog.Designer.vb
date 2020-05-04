<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DateRangePickerDialog
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.PayperiodsDataGridView = New System.Windows.Forms.DataGridView()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblYear = New System.Windows.Forms.Label()
        Me.btnIncrementYear = New System.Windows.Forms.Button()
        Me.btnDecrementYear = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        CType(Me.PayperiodsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(280, 5)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(360, 5)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'PayperiodsDataGridView
        '
        Me.PayperiodsDataGridView.AllowUserToAddRows = False
        Me.PayperiodsDataGridView.AllowUserToDeleteRows = False
        Me.PayperiodsDataGridView.AllowUserToResizeColumns = False
        Me.PayperiodsDataGridView.AllowUserToResizeRows = False
        Me.PayperiodsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.PayperiodsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.PayperiodsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column3, Me.Column1, Me.Column2})
        Me.PayperiodsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PayperiodsDataGridView.Location = New System.Drawing.Point(0, 0)
        Me.PayperiodsDataGridView.MultiSelect = False
        Me.PayperiodsDataGridView.Name = "PayperiodsDataGridView"
        Me.PayperiodsDataGridView.ReadOnly = True
        Me.PayperiodsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.PayperiodsDataGridView.Size = New System.Drawing.Size(440, 224)
        Me.PayperiodsDataGridView.TabIndex = 4
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "Period"
        Me.Column3.HeaderText = "Period"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "PayFromDate"
        Me.Column1.HeaderText = "Start"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "PayToDate"
        Me.Column2.HeaderText = "End"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "Period"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Start"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 191
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "PayFromDate"
        Me.DataGridViewTextBoxColumn2.HeaderText = "End"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 190
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "PayToDate"
        Me.DataGridViewTextBoxColumn3.HeaderText = "End"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 127
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.PayperiodsDataGridView)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 32)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(440, 224)
        Me.Panel1.TabIndex = 5
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblYear)
        Me.Panel2.Controls.Add(Me.btnIncrementYear)
        Me.Panel2.Controls.Add(Me.btnDecrementYear)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(440, 32)
        Me.Panel2.TabIndex = 6
        '
        'lblYear
        '
        Me.lblYear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblYear.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYear.Location = New System.Drawing.Point(147, 0)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(146, 32)
        Me.lblYear.TabIndex = 281
        Me.lblYear.Text = "Label1"
        Me.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnIncrementYear
        '
        Me.btnIncrementYear.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnIncrementYear.Location = New System.Drawing.Point(293, 0)
        Me.btnIncrementYear.Name = "btnIncrementYear"
        Me.btnIncrementYear.Size = New System.Drawing.Size(147, 32)
        Me.btnIncrementYear.TabIndex = 4
        Me.btnIncrementYear.Tag = "1"
        Me.btnIncrementYear.Text = "2020"
        Me.btnIncrementYear.UseVisualStyleBackColor = True
        '
        'btnDecrementYear
        '
        Me.btnDecrementYear.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnDecrementYear.Location = New System.Drawing.Point(0, 0)
        Me.btnDecrementYear.Name = "btnDecrementYear"
        Me.btnDecrementYear.Size = New System.Drawing.Size(147, 32)
        Me.btnDecrementYear.TabIndex = 5
        Me.btnDecrementYear.Tag = "-1"
        Me.btnDecrementYear.Text = "2018"
        Me.btnDecrementYear.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Button2)
        Me.Panel3.Controls.Add(Me.Button1)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 256)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(440, 33)
        Me.Panel3.TabIndex = 7
        '
        'DateRangePickerDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(440, 289)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel3)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "DateRangePickerDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DateRangePickerDialog"
        CType(Me.PayperiodsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents PayperiodsDataGridView As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents lblYear As Label
    Friend WithEvents btnIncrementYear As Button
    Friend WithEvents btnDecrementYear As Button
End Class
