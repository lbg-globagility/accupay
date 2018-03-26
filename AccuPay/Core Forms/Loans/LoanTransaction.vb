Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Namespace Global.PayrollSys

    <Table("scheduledloansperpayperiod")>
    Public Class LoanTransaction

        <Key>
        Public Overridable Property RowID As Integer?

        Public Overridable Property Created As Date

        Public Overridable Property CreatedBy As Integer?

        Public Overridable Property LastUpd As Date

        Public Overridable Property LastUpdBy As Integer?

        Public Overridable Property OrganizationID As Integer?

        Public Overridable Property PayPeriodID As Integer?

        Public Overridable Property EmployeeID As Integer?

        <Column("EmployeeLoanRecordID")>
        Public Overridable Property LoanScheduleID As Integer?

        Public Overridable Property LoanPayPeriodLeft As Integer

        <Column("TotalBalanceLeft")>
        Public Overridable Property TotalBalance As Decimal

        <Column("DeductionAmount")>
        Public Overridable Property Amount As Decimal

        <ForeignKey("LoanScheduleID")>
        Public Overridable Property LoanSchedule As LoanSchedule

    End Class

End Namespace
