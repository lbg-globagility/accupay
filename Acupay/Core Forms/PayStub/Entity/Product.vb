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

        Public Property LastArrivedQty As Integer?

        Public Property CreatedBy As Integer?

        Public Property LastUpdBy As Integer?

        Public Property Category As String

        Public Property CategoryID As Integer?

        Public Property AccountingAccountID As Integer?

        Public Property Catalog As String

        Public Property Comments As String

        Public Property Status As String

        Public Property Fixed As Boolean

        Public Property UnitPrice As Decimal

        Public Property VATPercent As Decimal

        Public Property FirstBillFlag As Char

        Public Property SecondBillFlag As Char

        Public Property ThirdBillFlag As Char

        Public Property PDCFlag As Char

        Public Property MonthlyBIllFlag As Char

        Public Property PenaltyFlag As Char

        Public Property WithholdingTaxPercent As Decimal

        Public Property CostPrice As Decimal

        Public Property UnitOfMeasure As String

        Public Property SKU As String

        Public Property LeadTime As Integer?

        Public Property BarCode As String

        Public Property BusinessUnitID As Integer?

        Public Property LastRcvdFromShipmentDate As Date

        Public Property LastRcvdFromShipmentCount As Integer?

        Public Property TotalShipmentCount As Integer?

        Public Property BookPageNo As String

        Public Property BrandName As String

        Public Property LastPurchaseDate As Date

        Public Property LastSoldDate As Date

        Public Property LastSoldCount As Integer?

        Public Property ReOrderPoint As Integer?

        Public Property AllocateBelowSafetyFlag As Char

        Public Property Strength As String

        Public Property UnitsBackordered As Integer?

        Public Property UnitsBackorderAsOf As Date

        Public Property DateLastInventoryCount As Date

        Public Property TaxVAT As Decimal

        Public Property WithholdingTax As Decimal

        Public Property COAId As Integer?

        Public Property ActiveData As Char

    End Class

End Namespace