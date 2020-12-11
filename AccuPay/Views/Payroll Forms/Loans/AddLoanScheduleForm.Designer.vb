<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddLoanScheduleForm
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
        Me.txtEmployeeFirstName = New System.Windows.Forms.TextBox()
        Me.txtEmployeeNumber = New System.Windows.Forms.TextBox()
        Me.pbEmployeePicture = New System.Windows.Forms.PictureBox()
        Me.btnAddAndNew = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnAddAndClose = New System.Windows.Forms.Button()
        Me.LoanUserControl1 = New LoanUserControl()
        Me.EmployeeInfoTabLayout.SuspendLayout()
        CType(Me.pbEmployeePicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'EmployeeInfoTabLayout
        '
        Me.EmployeeInfoTabLayout.BackColor = System.Drawing.Color.White
        Me.EmployeeInfoTabLayout.ColumnCount = 2
        Me.EmployeeInfoTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121.0!))
        Me.EmployeeInfoTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 861.0!))
        Me.EmployeeInfoTabLayout.Controls.Add(Me.txtEmployeeFirstName, 1, 0)
        Me.EmployeeInfoTabLayout.Controls.Add(Me.txtEmployeeNumber, 1, 1)
        Me.EmployeeInfoTabLayout.Controls.Add(Me.pbEmployeePicture, 0, 0)
        Me.EmployeeInfoTabLayout.Dock = System.Windows.Forms.DockStyle.Top
        Me.EmployeeInfoTabLayout.Location = New System.Drawing.Point(0, 0)
        Me.EmployeeInfoTabLayout.Name = "EmployeeInfoTabLayout"
        Me.EmployeeInfoTabLayout.RowCount = 2
        Me.EmployeeInfoTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.EmployeeInfoTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.EmployeeInfoTabLayout.Size = New System.Drawing.Size(803, 88)
        Me.EmployeeInfoTabLayout.TabIndex = 6
        '
        'txtEmployeeFirstName
        '
        Me.txtEmployeeFirstName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtEmployeeFirstName.BackColor = System.Drawing.Color.White
        Me.txtEmployeeFirstName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeFirstName.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeFirstName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.txtEmployeeFirstName.Location = New System.Drawing.Point(124, 13)
        Me.txtEmployeeFirstName.MaxLength = 250
        Me.txtEmployeeFirstName.Name = "txtEmployeeFirstName"
        Me.txtEmployeeFirstName.ReadOnly = True
        Me.txtEmployeeFirstName.Size = New System.Drawing.Size(668, 28)
        Me.txtEmployeeFirstName.TabIndex = 381
        Me.txtEmployeeFirstName.TabStop = False
        '
        'txtEmployeeNumber
        '
        Me.txtEmployeeNumber.BackColor = System.Drawing.Color.White
        Me.txtEmployeeNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEmployeeNumber.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtEmployeeNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.txtEmployeeNumber.ForeColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(89, Byte), Integer))
        Me.txtEmployeeNumber.Location = New System.Drawing.Point(124, 47)
        Me.txtEmployeeNumber.MaxLength = 50
        Me.txtEmployeeNumber.Multiline = True
        Me.txtEmployeeNumber.Name = "txtEmployeeNumber"
        Me.txtEmployeeNumber.ReadOnly = True
        Me.txtEmployeeNumber.Size = New System.Drawing.Size(855, 38)
        Me.txtEmployeeNumber.TabIndex = 380
        Me.txtEmployeeNumber.TabStop = False
        '
        'pbEmployeePicture
        '
        Me.pbEmployeePicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmployeePicture.Location = New System.Drawing.Point(28, 3)
        Me.pbEmployeePicture.Margin = New System.Windows.Forms.Padding(28, 3, 3, 3)
        Me.pbEmployeePicture.Name = "pbEmployeePicture"
        Me.EmployeeInfoTabLayout.SetRowSpan(Me.pbEmployeePicture, 2)
        Me.pbEmployeePicture.Size = New System.Drawing.Size(88, 82)
        Me.pbEmployeePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmployeePicture.TabIndex = 382
        Me.pbEmployeePicture.TabStop = False
        '
        'btnAddAndNew
        '
        Me.btnAddAndNew.Location = New System.Drawing.Point(369, 372)
        Me.btnAddAndNew.Name = "btnAddAndNew"
        Me.btnAddAndNew.Size = New System.Drawing.Size(75, 23)
        Me.btnAddAndNew.TabIndex = 7
        Me.btnAddAndNew.Text = "Add && &New"
        Me.btnAddAndNew.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(452, 372)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAddAndClose
        '
        Me.btnAddAndClose.Location = New System.Drawing.Point(278, 372)
        Me.btnAddAndClose.Name = "btnAddAndClose"
        Me.btnAddAndClose.Size = New System.Drawing.Size(85, 23)
        Me.btnAddAndClose.TabIndex = 9
        Me.btnAddAndClose.Text = "&Add && Close"
        Me.btnAddAndClose.UseVisualStyleBackColor = True
        '
        'LoanUserControl1
        '
        Me.LoanUserControl1.Location = New System.Drawing.Point(0, 88)
        Me.LoanUserControl1.Name = "LoanUserControl1"
        Me.LoanUserControl1.Size = New System.Drawing.Size(803, 247)
        Me.LoanUserControl1.TabIndex = 10
        '
        'AddLoanScheduleForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(803, 416)
        Me.Controls.Add(Me.LoanUserControl1)
        Me.Controls.Add(Me.btnAddAndClose)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAddAndNew)
        Me.Controls.Add(Me.EmployeeInfoTabLayout)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AddLoanScheduleForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Loan Schedule"
        Me.EmployeeInfoTabLayout.ResumeLayout(False)
        Me.EmployeeInfoTabLayout.PerformLayout()
        CType(Me.pbEmployeePicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents EmployeeInfoTabLayout As TableLayoutPanel
    Friend WithEvents txtEmployeeFirstName As TextBox
    Friend WithEvents txtEmployeeNumber As TextBox
    Friend WithEvents pbEmployeePicture As PictureBox
    Friend WithEvents btnAddAndNew As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnAddAndClose As Button
    Friend WithEvents LoanUserControl1 As LoanUserControl
End Class
