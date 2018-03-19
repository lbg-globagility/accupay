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
        Dim DataGridViewCellStyle39 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle40 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle37 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle38 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.FromDatePicker = New System.Windows.Forms.DateTimePicker()
        Me.ToDatePicker = New System.Windows.Forms.DateTimePicker()
        Me.OvertimeDataGridView = New System.Windows.Forms.DataGridView()
        Me.ApplyButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.EmployeeTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EndTimeTextBox = New AccuPay.TimeTextBox()
        Me.StartTimeTextBox = New AccuPay.TimeTextBox()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EmployeeTreeView = New AccuPay.AccuPayTreeView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.OvertimeDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FromDatePicker
        '
        Me.FromDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.FromDatePicker.Location = New System.Drawing.Point(240, 32)
        Me.FromDatePicker.Name = "FromDatePicker"
        Me.FromDatePicker.Size = New System.Drawing.Size(128, 20)
        Me.FromDatePicker.TabIndex = 4
        '
        'ToDatePicker
        '
        Me.ToDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.ToDatePicker.Location = New System.Drawing.Point(376, 32)
        Me.ToDatePicker.Name = "ToDatePicker"
        Me.ToDatePicker.Size = New System.Drawing.Size(128, 20)
        Me.ToDatePicker.TabIndex = 5
        '
        'OvertimeDataGridView
        '
        Me.OvertimeDataGridView.AllowUserToAddRows = False
        Me.OvertimeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.OvertimeDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column5, Me.Column4, Me.Column3, Me.Column6, Me.Column1, Me.Column2})
        Me.OvertimeDataGridView.Location = New System.Drawing.Point(240, 56)
        Me.OvertimeDataGridView.Name = "OvertimeDataGridView"
        Me.OvertimeDataGridView.Size = New System.Drawing.Size(624, 528)
        Me.OvertimeDataGridView.TabIndex = 6
        '
        'ApplyButton
        '
        Me.ApplyButton.Location = New System.Drawing.Point(720, 32)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(72, 23)
        Me.ApplyButton.TabIndex = 8
        Me.ApplyButton.Text = "Apply"
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.SaveButton.Location = New System.Drawing.Point(792, 32)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(72, 23)
        Me.SaveButton.TabIndex = 9
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'EmployeeTextBox
        '
        Me.EmployeeTextBox.Location = New System.Drawing.Point(8, 32)
        Me.EmployeeTextBox.Name = "EmployeeTextBox"
        Me.EmployeeTextBox.Size = New System.Drawing.Size(224, 20)
        Me.EmployeeTextBox.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 16)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Search"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        DataGridViewCellStyle39.Format = "mm/dd/yyyy"
        DataGridViewCellStyle39.NullValue = Nothing
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle39
        Me.DataGridViewTextBoxColumn3.HeaderText = "Date"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "OTStart"
        DataGridViewCellStyle40.Format = "ddd"
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle40
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
        Me.EndTimeTextBox.Location = New System.Drawing.Point(616, 32)
        Me.EndTimeTextBox.Name = "EndTimeTextBox"
        Me.EndTimeTextBox.Size = New System.Drawing.Size(96, 20)
        Me.EndTimeTextBox.TabIndex = 7
        '
        'StartTimeTextBox
        '
        Me.StartTimeTextBox.Location = New System.Drawing.Point(512, 32)
        Me.StartTimeTextBox.Name = "StartTimeTextBox"
        Me.StartTimeTextBox.Size = New System.Drawing.Size(96, 20)
        Me.StartTimeTextBox.TabIndex = 7
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
        DataGridViewCellStyle37.Format = "MM/dd/yyyy"
        DataGridViewCellStyle37.NullValue = Nothing
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle37
        Me.Column3.HeaderText = "Date"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column6
        '
        Me.Column6.DataPropertyName = "Date"
        DataGridViewCellStyle38.Format = "ddd"
        Me.Column6.DefaultCellStyle = DataGridViewCellStyle38
        Me.Column6.HeaderText = "Day"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "StartTime"
        Me.Column1.HeaderText = "OT Start"
        Me.Column1.Name = "Column1"
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "EndTime"
        Me.Column2.HeaderText = "OT End"
        Me.Column2.Name = "Column2"
        '
        'EmployeeTreeView
        '
        Me.EmployeeTreeView.CheckBoxes = True
        Me.EmployeeTreeView.Location = New System.Drawing.Point(8, 56)
        Me.EmployeeTreeView.Name = "EmployeeTreeView"
        Me.EmployeeTreeView.Size = New System.Drawing.Size(224, 528)
        Me.EmployeeTreeView.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(240, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 16)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Date From"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(376, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 16)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Date To"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(512, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(96, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Start Time"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(616, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 16)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "End Time"
        '
        'MassOvertimeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(871, 597)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
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
    Friend WithEvents StartTimeTextBox As TimeTextBox
    Friend WithEvents EndTimeTextBox As TimeTextBox
    Friend WithEvents ApplyButton As Button
    Friend WithEvents SaveButton As Button
    Friend WithEvents EmployeeTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
End Class
