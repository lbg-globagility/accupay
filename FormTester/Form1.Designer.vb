<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.PrintPayslipButton = New System.Windows.Forms.Button()
        Me.EmployeesTextBox = New System.Windows.Forms.TextBox()
        Me.PDFPayslipButton = New System.Windows.Forms.Button()
        Me.ViewPayslipFromLibraryButton = New System.Windows.Forms.Button()
        Me.FileNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PasswordTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.BodyTextBox = New System.Windows.Forms.TextBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SendToTextBox = New System.Windows.Forms.TextBox()
        Me.SubjectTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.AttachmentTextBox = New System.Windows.Forms.TextBox()
        Me.SendEmailButton = New System.Windows.Forms.Button()
        Me.EmailPayslipButton = New System.Windows.Forms.Button()
        Me.OfficialPayslipButton = New System.Windows.Forms.Button()
        Me.QueryPaystubButton = New System.Windows.Forms.Button()
        Me.tableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PrintPayslipButton
        '
        Me.PrintPayslipButton.Location = New System.Drawing.Point(55, 112)
        Me.PrintPayslipButton.Name = "PrintPayslipButton"
        Me.PrintPayslipButton.Size = New System.Drawing.Size(75, 23)
        Me.PrintPayslipButton.TabIndex = 0
        Me.PrintPayslipButton.Text = "Print Payslip"
        Me.PrintPayslipButton.UseVisualStyleBackColor = True
        '
        'EmployeesTextBox
        '
        Me.EmployeesTextBox.Location = New System.Drawing.Point(55, 86)
        Me.EmployeesTextBox.Name = "EmployeesTextBox"
        Me.EmployeesTextBox.Size = New System.Drawing.Size(359, 20)
        Me.EmployeesTextBox.TabIndex = 1
        '
        'PDFPayslipButton
        '
        Me.PDFPayslipButton.Location = New System.Drawing.Point(141, 112)
        Me.PDFPayslipButton.Name = "PDFPayslipButton"
        Me.PDFPayslipButton.Size = New System.Drawing.Size(99, 23)
        Me.PDFPayslipButton.TabIndex = 2
        Me.PDFPayslipButton.Text = "View Payslip PDF"
        Me.PDFPayslipButton.UseVisualStyleBackColor = True
        '
        'ViewPayslipFromLibraryButton
        '
        Me.ViewPayslipFromLibraryButton.Location = New System.Drawing.Point(251, 112)
        Me.ViewPayslipFromLibraryButton.Name = "ViewPayslipFromLibraryButton"
        Me.ViewPayslipFromLibraryButton.Size = New System.Drawing.Size(163, 23)
        Me.ViewPayslipFromLibraryButton.TabIndex = 3
        Me.ViewPayslipFromLibraryButton.Text = "View Payslip PDF (from library)"
        Me.ViewPayslipFromLibraryButton.UseVisualStyleBackColor = True
        '
        'FileNameTextBox
        '
        Me.FileNameTextBox.Location = New System.Drawing.Point(187, 35)
        Me.FileNameTextBox.Name = "FileNameTextBox"
        Me.FileNameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.FileNameTextBox.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(52, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(129, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "PDF filename (from library)"
        '
        'PasswordTextBox
        '
        Me.PasswordTextBox.Location = New System.Drawing.Point(187, 60)
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.Size = New System.Drawing.Size(100, 20)
        Me.PasswordTextBox.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(129, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "password"
        '
        'tableLayoutPanel1
        '
        Me.tableLayoutPanel1.ColumnCount = 2
        Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tableLayoutPanel1.Controls.Add(Me.BodyTextBox, 1, 2)
        Me.tableLayoutPanel1.Controls.Add(Me.label3, 0, 2)
        Me.tableLayoutPanel1.Controls.Add(Me.Label4, 0, 0)
        Me.tableLayoutPanel1.Controls.Add(Me.Label5, 0, 1)
        Me.tableLayoutPanel1.Controls.Add(Me.SendToTextBox, 1, 0)
        Me.tableLayoutPanel1.Controls.Add(Me.SubjectTextBox, 1, 1)
        Me.tableLayoutPanel1.Controls.Add(Me.Label6, 0, 3)
        Me.tableLayoutPanel1.Controls.Add(Me.AttachmentTextBox, 1, 3)
        Me.tableLayoutPanel1.Controls.Add(Me.SendEmailButton, 1, 4)
        Me.tableLayoutPanel1.Location = New System.Drawing.Point(55, 216)
        Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
        Me.tableLayoutPanel1.RowCount = 5
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tableLayoutPanel1.Size = New System.Drawing.Size(351, 138)
        Me.tableLayoutPanel1.TabIndex = 8
        '
        'BodyTextBox
        '
        Me.BodyTextBox.Location = New System.Drawing.Point(178, 55)
        Me.BodyTextBox.Name = "BodyTextBox"
        Me.BodyTextBox.Size = New System.Drawing.Size(170, 20)
        Me.BodyTextBox.TabIndex = 5
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(3, 52)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(34, 13)
        Me.label3.TabIndex = 4
        Me.label3.Text = "Body:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(23, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "To:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 26)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Subject:"
        '
        'SendToTextBox
        '
        Me.SendToTextBox.Location = New System.Drawing.Point(178, 3)
        Me.SendToTextBox.Name = "SendToTextBox"
        Me.SendToTextBox.Size = New System.Drawing.Size(170, 20)
        Me.SendToTextBox.TabIndex = 2
        Me.SendToTextBox.Text = "jsantos.globagility@gmail.com"
        '
        'SubjectTextBox
        '
        Me.SubjectTextBox.Location = New System.Drawing.Point(178, 29)
        Me.SubjectTextBox.Name = "SubjectTextBox"
        Me.SubjectTextBox.Size = New System.Drawing.Size(170, 20)
        Me.SubjectTextBox.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 78)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(107, 13)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Attachment (filepath):"
        '
        'AttachmentTextBox
        '
        Me.AttachmentTextBox.Location = New System.Drawing.Point(178, 81)
        Me.AttachmentTextBox.Name = "AttachmentTextBox"
        Me.AttachmentTextBox.Size = New System.Drawing.Size(170, 20)
        Me.AttachmentTextBox.TabIndex = 8
        '
        'SendEmailButton
        '
        Me.SendEmailButton.Location = New System.Drawing.Point(178, 107)
        Me.SendEmailButton.Name = "SendEmailButton"
        Me.SendEmailButton.Size = New System.Drawing.Size(75, 23)
        Me.SendEmailButton.TabIndex = 9
        Me.SendEmailButton.Text = "Send Email"
        Me.SendEmailButton.UseVisualStyleBackColor = True
        '
        'EmailPayslipButton
        '
        Me.EmailPayslipButton.Location = New System.Drawing.Point(150, 141)
        Me.EmailPayslipButton.Name = "EmailPayslipButton"
        Me.EmailPayslipButton.Size = New System.Drawing.Size(158, 23)
        Me.EmailPayslipButton.TabIndex = 9
        Me.EmailPayslipButton.Text = "Send Payslip Through Email"
        Me.EmailPayslipButton.UseVisualStyleBackColor = True
        '
        'OfficialPayslipButton
        '
        Me.OfficialPayslipButton.Location = New System.Drawing.Point(150, 171)
        Me.OfficialPayslipButton.Name = "OfficialPayslipButton"
        Me.OfficialPayslipButton.Size = New System.Drawing.Size(158, 23)
        Me.OfficialPayslipButton.TabIndex = 10
        Me.OfficialPayslipButton.Text = "Send Official Payslip"
        Me.OfficialPayslipButton.UseVisualStyleBackColor = True
        '
        'QueryPaystubButton
        '
        Me.QueryPaystubButton.Location = New System.Drawing.Point(61, 394)
        Me.QueryPaystubButton.Name = "QueryPaystubButton"
        Me.QueryPaystubButton.Size = New System.Drawing.Size(120, 23)
        Me.QueryPaystubButton.TabIndex = 10
        Me.QueryPaystubButton.Text = "Query Paystub Email"
        Me.QueryPaystubButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(485, 450)
        Me.Controls.Add(Me.QueryPaystubButton)
        Me.Controls.Add(Me.OfficialPayslipButton)
        Me.Controls.Add(Me.EmailPayslipButton)
        Me.Controls.Add(Me.tableLayoutPanel1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.FileNameTextBox)
        Me.Controls.Add(Me.ViewPayslipFromLibraryButton)
        Me.Controls.Add(Me.PDFPayslipButton)
        Me.Controls.Add(Me.EmployeesTextBox)
        Me.Controls.Add(Me.PrintPayslipButton)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.tableLayoutPanel1.ResumeLayout(False)
        Me.tableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PrintPayslipButton As Button
    Friend WithEvents EmployeesTextBox As TextBox
    Friend WithEvents PDFPayslipButton As Button
    Friend WithEvents ViewPayslipFromLibraryButton As Button
    Friend WithEvents FileNameTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PasswordTextBox As TextBox
    Friend WithEvents Label2 As Label
    Private WithEvents tableLayoutPanel1 As TableLayoutPanel
    Private WithEvents BodyTextBox As TextBox
    Private WithEvents label3 As Label
    Private WithEvents Label4 As Label
    Private WithEvents Label5 As Label
    Private WithEvents SendToTextBox As TextBox
    Private WithEvents SubjectTextBox As TextBox
    Private WithEvents Label6 As Label
    Private WithEvents AttachmentTextBox As TextBox
    Private WithEvents SendEmailButton As Button
    Friend WithEvents EmailPayslipButton As Button
    Friend WithEvents OfficialPayslipButton As Button
    Private WithEvents QueryPaystubButton As Button
End Class
