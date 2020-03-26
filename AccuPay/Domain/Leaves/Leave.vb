Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Utilities.Extensions

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
        Public Property StartTimeFull As Date?
            Get
                'Using Nothing as output on ternary operator does not work
                If StartTime Is Nothing Then
                    Return Nothing
                Else
                    Return StartDate.Date.ToMinimumHourValue.Add(StartTime.Value)
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