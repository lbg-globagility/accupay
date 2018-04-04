Option Strict On

Imports FluentNHibernate.Mapping
Imports AccuPay.Loans

Namespace Global.AccuPay.Database.Mappings

    Public Class LoanScheduleMap
        Inherits ClassMap(Of LoanSchedule)

        Public Sub New()
            Id(Function(x) x.RowID).GeneratedBy.Identity()
            Map(Function(x) x.OrganizationID)

            Map(Function(x) x.EmployeeID)
            Map(Function(x) x.LoanTypeID)
            Map(Function(x) x.BonusID)
            Map(Function(x) x.LoanNumber)
            Map(Function(x) x.DedEffectiveDateFrom)
            Map(Function(x) x.DedEffectiveDateTo)
            Map(Function(x) x.TotalLoanAmount)
            Map(Function(x) x.DeductionSchedule)
            Map(Function(x) x.DeductionAmount)
            Map(Function(x) x.TotalBalanceLeft)
            Map(Function(x) x.Status)
            Map(Function(x) x.DeductionPercentage)
            Map(Function(x) x.NoOfPayPeriod)
            Map(Function(x) x.LoanPayPeriodLeft)
            Map(Function(x) x.Comments)
            Map(Function(x) x.LoanName)
        End Sub

    End Class

End Namespace