Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Data

Namespace Global.AccuPay.Entity

    <Table("paystubemailhistory")>
    Public Class PaystubEmailHistory
        Implements IPaystubEmailHistory

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer Implements IPaystubEmailHistory.RowID

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date Implements IPaystubEmailHistory.Created

        Public Property SentBy As Integer Implements IPaystubEmailHistory.SentBy
        Public Property PaystubID As Integer Implements IPaystubEmailHistory.PaystubID
        Public Property SentDateTime As Date? Implements IPaystubEmailHistory.SentDateTime
        Public Property ReferenceNumber As String Implements IPaystubEmailHistory.ReferenceNumber
        Public Property EmailAddress As String Implements IPaystubEmailHistory.EmailAddress

        <ForeignKey("PaystubID")>
        Public Property Paystub As Paystub

    End Class

End Namespace