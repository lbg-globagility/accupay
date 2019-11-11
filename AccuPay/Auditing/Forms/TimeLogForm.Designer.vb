Namespace Auditing

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class TimeLogForm
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
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.AuditDataGridView = New System.Windows.Forms.DataGridView()
            Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            CType(Me.AuditDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'AuditDataGridView
            '
            Me.AuditDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.AuditDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
            Me.AuditDataGridView.BackgroundColor = System.Drawing.Color.White
            Me.AuditDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.AuditDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3})
            Me.AuditDataGridView.Location = New System.Drawing.Point(8, 8)
            Me.AuditDataGridView.Name = "AuditDataGridView"
            Me.AuditDataGridView.Size = New System.Drawing.Size(1168, 456)
            Me.AuditDataGridView.TabIndex = 0
            '
            'Column1
            '
            Me.Column1.DataPropertyName = "DateOccurred"
            Me.Column1.HeaderText = "Date"
            Me.Column1.Name = "Column1"
            '
            'Column2
            '
            Me.Column2.DataPropertyName = "Record"
            Me.Column2.HeaderText = "Record"
            Me.Column2.Name = "Column2"
            '
            'Column3
            '
            Me.Column3.DataPropertyName = "Details"
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.Column3.DefaultCellStyle = DataGridViewCellStyle2
            Me.Column3.HeaderText = "Change"
            Me.Column3.Name = "Column3"
            '
            'TimeLogForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1184, 473)
            Me.Controls.Add(Me.AuditDataGridView)
            Me.Name = "TimeLogForm"
            Me.Text = "TimeLogForm"
            CType(Me.AuditDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents AuditDataGridView As DataGridView
        Friend WithEvents Column1 As DataGridViewTextBoxColumn
        Friend WithEvents Column2 As DataGridViewTextBoxColumn
        Friend WithEvents Column3 As DataGridViewTextBoxColumn
    End Class

End Namespace