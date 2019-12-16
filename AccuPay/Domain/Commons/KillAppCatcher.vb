Imports System
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Threading

Namespace PayrollSystem

    Public Class KillAppCatcher

        Dim checker As Process

        Dim mainn As Process

        Dim mainProcessID As Integer = 0

        Sub New(ByVal args() As String)

            If args.Length = 0 Then

                mainn = Process.GetCurrentProcess()

                mainProcessID = mainn.Id


                checker = New Process

                checker.StartInfo.FileName = mainn.MainModule.FileName

                checker.StartInfo.Arguments = mainProcessID.ToString()


                checker.EnableRaisingEvents = True

                AddHandler checker.Exited, AddressOf checker_Exited

                checker.Start()

                Application.Run(New MDIPrimaryForm())

            Else

                mainn = Process.GetProcessById(Integer.Parse(args(0)))

                mainn.EnableRaisingEvents = True

                AddHandler mainn.Exited, AddressOf main_Exited

                While mainn.HasExited

                    Thread.Sleep(1000)

                End While

                Thread.Sleep(2000)

            End If

        End Sub

        Shared Sub Main(ByVal args() As String)

        End Sub

        Sub checker_Exited(sender As Object, e As EventArgs)

            If Process.GetProcessesByName("taskmgr").Length <> 1 Then

                MessageBox.Show("Task Manager killed helper process.")

            End If

        End Sub

        Sub main_Exited(sender As Object, e As EventArgs)

            If Process.GetProcessesByName("taskmgr").Length <> 1 Then

                MessageBox.Show("Task Manager killed my my app.")

            End If

        End Sub

    End Class

End Namespace
