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
            If MachineLocalization IsNot Nothing Then

                Thread.Sleep(1000)

                Try

                    For Each drow As DataRow In MachineLocalization.Rows

                        Thread.Sleep(100)

                    Next
                Catch ex As Exception
                    MsgBox(ex, MyBase.ToString)
                Finally
                    MachineLocalization.Dispose()

                End Try

            End If
        End Sub

        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup
            Try
                'Using context = New PayrollContext()
                'End Using

                'Dim repository As New Data.Repositories.BranchRepository()
                'Dim list = repository.GetAll(z_OrganizationID)
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

            custom_mysqldateformat = "%" & machineShortDateFormat.Replace("/", "/%")

            custom_mysqldateformat = custom_mysqldateformat.ToLower

            If custom_mysqldateformat.Contains("yyyy") Then
                custom_mysqldateformat = custom_mysqldateformat.Replace("yyyy", "Y")
            ElseIf custom_mysqldateformat.Contains("yy") Then
                custom_mysqldateformat = custom_mysqldateformat.Replace("yy", "Y")
            End If

            If custom_mysqldateformat.Contains("mm") Then
                custom_mysqldateformat = custom_mysqldateformat.Replace("mm", "c")
            ElseIf custom_mysqldateformat.Contains("m") Then
                custom_mysqldateformat = custom_mysqldateformat.Replace("m", "c")
            End If

            If custom_mysqldateformat.Contains("dd") Then
                custom_mysqldateformat = custom_mysqldateformat.Replace("dd", "e")
            ElseIf custom_mysqldateformat.Contains("d") Then
                custom_mysqldateformat = custom_mysqldateformat.Replace("d", "e")
            End If
        End Sub

        Dim main_process As Process

        Private Sub MyApplication_StartupNextInstance(sender As Object, e As ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            _logger.Error("Uncaught exception", e.Exception)
        End Sub

    End Class

End Namespace