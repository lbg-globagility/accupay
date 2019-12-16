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

        Public Property PayPhilHealthID As Integer?

        Public Property PhilHealthDeduction As Decimal

        Public Property HDMFAmount As Decimal

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

        Public Property DoPaySSSContribution As Boolean

        Public Property AutoComputePhilHealthContribution As Boolean

        Public Property AutoComputeHDMFContribution As Boolean

        Public ReadOnly Property IsIndefinite As Boolean
            Get
                Return Not EffectiveTo.HasValue
            End Get
        End Property

    End Class

End Namespace
