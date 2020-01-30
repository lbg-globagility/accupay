Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Extensions

Namespace Global.AccuPay.Entity

    <Table("employeeleave")>
    Public Class Leave

        Public Const StatusApproved = "Approved"

        Public Const StatusPending = "Pending"

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

        Public Overridable Property LeaveType As String

        Public Overridable Property LeaveHours As Decimal

        <Column("LeaveStartTime")>
        Public Overridable Property StartTime As TimeSpan?

        <Column("LeaveEndTime")>
        Public Overridable Property EndTime As TimeSpan?

        <Column("LeaveStartDate")>
        Public Overridable Property StartDate As Date

        <Column("LeaveEndDate")>
        Public Overridable Property EndDate As Date?

        Public Overridable Property Reason As String

        Public Overridable Property Comments As String

        Public Overridable Property Image As Byte()

        Public Overridable Property Status As String

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
                Return If(EndTime Is Nothing, Nothing,
                            If(EndDate Is Nothing, StartDate, EndDate.Value).Date.Add(EndTime.Value))
            End Get
            Set(value As Date)
                EndTime = value.TimeOfDay
            End Set
        End Property

        'End Date that is not nullable since it should not be null
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

            If EndDate Is Nothing Then

                Return "End Date cannot be empty."
            End If

            If (StartTime.HasValue AndAlso EndTime Is Nothing) OrElse
               (EndTime.HasValue AndAlso StartTime Is Nothing) Then

                Return "Both Start Time and End Time should have value or both should be empty."
            End If

            If StartTime Is Nothing And EndTime Is Nothing Then

                If StartDate.Date > EndDate.Value.Date Then

                    Return "Start Date cannot be greater than End Date (empty start and end times)"
                End If
            Else

                If StartDate.Date.Add(StartTime.Value.StripSeconds) >= EndDate.Value.Date.Add(EndTime.Value.StripSeconds) Then

                    Return "Start date and time cannot be greater than or equal to End date and time."

                End If

            End If

            If String.IsNullOrWhiteSpace(LeaveType) Then

                Return "Leave Type cannot be empty."

            End If

            If Not {StatusPending, StatusApproved}.Contains(Status) Then

                Return "Status is not valid."

            End If

            'Means no error
            Return Nothing

        End Function

    End Class

End Namespace