Option Strict On

Public Class BankFileReportProvider
    Implements IReportProvider
    Public Const BANK_FILE_TEXT As String = "Bank File"

    Private ReadOnly _organizationId As Integer
    Public Property Name As String = BANK_FILE_TEXT Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub New(organizationId As Integer)
        _organizationId = organizationId

    End Sub

    Public Sub Run() Implements IReportProvider.Run
        Dim form = New BankFileTextFormatForm(_organizationId)
        form.ShowDialog()

    End Sub

End Class
