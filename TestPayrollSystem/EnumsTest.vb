Option Strict On

Imports AccuPay.Core.Helpers

<TestFixture>
Public Class EnumsTest

    Private Enum MyEnum As Integer
        Yes
        No
    End Enum

    <Test>
    Public Sub EnumsShouldParseAsYes()
        Dim enumValue = "Yes"

        Dim answer = Enums(Of MyEnum).Parse(enumValue)

        Assert.AreEqual(MyEnum.Yes, answer)
    End Sub

End Class