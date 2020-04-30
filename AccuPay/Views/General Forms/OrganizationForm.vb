Imports System.IO

Public Class OrganizationForm
    Dim payFreq As New AutoCompleteStringCollection
    Dim isNew As Integer = 0

    Private Sub cleartextbox()
        txtcompAltEmailTxt.Clear()
        txtcompAltPhoneTxt.Clear()
        txtcompanyName.Clear()
        txtcompEmailTxt.Clear()
        txtcompFaxNumTxt.Clear()
        txtcompMainPhoneTxt.Clear()
        txtcompUrl.Clear()
        txtcontAltPhoneTxt.Clear()
        txtcontFaxNumTxt.Clear()
        txtcontMainPhoneTxt.Clear()
        txtemailAddTxt.Clear()
        txtfirstNameTxt.Clear()
        txtjobTitleTxt.Clear()
        txtlastNameTxt.Clear()
        txtmiddleNameTxt.Clear()
        txtmobilePhoneTxt.Clear()
        txtnickNameTxt.Clear()
        txtorgTinNumTxt.Clear()
        txtsuffixTxt.Clear()
        txttinNumTxt.Clear()
        txttradeName.Clear()
        txtworkPhoneTxt.Clear()
        cmbaddressCB.SelectedIndex = -1
        cmbEmpFlag.SelectedIndex = -1
        cmbgenderCB.SelectedIndex = -1
        cmborganizationTypeCB.SelectedIndex = -1
        cmbprimaryAddress.SelectedIndex = -1
        cmbsalutationCB.SelectedIndex = -1
        cmbstatusCB.SelectedIndex = -1
        cmbtypeCB.SelectedIndex = -1
        PhotoImages.Image = Nothing

        IsAgencyCheckBox.Checked = False

    End Sub

    Sub fillorganizationtype()
        Dim strQuery As String = "select DisplayValue from ListOfVal Where Type = 'Organization Type'"
        cmborganizationTypeCB.Items.Clear()
        cmborganizationTypeCB.Items.Add("")
        cmborganizationTypeCB.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmborganizationTypeCB.SelectedIndex = 0
    End Sub

    Sub filltype()
        Dim strQuery As String = "select DisplayValue from ListOfVal Where Type = 'Type'"
        cmbtypeCB.Items.Clear()
        cmbtypeCB.Items.Add("")
        cmbtypeCB.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbtypeCB.SelectedIndex = 0
    End Sub

    'concat(streetaddress1,' ', coalesce(streetaddress2,''),',',' ',citytown,',',' ',country)
    Sub filladdress()
        'Dim strQuery As String = "select concat(streetaddress1,' ', streetaddress2,'',',',' ',citytown,',',' ',country) from Address"
        'cmbaddressCB.Items.Clear()
        'cmbaddressCB.Items.Add("")
        'cmbaddressCB.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        'cmbaddressCB.SelectedIndex = 0
        cmbaddressCB.Items.Clear()

        fillCombobox("select distinct(concat(a.StreetAddress1,' ', a.StreetAddress2,'',',',' ',a.CityTown,',',' ',a.Country)) from address a ", cmbaddressCB)

        cmbprimaryAddress.Items.Clear()

        fillCombobox("select distinct(concat(a.StreetAddress1,' ', a.StreetAddress2,'',',',' ',a.CityTown,',',' ',a.Country)) from address a", cmbprimaryAddress)
    End Sub

    Sub fillstatus()
        Dim strQuery As String = "select DisplayValue from ListOfVal Where Type = 'Status'"
        cmbstatusCB.Items.Clear()
        cmbstatusCB.Items.Add("")
        cmbstatusCB.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbstatusCB.SelectedIndex = 0
    End Sub

    Sub fillpersonalstatus()
        Dim strQuery As String = "select Distinct(DisplayValue) from ListOfVal Where Type = 'Salutation'"
        cmbsalutationCB.Items.Clear()
        cmbsalutationCB.Items.Add("")
        cmbsalutationCB.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbsalutationCB.SelectedIndex = 0
    End Sub

    Private Sub fillOrganizationList(Optional refreshForm As Boolean = True)
        Dim dt As New DataTable
        dt = getDataTableForSQL("select * from organization o LEFT join address a on o.PrimaryAddressID = a.RowID " &
                                "LEFT join contact c on o.PrimaryContactID = c.RowID WHERE o.NoPurpose='0' ORDER BY o.Name;")

        ' where o.RowID = " & z_OrganizationID & "

        dgvCompanyList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvCompanyList.Rows.Add()
            With drow
                dgvCompanyList.Rows.Item(n).Cells(c_companyname.Index).Value = .Item("Name").ToString
                dgvCompanyList.Rows.Item(n).Cells(c_rowID.Index).Value = .Item("RowID").ToString
                dgvCompanyList.Rows.Item(n).Cells(c_ContactID.Index).Value = .Item("PrimaryContactID").ToString
                dgvCompanyList.Rows.Item(n).Cells(c_AddressID.Index).Value = .Item("PrimaryAddressID").ToString
            End With
        Next

        If refreshForm Then

            fillorgitems()

        End If

    End Sub

    Private Sub fillorgitems()
        If dgvCompanyList.Rows.Count = 0 Then
            cbopayfreq.SelectedIndex = -1
            txtvlallow.Text = ""
            txtslallow.Text = ""
            txtmlallow.Text = ""
            txtotherallow.Text = ""

            cbophhdeductsched.Text = ""
            cbosssdeductsched.Text = ""
            cbohdmfdeductsched.Text = ""
            cboTaxDeductSched.Text = ""

            txtmindayperyear.Text = ""

            txtRDO.Text = ""

            txtZIP.Text = ""
            address_RowID = Nothing

            IsAgencyCheckBox.Checked = False
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("select *,COALESCE(CONCAT(CURRENT_DATE(),TIME_FORMAT(NightShiftTimeFrom,' %h:%i %p')),'') 'nightshiftstart'" &
                                    ",COALESCE(CONCAT(CURRENT_DATE(),TIME_FORMAT(NightShiftTimeTo,' %h:%i %p')),'') 'nightshiftend'" &
                                    ",COALESCE(CONCAT(CURRENT_DATE(),TIME_FORMAT(NightDifferentialTimeFrom,' %h:%i %p')),'') 'ndiffshiftstart'" &
                                    ",COALESCE(CONCAT(CURRENT_DATE(),TIME_FORMAT(NightDifferentialTimeTo,' %h:%i %p')),'') 'ndiffshiftend'" &
                                    " from organization o LEFT join address a on o.PrimaryAddressID = a.RowID " &
                                    "LEFT join contact c on o.PrimaryContactID = c.RowID where o.RowID = '" & dgvCompanyList.CurrentRow.Cells(c_rowID.Index).Value & "'")

            'organizationid = " & z_OrganizationID & " and

            For Each drow As DataRow In dt.Rows

                With drow
                    'concat(streetaddress1,' ', coalesce(streetaddress2,''),',',' ',citytown,',',' ',country)
                    Dim rowid As Integer = .Item("RowID").ToString

                    Dim addid As String = .Item("AddressID").ToString
                    Dim primaddid As String = .Item("PrimaryAddressID").ToString
                    Dim addrID As String = getStringItem("Select concat(streetaddress1,' ', coalesce(streetaddress2,''),',',' ',citytown,',',' ',country) from address where rowid = '" & Val(addid) & "'")
                    Dim getaddrid As String = addrID
                    Dim addrIDprim As String = getStringItem("Select concat(streetaddress1,' ', coalesce(streetaddress2,''),',',' ',citytown,',',' ',country) from address where rowid = '" & Val(primaddid) & "'")
                    Dim getaddridprim As String = addrIDprim

                    Dim tinno As String = getStringItem("Select Tinno from Organization where rowid = '" & Val(rowid) & "'")
                    Dim gettinno As String = tinno

                    txtcompAltEmailTxt.Text = .Item("AltEmailAddress").ToString
                    txtcompAltPhoneTxt.Text = .Item("AltPhone").ToString
                    txtcompanyName.Text = .Item("Name").ToString
                    txtcompEmailTxt.Text = .Item("EmailAddress").ToString
                    txtcompFaxNumTxt.Text = .Item("FaxNumber").ToString
                    txtcompMainPhoneTxt.Text = .Item("MainPhone").ToString
                    txtcompUrl.Text = .Item("URL").ToString
                    txtcontAltPhoneTxt.Text = .Item("AlternatePhone").ToString
                    txtcontFaxNumTxt.Text = .Item("FaxNumber").ToString
                    txtcontMainPhoneTxt.Text = .Item("MainPhone").ToString
                    txtemailAddTxt.Text = .Item("EmailAddress").ToString
                    txtfirstNameTxt.Text = .Item("FirstName").ToString
                    txtjobTitleTxt.Text = .Item("JobTitle").ToString
                    txtlastNameTxt.Text = .Item("LastName").ToString
                    txtmiddleNameTxt.Text = .Item("MiddleName").ToString
                    txtmobilePhoneTxt.Text = .Item("MobilePhone").ToString
                    txtnickNameTxt.Text = .Item("Nickname").ToString
                    txtorgTinNumTxt.Text = tinno
                    txtsuffixTxt.Text = .Item("Suffix").ToString
                    txttinNumTxt.Text = .Item("TINNumber").ToString
                    txttradeName.Text = .Item("TradeName").ToString
                    txtworkPhoneTxt.Text = .Item("WorkPhone").ToString
                    cmbaddressCB.Text = getaddrid
                    cmbprimaryAddress.Text = getaddridprim
                    cmbEmpFlag.Text = .Item("EmployeeFlg").ToString
                    cmbgenderCB.Text = .Item("Gender").ToString
                    cmborganizationTypeCB.Text = .Item("OrganizationType").ToString

                    cmbsalutationCB.Text = .Item("Personaltitle").ToString
                    cmbstatusCB.Text = .Item("Status").ToString
                    cmbtypeCB.Text = .Item("Type").ToString

                    IsAgencyCheckBox.Checked = Not (String.IsNullOrWhiteSpace(.Item("IsAgency")) OrElse .Item("IsAgency") = 0)

                    'PhotoImages.Image = Nothing
                    Try
                        Dim img As Object = .Item("Image")
                        If IsDBNull(img) Then
                            PhotoImages.Image = Nothing
                        Else
                            PhotoImages.Image = ConvertByteToImage(.Item("Image"))
                        End If
                    Catch ex As Exception
                    End Try

                    If IsDBNull(.Item("PayFrequencyID")) Then
                        cbopayfreq.SelectedIndex = -1
                    Else
                        cbopayfreq.Text = EXECQUER("SELECT PayFrequencyType FROM payfrequency WHERE RowID='" & .Item("PayFrequencyID").ToString & "';")
                    End If

                    txtvlallow.Text = If(IsDBNull(.Item("VacationLeaveDays")), 0, .Item("VacationLeaveDays").ToString)
                    txtslallow.Text = If(IsDBNull(.Item("SickLeaveDays")), 0, .Item("SickLeaveDays").ToString)
                    txtmlallow.Text = If(IsDBNull(.Item("MaternityLeaveDays")), 0, .Item("MaternityLeaveDays").ToString)
                    txtotherallow.Text = If(IsDBNull(.Item("OthersLeaveDays")), 0, .Item("OthersLeaveDays").ToString)

                    cbophhdeductsched.Text = If(IsDBNull(.Item("PhilhealthDeductionSchedule")), "", .Item("PhilhealthDeductionSchedule").ToString)
                    If IsDBNull(.Item("PhilhealthDeductionSchedule")) = False Then
                        If .Item("PhilhealthDeductionSchedule").ToString = "" Then
                            cbophhdeductsched.SelectedIndex = -1
                        End If
                    Else
                        cbophhdeductsched.SelectedIndex = -1
                    End If

                    cbosssdeductsched.Text = If(IsDBNull(.Item("SSSDeductionSchedule")), "", .Item("SSSDeductionSchedule").ToString)
                    If IsDBNull(.Item("SSSDeductionSchedule")) = False Then
                        If .Item("SSSDeductionSchedule").ToString = "" Then
                            cbosssdeductsched.SelectedIndex = -1
                        End If
                    Else
                        cbosssdeductsched.SelectedIndex = -1
                    End If

                    cbohdmfdeductsched.Text = If(IsDBNull(.Item("PagIbigDeductionSchedule")), "", .Item("PagIbigDeductionSchedule").ToString)
                    If IsDBNull(.Item("PagIbigDeductionSchedule")) = False Then
                        If .Item("PagIbigDeductionSchedule").ToString = "" Then
                            cbohdmfdeductsched.SelectedIndex = -1
                        End If
                    Else
                        cbohdmfdeductsched.SelectedIndex = -1
                    End If

                    cboTaxDeductSched.Text = If(IsDBNull(.Item("WithholdingDeductionSchedule")), "", .Item("WithholdingDeductionSchedule").ToString)
                    If IsDBNull(.Item("WithholdingDeductionSchedule")) = False Then
                        If .Item("WithholdingDeductionSchedule").ToString = "" Then
                            cboTaxDeductSched.SelectedIndex = -1
                        End If
                    Else
                        cboTaxDeductSched.SelectedIndex = -1
                    End If

                    Dim currenttimestamp = EXECQUER("SELECT CURRENT_TIMESTAMP();")

                    If .Item("ndiffshiftstart").ToString <> "" Then
                        nightdiffshiftfrom.Value = .Item("ndiffshiftstart")
                    Else
                        nightdiffshiftfrom.Value = Format(CDate(currenttimestamp), "hh:mm tt")
                    End If

                    If .Item("ndiffshiftend").ToString <> "" Then
                        nightdiffshiftto.Value = .Item("ndiffshiftend")
                    Else
                        nightdiffshiftto.Value = Format(CDate(currenttimestamp), "hh:mm tt")
                    End If

                    If .Item("nightshiftstart").ToString <> "" Then
                        nightshiftfrom.Value = .Item("nightshiftstart") 'Format(CDate(.Item("nightshiftstart")), "MM-dd-yyyy hh:mm tt")
                    Else
                        nightshiftfrom.Value = Format(CDate(currenttimestamp), "hh:mm tt")
                    End If

                    If .Item("nightshiftend").ToString <> "" Then
                        nightshiftto.Value = .Item("nightshiftend") 'Format(CDate(.Item("nightshiftend")), "MM-dd-yyyy hh:mm tt")
                    Else
                        nightshiftto.Value = Format(CDate(currenttimestamp), "hh:mm tt")
                    End If

                    txtmindayperyear.Text = .Item("WorkDaysPerYear").ToString

                    txtRDO.Text = .Item("RDOCode").ToString

                    txtZIP.Text = .Item("ZIPCode").ToString

                    address_RowID = If(IsDBNull(.Item("PrimaryAddressID")), Nothing, ValNoComma(.Item("PrimaryAddressID")))

                End With

            Next

        End If

    End Sub

    'Private Sub addAddressLink1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles addAddressLink1.Click
    '    address.ShowDialog()

    'End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        isNew = 1
        cleartextbox()
        btnSave.Enabled = True
        dgvCompanyList.Enabled = False
        btnNew.Enabled = False

        txtcompanyName.Focus()

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        isNew = 0
        cleartextbox()
        'btnSave.Enabled = False
        dgvCompanyList.Enabled = True
        btnNew.Enabled = True
    End Sub

    Dim dontUpdate As SByte = 0

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Z_ErrorProvider.Dispose()
        If Not SetWarningIfEmpty(txtcompanyName) Then

            Exit Sub
        End If

        If isNew = 1 Then

            Dim caddrID As String = getStringItem("Select max(RoWID) from address where concat(StreetAddress1,' ', StreetAddress2,'',',',' ',CityTown,',',' ',Country) = '" & cmbaddressCB.Text & "'")
            Dim getcaddrid As Integer = Val(caddrID)

            If cmbaddressCB.SelectedIndex = -1 Then
                getcaddrid = 0
            End If

            I_contact(cmbstatusCB.Text, z_datetime, z_OrganizationID, txtcontMainPhoneTxt.Text, txtlastNameTxt.Text, txtfirstNameTxt.Text,
                  txtmiddleNameTxt.Text, txtmobilePhoneTxt.Text, txtworkPhoneTxt.Text, cmbgenderCB.Text, txtjobTitleTxt.Text,
                  txtemailAddTxt.Text, txtcontAltPhoneTxt.Text, txtcontFaxNumTxt.Text, z_datetime, z_User, z_User, cmbsalutationCB.Text, cmbtypeCB.Text, txtsuffixTxt.Text, getcaddrid, txttinNumTxt.Text)

            Dim contID As String = getStringItem("Select MAX(RowID) from Contact")
            Dim getContID As Integer = Val(contID)

            Dim addrID As String = getStringItem("Select max(RoWID) from address where concat(StreetAddress1,' ', StreetAddress2,'',',',' ',CityTown,',',' ',Country) = '" & cmbprimaryAddress.Text & "'")
            Dim getaddrid As Integer = ValNoComma(address_RowID)

            If txtfilename.Text = Nothing Then
                SP_Organization(txtcompanyName.Text, getaddrid, Trim(contID), txtcompMainPhoneTxt.Text, txtcompFaxNumTxt.Text,
                          txtcompEmailTxt.Text, txtcompAltEmailTxt.Text, txtcompAltPhoneTxt.Text, txtcompUrl.Text, z_datetime,
                          z_User, z_datetime, z_User, txtorgTinNumTxt.Text, txttradeName.Text, cmborganizationTypeCB.Text,
                          Val(txtvlallow.Text),
                          Val(txtslallow.Text),
                          Val(txtmlallow.Text),
                          Val(txtotherallow.Text),
                          MilitTime(Format(CDate(nightdiffshiftfrom.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightdiffshiftto.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightshiftfrom.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightshiftto.Value), "hh:mm tt")),
                          payfreqID,
                          cbophhdeductsched.Text,
                          cbosssdeductsched.Text,
                          cbohdmfdeductsched.Text,
                          txtmindayperyear.Text,
                          txtRDO.Text,
                          txtZIP.Text,
                          cboTaxDeductSched.Text,
                          I_IsAgency:=IsAgencyCheckBox.Checked)
            Else
                Dim fs As FileStream
                Dim br As BinaryReader
                Dim FileName As String = txtfilename.Text
                Dim ImageData() As Byte

                fs = New FileStream(FileName, FileMode.Open, FileAccess.Read)
                br = New BinaryReader(fs)
                ImageData = br.ReadBytes(CType(fs.Length, Integer))
                br.Close()
                fs.Close()

                If cmbprimaryAddress.SelectedIndex = -1 Then
                    getaddrid = 0
                End If

                getaddrid = ValNoComma(address_RowID)

                SP_OrganizationWithImage(txtcompanyName.Text, getaddrid, Trim(contID), txtcompMainPhoneTxt.Text, txtcompFaxNumTxt.Text,
                      txtcompEmailTxt.Text, txtcompAltEmailTxt.Text, txtcompAltPhoneTxt.Text, txtcompUrl.Text, z_datetime,
                      z_User, z_datetime, z_User, txtorgTinNumTxt.Text, txttradeName.Text, cmborganizationTypeCB.Text, ImageData,
                          Val(txtvlallow.Text),
                          Val(txtslallow.Text),
                          Val(txtmlallow.Text),
                          Val(txtotherallow.Text),
                          MilitTime(Format(CDate(nightdiffshiftfrom.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightdiffshiftto.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightshiftfrom.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightshiftto.Value), "hh:mm tt")),
                          payfreqID,
                          cbophhdeductsched.Text,
                          cbosssdeductsched.Text,
                          cbohdmfdeductsched.Text,
                          txtmindayperyear.Text,
                          txtRDO.Text,
                          txtZIP.Text,
                          cboTaxDeductSched.Text,
                          I_IsAgency:=IsAgencyCheckBox.Checked)
            End If

            dgvCompanyList.Enabled = True
            myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
            txtfilename.Clear()
        Else
            If dontUpdate = 1 Then
                Exit Sub
            End If

            Z_ErrorProvider.Dispose()
            If Not SetWarningIfEmpty(txtcompanyName) And SetWarningIfEmpty(txtcompMainPhoneTxt) And SetWarningIfEmpty(cmbprimaryAddress) _
                And SetWarningIfEmpty(txtcompEmailTxt) And SetWarningIfEmpty(txtorgTinNumTxt) And SetWarningIfEmpty(cmborganizationTypeCB) _
                And SetWarningIfEmpty(txtlastNameTxt) And SetWarningIfEmpty(txtfirstNameTxt) And SetWarningIfEmpty(cmbsalutationCB) _
                And SetWarningIfEmpty(cmbgenderCB) And SetWarningIfEmpty(cmbaddressCB) And SetWarningIfEmpty(txtjobTitleTxt) _
                And SetWarningIfEmpty(txtcontMainPhoneTxt) And SetWarningIfEmpty(txttinNumTxt) And SetWarningIfEmpty(txtmobilePhoneTxt) _
                And SetWarningIfEmpty(txtemailAddTxt) And SetWarningIfEmpty(cmbstatusCB) Then
                Exit Sub
            End If

            Dim caddrID As String = getStringItem("Select max(RoWID) from address where concat(StreetAddress1,' ', StreetAddress2,'',',',' ',CityTown,',',' ',Country) = '" & cmbaddressCB.Text & "'")
            Dim getcaddrid As Integer = Val(caddrID)

            I_contactUpdate(cmbstatusCB.Text, txtcontMainPhoneTxt.Text, txtlastNameTxt.Text, txtfirstNameTxt.Text,
          txtmiddleNameTxt.Text, txtmobilePhoneTxt.Text, txtworkPhoneTxt.Text, cmbgenderCB.Text, txtjobTitleTxt.Text,
          txtemailAddTxt.Text, txtcontAltPhoneTxt.Text, txtcontFaxNumTxt.Text, z_datetime, z_User, cmbsalutationCB.Text,
          cmbtypeCB.Text, txtsuffixTxt.Text, getcaddrid, txttinNumTxt.Text, Val(dgvCompanyList.CurrentRow.Cells(c_ContactID.Index).Value))

            Dim contID As String = getStringItem("Select MAX(RowID) from Contact")
            Dim getContID As Integer = Val(contID)

            Dim addrID As String = getStringItem("Select max(RoWID) from address where concat(StreetAddress1,' ', StreetAddress2,'',',',' ',CityTown,',',' ',Country) = '" & cmbprimaryAddress.Text & "'")
            Dim getaddrid As Integer = Val(addrID)

            If cmbprimaryAddress.SelectedIndex = -1 Then
                getaddrid = 0
            End If

            getaddrid = ValNoComma(address_RowID)

            If txtfilename.Text = Nothing Then
                SP_OrganizationUpdate(txtcompanyName.Text, getaddrid, Trim(contID), txtcompMainPhoneTxt.Text, txtcompFaxNumTxt.Text,
                          txtcompEmailTxt.Text, txtcompAltEmailTxt.Text, txtcompAltPhoneTxt.Text, txtcompUrl.Text,
                           z_datetime, z_User, txtorgTinNumTxt.Text, txttradeName.Text, cmborganizationTypeCB.Text, dgvCompanyList.CurrentRow.Cells(c_rowID.Index).Value,
                          Val(txtvlallow.Text),
                          Val(txtslallow.Text),
                          Val(txtmlallow.Text),
                          Val(txtotherallow.Text),
                          MilitTime(Format(CDate(nightdiffshiftfrom.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightdiffshiftto.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightshiftfrom.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightshiftto.Value), "hh:mm tt")),
                          payfreqID,
                          cbophhdeductsched.Text,
                          cbosssdeductsched.Text,
                          cbohdmfdeductsched.Text,
                          txtmindayperyear.Text,
                          txtRDO.Text,
                          txtZIP.Text,
                          cboTaxDeductSched.Text,
                            I_IsAgency:=IsAgencyCheckBox.Checked)
                myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
                txtfilename.Clear()
                dgvCompanyList.Enabled = True
            Else
                Dim fs As FileStream
                Dim br As BinaryReader
                Dim FileName As String = txtfilename.Text
                Dim ImageData() As Byte

                fs = New FileStream(FileName, FileMode.Open, FileAccess.Read)
                br = New BinaryReader(fs)
                ImageData = br.ReadBytes(CType(fs.Length, Integer))
                br.Close()
                fs.Close()

                getaddrid = ValNoComma(address_RowID)

                SP_OrganizationWithImageUpdate(txtcompanyName.Text, getaddrid, Trim(contID), txtcompMainPhoneTxt.Text, txtcompFaxNumTxt.Text,
                      txtcompEmailTxt.Text, txtcompAltEmailTxt.Text, txtcompAltPhoneTxt.Text, txtcompUrl.Text,
                       z_datetime, z_User, txtorgTinNumTxt.Text, txttradeName.Text, cmborganizationTypeCB.Text, ImageData, dgvCompanyList.CurrentRow.Cells(c_rowID.Index).Value,
                          Val(txtvlallow.Text),
                          Val(txtslallow.Text),
                          Val(txtmlallow.Text),
                          Val(txtotherallow.Text),
                          MilitTime(Format(CDate(nightdiffshiftfrom.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightdiffshiftto.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightshiftfrom.Value), "hh:mm tt")),
                          MilitTime(Format(CDate(nightshiftto.Value), "hh:mm tt")),
                          payfreqID,
                          cbophhdeductsched.Text,
                          cbosssdeductsched.Text,
                          cbohdmfdeductsched.Text,
                          txtmindayperyear.Text,
                          txtRDO.Text,
                          txtZIP.Text,
                          cboTaxDeductSched.Text,
                            I_IsAgency:=IsAgencyCheckBox.Checked)
                myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
                txtfilename.Clear()
                dgvCompanyList.Enabled = True
            End If

            If dgvCompanyList.CurrentRow.Cells(c_rowID.Index).Value = orgztnID Then
                MDIPrimaryForm.Text = Trim(txtcompanyName.Text)
                orgNam = MDIPrimaryForm.Text
            End If

        End If

        SetWarningIfEmpty(txtcompanyName, "Hide this Error Provider")
        SetWarningIfEmpty(txtcompMainPhoneTxt, "Hide this Error Provider")
        SetWarningIfEmpty(cmbprimaryAddress, "Hide this Error Provider")
        SetWarningIfEmpty(txtcompEmailTxt, "Hide this Error Provider")
        SetWarningIfEmpty(txtorgTinNumTxt, "Hide this Error Provider")
        SetWarningIfEmpty(cmborganizationTypeCB, "Hide this Error Provider")
        SetWarningIfEmpty(cmbsalutationCB, "Hide this Error Provider")
        SetWarningIfEmpty(txtfirstNameTxt, "Hide this Error Provider")
        SetWarningIfEmpty(txtlastNameTxt, "Hide this Error Provider")
        SetWarningIfEmpty(txtjobTitleTxt, "Hide this Error Provider")
        SetWarningIfEmpty(cmbaddressCB, "Hide this Error Provider")
        SetWarningIfEmpty(cmbgenderCB, "Hide this Error Provider")
        SetWarningIfEmpty(txtmobilePhoneTxt, "Hide this Error Provider")
        SetWarningIfEmpty(txttinNumTxt, "Hide this Error Provider")
        SetWarningIfEmpty(txtcontMainPhoneTxt, "Hide this Error Provider")
        SetWarningIfEmpty(cmbstatusCB, "Hide this Error Provider")
        SetWarningIfEmpty(txtemailAddTxt, "Hide this Error Provider")

        fillOrganizationList()

        If btnNew.Enabled = False Then
            btnNew.Enabled = True

            isNew = 0
        End If

    End Sub

    Private Sub txtcompFaxNumTxt_TextChanged(sender As Object, e As EventArgs) Handles txtcompFaxNumTxt.TextChanged
        TextboxTestNumeric(sender, 30, 2)
    End Sub

    Private Sub txtcompMainPhoneTxt_TextChanged(sender As Object, e As EventArgs) Handles txtcompMainPhoneTxt.TextChanged
        TextboxTestNumeric(sender, 30, 2)
    End Sub

    Private Sub txtorgTinNumTxt_TextChanged(sender As Object, e As EventArgs) Handles txtorgTinNumTxt.TextChanged
        TextboxTestNumeric(sender, 30, 2)
    End Sub

    Private Sub txtcontMainPhoneTxt_TextChanged(sender As Object, e As EventArgs) Handles txtcontMainPhoneTxt.TextChanged
        TextboxTestNumeric(sender, 30, 2)
    End Sub

    Private Sub txttinNumTxt_TextChanged(sender As Object, e As EventArgs) Handles txttinNumTxt.TextChanged
        TextboxTestNumeric(sender, 30, 2)
    End Sub

    Private Sub txtmobilePhoneTxt_TextChanged(sender As Object, e As EventArgs) Handles txtmobilePhoneTxt.TextChanged
        TextboxTestNumeric(sender, 30, 2)
    End Sub

    Private Sub txtcontAltPhoneTxt_TextChanged(sender As Object, e As EventArgs) Handles txtcontAltPhoneTxt.TextChanged
        TextboxTestNumeric(sender, 30, 2)
    End Sub

    Private Sub txtcontFaxNumTxt_TextChanged(sender As Object, e As EventArgs) Handles txtcontFaxNumTxt.TextChanged
        TextboxTestNumeric(sender, 30, 2)
    End Sub

    Private Sub txtworkPhoneTxt_TextChanged(sender As Object, e As EventArgs) Handles txtworkPhoneTxt.TextChanged
        TextboxTestNumeric(sender, 30, 2)
    End Sub

    'Private Sub lblAddOrgType_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddOrgType.Click
    '    AddListOfValueForm.lblName.Text = "Organization Type"
    '    AddListOfValueForm.ShowDialog()

    'End Sub

    'Private Sub lblAddType_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddType.Click
    '    AddListOfValueForm.lblName.Text = "Type"
    '    AddListOfValueForm.ShowDialog()
    'End Sub

    'Private Sub lblAddStatus_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddStatus.Click
    '    AddListOfValueForm.lblName.Text = "Status"
    '    AddListOfValueForm.ShowDialog()
    'End Sub

    'Private Sub lblPT_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblPT.Click
    '    AddListOfValueForm.lblName.Text = "Personal Title"
    '    AddListOfValueForm.ShowDialog()
    'End Sub

    Private Sub OrganizationForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            hintInfo.Dispose()
        Catch ex As Exception

        End Try

        'myBalloon(, , lblSaveMsg, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        'If FormLeft.Contains("Organization") Then
        '    FormLeft.Remove("Organization")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.Text = "Welcome"
        'Else
        '    MDIPrimaryForm.Text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

    Dim govdeducsched As New AutoCompleteStringCollection

    Dim view_ID As Integer = Nothing

    Private Sub OrganizationForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        fillorganizationtype()
        fillpersonalstatus()
        fillstatus()
        filltype()
        filladdress()
        fillOrganizationList(False)
        fillorgitems()
        loadPayFreqType()

        enlistTheLists("SELECT DisplayValue FROM listofval WHERE `Type`='Government deduction schedule' AND Active='Yes' ORDER BY OrderBy;", govdeducsched)

        For Each strval In govdeducsched
            cbophhdeductsched.Items.Add(strval)
            cbosssdeductsched.Items.Add(strval)
            cbohdmfdeductsched.Items.Add(strval)
            cboTaxDeductSched.Items.Add(strval)
        Next

        view_ID = VIEW_privilege("Organization", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then

            btnNew.Visible = 0
            btnSave.Visible = 0
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    btnNew.Visible = 0
                    btnSave.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        btnNew.Visible = 0
                    Else
                        btnNew.Visible = 1
                    End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

        If dgvCompanyList.RowCount <> 0 Then
            dgvCompanyList_CellClick(sender, New DataGridViewCellEventArgs(c_companyname.Index, 0))
        End If

    End Sub

    Private Sub browseBtn_Click(sender As Object, e As EventArgs) Handles browseBtn.Click
        Try
            Dim fileOpener As OpenFileDialog = New OpenFileDialog()
            fileOpener.Filter = "Image files | *.jpg"
            'fileOpener.Filter = "JPEG(*.jpg)|*.jpg|JPEG(*.jpeg)|*.jpg|PNG(*.PNG)|*.png|Bitmap(*.BMP)|*.bmp"
            If fileOpener.ShowDialog() = Windows.Forms.DialogResult.OK Then
                PhotoImages.Image = Image.FromFile(fileOpener.FileName)
                txtfilename.Text = fileOpener.FileName
            End If
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    'Private Sub removeImageLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles removeImageLink.Click
    '    PhotoImages.Image = Nothing
    '    txtfilename.Clear()

    'End Sub

    Private Sub dgvCompanyList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCompanyList.CellContentClick

    End Sub

    Private Sub dgvCompanyList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCompanyList.CellClick
        fillorgitems()

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Dim address_RowID = Nothing

    Private Sub addAddressLink1_LinkClicked_1(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles addAddressLink1.LinkClicked
        'address.ShowDialog()

        Dim n_AddressClass As New AddressClass

        n_AddressClass.IsAddNew = True

        If n_AddressClass.ShowDialog("") = Windows.Forms.DialogResult.OK Then

            Dim full_address = String.Empty

            With n_AddressClass

                address_RowID = .AddresRowID

                If .StreetAddress1 = Nothing Then
                    full_address = Nothing
                Else
                    full_address = .StreetAddress1 & ","
                End If

                If .StreetAddress2 <> Nothing Then
                    full_address &= .StreetAddress2 & ","
                End If

                If .Barangay <> Nothing Then
                    full_address &= .Barangay & ","
                End If

                If .City <> Nothing Then
                    full_address &= .City & ","
                End If

                If .State <> Nothing Then
                    full_address &= "," & .State & ","
                End If

                If .Country <> Nothing Then
                    full_address &= .Country & ","
                End If

                If .ZipCode <> Nothing Then
                    full_address &= .ZipCode
                End If

            End With

            Dim addressstringlength = full_address.Length

            Dim LastCharIsComma = String.Empty

            Try
                LastCharIsComma =
                full_address.Substring((addressstringlength - 1), 1)
            Catch ex As Exception
                LastCharIsComma = String.Empty
            End Try

            If LastCharIsComma.Trim = "," Then
                full_address = full_address.Substring(0, (addressstringlength - 1))

            End If

            full_address = full_address.Replace(",,", ",")

            If cmbprimaryAddress.Items.Contains(full_address) = False Then
                cmbprimaryAddress.Items.Add(full_address)

            End If

            cmbprimaryAddress.Text = full_address

        End If

    End Sub

    Private Sub lblAddOrgType_LinkClicked_1(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddOrgType.LinkClicked
        AddListOfValueForm.lblName.Text = "Organization Type"
        AddListOfValueForm.ShowDialog()
    End Sub

    Private Sub removeImageLink_LinkClicked_1(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles removeImageLink.LinkClicked

        If dgvCompanyList.RowCount <> 0 Then

            Dim dialogue_box = MessageBox.Show("Are you sure you want to clear the image ?",
                                               "Set image to nothing",
                                               MessageBoxButtons.YesNoCancel,
                                               MessageBoxIcon.Information)

            If dialogue_box = Windows.Forms.DialogResult.Yes Then

                EXECQUER("UPDATE organization" &
                         " SET Image=NULL" &
                         " WHERE RowID='" & dgvCompanyList.CurrentRow.Cells(c_rowID.Index).Value & "';")

                PhotoImages.Image = Nothing

                txtfilename.Clear()

            End If
        Else

            PhotoImages.Image = Nothing

            txtfilename.Clear()

        End If

    End Sub

    Private Sub lblPT_LinkClicked_1(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblPT.LinkClicked
        AddListOfValueForm.lblName.Text = "Personal Title"
        AddListOfValueForm.ShowDialog()
    End Sub

    Private Sub lblAddStatus_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddStatus.LinkClicked
        AddListOfValueForm.lblName.Text = "Status"
        AddListOfValueForm.ShowDialog()
    End Sub

    Private Sub lblAddType_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddType.LinkClicked
        AddListOfValueForm.lblName.Text = "Type"
        AddListOfValueForm.ShowDialog()
    End Sub

    Function payp_count() As Integer

        Dim params(2, 2) As Object

        params(0, 0) = "organization_ID"

        params(0, 1) = orgztnID

        Dim _divisor = EXEC_INSUPD_PROCEDURE(params,
                                              "COUNT_payperiodthisyear",
                                              "payp_count")

        payp_count = CInt(_divisor)

    End Function

    Private Sub txtvlallow_TextChanged(sender As Object, e As EventArgs) Handles txtvlallow.TextChanged

    End Sub

    Private Sub txtvlallow_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtvlallow.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        Dim n_TrapDecimalKey As New TrapDecimalKey(e_asc, txtvlallow.Text)

        e.Handled = n_TrapDecimalKey.ResultTrap

        'Dim e_KAsc As String = Asc(e.KeyChar)

        'Static onedot As SByte = 0

        'If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

        '    If e_KAsc = 46 Then
        '        onedot += 1
        '        If onedot >= 2 Then
        '            If txtvlallow.Text.Contains(".") Then
        '                e.Handled = True
        '                onedot = 2
        '            Else
        '                e.Handled = False
        '                onedot = 0
        '            End If
        '        Else
        '            If txtvlallow.Text.Contains(".") Then
        '                e.Handled = True
        '            Else
        '                e.Handled = False
        '            End If
        '        End If
        '    Else
        '        e.Handled = False
        '    End If

        'Else
        '    e.Handled = True
        'End If

    End Sub

    Private Sub txtslallow_TextChanged(sender As Object, e As EventArgs) Handles txtslallow.TextChanged

    End Sub

    Private Sub txtslallow_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtslallow.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        Dim n_TrapDecimalKey As New TrapDecimalKey(e_asc, txtslallow.Text)

        e.Handled = n_TrapDecimalKey.ResultTrap

        'Dim e_KAsc As String = Asc(e.KeyChar)

        'Static onedot As SByte = 0

        'If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

        '    If e_KAsc = 46 Then
        '        onedot += 1
        '        If onedot >= 2 Then
        '            If txtslallow.Text.Contains(".") Then
        '                e.Handled = True
        '                onedot = 2
        '            Else
        '                e.Handled = False
        '                onedot = 0
        '            End If
        '        Else
        '            If txtslallow.Text.Contains(".") Then
        '                e.Handled = True
        '            Else
        '                e.Handled = False
        '            End If
        '        End If
        '    Else
        '        e.Handled = False
        '    End If

        'Else
        '    e.Handled = True
        'End If

    End Sub

    Private Sub txtmlallow_TextChanged(sender As Object, e As EventArgs) Handles txtmlallow.TextChanged

    End Sub

    Private Sub txtmlallow_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtmlallow.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        Dim n_TrapDecimalKey As New TrapDecimalKey(e_asc, txtmlallow.Text)

        e.Handled = n_TrapDecimalKey.ResultTrap

        'Dim e_KAsc As String = Asc(e.KeyChar)

        'Static onedot As SByte = 0

        'If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

        '    If e_KAsc = 46 Then
        '        onedot += 1
        '        If onedot >= 2 Then
        '            If txtmlallow.Text.Contains(".") Then
        '                e.Handled = True
        '                onedot = 2
        '            Else
        '                e.Handled = False
        '                onedot = 0
        '            End If
        '        Else
        '            If txtmlallow.Text.Contains(".") Then
        '                e.Handled = True
        '            Else
        '                e.Handled = False
        '            End If
        '        End If
        '    Else
        '        e.Handled = False
        '    End If

        'Else
        '    e.Handled = True
        'End If

    End Sub

    Private Sub txtotherallow_TextChanged(sender As Object, e As EventArgs) Handles txtotherallow.TextChanged

    End Sub

    Private Sub txtotherallow_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtotherallow.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        Dim n_TrapDecimalKey As New TrapDecimalKey(e_asc, txtotherallow.Text)

        e.Handled = n_TrapDecimalKey.ResultTrap

        'Dim e_KAsc As String = Asc(e.KeyChar)

        'Static onedot As SByte = 0

        'If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

        '    If e_KAsc = 46 Then
        '        onedot += 1
        '        If onedot >= 2 Then
        '            If txtotherallow.Text.Contains(".") Then
        '                e.Handled = True
        '                onedot = 2
        '            Else
        '                e.Handled = False
        '                onedot = 0
        '            End If
        '        Else
        '            If txtotherallow.Text.Contains(".") Then
        '                e.Handled = True
        '            Else
        '                e.Handled = False
        '            End If
        '        End If
        '    Else
        '        e.Handled = False
        '    End If

        'Else
        '    e.Handled = True
        'End If

    End Sub

    Sub loadPayFreqType()
        enlistTheLists("SELECT CONCAT(RowID,'@',PayFrequencyType) FROM payfrequency", payFreq)
        For Each r In payFreq
            cbopayfreq.Items.Add(StrReverse(getStrBetween(StrReverse(r), "", "@")))
        Next
    End Sub

    Dim payfreqID As String = Nothing

    Private Sub cbopayfreq_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbopayfreq.SelectedIndexChanged, cbopayfreq.SelectedValueChanged

        If cbopayfreq.SelectedIndex = -1 Then
            payfreqID = Nothing
        Else
            payfreqID = payFreq.Item(cbopayfreq.SelectedIndex)

            payfreqID = getStrBetween(payfreqID, "", "@")
        End If

    End Sub

    Private Sub cbopayfreq_KeyDown(sender As Object, e As KeyEventArgs) Handles cbopayfreq.KeyDown
        If e.KeyCode = Keys.Back _
            Or e.KeyCode = Keys.Escape Then
            cbopayfreq.SelectedIndex = -1
        End If
    End Sub

    Function MilitTime(ByVal timeval As Object) As Object

        Dim retrnObj As Object

        retrnObj = New Object

        If timeval = Nothing Then
            retrnObj = Nothing
        Else

            Dim endtime As Object = timeval

            If endtime.ToString.Contains("P") Then

                Dim hrs As String = If(Val(getStrBetween(endtime, "", ":")) = 12, 12, Val(getStrBetween(endtime, "", ":")) + 12)

                Dim mins As String = StrReverse(getStrBetween(StrReverse(endtime.ToString), "", ":"))

                mins = getStrBetween(mins, "", " ")

                retrnObj = hrs & ":" & mins

            ElseIf endtime.ToString.Contains("A") Then

                Dim i As Integer = StrReverse(endtime).ToString.IndexOf(" ")

                endtime = endtime.ToString.Replace("A", "")

                'Dim i As Integer = StrReverse("3:15 AM").ToString.IndexOf(" ")

                ''endtime = endtime.ToString.Replace("A", "")

                'MsgBox(Trim(StrReverse(StrReverse("3:15 AM").ToString.Substring(i, ("3:15 AM").ToString.Length - i))).Length)

                Dim amTime As String = Trim(StrReverse(StrReverse(endtime.ToString).Substring(i,
                                                                                  endtime.ToString.Length - i)
                                          )
                               )

                amTime = If(getStrBetween(amTime, "", ":") = "12",
                            24 & ":" & StrReverse(getStrBetween(StrReverse(amTime), "", ":")),
                            amTime)

                retrnObj = amTime

            End If

        End If

        Return retrnObj

    End Function

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub txtmindayperyear_KeyDown(sender As Object, e As KeyEventArgs) Handles txtmindayperyear.KeyDown
        If e.KeyCode = Keys.Up Then
            txtmindayperyear.Text = Val(txtmindayperyear.Text) + 1
            txtmindayperyear.SelectionStart = txtmindayperyear.TextLength
        ElseIf e.KeyCode = Keys.Down Then
            txtmindayperyear.Text = Val(txtmindayperyear.Text) - 1
            txtmindayperyear.SelectionStart = txtmindayperyear.TextLength
        End If
    End Sub

    Private Sub txtmindayperyear_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtmindayperyear.KeyPress
        e.Handled = TrapNumKey(Asc(e.KeyChar))
    End Sub

    Private Sub txtmindayperyear_TextChanged(sender As Object, e As EventArgs) Handles txtmindayperyear.TextChanged

    End Sub

    Private Sub txtRDO_KeyDown(sender As Object, e As KeyEventArgs) Handles txtRDO.KeyDown
        If e.KeyCode = Keys.Up Then
            txtRDO.Text = Val(txtRDO.Text) + 1
            txtRDO.SelectionStart = txtRDO.TextLength
        ElseIf e.KeyCode = Keys.Down Then
            txtRDO.Text = Val(txtRDO.Text) - 1
            txtRDO.SelectionStart = txtRDO.TextLength
        ElseIf e.Control AndAlso e.KeyCode = Keys.C Then
            txtRDO.Copy()
        End If
    End Sub

    Private Sub txtRDO_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRDO.KeyPress
        e.Handled = TrapNumKey(Asc(e.KeyChar))
    End Sub

    Private Sub txtRDO_TextChanged(sender As Object, e As EventArgs) Handles txtRDO.TextChanged

    End Sub

    Private Sub txtZIP_KeyDown(sender As Object, e As KeyEventArgs) Handles txtZIP.KeyDown
        If e.KeyCode = Keys.Up Then
            txtZIP.Text = Val(txtZIP.Text) + 1
            txtZIP.SelectionStart = txtZIP.TextLength
        ElseIf e.KeyCode = Keys.Down Then
            txtZIP.Text = Val(txtZIP.Text) - 1
            txtZIP.SelectionStart = txtZIP.TextLength
        ElseIf e.Control AndAlso e.KeyCode = Keys.C Then
            txtZIP.Copy()
        End If

    End Sub

    Private Sub txtZIP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtZIP.KeyPress
        e.Handled = TrapNumKey(Asc(e.KeyChar))
    End Sub

    Private Sub txtZIP_TextChanged(sender As Object, e As EventArgs) Handles txtZIP.TextChanged

    End Sub

End Class