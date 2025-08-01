﻿Imports MySql.Data.MySqlClient

Module mdlValidation
    Public Z_Client As Integer
    Public z_OrganizationID As Integer
    Public z_User As Integer
    Public z_postName As String
    Public z_CompanyName As String

    Public connectionString As String = n_DataBaseConnection.GetStringMySQLConnectionString

    Public connection As MySqlConnection = New MySqlConnection(connectionString)

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

End Module