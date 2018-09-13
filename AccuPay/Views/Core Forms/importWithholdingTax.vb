Public Class importWithholdingTax

    Dim m_PayFrequencyID As Object = Nothing

    Public Property PayFrequencyID As Object

        Get
            Return m_PayFrequencyID

        End Get

        Set(value As Object)

            m_PayFrequencyID = value

        End Set

    End Property

    Dim m_FilingStatusID As Object = Nothing

    Public Property FilingStatusID As Object

        Get
            Return m_FilingStatusID

        End Get

        Set(value As Object)

            m_FilingStatusID = value

        End Set

    End Property

    Dim m_importfilepath As String = Nothing

    Public Property ImportFilePath As Object

        Get
            Return m_importfilepath

        End Get

        Set(value As Object)

            m_importfilepath = value

        End Set

    End Property

    Dim orgPayFreqID = Nothing

    Private Sub importWithholdingTax_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        cbopayfreq.ContextMenu = New ContextMenu

        cbomaritstat.ContextMenu = New ContextMenu

        txtnumdepend.ContextMenu = New ContextMenu

        enlistToCboBox("SELECT PayFrequencyType FROM payfrequency;", _
                        cbopayfreq)

        'orgPayFreqID = EXECQUER("SELECT PayFrequencyID FROM organization WHERE RowID='" & orgztnID & "';")

        'If orgPayFreqID = Nothing Then
        '    cbopayfreq.Enabled = False
        'Else
        '    cbopayfreq.Text = EXECQUER("SELECT PayFrequencyType FROM payfrequency WHERE RowID='" & orgPayFreqID & "' LIMIT 1;")

        'End If

        enlistToCboBox("SELECT DisplayValue FROM listofval lov WHERE lov.Type='Marital Status' AND Active='Yes'", _
                        cbomaritstat)

        cbomaritstat.Items.Add("None of the above")

    End Sub

    Private Sub txtnumdepend_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnumdepend.KeyPress
        e.Handled = TrapNumKey(Asc(e.KeyChar))

    End Sub

    Private Sub txtnumdepend_TextChanged(sender As Object, e As EventArgs) Handles txtnumdepend.TextChanged

    End Sub

    Private Sub cbomaritstat_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbomaritstat.KeyPress
        e.Handled = True
    End Sub

    Private Sub cbomaritstat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbomaritstat.SelectedIndexChanged

        If cbomaritstat.Text = "None of the above" Then
            txtnumdepend.Text = 0
        Else
            If Val(txtnumdepend.Text) = 0 Then
                txtnumdepend.Text = ""
            End If
        End If

    End Sub

    Private Sub cbopayfreq_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbopayfreq.KeyPress
        e.Handled = True
    End Sub

    Private Sub cbopayfreq_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbopayfreq.SelectedIndexChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        m_PayFrequencyID = EXECQUER("SELECT RowID FROM payfrequency WHERE PayFrequencyType='" & cbopayfreq.Text & "' LIMIT 1;")

        Dim str_maritstatus = If(cbomaritstat.SelectedIndex = (cbomaritstat.Items.Count - 1), "Zero", _
                                Trim(cbomaritstat.Text))

        If str_maritstatus = "Zero" Then
            txtnumdepend.Text = 0
        End If

        m_FilingStatusID = EXECQUER("SELECT RowID FROM filingstatus WHERE MaritalStatus='" & str_maritstatus & "' AND Dependent='" & Val(txtnumdepend.Text) & "' LIMIT 1;")

        m_importfilepath = txtfilepath.Text

        Me.DialogResult = Windows.Forms.DialogResult.OK

        Me.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        m_PayFrequencyID = Nothing

        m_FilingStatusID = Nothing

        m_importfilepath = Nothing

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Me.Close()

    End Sub

    Dim filepath As String = Nothing

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim browsefile As OpenFileDialog = New OpenFileDialog()
        
        browsefile.Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" & _
                                  "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"

        If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

            filepath = IO.Path.GetFullPath(browsefile.FileName)

            txtfilepath.Text = filepath

        Else

            txtfilepath.Text = ""

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