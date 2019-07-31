Option Strict On

Imports AccuPay.Entity

Public Class PayratesCalendar

    Private ReadOnly _payrates As IDictionary(Of Date, PayRate)

    Private ReadOnly _payrates2 As IList(Of PayRate)

    Public Sub New(payrates As IList(Of PayRate))
        _payrates2 = payrates
        _payrates = payrates.ToDictionary(Function(p) p.Date)
    End Sub

    Public Function Find([date] As Date) As PayRate
        If _payrates.ContainsKey([date]) Then
            Return _payrates([date])
        End If

        Return Nothing
    End Function

    Public Function LegalHolidays() As IList(Of PayRate)
        Return _payrates2.Where(Function(p) StrConv(p.PayType, VbStrConv.Lowercase) = "regular holiday").ToList()
    End Function

End Class