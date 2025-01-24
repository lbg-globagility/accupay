Option Strict On

Public Class BankFileReportProvider
    Implements IReportProvider

    Private ReadOnly _organizationId As Integer
    Public Property Name As String = "Bank File" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub New(organizationId As Integer)
        _organizationId = organizationId

    End Sub

    Public Sub Run() Implements IReportProvider.Run
        Dim form = New BankFileTextFormatForm(_organizationId)
        form.ShowDialog()

    End Sub

End Class
