Option Strict On

Public Class BankFileSecurityBankReportProvider
    Implements IReportProvider

    Private ReadOnly _organizationId As Integer
    Private ReadOnly _userId As Integer
    Public Property Name As String = "Bank File (Security Bank)" Implements IReportProvider.Name

    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub New(organizationId As Integer, userId As Integer)
        _organizationId = organizationId
        _userId = userId
    End Sub

    Public Sub Run() Implements IReportProvider.Run
        Dim form = New BankFileTextFormatSecurityBankForm(_organizationId, userId:=_userId)
        form.ShowDialog()
    End Sub

End Class
