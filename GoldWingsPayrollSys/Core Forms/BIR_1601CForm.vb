Imports Femiani.Forms.UI.Input

Public Class BIR_1601CForm

    Public TaxDateFrom As Object = Nothing

    Public TaxDateTo As Object = Nothing

    Sub New(Optional date_From As Object = Nothing, _
            Optional date_To As Object = Nothing)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Dim collectEmployeeID = Nothing

    Private Sub BIR_1601CForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        collectEmployeeID = EXECQUER("SELECT GROUP_CONCAT(EmployeeID) FROM employee WHERE OrganizationID='" & orgztnID & "';")

        bgwEmployeeID.RunWorkerAsync()

        ''

        bgReport.RunWorkerAsync()

    End Sub

    Private Sub bgwEmployeeID_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwEmployeeID.DoWork

        Dim arr_collectEmployeeID = Split(collectEmployeeID, ",")

        For Each strval In arr_collectEmployeeID

            AutoCompleteTextBox1.Items.Add(New AutoCompleteEntry(strval, StringToArray(strval)))

        Next

    End Sub

    Private Sub bgwEmployeeID_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwEmployeeID.ProgressChanged

    End Sub

    Private Sub bgwEmployeeID_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwEmployeeID.RunWorkerCompleted

        If e.Error IsNot Nothing Then

            MessageBox.Show("Error: " & e.Error.Message)

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")

        Else

        End If

        AutoCompleteTextBox1.Enabled = True

    End Sub

    Private Sub bgReport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgReport.DoWork

    End Sub

    Private Sub bgReport_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgReport.ProgressChanged

    End Sub

    Private Sub bgReport_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgReport.RunWorkerCompleted

    End Sub

End Class