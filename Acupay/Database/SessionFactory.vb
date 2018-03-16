Option Strict On

Imports FluentNHibernate.Cfg
Imports NHibernate

Public Class SessionFactory

    Public Shared Function CreateSessionFactory() As ISessionFactory
        Return Fluently.Configure().Database(
                Db.MySQLConfiguration.Standard.ConnectionString(connectionString)).
            Mappings(Function(m) m.FluentMappings.AddFromAssemblyOf(Of OvertimeMap)()).
            BuildSessionFactory()
    End Function

End Class
