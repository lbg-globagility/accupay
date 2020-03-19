Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Utilities.Extensions

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

    Public Overridable Property Reason As String

    Public Overridable Property Comments As String

    Public Sub New()
        Status = StatusPending
    End Sub

    <NotMapped>
    Public Property OTStartTimeFull As Date?
        Get
            'Using Nothing as output on ternary operator does not work
            If OTStartTime Is Nothing Then
                Return Nothing
            Else
                Return OTStartDate.Date.ToMinimumHourValue.Add(OTStartTime.Value)
            End If
        End Get
        Set(value As Date?)
            'Using Nothing as output on ternary operator does not work
            If value Is Nothing Then
                OTStartTime = Nothing
            Else
                OTStartTime = value?.TimeOfDay
            End If
        End Set
    End Property

    <NotMapped>
    Public Property OTEndTimeFull As Date?
        Get
            'Using Nothing as output on ternary operator does not work
            If OTEndTime Is Nothing Then
                Return Nothing
            Else
                Return OTEndDate.Date.ToMinimumHourValue.Add(OTEndTime.Value)
            End If
        End Get
        Set(value As Date?)
            'Using Nothing as output on ternary operator does not work
            If value Is Nothing Then
                OTEndTime = Nothing
            Else
                OTEndTime = value?.TimeOfDay
            End If
        End Set
    End Property

    Public Function Validate() As String

        If OTStartTime Is Nothing Then

            Return "Start Time cannot be empty."
        End If

        If OTEndTime Is Nothing Then

            Return "End Time cannot be empty."
        End If

        If OTStartDate.Date.Add(OTStartTime.Value.StripSeconds) >= OTEndDate.Date.Add(OTEndTime.Value.StripSeconds) Then

            Return "Start date and time cannot be greater than or equal to End date and time."

        End If

        If Not {StatusPending, StatusApproved}.Contains(Status) Then

            Return "Status is not valid."

        End If

        'Means no error
        Return Nothing

    End Function

End Class