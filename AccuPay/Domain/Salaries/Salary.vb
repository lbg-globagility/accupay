Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations
Imports AccuPay.Entity
Imports AccuPay.Data

Namespace Global.PayrollSys

    <Table("employeesalary")>
    Public Class Salary
        Implements ISalary

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer? Implements ISalary.RowID

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date Implements ISalary.Created

        Public Property CreatedBy As Integer? Implements ISalary.CreatedBy

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date? Implements ISalary.LastUpd

        Public Property LastUpdBy As Integer? Implements ISalary.LastUpdBy

        Public Property EmployeeID As Integer? Implements ISalary.EmployeeID

        Public Property OrganizationID As Integer? Implements ISalary.OrganizationID

        Public Property FilingStatusID As Integer? Implements ISalary.FilingStatusID

        Public Property PositionID As Integer? Implements ISalary.PositionID

        Public Property PayPhilHealthID As Integer? Implements ISalary.PayPhilHealthID

        Public Property PhilHealthDeduction As Decimal Implements ISalary.PhilHealthDeduction

        Public Property HDMFAmount As Decimal Implements ISalary.HDMFAmount

        <Column("Salary")>
        Public Property BasicSalary As Decimal Implements ISalary.BasicSalary

        <Column("UndeclaredSalary")>
        Public Property AllowanceSalary As Decimal Implements ISalary.AllowanceSalary

        <Column("TrueSalary")>
        Public Property TotalSalary As Decimal Implements ISalary.TotalSalary

        <Column("BasicDailyPay")>
        Public Property DailyRate As Decimal Implements ISalary.DailyRate

        <Column("BasicHourlyPay")>
        Public Property HourlyRate As Decimal Implements ISalary.HourlyRate

        Public Property NoOfDependents As Integer Implements ISalary.NoOfDependents

        Public Property MaritalStatus As String Implements ISalary.MaritalStatus

        <Column("EffectiveDateFrom")>
        Public Property EffectiveFrom As Date Implements ISalary.EffectiveFrom

        <Column("EffectiveDateTo")>
        Public Property EffectiveTo As Date? Implements ISalary.EffectiveTo

        Public Property DoPaySSSContribution As Boolean Implements ISalary.DoPaySSSContribution

        Public Property AutoComputePhilHealthContribution As Boolean Implements ISalary.AutoComputePhilHealthContribution

        Public Property AutoComputeHDMFContribution As Boolean Implements ISalary.AutoComputeHDMFContribution

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

        Public ReadOnly Property IsIndefinite As Boolean Implements ISalary.IsIndefinite
            Get
                Return Not EffectiveTo.HasValue
            End Get
        End Property

    End Class

End Namespace