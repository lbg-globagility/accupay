Option Strict On

Imports FluentNHibernate.Mapping
Imports AccuPay.Entity

Namespace Global.AccuPay.Database.Mappings

    Public Class ProductMap
        Inherits ClassMap(Of Product)

        Public Sub New()
            Table("product")

            Id(Function(x) x.RowID).GeneratedBy.Identity()
            Map(Function(x) x.OrganizationID)
            Map(Function(x) x.Created).Generated.Insert()
            Map(Function(x) x.CreatedBy)
            Map(Function(x) x.LastUpd).Generated.Always()
            Map(Function(x) x.LastUpdBy)

            Map(Function(x) x.SupplierID)
            Map(Function(x) x.CategoryID)
            Map(Function(x) x.Name)
            Map(Function(x) x.PartNo)
            Map(Function(x) x.Description)
            Map(Function(x) x.Status)
            Map(Function(x) x.Fixed)
        End Sub

    End Class

End Namespace