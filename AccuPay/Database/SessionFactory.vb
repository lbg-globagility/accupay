Option Strict On

Imports FluentNHibernate.Automapping
Imports FluentNHibernate.Cfg
Imports NHibernate

Public Class SessionFactory

    Private Shared ReadOnly _instance As New Lazy(Of ISessionFactory)(
        Function() CreateSessionFactory(),
        System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)

    Private Shared Function CreateSessionFactory() As ISessionFactory
        Return Fluently.Configure().Database(
                Db.MySQLConfiguration.Standard.ConnectionString(connectionString)).
            Mappings(
                Sub(m)
                    m.FluentMappings.AddFromAssemblyOf(Of OvertimeMap)()
                End Sub).
            BuildSessionFactory()
    End Function

    Public Shared ReadOnly Property Instance As ISessionFactory
        Get
            Return _instance.Value
        End Get
    End Property

End Class
