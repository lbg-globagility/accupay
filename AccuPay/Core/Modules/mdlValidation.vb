Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System.Data


Module mdlValidation
    Public z_OrganizationID As Integer
    Public z_User As Integer
    Public Z_UserName As String
    Public z_postName As String
    Public z_CompanyName As String
    Public z_CompanyAddr As String
    Public Z_Mouseleaver As Boolean = False
    Public Z_ErrorProvider As New ErrorProvider
    Public z_datetime As String = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
    Public z_ssid As Integer
    Public z_phID As Integer
    Public Z_encryptdata As String
    Public Z_encryptdata2 As String

    Public Sub ShowWaitForm(ByVal message As String)

        'MainForm.MainProgressBar.Minimum = 0
        'If Not message = Nothing Then
        '    WaitForm.Label1.Text = message
        'End If

        'WaitForm.Width = WaitForm.Label1.Width + 20
        'With MainForm

        '    For x As Integer = 0 To .StatusStrip1.Items.Count - 2
        '        .StatusStrip1.Items(x).Visible = False
        '    Next
        '    .MainProgressBar.Width = .MainProgressBar.Width
        '    .TopMost = False
        '    .Enabled = False
        '    WaitForm.TopLevel = False
        '    .Panel1.Controls.Add(WaitForm)
        '    WaitForm.Location = New Point((.Panel1.Width / 2) - (WaitForm.Width / 2), (.Panel1.Height / 2) - (WaitForm.Height / 2))
        '    WaitForm.Show()
        '    WaitForm.BringToFront()
        '    .GroupTimer.Stop()
        '    .Timer1.Stop()
        '    .MainProgressBar.Value = 0
        '    .MainProgressBar.Visible = True
        'End With
    End Sub

    Public Sub CloseWaitForm()
        'With MainForm
        '    For x As Integer = 0 To .StatusStrip1.Items.Count - 2
        '        .StatusStrip1.Items(x).Visible = True
        '        If .StatusStrip1.Items(x).Name.ToUpper() = "TOOLSTRIPSTATUSLABEL5" Then
        '            .StatusStrip1.Items(x).Visible = False
        '        End If
        '    Next
        'End With

        'MainForm.MainProgressBar.Visible = False
        'WaitForm.IsAllowClose = True
        'WaitForm.Close()
        'MainForm.GroupTimer.Start()
        'MainForm.Timer1.Start()
        'MainForm.Enabled = True

    End Sub
    Public Sub TextboxTestNumeric(ByVal textboxConts As TextBox, ByVal IntLen As Integer, ByVal DeciLen As Integer)
        If textboxConts.ReadOnly Then
            Exit Sub
        End If
        If textboxConts.Text = Nothing Then
            textboxConts.Text = 0
            textboxConts.SelectAll()
            Exit Sub
        End If
        Dim i As Integer = textboxConts.SelectionStart
        Dim txtSTR As String = textboxConts.Text
        Dim txtresult As String = Nothing

        For Each chrtxt As Char In txtSTR

            If IsNumeric(chrtxt) Or chrtxt = "," Or chrtxt = "." Then
                txtresult &= chrtxt
            Else
                i -= 1
            End If
        Next

        textboxConts.Text = txtresult
        Try
            textboxConts.SelectionStart = i
        Catch ex As Exception
            Exit Sub
        End Try


        If Not IsNumeric(textboxConts.Text) Then
            textboxConts.Text = textboxConts.Text.Remove(textboxConts.SelectionStart - 1, 1)
            textboxConts.SelectionStart = i - 1
            Exit Sub
        End If


        Dim TxtSplit() As String = Split(CDec(textboxConts.Text), ".")

        If IntLen < TxtSplit(0).Length Then
            textboxConts.Text = textboxConts.Text.Remove(textboxConts.SelectionStart - 1, 1)
            textboxConts.SelectionStart = i - 1
            Exit Sub
        End If
        If TxtSplit.Count > 1 Then
            If DeciLen < TxtSplit(1).Length Then
                textboxConts.Text = textboxConts.Text.Remove(textboxConts.SelectionStart - 1, 1)
                textboxConts.SelectionStart = i - 1
                Exit Sub
            End If
        End If

    End Sub

    Public Function SetWarningIfEmpty(ByVal co As Control, _
                                      Optional SetErrorString As String = Nothing)
        Z_ErrorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink
        If co.Text.Trim = Nothing Then
            Z_ErrorProvider.SetError(co, If(SetErrorString = Nothing, "Required to fill", Nothing))
            co.Focus()
            Return False
        End If
        Return True
    End Function


    Public Function ComputeEmpSalary(ByVal basicpay As Control, ByVal sss As Control, ByVal PhilHealth As Control)
        Dim basicrate As Double
        'basicrate = Convert.ToDouble(txtBasicrate.Text)
        basicrate = CDec(IIf(Decimal.TryParse(basicpay.Text, Nothing), basicpay.Text, "0"))
        Dim dt As New DataTable
        dt = getDataTableForSQL("select * from payphilhealth where SalaryRangeFrom <= '" & basicrate & "' And SalaryRangeTo >= '" & basicrate & "'")

        If dt.Rows.Count > 0 Then
            PhilHealth.Text = Nothing
            Dim philid As Integer

            For Each drow As DataRow In dt.Rows
                With drow
                    PhilHealth.Text = FormatNumber(.Item("EmployeeShare"), 2)
                    'txtPhilHealthSal
                    philid = .Item("RowID")
                    z_phID = philid
                End With
            Next
        Else


            dt = getDataTableForSQL("select MAX(EMployeeShare) as EMployeeShare , RowID from payphilhealth where SalaryRangeTo < '" & basicrate & "'")
            PhilHealth.Text = Nothing
            Dim philid As Integer
            If dt.Rows.Count > 0 Then
                For Each drow As DataRow In dt.Rows
                    With drow
                        PhilHealth.Text = FormatNumber(.Item("EmployeeShare"), 2)
                        philid = .Item("RowID")
                        z_phID = philid
                    End With
                Next
            End If
            
            '
        End If

        Dim dt1 As New DataTable
        dt1 = getDataTableForSQL("select * from paysocialsecurity where RangeFromAmount <= '" & basicrate & "' And RangeToAmount >= '" & basicrate & "'")



        If dt1.Rows.Count > 0 Then
            sss.Text = Nothing
            Dim ssid As Integer
            For Each drow As DataRow In dt1.Rows
                With drow
                    sss.Text = FormatNumber(.Item("EmployeeContributionAmount"), 2)
                    ssid = .Item("RoWID")
                    z_ssid = ssid
                End With
            Next
        Else
            dt1 = getDataTableForSQL("select MAX(EmployeeContributionAmount) as EmployeeContributionAmount, RowID from paysocialsecurity where RangeToAmount < '" & basicrate & "'")
            sss.Text = Nothing
            Dim ssid As Integer
            If dt1.Rows.Count > 0 Then
                For Each drow As DataRow In dt1.Rows
                    With drow
                        Dim amt As Double
                        If Double.TryParse(.Item("EmployeeContributionAmount").ToString, amt) Then
                            amt = FormatNumber(.Item("EmployeeContributionAmount"), 2)
                        Else
                            amt = 0

                        End If

                        sss.Text = amt

                        If Integer.TryParse(.Item("RowID").ToString, ssid) Then
                            ssid = .Item("RoWID").ToString
                        Else
                            ssid = 0
                        End If
                        z_ssid = ssid
                    End With
                Next
            End If

            
        End If

        Return True

    End Function
    Public Function execute(ByVal query As String) As DataTable
        Try
            Dim con As New MySqlConnection(connectionString)
            Dim da As New MySqlDataAdapter(query, con)
            Dim cb As New MySqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function getCount(ByVal Sqlcommand As String) As Integer
        connection = New MySqlConnection(connectionString)

        Dim ItemNumber As Integer = 0
        Try
            connection.Open()
            Dim command As MySqlCommand = _
                   New MySqlCommand(Sqlcommand, connection)
            command.CommandType = CommandType.Text
            Dim DR As MySqlDataReader = command.ExecuteReader
            Do While DR.Read
                ItemNumber = Val(DR.GetValue(0).ToString)
            Loop
        Catch ex As Exception
            ItemNumber = -1
        Finally
            connection.Close()
        End Try
        Return ItemNumber
    End Function
    'Public Function execute(ByVal query As String) As DataTable
    '    Try
    '        Dim con As New MySqlConnection(Newcon)
    '        Dim da As New MySqlDataAdapter(query, con)
    '        Dim cb As New MySqlCommandBuilder(da)
    '        Dim dt As New DataTable
    '        da.Fill(dt)
    '        Return dt
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function
    Public Function SQL_GetDataTable(ByVal sql_Queery As String) As DataTable
        Dim DataReturn As New DataTable
        Try
            Dim command As MySqlCommand = New MySqlCommand(sql_Queery, New MySqlConnection(connectionString))
            command.Connection.Open()
            Dim adapter As MySqlDataAdapter = New MySqlDataAdapter(command)
            adapter.Fill(DataReturn)
            command.Connection.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return DataReturn
    End Function

    Public Function getDataTableForSQL(ByVal COMMD As String)
        Dim command As MySqlCommand = New MySqlCommand(COMMD, connection)

        Try
            Dim DataReturn As New DataTable
            '    Dim sql As String = COMMD

            command.Connection.Open()


            Dim adapter As MySqlDataAdapter = New MySqlDataAdapter(command)
            adapter.Fill(DataReturn)
            command.Connection.Close()
            Return DataReturn
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        Finally
            command.Connection.Close()
        End Try
    End Function
    Public Function UpdateAuditTrail(ByVal strInputArray As Array, ByVal a_RowID As Integer, ByVal a_ViewID As Integer) As Boolean


        Dim created As String = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim UpperBound As Integer = strInputArray.GetUpperBound(0)
        For i As Integer = 0 To UpperBound
            If Not strInputArray(i, 1) = strInputArray(i, 2) Then
                'DirectCommand("UPDATE audittrail SET ")
                I_AuditTrail(created, Z_User, created, Z_User, Z_OrganizationID, a_ViewID, strInputArray(i, 0), a_RowID, strInputArray(i, 1), strInputArray(i, 2), "Update")
            End If
        Next
        Return True
    End Function

    Public Function InsertAudittrail(ByVal strInputArray As Array, ByVal a_RowID As Integer, ByVal a_ViewID As Integer) As Boolean
        Dim UpperBound As Integer = strInputArray.GetUpperBound(0)

        Dim created As String = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
        For i As Integer = 0 To UpperBound
            I_AuditTrail(created, Z_User, created, Z_User, Z_OrganizationID, a_ViewID, strInputArray(i, 0), a_RowID, "", strInputArray(i, 1), "Insert")
        Next
        Return True
    End Function
    Public Function getDataSetForTable(ByVal TableName As String)
        Dim sql As String = "SELECT  * FROM " & TableName
        Dim command As MySqlCommand = New MySqlCommand(sql, New MySqlConnection(connectionString))
        Try
            Dim DataReturn As New DataSet
            command.Connection.Open()
            Dim adapter As MySqlDataAdapter = New MySqlDataAdapter(command)
            adapter.Fill(DataReturn)
            command.Connection.Close()
            Return DataReturn
        Catch ex As Exception
            Return Nothing
        Finally
            command.Connection.Close()
        End Try
    End Function

    Public Function getDataSetForSQL(ByVal COMMD As String)
        Dim command As MySqlCommand = New MySqlCommand(COMMD, New MySqlConnection(connectionString))

        Try
            Dim DataReturn As New DataSet
            'Dim sql As String = COMMD
            command.Connection.Open()
            Dim adapter As MySqlDataAdapter = New MySqlDataAdapter(command)
            adapter.Fill(DataReturn)
            command.Connection.Close()
            Return DataReturn
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        Finally
            command.Connection.Close()
        End Try

    End Function
    Public Sub fillCombobox(ByVal sqlCommand As String, ByVal LvName As ComboBox)
        LvName.Items.Clear()
        Dim dtFiller As New DataTable
        dtFiller = execute(sqlCommand)

        For rowCounter = 0 To dtFiller.Rows.Count - 1
            LvName.Items.Add(dtFiller.Rows(rowCounter).Item(0))

        Next
    End Sub
    Public Function SQL_ArrayList(ByVal Sqlcommand As String) As ArrayList
        connection = New MySqlConnection(connectionString)

        Dim ArString As New ArrayList
        Try
            connection.Open()
            Dim command As MySqlCommand = _
                   New MySqlCommand(Sqlcommand, connection)
            command.CommandType = CommandType.Text
            Dim DR As MySqlDataReader = command.ExecuteReader
            Do While DR.Read
                ArString.Add(DR.GetValue(0))
            Loop
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            connection.Close()
        End Try
        Return ArString


    End Function


    Public Function EncrypedData(ByVal a As String)
        Dim Encryped As String = Nothing
        If Not a Is Nothing Then
            For Each x As Char In a
                Dim ToCOn As Integer = Convert.ToInt64(x) + 133
                Encryped &= Convert.ToChar(Convert.ToInt64(ToCOn))
            Next
        End If

        Return Encryped
    End Function
    Public Function DecrypedData(ByVal a As String)
        Dim DEcrypedio As String = Nothing
        If Not a Is Nothing Then
            For Each x As Char In a
                Dim ToCOn As Integer = Convert.ToInt64(x) - 133
                DEcrypedio &= Convert.ToChar(Convert.ToInt64(ToCOn))
            Next
        End If
        Return DEcrypedio
    End Function

    'Public Function SetWarningIfEmpty(ByVal co As Control)
    '    Z_ErrorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink
    '    If co.Text.Trim = Nothing Then
    '        Z_ErrorProvider.SetError(co, "Required to fill")
    '        co.Focus()
    '        Return False
    '    End If
    '    Return True
    'End Function

    Public Function DirectCommand(ByVal SqCommand As String)
        Dim NumberitemInserted As Integer
        Dim command As MySqlCommand = New MySqlCommand(SqCommand, New MySqlConnection(connectionString))
        Try
            Dim DataReturn As New DataSet
            command.CommandType = CommandType.Text
            command.Connection.Open()
            NumberitemInserted = command.ExecuteNonQuery()
            command.Connection.Close()
        Catch ex As Exception
            NumberitemInserted = -1
        Finally
            command.Connection.Close()
        End Try
        Return NumberitemInserted
    End Function

    Public Function EncrypedData2(ByVal a As String)
        Dim Encryped As String = Nothing
        If Not a Is Nothing Then
            For Each x As Char In a
                Dim ToCOn As Integer = Convert.ToInt64(x) + 133
                Encryped &= Convert.ToChar(Convert.ToInt64(ToCOn))

                Z_encryptdata2 = Encryped


            Next
        End If

        Return Encryped
    End Function
    Public Function ObjectToString(ByVal obj As Object) As String
        Try
            If IsDBNull(obj) Then
                Return ""
            ElseIf obj = Nothing Then
                Return ""
            Else
                Return obj
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function getStringItem(ByVal Sqlcommand As String) As String
        connection = New MySqlConnection(connectionString)
        Dim itemSTR As String = Nothing
        Try
            connection.Open()
            Dim command As MySqlCommand = _
                   New MySqlCommand(Sqlcommand, connection)
            command.CommandType = CommandType.Text
            Dim DR As MySqlDataReader = command.ExecuteReader
            Do While DR.Read
                itemSTR = ObjectToString(DR.GetValue(0))
            Loop
        Catch ex As Exception
            itemSTR = ""
        Finally
            connection.Close()
        End Try
        Return itemSTR
    End Function
    Public Function SQL_ArrayList_Decrypted(ByVal Sqlcommand As String) As ArrayList
        connection = New MySqlConnection(connectionString)

        Dim ArString As New ArrayList
        Try
            connection.Open()
            Dim command As MySqlCommand = _
                   New MySqlCommand(Sqlcommand, connection)
            command.CommandType = CommandType.Text
            Dim DR As MySqlDataReader = command.ExecuteReader
            Do While DR.Read
                ArString.Add(DecrypedData(DR.GetValue(0)))
            Loop
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            connection.Close()
        End Try
        Return ArString


    End Function
    'Public Sub fillCombobox(ByVal sqlCommand As String, ByVal LvName As ComboBox)
    '    LvName.Items.Clear()
    '    Dim dtFiller As New DataTable
    '    dtFiller = execute(sqlCommand)

    '    For rowCounter = 0 To dtFiller.Rows.Count - 1
    '        LvName.Items.Add(dtFiller.Rows(rowCounter).Item(0))

    '    Next
    'End Sub
    'Public Sub AutoSearch(ByRef txbox As System.Windows.Forms.TextBox)
    '    connection.Open()
    '    Try
    '        Dim strcol As AutoCompleteStringCollection = New AutoCompleteStringCollection
    '        Dim command As MySqlCommand = _
    '        New MySqlCommand("select DisplayValue From listofval", connection)
    '        command.CommandType = CommandType.Text
    '        Dim DR As MySqlDataReader = command.ExecuteReader
    '        Do While DR.Read
    '            strcol.Add(DR.GetString(0))
    '        Loop

    '        txbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend
    '        txbox.AutoCompleteSource = AutoCompleteSource.CustomSource
    '        txbox.AutoCompleteCustomSource = strcol

    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    Finally
    '        connection.Close()
    '    End Try
    'End Sub





    Public Function ConvertImageFiletoBytes(ByVal ImageFilePath As String) As Byte()
        Dim _tempByte() As Byte = Nothing
        If String.IsNullOrEmpty(ImageFilePath) = True Then
            'Throw New ArgumentNullException("Image File Name Cannot be Null or Empty", "ImageFilePath")
            Return Nothing
        End If
        Try
            Dim _fileInfo As New IO.FileInfo(ImageFilePath)
            Dim _NumBytes As Long = _fileInfo.Length
            Dim _FStream As New IO.FileStream(ImageFilePath, IO.FileMode.Open, IO.FileAccess.Read)
            Dim _BinaryReader As New IO.BinaryReader(_FStream)
            _tempByte = _BinaryReader.ReadBytes(Convert.ToInt32(_NumBytes))
            _fileInfo = Nothing
            _NumBytes = 0
            _FStream.Close()
            _FStream.Dispose()
            _BinaryReader.Close()
            Return _tempByte
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ConvertByteToImage(ByVal ImgByte As Byte()) As Image
        Dim img As Image = Nothing
        Try
            Dim stream As System.IO.MemoryStream
            stream = New System.IO.MemoryStream(ImgByte)
            img = Image.FromStream(stream)
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, "mdlValidation"))
        End Try
        Return img
    End Function

    Public Function getImage(ByVal Sqlcommand As String) As Object
        connection = New MySqlConnection(connectionString)
        Dim ItemNumber As New Object
        Try
            connection.Open()
            Dim command As MySqlCommand = _
                   New MySqlCommand(Sqlcommand, connection)
            command.CommandType = CommandType.Text
            Dim DR As MySqlDataReader = command.ExecuteReader
            Do While DR.Read
                ItemNumber = DR.GetValue(0)
            Loop
        Catch ex As Exception
            ItemNumber = Nothing
        Finally
            connection.Close()
        End Try
        Return ItemNumber
    End Function


    Public hintInfo As New ToolTip
    Public Sub myBalloon(Optional ToolTipStringContent As String = Nothing, Optional ToolTipStringTitle As String = Nothing, Optional objct As System.Windows.Forms.IWin32Window = Nothing, Optional x As Integer = 0, Optional y As Integer = 0, Optional dispo As SByte = 0, Optional duration As Integer = 3000)
        Try
            If hintInfo IsNot Nothing Then
                If dispo = 1 Then
                    hintInfo.Active = False
                    hintInfo.Hide(objct)
                    hintInfo.Dispose()
                Else
                    hintInfo = New ToolTip
                    hintInfo.IsBalloon = True
                    hintInfo.ToolTipTitle = ToolTipStringTitle
                    hintInfo.ToolTipIcon = ToolTipIcon.Info
                    hintInfo.Show(ToolTipStringContent, objct, x - 2, y - 2, duration)
                End If
            End If
        Catch ex As Exception
            'MsgBox(ex.Message & " ERR_NO 77-10 : myBalloon")
        End Try
    End Sub

    Public hintWarn As ToolTip 'New ToolTip
    Public Sub myBalloonWarn(Optional ToolTipStringContent As String = Nothing, Optional ToolTipStringTitle As String = Nothing, Optional objct As System.Windows.Forms.IWin32Window = Nothing, Optional x As Integer = 0, Optional y As Integer = 0, Optional dispo As Byte = 0, Optional duration As Integer = 2275)

        'Dim hint As New ToolTip
        Try
            If hintWarn IsNot Nothing Then
                If dispo = 1 Then
                    hintWarn.Hide(objct)
                    hintWarn.Dispose()
                    'Exit Try
                    'Exit Sub
                Else
                    hintWarn = New ToolTip
                    hintWarn.IsBalloon = True
                    hintWarn.ToolTipTitle = ToolTipStringTitle
                    hintWarn.ToolTipIcon = ToolTipIcon.Warning
                    hintWarn.Show(ToolTipStringContent, objct, x - 2, y - 2, duration)
                End If
            End If
        Catch ex As Exception
            'MsgBox(ex.Message & " ERR_NO 77-10 : myBalloonWarn")
        End Try

    End Sub


    Public Sub AutoSearchEmpName(ByRef txbox As System.Windows.Forms.TextBox)
        connection.Open()
        Try
            Dim strcol As AutoCompleteStringCollection = New AutoCompleteStringCollection
            Dim command As MySqlCommand = _
            New MySqlCommand("select concat(FirstName, ' ', MiddleName, ' ', LastName) as Name from employee order by rowid desc", connection)
            command.CommandType = CommandType.Text
            Dim DR As MySqlDataReader = command.ExecuteReader
            Do While DR.Read
                strcol.Add(DR.GetString(0))
            Loop

            txbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            txbox.AutoCompleteSource = AutoCompleteSource.CustomSource
            txbox.AutoCompleteCustomSource = strcol

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    Public Sub AutoSearchEmpID(ByRef txbox As System.Windows.Forms.TextBox)
        connection.Open()
        Try
            Dim strcol As AutoCompleteStringCollection = New AutoCompleteStringCollection
            Dim command As MySqlCommand = _
            New MySqlCommand("select EmployeeID from employee order by rowid desc", connection)
            command.CommandType = CommandType.Text
            Dim DR As MySqlDataReader = command.ExecuteReader
            Do While DR.Read
                strcol.Add(DR.GetString(0))
            Loop

            txbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            txbox.AutoCompleteSource = AutoCompleteSource.CustomSource
            txbox.AutoCompleteCustomSource = strcol

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub
End Module
