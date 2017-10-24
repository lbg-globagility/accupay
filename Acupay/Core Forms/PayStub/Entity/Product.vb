Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("product")>
    Public Class Product

        <Key>
        Public Property RowID As Integer?

        Public Property SupplierID As Integer?

        Public Property Name As String

        Public Property OrganizationID As Integer?

        Public Property Description As String

        Public Property PartNo As String

        Public Property Created As Date

        Public Property LastUpd As Date

        <NotMapped>
        Public Property LastArrivedQty As Integer?

        Public Property CreatedBy As Integer?

        Public Property LastUpdBy As Integer?

        <NotMapped>
        Public Property Category As String

        <NotMapped>
        Public Property CategoryID As Integer?

        <NotMapped>
        Public Property AccountingAccountID As Integer?

        <NotMapped>
        Public Property Catalog As String

        <NotMapped>
        Public Property Comments As String

        <NotMapped>
        Public Property Status As String

        <NotMapped>
        Public Property Fixed As Boolean

        <NotMapped>
        Public Property UnitPrice As Decimal

        <NotMapped>
        Public Property VATPercent As Decimal

        <NotMapped>
        Public Property FirstBillFlag As Char

        <NotMapped>
        Public Property SecondBillFlag As Char

        <NotMapped>
        Public Property ThirdBillFlag As Char

        <NotMapped>
        Public Property PDCFlag As Char

        <NotMapped>
        Public Property MonthlyBIllFlag As Char

        <NotMapped>
        Public Property PenaltyFlag As Char

        <NotMapped>
        Public Property WithholdingTaxPercent As Decimal

        <NotMapped>
        Public Property CostPrice As Decimal

        <NotMapped>
        Public Property UnitOfMeasure As String

        <NotMapped>
        Public Property SKU As String

        <NotMapped>
        Public Property LeadTime As Integer?

        <NotMapped>
        Public Property BarCode As String

        <NotMapped>
        Public Property BusinessUnitID As Integer?

        <NotMapped>
        Public Property LastRcvdFromShipmentDate As Date

        <NotMapped>
        Public Property LastRcvdFromShipmentCount As Integer?

        <NotMapped>
        Public Property TotalShipmentCount As Integer?

        <NotMapped>
        Public Property BookPageNo As String

        <NotMapped>
        Public Property BrandName As String

        <NotMapped>
        Public Property LastPurchaseDate As Date

        <NotMapped>
        Public Property LastSoldDate As Date

        <NotMapped>
        Public Property LastSoldCount As Integer?

        <NotMapped>
        Public Property ReOrderPoint As Integer?

        <NotMapped>
        Public Property AllocateBelowSafetyFlag As Char

        <NotMapped>
        Public Property Strength As String

        <NotMapped>
        Public Property UnitsBackordered As Integer?

        <NotMapped>
        Public Property UnitsBackorderAsOf As Date

        <NotMapped>
        Public Property DateLastInventoryCount As Date

        <NotMapped>
        Public Property TaxVAT As Decimal

        <NotMapped>
        Public Property WithholdingTax As Decimal

        <NotMapped>
        Public Property COAId As Integer?

        <NotMapped>
        Public Property ActiveData As Char

    End Class

End Namespace
