Option Strict On

Public Class LeaveType

    Public Const Sick As String = "Sick leave"

    Public Const Vacation As String = "Vacation leave"

    Public Const Maternity As String = "Maternity leave"

    Public Const Parental As String = "Parental"

    Public Const Others As String = "Others"

    Public Enum LeaveType

        Sick = 0
        Vacation = 1
        Others = 2
        Maternity = 3
        Parental = 4

    End Enum

End Class
