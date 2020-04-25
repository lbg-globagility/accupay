Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Services

<TestFixture>
Public Class ListOfValueCollectionTest

    <TestCase("true")>
    <TestCase("1")>
    <TestCase("-1")>
    Public Sub ShouldParseAsTrue(truthyValue As String)
        Dim listOfValues = CreateListOfValue(truthyValue)
        Dim answer = listOfValues.GetBoolean("sampletype", "samplelic")

        Assert.AreEqual(True, answer)
    End Sub

    <TestCase("false")>
    <TestCase("")>
    <TestCase(Nothing)>
    <TestCase("0")>
    Public Sub ShouldParseAsFalse(truthyValue As String)
        Dim listOfValues = CreateListOfValue(truthyValue)
        Dim answer = listOfValues.GetBoolean("sampletype", "samplelic")

        Assert.AreEqual(False, answer)
    End Sub

    <Test>
    Public Sub SimpleNameShouldReturnBoolean()
        Dim listOfValues = CreateListOfValue("true")
        Dim answer = listOfValues.GetBoolean("sampletype.samplelic")

        Assert.AreEqual(True, answer)
    End Sub

    <Test>
    Public Sub SimpleNameShouldReturnStringHello()
        Dim listOfValues = CreateListOfValue("Hello")
        Dim answer = listOfValues.GetString("sampletype.samplelic")

        Assert.AreEqual("Hello", answer)
    End Sub

    Private Function CreateListOfValue(value As String) As ListOfValueCollection
        Return New ListOfValueCollection(New List(Of ListOfValue) From {
            New ListOfValue() With {
                .Type = "sampletype",
                .LIC = "samplelic",
                .DisplayValue = value
            }
        })
    End Function

End Class