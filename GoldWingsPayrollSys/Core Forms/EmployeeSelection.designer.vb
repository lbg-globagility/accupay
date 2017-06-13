<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmployeeSelection
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvEmployee = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.ColRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column17 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column18 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column19 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column20 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column21 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DivisionRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column22 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSelect = New System.Windows.Forms.Button()
        Me.First = New System.Windows.Forms.LinkLabel()
        Me.Prev = New System.Windows.Forms.LinkLabel()
        Me.Last = New System.Windows.Forms.LinkLabel()
        Me.Nxt = New System.Windows.Forms.LinkLabel()
        Me.pbEmpPic = New System.Windows.Forms.PictureBox()
        Me.cbosearch5 = New System.Windows.Forms.ComboBox()
        Me.cbosearch3 = New System.Windows.Forms.ComboBox()
        Me.cbosearch2 = New System.Windows.Forms.ComboBox()
        Me.cbosearch1 = New System.Windows.Forms.ComboBox()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.txtSName = New System.Windows.Forms.TextBox()
        Me.txtLName = New System.Windows.Forms.TextBox()
        Me.txtFName = New System.Windows.Forms.TextBox()
        Me.txtEmpID = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.cbosearch4 = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMName = New System.Windows.Forms.TextBox()
        Me.autcomptxtagency = New Femiani.Forms.UI.Input.AutoCompleteTextBox()
        Me.bgworkSearchAutoComp = New System.ComponentModel.BackgroundWorker()
        CType(Me.dgvEmployee, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbEmpPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvEmployee
        '
        Me.dgvEmployee.AllowUserToAddRows = False
        Me.dgvEmployee.AllowUserToDeleteRows = False
        Me.dgvEmployee.AllowUserToOrderColumns = True
        Me.dgvEmployee.BackgroundColor = System.Drawing.Color.White
        Me.dgvEmployee.ColumnHeadersHeight = 34
        Me.dgvEmployee.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvEmployee.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColRowID, Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column8, Me.Column9, Me.Column10, Me.Column11, Me.Column12, Me.Column13, Me.Column14, Me.Column15, Me.Column16, Me.Column17, Me.Column18, Me.Column19, Me.Column20, Me.Column21, Me.DivisionRowID, Me.Column22})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEmployee.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvEmployee.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvEmployee.Location = New System.Drawing.Point(12, 141)
        Me.dgvEmployee.MultiSelect = False
        Me.dgvEmployee.Name = "dgvEmployee"
        Me.dgvEmployee.ReadOnly = True
        Me.dgvEmployee.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmployee.Size = New System.Drawing.Size(1179, 317)
        Me.dgvEmployee.TabIndex = 391
        '
        'ColRowID
        '
        Me.ColRowID.HeaderText = "RowID"
        Me.ColRowID.Name = "ColRowID"
        Me.ColRowID.ReadOnly = True
        Me.ColRowID.Visible = False
        '
        'Column1
        '
        Me.Column1.HeaderText = "Employee ID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.HeaderText = "First name"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.HeaderText = "Middle name"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column4
        '
        Me.Column4.HeaderText = "Last name"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column5
        '
        Me.Column5.HeaderText = "Surname"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Column6
        '
        Me.Column6.HeaderText = "Nickname"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'Column7
        '
        Me.Column7.HeaderText = "TIN No."
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        '
        'Column8
        '
        Me.Column8.HeaderText = "SSS No."
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        '
        'Column9
        '
        Me.Column9.HeaderText = "HDMF No."
        Me.Column9.Name = "Column9"
        Me.Column9.ReadOnly = True
        '
        'Column10
        '
        Me.Column10.HeaderText = "PhilHealth No."
        Me.Column10.Name = "Column10"
        Me.Column10.ReadOnly = True
        '
        'Column11
        '
        Me.Column11.HeaderText = "Position name"
        Me.Column11.Name = "Column11"
        Me.Column11.ReadOnly = True
        '
        'Column12
        '
        Me.Column12.HeaderText = "PositionID"
        Me.Column12.Name = "Column12"
        Me.Column12.ReadOnly = True
        Me.Column12.Visible = False
        '
        'Column13
        '
        Me.Column13.HeaderText = "Employment status"
        Me.Column13.Name = "Column13"
        Me.Column13.ReadOnly = True
        '
        'Column14
        '
        Me.Column14.HeaderText = "Home address"
        Me.Column14.Name = "Column14"
        Me.Column14.ReadOnly = True
        '
        'Column15
        '
        Me.Column15.HeaderText = "Gender"
        Me.Column15.Name = "Column15"
        Me.Column15.ReadOnly = True
        '
        'Column16
        '
        Me.Column16.HeaderText = "Marital status"
        Me.Column16.Name = "Column16"
        Me.Column16.ReadOnly = True
        '
        'Column17
        '
        Me.Column17.HeaderText = "No. of dependent(s)"
        Me.Column17.Name = "Column17"
        Me.Column17.ReadOnly = True
        '
        'Column18
        '
        Me.Column18.HeaderText = "Birth date"
        Me.Column18.Name = "Column18"
        Me.Column18.ReadOnly = True
        '
        'Column19
        '
        Me.Column19.HeaderText = "Start date"
        Me.Column19.Name = "Column19"
        Me.Column19.ReadOnly = True
        '
        'Column20
        '
        Me.Column20.HeaderText = "Pay frequency"
        Me.Column20.Name = "Column20"
        Me.Column20.ReadOnly = True
        '
        'Column21
        '
        Me.Column21.HeaderText = "PayFrequencyID"
        Me.Column21.Name = "Column21"
        Me.Column21.ReadOnly = True
        Me.Column21.Visible = False
        '
        'DivisionRowID
        '
        Me.DivisionRowID.HeaderText = "DivisionID"
        Me.DivisionRowID.Name = "DivisionRowID"
        Me.DivisionRowID.ReadOnly = True
        Me.DivisionRowID.Visible = False
        '
        'Column22
        '
        Me.Column22.HeaderText = "Image"
        Me.Column22.Name = "Column22"
        Me.Column22.ReadOnly = True
        Me.Column22.Visible = False
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(1077, 488)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(114, 45)
        Me.btnClose.TabIndex = 393
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSelect
        '
        Me.btnSelect.Location = New System.Drawing.Point(957, 488)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(114, 45)
        Me.btnSelect.TabIndex = 392
        Me.btnSelect.Text = "OK"
        Me.btnSelect.UseVisualStyleBackColor = True
        '
        'First
        '
        Me.First.AutoSize = True
        Me.First.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.First.Location = New System.Drawing.Point(9, 461)
        Me.First.Name = "First"
        Me.First.Size = New System.Drawing.Size(44, 15)
        Me.First.TabIndex = 14
        Me.First.TabStop = True
        Me.First.Text = "<<First"
        '
        'Prev
        '
        Me.Prev.AutoSize = True
        Me.Prev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Prev.Location = New System.Drawing.Point(59, 461)
        Me.Prev.Name = "Prev"
        Me.Prev.Size = New System.Drawing.Size(38, 15)
        Me.Prev.TabIndex = 15
        Me.Prev.TabStop = True
        Me.Prev.Text = "<Prev"
        '
        'Last
        '
        Me.Last.AutoSize = True
        Me.Last.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Last.Location = New System.Drawing.Point(149, 461)
        Me.Last.Name = "Last"
        Me.Last.Size = New System.Drawing.Size(44, 15)
        Me.Last.TabIndex = 17
        Me.Last.TabStop = True
        Me.Last.Text = "Last>>"
        '
        'Nxt
        '
        Me.Nxt.AutoSize = True
        Me.Nxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nxt.Location = New System.Drawing.Point(104, 461)
        Me.Nxt.Name = "Nxt"
        Me.Nxt.Size = New System.Drawing.Size(39, 15)
        Me.Nxt.TabIndex = 16
        Me.Nxt.TabStop = True
        Me.Nxt.Text = "Next>"
        '
        'pbEmpPic
        '
        Me.pbEmpPic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbEmpPic.Location = New System.Drawing.Point(12, 12)
        Me.pbEmpPic.Name = "pbEmpPic"
        Me.pbEmpPic.Size = New System.Drawing.Size(89, 77)
        Me.pbEmpPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbEmpPic.TabIndex = 374
        Me.pbEmpPic.TabStop = False
        '
        'cbosearch5
        '
        Me.cbosearch5.FormattingEnabled = True
        Me.cbosearch5.Items.AddRange(New Object() {"starts with", "contains like", "is exactly", "does not contain", "is empty null", "is not empty"})
        Me.cbosearch5.Location = New System.Drawing.Point(934, 116)
        Me.cbosearch5.Name = "cbosearch5"
        Me.cbosearch5.Size = New System.Drawing.Size(92, 21)
        Me.cbosearch5.TabIndex = 8
        Me.cbosearch5.Visible = False
        '
        'cbosearch3
        '
        Me.cbosearch3.FormattingEnabled = True
        Me.cbosearch3.Items.AddRange(New Object() {"starts with", "contains like", "is exactly", "does not contain", "is empty null", "is not empty"})
        Me.cbosearch3.Location = New System.Drawing.Point(934, 64)
        Me.cbosearch3.Name = "cbosearch3"
        Me.cbosearch3.Size = New System.Drawing.Size(92, 21)
        Me.cbosearch3.TabIndex = 4
        Me.cbosearch3.Visible = False
        '
        'cbosearch2
        '
        Me.cbosearch2.FormattingEnabled = True
        Me.cbosearch2.Items.AddRange(New Object() {"starts with", "contains like", "is exactly", "does not contain", "is empty null", "is not empty"})
        Me.cbosearch2.Location = New System.Drawing.Point(934, 38)
        Me.cbosearch2.Name = "cbosearch2"
        Me.cbosearch2.Size = New System.Drawing.Size(92, 21)
        Me.cbosearch2.TabIndex = 2
        Me.cbosearch2.Visible = False
        '
        'cbosearch1
        '
        Me.cbosearch1.FormattingEnabled = True
        Me.cbosearch1.Items.AddRange(New Object() {"starts with", "contains like", "is exactly", "does not contain", "is empty null", "is not empty"})
        Me.cbosearch1.Location = New System.Drawing.Point(934, 12)
        Me.cbosearch1.Name = "cbosearch1"
        Me.cbosearch1.Size = New System.Drawing.Size(92, 21)
        Me.cbosearch1.TabIndex = 0
        Me.cbosearch1.Visible = False
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.Location = New System.Drawing.Point(861, 124)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(43, 13)
        Me.Label60.TabIndex = 375
        Me.Label60.Text = "Surame"
        Me.Label60.Visible = False
        '
        'Label59
        '
        Me.Label59.AutoSize = True
        Me.Label59.Location = New System.Drawing.Point(861, 98)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(58, 13)
        Me.Label59.TabIndex = 376
        Me.Label59.Text = "Last Name"
        Me.Label59.Visible = False
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(861, 46)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(57, 13)
        Me.Label58.TabIndex = 377
        Me.Label58.Text = "First Name"
        Me.Label58.Visible = False
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(861, 20)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(67, 13)
        Me.Label29.TabIndex = 378
        Me.Label29.Text = "Employee ID"
        Me.Label29.Visible = False
        '
        'txtSName
        '
        Me.txtSName.Location = New System.Drawing.Point(1032, 116)
        Me.txtSName.Name = "txtSName"
        Me.txtSName.Size = New System.Drawing.Size(161, 20)
        Me.txtSName.TabIndex = 9
        Me.txtSName.Visible = False
        '
        'txtLName
        '
        Me.txtLName.Location = New System.Drawing.Point(1032, 90)
        Me.txtLName.Name = "txtLName"
        Me.txtLName.Size = New System.Drawing.Size(161, 20)
        Me.txtLName.TabIndex = 7
        Me.txtLName.Visible = False
        '
        'txtFName
        '
        Me.txtFName.Location = New System.Drawing.Point(1032, 38)
        Me.txtFName.Name = "txtFName"
        Me.txtFName.Size = New System.Drawing.Size(161, 20)
        Me.txtFName.TabIndex = 3
        Me.txtFName.Visible = False
        '
        'txtEmpID
        '
        Me.txtEmpID.Location = New System.Drawing.Point(1032, 12)
        Me.txtEmpID.Name = "txtEmpID"
        Me.txtEmpID.Size = New System.Drawing.Size(161, 20)
        Me.txtEmpID.TabIndex = 1
        Me.txtEmpID.Visible = False
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(437, 64)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(55, 25)
        Me.btnSearch.TabIndex = 390
        Me.btnSearch.Text = "&Refresh"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cbosearch4
        '
        Me.cbosearch4.FormattingEnabled = True
        Me.cbosearch4.Items.AddRange(New Object() {"starts with", "contains like", "is exactly", "does not contain", "is empty null", "is not empty"})
        Me.cbosearch4.Location = New System.Drawing.Point(934, 90)
        Me.cbosearch4.Name = "cbosearch4"
        Me.cbosearch4.Size = New System.Drawing.Size(92, 21)
        Me.cbosearch4.TabIndex = 6
        Me.cbosearch4.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(861, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 388
        Me.Label1.Text = "Middle Name"
        Me.Label1.Visible = False
        '
        'txtMName
        '
        Me.txtMName.Location = New System.Drawing.Point(1032, 64)
        Me.txtMName.Name = "txtMName"
        Me.txtMName.Size = New System.Drawing.Size(161, 20)
        Me.txtMName.TabIndex = 5
        Me.txtMName.Visible = False
        '
        'autcomptxtagency
        '
        Me.autcomptxtagency.Enabled = False
        Me.autcomptxtagency.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.autcomptxtagency.Location = New System.Drawing.Point(107, 64)
        Me.autcomptxtagency.Name = "autcomptxtagency"
        Me.autcomptxtagency.PopupBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.autcomptxtagency.PopupOffset = New System.Drawing.Point(12, 0)
        Me.autcomptxtagency.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.autcomptxtagency.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.autcomptxtagency.PopupWidth = 300
        Me.autcomptxtagency.Size = New System.Drawing.Size(324, 25)
        Me.autcomptxtagency.TabIndex = 389
        '
        'bgworkSearchAutoComp
        '
        '
        'EmployeeSelection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1203, 545)
        Me.Controls.Add(Me.autcomptxtagency)
        Me.Controls.Add(Me.cbosearch4)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtMName)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.cbosearch5)
        Me.Controls.Add(Me.cbosearch3)
        Me.Controls.Add(Me.cbosearch2)
        Me.Controls.Add(Me.cbosearch1)
        Me.Controls.Add(Me.Label60)
        Me.Controls.Add(Me.Label59)
        Me.Controls.Add(Me.Label58)
        Me.Controls.Add(Me.Label29)
        Me.Controls.Add(Me.txtSName)
        Me.Controls.Add(Me.txtLName)
        Me.Controls.Add(Me.txtFName)
        Me.Controls.Add(Me.txtEmpID)
        Me.Controls.Add(Me.pbEmpPic)
        Me.Controls.Add(Me.First)
        Me.Controls.Add(Me.Prev)
        Me.Controls.Add(Me.Last)
        Me.Controls.Add(Me.Nxt)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSelect)
        Me.Controls.Add(Me.dgvEmployee)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EmployeeSelection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "List of Employee(s)"
        CType(Me.dgvEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbEmpPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvEmployee As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnSelect As System.Windows.Forms.Button
    Friend WithEvents First As System.Windows.Forms.LinkLabel
    Friend WithEvents Prev As System.Windows.Forms.LinkLabel
    Friend WithEvents Last As System.Windows.Forms.LinkLabel
    Friend WithEvents Nxt As System.Windows.Forms.LinkLabel
    Friend WithEvents pbEmpPic As System.Windows.Forms.PictureBox
    Friend WithEvents cbosearch5 As System.Windows.Forms.ComboBox
    Friend WithEvents cbosearch3 As System.Windows.Forms.ComboBox
    Friend WithEvents cbosearch2 As System.Windows.Forms.ComboBox
    Friend WithEvents cbosearch1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents txtSName As System.Windows.Forms.TextBox
    Friend WithEvents txtLName As System.Windows.Forms.TextBox
    Friend WithEvents txtFName As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpID As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cbosearch4 As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtMName As System.Windows.Forms.TextBox
    Friend WithEvents ColRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column13 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column15 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column16 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column17 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column18 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column19 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column20 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column21 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DivisionRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column22 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents autcomptxtagency As Femiani.Forms.UI.Input.AutoCompleteTextBox
    Friend WithEvents bgworkSearchAutoComp As System.ComponentModel.BackgroundWorker
End Class
