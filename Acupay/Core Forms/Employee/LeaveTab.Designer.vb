Namespace Global.AccuPay.Employee

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class LeaveTab
        Inherits System.Windows.Forms.UserControl

        'UserControl overrides dispose to clean up the component list.
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
            Me.ToolStrip5 = New System.Windows.Forms.ToolStrip()
            Me.tsbtnNewLeave = New System.Windows.Forms.ToolStripButton()
            Me.tsbtnSaveLeave = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripLabel10 = New System.Windows.Forms.ToolStripLabel()
            Me.tsbtnDeletLeave = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripButton17 = New System.Windows.Forms.ToolStripButton()
            Me.pbEmpPicLeave = New System.Windows.Forms.PictureBox()
            Me.txtFNameLeave = New System.Windows.Forms.TextBox()
            Me.txtEmpIDLeave = New System.Windows.Forms.TextBox()
            Me.Label345 = New System.Windows.Forms.Label()
            Me.Label346 = New System.Windows.Forms.Label()
            Me.cboleavestatus = New System.Windows.Forms.ComboBox()
            Me.dtpendate = New System.Windows.Forms.DateTimePicker()
            Me.dtpstartdate = New System.Windows.Forms.DateTimePicker()
            Me.cboleavetypes = New System.Windows.Forms.ComboBox()
            Me.Label199 = New System.Windows.Forms.Label()
            Me.Label198 = New System.Windows.Forms.Label()
            Me.Label197 = New System.Windows.Forms.Label()
            Me.Label196 = New System.Windows.Forms.Label()
            Me.Label195 = New System.Windows.Forms.Label()
            Me.Label34 = New System.Windows.Forms.Label()
            Me.Label35 = New System.Windows.Forms.Label()
            Me.Label36 = New System.Windows.Forms.Label()
            Me.Label37 = New System.Windows.Forms.Label()
            Me.txtstarttime = New System.Windows.Forms.TextBox()
            Me.Label38 = New System.Windows.Forms.Label()
            Me.txtendtime = New System.Windows.Forms.TextBox()
            Me.Label32 = New System.Windows.Forms.Label()
            Me.Label33 = New System.Windows.Forms.Label()
            Me.txtcomments = New System.Windows.Forms.TextBox()
            Me.txtreason = New System.Windows.Forms.TextBox()
            Me.ToolStrip5.SuspendLayout()
            CType(Me.pbEmpPicLeave, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ToolStrip5
            '
            Me.ToolStrip5.BackColor = System.Drawing.Color.Transparent
            Me.ToolStrip5.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.ToolStrip5.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewLeave, Me.tsbtnSaveLeave, Me.ToolStripButton3, Me.ToolStripLabel10, Me.tsbtnDeletLeave, Me.ToolStripButton4, Me.ToolStripButton17})
            Me.ToolStrip5.Location = New System.Drawing.Point(0, 0)
            Me.ToolStrip5.Name = "ToolStrip5"
            Me.ToolStrip5.Size = New System.Drawing.Size(856, 25)
            Me.ToolStrip5.TabIndex = 1
            Me.ToolStrip5.Text = "ToolStrip5"
            '
            'tsbtnNewLeave
            '
            Me.tsbtnNewLeave.Image = Global.Acupay.My.Resources.Resources._new
            Me.tsbtnNewLeave.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsbtnNewLeave.Name = "tsbtnNewLeave"
            Me.tsbtnNewLeave.Size = New System.Drawing.Size(84, 22)
            Me.tsbtnNewLeave.Text = "&New Leave"
            '
            'tsbtnSaveLeave
            '
            Me.tsbtnSaveLeave.Image = Global.Acupay.My.Resources.Resources.Save
            Me.tsbtnSaveLeave.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsbtnSaveLeave.Name = "tsbtnSaveLeave"
            Me.tsbtnSaveLeave.Size = New System.Drawing.Size(84, 22)
            Me.tsbtnSaveLeave.Text = "&Save Leave"
            '
            'ToolStripButton3
            '
            Me.ToolStripButton3.Image = Global.Acupay.My.Resources.Resources.cancel1
            Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ToolStripButton3.Name = "ToolStripButton3"
            Me.ToolStripButton3.Size = New System.Drawing.Size(63, 22)
            Me.ToolStripButton3.Text = "Cancel"
            '
            'ToolStripLabel10
            '
            Me.ToolStripLabel10.AutoSize = False
            Me.ToolStripLabel10.Name = "ToolStripLabel10"
            Me.ToolStripLabel10.Size = New System.Drawing.Size(89, 22)
            '
            'tsbtnDeletLeave
            '
            Me.tsbtnDeletLeave.Image = Global.Acupay.My.Resources.Resources.CLOSE_00
            Me.tsbtnDeletLeave.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsbtnDeletLeave.Name = "tsbtnDeletLeave"
            Me.tsbtnDeletLeave.Size = New System.Drawing.Size(93, 22)
            Me.tsbtnDeletLeave.Text = "Delete Leave"
            '
            'ToolStripButton4
            '
            Me.ToolStripButton4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
            Me.ToolStripButton4.Image = Global.Acupay.My.Resources.Resources.Button_Delete_icon
            Me.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ToolStripButton4.Name = "ToolStripButton4"
            Me.ToolStripButton4.Size = New System.Drawing.Size(56, 22)
            Me.ToolStripButton4.Text = "Close"
            '
            'ToolStripButton17
            '
            Me.ToolStripButton17.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
            Me.ToolStripButton17.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.ToolStripButton17.Image = Global.Acupay.My.Resources.Resources.audit_trail_icon
            Me.ToolStripButton17.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ToolStripButton17.Name = "ToolStripButton17"
            Me.ToolStripButton17.Size = New System.Drawing.Size(23, 22)
            Me.ToolStripButton17.Text = "ToolStripButton1"
            Me.ToolStripButton17.ToolTipText = "Show audit trails"
            '
            'pbEmpPicLeave
            '
            Me.pbEmpPicLeave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.pbEmpPicLeave.Location = New System.Drawing.Point(8, 40)
            Me.pbEmpPicLeave.Name = "pbEmpPicLeave"
            Me.pbEmpPicLeave.Size = New System.Drawing.Size(89, 77)
            Me.pbEmpPicLeave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.pbEmpPicLeave.TabIndex = 181
            Me.pbEmpPicLeave.TabStop = False
            '
            'txtFNameLeave
            '
            Me.txtFNameLeave.BackColor = System.Drawing.Color.White
            Me.txtFNameLeave.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.txtFNameLeave.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
            Me.txtFNameLeave.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
            Me.txtFNameLeave.Location = New System.Drawing.Point(104, 40)
            Me.txtFNameLeave.MaxLength = 250
            Me.txtFNameLeave.Name = "txtFNameLeave"
            Me.txtFNameLeave.ReadOnly = True
            Me.txtFNameLeave.Size = New System.Drawing.Size(520, 28)
            Me.txtFNameLeave.TabIndex = 182
            '
            'txtEmpIDLeave
            '
            Me.txtEmpIDLeave.BackColor = System.Drawing.Color.White
            Me.txtEmpIDLeave.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.txtEmpIDLeave.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
            Me.txtEmpIDLeave.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
            Me.txtEmpIDLeave.Location = New System.Drawing.Point(104, 72)
            Me.txtEmpIDLeave.MaxLength = 50
            Me.txtEmpIDLeave.Name = "txtEmpIDLeave"
            Me.txtEmpIDLeave.ReadOnly = True
            Me.txtEmpIDLeave.Size = New System.Drawing.Size(520, 22)
            Me.txtEmpIDLeave.TabIndex = 183
            '
            'Label345
            '
            Me.Label345.AutoSize = True
            Me.Label345.Location = New System.Drawing.Point(11, 276)
            Me.Label345.Name = "Label345"
            Me.Label345.Size = New System.Drawing.Size(37, 13)
            Me.Label345.TabIndex = 525
            Me.Label345.Text = "Status"
            '
            'Label346
            '
            Me.Label346.AutoSize = True
            Me.Label346.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
            Me.Label346.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.Label346.Location = New System.Drawing.Point(43, 268)
            Me.Label346.Name = "Label346"
            Me.Label346.Size = New System.Drawing.Size(18, 24)
            Me.Label346.TabIndex = 526
            Me.Label346.Text = "*"
            '
            'cboleavestatus
            '
            Me.cboleavestatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
            Me.cboleavestatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
            Me.cboleavestatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboleavestatus.DropDownWidth = 150
            Me.cboleavestatus.FormattingEnabled = True
            Me.cboleavestatus.Location = New System.Drawing.Point(88, 256)
            Me.cboleavestatus.Name = "cboleavestatus"
            Me.cboleavestatus.Size = New System.Drawing.Size(100, 21)
            Me.cboleavestatus.TabIndex = 514
            '
            'dtpendate
            '
            Me.dtpendate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpendate.Location = New System.Drawing.Point(88, 184)
            Me.dtpendate.Name = "dtpendate"
            Me.dtpendate.Size = New System.Drawing.Size(100, 20)
            Me.dtpendate.TabIndex = 511
            '
            'dtpstartdate
            '
            Me.dtpstartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpstartdate.Location = New System.Drawing.Point(88, 160)
            Me.dtpstartdate.Name = "dtpstartdate"
            Me.dtpstartdate.Size = New System.Drawing.Size(100, 20)
            Me.dtpstartdate.TabIndex = 510
            '
            'cboleavetypes
            '
            Me.cboleavetypes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
            Me.cboleavetypes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
            Me.cboleavetypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboleavetypes.DropDownWidth = 150
            Me.cboleavetypes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cboleavetypes.FormattingEnabled = True
            Me.cboleavetypes.Location = New System.Drawing.Point(88, 136)
            Me.cboleavetypes.Name = "cboleavetypes"
            Me.cboleavetypes.Size = New System.Drawing.Size(100, 21)
            Me.cboleavetypes.TabIndex = 509
            '
            'Label199
            '
            Me.Label199.AutoSize = True
            Me.Label199.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
            Me.Label199.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.Label199.Location = New System.Drawing.Point(56, 182)
            Me.Label199.Name = "Label199"
            Me.Label199.Size = New System.Drawing.Size(18, 24)
            Me.Label199.TabIndex = 524
            Me.Label199.Text = "*"
            '
            'Label198
            '
            Me.Label198.AutoSize = True
            Me.Label198.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
            Me.Label198.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.Label198.Location = New System.Drawing.Point(59, 158)
            Me.Label198.Name = "Label198"
            Me.Label198.Size = New System.Drawing.Size(18, 24)
            Me.Label198.TabIndex = 523
            Me.Label198.Text = "*"
            '
            'Label197
            '
            Me.Label197.AutoSize = True
            Me.Label197.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
            Me.Label197.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.Label197.Location = New System.Drawing.Point(54, 241)
            Me.Label197.Name = "Label197"
            Me.Label197.Size = New System.Drawing.Size(18, 24)
            Me.Label197.TabIndex = 522
            Me.Label197.Text = "*"
            '
            'Label196
            '
            Me.Label196.AutoSize = True
            Me.Label196.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
            Me.Label196.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.Label196.Location = New System.Drawing.Point(57, 208)
            Me.Label196.Name = "Label196"
            Me.Label196.Size = New System.Drawing.Size(18, 24)
            Me.Label196.TabIndex = 521
            Me.Label196.Text = "*"
            '
            'Label195
            '
            Me.Label195.AutoSize = True
            Me.Label195.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
            Me.Label195.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
            Me.Label195.Location = New System.Drawing.Point(66, 134)
            Me.Label195.Name = "Label195"
            Me.Label195.Size = New System.Drawing.Size(18, 24)
            Me.Label195.TabIndex = 520
            Me.Label195.Text = "*"
            '
            'Label34
            '
            Me.Label34.AutoSize = True
            Me.Label34.Location = New System.Drawing.Point(11, 188)
            Me.Label34.Name = "Label34"
            Me.Label34.Size = New System.Drawing.Size(50, 13)
            Me.Label34.TabIndex = 518
            Me.Label34.Text = "End date"
            '
            'Label35
            '
            Me.Label35.AutoSize = True
            Me.Label35.Location = New System.Drawing.Point(11, 164)
            Me.Label35.Name = "Label35"
            Me.Label35.Size = New System.Drawing.Size(53, 13)
            Me.Label35.TabIndex = 519
            Me.Label35.Text = "Start date"
            '
            'Label36
            '
            Me.Label36.AutoSize = True
            Me.Label36.Location = New System.Drawing.Point(11, 140)
            Me.Label36.Name = "Label36"
            Me.Label36.Size = New System.Drawing.Size(60, 13)
            Me.Label36.TabIndex = 517
            Me.Label36.Text = "Leave type"
            '
            'Label37
            '
            Me.Label37.AutoSize = True
            Me.Label37.Location = New System.Drawing.Point(11, 249)
            Me.Label37.Name = "Label37"
            Me.Label37.Size = New System.Drawing.Size(48, 13)
            Me.Label37.TabIndex = 516
            Me.Label37.Text = "End time"
            '
            'txtstarttime
            '
            Me.txtstarttime.BackColor = System.Drawing.Color.White
            Me.txtstarttime.Location = New System.Drawing.Point(88, 208)
            Me.txtstarttime.Name = "txtstarttime"
            Me.txtstarttime.Size = New System.Drawing.Size(100, 20)
            Me.txtstarttime.TabIndex = 512
            '
            'Label38
            '
            Me.Label38.AutoSize = True
            Me.Label38.Location = New System.Drawing.Point(11, 216)
            Me.Label38.Name = "Label38"
            Me.Label38.Size = New System.Drawing.Size(51, 13)
            Me.Label38.TabIndex = 515
            Me.Label38.Text = "Start time"
            '
            'txtendtime
            '
            Me.txtendtime.BackColor = System.Drawing.Color.White
            Me.txtendtime.Location = New System.Drawing.Point(88, 232)
            Me.txtendtime.Name = "txtendtime"
            Me.txtendtime.Size = New System.Drawing.Size(100, 20)
            Me.txtendtime.TabIndex = 513
            '
            'Label32
            '
            Me.Label32.AutoSize = True
            Me.Label32.Location = New System.Drawing.Point(200, 200)
            Me.Label32.Name = "Label32"
            Me.Label32.Size = New System.Drawing.Size(51, 13)
            Me.Label32.TabIndex = 529
            Me.Label32.Text = "Comment"
            '
            'Label33
            '
            Me.Label33.Location = New System.Drawing.Point(200, 136)
            Me.Label33.Name = "Label33"
            Me.Label33.Size = New System.Drawing.Size(72, 24)
            Me.Label33.TabIndex = 530
            Me.Label33.Text = "Reason"
            '
            'txtcomments
            '
            Me.txtcomments.BackColor = System.Drawing.Color.White
            Me.txtcomments.Location = New System.Drawing.Point(312, 200)
            Me.txtcomments.MaxLength = 2000
            Me.txtcomments.Multiline = True
            Me.txtcomments.Name = "txtcomments"
            Me.txtcomments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtcomments.Size = New System.Drawing.Size(192, 56)
            Me.txtcomments.TabIndex = 528
            '
            'txtreason
            '
            Me.txtreason.BackColor = System.Drawing.Color.White
            Me.txtreason.Location = New System.Drawing.Point(312, 136)
            Me.txtreason.MaxLength = 500
            Me.txtreason.Multiline = True
            Me.txtreason.Name = "txtreason"
            Me.txtreason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtreason.Size = New System.Drawing.Size(192, 56)
            Me.txtreason.TabIndex = 527
            '
            'LeaveTab
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.Label32)
            Me.Controls.Add(Me.Label33)
            Me.Controls.Add(Me.txtcomments)
            Me.Controls.Add(Me.txtreason)
            Me.Controls.Add(Me.Label345)
            Me.Controls.Add(Me.Label346)
            Me.Controls.Add(Me.cboleavestatus)
            Me.Controls.Add(Me.dtpendate)
            Me.Controls.Add(Me.dtpstartdate)
            Me.Controls.Add(Me.cboleavetypes)
            Me.Controls.Add(Me.Label199)
            Me.Controls.Add(Me.Label198)
            Me.Controls.Add(Me.Label197)
            Me.Controls.Add(Me.Label196)
            Me.Controls.Add(Me.Label195)
            Me.Controls.Add(Me.Label34)
            Me.Controls.Add(Me.Label35)
            Me.Controls.Add(Me.Label36)
            Me.Controls.Add(Me.Label37)
            Me.Controls.Add(Me.txtstarttime)
            Me.Controls.Add(Me.Label38)
            Me.Controls.Add(Me.txtendtime)
            Me.Controls.Add(Me.txtEmpIDLeave)
            Me.Controls.Add(Me.txtFNameLeave)
            Me.Controls.Add(Me.pbEmpPicLeave)
            Me.Controls.Add(Me.ToolStrip5)
            Me.Name = "LeaveTab"
            Me.Size = New System.Drawing.Size(856, 552)
            Me.ToolStrip5.ResumeLayout(False)
            Me.ToolStrip5.PerformLayout()
            CType(Me.pbEmpPicLeave, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Friend WithEvents ToolStrip5 As ToolStrip
        Friend WithEvents tsbtnNewLeave As ToolStripButton
        Friend WithEvents tsbtnSaveLeave As ToolStripButton
        Friend WithEvents ToolStripButton3 As ToolStripButton
        Friend WithEvents ToolStripLabel10 As ToolStripLabel
        Friend WithEvents tsbtnDeletLeave As ToolStripButton
        Friend WithEvents ToolStripButton4 As ToolStripButton
        Friend WithEvents ToolStripButton17 As ToolStripButton
        Friend WithEvents pbEmpPicLeave As PictureBox
        Friend WithEvents txtFNameLeave As TextBox
        Friend WithEvents txtEmpIDLeave As TextBox
        Friend WithEvents Label345 As Label
        Friend WithEvents Label346 As Label
        Friend WithEvents cboleavestatus As ComboBox
        Friend WithEvents dtpendate As DateTimePicker
        Friend WithEvents dtpstartdate As DateTimePicker
        Friend WithEvents cboleavetypes As ComboBox
        Friend WithEvents Label199 As Label
        Friend WithEvents Label198 As Label
        Friend WithEvents Label197 As Label
        Friend WithEvents Label196 As Label
        Friend WithEvents Label195 As Label
        Friend WithEvents Label34 As Label
        Friend WithEvents Label35 As Label
        Friend WithEvents Label36 As Label
        Friend WithEvents Label37 As Label
        Friend WithEvents txtstarttime As TextBox
        Friend WithEvents Label38 As Label
        Friend WithEvents txtendtime As TextBox
        Friend WithEvents Label32 As Label
        Friend WithEvents Label33 As Label
        Friend WithEvents txtcomments As TextBox
        Friend WithEvents txtreason As TextBox
    End Class

End Namespace