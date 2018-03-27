Option Strict On

Imports FluentNHibernate.Mapping
Imports AccuPay.Entity

Public Class AllowancePerDayMap
    Inherits ClassMap(Of AllowancePerDay)

    Public Sub New()
        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.AllowanceItemID)
        Map(Function(x) x.Amount)
        Map(Function(x) x.Date)
    End Sub

End Class
