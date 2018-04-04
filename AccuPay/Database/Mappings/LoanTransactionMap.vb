Option Strict On

Imports FluentNHibernate.Mapping
Imports AccuPay.Loans

Public Class LoanTransactionMap
    Inherits ClassMap(Of LoanTransaction)

    Public Sub New()
        Table("scheduledloansperpayperiod")

        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.Created).Generated.Insert()
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpd).Generated.Always()
        Map(Function(x) x.LastUpdBy)

        Map(Function(x) x.PayPeriodID)
        Map(Function(x) x.EmployeeID)
        Map(Function(x) x.LoanPayPeriodLeft)
        Map(Function(x) x.TotalBalance).Column("TotalBalanceLeft")
        Map(Function(x) x.Amount).Column("DeductionAmount")

        References(Function(x) x.LoanSchedule).Column("EmployeeLoanRecordID")
    End Sub

End Class
