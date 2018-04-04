Option Strict On

Imports FluentNHibernate.Mapping
Imports AccuPay.Entity

Public Class AllowancePerDayMap
    Inherits ClassMap(Of AllowancePerDay)

    Public Sub New()
        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.Amount)
        Map(Function(x) x.Date)

        References(Function(x) x.AllowanceItem).Column("AllowanceItemID")
    End Sub

End Class
