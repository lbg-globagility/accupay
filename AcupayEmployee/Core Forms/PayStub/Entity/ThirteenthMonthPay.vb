﻿Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("thirteenthmonthpay")>
    Public Class ThirteenthMonthPay

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date

        Public Property LastUpdBy As Integer?

        <Column("PayStubID")>
        Public Property PaystubID As Integer?

        Public Property BasicPay As Decimal

        Public Property Amount As Decimal

        <ForeignKey("PaystubID")>
        Public Overridable Property Paystub As Paystub

    End Class

End Namespace