Option Strict On

Imports AccuPay.Extensions

<TestFixture>
Public Class StringExtensionsTest

#Region "ToDecimal"

    <TestCase("sdfsdf")>
    <TestCase("")>
    <TestCase(Nothing)>
    Public Sub ToDecimal_WithInvalidInput_ReturnsNull(ByVal num As String)
        Dim expected As Decimal = 0
        Dim result = ModuleClass.StringExtensions_ToDecimal(num)

        Assert.AreEqual(expected, result)

    End Sub

    <TestCase("1")>
    <TestCase("1.0")>
    <TestCase("1.00")>
    <TestCase("01.00")>
    <TestCase("0000001.0000000")>
    Public Sub ToDecimal_WithValueOne_ReturnsValueOne(ByVal num As String)
        Dim expected As Decimal = 1
        Dim result = ModuleClass.StringExtensions_ToDecimal(num)

        Assert.AreEqual(expected, result)
    End Sub

#End Region

#Region "ToNullableTimeSpan"

    'changes here should reflect in ObjectUtilsTest

    <TestCase("1/30/2017 16:35")>
    <TestCase("1/30/2017 16:35:00")>
    <TestCase("1/30/2017 4:35 PM")>
    <TestCase("1/30/2017 04:35 PM")>
    <TestCase("1/30/2017 4:35:00 PM")>
    <TestCase("1/30/2017 04:35:00 PM")>
    <TestCase("04:35:00 PM")>
    <TestCase("04:35 PM")>
    <TestCase("4:35:00 PM")>
    <TestCase("4:35 PM")>
    <TestCase("16:35:00")>
    <TestCase("16:35")>
    Public Shared Sub ToNullableTimeSpan_WithValidInput_ReturnsTimeSpan(time As String)

        Dim output = ModuleClass.StringExtensions_ToNullableTimeSpan(time)

        Assert.AreEqual(New TimeSpan(16, 35, 0), output)
    End Sub

    'commented testcase results to 12:00 PM
    '<TestCase("00:00 PM")>
    '<TestCase("00:00:00 PM")>
    '<TestCase("0:00 PM")>
    '<TestCase("0:00:00 PM")>
    '<TestCase("1/30/2017 00:00:00 PM")>
    '<TestCase("1/30/2017 0:00 PM")>
    '<TestCase("1/30/2017 0:00:00 PM")>
    <TestCase("1/30/2017 00:00")>
    <TestCase("1/30/2017 00:00:00")>
    <TestCase("1/30/2017 24:00")>
    <TestCase("1/30/2017 24:00:00")>
    <TestCase("1/30/2017 0:00 AM")>
    <TestCase("1/30/2017 00:00 AM")>
    <TestCase("1/30/2017 24:00 AM")>
    <TestCase("1/30/2017 24:00 PM")>
    <TestCase("1/30/2017 0:00:00 AM")>
    <TestCase("1/30/2017 00:00:00 AM")>
    <TestCase("1/30/2017 24:00:00 AM")>
    <TestCase("1/30/2017 24:00:00 PM")>
    <TestCase("00:00:00 AM")>
    <TestCase("00:00 AM")>
    <TestCase("0:00:00 AM")>
    <TestCase("0:00 AM")>
    <TestCase("24:00:00 AM")>
    <TestCase("24:00 AM")>
    <TestCase("24:00:00 PM")>
    <TestCase("24:00 PM")>
    <TestCase("00:00:00")>
    <TestCase("00:00")>
    <TestCase("24:00:00")>
    <TestCase("24:00")>
    Public Shared Sub ToNullableTimeSpan_With12AMInput_Returns12AM(time As String)

        Dim output = ModuleClass.StringExtensions_ToNullableTimeSpan(time)

        Assert.AreEqual(New TimeSpan(0, 0, 0), output)
    End Sub

#End Region

End Class