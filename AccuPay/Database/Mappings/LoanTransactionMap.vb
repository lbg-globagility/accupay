Option Strict On

Imports FluentNHibernate.Mapping
Imports PayrollSys

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
        Map(Function(x) x.LoanScheduleID).Column("EmployeeLoanRecordID")
        Map(Function(x) x.LoanPayPeriodLeft)
        Map(Function(X) X.TotalBalance).Column("TotalBalanceLeft")
        Map(Function(x) x.Amount).Column("DeductionAmount")
    End Sub

End Class
