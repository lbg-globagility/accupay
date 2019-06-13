Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeeallowance")>
    Public Class Allowance

        Public Const FREQUENCY_ONE_TIME As String = "One time"

        Public Const FREQUENCY_DAILY As String = "Daily"

        Public Const FREQUENCY_SEMI_MONTHLY As String = "Semi-monthly"

        Public Const FREQUENCY_MONTHLY As String = "Monthly"

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

        Public Property EffectiveEndDate As Date?

        <NotMapped>
        Public Property TaxableFlag As Char

        <Column("AllowanceAmount")>
        Public Property Amount As Decimal

        <ForeignKey("ProductID")>
        Public Overridable Property Product As Product

        <NotMapped>
        Public Property Type As String
            Get
                Return Product?.PartNo
            End Get
            Set(value As String)
                If Product IsNot Nothing Then Product.PartNo = value
            End Set
        End Property

        Public ReadOnly Property IsOneTime As Boolean
            Get
                Return AllowanceFrequency = FREQUENCY_ONE_TIME
            End Get
        End Property

        Public ReadOnly Property IsDaily As Boolean
            Get
                Return AllowanceFrequency = FREQUENCY_DAILY
            End Get
        End Property

        Public ReadOnly Property IsSemiMonthly As Boolean
            Get
                Return AllowanceFrequency = FREQUENCY_SEMI_MONTHLY
            End Get
        End Property

        Public ReadOnly Property IsMonthly As Boolean
            Get
                Return AllowanceFrequency = FREQUENCY_MONTHLY
            End Get
        End Property

    End Class

End Namespace