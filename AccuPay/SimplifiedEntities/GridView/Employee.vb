Option Strict On

Namespace Global.AccuPay.SimplifiedEntities.GridView

    Public Class Employee
        Implements IEmployeeBase

        Public Property RowID As Integer? Implements IEmployeeBase.RowID

        Public Property EmployeeNo As String Implements IEmployeeBase.EmployeeNo

        Public Property FirstName As String Implements IEmployeeBase.FirstName

        Public Property MiddleName As String Implements IEmployeeBase.MiddleName

        Public Property LastName As String Implements IEmployeeBase.LastName

        Public Property Image As Byte() Implements IEmployeeBase.Image

        Public Function FullName() As String
            Return FirstName & " " & LastName
        End Function

        Public Function FullNameWithMiddleName() As String
            Return FirstName & " " & If(String.IsNullOrWhiteSpace(MiddleName), "", MiddleName & " ") & LastName
        End Function

        Public Function FullNameWithMiddleNameInitial() As String
            Return FirstName & " " & If(String.IsNullOrWhiteSpace(MiddleName), "", MiddleName(0) & ". ") + LastName
        End Function

        Public Overrides Function ToString() As String
            Return EmployeeNo
        End Function

    End Class

End Namespace