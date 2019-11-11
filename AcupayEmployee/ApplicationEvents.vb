Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Imports System.Threading
Imports log4net

Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    Partial Friend Class MyApplication

        Private _logger As ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Private Sub MyApplication_NetworkAvailabilityChanged(sender As Object, e As Devices.NetworkAvailableEventArgs) Handles Me.NetworkAvailabilityChanged
        End Sub

        Private Sub MyApplication_Shutdown(sender As Object, e As EventArgs) Handles Me.Shutdown
        End Sub

        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup
            Try
                Using context = New PayrollContext()
                    context.Database.Initialize(False)
                End Using
            Catch ex As Exception
                MsgBox("A serious error occured while trying to initializing the database.", MsgBoxStyle.OkOnly, "Database Error")
                Throw
            End Try

            Try
                conn = New MySqlConnection
                conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString

                hasERR = 0
            Catch ex As Exception
                hasERR = 1
                MsgBox(getErrExcptn(ex, "Namespace_My") & vbNewLine & " MyApplication_Startup", MsgBoxStyle.Information, "Server Connection")
            Finally
                If hasERR = 0 Then
                    EXECQUER("CALL SET_group_concat_max_len();")
                End If
            End Try
        End Sub

        Private Sub MyApplication_StartupNextInstance(sender As Object, e As ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            _logger.Error("Uncaught exception", e.Exception)
        End Sub

    End Class

End Namespace

