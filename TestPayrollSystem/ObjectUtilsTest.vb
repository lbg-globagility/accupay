Option Strict On

Imports AccuPay.Utils

<TestFixture>
Public Class ObjectUtilsTest

#Region "ToDecimal"

    <TestCase("sdfsdf")>
    <TestCase("")>
    <TestCase(Nothing)>
    Public Sub ToDecimal_WithInvalidInput_ReturnsNull(ByVal num As Object)
        Dim expected As Decimal = 0
        Dim result = ObjectUtils.ToDecimal(num)

        Assert.AreEqual(expected, result)

    End Sub

    <TestCase("1")>
    <TestCase("1.0")>
    <TestCase("1.00")>
    <TestCase("01.00")>
    <TestCase("0000001.0000000")>
    <TestCase(1)>
    <TestCase(1.0)>
    Public Sub ToDecimal_WithValueOne_ReturnsValueOne(ByVal num As Object)
        Dim expected As Decimal = 1
        Dim result = ObjectUtils.ToDecimal(num)

        Assert.AreEqual(expected, result)
    End Sub
#End Region

#Region "ToNullableDecimal"

    <TestCase("sdfsdf")>
    <TestCase("")>
    <TestCase(Nothing)>
    Public Sub ToNullableDecimal_WithInvalidInput_ReturnsNull(ByVal num As Object)
        Dim expected As Decimal? = Nothing
        Dim result = ObjectUtils.ToNullableDecimal(num)

        Assert.AreEqual(expected, result)

    End Sub

    <TestCase("1")>
    <TestCase("1.0")>
    <TestCase("1.00")>
    <TestCase("01.00")>
    <TestCase("0000001.0000000")>
    <TestCase(1)>
    <TestCase(1.0)>
    Public Sub ToNullableDecimal_WithValueOne_ReturnsValueOne(ByVal num As Object)
        Dim expected As Decimal? = 1
        Dim result = ObjectUtils.ToNullableDecimal(num)

        Assert.AreEqual(expected, result)
    End Sub
#End Region

#Region "ToInteger"

    <TestCase("sdfsdf")>
    <TestCase("")>
    <TestCase("1.0")>
    <TestCase("1.00")>
    <TestCase("01.00")>
    <TestCase("0000001.0000000")>
    <TestCase(INTEGER_MAX_VALUE_PLUS_ONE)>
    <TestCase(INTEGER_MIN_VALUE_MINUS_ONE)>
    <TestCase(Nothing)>
    Public Sub ToInteger_WithInvalidInput_ReturnsNull(ByVal num As Object)
        Dim expected As Integer = 0
        Dim result = ObjectUtils.ToInteger(num)

        Assert.AreEqual(expected, result)

    End Sub

    <TestCase("1")>
    <TestCase(1)>
    <TestCase(1.0)>
    Public Sub ToInteger_WithValueOne_ReturnsValueOne(ByVal num As Object)
        Dim expected As Integer = 1
        Dim result = ObjectUtils.ToInteger(num)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase(Integer.MaxValue)>
    Public Sub ToInteger_WithIntegerMaxValue_ReturnsIntegerMaxValue(ByVal num As Object)
        Dim expected As Integer = Integer.MaxValue
        Dim result = ObjectUtils.ToInteger(num)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase(Integer.MinValue)>
    Public Sub ToInteger_WithIntegerMinValue_ReturnsIntegerMinValue(ByVal num As Object)
        Dim expected As Integer = Integer.MinValue
        Dim result = ObjectUtils.ToInteger(num)

        Assert.AreEqual(expected, result)
    End Sub
#End Region

