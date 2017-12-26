Option Strict On

Imports System.Data.Common
Imports AccuPay.Entity
Imports Acupay

<TestFixture>
Public Class PayrollContextTest

    Private _context As PayrollContext

    <SetUp>
    Public Sub SetUp()
        Dim connection = DbProviderFactories.GetFactory("MySql.Data.MySqlClient").CreateConnection()
        connection.ConnectionString = "server=127.0.0.1;user id=root;password=globagility;database=goldwingspayrolldb;"
        _context = New PayrollContext(connection)
    End Sub

    <TearDown>
    Public Sub TearDown()
        _context.Dispose()
    End Sub

    <Test>
    Public Sub Should_Retrieve_Product()
        Dim product = _context.Products.Find(1)

        Assert.IsInstanceOf(Of Product)(product)
    End Sub

    <Test>
    Public Sub Should_Retrieve_PaystubItem()
        Dim paystubItem = _context.PaystubItems.Find(1)

        Assert.IsInstanceOf(Of PaystubItem)(paystubItem)
    End Sub

    <Test>
    Public Sub Should_Retrieve_Employee()
        Dim employee = _context.Employees.FirstOrDefault()

        Assert.IsInstanceOf(Of Employee)(employee)
    End Sub

    <Test>
    Public Sub Should_Retrieve_PayRate()
        Dim payRate = _context.PayRates.FirstOrDefault()

        Assert.IsInstanceOf(Of PayRate)(payRate)
    End Sub

    <Test>
    Public Sub Should_Retrieve_Organization()
        Dim organization = _context.Organizations.FirstOrDefault()

        Assert.IsInstanceOf(Of Organization)(organization)
    End Sub

End Class