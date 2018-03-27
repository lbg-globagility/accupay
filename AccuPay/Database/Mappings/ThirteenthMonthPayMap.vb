Option Strict On

Imports AccuPay.Entity
Imports FluentNHibernate.Mapping

Public Class ThirteenthMonthPayMap
    Inherits ClassMap(Of ThirteenthMonthPay)

    Public Sub New()
        Table("thirteenthmonthpay")

        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.Created).Generated.Insert()
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpd).Generated.Always()
        Map(Function(x) x.LastUpdBy)

        Map(Function(x) x.BasicPay)
        Map(Function(x) x.Amount)

        References(Function(x) x.Paystub, "PaystubID").Unique()
    End Sub

End Class
