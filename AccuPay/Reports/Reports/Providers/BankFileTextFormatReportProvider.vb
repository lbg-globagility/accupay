Public Class BankFileTextFormatReportProvider
    Implements IReportProvider

    Public Property Name As String = "Bank File" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub Run() Implements IReportProvider.Run
        Dim form = New BankFileTextFormatForm
        If Not form.ShowDialog = DialogResult.OK Then Return

    End Sub

End Class
