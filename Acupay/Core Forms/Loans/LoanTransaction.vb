Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Namespace Global.PayrollSys

    <Table("scheduledloansperpayperiod")>
    Public Class LoanTransaction

        <Key>
        Public Property RowID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date

        Public Property LastUpdBy As Integer?

        Public Property OrganizationID As Integer?

        Public Property PayPeriodID As Integer?

        Public Property EmployeeID As Integer?

        <Column("EmployeeLoanRecordID")>
        Public Property LoanScheduleID As Integer?

        Public Property LoanPayPeriodLeft As Integer

        <Column("TotalBalanceLeft")>
        Public Property TotalBalance As Decimal

        <Column("DeductionAmount")>
        Public Property Amount As Decimal

        <ForeignKey("LoanScheduleID")>
        Public Overridable Property LoanSchedule As LoanSchedule

    End Class

End Namespace
