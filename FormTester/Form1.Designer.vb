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
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(485, 450)
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
End Class
