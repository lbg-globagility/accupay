Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("agency")>
Public Class Agency

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

    <Column("AgencyName")>
    Public Property Name As String

    <Column("AgencyFee")>
    Public Property Fee As Decimal

    Public Property IsActive As Boolean

End Class
