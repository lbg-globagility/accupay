Imports MySql.Data.MySqlClient
Public Class address
    Dim sCommand As MySqlCommand
    Dim sAdapter As MySqlDataAdapter
    Dim sBuilder As MySqlCommandBuilder
    Dim sDs As DataSet
    Dim sTable As DataTable
    'Dim manager As New Module1.Manager
    Dim nowDate = Date.Now.ToString("yyyy/MM/dd HH:mm:ss")
    Public userId As Integer

    Private Sub addressGrid_DataBindingComplete(ByVal sender As System.Object, _
    ByVal e As System.Windows.Forms.DataGridViewBindingCompleteEventArgs) _
    Handles addressGrid.DataBindingComplete

        Dim gridView As DataGridView
        gridView = CType(sender, DataGridView)
        gridView.ClearSelection()
    End Sub

    Private Sub Address_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        viewAllAddress()
        displayCountry()


    End Sub

    Private Function viewAllAddress()
        Dim sqlquery As String = "select rowid, Country, state as 'State/Province', citytown as 'City/Town', " & _
                            "Barangay, streetaddress1 as 'Street Address 1', streetaddress2 as 'Street Address 2', " & _
                            "zipcode as 'Zip Code' from address Order By RowID DESC"
        Dim conn As New MySqlConnection(connectionString)
        Dim sqlcmd As New MySqlCommand(sqlquery, conn)
        conn.Open()
        Dim sqlrd As MySqlDataReader = sqlcmd.ExecuteReader
        sqlcmd = Nothing

        Try
            If sqlrd.HasRows Then

                Dim table As New DataTable()
                table.Load(sqlrd)
                addressGrid.AutoGenerateColumns = True
                addressGrid.DataSource = table
                addressGrid.Refresh()

            End If

            addressGrid.Columns("rowid").Visible = False
            addressGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()

        Return True
    End Function

    Public Function displayCountry()
        Dim sqlquery As String = "select distinct(displayValue) from listofval where type = 'Country'"
        Dim conn As New MySqlConnection(connectionString)
        Dim sqlcmd As New MySqlCommand(sqlquery, conn)
        conn.Open()
        Dim sqlrd As MySqlDataReader = sqlcmd.ExecuteReader
        sqlcmd = Nothing
        Try
            If sqlrd.HasRows Then
                While sqlrd.Read
                    countryCB.Items.Add(sqlrd(0).ToString())
                End While
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()

        Return countryCB
    End Function

    Public Function displayStateProv(ByVal country As String)
        provStateCB.Items.Clear()
        Dim sqlquery As String = "select distinct(displayValue) from listofval where type in ('state', 'province') " & _
                            "and parentlic = '" & country & "' "
        Dim conn As New MySqlConnection(connectionString)
        Dim sqlcmd As New MySqlCommand(sqlquery, conn)
        conn.Open()
        Dim sqlrd As MySqlDataReader = sqlcmd.ExecuteReader
        sqlcmd = Nothing
        Try
            If sqlrd.HasRows Then
                While sqlrd.Read
                    provStateCB.Items.Add(sqlrd(0).ToString())
                End While
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()

        Return True
    End Function

    Public Function displayCityTown(ByVal any)
        cityTownCB.Items.Clear()
        Dim sqlquery As String = "select distinct(displayValue) from listofval where type in ('city', 'town') " & _
                            "and parentlic = '" & any & "'"
        Dim conn As New MySqlConnection(connectionString)
        Dim sqlcmd As New MySqlCommand(sqlquery, conn)
        conn.Open()
        Dim sqlrd As MySqlDataReader = sqlcmd.ExecuteReader
        sqlcmd = Nothing
        Try
            If sqlrd.HasRows Then
                While sqlrd.Read
                    cityTownCB.Items.Add(sqlrd(0).ToString())
                End While
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()

        Return cityTownCB
    End Function

    Public Function displayBrgy(ByVal some)
        brgyCB.Items.Clear()
        Dim sqlquery As String = "select distinct(displayValue) from listofval where type = 'barangay' " & _
                            "and parentlic = '" & some & "'"
        Dim conn As New MySqlConnection(connectionString)
        Dim sqlcmd As New MySqlCommand(sqlquery, conn)
        conn.Open()
        Dim sqlrd As MySqlDataReader = sqlcmd.ExecuteReader
        sqlcmd = Nothing
        Try
            If sqlrd.HasRows Then
                While sqlrd.Read
                    brgyCB.Items.Add(sqlrd(0).ToString())
                End While
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()

        Return brgyCB
    End Function

    Public Function addAddress(ByVal country, ByVal state, ByVal city, ByVal brgy, ByVal sa1, ByVal sa2, ByVal zipc)
        Dim connection As New MySqlConnection(connectionString)
        connection.Open()

        Dim sql As String = "insert into address(country, state, citytown, barangay, streetaddress1, " & _
                            "streetaddress2, zipcode, createdby, lastupdby, lastupd) values('" & country & "', " & _
                            "'" & state & "', '" & city & "', '" & brgy & "', '" & sa1 & "', '" & sa2 & "', '" & zipc & "', " & _
                            "" & userId & ", " & userId & ", '" & nowDate & "') "

        sCommand = New MySqlCommand(sql, connection)

        Try
            sCommand.ExecuteNonQuery()

            MsgBox("Address added successfully", MsgBoxStyle.Information, "System Message")
            reset()

            Return True
        Catch ex As System.Data.SqlClient.SqlException
            MsgBox(ex.Message)

        End Try

        connection.Close()

        Return Nothing
    End Function

    Private Sub countryCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles countryCB.SelectedIndexChanged
        'Dim country As String
        'country = countryCB.SelectedValue.ToString()
        displayStateProv(countryCB.SelectedItem)
        displayCityTown(countryCB.SelectedItem)
    End Sub

    Private Sub provStateCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles provStateCB.SelectedIndexChanged
        displayCityTown(provStateCB.SelectedItem)
    End Sub

    Private Sub cityTownCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cityTownCB.SelectedIndexChanged
        displayBrgy(cityTownCB.SelectedItem)
    End Sub

    Private Sub saveBtn_Click(sender As Object, e As EventArgs) Handles saveBtn.Click
        Dim prov As String = ""
        Dim brgy As String = ""
        Dim s2 As String = ""

        If countryCB.SelectedItem = Nothing Or cityTownCB.SelectedItem = Nothing Or sa1Txt.Text = "" Or zipCodeTxt.Text = "" Then
            MsgBox("Please complete the required data", MsgBoxStyle.Exclamation, "System Message")

        Else
            '    Public Function I_address(ByVal I_StreetAddress1 As String, _
            'ByVal I_StreetAddress2 As String, _
            'ByVal I_CityTown As String, _
            'ByVal I_Country As String, _
            'ByVal I_State As String, _
            'ByVal I_CreatedBy As Integer, _
            'ByVal I_LastUpdBy As Integer, _
            'ByVal I_Created As DateTime, _
            'ByVal I_LastUpd As DateTime, _
            'ByVal I_ZipCode As String, _
            'ByVal I_Barangay As String) As Boolean


            I_address(sa1Txt.Text.ToUpper, sa2Txt.Text.ToUpper, cityTownCB.Text.ToUpper, countryCB.Text.ToUpper, provStateCB.Text.ToUpper, _
                      z_User, z_User, z_datetime, z_datetime, zipCodeTxt.Text, brgyCB.Text.ToUpper)
            'If provStateCB.SelectedItem = "" Then
            '    If brgyCB.SelectedItem = "" Then
            '        If sa2Txt.Text = "" Then
            '            addAddress(countryCB.SelectedItem, prov, cityTownCB.SelectedItem, _
            '                brgy, sa1Txt.Text.Substring(0, 1).ToUpper() + sa1Txt.Text.Substring(1), s2, zipCodeTxt.Text)
            '        Else
            '            addAddress(countryCB.SelectedItem, prov, cityTownCB.SelectedItem, _
            '                brgy, sa1Txt.Text.Substring(0, 1).ToUpper() + sa1Txt.Text.Substring(1), sa2Txt.Text.Substring(0, 1).ToUpper() + sa2Txt.Text.Substring(1), zipCodeTxt.Text)
            '        End If
            '    Else
            '        If sa2Txt.Text = "" Then
            '            addAddress(countryCB.SelectedItem, prov, cityTownCB.SelectedItem, _
            '                brgyCB.SelectedItem, sa1Txt.Text.Substring(0, 1).ToUpper() + sa1Txt.Text.Substring(1), s2, zipCodeTxt.Text)
            '        Else
            '            addAddress(countryCB.SelectedItem, prov, cityTownCB.SelectedItem, _
            '                brgyCB.SelectedItem, sa1Txt.Text.Substring(0, 1).ToUpper() + sa1Txt.Text.Substring(1), sa2Txt.Text.Substring(0, 1).ToUpper() + sa2Txt.Text.Substring(1), zipCodeTxt.Text)
            '        End If
            '    End If
            'Else
            '    If brgyCB.SelectedItem = "" Then
            '        If sa2Txt.Text = "" Then
            '            addAddress(countryCB.SelectedItem, provStateCB.SelectedItem, cityTownCB.SelectedItem, _
            '                brgy, sa1Txt.Text.Substring(0, 1).ToUpper() + sa1Txt.Text.Substring(1), s2, zipCodeTxt.Text)
            '        Else
            '            addAddress(countryCB.SelectedItem, provStateCB.SelectedItem, cityTownCB.SelectedItem, _
            '                brgy, sa1Txt.Text.Substring(0, 1).ToUpper() + sa1Txt.Text.Substring(1), sa2Txt.Text.Substring(0, 1).ToUpper() + sa2Txt.Text.Substring(1), zipCodeTxt.Text)
            '        End If
            '    Else
            '        If sa2Txt.Text = "" Then
            '            addAddress(countryCB.SelectedItem, provStateCB.SelectedItem, cityTownCB.SelectedItem, _
            '                brgyCB.SelectedItem, sa1Txt.Text.Substring(0, 1).ToUpper() + sa1Txt.Text.Substring(1), s2, zipCodeTxt.Text)
            '        Else
            '            addAddress(countryCB.SelectedItem, provStateCB.SelectedItem, cityTownCB.SelectedItem, _
            '                brgyCB.SelectedItem, sa1Txt.Text.Substring(0, 1).ToUpper() + sa1Txt.Text.Substring(1), sa2Txt.Text.Substring(0, 1).ToUpper() + sa2Txt.Text.Substring(1), zipCodeTxt.Text)
            '        End If
            '    End If
            'End If
        End If

        viewAllAddress()
    End Sub

    Private Sub resetBtn_Click(sender As Object, e As EventArgs) Handles resetBtn.Click
        reset()
    End Sub

    Public Sub reset()
        countryCB.SelectedItem = Nothing
        provStateCB.SelectedItem = Nothing
        cityTownCB.SelectedItem = Nothing
        brgyCB.SelectedItem = Nothing
        sa1Txt.Text = ""
        sa2Txt.Text = ""
        zipCodeTxt.Text = ""
    End Sub

    Private Sub addressGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles addressGrid.CellClick
        addressGrid.ReadOnly = False
    End Sub

    Public Function updateAddress(ByVal rowid)
        Dim connection As New MySqlConnection(connectionString)
        Dim nowDate = Date.Now.ToString("yyyy/MM/dd HH:mm:ss")
        connection.Open()

        Dim sql As String = "update address  a set a.StreetAddress1 = @sa1, a.StreetAddress2 = @sa2, a.CityTown = @cityTown, a.Country = @country, " & _
            "a.State = @state, a.LastUpdBy = " & userId & ", a.LastUpd = @lastUpd, a.ZipCode = @zipCode, a.Barangay = @brgy where a.RowID = " & rowid & " "

        sCommand = New MySqlCommand(sql, connection)

        Try
            sCommand.Parameters.AddWithValue("@country", addressGrid.CurrentRow.Cells(1).FormattedValue)
            sCommand.Parameters.AddWithValue("@state", addressGrid.CurrentRow.Cells(2).FormattedValue)
            sCommand.Parameters.AddWithValue("@cityTown", addressGrid.CurrentRow.Cells(3).FormattedValue)
            sCommand.Parameters.AddWithValue("@brgy", addressGrid.CurrentRow.Cells(4).FormattedValue)
            sCommand.Parameters.AddWithValue("@sa1", addressGrid.CurrentRow.Cells(5).FormattedValue)
            sCommand.Parameters.AddWithValue("@sa2", addressGrid.CurrentRow.Cells(6).FormattedValue)
            sCommand.Parameters.AddWithValue("@zipCode", addressGrid.CurrentRow.Cells(7).FormattedValue)
            sCommand.Parameters.AddWithValue("@lastUpd", nowDate)
            sCommand.ExecuteNonQuery()

            MsgBox("Address updated successfully.", MsgBoxStyle.Information, "System Message")

            viewAllAddress()

        Catch ex As System.Data.SqlClient.SqlException
            MsgBox(ex.Message)
        End Try

        connection.Close()

        Return True
    End Function

    Private Sub addressGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles addressGrid.CellValueChanged
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to modify the cell?", "Modify Notification", MessageBoxButtons.OKCancel)
        If result = Windows.Forms.DialogResult.OK Then
            updateAddress(addressGrid.CurrentRow.Cells(0).FormattedValue)
        ElseIf result = Windows.Forms.DialogResult.Cancel Then
            addressGrid.ReadOnly = True
            viewAllAddress()
        End If
    End Sub

End Class