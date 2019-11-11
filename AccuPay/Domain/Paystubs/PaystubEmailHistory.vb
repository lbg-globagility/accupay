Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Data

Namespace Global.AccuPay.Entity

    <Table("paystubemailhistory")>
    Public Class PaystubEmailHistory

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer

        Public Property PaystubID As Integer
        Public Property ReferenceNumber As String
        Public Property SentDateTime As Date?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property SentBy As Integer

        <ForeignKey("PaystubID")>
        Public Property Paystub As Paystub

    End Class

End Namespace