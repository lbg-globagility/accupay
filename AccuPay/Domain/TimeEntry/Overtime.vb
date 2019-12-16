Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("employeeovertime")>
Public Class Overtime

    Public Const StatusApproved = "Approved"

    Public Const StatusPending = "Pending"

    Public Const DefaultType = "Overtime"

    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Overridable Property RowID As Integer?

    Public Overridable Property OrganizationID As Integer?

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Overridable Property Created As DateTime

    Public Overridable Property CreatedBy As Integer?

    <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
    Public Overridable Property LastUpd As DateTime?

    Public Overridable Property LastUpdBy As Integer?

    Public Overridable Property EmployeeID As Integer?

    <Column("OTType")>
    Public Overridable Property Type As String

    Public Overridable Property OTStartTime As TimeSpan?

    Public Overridable Property OTEndTime As TimeSpan?

    Public Overridable Property OTStartDate As Date

    Public Overridable Property OTEndDate As Date

    <Column("OTStatus")>
    Public Overridable Property Status As String

    <NotMapped>
    Public Overridable Property Start As Date?

    <NotMapped>
    Public Overridable Property [End] As Date?

    Public Sub New()
        Status = StatusPending
    End Sub

End Class