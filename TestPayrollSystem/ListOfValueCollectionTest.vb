Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Services
Imports Microsoft.Extensions.DependencyInjection

<TestFixture>
Public Class ListOfValueCollectionTest
    Inherits ServiceProvider

    Private ReadOnly _listOfValueService As ListOfValueService

    Sub New()

        _listOfValueService = MainServiceProvider.GetRequiredService(Of ListOfValueService)

    End Sub

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
        Return _listOfValueService.Create(New List(Of ListOfValue) From {
            New ListOfValue() With {
                .Type = "sampletype",
                .LIC = "samplelic",
                .DisplayValue = value
            }
        })
    End Function

End Class