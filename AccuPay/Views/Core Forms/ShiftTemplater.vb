Imports Femiani.Forms.UI.Input
Imports Microsoft.Win32

Public Enum MonthName As Integer
    January = 1
    February = 2
    March = 3
    April = 4
    May = 5
    June = 6
    July = 7
    August = 8
    September = 9
    October = 10
    November = 11
    December = 12
End Enum

Public Class ShiftTemplater

    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Dim machineShortTime As String = RegKey.GetValue("sShortTime").ToString

    Dim yearnow As Integer = CInt(EXECQUER("SELECT YEAR(CURDATE());"))

    Protected Overrides Sub OnLoad(e As EventArgs)

        linkPrev.Text = "← " & (yearnow - 1)

        linkNxt.Text = (yearnow + 1) & " →"

        VIEW_dates_weekly()

        MyBase.OnLoad(e)

    End Sub

    Private Sub VIEW_dates_weekly()

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL `VIEW_dates_weekly`('" & yearnow & "');")

        Dim dtresult = n_SQLQueryToDatatable.ResultTable

        Dim column_count = dtresult.Columns.Count - 1

        Dim row_array(column_count)

        RemoveHandler dgvcalendar.CurrentCellChanged, AddressOf dgvcalendar_CurrentCellChanged

        dgvcalendar.Rows.Clear()

        For Each drow As DataRow In dtresult.Rows

            Dim n_dgvrow = dgvcalendar.Rows.Add() 'New DataGridViewRow()

            For i = 0 To column_count
                Dim dayvalue = CDate(drow(i)).Day

                Dim monthvalue = Format(CDate(drow(i)), "yyyy-MM-dd") 'CDate(drow(i)).Month

                Dim month_int = CDate(drow(i)).Month

                'Dim custom_value = dayvalue & "@" & monthvalue

                If month_int Mod 2 = 0 Then

                    'dgvcalendar.Item(i, n_dgvrow).Style.BackColor = Color.White
                Else

                    dgvcalendar.Item(i, n_dgvrow).Style.BackColor = Color.FromArgb(242, 242, 242)

                End If

                'row_array(i) = custom_value

                dgvcalendar.Item(i, n_dgvrow).Value = dayvalue

                Dim values_array(2) As String

                values_array(0) = monthvalue

                values_array(2) = dayvalue

                dgvcalendar.Item(i, n_dgvrow).Tag = values_array

            Next

            'dgvcalendar.Rows.Add(row_array)

        Next

        AddHandler dgvcalendar.CurrentCellChanged, AddressOf dgvcalendar_CurrentCellChanged

        dgvcalendar_CurrentCellChanged(dgvcalendar, New EventArgs)

    End Sub

    Private Sub dgvcalendar_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvcalendar.CellPainting

        If e.RowIndex = -1 Then

            Dim n_DrawGridCustomHeaderColumns As _
                New DrawGridCustomHeaderColumns(dgvcalendar,
                                                e,
                                                My.Resources.ColumnBGStyle005,
                                                DGVHeaderImageAlignments.FillCell)

        End If

    End Sub

    Private Sub ShiftTemplater_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub dgvcalendar_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvcalendar.CellContentClick
        Dim i = e.RowIndex

        Dim ii = e.ColumnIndex

    End Sub

    Private Sub dgvcalendar_CellClick(sender As Object, e As DataGridViewCellEventArgs) 'Handles dgvcalendar.CellClick

        If e.ColumnIndex > -1 _
            And e.RowIndex > -1 Then

            Dim value_obj = dgvcalendar.Item(e.ColumnIndex, e.RowIndex).Tag(0)

            Dim catchvalue = Format(CDate(value_obj), "MM")

            Dim shift_value = dgvcalendar.Item(e.ColumnIndex, e.RowIndex).Tag(1)

            Label1.Text = monthindextostringname(CInt(catchvalue)) &
                " " & CDate(value_obj).Year

            If shift_value = Nothing Then

                Label2.Text = Nothing
            Else

                Label2.Text = New ExecuteQuery("SELECT " &
                                               "CONCAT(" &
                                               "TIME_FORMAT(TimeFrom, '%l:%i %p')" &
                                               ",' to '" &
                                               ",TIME_FORMAT(TimeTo, '%l:%i %p')" &
                                               ") AS ResultValue" &
                                               " FROM shift" &
                                               " WHERE RowID='" & shift_value & "'" &
                                               " AND OrganizationID='" & orgztnID & "';").Result

            End If
        Else

            Label1.Text = String.Empty

        End If

        'D:\Others\APPLICATION\Sync Fusion Essential Windows Form

    End Sub

    Private Sub dgvcalendar_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvcalendar.RowsAdded

        With dgvcalendar

            .Rows(e.RowIndex).Height = colSun.Width

        End With

    End Sub

    Private Sub dgvcalendar_CurrentCellChanged(sender As Object, e As EventArgs) 'Handles dgvcalendar.CurrentCellChanged

        Try
            dgvcalendar_CellClick(dgvcalendar,
                                  New DataGridViewCellEventArgs(dgvcalendar.CurrentCell.ColumnIndex,
                                                                dgvcalendar.CurrentRow.Index))
        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))

            Label1.Text = String.Empty

        End Try

    End Sub

    Private Sub NavigateYear(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked,
                                                                                            linkNxt.LinkClicked

        Dim objsender = DirectCast(sender, LinkLabel)

        objsender.Enabled = False
        dgvcalendar.Enabled = False

        If objsender.Name = "linkPrev" Then
            yearnow -= 1
        ElseIf objsender.Name = "linkNxt" Then
            yearnow += 1
        End If

        linkPrev.Text = "← " & (yearnow - 1)

        linkNxt.Text = (yearnow + 1) & " →"

        VIEW_dates_weekly()

        objsender.Enabled = True

        dgvcalendar.Enabled = True

    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label1_TextChanged(sender As Object, e As EventArgs) Handles Label1.TextChanged

        'MonthName
        'Label1.Text = MonthName.January
        'monthindextostringname(CInt(Label1.Text))

    End Sub

    Private Function monthindextostringname(ByVal aa As MonthName) As String

        Dim returnvalue = ""

        returnvalue = aa.ToString

        Return returnvalue

    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click, Button2.Click

    End Sub

    Private Sub dgvcalendar_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvcalendar.CellMouseDown

        If e.Button = Windows.Forms.MouseButtons.Right _
            And e.RowIndex > -1 Then

            dgvcalendar.Item(e.ColumnIndex, e.RowIndex).Selected = True

            'dgvcalendar_CellContentClick(dgvcalendar, New DataGridViewCellEventArgs(1, 1))

            'If Application.OpenForms().OfType(Of ShiftList).Count Then
            'End If

            Dim ii = Application.OpenForms().OfType(Of ShiftList).Count

            Dim MousePosition As Point

            MousePosition = Cursor.Position
            'MousePosition = e.Location

            Dim ptX = MousePosition.X - n_ShiftList.Width
            ptX = If(ptX < 0, 0, ptX)

            Dim ptY = MousePosition.Y - n_ShiftList.Height
            ptY = If(ptY < 0, 0, ptY)

            If ii > 0 Then

                n_ShiftList.Location = New Point(ptX,
                                                 ptY)

                n_ShiftList.BringToFront()
            Else

                'For i = 0 To ii
                '    ShiftList.Close()
                '    ShiftList.Dispose()
                'Next

                'n_ShiftList.Size = New Size(1, 1)

                'n_ShiftList.Location = New Point(MousePosition.X,
                '                                 MousePosition.Y)

                'n_ShiftList.Location = New Point((MousePosition.X + n_ShiftList.Width),
                '                                 (MousePosition.Y + n_ShiftList.Height))

                'n_ShiftList.Size = New Size(493, 401)

                n_ShiftList = New ShiftList

                n_ShiftList.Location = New Point(ptX,
                                                 ptY)

                'n_ShiftList.Show()
                If n_ShiftList.ShowDialog("") = Windows.Forms.DialogResult.OK Then

                    Dim i1 = Nothing
                    Dim i2 = Nothing
                    Dim i3 = Nothing

                    'Try
                    i1 = n_ShiftList.ShiftRowID

                    i2 = n_ShiftList.TimeFromValue

                    i3 = n_ShiftList.TimeToValue
                    'Catch ex As Exception
                    '    MsgBox(getErrExcptn(ex, Me.Name), , "n_ShiftList.ShowDialog")
                    'End Try

                    For Each seldgvcells As DataGridViewCell In dgvcalendar.SelectedCells

                        With seldgvcells

                            If i1 = Nothing Then

                                .Tag(1) = Nothing

                                .Value = .Tag(2)
                            Else

                                Dim value_obj = .Tag(0)

                                Dim catchvalue = CDate(value_obj).Day

                                .Tag(1) = i1

                                '.Value = catchvalue & " " &
                                .Value = .Tag(2) & " " &
                                    Format(i2, machineShortTime) &
                                    " to " & Format(i3, machineShortTime)

                            End If

                        End With

                    Next

                End If
                'n_ShiftList.ShowDialog("")

            End If

        ElseIf e.Button = Windows.Forms.MouseButtons.Left Then

            OnClick(New EventArgs)

        End If

    End Sub

    Dim n_ShiftList As New ShiftList

    Private Sub dgvcalendar_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvcalendar.MouseDown

    End Sub

    Protected Overrides Sub OnClick(e As EventArgs)

        Dim ii = Application.OpenForms().OfType(Of ShiftList).Count

        If ii > 0 Then

            n_ShiftList.Close()

        End If

        MyBase.OnClick(e)

    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)

        n_ShiftList.Close()

        MyBase.OnFormClosing(e)

    End Sub

    Private Sub rdbemployee_CheckedChanged(sender As Object, e As EventArgs) Handles rdbemployee.CheckedChanged

        Static once As SByte = 0

        Dim boolresult = rdbemployee.Checked

        If once = 0 Then
            once = 1
        Else
            dgvemp.Visible = boolresult
            dgvpos.Visible = False

        End If

        If boolresult Then
            bgworkempautocomp.RunWorkerAsync()
        End If

    End Sub

    Private Sub rdbposition_CheckedChanged(sender As Object, e As EventArgs) Handles rdbposition.CheckedChanged

        Dim boolresult = rdbposition.Checked

        Static once As SByte = 0

        If once = 0 Then
            once = 1

            dgvpos.Location = New Point(164, 504)

        End If

        dgvpos.Visible = boolresult
        dgvemp.Visible = False

        If boolresult Then
            bgworkposautocomp.RunWorkerAsync()
        End If

    End Sub

    Private Sub dgvemp_VisibleChanged(sender As Object, e As EventArgs) Handles dgvemp.VisibleChanged

        Dim boolresult As Boolean = dgvemp.Visible

        If boolresult Then
            dgvemp.BringToFront()
        End If

    End Sub

    Private Sub dgvpos_VisibleChanged(sender As Object, e As EventArgs) Handles dgvpos.VisibleChanged

        Dim boolresult As Boolean = dgvpos.Visible

        If boolresult Then
            dgvpos.BringToFront()
        End If

    End Sub

    Private Sub bgworkempautocomp_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkempautocomp.DoWork

        AutoCompleteTextBox1.Items.Clear()

        Dim dt As New DataTable

        dt = New SQLQueryToDatatable("SELECT RowID" &
                                     ",EmployeeID" &
                                     ",LastName" &
                                     ",FirstName" &
                                     " FROM employee" &
                                     " WHERE OrganizationID='" & orgztnID & "'" &
                                     " AND EmploymentStatus IN ('Regular','Probationary')" &
                                     " AND RevealInPayroll='1';").ResultTable

        Dim dtcolcount = (dt.Columns.Count - 1)

        For Each drow As DataRow In dt.Rows

            For i = 0 To dtcolcount

                Dim strval = CStr(drow(i).ToString)

                AutoCompleteTextBox1.Items.Add(New AutoCompleteEntry(strval, StringToArray(strval)))

            Next

        Next

    End Sub

    Private Sub bgworkempautocomp_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkempautocomp.ProgressChanged

    End Sub

    Private Sub bgworkempautocomp_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkempautocomp.RunWorkerCompleted
        AutoCompleteTextBox1.Enabled = True
    End Sub

    Private Sub bgworkposautocomp_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkposautocomp.DoWork

        AutoCompleteTextBox1.Items.Clear()

        Dim dt As New DataTable

        dt = New SQLQueryToDatatable("SELECT PositionName" &
                                     " FROM position" &
                                     " WHERE OrganizationID='" & orgztnID & "'" &
                                     " AND DivisionID IS NOT NULL;").ResultTable

        Dim dtcolcount = (dt.Columns.Count - 1)

        For Each drow As DataRow In dt.Rows

            For i = 0 To dtcolcount

                Dim strval = CStr(drow(i).ToString)

                AutoCompleteTextBox1.Items.Add(New AutoCompleteEntry(strval, StringToArray(strval)))

            Next

        Next

    End Sub

    Private Sub bgworkposautocomp_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkposautocomp.ProgressChanged

    End Sub

    Private Sub bgworkposautocomp_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkposautocomp.RunWorkerCompleted
        AutoCompleteTextBox1.Enabled = True
    End Sub

    Private Sub btnSaveDivisionShift_Click(sender As Object, e As EventArgs) Handles btnSaveDivisionShift.Click

    End Sub

    Private Sub dgvcalendar_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvcalendar.KeyDown

        If e.KeyCode = Keys.Apps Then

            Dim dgvcellbounds = dgvcalendar.GetCellDisplayRectangle(dgvcalendar.CurrentCell.ColumnIndex, dgvcalendar.CurrentRow.Index, True)

            'mouse_event(&H3, fsdfs.Location.X, fsdfs.Location.Y, 0, 0)
            'mouse_event(&H5, fsdfs.Location.X, fsdfs.Location.Y, 0, 0)

            dgvcalendar_CellMouseDown(dgvcalendar,
                                      New DataGridViewCellMouseEventArgs(dgvcalendar.CurrentCell.ColumnIndex,
                                                                         dgvcalendar.CurrentRow.Index,
                                                                         dgvcellbounds.Location.X,
                                                                         dgvcellbounds.Location.Y,
                                                                         New MouseEventArgs(Windows.Forms.MouseButtons.Right,
                                                                                            1,
                                                                                            dgvcellbounds.Location.X,
                                                                                            dgvcellbounds.Location.Y, 0)))

            e.Handled = True

        End If

    End Sub

    Private Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Integer, ByVal dx As Integer, ByVal dy As Integer, ByVal cButtons As Integer, ByVal dwExtraInfo As Integer)

    '&H2 means left mouse button down. &H4 means left mouse button up.

    'mouse_event(&H2, 0, 0, 0, 0)
    'mouse_event(&H4, 0, 0, 0, 0)

