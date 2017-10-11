<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Revised_Withholding_Tax_Tables
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Revised_Withholding_Tax_Tables))
        Me.dgvlisttaxableamt = New System.Windows.Forms.DataGridView()
        Me.cmbPayType = New System.Windows.Forms.ComboBox()
        Me.dgvfilingstatus = New System.Windows.Forms.DataGridView()
        Me.c_FilingStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_maritalStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_Dependent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_FID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNewTax = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSaveTax = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnimportwtax = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancelTax = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCloseTax = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnAudittrail = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.bgworkimporttax = New System.ComponentModel.BackgroundWorker()
        Me.c_Taxincomeframt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_taxincometoamt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_ExemptionAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_ExemptionExcessAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_rowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvlisttaxableamt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvfilingstatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvlisttaxableamt
        '
        Me.dgvlisttaxableamt.AllowUserToAddRows = False
        Me.dgvlisttaxableamt.AllowUserToDeleteRows = False
        Me.dgvlisttaxableamt.BackgroundColor = System.Drawing.Color.White
        Me.dgvlisttaxableamt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvlisttaxableamt.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_Taxincomeframt, Me.c_taxincometoamt, Me.c_ExemptionAmount, Me.c_ExemptionExcessAmount, Me.c_rowID})
        Me.dgvlisttaxableamt.Location = New System.Drawing.Point(375, 61)
        Me.dgvlisttaxableamt.MultiSelect = False
        Me.dgvlisttaxableamt.Name = "dgvlisttaxableamt"
        Me.dgvlisttaxableamt.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvlisttaxableamt.Size = New System.Drawing.Size(463, 420)
        Me.dgvlisttaxableamt.TabIndex = 0
        '
        'cmbPayType
        '
        Me.cmbPayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPayType.FormattingEnabled = True
        Me.cmbPayType.Location = New System.Drawing.Point(100, 34)
        Me.cmbPayType.Name = "cmbPayType"
        Me.cmbPayType.Size = New System.Drawing.Size(144, 21)
        Me.cmbPayType.TabIndex = 1
        '
        'dgvfilingstatus
        '
        Me.dgvfilingstatus.AllowUserToAddRows = False
        Me.dgvfilingstatus.AllowUserToDeleteRows = False
        Me.dgvfilingstatus.AllowUserToOrderColumns = True
        Me.dgvfilingstatus.BackgroundColor = System.Drawing.Color.White
        Me.dgvfilingstatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvfilingstatus.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_FilingStatus, Me.c_maritalStatus, Me.c_Dependent, Me.c_FID})
        Me.dgvfilingstatus.Location = New System.Drawing.Point(22, 61)
        Me.dgvfilingstatus.MultiSelect = False
        Me.dgvfilingstatus.Name = "dgvfilingstatus"
        Me.dgvfilingstatus.ReadOnly = True
        Me.dgvfilingstatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvfilingstatus.Size = New System.Drawing.Size(347, 420)
        Me.dgvfilingstatus.TabIndex = 2
        '
        'c_FilingStatus
        '
        Me.c_FilingStatus.HeaderText = "Filing Status"
        Me.c_FilingStatus.Name = "c_FilingStatus"
        Me.c_FilingStatus.ReadOnly = True
        '
        'c_maritalStatus
        '
        Me.c_maritalStatus.HeaderText = "Marital Status"
        Me.c_maritalStatus.Name = "c_maritalStatus"
        Me.c_maritalStatus.ReadOnly = True
        '
        'c_Dependent
        '
        Me.c_Dependent.HeaderText = "Dependent"
        Me.c_Dependent.Name = "c_Dependent"
        Me.c_Dependent.ReadOnly = True
        '
        'c_FID
        '
        Me.c_FID.HeaderText = "RowID"
        Me.c_FID.Name = "c_FID"
        Me.c_FID.ReadOnly = True
        Me.c_FID.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(375, 32)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        Me.btnSave.Visible = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNewTax, Me.tsbtnSaveTax, Me.tsbtnimportwtax, Me.tsbtnCancelTax, Me.tsbtnCloseTax, Me.tsbtnAudittrail, Me.ToolStripProgressBar1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1065, 25)
        Me.ToolStrip1.TabIndex = 4
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnNewTax
        '
        Me.tsbtnNewTax.Image = Global.Acupay.My.Resources.Resources._new
        Me.tsbtnNewTax.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNewTax.Name = "tsbtnNewTax"
        Me.tsbtnNewTax.Size = New System.Drawing.Size(72, 22)
        Me.tsbtnNewTax.Text = "&New Tax"
        '
        'tsbtnSaveTax
        '
        Me.tsbtnSaveTax.Image = Global.Acupay.My.Resources.Resources.Save
        Me.tsbtnSaveTax.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveTax.Name = "tsbtnSaveTax"
        Me.tsbtnSaveTax.Size = New System.Drawing.Size(72, 22)
        Me.tsbtnSaveTax.Text = "&Save Tax"
        '
        'tsbtnimportwtax
        '
        Me.tsbtnimportwtax.Image = CType(resources.GetObject("tsbtnimportwtax.Image"), System.Drawing.Image)
        Me.tsbtnimportwtax.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnimportwtax.Name = "tsbtnimportwtax"
        Me.tsbtnimportwtax.Size = New System.Drawing.Size(150, 22)
        Me.tsbtnimportwtax.Text = "Import Withholding tax"
        '
        'tsbtnCancelTax
        '
        Me.tsbtnCancelTax.Image = Global.Acupay.My.Resources.Resources.cancel1
        Me.tsbtnCancelTax.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancelTax.Name = "tsbtnCancelTax"
        Me.tsbtnCancelTax.Size = New System.Drawing.Size(63, 22)
        Me.tsbtnCancelTax.Text = "Cancel"
        '
        'tsbtnCloseTax
        '
        Me.tsbtnCloseTax.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnCloseTax.Image = Global.Acupay.My.Resources.Resources.Button_Delete_icon
        Me.tsbtnCloseTax.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCloseTax.Name = "tsbtnCloseTax"
        Me.tsbtnCloseTax.Size = New System.Drawing.Size(56, 22)
        Me.tsbtnCloseTax.Text = "Close"
        '
        'tsbtnAudittrail
        '
        Me.tsbtnAudittrail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnAudittrail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnAudittrail.Image = Global.Acupay.My.Resources.Resources.audit_trail_icon
        Me.tsbtnAudittrail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnAudittrail.Name = "tsbtnAudittrail"
        Me.tsbtnAudittrail.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnAudittrail.Text = "ToolStripButton1"
        Me.tsbtnAudittrail.ToolTipText = "Show audit trails"
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 22)
        Me.ToolStripProgressBar1.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Pay frequency"
        '
        'Label25
        '
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.Label25.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label25.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label25.Location = New System.Drawing.Point(0, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(1065, 22)
        Me.Label25.TabIndex = 101
        Me.Label25.Text = "WITHHOLDING TAX TABLE"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Controls.Add(Me.dgvlisttaxableamt)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmbPayType)
        Me.Panel1.Controls.Add(Me.btnSave)
        Me.Panel1.Controls.Add(Me.dgvfilingstatus)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 22)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1065, 485)
        Me.Panel1.TabIndex = 102
        '
        'bgworkimporttax
        '
        Me.bgworkimporttax.WorkerReportsProgress = True
        Me.bgworkimporttax.WorkerSupportsCancellation = True
        '
        'c_Taxincomeframt
        '
        Me.c_Taxincomeframt.HeaderText = "Taxable income from amount"
        Me.c_Taxincomeframt.Name = "c_Taxincomeframt"
        '
        'c_taxincometoamt
        '
        Me.c_taxincometoamt.HeaderText = "Taxable income to amount"
        Me.c_taxincometoamt.Name = "c_taxincometoamt"
        '
        'c_ExemptionAmount
        '
        Me.c_ExemptionAmount.HeaderText = "Exemption Amount"
        Me.c_ExemptionAmount.Name = "c_ExemptionAmount"
        '
        'c_ExemptionExcessAmount
        '
        Me.c_ExemptionExcessAmount.HeaderText = "Exemption Excess Amount"
        Me.c_ExemptionExcessAmount.Name = "c_ExemptionExcessAmount"
        '
        'c_rowID
        '
        Me.c_rowID.HeaderText = "RowID"
        Me.c_rowID.Name = "c_rowID"
        Me.c_rowID.Visible = False
        '
        'Revised_Withholding_Tax_Tables
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1065, 507)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label25)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Revised_Withholding_Tax_Tables"
        Me.Text = "REVISED WITHHOLDING TAX TABLES"
        CType(Me.dgvlisttaxableamt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvfilingstatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvlisttaxableamt As System.Windows.Forms.DataGridView
    Friend WithEvents cmbPayType As System.Windows.Forms.ComboBox
    Friend WithEvents dgvfilingstatus As System.Windows.Forms.DataGridView
    Friend WithEvents c_FilingStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_maritalStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_Dependent As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_FID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnNewTax As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnSaveTax As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancelTax As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCloseTax As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tsbtnAudittrail As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents tsbtnimportwtax As System.Windows.Forms.ToolStripButton
    Friend WithEvents bgworkimporttax As System.ComponentModel.BackgroundWorker
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents c_Taxincomeframt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_taxincometoamt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_ExemptionAmount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_ExemptionExcessAmount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_rowID As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
