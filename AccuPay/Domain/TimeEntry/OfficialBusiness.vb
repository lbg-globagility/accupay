Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Entity

<Table("employeeofficialbusiness")>
Public Class OfficialBusiness

    Public Const StatusApproved = "Approved"

    Public Const StatusPending = "Pending"

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

    <Column("OffBusType")>
    Public Property Type As String

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

    <NotMapped>
    Public Property StartTimeFull As Date
        Get
            Return If(StartTime Is Nothing, Nothing, StartDate.Date.Add(StartTime.Value))
        End Get
        Set(value As Date)
            StartTime = value.TimeOfDay
        End Set
    End Property

    <NotMapped>
    Public Property EndTimeFull As Date
        Get
            Return If(EndTime Is Nothing, Nothing, EndDate.Date.Add(EndTime.Value))
        End Get
        Set(value As Date)
            EndTime = value.TimeOfDay
        End Set
    End Property

End Class