#Region "ToNullableInteger"

    <TestCase("sdfsdf")>
    <TestCase("")>
    <TestCase("1.0")>
    <TestCase("1.00")>
    <TestCase("01.00")>
    <TestCase("0000001.0000000")>
    <TestCase(INTEGER_MAX_VALUE_PLUS_ONE)>
    <TestCase(INTEGER_MIN_VALUE_MINUS_ONE)>
    <TestCase(Nothing)>
    Public Sub ToNullableInteger_WithInvalidInput_ReturnsNull(ByVal num As Object)
        Dim expected As Integer? = Nothing
        Dim result = ObjectUtils.ToNullableInteger(num)

        Assert.AreEqual(expected, result)

    End Sub

    <TestCase("1")>
    <TestCase(1)>
    <TestCase(1.0)>
    Public Sub ToNullableInteger_WithValueOne_ReturnsValueOne(ByVal num As Object)
        Dim expected As Integer? = 1
        Dim result = ObjectUtils.ToNullableInteger(num)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase(Integer.MaxValue)>
    Public Sub ToNullableInteger_WithIntegerMaxValue_ReturnsIntegerMaxValue(ByVal num As Object)
        Dim expected As Integer? = Integer.MaxValue
        Dim result = ObjectUtils.ToNullableInteger(num)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase(Integer.MinValue)>
    Public Sub ToNullableInteger_WithIntegerMinValue_ReturnsIntegerMinValue(ByVal num As Object)
        Dim expected As Integer? = Integer.MinValue
        Dim result = ObjectUtils.ToNullableInteger(num)

        Assert.AreEqual(expected, result)
    End Sub
#End Region

#Region "ToDateTime"

    <TestCase("sdfsdf")>
    <TestCase("")>
    <TestCase("2019-21-01")>
    <TestCase("2019-13-01")>
    <TestCase("2019-01-32")>
    <TestCase("2019-00-00")>
    <TestCase(1)>
    <TestCase(0)>
    <TestCase(-1)>
    <TestCase(True)>
    <TestCase(False)>
    <TestCase(Nothing)>
    Public Sub ToDateTime_WithInvalidInput_ReturnsNull(ByVal dateInput As Object)
        Dim expected As Date = Date.MinValue
        Dim result = ObjectUtils.ToDateTime(dateInput)

        Assert.AreEqual(expected, result)

    End Sub

    <TestCase("2019-01-01")>
    Public Sub ToDateTime_WithValueFirstDayOfYear2019DateParse_ReturnsFirstDayOfYear2019DateParse(
        ByVal dateInput As Object)

        Dim expected As Date = Date.Parse("2019-01-01")
        Dim result = ObjectUtils.ToDateTime(dateInput)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("2019-01-01")>
    Public Sub ToDateTime_WithValueFirstDayOfYear2019DateTimeInstance_ReturnsFirstDayOfYear2019DateTimeInstance(
        ByVal dateInput As Object)

        Dim expected As Date = New DateTime(2019, 1, 1)
        Dim result = ObjectUtils.ToDateTime(dateInput)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("2019-01-01 15:22")>
    <TestCase("2019-01-01 3:22 PM")>
    Public Sub ToDateTime_WithYearMonthDayDash_ReturnsFirstDayOfYear2019DateParse(
        ByVal dateInput As Object)

        Dim expected As Date = New DateTime(2019, 1, 1, 15, 22, 0)
        Dim result = ObjectUtils.ToDateTime(dateInput)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("01-03-2019 13:24")>
    <TestCase("01-03-2019 1:24 PM")>
    Public Sub ToDateTime_WithMonthDayYearDash_ReturnsFirstDayOfYear2019DateParse(
        ByVal dateInput As Object)

        Dim expected As Date = New DateTime(2019, 1, 3, 13, 24, 0)
        Dim result = ObjectUtils.ToDateTime(dateInput)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("1/30/2017 16:35")>
    <TestCase("1/30/2017 4:35 PM")>
    Public Sub ToDateTime_WithMonthDayYearSlash_ReturnsFirstDayOfYear2019DateParse(
        ByVal dateInput As Object)

        Dim expected As Date = New DateTime(2017, 1, 30, 16, 35, 0)
        Dim result = ObjectUtils.ToDateTime(dateInput)

        Assert.AreEqual(expected, result)
    End Sub
#End Region

