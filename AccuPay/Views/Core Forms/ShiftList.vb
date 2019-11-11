Imports System.Threading
Imports AccuPay.DB

Public Class ShiftList

    Dim isShowAsDialog As Boolean = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult

        With Me

            isShowAsDialog = True

            .Text = someValue

        End With

        Return Me.ShowDialog

    End Function

    Protected Overrides Sub OnLoad(e As EventArgs)

        'Me.BringToFront()

        Dim dtshift As New DataTable

        dtshift = New SQLQueryToDatatable("SELECT '' AS RowID,'None' AS TimeFrom,'None' AS TimeTo" &
                                            " UNION" &
                                            " SELECT -1 AS RowID,'Set as' AS TimeFrom,'Rest day' AS TimeTo" &
                                            " UNION" &
                                            " SELECT RowID" &
                                            ",TIME_FORMAT(TimeFrom,'%l:%i %p') AS TimeFrom" &
                                            ",TIME_FORMAT(TimeTo,'%l:%i %p') AS TimeTo" &
                                            " FROM shift" &
                                            " WHERE OrganizationID='" & orgztnID & "'" &
                                            " AND TimeFrom IS NOT NULL" &
                                            " AND TimeTo IS NOT NULL" &
                                            " ORDER BY TIME_FORMAT(TimeFrom,'%p%h%i%s'),TIME_FORMAT(TimeTo,'%p%h%i%s');").ResultTable 'orgztnID

        For Each drow As DataRow In dtshift.Rows

            Dim row_array = drow.ItemArray

            dgvcalendar.Rows.Add(row_array)

        Next

        MyBase.OnLoad(e)

    End Sub

    Private Sub ShiftList_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub ShiftList_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return True

        ElseIf keyData = Keys.Enter Then

            dgvcalendar.Focus()

            Try
                Dim dgv_currRow = dgvcalendar.CurrentRow

                dgvcalendar_CellDoubleClick(dgvcalendar, New DataGridViewCellEventArgs(shTimeFrom.Index, dgv_currRow.Index))
            Catch ex As Exception

                MsgBox(getErrExcptn(ex, Me.Name))

            End Try

            Return False
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub dgvcalendar_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvcalendar.CellContentClick

    End Sub

    Dim n_ShiftRowID = Nothing

    Property ShiftRowID As Object

        Get
            Return n_ShiftRowID
        End Get
        Set(value As Object)
            n_ShiftRowID = value
        End Set
    End Property

    Dim n_TimFromValue = Nothing

    Property TimeFromValue As Object

        Get
            Return n_TimFromValue
        End Get
        Set(value As Object)
            n_TimFromValue = value
        End Set
    End Property

    Dim n_TimToValue = Nothing

    Property TimeToValue As Object

        Get
            Return n_TimToValue
        End Get
        Set(value As Object)
            n_TimToValue = value
        End Set
    End Property

    Private Sub dgvcalendar_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvcalendar.CellDoubleClick

        If e.RowIndex > -1 _
            And e.ColumnIndex > -1 Then

            With dgvcalendar.Rows(e.RowIndex)

                n_ShiftRowID = .Cells("shRowID").Value

                Dim n_SQLQueryToDatatable As _
                    New SQLQueryToDatatable("SELECT RowID" &
                                            ",TIMESTAMP(CONCAT(CURDATE(),' ',TimeFrom)) AS TimeFrom" &
                                            ",TIMESTAMP(CONCAT(CURDATE(),' ',TimeTo)) AS TimeTo" &
                                            " FROM shift" &
                                            " WHERE RowID='" & n_ShiftRowID & "';")

                Dim dtshift As New DataTable

                dtshift = n_SQLQueryToDatatable.ResultTable

                For Each drow As DataRow In dtshift.Rows

                    If IsDBNull(drow("TimeFrom")) = False Then
                        n_TimFromValue = drow("TimeFrom")
                    End If

                    If IsDBNull(drow("TimeTo")) = False Then
                        n_TimToValue = drow("TimeTo")
                    End If

                Next

                Me.DialogResult = Windows.Forms.DialogResult.OK

            End With
        Else

            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        End If

    End Sub

    Private Sub dgvcalendar_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvcalendar.CellMouseDown

        If e.Button = Windows.Forms.MouseButtons.Left Then

            If e.RowIndex > -1 _
                And e.ColumnIndex > -1 Then

                dgvcalendar.Item(e.ColumnIndex, e.RowIndex).Selected = True

            End If

            'Thread.Sleep(350)

            'Me.Close()

        End If

    End Sub

    Private Sub dgvcalendar_DoubleClick(sender As Object, e As EventArgs) Handles dgvcalendar.DoubleClick

    End Sub

    Private Sub dgvcalendar_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvcalendar.MouseDown

    End Sub

    Private Sub dgvcalendar_SelectionChanged(sender As Object, e As EventArgs) Handles dgvcalendar.SelectionChanged

    End Sub

End Class