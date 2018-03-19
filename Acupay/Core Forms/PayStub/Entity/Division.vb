Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("division")>
    Public Class Division

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

        Public Property Name As String

        Public Property ParentDivisionID As Integer?

        <ForeignKey("ParentDivisionID")>
        Public Overridable Property ParentDivision As Division

        Public ReadOnly Property IsRoot As Boolean
            Get
                Return ParentDivisionID Is Nothing
            End Get
        End Property

        Public Function IsParent(division As Division) As Boolean
            Return Nullable.Equals(ParentDivisionID, division.RowID)
        End Function

    End Class

End Namespace
