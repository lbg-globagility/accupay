Imports AccuPay.DB

Public Class AddressClass

    'SELECT  `RowID`,  `StreetAddress1`,  `StreetAddress2`,  `CityTown`,  `Country`,  `State`,  `CreatedBy`,  `LastUpdBy`,  `Created`,  `LastUpd`,  `ZipCode`,  `Barangay` FROM `rbxpayroll`.`address`

    Dim AddRowID = String.Empty

    Property AddresRowID As String

        Get
            Return AddRowID
        End Get
        Set(value As String)
            AddRowID = value
        End Set
    End Property

    Dim ad_StreetAddress1 = String.Empty

    Property StreetAddress1 As String

        Get
            Return ad_StreetAddress1
        End Get
        Set(value As String)
            ad_StreetAddress1 = value
        End Set
    End Property

    Dim ad_StreetAddress2 = String.Empty

    Property StreetAddress2 As String

        Get
            Return ad_StreetAddress2
        End Get
        Set(value As String)
            ad_StreetAddress2 = value
        End Set
    End Property

    Dim ad_Barangay = String.Empty

    Property Barangay As String

        Get
            Return ad_Barangay
        End Get
        Set(value As String)
            ad_Barangay = value
        End Set
    End Property

    Dim ad_City = String.Empty

    Property City As String

        Get
            Return ad_City
        End Get
        Set(value As String)
            ad_City = value
        End Set
    End Property

    Dim ad_State = String.Empty

    Property State As String

        Get
            Return ad_State
        End Get
        Set(value As String)
            ad_State = value
        End Set
    End Property

    Dim ad_Country = String.Empty

    Property Country As String

        Get
            Return ad_Country
        End Get
        Set(value As String)
            ad_Country = value
        End Set
    End Property

    Dim ad_ZipCode = String.Empty

    Property ZipCode As String

        Get
            Return ad_ZipCode
        End Get
        Set(value As String)
            ad_ZipCode = value
        End Set
    End Property

    Private is_add_new As Boolean = False

    Property IsAddNew As Boolean
        Get
            Return is_add_new
        End Get
        Set(value As Boolean)
            is_add_new = value
        End Set
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)

        MyBase.OnLoad(e)

    End Sub

    Private Sub AddressClass_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        btnRefresh_Click(btnRefresh, New EventArgs)

    End Sub

    Private Sub txtCity_TextChanged(sender As Object, e As EventArgs) Handles txtCity.TextChanged

    End Sub

    Private Sub txtState_TextChanged(sender As Object, e As EventArgs) Handles txtState.TextChanged

    End Sub

    Private Sub txtCountry_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCity.KeyPress,
                                                                                        txtState.KeyPress,
                                                                                        txtCountry.KeyPress
        Dim e_asc = Asc(e.KeyChar)

        e.Handled = TrapCharKey(e_asc)

    End Sub

    Private Sub txtCountry_TextChanged(sender As Object, e As EventArgs) Handles txtCountry.TextChanged

    End Sub

    Private Sub txtZip_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtZip.KeyPress
        Dim e_asc = Asc(e.KeyChar)

        e.Handled = TrapNumKey(e_asc)

    End Sub

    Private Sub txtZip_TextChanged(sender As Object, e As EventArgs) Handles txtZip.TextChanged

    End Sub

    Private Sub tsbtnNewAddress_Click(sender As Object, e As EventArgs) Handles tsbtnNewAddress.Click

        clearObjControl(Panel2)

        txtStreet.Focus()

        tsbtnNewAddress.Enabled = False

    End Sub

    Private Sub tsbtnSaveAddress_Click(sender As Object, e As EventArgs) Handles tsbtnSaveAddress.Click

        If tsbtnNewAddress.Enabled = False Then

            Dim addre_RowID =
            INSUPD_address(, txtStreet.Text.Trim,
                    txtStreet2.Text.Trim,
                    txtBrgy.Text.Trim,
                    txtCity.Text.Trim,
                    txtState.Text.Trim,
                    txtCountry.Text.Trim,
                    txtZip.Text.Trim)

            dgvAddress.Rows.Add(addre_RowID,
                    txtStreet.Text.Trim,
                    txtStreet2.Text.Trim,
                    txtBrgy.Text.Trim,
                    txtCity.Text.Trim,
                    txtState.Text.Trim,
                    txtCountry.Text.Trim,
                    txtZip.Text.Trim)

            tsbtnNewAddress.Enabled = True
        Else

            INSUPD_address(
                    addressRowID,
                    txtStreet.Text.Trim,
                    txtStreet2.Text.Trim,
                    txtBrgy.Text.Trim,
                    txtCity.Text.Trim,
                    txtState.Text.Trim,
                    txtCountry.Text.Trim,
                    txtZip.Text.Trim)

            With dgvAddress.CurrentRow

                .Cells("adStreet").Value = txtStreet.Text.Trim
                .Cells("adStreet2").Value = txtStreet2.Text.Trim
                .Cells("adBrgy").Value = txtBrgy.Text.Trim
                .Cells("adCity").Value = txtCity.Text.Trim
                .Cells("adState").Value = txtState.Text.Trim
                .Cells("adCountry").Value = txtCountry.Text.Trim
                .Cells("adZip").Value = txtZip.Text.Trim

            End With

        End If

        If is_add_new Then
            Width = 355
        End If

    End Sub

    Function INSUPD_address(Optional ad_RowID = Nothing,
                            Optional ad_StreetAddress1 = Nothing,
                            Optional ad_StreetAddress2 = Nothing,
                            Optional ad_Barangay = Nothing,
                            Optional ad_CityTown = Nothing,
                            Optional ad_State = Nothing,
                            Optional ad_Country = Nothing,
                            Optional ad_ZipCode = Nothing) As Object

        Dim params =
            New Object() {If(ad_RowID = Nothing, DBNull.Value, ad_RowID),
            z_User,
            ad_StreetAddress1,
            ad_StreetAddress2,
            ad_Barangay,
            ad_CityTown,
            ad_State,
            ad_Country,
            ad_ZipCode}

        Dim str_sql_quer As String =
            String.Concat("SELECT INSUPD_address(?ad_rowid",
                          ", ?user_rowid",
                          ", ?ad_street1",
                          ", ?ad_street2",
                          ", ?ad_brgy",
                          ", ?ad_city",
                          ", ?ad_state",
                          ", ?ad_country",
                          ", ?ad_zipcode) `Result`;")

        Dim sql As New SQL(str_sql_quer,
                           params)

        Dim returnvalue As Object = Nothing

        Try

            returnvalue = sql.GetFoundRow

            If sql.HasError Then
                Throw sql.ErrorException
            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Name))
        End Try

        Return returnvalue

    End Function

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click

        btnRefresh_Click(btnRefresh, New EventArgs)

        If tsbtnNewAddress.Enabled = False Then
            tsbtnNewAddress.Enabled = True
        End If

    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Me.Close()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        RemoveHandler dgvAddress.SelectionChanged, AddressOf dgvAddress_SelectionChanged

        VIEW_address(autcomptxtaddress.Text.Trim)

        If tsbtnNewAddress.Enabled = False Then
            tsbtnNewAddress.Enabled = True
        End If

        dgvAddress_SelectionChanged(dgvAddress, New EventArgs)

        AddHandler dgvAddress.SelectionChanged, AddressOf dgvAddress_SelectionChanged

        Static once As Boolean = True

        If once And is_add_new Then
            once = False

            tsbtnNewAddress_Click(tsbtnNewAddress, New EventArgs)

            txtStreet.Focus()

        End If

    End Sub

    Sub VIEW_address(Optional SearchText As String = Nothing)

        Dim n_ReadSQLProcedureToDatatable As New ReadSQLProcedureToDatatable("VIEW_address",
                                                                             SearchText)

        Dim catch_dt As New DataTable

        catch_dt = n_ReadSQLProcedureToDatatable.ResultTable

        dgvAddress.Rows.Clear()

        For Each drow As DataRow In catch_dt.Rows

            Dim row_array = drow.ItemArray

            dgvAddress.Rows.Add(row_array)

        Next

    End Sub

    Private Sub dgvAddress_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAddress.CellContentClick

    End Sub

    Dim addressRowID = Nothing

    Private Sub dgvAddress_SelectionChanged(sender As Object, e As EventArgs) ' Handles dgvAddress.SelectionChanged

        If dgvAddress.RowCount = 0 Then

            addressRowID = Nothing

            clearObjControl(Panel2)

            AddRowID = Nothing

            ad_StreetAddress1 = Nothing

            ad_StreetAddress2 = Nothing

            ad_Barangay = Nothing

            ad_City = Nothing

            ad_State = Nothing

            ad_Country = Nothing

            ad_ZipCode = Nothing
        Else
            With dgvAddress.CurrentRow

                addressRowID = .Cells("adRowID").Value

                txtStreet.Text = .Cells("adStreet").Value
                txtStreet2.Text = .Cells("adStreet2").Value
                txtBrgy.Text = .Cells("adBrgy").Value
                txtCity.Text = .Cells("adCity").Value
                txtState.Text = .Cells("adState").Value
                txtCountry.Text = .Cells("adCountry").Value
                txtZip.Text = .Cells("adZip").Value

                AddRowID = addressRowID

                ad_StreetAddress1 = txtStreet.Text

                ad_StreetAddress2 = txtStreet2.Text

                ad_Barangay = txtBrgy.Text

                ad_City = txtCity.Text

                ad_State = txtState.Text

                ad_Country = txtCountry.Text

                ad_ZipCode = txtZip.Text

            End With

        End If

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Dim isShowAsDialog = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult
        ' ...do something with "someValue"...
        With Me
            isShowAsDialog = True

            btnOK.Visible = True

            btnClose.Visible = True

            .Text = someValue

            .FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog

            .MaximizeBox = False
            .MinimizeBox = False

            .StartPosition = FormStartPosition.CenterScreen

            If is_add_new = False Then
                .Width = 346

            End If

            '398,368

        End With

        Return Me.ShowDialog

    End Function

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If isShowAsDialog Then

            If keyData = Keys.Escape Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel

                Me.Close()

                Return True
            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

End Class