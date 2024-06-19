<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SelfFilingForm
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
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LabelTitle = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtreason = New System.Windows.Forms.TextBox()
        Me.txtcomments = New System.Windows.Forms.TextBox()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TxtEmployeeFullName = New SergeUtils.EasyCompletionComboBox()
        Me.TxtEmployeeID = New System.Windows.Forms.TextBox()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.dtpendtime = New System.Windows.Forms.DateTimePicker()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.dtpstarttime = New System.Windows.Forms.DateTimePicker()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.dtpenddate = New System.Windows.Forms.DateTimePicker()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.dtpstartdate = New System.Windows.Forms.DateTimePicker()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.cboleavetypes = New System.Windows.Forms.ComboBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label199 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label198 = New System.Windows.Forms.Label()
        Me.Label195 = New System.Windows.Forms.Label()
        Me.Label197 = New System.Windows.Forms.Label()
        Me.Label196 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(203, 41)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(18, 24)
        Me.Label7.TabIndex = 265
        Me.Label7.Text = "*"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(124, 47)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 17)
        Me.Label8.TabIndex = 264
        Me.Label8.Text = "Full Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(203, 86)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 24)
        Me.Label2.TabIndex = 214
        Me.Label2.Text = "*"
        '
        'LabelTitle
        '
        Me.LabelTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.LabelTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelTitle.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.LabelTitle.Location = New System.Drawing.Point(0, 0)
        Me.LabelTitle.Name = "LabelTitle"
        Me.LabelTitle.Size = New System.Drawing.Size(895, 22)
        Me.LabelTitle.TabIndex = 217
        Me.LabelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(124, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 17)
        Me.Label1.TabIndex = 213
        Me.Label1.Text = "Employee ID"
        '
        'txtreason
        '
        Me.txtreason.BackColor = System.Drawing.Color.White
        Me.txtreason.Location = New System.Drawing.Point(535, 77)
        Me.txtreason.MaxLength = 500
        Me.txtreason.Multiline = True
        Me.txtreason.Name = "txtreason"
        Me.txtreason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtreason.Size = New System.Drawing.Size(221, 100)
        Me.txtreason.TabIndex = 7
        '
        'txtcomments
        '
        Me.txtcomments.BackColor = System.Drawing.Color.White
        Me.txtcomments.Location = New System.Drawing.Point(535, 183)
        Me.txtcomments.MaxLength = 2000
        Me.txtcomments.Multiline = True
        Me.txtcomments.Name = "txtcomments"
        Me.txtcomments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtcomments.Size = New System.Drawing.Size(221, 100)
        Me.txtcomments.TabIndex = 8
        '
        'btnApply
        '
        Me.btnApply.Font = New System.Drawing.Font("Segoe UI Semilight", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApply.Location = New System.Drawing.Point(699, 354)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(184, 61)
        Me.btnApply.TabIndex = 9
        Me.btnApply.Text = "&Done"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.TxtEmployeeFullName)
        Me.Panel1.Controls.Add(Me.TxtEmployeeID)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label8)
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
        Me.Panel1.Controls.Add(Me.dtpenddate)
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
        'TxtEmployeeFullName
        '
        Me.TxtEmployeeFullName.Font = New System.Drawing.Font("Segoe UI Semilight", 18.0!)
        Me.TxtEmployeeFullName.FormattingEnabled = True
        Me.TxtEmployeeFullName.Location = New System.Drawing.Point(228, 31)
        Me.TxtEmployeeFullName.Name = "TxtEmployeeFullName"
        Me.TxtEmployeeFullName.Size = New System.Drawing.Size(528, 40)
        Me.TxtEmployeeFullName.TabIndex = 266
        '
        'TxtEmployeeID
        '
        Me.TxtEmployeeID.BackColor = System.Drawing.Color.White
        Me.TxtEmployeeID.Font = New System.Drawing.Font("Segoe UI Semilight", 18.0!)
        Me.TxtEmployeeID.Location = New System.Drawing.Point(228, 77)
        Me.TxtEmployeeID.Name = "TxtEmployeeID"
        Me.TxtEmployeeID.ReadOnly = True
        Me.TxtEmployeeID.Size = New System.Drawing.Size(196, 39)
        Me.TxtEmployeeID.TabIndex = 1
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(124, 234)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(64, 17)
        Me.Label38.TabIndex = 197
        Me.Label38.Text = "Start time"
        '
        'dtpendtime
        '
        Me.dtpendtime.CustomFormat = "hh:mm tt"
        Me.dtpendtime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpendtime.Location = New System.Drawing.Point(228, 258)
        Me.dtpendtime.Name = "dtpendtime"
        Me.dtpendtime.ShowCheckBox = True
        Me.dtpendtime.ShowUpDown = True
        Me.dtpendtime.Size = New System.Drawing.Size(196, 25)
        Me.dtpendtime.TabIndex = 6
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(124, 268)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(59, 17)
        Me.Label37.TabIndex = 198
        Me.Label37.Text = "End time"
        '
        'dtpstarttime
        '
        Me.dtpstarttime.CustomFormat = "hh:mm tt"
        Me.dtpstarttime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpstarttime.Location = New System.Drawing.Point(228, 224)
        Me.dtpstarttime.Name = "dtpstarttime"
        Me.dtpstarttime.ShowCheckBox = True
        Me.dtpstarttime.ShowUpDown = True
        Me.dtpstarttime.Size = New System.Drawing.Size(196, 25)
        Me.dtpstarttime.TabIndex = 5
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(124, 132)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(70, 17)
        Me.Label36.TabIndex = 199
        Me.Label36.Text = "Leave type"
        Me.Label36.Visible = False
        '
        'dtpenddate
        '
        Me.dtpenddate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpenddate.Location = New System.Drawing.Point(228, 190)
        Me.dtpenddate.Name = "dtpenddate"
        Me.dtpenddate.Size = New System.Drawing.Size(196, 25)
        Me.dtpenddate.TabIndex = 4
        Me.dtpenddate.Visible = False
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(124, 166)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(65, 17)
        Me.Label35.TabIndex = 200
        Me.Label35.Text = "Start date"
        '
        'dtpstartdate
        '
        Me.dtpstartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpstartdate.Location = New System.Drawing.Point(228, 156)
        Me.dtpstartdate.Name = "dtpstartdate"
        Me.dtpstartdate.Size = New System.Drawing.Size(196, 25)
        Me.dtpstartdate.TabIndex = 3
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(124, 200)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(60, 17)
        Me.Label34.TabIndex = 203
        Me.Label34.Text = "End date"
        Me.Label34.Visible = False
        '
        'cboleavetypes
        '
        Me.cboleavetypes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboleavetypes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboleavetypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboleavetypes.DropDownWidth = 150
        Me.cboleavetypes.FormattingEnabled = True
        Me.cboleavetypes.Location = New System.Drawing.Point(228, 122)
        Me.cboleavetypes.Name = "cboleavetypes"
        Me.cboleavetypes.Size = New System.Drawing.Size(196, 25)
        Me.cboleavetypes.TabIndex = 2
        Me.cboleavetypes.Visible = False
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(458, 87)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(51, 17)
        Me.Label33.TabIndex = 201
        Me.Label33.Text = "Reason"
        '
        'Label199
        '
        Me.Label199.AutoSize = True
        Me.Label199.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label199.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label199.Location = New System.Drawing.Point(203, 190)
        Me.Label199.Name = "Label199"
        Me.Label199.Size = New System.Drawing.Size(18, 24)
        Me.Label199.TabIndex = 208
        Me.Label199.Text = "*"
        Me.Label199.Visible = False
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(458, 193)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(64, 17)
        Me.Label32.TabIndex = 202
        Me.Label32.Text = "Comment"
        '
        'Label198
        '
        Me.Label198.AutoSize = True
        Me.Label198.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label198.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label198.Location = New System.Drawing.Point(203, 156)
        Me.Label198.Name = "Label198"
        Me.Label198.Size = New System.Drawing.Size(18, 24)
        Me.Label198.TabIndex = 207
        Me.Label198.Text = "*"
        '
        'Label195
        '
        Me.Label195.AutoSize = True
        Me.Label195.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label195.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label195.Location = New System.Drawing.Point(203, 122)
        Me.Label195.Name = "Label195"
        Me.Label195.Size = New System.Drawing.Size(18, 24)
        Me.Label195.TabIndex = 204
        Me.Label195.Text = "*"
        Me.Label195.Visible = False
        '
        'Label197
        '
        Me.Label197.AutoSize = True
        Me.Label197.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label197.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label197.Location = New System.Drawing.Point(203, 258)
        Me.Label197.Name = "Label197"
        Me.Label197.Size = New System.Drawing.Size(18, 24)
        Me.Label197.TabIndex = 206
        Me.Label197.Text = "*"
        '
        'Label196
        '
        Me.Label196.AutoSize = True
        Me.Label196.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label196.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label196.Location = New System.Drawing.Point(203, 224)
        Me.Label196.Name = "Label196"
        Me.Label196.Size = New System.Drawing.Size(18, 24)
        Me.Label196.TabIndex = 205
        Me.Label196.Text = "*"
        '
        'SelfFilingForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(895, 449)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.LabelTitle)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelfFilingForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents LabelTitle As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtreason As TextBox
    Friend WithEvents txtcomments As TextBox
    Friend WithEvents btnApply As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label38 As Label
    Friend WithEvents dtpendtime As DateTimePicker
    Friend WithEvents Label37 As Label
    Friend WithEvents dtpstarttime As DateTimePicker
    Friend WithEvents Label36 As Label
    Friend WithEvents dtpenddate As DateTimePicker
    Friend WithEvents Label35 As Label
    Friend WithEvents dtpstartdate As DateTimePicker
    Friend WithEvents Label34 As Label
    Friend WithEvents cboleavetypes As ComboBox
    Friend WithEvents Label33 As Label
    Friend WithEvents Label199 As Label
    Friend WithEvents Label32 As Label
    Friend WithEvents Label198 As Label
    Friend WithEvents Label195 As Label
    Friend WithEvents Label197 As Label
    Friend WithEvents Label196 As Label
    Friend WithEvents TxtEmployeeID As TextBox
    Friend WithEvents TxtEmployeeFullName As SergeUtils.EasyCompletionComboBox
End Class
