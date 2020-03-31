<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CalendarMonthControl
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
        Me.DaysTableLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.MonthLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'DaysTableLayout
        '
        Me.DaysTableLayout.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DaysTableLayout.AutoSize = True
        Me.DaysTableLayout.ColumnCount = 7
        Me.DaysTableLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.DaysTableLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.DaysTableLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.DaysTableLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.DaysTableLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.DaysTableLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.DaysTableLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.DaysTableLayout.Location = New System.Drawing.Point(0, 32)
        Me.DaysTableLayout.Margin = New System.Windows.Forms.Padding(0)
        Me.DaysTableLayout.Name = "DaysTableLayout"
        Me.DaysTableLayout.RowCount = 1
        Me.DaysTableLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.DaysTableLayout.Size = New System.Drawing.Size(259, 116)
        Me.DaysTableLayout.TabIndex = 0
        '
        'MonthLabel
        '
        Me.MonthLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.MonthLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MonthLabel.Location = New System.Drawing.Point(0, 0)
        Me.MonthLabel.Name = "MonthLabel"
        Me.MonthLabel.Size = New System.Drawing.Size(258, 32)
        Me.MonthLabel.TabIndex = 1
        Me.MonthLabel.Text = "<Month>"
        Me.MonthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CalendarMonthControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.Controls.Add(Me.MonthLabel)
        Me.Controls.Add(Me.DaysTableLayout)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "CalendarMonthControl"
        Me.Size = New System.Drawing.Size(258, 150)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DaysTableLayout As TableLayoutPanel
    Friend WithEvents MonthLabel As Label
End Class
