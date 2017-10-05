<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LeaveForm
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
        Me.dtpendate = New System.Windows.Forms.DateTimePicker()
        Me.dtpstartdate = New System.Windows.Forms.DateTimePicker()
        Me.cboleavetypes = New System.Windows.Forms.ComboBox()
        Me.Label199 = New System.Windows.Forms.Label()
        Me.Label198 = New System.Windows.Forms.Label()
        Me.Label197 = New System.Windows.Forms.Label()
        Me.Label196 = New System.Windows.Forms.Label()
        Me.Label195 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.txtcomments = New System.Windows.Forms.TextBox()
        Me.txtreason = New System.Windows.Forms.TextBox()
        Me.dtpendtime = New System.Windows.Forms.DateTimePicker()
        Me.dtpstarttime = New System.Windows.Forms.DateTimePicker()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.bgSaving = New System.ComponentModel.BackgroundWorker()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cboxEmployees = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtEmployeeFullName1 = New txtEmployeeFullName()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.CboListOfValue1 = New cboListOfValue()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cboOrganization = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtEmployeeNumber1 = New txtEmployeeNumber()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.bgwEmpNames = New System.ComponentModel.BackgroundWorker()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dtpendate
        '
        Me.dtpendate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpendate.Location = New System.Drawing.Point(158, 190)
        Me.dtpendate.Name = "dtpendate"
        Me.dtpendate.Size = New System.Drawing.Size(196, 25)
        Me.dtpendate.TabIndex = 204
        '
        'dtpstartdate
        '
        Me.dtpstartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpstartdate.Location = New System.Drawing.Point(158, 156)
        Me.dtpstartdate.Name = "dtpstartdate"
        Me.dtpstartdate.Size = New System.Drawing.Size(196, 25)
        Me.dtpstartdate.TabIndex = 203
        '
        'cboleavetypes
        '
        Me.cboleavetypes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboleavetypes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboleavetypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboleavetypes.DropDownWidth = 150
        Me.cboleavetypes.FormattingEnabled = True
        Me.cboleavetypes.Location = New System.Drawing.Point(158, 122)
        Me.cboleavetypes.Name = "cboleavetypes"
        Me.cboleavetypes.Size = New System.Drawing.Size(196, 25)
        Me.cboleavetypes.TabIndex = 202
        '
        'Label199
        '
        Me.Label199.AutoSize = True
        Me.Label199.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label199.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label199.Location = New System.Drawing.Point(133, 190)
        Me.Label199.Name = "Label199"
        Me.Label199.Size = New System.Drawing.Size(19, 23)
        Me.Label199.TabIndex = 208
        Me.Label199.Text = "*"
        Me.ToolTip1.SetToolTip(Me.Label199, "This field is required")
        '
        'Label198
        '
        Me.Label198.AutoSize = True
        Me.Label198.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label198.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label198.Location = New System.Drawing.Point(133, 156)
        Me.Label198.Name = "Label198"
        Me.Label198.Size = New System.Drawing.Size(19, 23)
        Me.Label198.TabIndex = 207
        Me.Label198.Text = "*"
        Me.ToolTip1.SetToolTip(Me.Label198, "This field is required")
        '
        'Label197
        '
        Me.Label197.AutoSize = True
        Me.Label197.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label197.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label197.Location = New System.Drawing.Point(133, 258)
        Me.Label197.Name = "Label197"
        Me.Label197.Size = New System.Drawing.Size(19, 23)
        Me.Label197.TabIndex = 206
        Me.Label197.Text = "*"
        Me.ToolTip1.SetToolTip(Me.Label197, "This field is required")
        '
        'Label196
        '
        Me.Label196.AutoSize = True
        Me.Label196.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label196.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label196.Location = New System.Drawing.Point(133, 224)
        Me.Label196.Name = "Label196"
        Me.Label196.Size = New System.Drawing.Size(19, 23)
        Me.Label196.TabIndex = 205
        Me.Label196.Text = "*"
        Me.ToolTip1.SetToolTip(Me.Label196, "This field is required")
        '
        'Label195
        '
        Me.Label195.AutoSize = True
        Me.Label195.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label195.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label195.Location = New System.Drawing.Point(133, 122)
        Me.Label195.Name = "Label195"
        Me.Label195.Size = New System.Drawing.Size(19, 23)
        Me.Label195.TabIndex = 204
        Me.Label195.Text = "*"
        Me.ToolTip1.SetToolTip(Me.Label195, "This field is required")
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(388, 221)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(64, 17)
        Me.Label32.TabIndex = 202
        Me.Label32.Text = "Comment"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(388, 132)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(51, 17)
        Me.Label33.TabIndex = 201
        Me.Label33.Text = "Reason"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(54, 200)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(60, 17)
        Me.Label34.TabIndex = 203
        Me.Label34.Text = "End date"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(54, 166)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(65, 17)
        Me.Label35.TabIndex = 200
        Me.Label35.Text = "Start date"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(54, 132)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(70, 17)
        Me.Label36.TabIndex = 199
        Me.Label36.Text = "Leave type"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(54, 268)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(59, 17)
        Me.Label37.TabIndex = 198
        Me.Label37.Text = "End time"
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(54, 234)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(64, 17)
        Me.Label38.TabIndex = 197
        Me.Label38.Text = "Start time"
        '
        'txtcomments
        '
        Me.txtcomments.BackColor = System.Drawing.Color.White
        Me.txtcomments.Location = New System.Drawing.Point(465, 211)
        Me.txtcomments.MaxLength = 2000
        Me.txtcomments.Multiline = True
        Me.txtcomments.Name = "txtcomments"
        Me.txtcomments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtcomments.Size = New System.Drawing.Size(221, 75)
        Me.txtcomments.TabIndex = 209
        '
        'txtreason
        '
        Me.txtreason.BackColor = System.Drawing.Color.White
        Me.txtreason.Location = New System.Drawing.Point(465, 122)
        Me.txtreason.MaxLength = 500
        Me.txtreason.Multiline = True
        Me.txtreason.Name = "txtreason"
        Me.txtreason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtreason.Size = New System.Drawing.Size(221, 75)
        Me.txtreason.TabIndex = 208
        '
        'dtpendtime
        '
        Me.dtpendtime.CustomFormat = "hh:mm tt"
        Me.dtpendtime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpendtime.Location = New System.Drawing.Point(158, 258)
        Me.dtpendtime.Name = "dtpendtime"
        Me.dtpendtime.ShowUpDown = True
        Me.dtpendtime.Size = New System.Drawing.Size(196, 25)
        Me.dtpendtime.TabIndex = 206
        '
        'dtpstarttime
        '
        Me.dtpstarttime.CustomFormat = "hh:mm tt"
        Me.dtpstarttime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpstarttime.Location = New System.Drawing.Point(158, 224)
        Me.dtpstarttime.Name = "dtpstarttime"
        Me.dtpstarttime.ShowUpDown = True
        Me.dtpstarttime.Size = New System.Drawing.Size(196, 25)
        Me.dtpstarttime.TabIndex = 205
        '
        'btnApply
        '
        Me.btnApply.Font = New System.Drawing.Font("Segoe UI Semilight", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApply.Location = New System.Drawing.Point(699, 354)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(184, 61)
        Me.btnApply.TabIndex = 210
        Me.btnApply.Text = "&Done"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(54, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 17)
        Me.Label1.TabIndex = 213
        Me.Label1.Text = "Employee ID"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(133, 86)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(19, 23)
        Me.Label2.TabIndex = 214
        Me.Label2.Text = "*"
        Me.ToolTip1.SetToolTip(Me.Label2, "This field is required")
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label25.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label25.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label25.Location = New System.Drawing.Point(0, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(895, 22)
        Me.Label25.TabIndex = 215
        Me.Label25.Text = "LEAVE FORM"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'bgSaving
        '
        Me.bgSaving.WorkerReportsProgress = True
        Me.bgSaving.WorkerSupportsCancellation = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.cboxEmployees)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.TxtEmployeeFullName1)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.CboListOfValue1)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.cboOrganization)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.TxtEmployeeNumber1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtreason)
        Me.Panel1.Controls.Add(Me.txtcomments)
        Me.Panel1.Controls.Add(Me.btnApply)
        Me.Panel1.Controls.Add(Me.Label38)
        Me.Panel1.Controls.Add(Me.dtpendtime)
        Me.Panel1.Controls.Add(Me.Label37)
        Me.Panel1.Controls.Add(Me.dtpstarttime)
        Me.Panel1.Controls.Add(Me.Label36)
        Me.Panel1.Controls.Add(Me.dtpendate)
        Me.Panel1.Controls.Add(Me.Label35)
        Me.Panel1.Controls.Add(Me.dtpstartdate)
        Me.Panel1.Controls.Add(Me.Label34)
        Me.Panel1.Controls.Add(Me.cboleavetypes)
        Me.Panel1.Controls.Add(Me.Label33)
        Me.Panel1.Controls.Add(Me.Label199)
        Me.Panel1.Controls.Add(Me.Label32)
        Me.Panel1.Controls.Add(Me.Label198)
        Me.Panel1.Controls.Add(Me.Label195)
        Me.Panel1.Controls.Add(Me.Label197)
        Me.Panel1.Controls.Add(Me.Label196)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 22)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(895, 427)
        Me.Panel1.TabIndex = 0
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(139, 398)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(31, 17)
        Me.Label9.TabIndex = 267
        Me.Label9.Text = "- - -"
        Me.Label9.Visible = False
        '
        'cboxEmployees
        '
        Me.cboxEmployees.FormattingEnabled = True
        Me.cboxEmployees.Location = New System.Drawing.Point(12, 390)
        Me.cboxEmployees.Name = "cboxEmployees"
        Me.cboxEmployees.Size = New System.Drawing.Size(121, 25)
        Me.cboxEmployees.TabIndex = 266
        Me.cboxEmployees.Visible = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(133, 41)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(19, 23)
        Me.Label7.TabIndex = 265
        Me.Label7.Text = "*"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(54, 47)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 17)
        Me.Label8.TabIndex = 264
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
        Me.TxtEmployeeFullName1.TabIndex = 200
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(388, 96)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 17)
        Me.Label5.TabIndex = 218
        Me.Label5.Text = "Employer"
        Me.Label5.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(446, 86)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(19, 23)
        Me.Label6.TabIndex = 219
        Me.Label6.Text = "*"
        Me.ToolTip1.SetToolTip(Me.Label6, "This field is required")
        Me.Label6.Visible = False
        '
        'CboListOfValue1
        '
        Me.CboListOfValue1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboListOfValue1.DropDownWidth = 121
        Me.CboListOfValue1.FormattingEnabled = True
        Me.CboListOfValue1.ListOfValueType = "Leave Status"
        Me.CboListOfValue1.Location = New System.Drawing.Point(158, 292)
        Me.CboListOfValue1.Name = "CboListOfValue1"
        Me.CboListOfValue1.OrderByColumn = CType(CSByte(0), SByte)
        Me.CboListOfValue1.Size = New System.Drawing.Size(196, 25)
        Me.CboListOfValue1.TabIndex = 207
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(54, 299)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 17)
        Me.Label3.TabIndex = 216
        Me.Label3.Text = "Status"
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
        Me.cboOrganization.TabIndex = 6
        Me.cboOrganization.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Gisha", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(133, 289)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(19, 23)
        Me.Label4.TabIndex = 217
        Me.Label4.Text = "*"
        Me.ToolTip1.SetToolTip(Me.Label4, "This field is required")
        '
        'TxtEmployeeNumber1
        '
        Me.TxtEmployeeNumber1.Font = New System.Drawing.Font("Segoe UI Semilight", 18.0!)
        Me.TxtEmployeeNumber1.Location = New System.Drawing.Point(158, 74)
        Me.TxtEmployeeNumber1.MaxLength = 50
        Me.TxtEmployeeNumber1.Name = "TxtEmployeeNumber1"
        Me.TxtEmployeeNumber1.RowIDValue = ""
        Me.TxtEmployeeNumber1.Size = New System.Drawing.Size(196, 39)
        Me.TxtEmployeeNumber1.TabIndex = 201
        '
        'bgwEmpNames
        '
        '
        'LeaveForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(895, 449)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label25)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LeaveForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dtpendate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpstartdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents cboleavetypes As System.Windows.Forms.ComboBox
    Friend WithEvents Label199 As System.Windows.Forms.Label
    Friend WithEvents Label198 As System.Windows.Forms.Label
    Friend WithEvents Label197 As System.Windows.Forms.Label
    Friend WithEvents Label196 As System.Windows.Forms.Label
    Friend WithEvents Label195 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents txtcomments As System.Windows.Forms.TextBox
    Friend WithEvents txtreason As System.Windows.Forms.TextBox
    Friend WithEvents dtpendtime As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpstarttime As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents TxtEmployeeNumber1 As txtEmployeeNumber
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents bgSaving As System.ComponentModel.BackgroundWorker
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboOrganization As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents CboListOfValue1 As cboListOfValue
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TxtEmployeeFullName1 As txtEmployeeFullName
    Friend WithEvents bgwEmpNames As System.ComponentModel.BackgroundWorker
    Friend WithEvents cboxEmployees As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
End Class
