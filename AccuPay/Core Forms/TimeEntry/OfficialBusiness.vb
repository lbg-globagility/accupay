Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("employeeofficialbusiness")>
Public Class OfficialBusiness

    <Key>
    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    Public Property CreatedBy As Integer?

    Public Property Created As Date

    Public Property LastUpdBy As Integer?

    Public Property LastUpd As Date?

    Public Property EmployeeID As Integer?

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
