<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DepartmentMinWages
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvMinimumWages = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnSaveMinimum = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancelMinimum = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnDelMinimumWage = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel9 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dmw_RowID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dmw_Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dmw_EffectiveDateFrom = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        Me.dmw_EffectiveDateTo = New DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn()
        CType(Me.dgvMinimumWages, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvMinimumWages
        '
        Me.dgvMinimumWages.BackgroundColor = System.Drawing.Color.White
        Me.dgvMinimumWages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMinimumWages.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dmw_RowID, Me.dmw_Amount, Me.dmw_EffectiveDateFrom, Me.dmw_EffectiveDateTo})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvMinimumWages.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvMinimumWages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMinimumWages.GridColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.dgvMinimumWages.Location = New System.Drawing.Point(0, 25)
        Me.dgvMinimumWages.MultiSelect = False
        Me.dgvMinimumWages.Name = "dgvMinimumWages"
        Me.dgvMinimumWages.Size = New System.Drawing.Size(344, 237)
        Me.dgvMinimumWages.TabIndex = 3
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnSaveMinimum, Me.tsbtnCancelMinimum, Me.ToolStripLabel9, Me.tsbtnDelMinimumWage})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(344, 25)
        Me.ToolStrip1.TabIndex = 4
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnSaveMinimum
        '
        Me.tsbtnSaveMinimum.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.tsbtnSaveMinimum.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSaveMinimum.Name = "tsbtnSaveMinimum"
        Me.tsbtnSaveMinimum.Size = New System.Drawing.Size(140, 22)
        Me.tsbtnSaveMinimum.Text = "&Save Minimum Wage"
        '
        'tsbtnCancelMinimum
        '
        Me.tsbtnCancelMinimum.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.tsbtnCancelMinimum.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancelMinimum.Name = "tsbtnCancelMinimum"
        Me.tsbtnCancelMinimum.Size = New System.Drawing.Size(113, 22)
        Me.tsbtnCancelMinimum.Text = "Discard changes"
        '
        'tsbtnDelMinimumWage
        '
        Me.tsbtnDelMinimumWage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnDelMinimumWage.Image = Global.AccuPay.My.Resources.Resources.CLOSE_00
        Me.tsbtnDelMinimumWage.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDelMinimumWage.Name = "tsbtnDelMinimumWage"
        Me.tsbtnDelMinimumWage.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnDelMinimumWage.Text = "ToolStripButton1"
        Me.tsbtnDelMinimumWage.ToolTipText = "Delete Minimum wage"
        '
        'ToolStripLabel9
        '
        Me.ToolStripLabel9.Name = "ToolStripLabel9"
        Me.ToolStripLabel9.Size = New System.Drawing.Size(52, 22)
        Me.ToolStripLabel9.Text = "               "
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "RowID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Amount"
        Me.DataGridViewTextBoxColumn2.MaxInputLength = 12
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'dmw_RowID
        '
        Me.dmw_RowID.HeaderText = "RowID"
        Me.dmw_RowID.Name = "dmw_RowID"
        Me.dmw_RowID.ReadOnly = True
        Me.dmw_RowID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dmw_RowID.Visible = False
        '
        'dmw_Amount
        '
        Me.dmw_Amount.HeaderText = "Amount"
        Me.dmw_Amount.MaxInputLength = 12
        Me.dmw_Amount.Name = "dmw_Amount"
        Me.dmw_Amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'dmw_EffectiveDateFrom
        '
        '
        '
        '
        Me.dmw_EffectiveDateFrom.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dmw_EffectiveDateFrom.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.dmw_EffectiveDateFrom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dmw_EffectiveDateFrom.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.dmw_EffectiveDateFrom.HeaderText = "Effective date from"
        Me.dmw_EffectiveDateFrom.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.dmw_EffectiveDateFrom.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dmw_EffectiveDateFrom.MonthCalendar.BackgroundStyle.Class = ""
        Me.dmw_EffectiveDateFrom.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.dmw_EffectiveDateFrom.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dmw_EffectiveDateFrom.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dmw_EffectiveDateFrom.MonthCalendar.DisplayMonth = New Date(2016, 8, 1, 0, 0, 0, 0)
        Me.dmw_EffectiveDateFrom.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dmw_EffectiveDateFrom.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dmw_EffectiveDateFrom.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dmw_EffectiveDateFrom.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dmw_EffectiveDateFrom.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dmw_EffectiveDateFrom.Name = "dmw_EffectiveDateFrom"
        Me.dmw_EffectiveDateFrom.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dmw_EffectiveDateFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'dmw_EffectiveDateTo
        '
        '
        '
        '
        Me.dmw_EffectiveDateTo.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dmw_EffectiveDateTo.BackgroundStyle.Class = "DataGridViewDateTimeBorder"
        Me.dmw_EffectiveDateTo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dmw_EffectiveDateTo.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText
        Me.dmw_EffectiveDateTo.HeaderText = "Effective date to"
        Me.dmw_EffectiveDateTo.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left
        '
        '
        '
        Me.dmw_EffectiveDateTo.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dmw_EffectiveDateTo.MonthCalendar.BackgroundStyle.Class = ""
        Me.dmw_EffectiveDateTo.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.dmw_EffectiveDateTo.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dmw_EffectiveDateTo.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dmw_EffectiveDateTo.MonthCalendar.DisplayMonth = New Date(2016, 8, 1, 0, 0, 0, 0)
        Me.dmw_EffectiveDateTo.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dmw_EffectiveDateTo.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dmw_EffectiveDateTo.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dmw_EffectiveDateTo.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dmw_EffectiveDateTo.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dmw_EffectiveDateTo.Name = "dmw_EffectiveDateTo"
        Me.dmw_EffectiveDateTo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dmw_EffectiveDateTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DepartmentMinWages
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(344, 262)
        Me.Controls.Add(Me.dgvMinimumWages)
        Me.Controls.Add(Me.ToolStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DepartmentMinWages"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.dgvMinimumWages, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvMinimumWages As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnSaveMinimum As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCancelMinimum As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnDelMinimumWage As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel9 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dmw_RowID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dmw_Amount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dmw_EffectiveDateFrom As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
    Friend WithEvents dmw_EffectiveDateTo As DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn
End Class
