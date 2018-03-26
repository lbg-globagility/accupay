Option Strict On

Imports NHibernate.Linq
Imports AccuPay.Entity
Imports AccuPay

<TestFixture>
Public Class NHibernateTest

    <Test>
    Public Sub Should_Get_Overtime()
        Dim factory = SessionFactory.Instance

        Dim overtime As Overtime
        Using session = factory.OpenSession()
            overtime = session.Query(Of Overtime).FirstOrDefault()
        End Using

        Assert.IsInstanceOf(Of Overtime)(overtime)
    End Sub

    <Test>
    Public Sub Should_Get_Paystub()
        Dim paystub As Paystub
        Using session = SessionFactory.Instance.OpenSession()
            paystub = session.Query(Of Paystub).FirstOrDefault()
        End Using

        Assert.IsInstanceOf(Of Paystub)(paystub)
    End Sub

    <Test>
    Public Sub Should_Get_Paystub_In_Linq()
        Dim paystubs As IList(Of Paystub) = Nothing

        Dim t = Task.Run(
            Async Function()
                Using session = SessionFactory.Instance.OpenSession()
                    paystubs = Await session.Query(Of Paystub).ToListAsync()
                End Using
            End Function)

        t.Wait()

        Assert.IsInstanceOf(Of IList(Of Paystub))(paystubs)
    End Sub

End Class
