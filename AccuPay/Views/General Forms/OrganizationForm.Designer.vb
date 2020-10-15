<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OrganizationForm
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.FirstPayPeriodGroupBox = New System.Windows.Forms.GroupBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.EndOfTheMonthEndDate = New System.Windows.Forms.DateTimePicker()
        Me.EndOfTheMonthStartDate = New System.Windows.Forms.DateTimePicker()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.FirstHalfEndDate = New System.Windows.Forms.DateTimePicker()
        Me.FirstHalfStartDate = New System.Windows.Forms.DateTimePicker()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.IsAgencyCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.txtZIP = New System.Windows.Forms.TextBox()
        Me.txtRDO = New System.Windows.Forms.TextBox()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.Label70 = New System.Windows.Forms.Label()
        Me.nightdiffshiftto = New System.Windows.Forms.DateTimePicker()
        Me.nightdiffshiftfrom = New System.Windows.Forms.DateTimePicker()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.txtcompEmailTxt = New System.Windows.Forms.TextBox()
        Me.RemoveImageLink = New System.Windows.Forms.LinkLabel()
        Me.BrowseImageButton = New System.Windows.Forms.Button()
        Me.PhotoImages = New System.Windows.Forms.PictureBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtorgTinNumTxt = New System.Windows.Forms.TextBox()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.addAddressLink1 = New System.Windows.Forms.LinkLabel()
        Me.txttradeName = New System.Windows.Forms.TextBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.txtcompUrl = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtcompAltPhoneTxt = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtcompAltEmailTxt = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtcompFaxNumTxt = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtcompMainPhoneTxt = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.AddressComboBox = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtcompanyName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.lblSaveMsg = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.NewButton = New System.Windows.Forms.ToolStripButton()
        Me.SaveButton = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.CancelToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.OrganizationGridView = New System.Windows.Forms.DataGridView()
        Me.SearchTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.c_companyname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.FirstPayPeriodGroupBox.SuspendLayout()
        CType(Me.PhotoImages, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.OrganizationGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.SplitContainer1)
        Me.Panel1.Controls.Add(Me.lblSaveMsg)
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Location = New System.Drawing.Point(270, 24)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1013, 612)
        Me.Panel1.TabIndex = 0
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 25)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.Controls.Add(Me.FirstPayPeriodGroupBox)
        Me.SplitContainer1.Panel1.Controls.Add(Me.IsAgencyCheckBox)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label71)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtZIP)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtRDO)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label68)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label70)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nightdiffshiftto)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nightdiffshiftfrom)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label60)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label59)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label58)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtcompEmailTxt)
        Me.SplitContainer1.Panel1.Controls.Add(Me.RemoveImageLink)
        Me.SplitContainer1.Panel1.Controls.Add(Me.BrowseImageButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.PhotoImages)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label10)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtorgTinNumTxt)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label51)
        Me.SplitContainer1.Panel1.Controls.Add(Me.addAddressLink1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txttradeName)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label32)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtcompUrl)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label9)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtcompAltPhoneTxt)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label8)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtcompAltEmailTxt)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label7)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label6)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtcompFaxNumTxt)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label5)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtcompMainPhoneTxt)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.AddressComboBox)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtcompanyName)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label40)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2Collapsed = True
        Me.SplitContainer1.Size = New System.Drawing.Size(1011, 585)
        Me.SplitContainer1.SplitterDistance = 527
        Me.SplitContainer1.TabIndex = 310
        '
        'FirstPayPeriodGroupBox
        '
        Me.FirstPayPeriodGroupBox.Controls.Add(Me.Label16)
        Me.FirstPayPeriodGroupBox.Controls.Add(Me.Label15)
        Me.FirstPayPeriodGroupBox.Controls.Add(Me.EndOfTheMonthEndDate)
        Me.FirstPayPeriodGroupBox.Controls.Add(Me.EndOfTheMonthStartDate)
        Me.FirstPayPeriodGroupBox.Controls.Add(Me.Label11)
        Me.FirstPayPeriodGroupBox.Controls.Add(Me.FirstHalfEndDate)
        Me.FirstPayPeriodGroupBox.Controls.Add(Me.FirstHalfStartDate)
        Me.FirstPayPeriodGroupBox.Controls.Add(Me.Label12)
        Me.FirstPayPeriodGroupBox.Controls.Add(Me.Label13)
        Me.FirstPayPeriodGroupBox.Controls.Add(Me.Label14)
        Me.FirstPayPeriodGroupBox.Location = New System.Drawing.Point(360, 275)
        Me.FirstPayPeriodGroupBox.Name = "FirstPayPeriodGroupBox"
        Me.FirstPayPeriodGroupBox.Size = New System.Drawing.Size(271, 144)
        Me.FirstPayPeriodGroupBox.TabIndex = 378
        Me.FirstPayPeriodGroupBox.TabStop = False
        Me.FirstPayPeriodGroupBox.Text = "First pay period"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label16.Location = New System.Drawing.Point(149, 102)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(16, 13)
        Me.Label16.TabIndex = 365
        Me.Label16.Text = "to"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label15.Location = New System.Drawing.Point(15, 102)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(27, 13)
        Me.Label15.TabIndex = 364
        Me.Label15.Text = "from"
        '
        'EndOfTheMonthEndDate
        '
        Me.EndOfTheMonthEndDate.CustomFormat = "hh:mm tt"
        Me.EndOfTheMonthEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.EndOfTheMonthEndDate.Location = New System.Drawing.Point(171, 96)
        Me.EndOfTheMonthEndDate.Name = "EndOfTheMonthEndDate"
        Me.EndOfTheMonthEndDate.Size = New System.Drawing.Size(95, 20)
        Me.EndOfTheMonthEndDate.TabIndex = 359
        '
        'EndOfTheMonthStartDate
        '
        Me.EndOfTheMonthStartDate.CustomFormat = "hh:mm tt"
        Me.EndOfTheMonthStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.EndOfTheMonthStartDate.Location = New System.Drawing.Point(48, 96)
        Me.EndOfTheMonthStartDate.Name = "EndOfTheMonthStartDate"
        Me.EndOfTheMonthStartDate.Size = New System.Drawing.Size(95, 20)
        Me.EndOfTheMonthStartDate.TabIndex = 358
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(4, 79)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(88, 13)
        Me.Label11.TabIndex = 363
        Me.Label11.Text = "End of the month"
        Me.Label11.Visible = False
        '
        'FirstHalfEndDate
        '
        Me.FirstHalfEndDate.CustomFormat = "hh:mm tt"
        Me.FirstHalfEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.FirstHalfEndDate.Location = New System.Drawing.Point(171, 45)
        Me.FirstHalfEndDate.Name = "FirstHalfEndDate"
        Me.FirstHalfEndDate.Size = New System.Drawing.Size(95, 20)
        Me.FirstHalfEndDate.TabIndex = 357
        '
        'FirstHalfStartDate
        '
        Me.FirstHalfStartDate.CustomFormat = "hh:mm tt"
        Me.FirstHalfStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.FirstHalfStartDate.Location = New System.Drawing.Point(48, 45)
        Me.FirstHalfStartDate.Name = "FirstHalfStartDate"
        Me.FirstHalfStartDate.Size = New System.Drawing.Size(95, 20)
        Me.FirstHalfStartDate.TabIndex = 356
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label12.Location = New System.Drawing.Point(149, 51)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(16, 13)
        Me.Label12.TabIndex = 362
        Me.Label12.Text = "to"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label13.Location = New System.Drawing.Point(15, 51)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(27, 13)
        Me.Label13.TabIndex = 361
        Me.Label13.Text = "from"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(4, 28)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(48, 13)
        Me.Label14.TabIndex = 360
        Me.Label14.Text = "First Half"
        '
        'IsAgencyCheckBox
        '
        Me.IsAgencyCheckBox.AutoSize = True
        Me.IsAgencyCheckBox.Location = New System.Drawing.Point(186, 307)
        Me.IsAgencyCheckBox.Name = "IsAgencyCheckBox"
        Me.IsAgencyCheckBox.Size = New System.Drawing.Size(15, 14)
        Me.IsAgencyCheckBox.TabIndex = 377
        Me.IsAgencyCheckBox.UseVisualStyleBackColor = True
        '
        'Label71
        '
        Me.Label71.AutoSize = True
        Me.Label71.Location = New System.Drawing.Point(15, 307)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(57, 13)
        Me.Label71.TabIndex = 376
        Me.Label71.Text = "Is Agency:"
        '
        'txtZIP
        '
        Me.txtZIP.Location = New System.Drawing.Point(187, 353)
        Me.txtZIP.MaxLength = 8
        Me.txtZIP.Name = "txtZIP"
        Me.txtZIP.Size = New System.Drawing.Size(76, 20)
        Me.txtZIP.TabIndex = 375
        '
        'txtRDO
        '
        Me.txtRDO.Location = New System.Drawing.Point(187, 327)
        Me.txtRDO.MaxLength = 8
        Me.txtRDO.Name = "txtRDO"
        Me.txtRDO.Size = New System.Drawing.Size(76, 20)
        Me.txtRDO.TabIndex = 374
        '
        'Label68
        '
        Me.Label68.AutoSize = True
        Me.Label68.Location = New System.Drawing.Point(15, 360)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(58, 13)
        Me.Label68.TabIndex = 373
        Me.Label68.Text = "ZIP Code :"
        '
        'Label70
        '
        Me.Label70.AutoSize = True
        Me.Label70.Location = New System.Drawing.Point(15, 334)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(65, 13)
        Me.Label70.TabIndex = 372
        Me.Label70.Text = "RDO Code :"
        '
        'nightdiffshiftto
        '
        Me.nightdiffshiftto.CustomFormat = "hh:mm tt"
        Me.nightdiffshiftto.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.nightdiffshiftto.Location = New System.Drawing.Point(531, 231)
        Me.nightdiffshiftto.Name = "nightdiffshiftto"
        Me.nightdiffshiftto.ShowUpDown = True
        Me.nightdiffshiftto.Size = New System.Drawing.Size(95, 20)
        Me.nightdiffshiftto.TabIndex = 323
        '
        'nightdiffshiftfrom
        '
        Me.nightdiffshiftfrom.CustomFormat = "hh:mm tt"
        Me.nightdiffshiftfrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.nightdiffshiftfrom.Location = New System.Drawing.Point(408, 231)
        Me.nightdiffshiftfrom.Name = "nightdiffshiftfrom"
        Me.nightdiffshiftfrom.ShowUpDown = True
        Me.nightdiffshiftfrom.Size = New System.Drawing.Size(95, 20)
        Me.nightdiffshiftfrom.TabIndex = 322
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label60.Location = New System.Drawing.Point(509, 237)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(16, 13)
        Me.Label60.TabIndex = 352
        Me.Label60.Text = "to"
        '
        'Label59
        '
        Me.Label59.AutoSize = True
        Me.Label59.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label59.Location = New System.Drawing.Point(375, 237)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(27, 13)
        Me.Label59.TabIndex = 351
        Me.Label59.Text = "from"
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(364, 214)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(105, 13)
        Me.Label58.TabIndex = 350
        Me.Label58.Text = "Night differential shift"
        '
        'txtcompEmailTxt
        '
        Me.txtcompEmailTxt.Location = New System.Drawing.Point(18, 230)
        Me.txtcompEmailTxt.Name = "txtcompEmailTxt"
        Me.txtcompEmailTxt.Size = New System.Drawing.Size(245, 20)
        Me.txtcompEmailTxt.TabIndex = 314
        '
        'RemoveImageLink
        '
        Me.RemoveImageLink.AutoSize = True
        Me.RemoveImageLink.Location = New System.Drawing.Point(843, 213)
        Me.RemoveImageLink.Name = "RemoveImageLink"
        Me.RemoveImageLink.Size = New System.Drawing.Size(79, 13)
        Me.RemoveImageLink.TabIndex = 341
        Me.RemoveImageLink.TabStop = True
        Me.RemoveImageLink.Text = "Remove Image"
        '
        'BrowseImageButton
        '
        Me.BrowseImageButton.Location = New System.Drawing.Point(751, 210)
        Me.BrowseImageButton.Name = "BrowseImageButton"
        Me.BrowseImageButton.Size = New System.Drawing.Size(75, 23)
        Me.BrowseImageButton.TabIndex = 338
        Me.BrowseImageButton.Text = "Browse"
        Me.BrowseImageButton.UseVisualStyleBackColor = True
        '
        'PhotoImages
        '
        Me.PhotoImages.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.PhotoImages.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PhotoImages.Location = New System.Drawing.Point(727, 39)
        Me.PhotoImages.Name = "PhotoImages"
        Me.PhotoImages.Size = New System.Drawing.Size(215, 165)
        Me.PhotoImages.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PhotoImages.TabIndex = 340
        Me.PhotoImages.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(728, 23)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(42, 13)
        Me.Label10.TabIndex = 339
        Me.Label10.Text = "Image :"
        '
        'txtorgTinNumTxt
        '
        Me.txtorgTinNumTxt.Location = New System.Drawing.Point(367, 113)
        Me.txtorgTinNumTxt.Name = "txtorgTinNumTxt"
        Me.txtorgTinNumTxt.Size = New System.Drawing.Size(214, 20)
        Me.txtorgTinNumTxt.TabIndex = 318
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Location = New System.Drawing.Point(364, 96)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(71, 13)
        Me.Label51.TabIndex = 335
        Me.Label51.Text = "TIN Number :"
        '
        'addAddressLink1
        '
        Me.addAddressLink1.AutoSize = True
        Me.addAddressLink1.Location = New System.Drawing.Point(269, 120)
        Me.addAddressLink1.Name = "addAddressLink1"
        Me.addAddressLink1.Size = New System.Drawing.Size(92, 13)
        Me.addAddressLink1.TabIndex = 312
        Me.addAddressLink1.TabStop = True
        Me.addAddressLink1.Text = "Add New Address"
        '
        'txttradeName
        '
        Me.txttradeName.Location = New System.Drawing.Point(18, 73)
        Me.txttradeName.Name = "txttradeName"
        Me.txttradeName.Size = New System.Drawing.Size(245, 20)
        Me.txttradeName.TabIndex = 310
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(15, 57)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(72, 13)
        Me.Label32.TabIndex = 332
        Me.Label32.Text = "Trade Name :"
        '
        'txtcompUrl
        '
        Me.txtcompUrl.Location = New System.Drawing.Point(367, 34)
        Me.txtcompUrl.Name = "txtcompUrl"
        Me.txtcompUrl.Size = New System.Drawing.Size(214, 20)
        Me.txtcompUrl.TabIndex = 316
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(364, 17)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 13)
        Me.Label9.TabIndex = 328
        Me.Label9.Text = "URL :"
        '
        'txtcompAltPhoneTxt
        '
        Me.txtcompAltPhoneTxt.Location = New System.Drawing.Point(367, 73)
        Me.txtcompAltPhoneTxt.Name = "txtcompAltPhoneTxt"
        Me.txtcompAltPhoneTxt.Size = New System.Drawing.Size(214, 20)
        Me.txtcompAltPhoneTxt.TabIndex = 317
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(364, 57)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(89, 13)
        Me.Label8.TabIndex = 327
        Me.Label8.Text = "Alternate Phone :"
        '
        'txtcompAltEmailTxt
        '
        Me.txtcompAltEmailTxt.Location = New System.Drawing.Point(367, 152)
        Me.txtcompAltEmailTxt.Name = "txtcompAltEmailTxt"
        Me.txtcompAltEmailTxt.Size = New System.Drawing.Size(214, 20)
        Me.txtcompAltEmailTxt.TabIndex = 319
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(364, 136)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(124, 13)
        Me.Label7.TabIndex = 326
        Me.Label7.Text = "Alternate Email Address :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(15, 214)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(79, 13)
        Me.Label6.TabIndex = 325
        Me.Label6.Text = "Email Address :"
        '
        'txtcompFaxNumTxt
        '
        Me.txtcompFaxNumTxt.Location = New System.Drawing.Point(18, 191)
        Me.txtcompFaxNumTxt.Name = "txtcompFaxNumTxt"
        Me.txtcompFaxNumTxt.Size = New System.Drawing.Size(245, 20)
        Me.txtcompFaxNumTxt.TabIndex = 313
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 175)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 13)
        Me.Label5.TabIndex = 324
        Me.Label5.Text = "Fax Number :"
        '
        'txtcompMainPhoneTxt
        '
        Me.txtcompMainPhoneTxt.Location = New System.Drawing.Point(18, 152)
        Me.txtcompMainPhoneTxt.Name = "txtcompMainPhoneTxt"
        Me.txtcompMainPhoneTxt.Size = New System.Drawing.Size(245, 20)
        Me.txtcompMainPhoneTxt.TabIndex = 312
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 136)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 13)
        Me.Label3.TabIndex = 323
        Me.Label3.Text = "Main Phone :"
        '
        'AddressComboBox
        '
        Me.AddressComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AddressComboBox.FormattingEnabled = True
        Me.AddressComboBox.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.AddressComboBox.Location = New System.Drawing.Point(18, 112)
        Me.AddressComboBox.Name = "AddressComboBox"
        Me.AddressComboBox.Size = New System.Drawing.Size(245, 21)
        Me.AddressComboBox.TabIndex = 311
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 322
        Me.Label1.Text = "Primary Address :"
        '
        'txtcompanyName
        '
        Me.txtcompanyName.Location = New System.Drawing.Point(18, 34)
        Me.txtcompanyName.Name = "txtcompanyName"
        Me.txtcompanyName.Size = New System.Drawing.Size(245, 20)
        Me.txtcompanyName.TabIndex = 309
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(88, 13)
        Me.Label4.TabIndex = 320
        Me.Label4.Text = "Company Name :"
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label40.ForeColor = System.Drawing.Color.Red
        Me.Label40.Location = New System.Drawing.Point(100, 15)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(18, 24)
        Me.Label40.TabIndex = 321
        Me.Label40.Text = "*"
        '
        'lblSaveMsg
        '
        Me.lblSaveMsg.AutoSize = True
        Me.lblSaveMsg.Location = New System.Drawing.Point(58, 25)
        Me.lblSaveMsg.Name = "lblSaveMsg"
        Me.lblSaveMsg.Size = New System.Drawing.Size(0, 13)
        Me.lblSaveMsg.TabIndex = 309
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewButton, Me.SaveButton, Me.btnDelete, Me.CancelToolStripButton, Me.btnClose})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1011, 25)
        Me.ToolStrip1.TabIndex = 303
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'NewButton
        '
        Me.NewButton.Image = Global.AccuPay.My.Resources.Resources._new
        Me.NewButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewButton.Name = "NewButton"
        Me.NewButton.Size = New System.Drawing.Size(51, 22)
        Me.NewButton.Text = "&New"
        '
        'SaveButton
        '
        Me.SaveButton.Image = Global.AccuPay.My.Resources.Resources.Save
        Me.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(51, 22)
        Me.SaveButton.Text = "&Save"
        '
        'btnDelete
        '
        Me.btnDelete.Image = Global.AccuPay.My.Resources.Resources.deleteuser
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(60, 22)
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.Visible = False
        '
        'CancelToolStripButton
        '
        Me.CancelToolStripButton.Image = Global.AccuPay.My.Resources.Resources.cancel1
        Me.CancelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CancelToolStripButton.Name = "CancelToolStripButton"
        Me.CancelToolStripButton.Size = New System.Drawing.Size(63, 22)
        Me.CancelToolStripButton.Text = "&Cancel"
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.Image = Global.AccuPay.My.Resources.Resources.Button_Delete_icon
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 22)
        Me.btnClose.Text = "&Close"
        '
        'OrganizationGridView
        '
        Me.OrganizationGridView.AllowUserToAddRows = False
        Me.OrganizationGridView.AllowUserToDeleteRows = False
        Me.OrganizationGridView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.OrganizationGridView.BackgroundColor = System.Drawing.Color.White
        Me.OrganizationGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.OrganizationGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.c_companyname})
        Me.OrganizationGridView.Location = New System.Drawing.Point(9, 83)
        Me.OrganizationGridView.Name = "OrganizationGridView"
        Me.OrganizationGridView.ReadOnly = True
        Me.OrganizationGridView.Size = New System.Drawing.Size(255, 554)
        Me.OrganizationGridView.TabIndex = 303
        '
        'SearchTextBox
        '
        Me.SearchTextBox.Location = New System.Drawing.Point(9, 55)
        Me.SearchTextBox.Name = "SearchTextBox"
        Me.SearchTextBox.Size = New System.Drawing.Size(255, 20)
        Me.SearchTextBox.TabIndex = 304
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 305
        Me.Label2.Text = "Search"
        '
        'Label35
        '
        Me.Label35.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(228, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Label35.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label35.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label35.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label35.Location = New System.Drawing.Point(0, 0)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(1288, 24)
        Me.Label35.TabIndex = 312
        Me.Label35.Text = "ORGANIZATION"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "Name"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Organization Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 200
        '
        'c_companyname
        '
        Me.c_companyname.DataPropertyName = "Name"
        Me.c_companyname.HeaderText = "Organization Name"
        Me.c_companyname.Name = "c_companyname"
        Me.c_companyname.ReadOnly = True
        Me.c_companyname.Width = 200
        '
        'OrganizationForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(132, Byte), Integer), CType(CType(204, Byte), Integer), CType(CType(196, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1288, 645)
        Me.Controls.Add(Me.Label35)
        Me.Controls.Add(Me.SearchTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.OrganizationGridView)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.HelpButton = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OrganizationForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Organization Form"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.FirstPayPeriodGroupBox.ResumeLayout(False)
        Me.FirstPayPeriodGroupBox.PerformLayout()
        CType(Me.PhotoImages, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.OrganizationGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents OrganizationGridView As System.Windows.Forms.DataGridView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents NewButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SaveButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents CancelToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SearchTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblSaveMsg As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents txtcompEmailTxt As System.Windows.Forms.TextBox
    Friend WithEvents RemoveImageLink As System.Windows.Forms.LinkLabel
    Friend WithEvents BrowseImageButton As System.Windows.Forms.Button
    Friend WithEvents PhotoImages As System.Windows.Forms.PictureBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtorgTinNumTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents addAddressLink1 As System.Windows.Forms.LinkLabel
    Friend WithEvents txttradeName As System.Windows.Forms.TextBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents txtcompUrl As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtcompAltPhoneTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtcompAltEmailTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtcompFaxNumTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtcompMainPhoneTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents AddressComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents txtcompanyName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents nightdiffshiftfrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents nightdiffshiftto As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtZIP As System.Windows.Forms.TextBox
    Friend WithEvents txtRDO As System.Windows.Forms.TextBox
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents IsAgencyCheckBox As CheckBox
    Friend WithEvents Label71 As Label
    Friend WithEvents c_companyname As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents FirstPayPeriodGroupBox As GroupBox
    Friend WithEvents EndOfTheMonthEndDate As DateTimePicker
    Friend WithEvents EndOfTheMonthStartDate As DateTimePicker
    Friend WithEvents Label11 As Label
    Friend WithEvents FirstHalfEndDate As DateTimePicker
    Friend WithEvents FirstHalfStartDate As DateTimePicker
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label15 As Label
End Class
