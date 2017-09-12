Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Namespace Global.PayrollSys

    <Table("scheduledloansperpayperiod")>
    Public Class LoanTransaction

        Public Property RowID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date

        Public Property LastUpdBy As Integer?

        Public Property OrganizatonID As Integer?

        Public Property PayPeriod As Integer?

        Public Property EmployeeID As Integer?

        Public Property LoanScheduleID As Integer?

        Public Property LoanPayPeriodLeft As Integer

        Public Property TotalBalanceLeft As Decimal

    End Class

End Namespace
