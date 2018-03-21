<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OverTimeForm
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
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cboxEmployees = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtEmployeeFullName1 = New AccuPay.txtEmployeeFullName()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboOrganization = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label199 = New System.Windows.Forms.Label()
        Me.Label198 = New System.Windows.Forms.Label()
        Me.Label195 = New System.Windows.Forms.Label()
        Me.Label197 = New System.Windows.Forms.Label()
        Me.Label196 = New System.Windows.Forms.Label()
        Me.dtpendtime = New System.Windows.Forms.DateTimePicker()
        Me.dtpstarttime = New System.Windows.Forms.DateTimePicker()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboOTStatus = New AccuPay.cboListOfValue()
        Me.cboOTType = New AccuPay.cboListOfValue()
        Me.TxtEmployeeNumber1 = New AccuPay.txtEmployeeNumber()
        Me.dtpendateEmpOT = New System.Windows.Forms.DateTimePicker()
        Me.dtpstartdateEmpOT = New System.Windows.Forms.DateTimePicker()
        Me.Label186 = New System.Windows.Forms.Label()
        Me.Label187 = New System.Windows.Forms.Label()
        Me.Label188 = New System.Windows.Forms.Label()
        Me.Label189 = New System.Windows.Forms.Label()
        Me.Label190 = New System.Windows.Forms.Label()
        Me.Label191 = New System.Windows.Forms.Label()
        Me.Label192 = New System.Windows.Forms.Label()
        Me.txtreasonEmpOT = New System.Windows.Forms.TextBox()
        Me.Label193 = New System.Windows.Forms.Label()
        Me.txtcommentsEmpOT = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.bgwSaving = New System.ComponentModel.BackgroundWorker()
        Me.bgwEmpNames = New System.ComponentModel.BackgroundWorker()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label25.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label25.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label25.Location = New System.Drawing.Point(0, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(895, 22)
        Me.Label25.TabIndex = 216
        Me.Label25.Text = "OVERTIME FORM"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.cboxEmployees)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.TxtEmployeeFullName1)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.cboOrganization)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label199)
        Me.Panel1.Controls.Add(Me.Label198)
        Me.Panel1.Controls.Add(Me.Label195)
        Me.Panel1.Controls.Add(Me.Label197)
        Me.Panel1.Controls.Add(Me.Label196)
        Me.Panel1.Controls.Add(Me.dtpendtime)
        Me.Panel1.Controls.Add(Me.dtpstarttime)
        Me.Panel1.Controls.Add(Me.btnApply)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cboOTStatus)
        Me.Panel1.Controls.Add(Me.cboOTType)
        Me.Panel1.Controls.Add(Me.TxtEmployeeNumber1)
        Me.Panel1.Controls.Add(Me.dtpendateEmpOT)
        Me.Panel1.Controls.Add(Me.dtpstartdateEmpOT)
        Me.Panel1.Controls.Add(Me.Label186)
        Me.Panel1.Controls.Add(Me.Label187)
        Me.Panel1.Controls.Add(Me.Label188)
        Me.Panel1.Controls.Add(Me.Label189)
        Me.Panel1.Controls.Add(Me.Label190)
        Me.Panel1.Controls.Add(Me.Label191)
        Me.Panel1.Controls.Add(Me.Label192)
        Me.Panel1.Controls.Add(Me.txtreasonEmpOT)
        Me.Panel1.Controls.Add(Me.Label193)
        Me.Panel1.Controls.Add(Me.txtcommentsEmpOT)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Panel1.Location = New System.Drawing.Point(0, 22)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(895, 427)
        Me.Panel1.TabIndex = 0
        '
        'cboxEmployees
        '
        Me.cboxEmployees.FormattingEnabled = True
        Me.cboxEmployees.Location = New System.Drawing.Point(12, 390)
        Me.cboxEmployees.Name = "cboxEmployees"
        Me.cboxEmployees.Size = New System.Drawing.Size(121, 25)
        Me.cboxEmployees.TabIndex = 269
        Me.cboxEmployees.Visible = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(133, 41)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(18, 24)
        Me.Label7.TabIndex = 268
        Me.Label7.Text = "*"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(54, 47)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 17)
        Me.Label8.TabIndex = 267
        Me.Label8.Text = "Full Name"
        '
        'TxtEmployeeFullName1
        '
        Me.TxtEmployeeFullName1.BackColor = System.Drawing.SystemColors.Window
        Me.TxtEmployeeFullName1.BorderColor = System.Drawing.Color.LightSteelBlue
        Me.TxtEmployeeFullName1.EmployeeTableColumnName = ""
        Me.TxtEmployeeFullName1.Enabled = False
        Me.TxtEmployeeFullName1.Font = New System.Drawing.Font("Segoe UI Semilight", 18.0!)
        Me.TxtEmployeeFullName1.Location = New System.Drawing.Point(158, 24)
        Me.TxtEmployeeFullName1.Name = "TxtEmployeeFullName1"
        Me.TxtEmployeeFullName1.Padding = New System.Windows.Forms.Padding(4)
        Me.TxtEmployeeFullName1.PopupWidth = 120
        Me.TxtEmployeeFullName1.SelectedItemBackColor = System.Drawing.SystemColors.Highlight
        Me.TxtEmployeeFullName1.SelectedItemForeColor = System.Drawing.SystemColors.HighlightText
        Me.TxtEmployeeFullName1.Size = New System.Drawing.Size(465, 40)
        Me.TxtEmployeeFullName1.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(388, 96)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 17)
        Me.Label5.TabIndex = 261
        Me.Label5.Text = "Employer"
        Me.Label5.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(446, 86)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(18, 24)
        Me.Label6.TabIndex = 262
        Me.Label6.Text = "*"
        Me.ToolTip1.SetToolTip(Me.Label6, "This field is required")
        Me.Label6.Visible = False
        '
        'cboOrganization
        '
        Me.cboOrganization.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboOrganization.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboOrganization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOrganization.DropDownWidth = 150
        Me.cboOrganization.FormattingEnabled = True
        Me.cboOrganization.Location = New System.Drawing.Point(465, 88)
        Me.cboOrganization.Name = "cboOrganization"
        Me.cboOrganization.Size = New System.Drawing.Size(221, 25)
        Me.cboOrganization.TabIndex = 8
        Me.cboOrganization.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(133, 289)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(18, 24)
        Me.Label4.TabIndex = 231
        Me.Label4.Text = "*"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(133, 86)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 24)
        Me.Label2.TabIndex = 230
        Me.Label2.Text = "*"
        '
        'Label199
        '
        Me.Label199.AutoSize = True
        Me.Label199.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label199.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label199.Location = New System.Drawing.Point(133, 190)
        Me.Label199.Name = "Label199"
        Me.Label199.Size = New System.Drawing.Size(18, 24)
        Me.Label199.TabIndex = 229
        Me.Label199.Text = "*"
        '
        'Label198
        '
        Me.Label198.AutoSize = True
        Me.Label198.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label198.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label198.Location = New System.Drawing.Point(133, 156)
        Me.Label198.Name = "Label198"
        Me.Label198.Size = New System.Drawing.Size(18, 24)
        Me.Label198.TabIndex = 228
        Me.Label198.Text = "*"
        '
        'Label195
        '
        Me.Label195.AutoSize = True
        Me.Label195.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label195.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label195.Location = New System.Drawing.Point(133, 122)
        Me.Label195.Name = "Label195"
        Me.Label195.Size = New System.Drawing.Size(18, 24)
        Me.Label195.TabIndex = 225
        Me.Label195.Text = "*"
        '
        'Label197
        '
        Me.Label197.AutoSize = True
        Me.Label197.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label197.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label197.Location = New System.Drawing.Point(133, 258)
        Me.Label197.Name = "Label197"
        Me.Label197.Size = New System.Drawing.Size(18, 24)
        Me.Label197.TabIndex = 227
        Me.Label197.Text = "*"
        '
        'Label196
        '
        Me.Label196.AutoSize = True
        Me.Label196.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label196.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label196.Location = New System.Drawing.Point(133, 224)
        Me.Label196.Name = "Label196"
        Me.Label196.Size = New System.Drawing.Size(18, 24)
        Me.Label196.TabIndex = 226
        Me.Label196.Text = "*"
        '
        'dtpendtime
        '
        Me.dtpendtime.CustomFormat = "hh:mm tt"
        Me.dtpendtime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpendtime.Location = New System.Drawing.Point(158, 258)
        Me.dtpendtime.Name = "dtpendtime"
        Me.dtpendtime.ShowUpDown = True
        Me.dtpendtime.Size = New System.Drawing.Size(196, 25)
        Me.dtpendtime.TabIndex = 6
        '
        'dtpstarttime
        '
        Me.dtpstarttime.CustomFormat = "hh:mm tt"
        Me.dtpstarttime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpstarttime.Location = New System.Drawing.Point(158, 224)
        Me.dtpstarttime.Name = "dtpstarttime"
        Me.dtpstarttime.ShowUpDown = True
        Me.dtpstarttime.Size = New System.Drawing.Size(196, 25)
        Me.dtpstarttime.TabIndex = 5
        '
        'btnApply
        '
        Me.btnApply.Font = New System.Drawing.Font("Segoe UI Semilight", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApply.Location = New System.Drawing.Point(699, 354)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(184, 61)
        Me.btnApply.TabIndex = 11
        Me.btnApply.Text = "&Done"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(54, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 17)
        Me.Label1.TabIndex = 221
        Me.Label1.Text = "Employee ID"
        '
        'cboOTStatus
        '
        Me.cboOTStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOTStatus.DropDownWidth = 121
        Me.cboOTStatus.FormattingEnabled = True
        Me.cboOTStatus.ListOfValueType = "Leave Status"
        Me.cboOTStatus.Location = New System.Drawing.Point(158, 292)
        Me.cboOTStatus.Name = "cboOTStatus"
        Me.cboOTStatus.OrderByColumn = CType(CSByte(0), SByte)
        Me.cboOTStatus.Size = New System.Drawing.Size(196, 25)
        Me.cboOTStatus.TabIndex = 7
        '
        'cboOTType
        '
        Me.cboOTType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOTType.DropDownWidth = 121
        Me.cboOTType.FormattingEnabled = True
        Me.cboOTType.ListOfValueType = "Employee OT Type"
        Me.cboOTType.Location = New System.Drawing.Point(158, 122)
        Me.cboOTType.Name = "cboOTType"
        Me.cboOTType.OrderByColumn = CType(CSByte(0), SByte)
        Me.cboOTType.Size = New System.Drawing.Size(196, 25)
        Me.cboOTType.TabIndex = 2
        '
        'TxtEmployeeNumber1
        '
        Me.TxtEmployeeNumber1.BackColor = System.Drawing.Color.White
        Me.TxtEmployeeNumber1.Font = New System.Drawing.Font("Segoe UI Semilight", 18.0!)
        Me.TxtEmployeeNumber1.Location = New System.Drawing.Point(158, 74)
        Me.TxtEmployeeNumber1.MaxLength = 50
        Me.TxtEmployeeNumber1.Name = "TxtEmployeeNumber1"
        Me.TxtEmployeeNumber1.ReadOnly = True
        Me.TxtEmployeeNumber1.RowIDValue = ""
        Me.TxtEmployeeNumber1.Size = New System.Drawing.Size(196, 39)
        Me.TxtEmployeeNumber1.TabIndex = 1
        '
        'dtpendateEmpOT
        '
        Me.dtpendateEmpOT.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpendateEmpOT.Location = New System.Drawing.Point(158, 190)
        Me.dtpendateEmpOT.Name = "dtpendateEmpOT"
        Me.dtpendateEmpOT.Size = New System.Drawing.Size(196, 25)
        Me.dtpendateEmpOT.TabIndex = 4
        '
        'dtpstartdateEmpOT
        '
        Me.dtpstartdateEmpOT.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpstartdateEmpOT.Location = New System.Drawing.Point(158, 156)
        Me.dtpstartdateEmpOT.Name = "dtpstartdateEmpOT"
        Me.dtpstartdateEmpOT.Size = New System.Drawing.Size(196, 25)
        Me.dtpstartdateEmpOT.TabIndex = 3
        '
        'Label186
        '
        Me.Label186.AutoSize = True
        Me.Label186.Location = New System.Drawing.Point(54, 300)
        Me.Label186.Name = "Label186"
        Me.Label186.Size = New System.Drawing.Size(43, 17)
        Me.Label186.TabIndex = 211
        Me.Label186.Text = "Status"
        '
        'Label187
        '
        Me.Label187.AutoSize = True
        Me.Label187.Location = New System.Drawing.Point(388, 221)
        Me.Label187.Name = "Label187"
        Me.Label187.Size = New System.Drawing.Size(64, 17)
        Me.Label187.TabIndex = 209
        Me.Label187.Text = "Comment"
        '
        'Label188
        '
        Me.Label188.AutoSize = True
        Me.Label188.Location = New System.Drawing.Point(388, 132)
        Me.Label188.Name = "Label188"
        Me.Label188.Size = New System.Drawing.Size(51, 17)
        Me.Label188.TabIndex = 208
        Me.Label188.Text = "Reason"
        '
        'Label189
        '
        Me.Label189.AutoSize = True
        Me.Label189.Location = New System.Drawing.Point(54, 198)
        Me.Label189.Name = "Label189"
        Me.Label189.Size = New System.Drawing.Size(60, 17)
        Me.Label189.TabIndex = 210
        Me.Label189.Text = "End date"
        '
        'Label190
        '
        Me.Label190.AutoSize = True
        Me.Label190.Location = New System.Drawing.Point(54, 164)
        Me.Label190.Name = "Label190"
        Me.Label190.Size = New System.Drawing.Size(65, 17)
        Me.Label190.TabIndex = 207
        Me.Label190.Text = "Start date"
        '
        'Label191
        '
        Me.Label191.AutoSize = True
        Me.Label191.Location = New System.Drawing.Point(54, 130)
        Me.Label191.Name = "Label191"
        Me.Label191.Size = New System.Drawing.Size(35, 17)
        Me.Label191.TabIndex = 206
        Me.Label191.Text = "Type"
        '
        'Label192
        '
        Me.Label192.AutoSize = True
        Me.Label192.Location = New System.Drawing.Point(54, 266)
        Me.Label192.Name = "Label192"
        Me.Label192.Size = New System.Drawing.Size(59, 17)
        Me.Label192.TabIndex = 205
        Me.Label192.Text = "End time"
        '
        'txtreasonEmpOT
        '
        Me.txtreasonEmpOT.BackColor = System.Drawing.Color.White
        Me.txtreasonEmpOT.Location = New System.Drawing.Point(465, 122)
        Me.txtreasonEmpOT.MaxLength = 500
        Me.txtreasonEmpOT.Multiline = True
        Me.txtreasonEmpOT.Name = "txtreasonEmpOT"
        Me.txtreasonEmpOT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtreasonEmpOT.Size = New System.Drawing.Size(221, 75)
        Me.txtreasonEmpOT.TabIndex = 9
        '
        'Label193
        '
        Me.Label193.AutoSize = True
        Me.Label193.Location = New System.Drawing.Point(54, 232)
        Me.Label193.Name = "Label193"
        Me.Label193.Size = New System.Drawing.Size(64, 17)
        Me.Label193.TabIndex = 204
        Me.Label193.Text = "Start time"
        '
        'txtcommentsEmpOT
        '
        Me.txtcommentsEmpOT.BackColor = System.Drawing.Color.White
        Me.txtcommentsEmpOT.Location = New System.Drawing.Point(465, 211)
        Me.txtcommentsEmpOT.MaxLength = 2000
        Me.txtcommentsEmpOT.Multiline = True
        Me.txtcommentsEmpOT.Name = "txtcommentsEmpOT"
        Me.txtcommentsEmpOT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtcommentsEmpOT.Size = New System.Drawing.Size(221, 75)
        Me.txtcommentsEmpOT.TabIndex = 10
        '
        'bgwSaving
        '
        Me.bgwSaving.WorkerReportsProgress = True
        Me.bgwSaving.WorkerSupportsCancellation = True
        '
        'bgwEmpNames
        '
        '
        'OverTimeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(895, 449)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label25)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OverTimeForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dtpendateEmpOT As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpstartdateEmpOT As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label186 As System.Windows.Forms.Label
    Friend WithEvents Label187 As System.Windows.Forms.Label
    Friend WithEvents Label188 As System.Windows.Forms.Label
    Friend WithEvents Label189 As System.Windows.Forms.Label
    Friend WithEvents Label190 As System.Windows.Forms.Label
    Friend WithEvents Label191 As System.Windows.Forms.Label
    Friend WithEvents Label192 As System.Windows.Forms.Label
    Friend WithEvents txtreasonEmpOT As System.Windows.Forms.TextBox
    Friend WithEvents Label193 As System.Windows.Forms.Label
    Friend WithEvents txtcommentsEmpOT As System.Windows.Forms.TextBox
    Friend WithEvents TxtEmployeeNumber1 As AccuPay.txtEmployeeNumber
    Friend WithEvents cboOTType As cboListOfValue
    Friend WithEvents cboOTStatus As cboListOfValue
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents dtpendtime As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpstarttime As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label199 As System.Windows.Forms.Label
    Friend WithEvents Label198 As System.Windows.Forms.Label
    Friend WithEvents Label195 As System.Windows.Forms.Label
    Friend WithEvents Label197 As System.Windows.Forms.Label
    Friend WithEvents Label196 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents bgwSaving As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cboOrganization As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TxtEmployeeFullName1 As txtEmployeeFullName
    Friend WithEvents bgwEmpNames As System.ComponentModel.BackgroundWorker
    Friend WithEvents cboxEmployees As System.Windows.Forms.ComboBox
End Class
