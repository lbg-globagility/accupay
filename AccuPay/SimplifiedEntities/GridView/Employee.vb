Option Strict On

Namespace Global.AccuPay.SimplifiedEntities.GridView

    Public Class Employee
        Implements IEmployeeBase

        Public Property RowID As Integer? Implements IEmployeeBase.RowID

        Public Property EmployeeID As String Implements IEmployeeBase.EmployeeID

        Public Property FirstName As String Implements IEmployeeBase.FirstName

        Public Property LastName As String Implements IEmployeeBase.LastName


        Public Function FullName() As String
            Return FirstName + " " + LastName
        End Function

        Public Overrides Function ToString() As String
            Return EmployeeID
        End Function

    End Class

End Namespace