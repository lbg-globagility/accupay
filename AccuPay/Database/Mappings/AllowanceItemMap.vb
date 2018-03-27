Option Strict On

Imports FluentNHibernate.Mapping
Imports AccuPay.Entity

Public Class AllowanceItemMap
    Inherits ClassMap(Of AllowanceItem)

    Public Sub New()
        Table("allowanceitem")

        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.Created).Generated.Insert()
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpd).Generated.Always()
        Map(Function(x) x.LastUpdBy)

        Map(Function(x) x.AllowanceID)
        Map(Function(x) x.PayPeriodID)
        Map(Function(x) x.Amount)

        References(Function(x) x.Paystub).Column("PaystubID")
    End Sub

End Class
