Option Strict On

Namespace Global.AccuPay.SimplifiedEntities

    Public Interface IEmployeeBase

        Property RowID As Integer?

        Property EmployeeNo As String

        Property FirstName As String

        Property MiddleName As String

        Property LastName As String

        Property Image As Byte()

    End Interface

End Namespace
