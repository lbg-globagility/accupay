<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmailDashboardForm
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
        Me.NumberOfQueueLabel = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CurrentProcessingLabel = New System.Windows.Forms.Label()
        Me.NextOnQueueLabel = New System.Windows.Forms.Label()
        Me.StatusLabel = New System.Windows.Forms.Label()
        Me.DashboardPanel = New System.Windows.Forms.Panel()
        Me.RestartEmailServiceLinkLabel = New System.Windows.Forms.LinkLabel()
        Me.LoadingPictureBox = New System.Windows.Forms.PictureBox()
        Me.RefreshTimer = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ProcessingDateLabel = New System.Windows.Forms.Label()
        Me.DashboardPanel.SuspendLayout()
        CType(Me.LoadingPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NumberOfQueueLabel
        '
        Me.NumberOfQueueLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NumberOfQueueLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NumberOfQueueLabel.Location = New System.Drawing.Point(0, 40)
        Me.NumberOfQueueLabel.Name = "NumberOfQueueLabel"
        Me.NumberOfQueueLabel.Size = New System.Drawing.Size(490, 21)
        Me.NumberOfQueueLabel.TabIndex = 0
        Me.NumberOfQueueLabel.Text = "4 emails on queue."
        Me.NumberOfQueueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(30, 101)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(157, 21)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Current Processing:"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(30, 228)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(127, 21)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Next on queue:"
        '
        'CurrentProcessingLabel
        '
        Me.CurrentProcessingLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CurrentProcessingLabel.AutoSize = True
        Me.CurrentProcessingLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentProcessingLabel.Location = New System.Drawing.Point(30, 122)
        Me.CurrentProcessingLabel.Name = "CurrentProcessingLabel"
        Me.CurrentProcessingLabel.Size = New System.Drawing.Size(236, 21)
        Me.CurrentProcessingLabel.TabIndex = 3
        Me.CurrentProcessingLabel.Text = "Vincent Cruz (vcruz@gmail.com)"
        '
        'NextOnQueueLabel
        '
        Me.NextOnQueueLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NextOnQueueLabel.AutoSize = True
        Me.NextOnQueueLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NextOnQueueLabel.Location = New System.Drawing.Point(30, 249)
        Me.NextOnQueueLabel.Name = "NextOnQueueLabel"
        Me.NextOnQueueLabel.Size = New System.Drawing.Size(331, 21)
        Me.NextOnQueueLabel.TabIndex = 4
        Me.NextOnQueueLabel.Text = "Joshua Santos (joshuanoelsantos@gmailc.om)"
        '
        'StatusLabel
        '
        Me.StatusLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatusLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLabel.Location = New System.Drawing.Point(0, 19)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(490, 21)
        Me.StatusLabel.TabIndex = 5
        Me.StatusLabel.Text = "Status: ONLINE"
        Me.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DashboardPanel
        '
        Me.DashboardPanel.BackColor = System.Drawing.Color.DarkRed
        Me.DashboardPanel.Controls.Add(Me.Label1)
        Me.DashboardPanel.Controls.Add(Me.ProcessingDateLabel)
        Me.DashboardPanel.Controls.Add(Me.RestartEmailServiceLinkLabel)
        Me.DashboardPanel.Controls.Add(Me.NumberOfQueueLabel)
        Me.DashboardPanel.Controls.Add(Me.Label2)
        Me.DashboardPanel.Controls.Add(Me.Label3)
        Me.DashboardPanel.Controls.Add(Me.CurrentProcessingLabel)
        Me.DashboardPanel.Controls.Add(Me.NextOnQueueLabel)
        Me.DashboardPanel.Controls.Add(Me.StatusLabel)
        Me.DashboardPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DashboardPanel.Location = New System.Drawing.Point(0, 0)
        Me.DashboardPanel.Name = "DashboardPanel"
        Me.DashboardPanel.Size = New System.Drawing.Size(491, 346)
        Me.DashboardPanel.TabIndex = 6
        Me.DashboardPanel.Visible = False
        '
        'RestartEmailServiceLinkLabel
        '
        Me.RestartEmailServiceLinkLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RestartEmailServiceLinkLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.RestartEmailServiceLinkLabel.LinkColor = System.Drawing.Color.LimeGreen
        Me.RestartEmailServiceLinkLabel.Location = New System.Drawing.Point(0, 304)
        Me.RestartEmailServiceLinkLabel.Name = "RestartEmailServiceLinkLabel"
        Me.RestartEmailServiceLinkLabel.Size = New System.Drawing.Size(490, 21)
        Me.RestartEmailServiceLinkLabel.TabIndex = 7
        Me.RestartEmailServiceLinkLabel.TabStop = True
        Me.RestartEmailServiceLinkLabel.Text = "Restart Email Service"
        Me.RestartEmailServiceLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.RestartEmailServiceLinkLabel.VisitedLinkColor = System.Drawing.Color.LawnGreen
        '
        'LoadingPictureBox
        '
        Me.LoadingPictureBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LoadingPictureBox.Image = Global.AccuPay.My.Resources.Resources.ajax_loader
        Me.LoadingPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.LoadingPictureBox.Name = "LoadingPictureBox"
        Me.LoadingPictureBox.Size = New System.Drawing.Size(491, 346)
        Me.LoadingPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.LoadingPictureBox.TabIndex = 7
        Me.LoadingPictureBox.TabStop = False
        '
        'RefreshTimer
        '
        Me.RefreshTimer.Interval = 1000
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(30, 143)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(155, 21)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Started Processing:"
        '
        'ProcessingDateLabel
        '
        Me.ProcessingDateLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProcessingDateLabel.AutoSize = True
        Me.ProcessingDateLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProcessingDateLabel.Location = New System.Drawing.Point(30, 164)
        Me.ProcessingDateLabel.Name = "ProcessingDateLabel"
        Me.ProcessingDateLabel.Size = New System.Drawing.Size(177, 21)
        Me.ProcessingDateLabel.TabIndex = 9
        Me.ProcessingDateLabel.Text = "10/8/2020 10:24:15 AM"
        '
        'EmailDashboardForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(491, 346)
        Me.Controls.Add(Me.DashboardPanel)
        Me.Controls.Add(Me.LoadingPictureBox)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.ForeColor = System.Drawing.Color.White
        Me.Name = "EmailDashboardForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Email Dashboard"
        Me.DashboardPanel.ResumeLayout(False)
        Me.DashboardPanel.PerformLayout()
        CType(Me.LoadingPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents NumberOfQueueLabel As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents CurrentProcessingLabel As Label
    Friend WithEvents NextOnQueueLabel As Label
    Friend WithEvents StatusLabel As Label
    Friend WithEvents DashboardPanel As Panel
    Friend WithEvents LoadingPictureBox As PictureBox
    Friend WithEvents RestartEmailServiceLinkLabel As LinkLabel
    Friend WithEvents RefreshTimer As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents ProcessingDateLabel As Label
End Class
