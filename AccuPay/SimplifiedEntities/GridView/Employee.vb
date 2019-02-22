Option Strict On

Namespace Global.AccuPay.SimplifiedEntities.GridView

    Public Class Employee
        Implements IEmployeeBase

        Public Property RowID As Integer? Implements IEmployeeBase.RowID

        Public Property EmployeeID As String Implements IEmployeeBase.EmployeeID

        Public Property FirstName As String Implements IEmployeeBase.FirstName

        Public Property MiddleName As String Implements IEmployeeBase.MiddleName

        Public Property LastName As String Implements IEmployeeBase.LastName


        Public Function FullName() As String
            Return FirstName + " " + LastName
        End Function

        Public Function FullNameWithMiddleName() As String
            Return FirstName + " " + If(String.IsNullOrWhiteSpace(MiddleName), "", MiddleName + " ") + LastName
        End Function

        Public Function FullNameWithMiddleNameInitial() As String
            Return FirstName + " " + If(String.IsNullOrWhiteSpace(MiddleName), "", MiddleName(0) + ". ") + LastName
        End Function

        Public Overrides Function ToString() As String
            Return EmployeeID
        End Function

    End Class

End Namespace