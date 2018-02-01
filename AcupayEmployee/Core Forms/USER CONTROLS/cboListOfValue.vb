Imports MySql.Data.MySqlClient

Namespace PayrollSystem

End Namespace

Public Class cboListOfValue

    Inherits ComboBox

    Dim n_OrderByColumn As SByte = 0

    Dim orderbystring As String = "RowID"

    Dim l_listofvalColumn As New List(Of String)

    Sub New()

        Me.ContextMenu = New ContextMenu

    End Sub

    Public Property OrderByColumn As SByte

        Get

            Return n_OrderByColumn

        End Get

        Set(value As SByte)

            n_OrderByColumn = value

            ListOfValColumns()

            Try
                orderbystring = l_listofvalColumn.Item(n_OrderByColumn)
            Catch ex As Exception
                orderbystring = "RowID"
            End Try

            ReloadItems()

        End Set

    End Property

    Dim n_ListOfValueType As String = String.Empty

    Public Property ListOfValueType As String

        Get
            Return n_ListOfValueType

        End Get

        Set(value As String)

            n_ListOfValueType = value.Trim

        End Set

    End Property

    Private Sub ReloadItems()

        If n_ListOfValueType.Length = 0 Then 'Or n_OrderByColumn = -1

            Me.Items.Clear()

        Else

            enlistingToCBoxItems()

            AdjustWidthComboBox_DropDown()

        End If

    End Sub

    Private Sub enlistingToCBoxItems()

        Dim dr As MySqlDataReader
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            conn.Open()
            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT DisplayValue FROM listofval WHERE `Type`='" & n_ListOfValueType & "' ORDER BY " & orderbystring & ";"
            End With
            dr = cmd.ExecuteReader()

            Me.Items.Clear()

            Do While dr.Read
                If dr.GetString(0) <> "" Then
                    Me.Items.Add(dr(0)) 'GetString
                End If
            Loop
            dr.Close()
            hasERR = 0
        Catch ex As Exception
            hasERR = 1
            'MsgBox(getErrExcptn(ex, "cboListOfValue"), MsgBoxStyle.Information, "UNEXPECTED MESSAGE")
        Finally
            conn.Close()
            cmd.Dispose()
            conn.Close()
        End Try

    End Sub

    Private Sub ListOfValColumns()

        Dim n_listofvalColumn As New AutoCompleteStringCollection

        If n_OrderByColumn = -1 Then

            n_listofvalColumn.Clear()

            l_listofvalColumn.Clear()

        Else
            enlistingTheLists(n_listofvalColumn)

            'enlistTheLists("", _
            '                n_listofvalColumn)

            For Each strval In n_listofvalColumn
                l_listofvalColumn.Add(strval)
            Next
            'n_listofvalColumn.CopyTo(Array() as String, index as Integer)
        End If

    End Sub

    Private Sub enlistingTheLists(ByVal AutoCompStrCollec As AutoCompleteStringCollection)

        If sys_db <> Nothing Then

            Dim dr As MySqlDataReader
            Try
                If conn.State = ConnectionState.Open Then : conn.Close() : End If
                conn.Open()
                cmd = New MySqlCommand
                With cmd
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT `COLUMN_NAME` FROM `INFORMATION_SCHEMA`.`COLUMNS` WHERE `TABLE_SCHEMA`='" & sys_db & "' AND `TABLE_NAME`='listofval';"
                End With
                dr = cmd.ExecuteReader()

                Me.Items.Clear()

                Do While dr.Read
                    If dr.GetString(0) <> "" Then
                        'Me.Items.Add(dr(0)) 'GetString
                        AutoCompStrCollec.Add(dr.GetString(0))
                    End If
                Loop
                dr.Close()
                hasERR = 0
            Catch ex As Exception
                hasERR = 1
                MsgBox(getErrExcptn(ex, "cboListOfValue"), MsgBoxStyle.Information, "UNEXPECTED MESSAGE")
            Finally
                conn.Close()
                cmd.Dispose()
                conn.Close()
            End Try

        End If

    End Sub

    Private Sub AdjustWidthComboBox_DropDown() 'e As EventArgs

        Dim cboxwidth = Me.DropDownWidth

        Dim g As Graphics = Me.CreateGraphics

        Dim vertScrollBarWidth As Integer = If(Me.Items.Count > Me.MaxDropDownItems, _
                                               SystemInformation.VerticalScrollBarWidth, _
                                               0)

        Dim newWidth As Integer = 0

        For Each strval As String In Me.Items

            newWidth = g.MeasureString(strval, Me.Font).Width _
                       + vertScrollBarWidth

            If cboxwidth < newWidth Then

                cboxwidth = newWidth

            End If

        Next

        Me.DropDownWidth = cboxwidth

    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If ObjectCopyToClipBoard(e, Me) Then

        End If
        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
        MyBase.OnKeyPress(e)
    End Sub

End Class