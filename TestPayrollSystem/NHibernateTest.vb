Option Strict On

Imports System.Data.Common
Imports AccuPay.Entity
Imports Acupay

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

End Class
