Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("employeeofficialbusiness")>
Public Class OfficialBusiness

    Public Const StatusApproved As String = "Approved"

    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    Public Property CreatedBy As Integer?

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property Created As Date

    Public Property LastUpdBy As Integer?

    <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
    Public Property LastUpd As Date?

    Public Property EmployeeID As Integer?

    <Column("OffBusStatus")>
    Public Property Status As String

    <Column("OffBusStartTime")>
    Public Property StartTime As TimeSpan?

    <Column("OffBusEndTime")>
    Public Property EndTime As TimeSpan?

    <Column("OffBusStartDate")>
    Public Property StartDate As Date

    <Column("OffBusEndDate")>
    Public Property EndDate As Date

    Public Property Reason As String

    Public Property Comments As String

End Class
