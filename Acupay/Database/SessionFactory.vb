Option Strict On

Imports FluentNHibernate.Cfg
Imports NHibernate

Public Class SessionFactory

    Private Shared ReadOnly _instance As New Lazy(Of ISessionFactory)(
        Function()
            Return Fluently.Configure().Database(
                Db.MySQLConfiguration.Standard.ConnectionString(connectionString)).
            Mappings(Function(m) m.FluentMappings.AddFromAssemblyOf(Of OvertimeMap)()).
            BuildSessionFactory()
        End Function,
        System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)

    Public Shared ReadOnly Property Instance As ISessionFactory
        Get
            Return _instance.Value
        End Get
    End Property

End Class