#Region "ToNullableDateTime"

    <TestCase("sdfsdf")>
    <TestCase("")>
    <TestCase("2019-21-01")>
    <TestCase("2019-13-01")>
    <TestCase("2019-01-32")>
    <TestCase("2019-00-00")>
    <TestCase(1)>
    <TestCase(0)>
    <TestCase(-1)>
    <TestCase(True)>
    <TestCase(False)>
    <TestCase(Nothing)>
    Public Sub ToNullableDateTime_WithInvalidInput_ReturnsNull(ByVal dateInput As Object)
        Dim expected As Date? = Nothing
        Dim result = ObjectUtils.ToNullableDateTime(dateInput)

        Assert.AreEqual(expected, result)

    End Sub

    <TestCase("2019-01-01")>
    Public Sub ToNullableDateTime_WithValueFirstDayOfYear2019DateParse_ReturnsFirstDayOfYear2019DateParse(
        ByVal dateInput As Object)

        Dim expected As Date = Date.Parse("2019-01-01")
        Dim result = ObjectUtils.ToNullableDateTime(dateInput)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("2019-01-01")>
    Public Sub ToNullableDateTime_WithValueFirstDayOfYear2019DateTimeInstance_ReturnsFirstDayOfYear2019DateTimeInstance(
        ByVal dateInput As Object)

        Dim expected As Date = New DateTime(2019, 1, 1)
        Dim result = ObjectUtils.ToNullableDateTime(dateInput)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("2019-01-01 15:22")>
    <TestCase("2019-01-01 3:22 PM")>
    Public Sub ToNullableDateTime_WithYearMonthDayDash_ReturnsFirstDayOfYear2019DateParse(
        ByVal dateInput As Object)

        Dim expected As Date? = New DateTime(2019, 1, 1, 15, 22, 0)
        Dim result = ObjectUtils.ToNullableDateTime(dateInput)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("01-03-2019 13:24")>
    <TestCase("01-03-2019 1:24 PM")>
    Public Sub ToNullableDateTime_WithMonthDayYearDash_ReturnsFirstDayOfYear2019DateParse(
        ByVal dateInput As Object)

        Dim expected As Date? = New DateTime(2019, 1, 3, 13, 24, 0)
        Dim result = ObjectUtils.ToNullableDateTime(dateInput)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("1/30/2017 16:35")>
    <TestCase("1/30/2017 4:35 PM")>
    Public Sub ToNullableDateTime_WithMonthDayYearSlash_ReturnsFirstDayOfYear2019DateParse(
        ByVal dateInput As Object)

        Dim expected As Date? = New DateTime(2017, 1, 30, 16, 35, 0)
        Dim result = ObjectUtils.ToNullableDateTime(dateInput)

        Assert.AreEqual(expected, result)
    End Sub
#End Region

#Region "ToStringOrNull"

    <TestCase(Nothing)>
    Public Sub ToStringOrNull_WithInvalidInput_ReturnsNull(ByVal input As Object)
        Dim expected As String = Nothing
        Dim result = ObjectUtils.ToStringOrNull(input)

        Assert.AreEqual(expected, result)

    End Sub

    <TestCase("random text")>
    <TestCase("2019-00-00")>
    <TestCase("")>
    <TestCase(1)>
    <TestCase(0)>
    <TestCase(-1)>
    <TestCase(True)>
    <TestCase(False)>
    Public Sub ToStringOrNull_WithValidInput_ReturnsInput(ByVal input As Object)
        Dim expected As String = input.ToString()
        Dim result = ObjectUtils.ToStringOrNull(input)

        Assert.AreEqual(expected, result)
    End Sub
#End Region

#Region "ToStringOrEmpty"

    <TestCase(Nothing)>
    Public Sub ToStringOrEmpty_WithInvalidInput_ReturnsNull(ByVal input As Object)
        Dim expected As String = ""
        Dim result = ObjectUtils.ToStringOrEmpty(input)

        Assert.AreEqual(expected, result)

    End Sub

    <TestCase("random text")>
    <TestCase("2019-00-00")>
    <TestCase("")>
    <TestCase(1)>
    <TestCase(0)>
    <TestCase(-1)>
    <TestCase(True)>
    <TestCase(False)>
    Public Sub ToStringOrEmpty_WithValidInput_ReturnsInput(ByVal input As Object)
        Dim expected As String = input.ToString()
        Dim result = ObjectUtils.ToStringOrEmpty(input)

        Assert.AreEqual(expected, result)
    End Sub
#End Region

End Class
