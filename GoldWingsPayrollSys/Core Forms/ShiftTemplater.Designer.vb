<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShiftTemplater
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
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvcalendar = New System.Windows.Forms.DataGridView()
        Me.linkNxt = New System.Windows.Forms.LinkLabel()
        Me.linkPrev = New System.Windows.Forms.LinkLabel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.rdbemployee = New System.Windows.Forms.RadioButton()
        Me.rdbposition = New System.Windows.Forms.RadioButton()
        Me.AutoCompleteTextBox1 = New Femiani.Forms.UI.Input.AutoCompleteTextBox()
        Me.dgvemp = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.echeck = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.dgvpos = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.poscheck = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.bgworkempautocomp = New System.ComponentModel.BackgroundWorker()
        Me.bgworkposautocomp = New System.ComponentModel.BackgroundWorker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSaveDivisionShift = New System.Windows.Forms.Button()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.posRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.posName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eRowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eLName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eFName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eMidName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSun = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colMon = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colWed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colThu = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFri = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSat = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvcalendar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvemp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvpos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvcalendar
        '
        Me.dgvcalendar.AllowUserToAddRows = False
        Me.dgvcalendar.AllowUserToDeleteRows = False
        Me.dgvcalendar.AllowUserToResizeColumns = False
        Me.dgvcalendar.AllowUserToResizeRows = False
        Me.dgvcalendar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvcalendar.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.DimGray
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvcalendar.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvcalendar.ColumnHeadersHeight = 25
        Me.dgvcalendar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvcalendar.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colSun, Me.colMon, Me.colTue, Me.colWed, Me.colThu, Me.colFri, Me.colSat})
        Me.dgvcalendar.Location = New System.Drawing.Point(185, 37)
        Me.dgvcalendar.Name = "dgvcalendar"
        Me.dgvcalendar.ReadOnly = True
        Me.dgvcalendar.RowHeadersVisible = False
        Me.dgvcalendar.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvcalendar.RowTemplate.Height = 95
        Me.dgvcalendar.Size = New System.Drawing.Size(671, 378)
        Me.dgvcalendar.TabIndex = 29
        '
        'linkNxt
        '
        Me.linkNxt.AutoSize = True
        Me.linkNxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkNxt.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkNxt.Location = New System.Drawing.Point(817, 19)
        Me.linkNxt.Name = "linkNxt"
        Me.linkNxt.Size = New System.Drawing.Size(39, 15)
        Me.linkNxt.TabIndex = 2
        Me.linkNxt.TabStop = True
        Me.linkNxt.Text = "Next>"
        '
        'linkPrev
        '
        Me.linkPrev.AutoSize = True
        Me.linkPrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.linkPrev.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.linkPrev.Location = New System.Drawing.Point(182, 19)
        Me.linkPrev.Name = "linkPrev"
        Me.linkPrev.Size = New System.Drawing.Size(38, 15)
        Me.linkPrev.TabIndex = 1
        Me.linkPrev.TabStop = True
        Me.linkPrev.Text = "<Prev"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(283, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(474, 29)
        Me.Label1.TabIndex = 285
        Me.Label1.Text = "Label1"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(781, 421)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 286
        Me.Button1.Text = "Add"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(185, 421)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 286
        Me.Button2.Text = "Reset"
        Me.Button2.UseVisualStyleBackColor = True
        Me.Button2.Visible = False
        '
        'rdbemployee
        '
        Me.rdbemployee.AutoSize = True
        Me.rdbemployee.Checked = True
        Me.rdbemployee.Location = New System.Drawing.Point(337, 489)
        Me.rdbemployee.Name = "rdbemployee"
        Me.rdbemployee.Size = New System.Drawing.Size(117, 17)
        Me.rdbemployee.TabIndex = 287
        Me.rdbemployee.TabStop = True
        Me.rdbemployee.Text = "Apply per employee"
        Me.rdbemployee.UseVisualStyleBackColor = True
        Me.rdbemployee.Visible = False
        '
        'rdbposition
        '
        Me.rdbposition.AutoSize = True
        Me.rdbposition.Location = New System.Drawing.Point(460, 489)
        Me.rdbposition.Name = "rdbposition"
        Me.rdbposition.Size = New System.Drawing.Size(108, 17)
        Me.rdbposition.TabIndex = 288
        Me.rdbposition.Text = "Apply per position"
        Me.rdbposition.UseVisualStyleBackColor = True
        Me.rdbposition.Visible = False
        '
        'AutoCompleteTextBox1
        '
        Me.AutoCompleteTextBox1.Enabled = False
        Me.AutoCompleteTextBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AutoCompleteTextBox1.Location = New System.Drawing.Point(337, 458)
        Me.AutoCompleteTextBox1.Name = "AutoCompleteTextBox1"
        Me.AutoCompleteTextBox1.PopupBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.AutoCompleteTextBox1.PopupOffset = New System.Drawing.Point(12, 0)
        Me.AutoCompleteTextBox1.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.AutoCompleteTextBox1.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.AutoCompleteTextBox1.PopupWidth = 300
        Me.AutoCompleteTextBox1.Size = New System.Drawing.Size(278, 25)
        Me.AutoCompleteTextBox1.TabIndex = 289
        Me.AutoCompleteTextBox1.Visible = False
        '
        'dgvemp
        '
        Me.dgvemp.AllowUserToAddRows = False
        Me.dgvemp.AllowUserToDeleteRows = False
        Me.dgvemp.AllowUserToResizeColumns = False
        Me.dgvemp.AllowUserToResizeRows = False
        Me.dgvemp.BackgroundColor = System.Drawing.Color.White
        Me.dgvemp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvemp.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.eRowID, Me.echeck, Me.eID, Me.eLName, Me.eFName, Me.eMidName})
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvemp.DefaultCellStyle = DataGridViewCellStyle9
        Me.dgvemp.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvemp.Location = New System.Drawing.Point(337, 512)
        Me.dgvemp.MultiSelect = False
        Me.dgvemp.Name = "dgvemp"
        Me.dgvemp.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvemp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvemp.Size = New System.Drawing.Size(369, 81)
        Me.dgvemp.TabIndex = 290
        Me.dgvemp.Visible = False
        '
        'echeck
        '
        Me.echeck.HeaderText = ""
        Me.echeck.Name = "echeck"
        Me.echeck.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.echeck.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.echeck.Width = 18
        '
        'dgvpos
        '
        Me.dgvpos.AllowUserToAddRows = False
        Me.dgvpos.AllowUserToDeleteRows = False
        Me.dgvpos.AllowUserToResizeColumns = False
        Me.dgvpos.AllowUserToResizeRows = False
        Me.dgvpos.BackgroundColor = System.Drawing.Color.White
        Me.dgvpos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvpos.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.posRowID, Me.poscheck, Me.posName})
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvpos.DefaultCellStyle = DataGridViewCellStyle10
        Me.dgvpos.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvpos.Location = New System.Drawing.Point(712, 512)
        Me.dgvpos.MultiSelect = False
        Me.dgvpos.Name = "dgvpos"
        Me.dgvpos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvpos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvpos.Size = New System.Drawing.Size(369, 81)
        Me.dgvpos.TabIndex = 291
        Me.dgvpos.Visible = False
        '
        'poscheck
        '
        Me.poscheck.HeaderText = ""
        Me.poscheck.Name = "poscheck"
        Me.poscheck.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.poscheck.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.poscheck.Width = 18
        '
        'bgworkempautocomp
        '
        Me.bgworkempautocomp.WorkerReportsProgress = True
        Me.bgworkempautocomp.WorkerSupportsCancellation = True
        '
        'bgworkposautocomp
        '
        Me.bgworkposautocomp.WorkerReportsProgress = True
        Me.bgworkposautocomp.WorkerSupportsCancellation = True
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(283, 421)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(474, 29)
        Me.Label2.TabIndex = 292
        Me.Label2.Text = "Label2"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSaveDivisionShift
        '
        Me.btnSaveDivisionShift.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveDivisionShift.Location = New System.Drawing.Point(885, 37)
        Me.btnSaveDivisionShift.Name = "btnSaveDivisionShift"
        Me.btnSaveDivisionShift.Size = New System.Drawing.Size(108, 53)
        Me.btnSaveDivisionShift.TabIndex = 293
        Me.btnSaveDivisionShift.Text = "Add"
        Me.btnSaveDivisionShift.UseVisualStyleBackColor = True
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.Color.DimGray
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle11
        Me.DataGridViewTextBoxColumn1.HeaderText = "SUNDAY"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn1.Width = 95
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.Color.DimGray
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle12
        Me.DataGridViewTextBoxColumn2.HeaderText = "MONDAY"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn2.Width = 96
        '
        'DataGridViewTextBoxColumn3
        '
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.Color.DimGray
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle13
        Me.DataGridViewTextBoxColumn3.HeaderText = "TUESDAY"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn3.Width = 95
        '
        'DataGridViewTextBoxColumn4
        '
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle14.ForeColor = System.Drawing.Color.DimGray
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle14
        Me.DataGridViewTextBoxColumn4.HeaderText = "WEDNESDAY"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn4.Width = 96
        '
        'DataGridViewTextBoxColumn5
        '
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.Color.DimGray
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle15
        Me.DataGridViewTextBoxColumn5.HeaderText = "THURSDAY"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn5.Width = 95
        '
        'DataGridViewTextBoxColumn6
        '
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle16.ForeColor = System.Drawing.Color.DimGray
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle16
        Me.DataGridViewTextBoxColumn6.HeaderText = "FRIDAY"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn6.Width = 96
        '
        'DataGridViewTextBoxColumn7
        '
        DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle17.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle17.ForeColor = System.Drawing.Color.DimGray
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle17
        Me.DataGridViewTextBoxColumn7.HeaderText = "SATURDAY"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn7.Width = 95
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.Visible = False
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.HeaderText = "Employee ID"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "Last Name"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.HeaderText = "First Name"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.HeaderText = "Middle Initial"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        Me.DataGridViewTextBoxColumn13.Visible = False
        '
        'DataGridViewTextBoxColumn14
        '
        Me.DataGridViewTextBoxColumn14.HeaderText = "Position Name"
        Me.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        '
        'posRowID
        '
        Me.posRowID.HeaderText = "RowID"
        Me.posRowID.Name = "posRowID"
        Me.posRowID.Visible = False
        '
        'posName
        '
        Me.posName.HeaderText = "Position Name"
        Me.posName.Name = "posName"
        '
        'eRowID
        '
        Me.eRowID.HeaderText = "RowID"
        Me.eRowID.Name = "eRowID"
        Me.eRowID.Visible = False
        '
        'eID
        '
        Me.eID.HeaderText = "Employee ID"
        Me.eID.Name = "eID"
        '
        'eLName
        '
        Me.eLName.HeaderText = "Last Name"
        Me.eLName.Name = "eLName"
        '
        'eFName
        '
        Me.eFName.HeaderText = "First Name"
        Me.eFName.Name = "eFName"
        '
        'eMidName
        '
        Me.eMidName.HeaderText = "Middle Initial"
        Me.eMidName.Name = "eMidName"
        '
        'colSun
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.DimGray
        Me.colSun.DefaultCellStyle = DataGridViewCellStyle2
        Me.colSun.HeaderText = "SUNDAY"
        Me.colSun.Name = "colSun"
        Me.colSun.ReadOnly = True
        Me.colSun.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colMon
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.DimGray
        Me.colMon.DefaultCellStyle = DataGridViewCellStyle3
        Me.colMon.HeaderText = "MONDAY"
        Me.colMon.Name = "colMon"
        Me.colMon.ReadOnly = True
        Me.colMon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colTue
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.DimGray
        Me.colTue.DefaultCellStyle = DataGridViewCellStyle4
        Me.colTue.HeaderText = "TUESDAY"
        Me.colTue.Name = "colTue"
        Me.colTue.ReadOnly = True
        Me.colTue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colWed
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.DimGray
        Me.colWed.DefaultCellStyle = DataGridViewCellStyle5
        Me.colWed.HeaderText = "WEDNESDAY"
        Me.colWed.Name = "colWed"
        Me.colWed.ReadOnly = True
        Me.colWed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colThu
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.DimGray
        Me.colThu.DefaultCellStyle = DataGridViewCellStyle6
        Me.colThu.HeaderText = "THURSDAY"
        Me.colThu.Name = "colThu"
        Me.colThu.ReadOnly = True
        Me.colThu.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colFri
        '
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.DimGray
        Me.colFri.DefaultCellStyle = DataGridViewCellStyle7
        Me.colFri.HeaderText = "FRIDAY"
        Me.colFri.Name = "colFri"
        Me.colFri.ReadOnly = True
        Me.colFri.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colSat
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.DimGray
        Me.colSat.DefaultCellStyle = DataGridViewCellStyle8
        Me.colSat.HeaderText = "SATURDAY"
        Me.colSat.Name = "colSat"
        Me.colSat.ReadOnly = True
        Me.colSat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'ShiftTemplater
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1010, 463)
        Me.Controls.Add(Me.btnSaveDivisionShift)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dgvpos)
        Me.Controls.Add(Me.dgvemp)
        Me.Controls.Add(Me.AutoCompleteTextBox1)
        Me.Controls.Add(Me.rdbposition)
        Me.Controls.Add(Me.rdbemployee)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.linkNxt)
        Me.Controls.Add(Me.linkPrev)
        Me.Controls.Add(Me.dgvcalendar)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShiftTemplater"
        CType(Me.dgvcalendar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvemp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvpos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvcalendar As System.Windows.Forms.DataGridView
    Friend WithEvents colSun As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colMon As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colWed As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colThu As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFri As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSat As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents linkNxt As System.Windows.Forms.LinkLabel
    Friend WithEvents linkPrev As System.Windows.Forms.LinkLabel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents rdbemployee As System.Windows.Forms.RadioButton
    Friend WithEvents rdbposition As System.Windows.Forms.RadioButton
    Friend WithEvents AutoCompleteTextBox1 As Femiani.Forms.UI.Input.AutoCompleteTextBox
    Friend WithEvents dgvemp As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents dgvpos As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents eRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents echeck As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents eID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eLName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eFName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eMidName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents posRowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents poscheck As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents posName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bgworkempautocomp As System.ComponentModel.BackgroundWorker
    Friend WithEvents bgworkposautocomp As System.ComponentModel.BackgroundWorker
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSaveDivisionShift As System.Windows.Forms.Button
End Class
