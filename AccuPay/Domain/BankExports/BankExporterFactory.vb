Option Strict On

Public Class BankExporterFactory

    Public Function GetBankExporter(bankName As String) As IBankExporter
        Select Case bankName
            Case "BDO"
                Return New BDOBankExporter()
            Case Else
                Throw New Exception($"'{bankName}' is not supported.")
        End Select
    End Function

End Class
