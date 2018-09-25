Option Strict On
Imports AccuPay.Entity

Public Class PayratesCalendar

    Private ReadOnly _payrates As IDictionary(Of Date, PayRate)

    Public Sub New(payrates As IList(Of PayRate))
        _payrates = payrates.ToDictionary(Function(p) p.Date)
    End Sub

    Public Function Find([date] As Date) As PayRate
        If _payrates.ContainsKey([date]) Then
            Return _payrates([date])
        End If

        Return Nothing
    End Function

End Class
