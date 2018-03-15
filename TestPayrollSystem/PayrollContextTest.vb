Option Strict On

Imports System.Data.Common
Imports AccuPay.Entity
Imports Acupay

<TestFixture>
Public Class PayrollContextTest

    Private _context As PayrollContext

    <SetUp>
    Public Sub SetUp()
        _context = New PayrollContext()
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

    <Test>
    Public Sub Should_Retrieve_ShiftSchedule()
        Dim shiftSchedule = _context.ShiftSchedules.FirstOrDefault()

        Assert.IsInstanceOf(Of ShiftSchedule)(shiftSchedule)
    End Sub

    <Test>
    Public Sub Should_Retrieve_TimeEntry()
        Dim timeEntry = _context.TimeEntries.FirstOrDefault()

        Assert.IsInstanceOf(Of TimeEntry)(timeEntry)
    End Sub

    <Test>
    Public Sub Should_Retrieve_WithholdingTaxBracket()
        Dim withholdingTaxBracket = _context.WithholdingTaxBrackets.FirstOrDefault()

        Assert.IsInstanceOf(Of WithholdingTaxBracket)(withholdingTaxBracket)
    End Sub

    <Test>
    Public Sub Should_Retrieve_PayPeriod()
        Dim payPeriod = _context.PayPeriods.FirstOrDefault()

        Assert.IsInstanceOf(Of PayPeriod)(payPeriod)
    End Sub

    <Test>
    Public Sub Should_Retrieve_Allowance()
        Dim allowance = _context.Allowances.FirstOrDefault()

        Assert.IsInstanceOf(Of Allowance)(allowance)
    End Sub

    <Test>
    Public Sub Should_Retrieve_LeaveLedger()
        Dim leaveLedger = _context.LeaveLedgers.FirstOrDefault()

        Assert.IsInstanceOf(Of LeaveLedger)(leaveLedger)
    End Sub

    <Test>
    Public Sub Should_Retrieve_Leave()
        Dim leave = _context.Leaves.FirstOrDefault()

        Assert.IsInstanceOf(Of Leave)(leave)
    End Sub

    <Test>
    Public Sub Should_Retrieve_Overtime()
        Dim overtime = _context.Overtimes.FirstOrDefault()

        Assert.IsInstanceOf(Of Overtime)(overtime)
    End Sub

End Class