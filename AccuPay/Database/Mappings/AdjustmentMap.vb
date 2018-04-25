Option Strict On

Imports FluentNHibernate.Mapping
Imports AccuPay.Entity

Public Class AdjustmentMap
    Inherits ClassMap(Of Adjustment)

    Public Sub New()
        Table("paystubadjustment")

        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.Created).Generated.Insert()
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpd).Generated.Always()
        Map(Function(x) x.LastUpdBy)

        Map(Function(x) x.ProductID)
        Map(Function(x) x.Amount).Column("PayAmount")
        Map(Function(x) x.Comment)
        Map(Function(x) x.IsActual)

        References(Function(x) x.Paystub).Column("PaystubID")
    End Sub

End Class
