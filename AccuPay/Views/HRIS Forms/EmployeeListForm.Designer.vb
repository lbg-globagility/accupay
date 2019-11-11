<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmployeeListForm
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
        Me.dgvemplist = New System.Windows.Forms.DataGridView()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.c_rowid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_EmployeeID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_fname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_mname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_lname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_jobtitle = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_positonid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_hphone = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_mphone = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_wphone = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_empStat = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_bday = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_tinno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_sssno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_hdmfno = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_philhealth = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnOK = New System.Windows.Forms.Button()
        CType(Me.dgvemplist, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvemplist
        '
        Me.dgvemplist.AllowUserToAddRows = False
        Me.dgvemplist.AllowUserToDeleteRows = False
        Me.dgvemplist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvemplist.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_rowid, Me.c_EmployeeID, Me.c_fname, Me.c_mname, Me.c_lname, Me.c_jobtitle, Me.c_positonid, Me.c_hphone, Me.c_mphone, Me.c_wphone, Me.c_empStat, Me.c_bday, Me.c_tinno, Me.c_sssno, Me.c_hdmfno, Me.c_philhealth})
        Me.dgvemplist.Location = New System.Drawing.Point(12, 122)
        Me.dgvemplist.Name = "dgvemplist"
        Me.dgvemplist.ReadOnly = True
        Me.dgvemplist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvemplist.Size = New System.Drawing.Size(865, 301)
        Me.dgvemplist.TabIndex = 0
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(85, 19)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(191, 20)
        Me.TextBox1.TabIndex = 1
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(85, 45)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(191, 20)
        Me.TextBox2.TabIndex = 2
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(85, 71)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(191, 20)
        Me.TextBox3.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Employee ID:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.TextBox3)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(298, 104)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Last Name:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 71)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "First Name:"
        '
        'c_rowid
        '
        Me.c_rowid.HeaderText = "rowID"
        Me.c_rowid.Name = "c_rowid"
        Me.c_rowid.ReadOnly = True
        Me.c_rowid.Visible = False
        '
        'c_EmployeeID
        '
        Me.c_EmployeeID.HeaderText = "Employee ID"
        Me.c_EmployeeID.Name = "c_EmployeeID"
        Me.c_EmployeeID.ReadOnly = True
        Me.c_EmployeeID.Width = 70
        '
        'c_fname
        '
        Me.c_fname.HeaderText = "First Name"
        Me.c_fname.Name = "c_fname"
        Me.c_fname.ReadOnly = True
        '
        'c_mname
        '
        Me.c_mname.HeaderText = "Middle Name"
        Me.c_mname.Name = "c_mname"
        Me.c_mname.ReadOnly = True
        '
        'c_lname
        '
        Me.c_lname.HeaderText = "Last Name"
        Me.c_lname.Name = "c_lname"
        Me.c_lname.ReadOnly = True
        '
        'c_jobtitle
        '
        Me.c_jobtitle.HeaderText = "Job Title"
        Me.c_jobtitle.Name = "c_jobtitle"
        Me.c_jobtitle.ReadOnly = True
        '
        'c_positonid
        '
        Me.c_positonid.HeaderText = "Position Name"
        Me.c_positonid.Name = "c_positonid"
        Me.c_positonid.ReadOnly = True
        '
        'c_hphone
        '
        Me.c_hphone.HeaderText = "Home Phone"
        Me.c_hphone.Name = "c_hphone"
        Me.c_hphone.ReadOnly = True
        '
        'c_mphone
        '
        Me.c_mphone.HeaderText = "Mobile Phone"
        Me.c_mphone.Name = "c_mphone"
        Me.c_mphone.ReadOnly = True
        '
        'c_wphone
        '
        Me.c_wphone.HeaderText = "Work Phone"
        Me.c_wphone.Name = "c_wphone"
        Me.c_wphone.ReadOnly = True
        '
        'c_empStat
        '
        Me.c_empStat.HeaderText = "Employment Status"
        Me.c_empStat.Name = "c_empStat"
        Me.c_empStat.ReadOnly = True
        '
        'c_bday
        '
        Me.c_bday.HeaderText = "Birth Date"
        Me.c_bday.Name = "c_bday"
        Me.c_bday.ReadOnly = True
        '
        'c_tinno
        '
        Me.c_tinno.HeaderText = "TIN No."
        Me.c_tinno.Name = "c_tinno"
        Me.c_tinno.ReadOnly = True
        '
        'c_sssno
        '
        Me.c_sssno.HeaderText = "SSS No."
        Me.c_sssno.Name = "c_sssno"
        Me.c_sssno.ReadOnly = True
        '
        'c_hdmfno
        '
        Me.c_hdmfno.HeaderText = "HDMF No."
        Me.c_hdmfno.Name = "c_hdmfno"
        Me.c_hdmfno.ReadOnly = True
        '
        'c_philhealth
        '
        Me.c_philhealth.HeaderText = "Phil Health No."
        Me.c_philhealth.Name = "c_philhealth"
        Me.c_philhealth.ReadOnly = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(802, 429)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 39)
        Me.btnOK.TabIndex = 6
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'EmployeeListForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(892, 478)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.dgvemplist)
        Me.HelpButton = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EmployeeListForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee List Form"
        CType(Me.dgvemplist, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvemplist As System.Windows.Forms.DataGridView
    Friend WithEvents c_rowid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_EmployeeID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_fname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_mname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_lname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_jobtitle As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_positonid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_hphone As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_mphone As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_wphone As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_empStat As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_bday As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_tinno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_sssno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_hdmfno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_philhealth As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
End Class
