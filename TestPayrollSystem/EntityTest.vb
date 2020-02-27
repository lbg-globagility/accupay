Option Strict On

Imports AccuPay.Data.Repositories
Imports AccuPay
Imports AccuPay.Utilities
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

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
    Public Async Function TestEntity2() As Task

        'Using context = New PayrollContext()

        '    Dim paystubCount = context.Paystubs.ToList.Count

        '    Assert.IsTrue(context.Paystubs.ToList.Count > 0)

        'End Using

        Dim officialbusType As New ListOfValue

        Using context = New PayrollContext

            Dim listOfVal As New ListOfValue
            listOfVal.DisplayValue = "FIELDWORK DUTY"
            listOfVal.Type = "Official Business Type"
            listOfVal.Active = "Yes"

            listOfVal.Created = Date.Now
            listOfVal.CreatedBy = 1
            listOfVal.LastUpd = Date.Now
            listOfVal.LastUpdBy = 1
            context.ListOfValues.Add(listOfVal)

            Await context.SaveChangesAsync()

            officialbusType = Await context.ListOfValues.
                            FirstOrDefaultAsync(Function(l) Nullable.Equals(l.RowID, listOfVal.RowID))

            Assert.IsTrue(officialbusType IsNot Nothing)

        End Using

    End Function

End Class