<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddOfficialBusinessForm
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
        Me.EmployeeInfoTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.EmployeeNameTextBox = New System.Windows.Forms.TextBox()
        Me.EmployeeNumberTextBox = New System.Windows.Forms.TextBox()
        Me.EmployeePictureBox = New System.Windows.Forms.PictureBox()
        Me.DetailsTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CommentTextBox = New System.Windows.Forms.TextBox()
        Me.ReasonTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label163 = New System.Windows.Forms.Label()
        Me.Label168 = New System.Windows.Forms.Label()
        Me.Label167 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.StatusComboBox = New System.Windows.Forms.ComboBox()
        Me.btnAddAndClose = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.btnAddAndNew = New System.Windows.Forms.Button()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.StartDatePicker = New System.Windows.Forms.DateTimePicker()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.EndDatePicker = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.StartTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.EndTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.EmployeeInfoTabLayout.SuspendLayout()
        CType(Me.EmployeePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DetailsTabLayout.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'EmployeeInfoTabLayout
        '
        Me.EmployeeInfoTabLayout.BackColor = System.Drawing.Color.White
        Me.EmployeeInfoTabLayout.ColumnCount = 2
        Me.EmployeeInfoTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121.0!))
        Me.EmployeeInfoTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 861.0!))
        Me.EmployeeInfoTabLayout.Controls.Add(Me.EmployeeNameTextBox, 1, 0)
        Me.EmployeeInfoTabLayout.Controls.Add(Me.EmployeeNumberTextBox, 1, 1)
        Me.EmployeeInfoTabLayout.Controls.Add(Me.EmployeePictureBox, 0, 0)
        Me.EmployeeInfoTabLayout.Dock = System.Windows.Forms.DockStyle.Top
        Me.EmployeeInfoTabLayout.Location = New System.Drawing.Point(0, 0)
        Me.EmployeeInfoTabLayout.Name = "EmployeeInfoTabLayout"
        Me.EmployeeInfoTabLayout.RowCount = 2
        Me.EmployeeInfoTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.EmployeeInfoTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.EmployeeInfoTabLayout.Size = New System.Drawing.Size(684, 88)
        Me.EmployeeInfoTabLayout.TabIndex = 4
        '
        'EmployeeNameTextBox
        '
        Me.EmployeeNameTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EmployeeNameTextBox.BackColor = System.Drawing.Color.White
        Me.EmployeeNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.EmployeeNameTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.EmployeeNameTextBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.EmployeeNameTextBox.Location = New System.Drawing.Point(124, 13)
        Me.EmployeeNameTextBox.MaxLength = 250
        Me.EmployeeNameTextBox.Name = "EmployeeNameTextBox"
        Me.EmployeeNameTextBox.ReadOnly = True
        Me.EmployeeNameTextBox.Size = New System.Drawing.Size(668, 28)
        Me.EmployeeNameTextBox.TabIndex = 0
        '
        'EmployeeNumberTextBox
        '
        Me.EmployeeNumberTextBox.BackColor = System.Drawing.Color.White
        Me.EmployeeNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.EmployeeNumberTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EmployeeNumberTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.EmployeeNumberTextBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.EmployeeNumberTextBox.Location = New System.Drawing.Point(124, 47)
        Me.EmployeeNumberTextBox.MaxLength = 50
        Me.EmployeeNumberTextBox.Multiline = True
        Me.EmployeeNumberTextBox.Name = "EmployeeNumberTextBox"
        Me.EmployeeNumberTextBox.ReadOnly = True
        Me.EmployeeNumberTextBox.Size = New System.Drawing.Size(855, 38)
        Me.EmployeeNumberTextBox.TabIndex = 1
        '
        'EmployeePictureBox
        '
        Me.EmployeePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.EmployeePictureBox.Location = New System.Drawing.Point(28, 3)
        Me.EmployeePictureBox.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.EmployeePictureBox.Name = "EmployeePictureBox"
        Me.EmployeeInfoTabLayout.SetRowSpan(Me.EmployeePictureBox, 2)
        Me.EmployeePictureBox.Size = New System.Drawing.Size(88, 82)
        Me.EmployeePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EmployeePictureBox.TabIndex = 382
        Me.EmployeePictureBox.TabStop = False
        '
        'DetailsTabLayout
        '
        Me.DetailsTabLayout.ColumnCount = 3
        Me.DetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.DetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.DetailsTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.DetailsTabLayout.Controls.Add(Me.Panel5, 0, 7)
        Me.DetailsTabLayout.Controls.Add(Me.Panel3, 0, 5)
        Me.DetailsTabLayout.Controls.Add(Me.Panel2, 0, 3)
        Me.DetailsTabLayout.Controls.Add(Me.Panel4, 0, 1)
        Me.DetailsTabLayout.Controls.Add(Me.Label7, 2, 0)
        Me.DetailsTabLayout.Controls.Add(Me.CommentTextBox, 1, 5)
        Me.DetailsTabLayout.Controls.Add(Me.ReasonTextBox, 1, 1)
        Me.DetailsTabLayout.Controls.Add(Me.Label6, 1, 4)
        Me.DetailsTabLayout.Controls.Add(Me.Label5, 1, 0)
        Me.DetailsTabLayout.Controls.Add(Me.Label4, 0, 6)
        Me.DetailsTabLayout.Controls.Add(Me.Label163, 0, 4)
        Me.DetailsTabLayout.Controls.Add(Me.Label168, 0, 2)
        Me.DetailsTabLayout.Controls.Add(Me.Label167, 0, 0)
        Me.DetailsTabLayout.Controls.Add(Me.Panel1, 2, 1)
        Me.DetailsTabLayout.Dock = System.Windows.Forms.DockStyle.Top
        Me.DetailsTabLayout.Location = New System.Drawing.Point(0, 88)
        Me.DetailsTabLayout.Name = "DetailsTabLayout"
        Me.DetailsTabLayout.RowCount = 8
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.DetailsTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.DetailsTabLayout.Size = New System.Drawing.Size(684, 193)
        Me.DetailsTabLayout.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(493, 0)
        Me.Label7.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 13)
        Me.Label7.TabIndex = 516
        Me.Label7.Text = "Status"
        '
        'CommentTextBox
        '
        Me.CommentTextBox.Location = New System.Drawing.Point(220, 112)
        Me.CommentTextBox.Margin = New System.Windows.Forms.Padding(10, 0, 3, 0)
        Me.CommentTextBox.Multiline = True
        Me.CommentTextBox.Name = "CommentTextBox"
        Me.DetailsTabLayout.SetRowSpan(Me.CommentTextBox, 3)
        Me.CommentTextBox.ShortcutsEnabled = False
        Me.CommentTextBox.Size = New System.Drawing.Size(160, 74)
        Me.CommentTextBox.TabIndex = 9
        '
        'ReasonTextBox
        '
        Me.ReasonTextBox.Location = New System.Drawing.Point(220, 16)
        Me.ReasonTextBox.Margin = New System.Windows.Forms.Padding(10, 0, 2, 0)
        Me.ReasonTextBox.Multiline = True
        Me.ReasonTextBox.Name = "ReasonTextBox"
        Me.DetailsTabLayout.SetRowSpan(Me.ReasonTextBox, 3)
        Me.ReasonTextBox.ShortcutsEnabled = False
        Me.ReasonTextBox.Size = New System.Drawing.Size(160, 74)
        Me.ReasonTextBox.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(230, 96)
        Me.Label6.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 13)
        Me.Label6.TabIndex = 513
        Me.Label6.Text = "Comments"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(230, 0)
        Me.Label5.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 512
        Me.Label5.Text = "Reason"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 144)
        Me.Label4.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 510
        Me.Label4.Text = "End time"
        '
        'Label163
        '
        Me.Label163.AutoSize = True
        Me.Label163.Location = New System.Drawing.Point(20, 96)
        Me.Label163.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label163.Name = "Label163"
        Me.Label163.Size = New System.Drawing.Size(56, 13)
        Me.Label163.TabIndex = 381
        Me.Label163.Text = "Start time"
        '
        'Label168
        '
        Me.Label168.AutoSize = True
        Me.Label168.Location = New System.Drawing.Point(20, 48)
        Me.Label168.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label168.Name = "Label168"
        Me.Label168.Size = New System.Drawing.Size(53, 13)
        Me.Label168.TabIndex = 374
        Me.Label168.Text = "End date"
        '
        'Label167
        '
        Me.Label167.AutoSize = True
        Me.Label167.Location = New System.Drawing.Point(20, 0)
        Me.Label167.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label167.Name = "Label167"
        Me.Label167.Size = New System.Drawing.Size(57, 13)
        Me.Label167.TabIndex = 372
        Me.Label167.Text = "Start date"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.StatusComboBox)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(473, 16)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(211, 32)
        Me.Panel1.TabIndex = 10
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(3, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(13, 13)
        Me.Label2.TabIndex = 507
        Me.Label2.Text = "*"
        '
        'StatusComboBox
        '
        Me.StatusComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.StatusComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.StatusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.StatusComboBox.FormattingEnabled = True
        Me.StatusComboBox.Location = New System.Drawing.Point(20, 2)
        Me.StatusComboBox.Name = "StatusComboBox"
        Me.StatusComboBox.Size = New System.Drawing.Size(110, 21)
        Me.StatusComboBox.TabIndex = 10
        '
        'btnAddAndClose
        '
        Me.btnAddAndClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddAndClose.Location = New System.Drawing.Point(230, 325)
        Me.btnAddAndClose.Name = "btnAddAndClose"
        Me.btnAddAndClose.Size = New System.Drawing.Size(85, 23)
        Me.btnAddAndClose.TabIndex = 12
        Me.btnAddAndClose.Text = "&Add && Close"
        Me.btnAddAndClose.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.Location = New System.Drawing.Point(404, 325)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton.TabIndex = 11
        Me.CancelButton.Text = "&Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'btnAddAndNew
        '
        Me.btnAddAndNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddAndNew.Location = New System.Drawing.Point(321, 325)
        Me.btnAddAndNew.Name = "btnAddAndNew"
        Me.btnAddAndNew.Size = New System.Drawing.Size(75, 23)
        Me.btnAddAndNew.TabIndex = 10
        Me.btnAddAndNew.Text = "Add && &New"
        Me.btnAddAndNew.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.StartDatePicker)
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 16)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(210, 32)
        Me.Panel4.TabIndex = 14
        '
        'StartDatePicker
        '
        Me.StartDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.StartDatePicker.Location = New System.Drawing.Point(20, 3)
        Me.StartDatePicker.Name = "StartDatePicker"
        Me.StartDatePicker.Size = New System.Drawing.Size(110, 22)
        Me.StartDatePicker.TabIndex = 4
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(3, 4)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(13, 13)
        Me.Label8.TabIndex = 507
        Me.Label8.Text = "*"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.EndDatePicker)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 64)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(210, 32)
        Me.Panel2.TabIndex = 15
        '
        'EndDatePicker
        '
        Me.EndDatePicker.Enabled = False
        Me.EndDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.EndDatePicker.Location = New System.Drawing.Point(20, 3)
        Me.EndDatePicker.Name = "EndDatePicker"
        Me.EndDatePicker.Size = New System.Drawing.Size(110, 22)
        Me.EndDatePicker.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(3, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 507
        Me.Label3.Text = "*"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.StartTimePicker)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 112)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(210, 32)
        Me.Panel3.TabIndex = 14
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(3, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 507
        Me.Label1.Text = "*"
        '
        'StartTimePicker
        '
        Me.StartTimePicker.CustomFormat = "  hh:mm tt"
        Me.StartTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.StartTimePicker.Location = New System.Drawing.Point(20, 3)
        Me.StartTimePicker.Name = "StartTimePicker"
        Me.StartTimePicker.ShowUpDown = True
        Me.StartTimePicker.Size = New System.Drawing.Size(110, 22)
        Me.StartTimePicker.TabIndex = 8
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.Label9)
        Me.Panel5.Controls.Add(Me.EndTimePicker)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(0, 159)
        Me.Panel5.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(210, 34)
        Me.Panel5.TabIndex = 15
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(3, 4)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(13, 13)
        Me.Label9.TabIndex = 507
        Me.Label9.Text = "*"
        '
        'EndTimePicker
        '
        Me.EndTimePicker.CustomFormat = "  hh:mm tt"
        Me.EndTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.EndTimePicker.Location = New System.Drawing.Point(20, 3)
        Me.EndTimePicker.Name = "EndTimePicker"
        Me.EndTimePicker.ShowUpDown = True
        Me.EndTimePicker.Size = New System.Drawing.Size(110, 22)
        Me.EndTimePicker.TabIndex = 8
        '
        'AddOfficialBusinessForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(684, 361)
        Me.Controls.Add(Me.btnAddAndClose)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.btnAddAndNew)
        Me.Controls.Add(Me.DetailsTabLayout)
        Me.Controls.Add(Me.EmployeeInfoTabLayout)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AddOfficialBusinessForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Official Business"
        Me.EmployeeInfoTabLayout.ResumeLayout(False)
        Me.EmployeeInfoTabLayout.PerformLayout()
        CType(Me.EmployeePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DetailsTabLayout.ResumeLayout(False)
        Me.DetailsTabLayout.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents EmployeeInfoTabLayout As TableLayoutPanel
    Friend WithEvents EmployeeNameTextBox As TextBox
    Friend WithEvents EmployeeNumberTextBox As TextBox
    Friend WithEvents EmployeePictureBox As PictureBox
    Friend WithEvents DetailsTabLayout As TableLayoutPanel
    Friend WithEvents Label7 As Label
    Friend WithEvents CommentTextBox As TextBox
    Friend WithEvents ReasonTextBox As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label163 As Label
    Friend WithEvents Label168 As Label
    Friend WithEvents Label167 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents StatusComboBox As ComboBox
    Friend WithEvents btnAddAndClose As Button
    Friend WithEvents CancelButton As Button
    Friend WithEvents btnAddAndNew As Button
    Friend WithEvents Panel4 As Panel
    Friend WithEvents StartDatePicker As DateTimePicker
    Friend WithEvents Label8 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents EndDatePicker As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents StartTimePicker As DateTimePicker
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents EndTimePicker As DateTimePicker
End Class
