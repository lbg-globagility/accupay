Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeebonus")>
    Public Class Bonus

        <Key>
        Public Property RowID As Integer

        Public Property OrganizationID As Integer

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property Created As DateTime

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As DateTime?

        Public Property LastUpdBy As Integer?
        Public Property EmployeeID As Integer?
        Public Property ProductID As Integer?
        Public Property EffectiveStartDate As Date?
        Public Property AllowanceFrequency As String
        Public Property EffectiveEndDate As Date?
        Public Property TaxableFlag As String
        Public Property BonusAmount As Decimal?
        Public Property RemainingBalance As Decimal?

    End Class

End Namespace