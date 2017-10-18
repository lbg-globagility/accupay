Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.PayrollSys

    <Table("employeeloanschedule")>
    Public Class LoanSchedule

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property EmployeeID As Integer?

        Public Property LoanTypeID As Integer?

        Public Property BonusID As Integer?

        Public Property LoanNumber As String

        Public Property DedEffectiveDateFrom As Date

        Public Property DedEffectiveDateTo As Date

        Public Property TotalLoanAmount As Decimal

        Public Property DeductionSchedule As String

        Public Property DeductionAmount As Decimal

        Public Property TotalBalanceLeft As Decimal

        Public Property Status As String

        Public Property DeductionPercentage As Decimal

        Public Property NoOfPayPeriod As Decimal

        Public Property LoanPayPeriodLeft As Decimal

        Public Property Comments As String

        Public Property LoanName As String

        Public Overridable Property LoanTransactions As ICollection(Of LoanTransaction)

    End Class

End Namespace
