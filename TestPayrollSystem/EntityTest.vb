Option Strict On

Imports AccuPay.Data.Repositories
Imports AccuPay

<TestFixture>
Public Class EntityTest

    <Test>
    Public Sub EnumsShouldParseAsYes()
        Dim answer = True

        Assert.AreEqual(True, answer)
    End Sub

    <Test>
    Public Sub TestEntity()
        Dim repo As New PaystubRepository()

        Dim count = repo.List.Count

        Assert.IsTrue(repo.List.Count > 0)
    End Sub

    <Test>
    Public Sub TestEntity2()

        Using context = New PayrollContext()

            Assert.IsTrue(context.Paystubs.ToList.Count > 0)

        End Using

    End Sub

End Class