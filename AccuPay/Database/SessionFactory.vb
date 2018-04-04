Option Strict On

Imports FluentNHibernate.Cfg
Imports NHibernate
Imports AccuPay.Database.Mappings

Public Class SessionFactory

    Private Shared ReadOnly _instance As New Lazy(Of ISessionFactory)(
        Function() CreateSessionFactory(),
        System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)

    Private Shared Function CreateSessionFactory() As ISessionFactory
        Return Fluently.Configure().Database(
                Db.MySQLConfiguration.Standard.ConnectionString(connectionString)).
            Mappings(
                Sub(m)
                    m.FluentMappings.
                        AddFromAssemblyOf(Of AllowanceItemMap)().
                        AddFromAssemblyOf(Of AllowancePerDayMap)().
                        AddFromAssemblyOf(Of LeaveLedgerMap)().
                        AddFromAssemblyOf(Of LeaveMap)().
                        AddFromAssemblyOf(Of LeaveTransactionMap)().
                        AddFromAssemblyOf(Of LoanScheduleMap)().
                        AddFromAssemblyOf(Of LoanTransactionMap)().
                        AddFromAssemblyOf(Of OvertimeMap)().
                        AddFromAssemblyOf(Of PaystubItemMap)().
                        AddFromAssemblyOf(Of PaystubMap)().
                        AddFromAssemblyOf(Of ThirteenthMonthPayMap)()
                End Sub).
            BuildSessionFactory()
    End Function

    Public Shared ReadOnly Property Instance As ISessionFactory
        Get
            Return _instance.Value
        End Get
    End Property

End Class
