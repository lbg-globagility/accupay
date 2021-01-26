<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PositionUserControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
        Me.JobLevelComboBox = New System.Windows.Forms.ComboBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.DivisionComboBox = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.JobLevelLabel = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PositionNameTextBox = New System.Windows.Forms.TextBox()
        Me.lblTotalLoanAmount = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel5.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.ColumnCount = 1
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.JobLevelComboBox, 0, 5)
        Me.TableLayoutPanel5.Controls.Add(Me.Panel3, 0, 1)
        Me.TableLayoutPanel5.Controls.Add(Me.JobLevelLabel, 0, 4)
        Me.TableLayoutPanel5.Controls.Add(Me.Panel2, 0, 3)
        Me.TableLayoutPanel5.Controls.Add(Me.lblTotalLoanAmount, 0, 2)
        Me.TableLayoutPanel5.Controls.Add(Me.Label4, 0, 0)
        Me.TableLayoutPanel5.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 6
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(230, 148)
        Me.TableLayoutPanel5.TabIndex = 333
        '
        'JobLevelComboBox
        '
        Me.JobLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.JobLevelComboBox.FormattingEnabled = True
        Me.JobLevelComboBox.Location = New System.Drawing.Point(20, 115)
        Me.JobLevelComboBox.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.JobLevelComboBox.Name = "JobLevelComboBox"
        Me.JobLevelComboBox.Size = New System.Drawing.Size(195, 21)
        Me.JobLevelComboBox.TabIndex = 387
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.DivisionComboBox)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 16)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(230, 32)
        Me.Panel3.TabIndex = 384
        '
        'DivisionComboBox
        '
        Me.DivisionComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DivisionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DivisionComboBox.FormattingEnabled = True
        Me.DivisionComboBox.Location = New System.Drawing.Point(20, 2)
        Me.DivisionComboBox.Name = "DivisionComboBox"
        Me.DivisionComboBox.Size = New System.Drawing.Size(195, 21)
        Me.DivisionComboBox.TabIndex = 353
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(3, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(13, 13)
        Me.Label5.TabIndex = 507
        Me.Label5.Text = "*"
        '
        'JobLevelLabel
        '
        Me.JobLevelLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.JobLevelLabel.AutoSize = True
        Me.JobLevelLabel.Location = New System.Drawing.Point(20, 99)
        Me.JobLevelLabel.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.JobLevelLabel.Name = "JobLevelLabel"
        Me.JobLevelLabel.Size = New System.Drawing.Size(53, 13)
        Me.JobLevelLabel.TabIndex = 380
        Me.JobLevelLabel.Text = "Job Level"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.PositionNameTextBox)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 64)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(230, 32)
        Me.Panel2.TabIndex = 385
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(3, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 507
        Me.Label3.Text = "*"
        '
        'PositionNameTextBox
        '
        Me.PositionNameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PositionNameTextBox.Location = New System.Drawing.Point(20, 3)
        Me.PositionNameTextBox.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.PositionNameTextBox.Name = "PositionNameTextBox"
        Me.PositionNameTextBox.Size = New System.Drawing.Size(195, 22)
        Me.PositionNameTextBox.TabIndex = 354
        '
        'lblTotalLoanAmount
        '
        Me.lblTotalLoanAmount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalLoanAmount.AutoSize = True
        Me.lblTotalLoanAmount.Location = New System.Drawing.Point(20, 51)
        Me.lblTotalLoanAmount.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.lblTotalLoanAmount.Name = "lblTotalLoanAmount"
        Me.lblTotalLoanAmount.Size = New System.Drawing.Size(81, 13)
        Me.lblTotalLoanAmount.TabIndex = 362
        Me.lblTotalLoanAmount.Text = "Position Name"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 3)
        Me.Label4.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 13)
        Me.Label4.TabIndex = 386
        Me.Label4.Text = "Division"
        '
        'PositionUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TableLayoutPanel5)
        Me.Name = "PositionUserControl"
        Me.Size = New System.Drawing.Size(230, 148)
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.TableLayoutPanel5.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel5 As TableLayoutPanel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents DivisionComboBox As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents JobLevelLabel As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents PositionNameTextBox As TextBox
    Friend WithEvents lblTotalLoanAmount As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents JobLevelComboBox As ComboBox
End Class
