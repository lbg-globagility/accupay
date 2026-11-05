Imports AccuPay.Core.Entities

Public Class ProjectEmployeeModel
    Private ReadOnly _employee As Employee
    Private ReadOnly _originExists As Boolean

    Public Sub New(employee As Employee, isExists As Boolean)
        _employee = employee
        _originExists = isExists

        CustomDisplayText = employee.CustomDisplayText
        CompanyName = employee.OrganizationName
        LastName = employee.LastName
        FirstName = employee.FirstName
        EmployeeNo = employee.EmployeeNo
        JobDescription = employee.Position?.Name

        IsSelected = isExists

    End Sub

    Public ReadOnly Property CustomDisplayText As String
    Public ReadOnly Property CompanyName As String
    Public ReadOnly Property LastName As String
    Public ReadOnly Property FirstName As String
    Public ReadOnly Property EmployeeNo As String
    Public ReadOnly Property JobDescription As String

    Public Property IsSelected As Boolean

    Public ReadOnly Property IsExists As Boolean
        Get
            Return _originExists
        End Get
    End Property

    Public ReadOnly Property HasChanges As Boolean
        Get
            Return Not _originExists = IsSelected
        End Get
    End Property

End Class
