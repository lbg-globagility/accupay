Option Strict On

Imports FluentNHibernate.Mapping
Imports AccuPay.Entity

Public Class PaystubItemMap
    Inherits ClassMap(Of PaystubItem)

    Public Sub New()
        Table("paystubitem")

        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.Created).Generated.Insert()
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpd).Generated.Always()
        Map(Function(x) x.LastUpdBy)

        Map(Function(x) x.PayAmount)
        Map(Function(x) x.Undeclared)

        References(Function(x) x.Product).Column("ProductID")
        References(Function(x) x.Paystub).Column("PayStubID")
    End Sub

End Class
