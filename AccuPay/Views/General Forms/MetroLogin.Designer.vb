<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MetroLogin
    'Inherits System.Windows.Forms.Form
    Inherits MetroFramework.Forms.MetroForm

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MetroLogin))
        Me.MetroStyleManager1 = New MetroFramework.Components.MetroStyleManager(Me.components)
        Me.MetroLabel2 = New MetroFramework.Controls.MetroLabel()
        Me.MetroLabel1 = New MetroFramework.Controls.MetroLabel()
        Me.PasswordTextBox = New MetroFramework.Controls.MetroTextBox()
        Me.UserNameTextBox = New MetroFramework.Controls.MetroTextBox()
        Me.OrganizationComboBox = New MetroFramework.Controls.MetroComboBox()
        Me.btnlogin = New MetroFramework.Controls.MetroButton()
        Me.MetroLabel3 = New MetroFramework.Controls.MetroLabel()
        Me.PhotoImages = New System.Windows.Forms.PictureBox()
        Me.MetroLink1 = New MetroFramework.Controls.MetroLink()
        CType(Me.MetroStyleManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PhotoImages, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MetroStyleManager1
        '
        Me.MetroStyleManager1.Owner = Me
        '
        'MetroLabel2
        '
        Me.MetroLabel2.AutoSize = True
        Me.MetroLabel2.Location = New System.Drawing.Point(23, 118)
        Me.MetroLabel2.Name = "MetroLabel2"
        Me.MetroLabel2.Size = New System.Drawing.Size(63, 19)
        Me.MetroLabel2.TabIndex = 13
        Me.MetroLabel2.Text = "Password"
        '
        'MetroLabel1
        '
        Me.MetroLabel1.AutoSize = True
        Me.MetroLabel1.Location = New System.Drawing.Point(23, 70)
        Me.MetroLabel1.Name = "MetroLabel1"
        Me.MetroLabel1.Size = New System.Drawing.Size(68, 19)
        Me.MetroLabel1.TabIndex = 12
        Me.MetroLabel1.Text = "Username"
        '
        'txtbxPword
        '
        Me.PasswordTextBox.Lines = New String(-1) {}
        Me.PasswordTextBox.Location = New System.Drawing.Point(23, 140)
        Me.PasswordTextBox.MaxLength = 32767
        Me.PasswordTextBox.Name = "txtbxPword"
        Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.PasswordTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.PasswordTextBox.SelectedText = ""
        Me.PasswordTextBox.Size = New System.Drawing.Size(213, 23)
        Me.PasswordTextBox.TabIndex = 1
        Me.PasswordTextBox.UseSelectable = True
        Me.PasswordTextBox.UseSystemPasswordChar = True
        '
        'txtbxUserID
        '
        Me.UserNameTextBox.Lines = New String(-1) {}
        Me.UserNameTextBox.Location = New System.Drawing.Point(23, 92)
        Me.UserNameTextBox.MaxLength = 32767
        Me.UserNameTextBox.Name = "txtbxUserID"
        Me.UserNameTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.UserNameTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.UserNameTextBox.SelectedText = ""
        Me.UserNameTextBox.Size = New System.Drawing.Size(213, 23)
        Me.UserNameTextBox.TabIndex = 0
        Me.UserNameTextBox.UseSelectable = True
        '
        'cbxorganiz
        '
        Me.OrganizationComboBox.FontSize = MetroFramework.MetroComboBoxSize.Small
        Me.OrganizationComboBox.FormattingEnabled = True
        Me.OrganizationComboBox.ItemHeight = 19
        Me.OrganizationComboBox.Location = New System.Drawing.Point(23, 220)
        Me.OrganizationComboBox.MaxDropDownItems = 1
        Me.OrganizationComboBox.Name = "cbxorganiz"
        Me.OrganizationComboBox.Size = New System.Drawing.Size(213, 25)
        Me.OrganizationComboBox.TabIndex = 2
        Me.OrganizationComboBox.UseSelectable = True
        '
        'btnlogin
        '
        Me.btnlogin.FontWeight = MetroFramework.MetroButtonWeight.Regular
        Me.btnlogin.Location = New System.Drawing.Point(97, 273)
        Me.btnlogin.Name = "btnlogin"
        Me.btnlogin.Size = New System.Drawing.Size(64, 23)
        Me.btnlogin.TabIndex = 3
        Me.btnlogin.Text = "Login"
        Me.btnlogin.UseSelectable = True
        '
        'MetroLabel3
        '
        Me.MetroLabel3.AutoSize = True
        Me.MetroLabel3.Location = New System.Drawing.Point(23, 198)
        Me.MetroLabel3.Name = "MetroLabel3"
        Me.MetroLabel3.Size = New System.Drawing.Size(66, 19)
        Me.MetroLabel3.TabIndex = 14
        Me.MetroLabel3.Text = "Company"
        '
        'PhotoImages
        '
        Me.PhotoImages.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PhotoImages.BackColor = System.Drawing.Color.FromArgb(CType(CType(238, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(238, Byte), Integer))
        Me.PhotoImages.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PhotoImages.Location = New System.Drawing.Point(242, 89)
        Me.PhotoImages.Name = "PhotoImages"
        Me.PhotoImages.Size = New System.Drawing.Size(279, 207)
        Me.PhotoImages.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PhotoImages.TabIndex = 19
        Me.PhotoImages.TabStop = False
        '
        'MetroLink1
        '
        Me.MetroLink1.Location = New System.Drawing.Point(132, 169)
        Me.MetroLink1.Name = "MetroLink1"
        Me.MetroLink1.Size = New System.Drawing.Size(104, 23)
        Me.MetroLink1.TabIndex = 4
        Me.MetroLink1.Text = "Forgot password?"
        Me.MetroLink1.UseSelectable = True
        '
        'MetroLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(544, 371)
        Me.Controls.Add(Me.MetroLink1)
        Me.Controls.Add(Me.PhotoImages)
        Me.Controls.Add(Me.MetroLabel3)
        Me.Controls.Add(Me.MetroLabel2)
        Me.Controls.Add(Me.MetroLabel1)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.UserNameTextBox)
        Me.Controls.Add(Me.OrganizationComboBox)
        Me.Controls.Add(Me.btnlogin)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Movable = False
        Me.Name = "MetroLogin"
        Me.Resizable = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Welcome"
        CType(Me.MetroStyleManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PhotoImages, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MetroStyleManager1 As MetroFramework.Components.MetroStyleManager
    Friend WithEvents MetroLabel2 As MetroFramework.Controls.MetroLabel
    Friend WithEvents MetroLabel1 As MetroFramework.Controls.MetroLabel
    Friend WithEvents PasswordTextBox As MetroFramework.Controls.MetroTextBox
    Friend WithEvents UserNameTextBox As MetroFramework.Controls.MetroTextBox
    Friend WithEvents OrganizationComboBox As MetroFramework.Controls.MetroComboBox
    Friend WithEvents btnlogin As MetroFramework.Controls.MetroButton
    Friend WithEvents MetroLabel3 As MetroFramework.Controls.MetroLabel
    Friend WithEvents PhotoImages As System.Windows.Forms.PictureBox
    Friend WithEvents MetroLink1 As MetroFramework.Controls.MetroLink
End Class
