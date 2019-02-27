Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations
Imports AccuPay.Entity

Namespace Global.AccuPay.Loans

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

        Public Overridable Property LoanPayPeriodLeft As Integer

        <Column("EmployeeLoanRecordID")>
        Public Overridable Property LoanScheduleID As Integer

        <Column("TotalBalanceLeft")>
        Public Overridable Property TotalBalance As Decimal

        <Column("DeductionAmount")>
        Public Overridable Property Amount As Decimal

        <ForeignKey("LoanScheduleID")>
        Public Overridable Property LoanSchedule As LoanSchedule

        <ForeignKey("PayPeriodID")>
        Public Overridable Property PayPeriod As PayPeriod


        Public ReadOnly Property PayPeriodPayToDate() As Date?
            Get
                Return PayPeriod?.PayToDate
            End Get
        End Property

    End Class

End Namespace
