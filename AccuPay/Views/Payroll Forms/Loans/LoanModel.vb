Option Strict On

Imports AccuPay.Data.Entities

Public Class LoanModel

    Public Property Id As Integer?
    Public Property EmployeeId As Integer
    Public Property LoanTypeId As Integer?
    Public Property LoanType As Product
    Public Property LoanNumber As String
    Public Property TotalLoanAmount As Decimal
    Public Property TotalBalanceLeft As Decimal
    Public Property DeductionAmount As Decimal
    Public Property EffectiveFrom As Date
    Public Property Status As String
    Public Property InterestPercentage As Decimal
    Public Property DeductionSchedule As String
    Public Property Comments As String
    Public ReadOnly Property HasTransactions As Boolean
    Public ReadOnly Property IsUnEditable As Boolean

    Public ReadOnly Property LoanTypeName As String
        Get
            Return LoanType?.PartNo
        End Get
    End Property

    Private Sub New(loan As LoanSchedule)
        Me.Id = loan?.RowID
        Me.EmployeeId = If(loan?.EmployeeID, 0)
        Me.LoanTypeId = loan?.LoanTypeID
        Me.LoanType = loan?.LoanType
        Me.LoanNumber = loan?.LoanNumber
        Me.TotalLoanAmount = If(loan?.TotalLoanAmount, 0)
        Me.TotalBalanceLeft = If(loan?.TotalBalanceLeft, 0)
        Me.EffectiveFrom = If(loan?.DedEffectiveDateFrom, Date.Now)
        Me.DeductionAmount = If(loan?.DeductionAmount, 0)
        Me.Status = loan?.Status
        Me.InterestPercentage = If(loan?.DeductionPercentage, 0)
        Me.DeductionSchedule = loan?.DeductionSchedule
        Me.Comments = loan?.Comments

        Me.HasTransactions = If(loan?.HasTransactions, False)
        Me.IsUnEditable = If(loan?.IsUnEditable, False)
    End Sub

    Public Shared Function Create(loan As LoanSchedule) As LoanModel

        Return New LoanModel(loan)

    End Function

    Public Function CreateLoan() As LoanSchedule

        Dim totalBalance = If(Me.Id Is Nothing, Me.TotalLoanAmount, Me.TotalBalanceLeft)

        Return New LoanSchedule() With {
            .RowID = Me.Id,
            .EmployeeID = Me.EmployeeId,
            .OrganizationID = z_OrganizationID,
            .LoanTypeID = Me.LoanType?.RowID,
            .LoanType = Me.LoanType,
            .LoanNumber = Me.LoanNumber,
            .TotalLoanAmount = Me.TotalLoanAmount,
            .TotalBalanceLeft = totalBalance,
            .DedEffectiveDateFrom = Me.EffectiveFrom,
            .DeductionAmount = Me.DeductionAmount,
            .Status = Me.Status,
            .DeductionPercentage = Me.InterestPercentage,
            .DeductionSchedule = Me.DeductionSchedule,
            .Comments = Me.Comments
        }

    End Function

    Public Function Clone() As LoanModel
        Dim loan = CreateLoan()

        Return New LoanModel(loan)

    End Function

End Class
