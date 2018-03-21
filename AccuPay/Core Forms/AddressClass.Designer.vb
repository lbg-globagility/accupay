<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddressClass
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddressClass))
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvAddress = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.adRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.adStreet = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.adStreet2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.adBrgy = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.adCity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.adState = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.adCountry = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.adZip = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.autcomptxtaddress = New Femiani.Forms.UI.Input.AutoCompleteTextBox()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtZip = New System.Windows.Forms.TextBox()
        Me.txtCountry = New System.Windows.Forms.TextBox()
        Me.txtState = New System.Windows.Forms.TextBox()
        Me.txtCity = New System.Windows.Forms.TextBox()
        Me.txtBrgy = New System.Windows.Forms.TextBox()
        Me.txtStreet2 = New System.Windows.Forms.TextBox()
        Me.txtStreet = New System.Windows.Forms.TextBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewAddress = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveAddress = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancel = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        CType(Me.dgvAddress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvAddress
        '
        Me.dgvAddress.AllowUserToAddRows = False
        Me.dgvAddress.AllowUserToDeleteRows = False
        Me.dgvAddress.AllowUserToOrderColumns = True
        Me.dgvAddress.AllowUserToResizeColumns = False
        Me.dgvAddress.AllowUserToResizeRows = False
        Me.dgvAddress.BackgroundColor = System.Drawing.Color.White
        resources.ApplyResources(Me.dgvAddress, "dgvAddress")
        Me.dgvAddress.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.adRowID, Me.adStreet, Me.adStreet2, Me.adBrgy, Me.adCity, Me.adState, Me.adCountry, Me.adZip})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvAddress.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgvAddress.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvAddress.MultiSelect = False
        Me.dgvAddress.Name = "dgvAddress"
        Me.dgvAddress.ReadOnly = True
        Me.dgvAddress.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        '
        'adRowID
        '
        resources.ApplyResources(Me.adRowID, "adRowID")
        Me.adRowID.Name = "adRowID"
        Me.adRowID.ReadOnly = True
        '
        'adStreet
        '
        resources.ApplyResources(Me.adStreet, "adStreet")
        Me.adStreet.Name = "adStreet"
        Me.adStreet.ReadOnly = True
        '
        'adStreet2
        '
        resources.ApplyResources(Me.adStreet2, "adStreet2")
        Me.adStreet2.Name = "adStreet2"
        Me.adStreet2.ReadOnly = True
        '
        'adBrgy
        '
        resources.ApplyResources(Me.adBrgy, "adBrgy")
        Me.adBrgy.Name = "adBrgy"
        Me.adBrgy.ReadOnly = True
        '
        'adCity
        '
        resources.ApplyResources(Me.adCity, "adCity")
        Me.adCity.Name = "adCity"
        Me.adCity.ReadOnly = True
        '
        'adState
        '
        resources.ApplyResources(Me.adState, "adState")
        Me.adState.Name = "adState"
        Me.adState.ReadOnly = True
        '
        'adCountry
        '
        resources.ApplyResources(Me.adCountry, "adCountry")
        Me.adCountry.Name = "adCountry"
        Me.adCountry.ReadOnly = True
        '
        'adZip
        '
        resources.ApplyResources(Me.adZip, "adZip")
        Me.adZip.Name = "adZip"
        Me.adZip.ReadOnly = True
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'autcomptxtaddress
        '
        resources.ApplyResources(Me.autcomptxtaddress, "autcomptxtaddress")
        Me.autcomptxtaddress.Name = "autcomptxtaddress"
        Me.autcomptxtaddress.PopupBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.autcomptxtaddress.PopupOffset = New System.Drawing.Point(12, 0)
        Me.autcomptxtaddress.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.autcomptxtaddress.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.autcomptxtaddress.PopupWidth = 300
        '
        'btnRefresh
        '
        resources.ApplyResources(Me.btnRefresh, "btnRefresh")
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.txtZip)
        Me.Panel2.Controls.Add(Me.txtCountry)
        Me.Panel2.Controls.Add(Me.txtState)
        Me.Panel2.Controls.Add(Me.txtCity)
        Me.Panel2.Controls.Add(Me.txtBrgy)
        Me.Panel2.Controls.Add(Me.txtStreet2)
        Me.Panel2.Controls.Add(Me.txtStreet)
        resources.ApplyResources(Me.Panel2, "Panel2")
        Me.Panel2.Name = "Panel2"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'txtZip
        '
        resources.ApplyResources(Me.txtZip, "txtZip")
        Me.txtZip.Name = "txtZip"
        '
        'txtCountry
        '
        resources.ApplyResources(Me.txtCountry, "txtCountry")
        Me.txtCountry.Name = "txtCountry"
        '
        'txtState
        '
        resources.ApplyResources(Me.txtState, "txtState")
        Me.txtState.Name = "txtState"
        '
        'txtCity
        '
        resources.ApplyResources(Me.txtCity, "txtCity")
        Me.txtCity.Name = "txtCity"
        '
        'txtBrgy
        '
        resources.ApplyResources(Me.txtBrgy, "txtBrgy")
        Me.txtBrgy.Name = "txtBrgy"
        '
        'txtStreet2
        '
        resources.ApplyResources(Me.txtStreet2, "txtStreet2")
        Me.txtStreet2.Name = "txtStreet2"
        '
        'txtStreet
        '
        resources.ApplyResources(Me.txtStreet, "txtStreet")
        Me.txtStreet.Name = "txtStreet"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewAddress, Me.tsbtnSaveAddress, Me.tsbtnCancel, Me.tsbtnClose})
        resources.ApplyResources(Me.ToolStrip1, "ToolStrip1")
        Me.ToolStrip1.Name = "ToolStrip1"
        '
        'tsbtnNewAddress
        '
        resources.ApplyResources(Me.tsbtnNewAddress, "tsbtnNewAddress")
        Me.tsbtnNewAddress.Name = "tsbtnNewAddress"
        '
        'tsbtnSaveAddress
        '
        resources.ApplyResources(Me.tsbtnSaveAddress, "tsbtnSaveAddress")
        Me.tsbtnSaveAddress.Name = "tsbtnSaveAddress"
        '
        'tsbtnCancel
        '
        resources.ApplyResources(Me.tsbtnCancel, "tsbtnCancel")
        Me.tsbtnCancel.Name = "tsbtnCancel"
        '
        'tsbtnClose
        '
        Me.tsbtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        resources.ApplyResources(Me.tsbtnClose, "tsbtnClose")
        Me.tsbtnClose.Name = "tsbtnClose"
        '
        'btnOK
        '
        resources.ApplyResources(Me.btnOK, "btnOK")
        Me.btnOK.Name = "btnOK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        resources.ApplyResources(Me.btnClose, "btnClose")
        Me.btnClose.Name = "btnClose"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'AddressClass
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.autcomptxtaddress)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.dgvAddress)
        Me.KeyPreview = True
        Me.Name = "AddressClass"
        CType(Me.dgvAddress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvAddress As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents autcomptxtaddress As Femiani.Forms.UI.Input.AutoCompleteTextBox
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnNewAddress As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnSaveAddress As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents txtStreet As System.Windows.Forms.TextBox
    Friend WithEvents txtZip As System.Windows.Forms.TextBox
    Friend WithEvents txtCountry As System.Windows.Forms.TextBox
    Friend WithEvents txtState As System.Windows.Forms.TextBox
    Friend WithEvents txtCity As System.Windows.Forms.TextBox
    Friend WithEvents txtBrgy As System.Windows.Forms.TextBox
    Friend WithEvents txtStreet2 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents adRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents adStreet As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents adStreet2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents adBrgy As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents adCity As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents adState As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents adCountry As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents adZip As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
