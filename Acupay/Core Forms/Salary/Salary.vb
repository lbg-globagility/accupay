Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Namespace Global.PayrollSys

    <Table("employeesalary")>
    Public Class Salary

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property EmployeeID As Integer?

        Public Property OrganizationID As Integer?

        Public Property FilingStatusID As Integer?

        Public Property PositionID As Integer?

        Public Property PaySocialSecurityID As Integer?

        Public Property PayPhilHealthID As Integer?

        Public Property PhilHealthDeduction As Decimal

        Public Property HDMFAmount As Decimal

        Public Property BasicPay As Decimal

        <Column("Salary")>
        Public Property BasicSalary As Decimal

        <Column("UndeclaredSalary")>
        Public Property AllowanceSalary As Decimal

        <Column("TrueSalary")>
        Public Property TotalSalary As Decimal

        <Column("BasicDailyPay")>
        Public Property DailyRate As Decimal

        <Column("BasicHourlyPay")>
        Public Property HourlyRate As Decimal

        Public Property NoOfDependents As Integer

        Public Property MaritalStatus As String

        <Column("EffectiveDateFrom")>
        Public Property EffectiveFrom As Date

        <Column("EffectiveDateTo")>
        Public Property EffectiveTo As Date?

        Public Property OverrideDiscardSSSContrib As Boolean

        Public Property OverrideDiscardPhilHealthContrib As Boolean

        Public ReadOnly Property SSSDeduction As Decimal
            Get
                Return If(SocialSecurityBracket?.EmployeeContributionAmount, 0)
            End Get
        End Property

        Public ReadOnly Property IsIndefinite As Boolean
            Get
                Return Not EffectiveTo.HasValue
            End Get
        End Property

        <ForeignKey("PaySocialSecurityID")>
        Public Overridable Property SocialSecurityBracket As SocialSecurityBracket

    End Class

End Namespace
