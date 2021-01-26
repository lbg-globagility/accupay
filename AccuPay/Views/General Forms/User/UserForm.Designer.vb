<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UserForm
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
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ToolStrip3 = New System.Windows.Forms.ToolStrip()
        Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.DeleteToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.CancelToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.CloseToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.UserGrid = New System.Windows.Forms.DataGridView()
        Me.LastNameColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FirstNameColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserUserControl = New UserUserControl()
        Me.LabelForBalloon = New System.Windows.Forms.Label()
        Me.ToolStrip3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.UserGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label8.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label8.Location = New System.Drawing.Point(0, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(1235, 24)
        Me.Label8.TabIndex = 66
        Me.Label8.Text = "USER FORM"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ToolStrip3
        '
        Me.ToolStrip3.AutoSize = False
        Me.ToolStrip3.BackColor = System.Drawing.Color.White
        Me.ToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.SaveToolStripButton, Me.DeleteToolStripButton, Me.CancelToolStripButton, Me.CloseToolStripButton})
        Me.ToolStrip3.Location = New System.Drawing.Point(343, 24)
        Me.ToolStrip3.Name = "ToolStrip3"
        Me.ToolStrip3.Size = New System.Drawing.Size(892, 22)
        Me.ToolStrip3.TabIndex = 67
        Me.ToolStrip3.Text = "ToolStrip3"
        '
        'NewButton
        '
        Me.NewToolStripButton.Image = Global.AccuPay.My.Resources.Resources._new
        Me.NewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripButton.Name = "NewButton"
        Me.NewToolStripButton.Size = New System.Drawing.Size(51, 19)
        Me.NewToolStripButton.Text = "&New"
        Me.NewToolStripButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SaveButton
        '
        Me.SaveToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripButton.Name = "SaveButton"
        Me.SaveToolStripButton.Size = New System.Drawing.Size(51, 19)
        Me.SaveToolStripButton.Text = "&Save"
        '
        'DeleteButton
        '
        Me.DeleteToolStripButton.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteToolStripButton.Name = "DeleteButton"
        Me.DeleteToolStripButton.Size = New System.Drawing.Size(60, 19)
        Me.DeleteToolStripButton.Text = "&Delete"
        Me.DeleteToolStripButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CancelButton
        '
        Me.CancelToolStripButton.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.CancelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CancelToolStripButton.Name = "CancelButton"
        Me.CancelToolStripButton.Size = New System.Drawing.Size(63, 19)
        Me.CancelToolStripButton.Text = "&Cancel"
        '
        'CloseButton
        '
        Me.CloseToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.CloseToolStripButton.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.CloseToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CloseToolStripButton.Name = "CloseButton"
        Me.CloseToolStripButton.Size = New System.Drawing.Size(56, 19)
        Me.CloseToolStripButton.Text = "&Close"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.UserGrid)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel2.Location = New System.Drawing.Point(0, 24)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(343, 448)
        Me.Panel2.TabIndex = 158
        '
        'UserGrid
        '
        Me.UserGrid.AllowUserToAddRows = False
        Me.UserGrid.AllowUserToDeleteRows = False
        Me.UserGrid.AllowUserToOrderColumns = True
        Me.UserGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UserGrid.BackgroundColor = System.Drawing.Color.White
        Me.UserGrid.ColumnHeadersHeight = 38
        Me.UserGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.UserGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.LastNameColumn, Me.FirstNameColumn, Me.Column2})
        Me.UserGrid.Location = New System.Drawing.Point(12, 145)
        Me.UserGrid.MultiSelect = False
        Me.UserGrid.Name = "UserGrid"
        Me.UserGrid.ReadOnly = True
        Me.UserGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.UserGrid.Size = New System.Drawing.Size(324, 274)
        Me.UserGrid.TabIndex = 109
        '
        'LastNameColumn
        '
        Me.LastNameColumn.DataPropertyName = "LastName"
        Me.LastNameColumn.HeaderText = "Last Name"
        Me.LastNameColumn.Name = "LastNameColumn"
        Me.LastNameColumn.ReadOnly = True
        '
        'FirstNameColumn
        '
        Me.FirstNameColumn.DataPropertyName = "FirstName"
        Me.FirstNameColumn.HeaderText = "First Name"
        Me.FirstNameColumn.Name = "FirstNameColumn"
        Me.FirstNameColumn.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Column2.DataPropertyName = "UserName"
        Me.Column2.HeaderText = "Username"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "DisplayName"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Username"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "LastName"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "FirstName"
        Me.DataGridViewTextBoxColumn3.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "MiddleName"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Middle Name"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'UserUserControl
        '
        Me.UserUserControl.BackColor = System.Drawing.Color.White
        Me.UserUserControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserUserControl.Location = New System.Drawing.Point(343, 46)
        Me.UserUserControl.Name = "UserUserControl"
        Me.UserUserControl.Size = New System.Drawing.Size(892, 426)
        Me.UserUserControl.TabIndex = 159
        '
        'LabelForBalloon
        '
        Me.LabelForBalloon.AutoSize = True
        Me.LabelForBalloon.Location = New System.Drawing.Point(468, 38)
        Me.LabelForBalloon.Name = "LabelForBalloon"
        Me.LabelForBalloon.Size = New System.Drawing.Size(90, 13)
        Me.LabelForBalloon.TabIndex = 160
        Me.LabelForBalloon.Text = "label for baloon"
        '
        'UsersForm2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1235, 472)
        Me.ControlBox = False
        Me.Controls.Add(Me.UserUserControl)
        Me.Controls.Add(Me.ToolStrip3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.LabelForBalloon)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.HelpButton = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UsersForm2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ToolStrip3.ResumeLayout(False)
        Me.ToolStrip3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.UserGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label8 As Label
    Friend WithEvents ToolStrip3 As ToolStrip
    Friend WithEvents NewToolStripButton As ToolStripButton
    Friend WithEvents SaveToolStripButton As ToolStripButton
    Friend WithEvents DeleteToolStripButton As ToolStripButton
    Friend WithEvents CancelToolStripButton As ToolStripButton
    Friend WithEvents CloseToolStripButton As ToolStripButton
    Friend WithEvents Panel2 As Panel
    Friend WithEvents UserGrid As DataGridView
    Friend WithEvents UserUserControl As UserUserControl
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents LabelForBalloon As Label
    Friend WithEvents LastNameColumn As DataGridViewTextBoxColumn
    Friend WithEvents FirstNameColumn As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
End Class
