Option Strict On

Imports FluentNHibernate.Mapping
Imports AccuPay.Entity

Public Class PaystubItemMap
    Inherits ClassMap(Of PaystubItem)

    Public Sub New()
        Table("paystubitem")

        Id(Function(x) x.RowID).GeneratedBy.Increment()
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.Created).Generated.Insert()
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpd).Generated.Always()
        Map(Function(x) x.LastUpdBy)

        Map(Function(x) x.PayStubID)
        Map(Function(x) x.ProductID)
        Map(Function(x) x.PayAmount)
        Map(Function(x) x.Undeclared)
    End Sub

End Class
