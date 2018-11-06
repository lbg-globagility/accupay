Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeeallowance")>
    Public Class Allowance

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property EmployeeID As Integer?

        Public Property ProductID As Integer?

        Public Property EffectiveStartDate As Date

        Public Property AllowanceFrequency As String

        Public Property EffectiveEndDate As Date

        Public Property TaxableFlag As Char

        <Column("AllowanceAmount")>
        Public Property Amount As Decimal

        <ForeignKey("ProductID")>
        Public Overridable Property Product As Product

        Public ReadOnly Property IsOneTime As Boolean
            Get
                Return AllowanceFrequency = "One time"
            End Get
        End Property

        Public ReadOnly Property IsDaily As Boolean
            Get
                Return AllowanceFrequency = "Daily"
            End Get
        End Property

        Public ReadOnly Property IsSemiMonthly As Boolean
            Get
                Return AllowanceFrequency = "Semi-monthly"
            End Get
        End Property

        Public ReadOnly Property IsMonthly As Boolean
            Get
                Return AllowanceFrequency = "Monthly"
            End Get
        End Property

    End Class

End Namespace
