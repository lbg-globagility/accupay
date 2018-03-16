<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MassOvertimeForm
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.FromDatePicker = New System.Windows.Forms.DateTimePicker()
        Me.ToDatePicker = New System.Windows.Forms.DateTimePicker()
        Me.OvertimeDataGridView = New System.Windows.Forms.DataGridView()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EndTimeTextBox = New Acupay.TimeTextBox()
        Me.StartTimeTextBox = New Acupay.TimeTextBox()
        Me.EmployeeTreeView = New Acupay.AccuPayTreeView()
        Me.ApplyButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.EmployeeTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.OvertimeDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FromDatePicker
        '
        Me.FromDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.FromDatePicker.Location = New System.Drawing.Point(240, 8)
        Me.FromDatePicker.Name = "FromDatePicker"
        Me.FromDatePicker.Size = New System.Drawing.Size(136, 20)
        Me.FromDatePicker.TabIndex = 4
        '
        'ToDatePicker
        '
        Me.ToDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.ToDatePicker.Location = New System.Drawing.Point(384, 8)
        Me.ToDatePicker.Name = "ToDatePicker"
        Me.ToDatePicker.Size = New System.Drawing.Size(136, 20)
        Me.ToDatePicker.TabIndex = 5
        '
        'OvertimeDataGridView
        '
        Me.OvertimeDataGridView.AllowUserToAddRows = False
        Me.OvertimeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.OvertimeDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column5, Me.Column4, Me.Column3, Me.Column6, Me.Column1, Me.Column2})
        Me.OvertimeDataGridView.Location = New System.Drawing.Point(240, 40)
        Me.OvertimeDataGridView.Name = "OvertimeDataGridView"
        Me.OvertimeDataGridView.Size = New System.Drawing.Size(616, 544)
        Me.OvertimeDataGridView.TabIndex = 6
        '
        'Column5
        '
        Me.Column5.DataPropertyName = "EmployeeNo"
        Me.Column5.HeaderText = "Employee No"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Column4
        '
        Me.Column4.DataPropertyName = "Name"
        Me.Column4.HeaderText = "Name"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "Date"
        DataGridViewCellStyle5.Format = "MM/dd/yyyy"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column3.HeaderText = "Date"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column6
        '
        Me.Column6.DataPropertyName = "Date"
        DataGridViewCellStyle6.Format = "ddd"
        Me.Column6.DefaultCellStyle = DataGridViewCellStyle6
        Me.Column6.HeaderText = "Day"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "OTStart"
        Me.Column1.HeaderText = "OT Start"
        Me.Column1.Name = "Column1"
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "OTEnd"
        Me.Column2.HeaderText = "OT End"
        Me.Column2.Name = "Column2"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "EmployeeNo"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Employee No"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "Name"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "Date"
        DataGridViewCellStyle7.Format = "mm/dd/yyyy"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTextBoxColumn3.HeaderText = "Date"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "OTStart"
        DataGridViewCellStyle8.Format = "ddd"
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn4.HeaderText = "OT Start"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "OTEnd"
        Me.DataGridViewTextBoxColumn5.HeaderText = "OT End"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "OTEnd"
        Me.DataGridViewTextBoxColumn6.HeaderText = "OT End"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        '
        'EndTimeTextBox
        '
        Me.EndTimeTextBox.Location = New System.Drawing.Point(632, 8)
        Me.EndTimeTextBox.Name = "EndTimeTextBox"
        Me.EndTimeTextBox.Size = New System.Drawing.Size(100, 20)
        Me.EndTimeTextBox.TabIndex = 7
        '
        'StartTimeTextBox
        '
        Me.StartTimeTextBox.Location = New System.Drawing.Point(528, 8)
        Me.StartTimeTextBox.Name = "StartTimeTextBox"
        Me.StartTimeTextBox.Size = New System.Drawing.Size(100, 20)
        Me.StartTimeTextBox.TabIndex = 7
        '
        'EmployeeTreeView
        '
        Me.EmployeeTreeView.CheckBoxes = True
        Me.EmployeeTreeView.Location = New System.Drawing.Point(8, 64)
        Me.EmployeeTreeView.Name = "EmployeeTreeView"
        Me.EmployeeTreeView.Size = New System.Drawing.Size(224, 520)
        Me.EmployeeTreeView.TabIndex = 1
        '
        'ApplyButton
        '
        Me.ApplyButton.Location = New System.Drawing.Point(736, 8)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(56, 23)
        Me.ApplyButton.TabIndex = 8
        Me.ApplyButton.Text = "Apply"
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(800, 8)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(56, 23)
        Me.SaveButton.TabIndex = 9
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'EmployeeTextBox
        '
        Me.EmployeeTextBox.Location = New System.Drawing.Point(8, 40)
        Me.EmployeeTextBox.Name = "EmployeeTextBox"
        Me.EmployeeTextBox.Size = New System.Drawing.Size(224, 20)
        Me.EmployeeTextBox.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 24)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Search"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MassOvertimeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(871, 597)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.EmployeeTextBox)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.ApplyButton)
        Me.Controls.Add(Me.EndTimeTextBox)
        Me.Controls.Add(Me.StartTimeTextBox)
        Me.Controls.Add(Me.OvertimeDataGridView)
        Me.Controls.Add(Me.ToDatePicker)
        Me.Controls.Add(Me.FromDatePicker)
        Me.Controls.Add(Me.EmployeeTreeView)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "MassOvertimeForm"
        Me.Text = "MassOvertimeForm"
        CType(Me.OvertimeDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents EmployeeTreeView As AccuPayTreeView
    Friend WithEvents FromDatePicker As DateTimePicker
    Friend WithEvents ToDatePicker As DateTimePicker
    Friend WithEvents OvertimeDataGridView As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents StartTimeTextBox As TimeTextBox
    Friend WithEvents EndTimeTextBox As TimeTextBox
    Friend WithEvents ApplyButton As Button
    Friend WithEvents SaveButton As Button
    Friend WithEvents EmployeeTextBox As TextBox
    Friend WithEvents Label1 As Label
End Class
