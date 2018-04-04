Option Strict On

Imports AccuPay.Entity
Imports FluentNHibernate.Mapping

Public Class LeaveLedgerMap
    Inherits ClassMap(Of LeaveLedger)

    Public Sub New()
        Table("leaveledger")

        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.Created).Generated.Insert()
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpd).Generated.Always()
        Map(Function(x) x.LastUpdBy)

        Map(Function(x) x.EmployeeID)

        References(Function(x) x.LastTransaction).Column("LastTransactionID")
        References(Function(x) x.Product).Column("ProductID")
    End Sub

End Class
