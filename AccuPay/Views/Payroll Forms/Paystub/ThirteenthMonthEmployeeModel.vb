Option Strict On

Imports AccuPay.Core.Entities

Public Class ThirteenthMonthEmployeeModel
    Public ReadOnly Property EmployeeId As Integer
    Public ReadOnly Property PaystubObject As Paystub
    Public ReadOnly Property EmployeeObject As Employee
    Public ReadOnly Property EmployeeNumber As String
    Public ReadOnly Property FirstName As String
    Public ReadOnly Property MiddleName As String
    Public ReadOnly Property LastName As String
    Public ReadOnly Property FullName As String
    Public ReadOnly Property EmployeeType As String
    Public ReadOnly Property PositionName As String
    Public ReadOnly Property DivisionName As String
    Public ReadOnly Property ThirteenthMonthAmount As Decimal
    Public ReadOnly Property ThirteenthMonthBasicPay As Decimal
    Public Property IsSelected As Boolean
    Public ReadOnly Property Adjustments As List(Of Adjustment)
    Public ReadOnly Property ActualAdjustments As List(Of ActualAdjustment)

    Sub New()

    End Sub

    Sub New(paystub As Paystub)

        EmployeeId = paystub.EmployeeID.Value
        PaystubObject = paystub
        EmployeeObject = paystub.Employee

        EmployeeNumber = paystub.Employee.EmployeeNo
        FirstName = paystub.Employee.FirstName
        MiddleName = paystub.Employee.MiddleName
        LastName = paystub.Employee.LastName
        EmployeeType = paystub.Employee.EmployeeType
        FullName = paystub.Employee.FullNameWithMiddleInitialLastNameFirst

        PositionName = paystub.Employee.Position?.Name
        DivisionName = paystub.Employee.Position?.Division?.Name

        IsSelected = True
        ThirteenthMonthAmount = 0

        Adjustments = PaystubObject.Adjustments.ToList()

        ActualAdjustments = PaystubObject.ActualAdjustments.ToList()

    End Sub

    Public Sub UpdateThirteenthMonthPayAmount(amount As Decimal, basicPay As Decimal)

        _ThirteenthMonthAmount = amount
        _ThirteenthMonthBasicPay = basicPay

    End Sub

End Class
