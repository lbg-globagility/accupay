Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("employee")>
Public Class Employee

    <Key>
    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    Public Property FirstName As String

    Public Property LastName As String

    <Column("EmployeeID")>
    Public Property EmployeeNo As String

End Class
