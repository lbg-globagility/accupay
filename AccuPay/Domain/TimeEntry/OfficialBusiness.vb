Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Utilities.Extensions

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
    Public Property StartTimeFull As Date?
        Get
            Return If(StartTime Is Nothing, Nothing, StartDate.Date.Add(StartTime.Value))
        End Get
        Set(value As Date?)
            StartTime = If(value Is Nothing, Nothing, value?.TimeOfDay)
        End Set
    End Property

    <NotMapped>
    Public Property EndTimeFull As Date?
        Get
            Return If(EndTime Is Nothing, Nothing, EndDate.Date.Add(EndTime.Value))
        End Get
        Set(value As Date?)
            EndTime = If(value Is Nothing, Nothing, value?.TimeOfDay)
        End Set
    End Property

    Public Function Validate() As String

        If StartTime Is Nothing Then

            Return "Start Time cannot be empty."
        End If

        If EndTime Is Nothing Then

            Return "End Time cannot be empty."
        End If

        If StartDate.Date.Add(StartTime.Value.StripSeconds) >= EndDate.Date.Add(EndTime.Value.StripSeconds) Then

            Return "Start date and time cannot be greater than or equal to End date and time."

        End If

        If Not {StatusPending, StatusApproved}.Contains(Status) Then

            Return "Status is not valid."

        End If

        'Means no error
        Return Nothing

    End Function

End Class