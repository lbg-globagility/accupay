Public Class CinemaTardinessReportModel

    Public Const DaysLateLimit As Integer = 8

    Public Property EmployeeId As Integer
    Public Property EmployeeName As String
    Public Property Days As Decimal
    Public Property Hours As Decimal
    Public Property NumberOfOffense As Integer

    Public ReadOnly Property NumberOfOffenseOrdinal As String
        Get

            If NumberOfOffense < 1 Then Return "-"

            Select Case NumberOfOffense Mod 100
                Case 11, 12, 13
                    Return NumberOfOffense & "th"
            End Select

            Select Case NumberOfOffense Mod 10
                Case 1
                    Return NumberOfOffense & "st"
                Case 2
                    Return NumberOfOffense & "nd"
                Case 3
                    Return NumberOfOffense & "rd"
                Case Else
                    Return NumberOfOffense & "th"
            End Select

        End Get
    End Property

    Public ReadOnly Property Sanction As String
        Get
            If NumberOfOffense < 1 Then

                Return "-"

            ElseIf NumberOfOffense = 1 Then

                Return "Written Reprimand"

            ElseIf NumberOfOffense = 2 Then

                Return "2 day Suspension"

            ElseIf NumberOfOffense = 3 Then

                Return "4 days Suspension"

            ElseIf NumberOfOffense = 4 Then

                Return "10 days Suspension"

            ElseIf NumberOfOffense = 5 Then

                Return "Dismissal with Due Process"

            ElseIf NumberOfOffense > 5 Then

                Return "HR: FOR IMMEDIATE ACTION"

            End If
        End Get
    End Property

    Public Class PerMonth
        Public Property EmployeeId As Integer
        Public Property Month As Integer
        Public Property Days As Integer
        Public Property Hours As Integer

    End Class

End Class