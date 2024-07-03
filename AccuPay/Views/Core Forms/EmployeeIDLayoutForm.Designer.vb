<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EmployeeIDLayoutForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EmployeeIDLayoutForm))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.EmployeeSurName = New System.Windows.Forms.Label()
        Me.EmployeeName = New System.Windows.Forms.Label()
        Me.EmpPosition = New System.Windows.Forms.Label()
        Me.EmpExpireDate = New System.Windows.Forms.Label()
        Me.EmpIDNo = New System.Windows.Forms.Label()
        Me.EmpPicture = New System.Windows.Forms.PictureBox()
        Me.EmpBirthdate = New System.Windows.Forms.Label()
        Me.EmpAddress = New System.Windows.Forms.Label()
        Me.EmpContactNo = New System.Windows.Forms.Label()
        Me.EmpSSS = New System.Windows.Forms.Label()
        Me.EmpPhilhealth = New System.Windows.Forms.Label()
        Me.EmpHDMF = New System.Windows.Forms.Label()
        Me.EmpTIN = New System.Windows.Forms.Label()
        Me.EmpEmergencyName = New System.Windows.Forms.Label()
        Me.EmpEmergencyRelation = New System.Windows.Forms.Label()
        Me.EmpEmergencyNo = New System.Windows.Forms.Label()
        Me.PrintBtn = New System.Windows.Forms.Button()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.Panel1 = New System.Windows.Forms.Panel()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmpPicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.GridColor = System.Drawing.SystemColors.Control
        Me.DataGridView1.Location = New System.Drawing.Point(0, 43)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(501, 433)
        Me.DataGridView1.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Image = Global.AccuPay.My.Resources.Resources.TraineeID_F
        Me.PictureBox1.Location = New System.Drawing.Point(35, 83)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(206, 327)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox2.Image = Global.AccuPay.My.Resources.Resources.TraineeID
        Me.PictureBox2.Location = New System.Drawing.Point(263, 83)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(204, 325)
        Me.PictureBox2.TabIndex = 2
        Me.PictureBox2.TabStop = False
        '
        'EmployeeSurName
        '
        Me.EmployeeSurName.AutoSize = True
        Me.EmployeeSurName.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.EmployeeSurName.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmployeeSurName.Location = New System.Drawing.Point(45, 318)
        Me.EmployeeSurName.Name = "EmployeeSurName"
        Me.EmployeeSurName.Size = New System.Drawing.Size(104, 25)
        Me.EmployeeSurName.TabIndex = 3
        Me.EmployeeSurName.Text = "Last Name"
        '
        'EmployeeName
        '
        Me.EmployeeName.AutoSize = True
        Me.EmployeeName.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.EmployeeName.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmployeeName.Location = New System.Drawing.Point(45, 339)
        Me.EmployeeName.Name = "EmployeeName"
        Me.EmployeeName.Size = New System.Drawing.Size(141, 25)
        Me.EmployeeName.TabIndex = 4
        Me.EmployeeName.Text = "First Name & MI"
        '
        'EmpPosition
        '
        Me.EmpPosition.AutoSize = True
        Me.EmpPosition.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.EmpPosition.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpPosition.Location = New System.Drawing.Point(47, 360)
        Me.EmpPosition.Name = "EmpPosition"
        Me.EmpPosition.Size = New System.Drawing.Size(51, 17)
        Me.EmpPosition.TabIndex = 5
        Me.EmpPosition.Text = "Position"
        '
        'EmpExpireDate
        '
        Me.EmpExpireDate.AutoSize = True
        Me.EmpExpireDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer))
        Me.EmpExpireDate.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpExpireDate.ForeColor = System.Drawing.Color.White
        Me.EmpExpireDate.Location = New System.Drawing.Point(45, 393)
        Me.EmpExpireDate.Name = "EmpExpireDate"
        Me.EmpExpireDate.Size = New System.Drawing.Size(87, 13)
        Me.EmpExpireDate.TabIndex = 6
        Me.EmpExpireDate.Text = "Expiration Date"
        '
        'EmpIDNo
        '
        Me.EmpIDNo.AutoSize = True
        Me.EmpIDNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(55, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.EmpIDNo.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpIDNo.ForeColor = System.Drawing.Color.White
        Me.EmpIDNo.Location = New System.Drawing.Point(185, 393)
        Me.EmpIDNo.Name = "EmpIDNo"
        Me.EmpIDNo.Size = New System.Drawing.Size(40, 13)
        Me.EmpIDNo.TabIndex = 7
        Me.EmpIDNo.Text = "ID No."
        '
        'EmpPicture
        '
        Me.EmpPicture.Location = New System.Drawing.Point(78, 181)
        Me.EmpPicture.Name = "EmpPicture"
        Me.EmpPicture.Size = New System.Drawing.Size(122, 122)
        Me.EmpPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EmpPicture.TabIndex = 8
        Me.EmpPicture.TabStop = False
        '
        'EmpBirthdate
        '
        Me.EmpBirthdate.AutoSize = True
        Me.EmpBirthdate.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.EmpBirthdate.Font = New System.Drawing.Font("Segoe UI Semibold", 6.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpBirthdate.Location = New System.Drawing.Point(366, 147)
        Me.EmpBirthdate.Name = "EmpBirthdate"
        Me.EmpBirthdate.Size = New System.Drawing.Size(28, 11)
        Me.EmpBirthdate.TabIndex = 9
        Me.EmpBirthdate.Text = "Label1"
        '
        'EmpAddress
        '
        Me.EmpAddress.AutoSize = True
        Me.EmpAddress.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.EmpAddress.Font = New System.Drawing.Font("Segoe UI Semibold", 4.6!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpAddress.Location = New System.Drawing.Point(352, 158)
        Me.EmpAddress.Margin = New System.Windows.Forms.Padding(0)
        Me.EmpAddress.MaximumSize = New System.Drawing.Size(116, 25)
        Me.EmpAddress.Name = "EmpAddress"
        Me.EmpAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.EmpAddress.Size = New System.Drawing.Size(31, 10)
        Me.EmpAddress.TabIndex = 10
        Me.EmpAddress.Text = "Address"
        '
        'EmpContactNo
        '
        Me.EmpContactNo.AutoSize = True
        Me.EmpContactNo.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.EmpContactNo.Font = New System.Drawing.Font("Segoe UI Semibold", 6.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpContactNo.Location = New System.Drawing.Point(365, 179)
        Me.EmpContactNo.Name = "EmpContactNo"
        Me.EmpContactNo.Size = New System.Drawing.Size(28, 11)
        Me.EmpContactNo.TabIndex = 11
        Me.EmpContactNo.Text = "Label1"
        '
        'EmpSSS
        '
        Me.EmpSSS.AutoSize = True
        Me.EmpSSS.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.EmpSSS.Font = New System.Drawing.Font("Segoe UI Semibold", 6.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpSSS.Location = New System.Drawing.Point(366, 199)
        Me.EmpSSS.Name = "EmpSSS"
        Me.EmpSSS.Size = New System.Drawing.Size(28, 11)
        Me.EmpSSS.TabIndex = 12
        Me.EmpSSS.Text = "Label1"
        '
        'EmpPhilhealth
        '
        Me.EmpPhilhealth.AutoSize = True
        Me.EmpPhilhealth.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.EmpPhilhealth.Font = New System.Drawing.Font("Segoe UI Semibold", 6.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpPhilhealth.Location = New System.Drawing.Point(366, 211)
        Me.EmpPhilhealth.Name = "EmpPhilhealth"
        Me.EmpPhilhealth.Size = New System.Drawing.Size(28, 11)
        Me.EmpPhilhealth.TabIndex = 13
        Me.EmpPhilhealth.Text = "Label1"
        '
        'EmpHDMF
        '
        Me.EmpHDMF.AutoSize = True
        Me.EmpHDMF.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.EmpHDMF.Font = New System.Drawing.Font("Segoe UI Semibold", 6.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpHDMF.Location = New System.Drawing.Point(366, 225)
        Me.EmpHDMF.Name = "EmpHDMF"
        Me.EmpHDMF.Size = New System.Drawing.Size(28, 11)
        Me.EmpHDMF.TabIndex = 14
        Me.EmpHDMF.Text = "Label1"
        '
        'EmpTIN
        '
        Me.EmpTIN.AutoSize = True
        Me.EmpTIN.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.EmpTIN.Font = New System.Drawing.Font("Segoe UI Semibold", 6.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpTIN.Location = New System.Drawing.Point(366, 239)
        Me.EmpTIN.Name = "EmpTIN"
        Me.EmpTIN.Size = New System.Drawing.Size(28, 11)
        Me.EmpTIN.TabIndex = 15
        Me.EmpTIN.Text = "Label1"
        '
        'EmpEmergencyName
        '
        Me.EmpEmergencyName.AutoSize = True
        Me.EmpEmergencyName.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.EmpEmergencyName.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpEmergencyName.Location = New System.Drawing.Point(347, 265)
        Me.EmpEmergencyName.Name = "EmpEmergencyName"
        Me.EmpEmergencyName.Size = New System.Drawing.Size(49, 19)
        Me.EmpEmergencyName.TabIndex = 16
        Me.EmpEmergencyName.Text = "Label1"
        Me.EmpEmergencyName.Visible = False
        '
        'EmpEmergencyRelation
        '
        Me.EmpEmergencyRelation.AutoSize = True
        Me.EmpEmergencyRelation.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.EmpEmergencyRelation.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpEmergencyRelation.Location = New System.Drawing.Point(347, 283)
        Me.EmpEmergencyRelation.Name = "EmpEmergencyRelation"
        Me.EmpEmergencyRelation.Size = New System.Drawing.Size(49, 19)
        Me.EmpEmergencyRelation.TabIndex = 17
        Me.EmpEmergencyRelation.Text = "Label1"
        Me.EmpEmergencyRelation.Visible = False
        '
        'EmpEmergencyNo
        '
        Me.EmpEmergencyNo.AutoSize = True
        Me.EmpEmergencyNo.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.EmpEmergencyNo.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmpEmergencyNo.Location = New System.Drawing.Point(347, 300)
        Me.EmpEmergencyNo.Name = "EmpEmergencyNo"
        Me.EmpEmergencyNo.Size = New System.Drawing.Size(49, 19)
        Me.EmpEmergencyNo.TabIndex = 18
        Me.EmpEmergencyNo.Text = "Label1"
        Me.EmpEmergencyNo.Visible = False
        '
        'PrintBtn
        '
        Me.PrintBtn.Image = CType(resources.GetObject("PrintBtn.Image"), System.Drawing.Image)
        Me.PrintBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.PrintBtn.Location = New System.Drawing.Point(409, 9)
        Me.PrintBtn.Name = "PrintBtn"
        Me.PrintBtn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PrintBtn.Size = New System.Drawing.Size(87, 23)
        Me.PrintBtn.TabIndex = 19
        Me.PrintBtn.Text = "Print"
        Me.PrintBtn.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PrintBtn)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(501, 40)
        Me.Panel1.TabIndex = 20
        '
        'EmployeeIDLayoutForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(501, 479)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.EmpEmergencyNo)
        Me.Controls.Add(Me.EmpEmergencyRelation)
        Me.Controls.Add(Me.EmpEmergencyName)
        Me.Controls.Add(Me.EmpTIN)
        Me.Controls.Add(Me.EmpHDMF)
        Me.Controls.Add(Me.EmpPhilhealth)
        Me.Controls.Add(Me.EmpSSS)
        Me.Controls.Add(Me.EmpContactNo)
        Me.Controls.Add(Me.EmpAddress)
        Me.Controls.Add(Me.EmpBirthdate)
        Me.Controls.Add(Me.EmpPicture)
        Me.Controls.Add(Me.EmpIDNo)
        Me.Controls.Add(Me.EmpExpireDate)
        Me.Controls.Add(Me.EmpPosition)
        Me.Controls.Add(Me.EmployeeName)
        Me.Controls.Add(Me.EmployeeSurName)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "EmployeeIDLayoutForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee ID Layout"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmpPicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents EmployeeSurName As Label
    Friend WithEvents EmployeeName As Label
    Friend WithEvents EmpPosition As Label
    Friend WithEvents EmpExpireDate As Label
    Friend WithEvents EmpIDNo As Label
    Friend WithEvents EmpPicture As PictureBox
    Friend WithEvents EmpBirthdate As Label
    Friend WithEvents EmpAddress As Label
    Friend WithEvents EmpContactNo As Label
    Friend WithEvents EmpSSS As Label
    Friend WithEvents EmpPhilhealth As Label
    Friend WithEvents EmpHDMF As Label
    Friend WithEvents EmpTIN As Label
    Friend WithEvents EmpEmergencyName As Label
    Friend WithEvents EmpEmergencyRelation As Label
    Friend WithEvents EmpEmergencyNo As Label
    Friend WithEvents PrintBtn As Button
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents Panel1 As Panel
End Class
