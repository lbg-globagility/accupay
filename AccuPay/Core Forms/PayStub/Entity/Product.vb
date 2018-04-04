Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("product")>
    Public Class Product

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property RowID As Integer?

        Public Overridable Property SupplierID As Integer?

        Public Overridable Property Name As String

        Public Overridable Property OrganizationID As Integer?

        Public Overridable Property Description As String

        Public Overridable Property PartNo As String

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property Created As Date

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Overridable Property LastUpd As Date?

        <NotMapped>
        Public Overridable Property LastArrivedQty As Integer?

        Public Overridable Property CreatedBy As Integer?

        Public Overridable Property LastUpdBy As Integer?

        Public Overridable Property Category As String

        Public Overridable Property CategoryID As Integer?

        <NotMapped>
        Public Overridable Property AccountingAccountID As Integer?

        <NotMapped>
        Public Overridable Property Catalog As String

        <NotMapped>
        Public Overridable Property Comments As String

        Public Overridable Property Status As String

        Public Overridable Property Fixed As Boolean

        <NotMapped>
        Public Overridable Property UnitPrice As Decimal

        <NotMapped>
        Public Overridable Property VATPercent As Decimal

        <NotMapped>
        Public Overridable Property FirstBillFlag As Char

        <NotMapped>
        Public Overridable Property SecondBillFlag As Char

        <NotMapped>
        Public Overridable Property ThirdBillFlag As Char

        <NotMapped>
        Public Overridable Property PDCFlag As Char

        <NotMapped>
        Public Overridable Property MonthlyBIllFlag As Char

        <NotMapped>
        Public Overridable Property PenaltyFlag As Char

        <NotMapped>
        Public Overridable Property WithholdingTaxPercent As Decimal

        <NotMapped>
        Public Overridable Property CostPrice As Decimal

        <NotMapped>
        Public Overridable Property UnitOfMeasure As String

        <NotMapped>
        Public Overridable Property SKU As String

        <NotMapped>
        Public Overridable Property LeadTime As Integer?

        <NotMapped>
        Public Overridable Property BarCode As String

        <NotMapped>
        Public Overridable Property BusinessUnitID As Integer?

        <NotMapped>
        Public Overridable Property LastRcvdFromShipmentDate As Date

        <NotMapped>
        Public Overridable Property LastRcvdFromShipmentCount As Integer?

        <NotMapped>
        Public Overridable Property TotalShipmentCount As Integer?

        <NotMapped>
        Public Overridable Property BookPageNo As String

        <NotMapped>
        Public Overridable Property BrandName As String

        <NotMapped>
        Public Overridable Property LastPurchaseDate As Date

        <NotMapped>
        Public Overridable Property LastSoldDate As Date

        <NotMapped>
        Public Overridable Property LastSoldCount As Integer?

        <NotMapped>
        Public Overridable Property ReOrderPoint As Integer?

        <NotMapped>
        Public Overridable Property AllocateBelowSafetyFlag As Char

        <NotMapped>
        Public Overridable Property Strength As String

        <NotMapped>
        Public Overridable Property UnitsBackordered As Integer?

        <NotMapped>
        Public Overridable Property UnitsBackorderAsOf As Date

        <NotMapped>
        Public Overridable Property DateLastInventoryCount As Date

        <NotMapped>
        Public Overridable Property TaxVAT As Decimal

        <NotMapped>
        Public Overridable Property WithholdingTax As Decimal

        <NotMapped>
        Public Overridable Property COAId As Integer?

        <NotMapped>
        Public Overridable Property ActiveData As Char

        Public Overridable ReadOnly Property IsTaxable As Boolean
            Get
                Return Status = "1"
            End Get
        End Property

    End Class

End Namespace
