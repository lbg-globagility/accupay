Imports System.Threading

Public Class LoginForm

    Dim n_CountDownExpiration As CountDownExpiration

    Protected Overrides Sub OnLoad(e As EventArgs)

        'dbconn()

        'Try

        '    n_CountDownExpiration = New CountDownExpiration

        'Catch ex As Exception
        '    MsgBox(getErrExcptn(ex, Me.Name))

        'End Try


        'n_FileObserver.Detect()

        'Dim nt_CountDownExpiration = New Thread(AddressOf CountDownExpiration)

        'nt_CountDownExpiration.IsBackground = True

        'nt_CountDownExpiration.Start()

        MyBase.OnLoad(e)

    End Sub

    Sub fillPosition()
        Dim strQuery As String = "select Name from organization WHERE NoPurpose='0';"
        cmbBranchName.Items.Clear()
        cmbBranchName.Items.Add("-Select One-")
        cmbBranchName.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbBranchName.SelectedIndex = 0

    End Sub

    Function loadImage(ByVal branch As String) As Boolean

        PhotoImages.Image = Nothing

        Dim dt As New DataTable
        dt = execute("select Image from organization where Name = '" & branch & "'")
        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                With dr
                    Try
                        PhotoImages.Image = ConvertByteToImage(.Item("Image"))
                    Catch ex As Exception

                    End Try
                End With
            Next

        Else
            PhotoImages.Image = Nothing

        End If

        Return True

    End Function
    Private Function SecurityFormAllow(ByVal Menuname As String) As Boolean

        Dim postid As String = getStringItem("Select PositionID From user where userid = '" & EncrypedData(UsernameTextBox.Text) & "' And Password = '" & EncrypedData(PasswordTextBox.Text) & "'")
        Dim getpostid As Integer = Val(postid)
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * from position_view pv inner join position p on pv.PositionID = p.RowID " & _
                                "inner join `view` v on pv.ViewID = v.RowID " & _
                                "Where  pv.OrganizationID = " & z_OrganizationID & " " & _
                                "And p.RowID = '" & getpostid & "' And v.ViewName = '" & Menuname & "'")

        For Each drow As DataRow In dt.Rows
            If drow.Item("ViewName") = Menuname Then
                If drow.Item("Readonly").ToString = "Y" Then
                    'z_read = True
                Else
                    'z_read = False
                End If

            End If
        Next
        Return True
    End Function
    Public Sub allowforms()
        'With MDImain
        '    If SecurityFormAllow(.UserPrivilege.Text) Then
        '        .UserPrivilege.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.UsersToolStripMenuItem.Text) Then
        '        .UsersToolStripMenuItem.Visible = z_read
        '        .cmdCreateUser.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.ListOfValueToolStripMenuItem1.Text) Then
        '        .ListOfValueToolStripMenuItem1.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.OrganizationToolStripMenuItem1.Text) Then
        '        .OrganizationToolStripMenuItem1.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.AuditTrailToolStripMenuItem1.Text) Then
        '        .AuditTrailToolStripMenuItem1.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.ChangePasswordToolStripMenuItem.Text) Then
        '        .ChangePasswordToolStripMenuItem.Visible = z_read
        '        .cmdChangePassword.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.PurchaseRequisitionToolStripMenuItem1.Text) Then
        '        .PurchaseRequisitionToolStripMenuItem1.Visible = z_read
        '        '.lblNewPO.Visible = z_read
        '        '.lblListPO.Visible = z_read
        '        '.lblPurchaseList.Visible = z_read
        '        '.lblNewPurchase.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.PurchaseRequisitionForCEOToolStripMenuItem1.Text) Then
        '        .PurchaseRequisitionForCEOToolStripMenuItem1.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.PurchaseRequisitionForGeneralManagerToolStripMenuItem1.Text) Then
        '        .PurchaseRequisitionForGeneralManagerToolStripMenuItem1.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.MerchandiseRequestFormForOfficeStaffToolStripMenuItem.Text) Then
        '        '.lblMerchandiseList.Visible = z_read
        '        '.lblMRText.Visible = z_read
        '        '.lblNewMerchandise.Visible = z_read
        '        '.lblnewMerchandisetext.Visible = z_read
        '        .MerchandiseRequestFormForOfficeStaffToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.MerchandiseRequestFormForSalesCoordinatorToolStripMenuItem.Text) Then

        '        .MerchandiseRequestFormForSalesCoordinatorToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.MerchandiseRequestFormForWareHouseMngrToolStripMenuItem.Text) Then

        '        .MerchandiseRequestFormForWareHouseMngrToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.ReceivingReportToolStripMenuItem2.Text) Then

        '        .ReceivingReportToolStripMenuItem2.Visible = z_read
        '        '.lblReceiveText.Visible = z_read
        '        '.lblReceivingList.Visible = z_read
        '        '.lblNewReceiving.Visible = z_read
        '        '.lblRlisttext.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.PackingListToolStripMenuItem.Text) Then

        '        .PackingListToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.SupplierFormToolStripMenuItem1.Text) Then

        '        .SupplierFormToolStripMenuItem1.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.ProductListToolStripMenuItem.Text) Then
        '        .lblnewprodtext.Visible = z_read
        '        .lblNewProduct.Visible = z_read
        '        .lblprodlisttext.Visible = z_read
        '        .lblProductList.Visible = z_read
        '        .ProductListToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.AdjustStockToolStripMenuItem.Text) Then
        '        .AdjustStockToolStripMenuItem.Visible = z_read
        '        .lblAdjustStock.Visible = z_read
        '        .lblAdjustText.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.TransferStockToolStripMenuItem.Text) Then
        '        .lblTransfer.Visible = z_read
        '        .lbltransfertext.Visible = z_read
        '        .TransferStockToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.ApprovedToolStripMenuItem.Text) Then
        '        .ApprovedToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.InventoryLocationToolStripMenuItem.Text) Then
        '        .lblCurrentStock.Visible = z_read
        '        .lblCurrText.Visible = z_read
        '        .InventoryLocationToolStripMenuItem.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.MovementHistoryToolStripMenuItem.Text) Then
        '        .MovementHistoryToolStripMenuItem.Visible = z_read
        '        .lblMovementhistory.Visible = z_read
        '        .lblmovetext.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.ReorderPointToolStripMenuItem.Text) Then
        '        .MovementHistoryToolStripMenuItem.Visible = z_read
        '        .lblReorderStock.Visible = z_read
        '        .lblreordertext.Visible = z_read

        '    End If
        '    If SecurityFormAllow(.CustomerOrderToolStripMenuItem.Text) Then

        '        .CustomerOrderToolStripMenuItem.Visible = z_read
        '        .lblNewOrder.Visible = z_read
        '        .lblnewordertext.Visible = z_read
        '        .lblOrderList.Visible = z_read
        '        .lblorderlisttext.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.DailySalesReportToolStripMenuItem.Text) Then
        '        .DailySalesReportToolStripMenuItem.Visible = z_read
        '    End If
        '    'If SecurityFormAllow(.AllBranchToolStripMenuItem.Text) Then
        '    '    .AllBranchToolStripMenuItem.Visible = z_read
        '    'End If
        '    If SecurityFormAllow(.ListOfDeliveryToolStripMenuItem.Text) Then
        '        .lblDRlist.Visible = z_read
        '        .lblDRtext.Visible = z_read
        '        .ListOfDeliveryToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.NewDeliveryToolStripMenuItem.Text) Then


        '        .lblDRequestText.Visible = z_read
        '        .lblDeliveryRequest.Visible = z_read
        '        .NewDeliveryToolStripMenuItem.Visible = z_read

        '    End If
        '    If SecurityFormAllow(.PurchaseRequisitionDataEntryToolStripMenuItem.Text) Then
        '        .PurchaseRequisitionDataEntryToolStripMenuItem.Visible = z_read
        '        '.lblPurchaseList.Visible = z_read

        '    End If
        '    If SecurityFormAllow(.MerchandiseRequisitionToolStripMenuItem.Text) Then
        '        .MerchandiseRequisitionToolStripMenuItem.Visible = z_read

        '    End If
        '    If SecurityFormAllow(.CountSheetToolStripMenuItem1.Text) Then
        '        .CountSheetToolStripMenuItem1.Visible = z_read
        '        .lblCycleCount.Visible = z_read
        '        .lblcycletext.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.TransferStockToolStripMenuItem1.Text) Then
        '        .TransferStockToolStripMenuItem1.Visible = z_read
        '        .lblTransfer.Visible = z_read
        '        .lbltransfertext.Visible = z_read

        '    End If
        '    If SecurityFormAllow(.DailySalesReportToolStripMenuItem.Text) Then
        '        .DailySalesReportToolStripMenuItem.Visible = z_read

        '    End If

        '    If SecurityFormAllow(.SendingEmailAddressToolStripMenuItem.Text) Then
        '        .SendingEmailAddressToolStripMenuItem.Visible = z_read
        '        .lblSendEmail.Visible = z_read
        '        .lblSendEmailText.Visible = z_read

        '    End If
        '    If SecurityFormAllow(.InventoryReportBySupplierToolStripMenuItem.Text) Then
        '        .InventoryReportBySupplierToolStripMenuItem.Visible = z_read
        '        .lblInventoryReport.Visible = z_read
        '        .lblinvtext.Visible = z_read

        '    End If

        '    If SecurityFormAllow(.SalesReportByBranchToolStripMenuItem.Text) Then
        '        .SalesReportByBranchToolStripMenuItem.Visible = z_read
        '        .lblSalesreport.Visible = z_read
        '        .lblsalesreporttext.Visible = z_read

        '    End If

        '    If SecurityFormAllow(.PurchaseReportToolStripMenuItem.Text) Then
        '        .PurchaseReportToolStripMenuItem.Visible = z_read
        '        .lblPurchaseReport.Visible = z_read
        '        .lblprreport.Visible = z_read

        '    End If

        '    If SecurityFormAllow(.TotalSalesReportPerBranchToolStripMenuItem.Text) Then
        '        .TotalSalesReportPerBranchToolStripMenuItem.Visible = z_read
        '        .lblTotSales.Visible = z_read
        '        .lbltotsalestext.Visible = z_read

        '    End If

        '    If SecurityFormAllow(.BankListToolStripMenuItem.Text) Then
        '        .BankListToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.RefundToolStripMenuItem.Text) Then
        '        .RefundToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.ReceivingReportToolStripMenuItem2.Text) Then
        '        .ReceivingReportToolStripMenuItem2.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.ReceivingReportForCEOToolStripMenuItem1.Text) Then
        '        .ReceivingReportForCEOToolStripMenuItem1.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.ReceivingReportForGMToolStripMenuItem.Text) Then
        '        .ReceivingReportForGMToolStripMenuItem.Visible = z_read

        '    End If
        '    If SecurityFormAllow(.PackingListForCEOToolStripMenuItem.Text) Then
        '        .PackingListForCEOToolStripMenuItem.Visible = z_read
        '        '.lblNewPl.Visible = z_read
        '        '.lblPLtext.Visible = z_read
        '        '.lblPLListtxt.Visible = z_read
        '        '.lblPLList.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.PackingListForGeneralManagerToolStripMenuItem.Text) Then
        '        .PackingListForGeneralManagerToolStripMenuItem.Visible = z_read
        '        '.lblNewPl.Visible = z_read
        '        '.lblPLtext.Visible = z_read
        '        '.lblPLListtxt.Visible = z_read
        '        '.lblPLList.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.PackingListToolStripMenuItem1.Text) Then
        '        .PackingListToolStripMenuItem1.Visible = z_read
        '        '.lblNewPl.Visible = z_read
        '        '.lblPLtext.Visible = z_read
        '        '.lblPLListtxt.Visible = z_read
        '        '.lblPLList.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.RepairsToolStripMenuItem1.Text) Then
        '        .RepairsToolStripMenuItem1.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.ReorderPointToolStripMenuItem.Text) Then
        '        .ReorderPointToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.ProductAgingReportToolStripMenuItem.Text) Then
        '        .ProductAgingReportToolStripMenuItem.Visible = z_read
        '        .lblProdAgingrpt.Visible = z_read
        '        .lblpartext.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.SupplierShipmentReportToolStripMenuItem.Text) Then
        '        .SupplierShipmentReportToolStripMenuItem.Visible = z_read
        '        .lblSupplierShipRPT.Visible = z_read
        '        .lblssrtext.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.InventoryReportBySupplierToolStripMenuItem.Text) Then
        '        .InventoryReportBySupplierToolStripMenuItem.Visible = z_read
        '        .lblInventoryReport.Visible = z_read
        '        .lblinvtext.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.SalesReportByBranchToolStripMenuItem.Text) Then
        '        .SalesReportByBranchToolStripMenuItem.Visible = z_read
        '        .lblsalesreporttext.Visible = z_read
        '        .lblSalesreport.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.PurchaseReportToolStripMenuItem.Text) Then
        '        .PurchaseReportToolStripMenuItem.Visible = z_read
        '        .lblPurchaseReport.Visible = z_read
        '        .lblprreport.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.TotalSalesReportPerBranchToolStripMenuItem.Text) Then
        '        .TotalSalesReportPerBranchToolStripMenuItem.Visible = z_read
        '        .lbltotsalestext.Visible = z_read
        '        .lblTotSales.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.PurchasingToolStripMenuItem.Text) Then
        '        .PurchasingToolStripMenuItem.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.ReceivingToolStripMenuItem.Text) Then
        '        .ReceivingToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.InventoryToolStripMenuItem.Text) Then
        '        .InventoryToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.MerchandiseRequisitionToolStripMenuItem.Text) Then
        '        .InventoryToolStripMenuItem.Visible = z_read
        '    End If
        '    If SecurityFormAllow(.DeliveryToolStripMenuItem.Text) Then
        '        .DeliveryToolStripMenuItem.Visible = z_read
        '    End If

        '    If SecurityFormAllow(.GeneralToolStripMenuItem.Text) Then
        '        .GeneralToolStripMenuItem.Visible = z_read
        '    End If
        'End With




    End Sub
    Dim ctr As Integer = 0

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click

        If Trim(UsernameTextBox.Text) = "" Or Trim(PasswordTextBox.Text) = "" Then
            'MsgBox("Please enter your Username And Password.", MsgBoxStyle.Exclamation, "")
            WarnBalloon("Please enter your Username And Password.", "Invalid login", OK, OK.Width - 18, -69)
            If Trim(UsernameTextBox.Text) = "" Then
                UsernameTextBox.Focus()
            ElseIf Trim(PasswordTextBox.Text) = "" Then
                PasswordTextBox.Focus()
            End If

            Exit Sub
        ElseIf cmbBranchName.Text = "-Select One-" Then
            WarnBalloon("Please select a company.", "Invalid Company", cmbBranchName, cmbBranchName.Width - 18, -69)

            'MsgBox("Please select a  Company.", MsgBoxStyle.Exclamation, "")
            cmbBranchName.Focus()
            Exit Sub
        Else
            Dim org As String = getStringItem("Select RowID from Organization where name = '" & cmbBranchName.Text & "'")
            z_OrganizationID = Val(org)

            Dim user As String = getStringItem("Select RowID From user where UserID = '" & EncrypedData(UsernameTextBox.Text) & "' And Password = '" & EncrypedData(PasswordTextBox.Text) & "';")
            ' And OrganizationID = '" & z_OrganizationID & "'
            z_User = Val(user)

            Dim pName As String = getStringItem("select p.PositionName from position p inner join user u on u.RowID = '" & z_User & "' where p.RowID = u.PositionID;")
            'getStringItem("select p.PositionName from position p inner join user u on p.RowID = u.PositionID where u.OrganizationID = '" & z_OrganizationID & "' and u.UserID = '" & EncrypedData(UsernameTextBox.Text) & "' and u.Password = '" & EncrypedData(PasswordTextBox.Text) & "'")
            z_postName = pName
            z_CompanyName = cmbBranchName.Text

            Dim ds As New DataSet
            ds = getDataSetForSQL("Select * From Organization Where Name = '" & cmbBranchName.Text & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                Dim addrid As String
                Dim getaddrid As Integer
                For Each drow As DataRow In ds.Tables(0).Rows
                    addrid = drow.Item("PrimaryAddressID").ToString
                    getaddrid = Val(addrid)
                    If getaddrid = 0 Then
                        z_CompanyAddr = ""
                    Else
                        Dim compaddr As String = getStringItem("Select CONCAT(COALESCE(StreetAddress1,' '), COALESCE(Barangay,' '), COALESCE(CityTown,' '), COALESCE(State,' ')) As Addresses from address Where RowID = '" & getaddrid & "'")
                        z_CompanyAddr = compaddr
                    End If
                Next
            Else
                z_CompanyAddr = ""
            End If

            Dim dt As New DataTable
            'dt = getDataTableForSQL("Select * From User u Inner join Organization o on u.OrganizationID = o.RowID Where u.UserID = '" & EncrypedData(UsernameTextBox.Text.Trim).ToString & "' And u.Password = '" & EncrypedData(PasswordTextBox.Text.Trim).ToString & "' And o.Name = '" & cmbBranchName.Text & "'")
            dt = getDataTableForSQL("Select * From User u Inner join Organization o on u.OrganizationID = o.RowID Where u.RowID = '" & z_User & "';")

            Dim pos As String = pName ' getStringItem("select pos.PositionName From Position pos inner join user u on pos.RowID = u.PositionID Where u.UserID = '" & z_User & "'")
            'getStringItem("select pos.PositionName From Position pos inner join user u on pos.RowID = u.PositionID Where u.UserID = '" & EncrypedData(UsernameTextBox.Text) & "'")

            Dim getpos As String = pos

            If ctr = 3 Then

                Me.Close()
            ElseIf ctr <= 2 Then
                If dt.Rows.Count > 0 Then

                    Dim uname As String = DecrypedData(dt.Rows(0)("UserID").ToString)
                    Dim passname As String = DecrypedData(dt.Rows(0)("Password").ToString)
                    Dim passn2 As String = EncrypedData(PasswordTextBox.Text)
                    Dim passn As String = EncrypedData2(passname)
                    Dim usname As String = EncrypedData(uname)
                    Dim usname1 As String = EncrypedData2(UsernameTextBox.Text)

                    If passn2 = passn And usname = usname1 Then

                        If Val(z_User) = 0 Then

                        Else

                            Dim userFNameLName = EXECQUER("SELECT CONCAT(COALESCE(FirstName,'.'),',',COALESCE(LastName,'.')) FROM user WHERE RowID=" & z_User & ";")

                            Dim splitFNameLName = Split(userFNameLName, ",")

                            userFirstName = splitFNameLName(0).ToString.Replace(".", "")

                            If userFirstName = "" Then
                            Else
                                userFirstName = StrConv(userFirstName, VbStrConv.ProperCase)
                            End If

                            userLastName = splitFNameLName(1).ToString.Replace(".", "")

                            If userLastName = "" Then
                            Else
                                userLastName = StrConv(userLastName, VbStrConv.ProperCase)
                            End If

                        End If


                        If dbnow = Nothing Then
                            dbnow = EXECQUER(CURDATE_MDY)
                        End If


                        Static freq As Integer = -1
                        If cmbBranchName.SelectedIndex <> -1 Then
                            orgNam = cmbBranchName.SelectedItem 'Text'SelectedItem

                            orgztnID = EXECQUER(SYS_ORGZTN_ID & orgNam & "'")

                            numofdaysthisyear = EXECQUER("SELECT DAYOFYEAR(LAST_DAY(CONCAT(YEAR(CURRENT_DATE()),'-12-01')));")

                            If freq <> orgztnID Then
                                freq = orgztnID

                            End If

                            position_view_table = retAsDatTbl("SELECT *" & _
                                                              " FROM position_view" & _
                                                              " WHERE PositionID=(SELECT PositionID FROM user WHERE RowID=" & z_User & ")" & _
                                                              " AND OrganizationID='" & orgztnID & "';")

                            Dim i = position_view_table.Rows.Count

                            If i = 0 Then

                            End If

                        End If

                        'If getpos = "Administrator" Then
                        ''Dim n_MDIPrimaryForm As New MDIPrimaryForm
                        MDIPrimaryForm.Show()
                        '    ctr = 0
                        'Else
                        '    'allowforms()
                        '    MDIPrimaryForm.Show()
                        '    ctr = 0
                        'End If
                        Me.Hide()
                    Else

                        WarnBalloon("Either username or password is mismatched.", "Incorrect account", OK, OK.Width - 18, -69)

                        'MsgBox("Incorrect Login Fields!", MsgBoxStyle.Critical, "LOGIN FAILED")
                        UsernameTextBox.Clear()
                        PasswordTextBox.Clear()
                        UsernameTextBox.Focus()
                        ctr += 1
                    End If
                Else

                    WarnBalloon("Either username or password is mismatched.", "Incorrect account", OK, OK.Width - 18, -69)

                    'MsgBox("Incorrect Login Fields!", MsgBoxStyle.Critical, "LOGIN FAILED")
                    UsernameTextBox.Clear()
                    PasswordTextBox.Clear()
                    UsernameTextBox.Focus()
                    ctr += 1
                End If
            End If

        End If


        'Dim org As String = getStringItem("Select RowID from Organization where name = '" & cmbBranchName.Text & "'")
        'Z_OrganizationID = Val(org)

        'Dim user As String = getStringItem("Select RowID From user where UserID = '" & EncrypedData(UsernameTextBox.Text) & "' And Password = '" & EncrypedData(PasswordTextBox.Text) & "'")
        'Z_User = Val(user)

        'Dim pName As String = getStringItem("select p.PositionName from position p inner join user u on p.RowID = u.PositionID where u.OrganizationID = '" & Z_OrganizationID & "' and u.UserID = '" & EncrypedData(UsernameTextBox.Text) & "' and u.Password = '" & EncrypedData(PasswordTextBox.Text) & "'")
        'Z_postName = pName
        'z_CompanyName = cmbBranchName.Text


        'Dim dtA As DataTable = execute("Select * From Organization o inner join Address a on o.PrimaryAddressID = a.RowID Where o.Name = '" & cmbBranchName.Text & "'")
        'If dtA.Rows.Count > 0 Then
        '    z_CompanyAddr = ""
        'Else
        '    Dim st1, st2, city, country, state As String
        '    st1 = dtA.Rows(0)("StreetAddress1").ToString
        '    st2 = dtA.Rows(0)("StreetAddress2").ToString
        '    city = dtA.Rows(0)("CityTown").ToString
        '    country = dtA.Rows(0)("Country").ToString
        '    state = dtA.Rows(0)("State").ToString

        '    z_CompanyAddr = st1 + " " + st2 + " " + city + " " + country + " " + state
        'End If


        'Dim dt As New DataTable
        'dt = getDataTableForSQL("Select * From User u Inner join Organization o on u.OrganizationID = o.RowID Where u.UserID = '" & EncrypedData(UsernameTextBox.Text).ToString.ToUpper.Trim & "' And u.Password = '" & EncrypedData(PasswordTextBox.Text).ToString.ToUpper.Trim & "' And o.Name = '" & cmbBranchName.Text & "'")
        'Dim pos As String = getStringItem("select pos.PositionName From Position pos inner join user u on pos.RowID = u.PositionID Where u.UserID = '" & EncrypedData(UsernameTextBox.Text) & "'")
        'Dim getpos As String = pos
        'If UsernameTextBox.Text = "" And PasswordTextBox.Text = "" Then
        '    MsgBox("Please Enter Your UserID And Password.", MsgBoxStyle.Exclamation, "")
        'ElseIf cmbBranchName.Text = "-Select One-" Then
        '    MsgBox("Select Company.", MsgBoxStyle.Exclamation, "")
        '    Exit Sub
        'Else
        '    If ctr = 3 Then
        '        Me.Close()
        '    ElseIf ctr <= 2 Then

        '        If dt.Rows.Count > 0 Then
        '            If getpos = "Administrator" Then
        '                'MDImain.Show()
        '                MDIPrimaryForm.Show()

        '                ctr = 0
        '                Me.Hide()
        '            Else
        '                allowforms()
        '                'MDImain.Show()
        '                MDIPrimaryForm.Show()

        '                ctr = 0
        '                Me.Hide()
        '            End If
        '        Else
        '            MsgBox("Incorrect Login Fields!", MsgBoxStyle.Critical, "LOGIN FAILED")
        '            UsernameTextBox.Clear()
        '            PasswordTextBox.Clear()
        '            UsernameTextBox.Focus()
        '            ctr += 1
        '        End If
        '    End If
        'End If

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click

        'TrialForm.Show()

        'Application.Exit()

        'Dim n_ListOfValFrm As New ListOfValFrm

        'n_ListOfValFrm.Show()

    End Sub

    Private Sub LoginForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        WarnBalloon(, , OK, , , 1)

        WarnBalloon(, , cmbBranchName, , , 1)

        'n_FileObserver.Undetect()

        Application.Exit()
    End Sub

    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'PasswordTextBox.ContextMenu = New ContextMenu

        If getCount("Select count(RowID) from user LIMIT 1;") = 0 Then

            Thread.Sleep(1500)

            Application.Exit()

        End If

        fillPosition()

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        ForgotPasswordForm.Show()

    End Sub

    Private Sub cmbBranchName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBranchName.SelectedIndexChanged
        loadImage(cmbBranchName.Text)

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim n_LeaveForm As New LeaveForm

        With n_LeaveForm

            .CboListOfValue1.Visible = False

            .Label3.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim n_OverTimeForm As New OverTimeForm

        With n_OverTimeForm

            .cboOTStatus.Visible = False

            .Label186.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim n_OBFForm As New OBFForm

        With n_OBFForm

            .cboOBFStatus.Visible = False

            .Label186.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub PasswordTextBox_TextChanged(sender As Object, e As EventArgs) Handles PasswordTextBox.TextChanged

    End Sub

    Private Sub UsernameTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles UsernameTextBox.KeyPress
        'MsgBox(Asc(e.KeyChar))
    End Sub

    Private Sub UsernameTextBox_TextChanged(sender As Object, e As EventArgs) Handles UsernameTextBox.TextChanged

    End Sub

End Class
