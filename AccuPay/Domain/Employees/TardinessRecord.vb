Option Strict On
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("tardinessrecord")>
    Public Class TardinessRecord

        Public Property Year As Integer
        Public Property EmployeeId As Integer
        Public Property FirstOffenseDate As Date

    End Class

End Namespace