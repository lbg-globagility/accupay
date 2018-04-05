Option Strict On

Imports AccuPay.Entity
Imports FluentNHibernate.Mapping

Public Class LeaveTransactionMap
    Inherits ClassMap(Of LeaveTransaction)

    Public Sub New()
        Table("leavetransaction")

        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.Created).Generated.Insert()
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpd).Generated.Always()
        Map(Function(x) x.LastUpdBy)

        Map(Function(x) x.EmployeeID)
        Map(Function(x) x.PayPeriodID)
        Map(Function(x) x.ReferenceID)
        Map(Function(x) x.TransactionDate)
        Map(Function(x) x.Type)
        Map(Function(x) x.Balance)
        Map(Function(x) x.Amount)

        References(Function(x) x.LeaveLedger).Column("LeaveLedgerID")
    End Sub

End Class
