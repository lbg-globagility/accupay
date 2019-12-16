Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("agencyfee")>
Public Class AgencyFee

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

    Public Property AgencyID As Integer?

    Public Property EmployeeID As Integer?

    Public Property DivisionID As Integer?

    Public Property TimeEntryID As Integer?

    <Column("TimeEntryDate")>
    Public Property [Date] As Date

    <Column("DailyFee")>
    Public Property Amount As Decimal

End Class
