<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MetroLogin
    'Inherits System.Windows.Forms.Form
    Inherits MetroFramework.Forms.MetroForm
    
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
        Me.MetroStyleManager1 = New MetroFramework.Components.MetroStyleManager(Me.components)
        Me.MetroLabel2 = New MetroFramework.Controls.MetroLabel()
        Me.MetroLabel1 = New MetroFramework.Controls.MetroLabel()
        Me.txtbxPword = New MetroFramework.Controls.MetroTextBox()
        Me.txtbxUserID = New MetroFramework.Controls.MetroTextBox()
        Me.cbxorganiz = New MetroFramework.Controls.MetroComboBox()
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
        Me.MetroLabel2.Size = New System.Drawing.Size(64, 19)
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
        Me.txtbxPword.Lines = New String(-1) {}
        Me.txtbxPword.Location = New System.Drawing.Point(23, 140)
        Me.txtbxPword.MaxLength = 32767
        Me.txtbxPword.Name = "txtbxPword"
        Me.txtbxPword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtbxPword.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtbxPword.SelectedText = ""
        Me.txtbxPword.Size = New System.Drawing.Size(213, 23)
        Me.txtbxPword.TabIndex = 1
        Me.txtbxPword.UseSelectable = True
        Me.txtbxPword.UseSystemPasswordChar = True
        '
        'txtbxUserID
        '
        Me.txtbxUserID.Lines = New String(-1) {}
        Me.txtbxUserID.Location = New System.Drawing.Point(23, 92)
        Me.txtbxUserID.MaxLength = 32767
        Me.txtbxUserID.Name = "txtbxUserID"
        Me.txtbxUserID.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtbxUserID.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.txtbxUserID.SelectedText = ""
        Me.txtbxUserID.Size = New System.Drawing.Size(213, 23)
        Me.txtbxUserID.TabIndex = 0
        Me.txtbxUserID.UseSelectable = True
        '
        'cbxorganiz
        '
        Me.cbxorganiz.FontSize = MetroFramework.MetroComboBoxSize.Small
        Me.cbxorganiz.FormattingEnabled = True
        Me.cbxorganiz.ItemHeight = 19
        Me.cbxorganiz.Location = New System.Drawing.Point(23, 220)
        Me.cbxorganiz.MaxDropDownItems = 1
        Me.cbxorganiz.Name = "cbxorganiz"
        Me.cbxorganiz.Size = New System.Drawing.Size(213, 25)
        Me.cbxorganiz.TabIndex = 2
        Me.cbxorganiz.UseSelectable = True
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
        Me.ClientSize = New System.Drawing.Size(544, 319)
        Me.Controls.Add(Me.MetroLink1)
        Me.Controls.Add(Me.PhotoImages)
        Me.Controls.Add(Me.MetroLabel3)
        Me.Controls.Add(Me.MetroLabel2)
        Me.Controls.Add(Me.MetroLabel1)
        Me.Controls.Add(Me.txtbxPword)
        Me.Controls.Add(Me.txtbxUserID)
        Me.Controls.Add(Me.cbxorganiz)
        Me.Controls.Add(Me.btnlogin)
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
    Friend WithEvents txtbxPword As MetroFramework.Controls.MetroTextBox
    Friend WithEvents txtbxUserID As MetroFramework.Controls.MetroTextBox
    Friend WithEvents cbxorganiz As MetroFramework.Controls.MetroComboBox
    Friend WithEvents btnlogin As MetroFramework.Controls.MetroButton
    Friend WithEvents MetroLabel3 As MetroFramework.Controls.MetroLabel
    Friend WithEvents PhotoImages As System.Windows.Forms.PictureBox
    Friend WithEvents MetroLink1 As MetroFramework.Controls.MetroLink
End Class
