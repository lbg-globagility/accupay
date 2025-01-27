Option Strict On

Imports System.Text.RegularExpressions
Imports AccuPay.Core.Entities

Public Class BankFileModel
    Private _isSelected As Boolean
    Private ReadOnly _paystub As Paystub
    Public ReadOnly Property AccountNumber As String
    Public ReadOnly Property LastName As String
    Public ReadOnly Property FirstName As String
    Public ReadOnly Property MiddleName As String
    Public ReadOnly Property Amount As Decimal
    Public ReadOnly Property CompanyName As String

    Public Sub New(paystub As Paystub)
        _paystub = paystub

        AccountNumber = Regex.Replace(_paystub.Employee.AtmNo, "\D", String.Empty)
        LastName = _paystub.Employee.LastName
        FirstName = _paystub.Employee.FirstName
        MiddleName = _paystub.Employee.MiddleName
        Amount = _paystub.NetPay
        CompanyName = String.Empty
    End Sub

    Public Sub New(models As List(Of BankFileModel))
        Amount = models.Sum(Function(t) t.Amount)
    End Sub

    Public Property IsSelected As Boolean
        Get
            Return _isSelected
        End Get
        Set(ByVal value As Boolean)
            If _paystub Is Nothing Then
                value = False
            End If
            _isSelected = value
        End Set
    End Property

    Public ReadOnly Property DataHash As String
        Get
            Dim acctNum = $"{AccountNumber}"
            If acctNum.Trim.Length = 0 Then Return "0"

            Dim given1 = CDec(Mid(acctNum, 5, 2)) * Amount
            Dim given2 = CDec(Mid(acctNum, 7, 2)) * Amount
            Dim given3 = CDec(Mid(acctNum, 9, 2)) * Amount

            Dim result = given1 + given2 + given3
            Return result.ToString("000000000000")
        End Get
    End Property

    Public ReadOnly Property DataHashInt As Integer
        Get
            Return CInt(DataHash)
        End Get
    End Property

    Public ReadOnly Property AccountNumberDecimal As Decimal
        Get
            If String.IsNullOrEmpty(AccountNumber) Then Return 0D

            Return CDec(AccountNumber)
        End Get
    End Property

    Public Function GetDetails(companyCode As String,
            payrollDate As String,
            batchNo As String) As String

        Dim totalSummary = Math.Round(Amount, 2).ToString().Replace(".", String.Empty)

        Return String.Concat("D",
            companyCode,
            payrollDate,
            batchNo,
            "3",
            AccountNumberDecimal.ToString("0000000000"),
            CInt(totalSummary).ToString("000000000000"),
            DataHash,
            Space(79))
    End Function

    Public ReadOnly Property MiddleInitial As String
        Get
            Dim properMiddleName As String = Replace(
                Replace(
                    Replace(Replace(MiddleName, Find:=".", Replacement:=""),
                        Find:="Jr",
                        Replacement:=""),
                    Find:="jr",
                    Replacement:=""),
                Find:="JR",
                Replacement:="")

            properMiddleName = Replace(
                Replace(
                    Replace(properMiddleName,
                        Find:="Jr.",
                        Replacement:=""),
                    Find:="jr.",
                    Replacement:=""),
                Find:="JR.",
                Replacement:="")?.
                Trim()

            Dim middleInitials = Split(properMiddleName, Delimiter:=" ").
                Select(Function(t) Left(t, 1))

            Return String.Join(separator:="", middleInitials)
        End Get
    End Property

    Friend Shared Function BankFileModelSummary(models As List(Of BankFileModel)) As BankFileModel
        Return New BankFileModel(models:=models)
    End Function

    Public ReadOnly Property ObviousErrorDescription As String
        Get
            Dim errorDescriptions As New List(Of String)

            If String.IsNullOrEmpty(AccountNumber) Then errorDescriptions.Add("Invalid Account No.")

            Return String.Join("; ", errorDescriptions)
        End Get
    End Property

    Public ReadOnly Property HasError As Boolean
        Get
            Return Not String.IsNullOrEmpty(ObviousErrorDescription)
        End Get
    End Property

End Class
