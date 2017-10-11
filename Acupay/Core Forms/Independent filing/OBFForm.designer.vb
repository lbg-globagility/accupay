<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OBFForm
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtOBFEndTime = New System.Windows.Forms.TextBox()
        Me.txtOBFStartTime = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.dtpOBFStartTime = New System.Windows.Forms.DateTimePicker()
        Me.chkOBTimeOut = New System.Windows.Forms.CheckBox()
        Me.dtpOBFEndTime = New System.Windows.Forms.DateTimePicker()
        Me.chkOBTimeIn = New System.Windows.Forms.CheckBox()
        Me.cboxEmployees = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtEmployeeFullName1 = New Acupay.txtEmployeeFullName()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboOrganization = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label199 = New System.Windows.Forms.Label()
        Me.Label198 = New System.Windows.Forms.Label()
        Me.Label195 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboOBFStatus = New Acupay.cboListOfValue()
        Me.cboOBFtype = New Acupay.cboListOfValue()
        Me.TxtEmployeeNumber1 = New Acupay.txtEmployeeNumber()
        Me.dtpOBFEndDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpOBFStartDate = New System.Windows.Forms.DateTimePicker()
        Me.Label186 = New System.Windows.Forms.Label()
        Me.Label187 = New System.Windows.Forms.Label()
        Me.Label188 = New System.Windows.Forms.Label()
        Me.Label189 = New System.Windows.Forms.Label()
        Me.Label190 = New System.Windows.Forms.Label()
        Me.Label191 = New System.Windows.Forms.Label()
        Me.Label192 = New System.Windows.Forms.Label()
        Me.txtOBFReason = New System.Windows.Forms.TextBox()
        Me.Label193 = New System.Windows.Forms.Label()
        Me.txtOBFComments = New System.Windows.Forms.TextBox()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.bgwSaving = New System.ComponentModel.BackgroundWorker()
        Me.bgwEmpNames = New System.ComponentModel.BackgroundWorker()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.txtOBFEndTime)
        Me.Panel1.Controls.Add(Me.txtOBFStartTime)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.cboxEmployees)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.TxtEmployeeFullName1)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.cboOrganization)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label199)
        Me.Panel1.Controls.Add(Me.Label198)
        Me.Panel1.Controls.Add(Me.Label195)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cboOBFStatus)
        Me.Panel1.Controls.Add(Me.cboOBFtype)
        Me.Panel1.Controls.Add(Me.TxtEmployeeNumber1)
        Me.Panel1.Controls.Add(Me.dtpOBFEndDate)
        Me.Panel1.Controls.Add(Me.dtpOBFStartDate)
        Me.Panel1.Controls.Add(Me.Label186)
        Me.Panel1.Controls.Add(Me.Label187)
        Me.Panel1.Controls.Add(Me.Label188)
        Me.Panel1.Controls.Add(Me.Label189)
        Me.Panel1.Controls.Add(Me.Label190)
        Me.Panel1.Controls.Add(Me.Label191)
        Me.Panel1.Controls.Add(Me.Label192)
        Me.Panel1.Controls.Add(Me.txtOBFReason)
        Me.Panel1.Controls.Add(Me.Label193)
        Me.Panel1.Controls.Add(Me.txtOBFComments)
        Me.Panel1.Controls.Add(Me.btnApply)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Panel1.Location = New System.Drawing.Point(0, 22)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(895, 427)
        Me.Panel1.TabIndex = 0
        '
        'txtOBFEndTime
        '
        Me.txtOBFEndTime.Location = New System.Drawing.Point(158, 257)
        Me.txtOBFEndTime.Name = "txtOBFEndTime"
        Me.txtOBFEndTime.Size = New System.Drawing.Size(196, 25)
        Me.txtOBFEndTime.TabIndex = 6
        '
        'txtOBFStartTime
        '
        Me.txtOBFStartTime.Location = New System.Drawing.Point(158, 223)
        Me.txtOBFStartTime.Name = "txtOBFStartTime"
        Me.txtOBFStartTime.Size = New System.Drawing.Size(196, 25)
        Me.txtOBFStartTime.TabIndex = 5
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.dtpOBFStartTime)
        Me.Panel2.Controls.Add(Me.chkOBTimeOut)
        Me.Panel2.Controls.Add(Me.dtpOBFEndTime)
        Me.Panel2.Controls.Add(Me.chkOBTimeIn)
        Me.Panel2.Location = New System.Drawing.Point(282, 412)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(200, 93)
        Me.Panel2.TabIndex = 359
        Me.Panel2.Visible = False
        '
        'dtpOBFStartTime
        '
        Me.dtpOBFStartTime.CalendarTitleBackColor = System.Drawing.SystemColors.ControlText
        Me.dtpOBFStartTime.CalendarTitleForeColor = System.Drawing.Color.Aqua
        Me.dtpOBFStartTime.CustomFormat = "hh:mm tt"
        Me.dtpOBFStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpOBFStartTime.Location = New System.Drawing.Point(24, 22)
        Me.dtpOBFStartTime.Name = "dtpOBFStartTime"
        Me.dtpOBFStartTime.ShowUpDown = True
        Me.dtpOBFStartTime.Size = New System.Drawing.Size(175, 25)
        Me.dtpOBFStartTime.TabIndex = 5
        '
        'chkOBTimeOut
        '
        Me.chkOBTimeOut.AutoSize = True
        Me.chkOBTimeOut.Checked = True
        Me.chkOBTimeOut.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOBTimeOut.Location = New System.Drawing.Point(3, 67)
        Me.chkOBTimeOut.Name = "chkOBTimeOut"
        Me.chkOBTimeOut.Size = New System.Drawing.Size(15, 14)
        Me.chkOBTimeOut.TabIndex = 358
        Me.chkOBTimeOut.UseVisualStyleBackColor = True
        '
        'dtpOBFEndTime
        '
        Me.dtpOBFEndTime.CustomFormat = "hh:mm tt"
        Me.dtpOBFEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpOBFEndTime.Location = New System.Drawing.Point(24, 56)
        Me.dtpOBFEndTime.Name = "dtpOBFEndTime"
        Me.dtpOBFEndTime.ShowUpDown = True
        Me.dtpOBFEndTime.Size = New System.Drawing.Size(175, 25)
        Me.dtpOBFEndTime.TabIndex = 6
        '
        'chkOBTimeIn
        '
        Me.chkOBTimeIn.AutoSize = True
        Me.chkOBTimeIn.Checked = True
        Me.chkOBTimeIn.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOBTimeIn.Location = New System.Drawing.Point(3, 33)
        Me.chkOBTimeIn.Name = "chkOBTimeIn"
        Me.chkOBTimeIn.Size = New System.Drawing.Size(15, 14)
        Me.chkOBTimeIn.TabIndex = 357
        Me.chkOBTimeIn.UseVisualStyleBackColor = True
        '
        'cboxEmployees
        '
        Me.cboxEmployees.FormattingEnabled = True
        Me.cboxEmployees.Location = New System.Drawing.Point(12, 390)
        Me.cboxEmployees.Name = "cboxEmployees"
        Me.cboxEmployees.Size = New System.Drawing.Size(121, 25)
        Me.cboxEmployees.TabIndex = 356
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
        Me.Label7.TabIndex = 262
        Me.Label7.Text = "*"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(54, 47)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(66, 17)
        Me.Label3.TabIndex = 261
        Me.Label3.Text = "Full Name"
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
        Me.Label5.Location = New System.Drawing.Point(388, 95)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 17)
        Me.Label5.TabIndex = 258
        Me.Label5.Text = "Employer"
        Me.Label5.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(446, 85)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(18, 24)
        Me.Label6.TabIndex = 259
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
        Me.cboOrganization.Location = New System.Drawing.Point(465, 87)
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
        Me.Label4.Location = New System.Drawing.Point(133, 288)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(18, 24)
        Me.Label4.TabIndex = 256
        Me.Label4.Text = "*"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(133, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 24)
        Me.Label2.TabIndex = 255
        Me.Label2.Text = "*"
        '
        'Label199
        '
        Me.Label199.AutoSize = True
        Me.Label199.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label199.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label199.Location = New System.Drawing.Point(133, 189)
        Me.Label199.Name = "Label199"
        Me.Label199.Size = New System.Drawing.Size(18, 24)
        Me.Label199.TabIndex = 254
        Me.Label199.Text = "*"
        '
        'Label198
        '
        Me.Label198.AutoSize = True
        Me.Label198.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label198.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label198.Location = New System.Drawing.Point(133, 155)
        Me.Label198.Name = "Label198"
        Me.Label198.Size = New System.Drawing.Size(18, 24)
        Me.Label198.TabIndex = 253
        Me.Label198.Text = "*"
        '
        'Label195
        '
        Me.Label195.AutoSize = True
        Me.Label195.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label195.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label195.Location = New System.Drawing.Point(133, 121)
        Me.Label195.Name = "Label195"
        Me.Label195.Size = New System.Drawing.Size(18, 24)
        Me.Label195.TabIndex = 250
        Me.Label195.Text = "*"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(54, 95)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 17)
        Me.Label1.TabIndex = 249
        Me.Label1.Text = "Employee ID"
        '
        'cboOBFStatus
        '
        Me.cboOBFStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOBFStatus.DropDownWidth = 121
        Me.cboOBFStatus.FormattingEnabled = True
        Me.cboOBFStatus.ListOfValueType = "Leave Status"
        Me.cboOBFStatus.Location = New System.Drawing.Point(158, 291)
        Me.cboOBFStatus.Name = "cboOBFStatus"
        Me.cboOBFStatus.OrderByColumn = CType(CSByte(0), SByte)
        Me.cboOBFStatus.Size = New System.Drawing.Size(196, 25)
        Me.cboOBFStatus.TabIndex = 7
        '
        'cboOBFtype
        '
        Me.cboOBFtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOBFtype.DropDownWidth = 121
        Me.cboOBFtype.FormattingEnabled = True
        Me.cboOBFtype.ListOfValueType = "Official Business Type"
        Me.cboOBFtype.Location = New System.Drawing.Point(158, 121)
        Me.cboOBFtype.Name = "cboOBFtype"
        Me.cboOBFtype.OrderByColumn = CType(CSByte(0), SByte)
        Me.cboOBFtype.Size = New System.Drawing.Size(196, 25)
        Me.cboOBFtype.TabIndex = 2
        '
        'TxtEmployeeNumber1
        '
        Me.TxtEmployeeNumber1.BackColor = System.Drawing.Color.White
        Me.TxtEmployeeNumber1.Font = New System.Drawing.Font("Segoe UI Semilight", 18.0!)
        Me.TxtEmployeeNumber1.Location = New System.Drawing.Point(158, 73)
        Me.TxtEmployeeNumber1.MaxLength = 50
        Me.TxtEmployeeNumber1.Name = "TxtEmployeeNumber1"
        Me.TxtEmployeeNumber1.ReadOnly = True
        Me.TxtEmployeeNumber1.RowIDValue = ""
        Me.TxtEmployeeNumber1.Size = New System.Drawing.Size(196, 39)
        Me.TxtEmployeeNumber1.TabIndex = 1
        '
        'dtpOBFEndDate
        '
        Me.dtpOBFEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpOBFEndDate.Location = New System.Drawing.Point(158, 189)
        Me.dtpOBFEndDate.Name = "dtpOBFEndDate"
        Me.dtpOBFEndDate.Size = New System.Drawing.Size(196, 25)
        Me.dtpOBFEndDate.TabIndex = 4
        '
        'dtpOBFStartDate
        '
        Me.dtpOBFStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpOBFStartDate.Location = New System.Drawing.Point(158, 155)
        Me.dtpOBFStartDate.Name = "dtpOBFStartDate"
        Me.dtpOBFStartDate.Size = New System.Drawing.Size(196, 25)
        Me.dtpOBFStartDate.TabIndex = 3
        '
        'Label186
        '
        Me.Label186.AutoSize = True
        Me.Label186.Location = New System.Drawing.Point(54, 299)
        Me.Label186.Name = "Label186"
        Me.Label186.Size = New System.Drawing.Size(43, 17)
        Me.Label186.TabIndex = 248
        Me.Label186.Text = "Status"
        '
        'Label187
        '
        Me.Label187.AutoSize = True
        Me.Label187.Location = New System.Drawing.Point(388, 220)
        Me.Label187.Name = "Label187"
        Me.Label187.Size = New System.Drawing.Size(64, 17)
        Me.Label187.TabIndex = 246
        Me.Label187.Text = "Comment"
        '
        'Label188
        '
        Me.Label188.AutoSize = True
        Me.Label188.Location = New System.Drawing.Point(388, 131)
        Me.Label188.Name = "Label188"
        Me.Label188.Size = New System.Drawing.Size(51, 17)
        Me.Label188.TabIndex = 245
        Me.Label188.Text = "Reason"
        '
        'Label189
        '
        Me.Label189.AutoSize = True
        Me.Label189.Location = New System.Drawing.Point(54, 197)
        Me.Label189.Name = "Label189"
        Me.Label189.Size = New System.Drawing.Size(60, 17)
        Me.Label189.TabIndex = 247
        Me.Label189.Text = "End date"
        '
        'Label190
        '
        Me.Label190.AutoSize = True
        Me.Label190.Location = New System.Drawing.Point(54, 163)
        Me.Label190.Name = "Label190"
        Me.Label190.Size = New System.Drawing.Size(65, 17)
        Me.Label190.TabIndex = 244
        Me.Label190.Text = "Start date"
        '
        'Label191
        '
        Me.Label191.AutoSize = True
        Me.Label191.Location = New System.Drawing.Point(54, 129)
        Me.Label191.Name = "Label191"
        Me.Label191.Size = New System.Drawing.Size(35, 17)
        Me.Label191.TabIndex = 243
        Me.Label191.Text = "Type"
        '
        'Label192
        '
        Me.Label192.AutoSize = True
        Me.Label192.Location = New System.Drawing.Point(54, 265)
        Me.Label192.Name = "Label192"
        Me.Label192.Size = New System.Drawing.Size(59, 17)
        Me.Label192.TabIndex = 242
        Me.Label192.Text = "End time"
        '
        'txtOBFReason
        '
        Me.txtOBFReason.BackColor = System.Drawing.Color.White
        Me.txtOBFReason.Location = New System.Drawing.Point(465, 121)
        Me.txtOBFReason.MaxLength = 500
        Me.txtOBFReason.Multiline = True
        Me.txtOBFReason.Name = "txtOBFReason"
        Me.txtOBFReason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtOBFReason.Size = New System.Drawing.Size(221, 75)
        Me.txtOBFReason.TabIndex = 9
        '
        'Label193
        '
        Me.Label193.AutoSize = True
        Me.Label193.Location = New System.Drawing.Point(54, 231)
        Me.Label193.Name = "Label193"
        Me.Label193.Size = New System.Drawing.Size(64, 17)
        Me.Label193.TabIndex = 241
        Me.Label193.Text = "Start time"
        '
        'txtOBFComments
        '
        Me.txtOBFComments.BackColor = System.Drawing.Color.White
        Me.txtOBFComments.Location = New System.Drawing.Point(465, 210)
        Me.txtOBFComments.MaxLength = 2000
        Me.txtOBFComments.Multiline = True
        Me.txtOBFComments.Name = "txtOBFComments"
        Me.txtOBFComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtOBFComments.Size = New System.Drawing.Size(221, 75)
        Me.txtOBFComments.TabIndex = 10
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
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label25.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label25.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label25.Location = New System.Drawing.Point(0, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(895, 22)
        Me.Label25.TabIndex = 218
        Me.Label25.Text = "OFFICIAL BUSINESS FILING FORM"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'bgwSaving
        '
        Me.bgwSaving.WorkerReportsProgress = True
        Me.bgwSaving.WorkerSupportsCancellation = True
        '
        'bgwEmpNames
        '
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'OBFForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(895, 449)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label25)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OBFForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label199 As System.Windows.Forms.Label
    Friend WithEvents Label198 As System.Windows.Forms.Label
    Friend WithEvents Label195 As System.Windows.Forms.Label
    Friend WithEvents dtpOBFEndTime As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpOBFStartTime As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboOBFStatus As cboListOfValue
    Friend WithEvents cboOBFtype As cboListOfValue
    Friend WithEvents TxtEmployeeNumber1 As Acupay.txtEmployeeNumber
    Friend WithEvents dtpOBFEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpOBFStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label186 As System.Windows.Forms.Label
    Friend WithEvents Label187 As System.Windows.Forms.Label
    Friend WithEvents Label188 As System.Windows.Forms.Label
    Friend WithEvents Label189 As System.Windows.Forms.Label
    Friend WithEvents Label190 As System.Windows.Forms.Label
    Friend WithEvents Label191 As System.Windows.Forms.Label
    Friend WithEvents Label192 As System.Windows.Forms.Label
    Friend WithEvents txtOBFReason As System.Windows.Forms.TextBox
    Friend WithEvents Label193 As System.Windows.Forms.Label
    Friend WithEvents txtOBFComments As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents bgwSaving As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cboOrganization As System.Windows.Forms.ComboBox
    Friend WithEvents TxtEmployeeFullName1 As txtEmployeeFullName
    Friend WithEvents bgwEmpNames As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cboxEmployees As System.Windows.Forms.ComboBox
    Friend WithEvents chkOBTimeOut As CheckBox
    Friend WithEvents chkOBTimeIn As CheckBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents txtOBFEndTime As TextBox
    Friend WithEvents txtOBFStartTime As TextBox
    Friend WithEvents ErrorProvider1 As ErrorProvider
End Class
