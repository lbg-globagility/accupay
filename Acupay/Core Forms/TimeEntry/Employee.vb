Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Entity

<Table("employee")>
Public Class Employee

    <Key>
    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    Public Property PositionID As Integer?

    Public Property FirstName As String

    Public Property LastName As String

    Public Property MiddleName As String

    Public ReadOnly Property MiddleInitial As String
        Get
            Return If(String.IsNullOrEmpty(MiddleName), Nothing, MiddleName.Substring(0, 1))
        End Get
    End Property

    <Column("EmployeeID")>
    Public Property EmployeeNo As String

    Public Property AdvancementPoints As Integer

    <ForeignKey("PositionID")>
    Public Overridable Property Position As Position

End Class
