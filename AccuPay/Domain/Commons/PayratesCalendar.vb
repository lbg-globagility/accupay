Option Strict On

Imports AccuPay.Data

Public Class PayratesCalendar

    Private ReadOnly _payrates As IDictionary(Of Date, IPayrate)

    Public Sub New(payrates As IEnumerable(Of IPayrate))
        _payrates = payrates.ToDictionary(Function(p) p.Date)
    End Sub

    Public Function Find([date] As Date) As IPayrate
        If _payrates.ContainsKey([date]) Then
            Return _payrates([date])
        End If

        Return Nothing
    End Function

    Public Function LegalHolidays() As IList(Of IPayrate)
        Return _payrates.
            Where(Function(p) p.Value.IsRegularHoliday).
            Select(Function(p) p.Value).
            ToList()
    End Function

End Class