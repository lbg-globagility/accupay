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
    Public Property StartDate As Date?

    <Column("OffBusEndDate")>
    Public Property EndDate As Date?

    Public Property Reason As String

    Public Property Comments As String

    <NotMapped>
    Public Property StartTimeFull As Date?
        Get
            'Using Nothing as output on ternary operator does not work
            If StartTime Is Nothing Then
                Return Nothing
            Else
                Return If(StartDate Is Nothing, Date.Now.ToMinimumHourValue.Add(StartTime.Value),
                                                StartDate.Value.Date.ToMinimumHourValue.Add(StartTime.Value))
            End If
        End Get
        Set(value As Date?)
            'Using Nothing as output on ternary operator does not work
            If value Is Nothing Then
                StartTime = Nothing
            Else
                StartTime = value?.TimeOfDay
            End If
        End Set
    End Property

    <NotMapped>
    Public Property EndTimeFull As Date?
        Get
            'Using Nothing as output on ternary operator does not work
            If EndTime Is Nothing Then
                Return Nothing
            Else
                Return If(EndDate Is Nothing, Date.Now.ToMinimumHourValue.Add(EndTime.Value),
                                                EndDate.Value.Date.ToMinimumHourValue.Add(EndTime.Value))
            End If
        End Get
        Set(value As Date?)
            'Using Nothing as output on ternary operator does not work
            If value Is Nothing Then
                EndTime = Nothing
            Else
                EndTime = value?.TimeOfDay
            End If
        End Set
    End Property

    'Start Date that is not nullable since it should not be nullable
    <NotMapped>
    Public Property ProperStartDate As Date
        Get
            Return If(StartDate, Date.Now.Date)
        End Get
        Set(value As Date)
            StartDate = value
        End Set
    End Property

    'End Date that is not nullable since it should not be nullable
    <NotMapped>
    Public Property ProperEndDate As Date
        Get
            Return If(EndDate, Date.Now.Date)
        End Get
        Set(value As Date)
            EndDate = value
        End Set
    End Property

    Public Function Validate() As String

        If StartDate Is Nothing AndAlso EndDate Is Nothing Then

            Return "Start Date and End Date cannot be both empty."
        End If

        If StartDate IsNot Nothing AndAlso StartTime Is Nothing Then

            Return "Start Time cannot be empty if Start Date has value."
        End If

        If EndDate IsNot Nothing AndAlso EndTime Is Nothing Then

            Return "End Time cannot be empty if End Date has value."
        End If

        If StartTime IsNot Nothing AndAlso StartDate Is Nothing Then

            Return "Start Date cannot be empty if Start Time has value."
        End If

        If EndTime IsNot Nothing AndAlso EndDate Is Nothing Then

            Return "End Date cannot be empty if End Time has value."
        End If

        If StartDate IsNot Nothing AndAlso EndDate IsNot Nothing AndAlso StartDate.Value.Date.Add(StartTime.Value.StripSeconds) >= EndDate.Value.Date.Add(EndTime.Value.StripSeconds) Then

            Return "Start date and time cannot be greater than or equal to End date and time."

        End If

        If Not {StatusPending, StatusApproved}.Contains(Status) Then

            Return "Status is not valid."

        End If

        'Means no error
        Return Nothing

    End Function

End Class