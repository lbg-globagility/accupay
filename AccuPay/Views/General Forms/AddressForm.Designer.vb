<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddressForm
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
        Me.btnreset = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnsave = New System.Windows.Forms.Button()
        Me.zipCodeTxt = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.sa2Txt = New System.Windows.Forms.TextBox()
        Me.sa1Txt = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.brgyCB = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cityTownCB = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.provStateCB = New System.Windows.Forms.ComboBox()
        Me.stateProvCB = New System.Windows.Forms.Label()
        Me.countryCB = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.addressGrid = New System.Windows.Forms.DataGridView()
        Me.c_country = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_state = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_city = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_brgy = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_street1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_street2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_ZipCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1.SuspendLayout()
        CType(Me.addressGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnreset
        '
        Me.btnreset.Location = New System.Drawing.Point(716, 67)
        Me.btnreset.Name = "btnreset"
        Me.btnreset.Size = New System.Drawing.Size(75, 34)
        Me.btnreset.TabIndex = 19
        Me.btnreset.Text = "Reset"
        Me.btnreset.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(669, 33)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(13, 17)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "*"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Red
        Me.Label9.Location = New System.Drawing.Point(394, 67)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(13, 17)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "*"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(84, 98)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(13, 17)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "*"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(84, 34)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(13, 17)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "*"
        '
        'btnsave
        '
        Me.btnsave.Location = New System.Drawing.Point(633, 67)
        Me.btnsave.Name = "btnsave"
        Me.btnsave.Size = New System.Drawing.Size(75, 34)
        Me.btnsave.TabIndex = 14
        Me.btnsave.Text = "Save"
        Me.btnsave.UseVisualStyleBackColor = True
        '
        'zipCodeTxt
        '
        Me.zipCodeTxt.Location = New System.Drawing.Point(683, 30)
        Me.zipCodeTxt.Name = "zipCodeTxt"
        Me.zipCodeTxt.Size = New System.Drawing.Size(104, 20)
        Me.zipCodeTxt.TabIndex = 13
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(613, 33)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Zip Code :"
        '
        'sa2Txt
        '
        Me.sa2Txt.Location = New System.Drawing.Point(408, 97)
        Me.sa2Txt.Name = "sa2Txt"
        Me.sa2Txt.Size = New System.Drawing.Size(205, 20)
        Me.sa2Txt.TabIndex = 11
        '
        'sa1Txt
        '
        Me.sa1Txt.Location = New System.Drawing.Point(408, 63)
        Me.sa1Txt.Name = "sa1Txt"
        Me.sa1Txt.Size = New System.Drawing.Size(205, 20)
        Me.sa1Txt.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(300, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(91, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Street Address 2 :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(300, 66)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Street Address 1 :"
        '
        'brgyCB
        '
        Me.brgyCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.brgyCB.FormattingEnabled = True
        Me.brgyCB.Location = New System.Drawing.Point(408, 30)
        Me.brgyCB.Name = "brgyCB"
        Me.brgyCB.Size = New System.Drawing.Size(146, 21)
        Me.brgyCB.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(300, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Barangay :"
        '
        'cityTownCB
        '
        Me.cityTownCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cityTownCB.FormattingEnabled = True
        Me.cityTownCB.Location = New System.Drawing.Point(97, 97)
        Me.cityTownCB.Name = "cityTownCB"
        Me.cityTownCB.Size = New System.Drawing.Size(146, 21)
        Me.cityTownCB.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "City/Town :"
        '
        'provStateCB
        '
        Me.provStateCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.provStateCB.FormattingEnabled = True
        Me.provStateCB.Location = New System.Drawing.Point(97, 63)
        Me.provStateCB.Name = "provStateCB"
        Me.provStateCB.Size = New System.Drawing.Size(146, 21)
        Me.provStateCB.TabIndex = 3
        '
        'stateProvCB
        '
        Me.stateProvCB.AutoSize = True
        Me.stateProvCB.Location = New System.Drawing.Point(6, 66)
        Me.stateProvCB.Name = "stateProvCB"
        Me.stateProvCB.Size = New System.Drawing.Size(85, 13)
        Me.stateProvCB.TabIndex = 2
        Me.stateProvCB.Text = "State/Province :"
        '
        'countryCB
        '
        Me.countryCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.countryCB.FormattingEnabled = True
        Me.countryCB.Location = New System.Drawing.Point(98, 30)
        Me.countryCB.Name = "countryCB"
        Me.countryCB.Size = New System.Drawing.Size(145, 21)
        Me.countryCB.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Country :"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnreset)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.btnsave)
        Me.GroupBox1.Controls.Add(Me.zipCodeTxt)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.sa2Txt)
        Me.GroupBox1.Controls.Add(Me.sa1Txt)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.brgyCB)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.cityTownCB)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.provStateCB)
        Me.GroupBox1.Controls.Add(Me.stateProvCB)
        Me.GroupBox1.Controls.Add(Me.countryCB)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(14, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(799, 140)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Add New"
        '
        'addressGrid
        '
        Me.addressGrid.AllowUserToAddRows = False
        Me.addressGrid.AllowUserToDeleteRows = False
        Me.addressGrid.AllowUserToResizeColumns = False
        Me.addressGrid.AllowUserToResizeRows = False
        Me.addressGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.addressGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.addressGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.addressGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_country, Me.c_state, Me.c_city, Me.c_brgy, Me.c_street1, Me.c_street2, Me.c_ZipCode, Me.c_RowID})
        Me.addressGrid.Location = New System.Drawing.Point(12, 158)
        Me.addressGrid.Name = "addressGrid"
        Me.addressGrid.ReadOnly = True
        Me.addressGrid.RowHeadersVisible = False
        Me.addressGrid.Size = New System.Drawing.Size(799, 271)
        Me.addressGrid.TabIndex = 4
        '
        'c_country
        '
        Me.c_country.HeaderText = "Country"
        Me.c_country.Name = "c_country"
        Me.c_country.ReadOnly = True
        '
        'c_state
        '
        Me.c_state.HeaderText = "State/Province"
        Me.c_state.Name = "c_state"
        Me.c_state.ReadOnly = True
        '
        'c_city
        '
        Me.c_city.HeaderText = "City/Town"
        Me.c_city.Name = "c_city"
        Me.c_city.ReadOnly = True
        '
        'c_brgy
        '
        Me.c_brgy.HeaderText = "Barangay"
        Me.c_brgy.Name = "c_brgy"
        Me.c_brgy.ReadOnly = True
        '
        'c_street1
        '
        Me.c_street1.HeaderText = "Street Address 1"
        Me.c_street1.Name = "c_street1"
        Me.c_street1.ReadOnly = True
        '
        'c_street2
        '
        Me.c_street2.HeaderText = "Street Address 2"
        Me.c_street2.Name = "c_street2"
        Me.c_street2.ReadOnly = True
        '
        'c_ZipCode
        '
        Me.c_ZipCode.HeaderText = "Zip Code"
        Me.c_ZipCode.Name = "c_ZipCode"
        Me.c_ZipCode.ReadOnly = True
        '
        'c_RowID
        '
        Me.c_RowID.HeaderText = "RowID"
        Me.c_RowID.Name = "c_RowID"
        Me.c_RowID.ReadOnly = True
        Me.c_RowID.Visible = False
        '
        'AddressForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(825, 441)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.addressGrid)
        Me.Name = "AddressForm"
        Me.Text = "Address Form"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.addressGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnreset As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnsave As System.Windows.Forms.Button
    Friend WithEvents zipCodeTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents sa2Txt As System.Windows.Forms.TextBox
    Friend WithEvents sa1Txt As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents brgyCB As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cityTownCB As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents provStateCB As System.Windows.Forms.ComboBox
    Friend WithEvents stateProvCB As System.Windows.Forms.Label
    Friend WithEvents countryCB As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents addressGrid As System.Windows.Forms.DataGridView
    Friend WithEvents c_country As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_state As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_city As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_brgy As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_street1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_street2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_ZipCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_RowID As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
