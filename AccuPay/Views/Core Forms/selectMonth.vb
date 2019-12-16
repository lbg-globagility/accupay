Public Class selectMonth

    Dim m_MonthValue As Object = Nothing

    Property MonthValue As Object

        Get
            Return m_MonthValue

        End Get

        Set(value As Object)
            m_MonthValue = value
        End Set

    End Property

    Dim m_MonthFirstDate = Nothing

    Property MonthFirstDate As Object

        Get
            Return m_MonthFirstDate

        End Get

        Set(value As Object)
            m_MonthFirstDate = value
        End Set

    End Property


    Dim m_MonthLastDate = Nothing

    Property MonthLastDate As Object

        Get
            Return m_MonthLastDate

        End Get

        Set(value As Object)
            m_MonthLastDate = value
        End Set

    End Property


    Dim yearnow = CDate(dbnow).Year

    Private Sub selectMonth_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim listofmonth As New AutoCompleteStringCollection

        enlistTheLists("SELECT DisplayValue FROM listofval WHERE Type='Month' ORDER BY OrderBy;", _
                        listofmonth)

        For Each strval In listofmonth
            lbMonth.Items.Add(strval)
        Next

        If lbMonth.Items.Count <> 0 Then
            lbMonth.SelectedIndex = 0

        End If

        lbMonth_SelectedIndexChanged(sender, e)

        linkPrev.Text = "← " & (yearnow - 1)
        linkNxt.Text = (yearnow + 1) & " →"

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click



        If lbMonth.Items.Count <> 0 Then

            m_MonthFirstDate = CDate(paypFrom)

            m_MonthLastDate = CDate(paypTo)


            Me.DialogResult = Windows.Forms.DialogResult.OK

        Else

            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Me.Close()

    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        yearnow = yearnow - 1

        linkPrev.Text = "← " & (yearnow - 1)
        linkNxt.Text = (yearnow + 1) & " →"

        lbMonth_SelectedIndexChanged(sender, e)

    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        yearnow = yearnow + 1

        linkNxt.Text = (yearnow + 1) & " →"
        linkPrev.Text = "← " & (yearnow - 1)

        lbMonth_SelectedIndexChanged(sender, e)

    End Sub

    Private Sub lbMonth_KeyDown(sender As Object, e As KeyEventArgs) Handles lbMonth.KeyDown

        If e.KeyCode = Keys.Enter Then
            Button1_Click(sender, e)

        End If

    End Sub

    Dim numofweekdays = 0

    Dim numofweekends = 0

    Dim paypFrom = Nothing

    Dim paypTo = Nothing

    Private Sub lbMonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbMonth.SelectedIndexChanged

        If lbMonth.Items.Count <> 0 Then

            If lbMonth.SelectedItem = Nothing Then

                Label1.Text = ""

            Else

                Label1.Text = lbMonth.SelectedItem & " " & yearnow

            End If

            m_MonthValue = lbMonth.SelectedItem & " 1 " & yearnow

            paypFrom = Format(CDate(m_MonthValue), "yyyy-MM-dd")

            paypTo = EXECQUER("SELECT LAST_DAY('" & Format(CDate(m_MonthValue), "yyyy-MM-dd") & "');")

            paypTo = Format(CDate(paypTo), "yyyy-MM-dd")

        Else
            Label1.Text = ""

        End If

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Button2_Click(Button2, New EventArgs)

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

End Class