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
        Me.SuspendLayout()
        '
        'PrintPayslipButton
        '
        Me.PrintPayslipButton.Location = New System.Drawing.Point(105, 114)
        Me.PrintPayslipButton.Name = "PrintPayslipButton"
        Me.PrintPayslipButton.Size = New System.Drawing.Size(75, 23)
        Me.PrintPayslipButton.TabIndex = 0
        Me.PrintPayslipButton.Text = "Print Payslip"
        Me.PrintPayslipButton.UseVisualStyleBackColor = True
        '
        'EmployeesTextBox
        '
        Me.EmployeesTextBox.Location = New System.Drawing.Point(105, 88)
        Me.EmployeesTextBox.Name = "EmployeesTextBox"
        Me.EmployeesTextBox.Size = New System.Drawing.Size(253, 20)
        Me.EmployeesTextBox.TabIndex = 1
        '
        'PDFPayslipButton
        '
        Me.PDFPayslipButton.Location = New System.Drawing.Point(196, 114)
        Me.PDFPayslipButton.Name = "PDFPayslipButton"
        Me.PDFPayslipButton.Size = New System.Drawing.Size(99, 23)
        Me.PDFPayslipButton.TabIndex = 2
        Me.PDFPayslipButton.Text = "View Payslip PDF"
        Me.PDFPayslipButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(485, 450)
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
End Class
