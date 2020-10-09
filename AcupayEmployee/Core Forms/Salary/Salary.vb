Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Namespace Global.PayrollSys

    <Table("employeesalary")>
    Public Class Salary

        <Key>
        Public Property RowID As Integer?

        Public Property EmployeeID As Integer?

        Public Property OrganizationID As Integer?

        Public Property PositionID As Integer?

        Public Property PaySocialSecurityID As Integer?

        Public Property PayPhilHealthID As Integer?

        Public Property HDMFAmount As Decimal

        Public Property TrueSalary As Decimal

        Public Property BasicPay As Decimal

        <Column("Salary")>
        Public Property Amount As Decimal

        Public Property UndeclaredSalary As Decimal

        Public Property BasicDailyPay As Decimal

        Public Property BasicHourlyPay As Decimal

        Public Property NoOfDependents As Integer

        Public Property MaritalStatus As String

        Public Property EffectiveDateFrom As Date

        Public Property EffectiveDateTo As Date?

        Public Property OverrideDiscardSSSContrib As Boolean

        Public Property OverrideDiscardPhilHealthContrib As Boolean

        Public ReadOnly Property IsIndefinite As Boolean
            Get
                Return Not EffectiveDateTo.HasValue
            End Get
        End Property

    End Class

End Namespace
