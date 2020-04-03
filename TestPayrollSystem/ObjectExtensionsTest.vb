Option Strict On

Imports AccuPay.Utilities.Extensions

<TestFixture>
Public Class ObjectExtensionsTest

    <Test>
    Public Sub NullableEquals_WithValidInput()

        Dim stringNull1 As String = Nothing
        Dim stringNull2 As String = Nothing
        Dim string1 As String = "1"
        Dim string1_2 As String = "1"
        Dim string2 As String = "2"

        Dim intNull1 As Integer? = Nothing
        Dim intNull2 As Integer? = Nothing
        Dim int1 As Integer? = 1
        Dim int1_2 As Integer? = 1
        Dim int2 As Integer? = 2

        Dim dateNull1 As Date? = Nothing
        Dim dateNull2 As Date? = Nothing
        Dim date1 As Date? = New Date(2020, 4, 1)
        Dim date1_2 As Date? = New Date(2020, 4, 1)
        Dim date2 As Date? = New Date(2020, 4, 2)

        Dim concreteClassNull1 As ConcreteClassSample = Nothing
        Dim concreteClassNull2 As ConcreteClassSample = Nothing
        Dim concreteClass1 As New ConcreteClassSample
        Dim concreteClass2 As New ConcreteClassSample

        Assert.AreEqual(Nothing, Nothing)

        Assert.True(stringNull1.NullableEquals(stringNull2))
        Assert.True(string1.NullableEquals(string1_2))
        Assert.False(string1.NullableEquals(stringNull1))
        Assert.False(string1.NullableEquals(string2))

        Assert.True(intNull1.NullableEquals(intNull2))
        Assert.True(int1.NullableEquals(int1_2))
        Assert.False(int1.NullableEquals(intNull1))
        Assert.False(int1.NullableEquals(int2))

        Assert.True(dateNull1.NullableEquals(dateNull2))
        Assert.True(date1.NullableEquals(date1_2))
        Assert.False(date1.NullableEquals(dateNull1))
        Assert.False(date1.NullableEquals(date2))

        Assert.True(concreteClassNull1.NullableEquals(concreteClassNull2))
        Assert.True(concreteClass1.NullableEquals(concreteClass1))
        Assert.False(concreteClass1.NullableEquals(concreteClassNull1))
        Assert.False(concreteClass1.NullableEquals(concreteClass2))

    End Sub

    Private Class ConcreteClassSample

    End Class

End Class