End Class

Friend Class DrawGridCustomHeaderColumns

    Sub New(ByVal dgv As DataGridView,
     ByVal e As DataGridViewCellPaintingEventArgs, ByVal img As Image,
     ByVal Style As DGVHeaderImageAlignments)

        ' All of the graphical Processing is done here.
        Dim gr As Graphics = e.Graphics
        ' Fill the BackGround with the BackGroud Color of Headers.
        ' This step is necessary, for transparent images, or what's behind
        ' would be painted instead.
        gr.FillRectangle(
         New SolidBrush(dgv.ColumnHeadersDefaultCellStyle.BackColor),
         e.CellBounds)
        If img IsNot Nothing Then
            Select Case Style
                Case DGVHeaderImageAlignments.FillCell
                    gr.DrawImage(
                     img, e.CellBounds.X, e.CellBounds.Y,
                     e.CellBounds.Width, e.CellBounds.Height)
                Case DGVHeaderImageAlignments.SingleCentered
                    gr.DrawImage(img,
                     ((e.CellBounds.Width - img.Width) \ 2) + e.CellBounds.X,
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y,
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.SingleLeft
                    gr.DrawImage(img, e.CellBounds.X,
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y,
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.SingleRight
                    gr.DrawImage(img,
                     (e.CellBounds.Width - img.Width) + e.CellBounds.X,
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y,
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.Tile
                    ' ********************************************************
                    ' To correct: It sould display just a stripe of images,
                    ' long as the whole header, but centered in the header's
                    ' height.
                    ' This code WON'T WORK.
                    ' Any one got any better solution?
                    'Dim rect As New Rectangle(e.CellBounds.X, _
                    ' ((e.CellBounds.Height - img.Height) \ 2), _
                    ' e.ClipBounds.Width, _
                    ' ((e.CellBounds.Height \ 2 + img.Height \ 2)))
                    'Dim br As New TextureBrush(img, Drawing2D.WrapMode.Tile, _
                    ' rect)
                    ' ********************************************************
                    ' This one works... but poorly (the image is repeated
                    ' vertically, too).
                    Dim br As New TextureBrush(img, Drawing2D.WrapMode.Tile)
                    gr.FillRectangle(br, e.ClipBounds)
                Case Else
                    gr.DrawImage(
                     img, e.CellBounds.X, e.CellBounds.Y,
                     e.ClipBounds.Width, e.CellBounds.Height)
            End Select
        End If
        'e.PaintContent(e.CellBounds)
        If e.Value Is Nothing Then
            e.Handled = True
            Return
        End If
        Using sf As New StringFormat
            With sf
                Select Case dgv.ColumnHeadersDefaultCellStyle.Alignment
                    Case DataGridViewContentAlignment.BottomCenter
                        .Alignment = StringAlignment.Center
                        .LineAlignment = StringAlignment.Far
                    Case DataGridViewContentAlignment.BottomLeft
                        .Alignment = StringAlignment.Near
                        .LineAlignment = StringAlignment.Far
                    Case DataGridViewContentAlignment.BottomRight
                        .Alignment = StringAlignment.Far
                        .LineAlignment = StringAlignment.Far
                    Case DataGridViewContentAlignment.MiddleCenter
                        .Alignment = StringAlignment.Center
                        .LineAlignment = StringAlignment.Center
                    Case DataGridViewContentAlignment.MiddleLeft
                        .Alignment = StringAlignment.Near
                        .LineAlignment = StringAlignment.Center
                    Case DataGridViewContentAlignment.MiddleRight
                        .Alignment = StringAlignment.Far
                        .LineAlignment = StringAlignment.Center
                    Case DataGridViewContentAlignment.TopCenter
                        .Alignment = StringAlignment.Center
                        .LineAlignment = StringAlignment.Near
                    Case DataGridViewContentAlignment.TopLeft
                        .Alignment = StringAlignment.Near
                        .LineAlignment = StringAlignment.Near
                    Case DataGridViewContentAlignment.TopRight
                        .Alignment = StringAlignment.Far
                        .LineAlignment = StringAlignment.Near
                End Select
                ' This part could be handled...
                'Select Case dgv.ColumnHeadersDefaultCellStyle.WrapMode
                '	Case DataGridViewTriState.False
                '		.FormatFlags = StringFormatFlags.NoWrap
                '	Case DataGridViewTriState.NotSet
                '		.FormatFlags = StringFormatFlags.NoWrap
                '	Case DataGridViewTriState.True
                '		.FormatFlags = StringFormatFlags.FitBlackBox
                'End Select
                .HotkeyPrefix = Drawing.Text.HotkeyPrefix.None
                .Trimming = StringTrimming.None
            End With

            Dim newForeColor = Color.FromArgb(0, 0, 0)

            With dgv.ColumnHeadersDefaultCellStyle
                gr.DrawString(e.Value.ToString, .Font,
                 New SolidBrush(newForeColor), e.CellBounds, sf)
            End With
        End Using

        e.Handled = True

    End Sub

End Class