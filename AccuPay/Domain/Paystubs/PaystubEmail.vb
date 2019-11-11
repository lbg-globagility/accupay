Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Data

Namespace Global.AccuPay.Entity

    <Table("paystubemail")>
    Public Class PaystubEmail
        Implements IPaystubEmail

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer Implements IPaystubEmail.RowID

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date Implements IPaystubEmail.Created

        Public Property CreatedBy As Integer Implements IPaystubEmail.CreatedBy
        Public Property PaystubID As Integer Implements IPaystubEmail.PaystubID
        Public Property ProcessingStarted As Date? Implements IPaystubEmail.ProcessingStarted

        Public Property ErrorLogMessage As String Implements IPaystubEmail.ErrorLogMessage
        Public Property Status As String Implements IPaystubEmail.Status

        <ForeignKey("PaystubID")>
        Public Property Paystub As Paystub

    End Class

End Namespace