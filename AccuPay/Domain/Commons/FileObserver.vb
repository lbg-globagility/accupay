Imports System.IO

Public Class FileObserver

    Dim file_observer As FileSystemWatcher

    Dim Dirty As Boolean = False

    Sub New(filePathAndName As String)
        Detect()
    End Sub

    Sub FileObserver()
        'D:\Others\APPLICATION\RBX PAYROLL
    End Sub

    Sub Detect()

        Try

            file_observer = New FileSystemWatcher

            With file_observer

                .Filter = installerpath.Substring(installerpath.LastIndexOf("\") + 1)

                .Path = installerpath.Substring(0, installerpath.Length - .Filter.Length)

                .NotifyFilter = NotifyFilters.LastAccess Or NotifyFilters.LastWrite _
                                Or NotifyFilters.FileName Or NotifyFilters.DirectoryName

                AddHandler .Changed, AddressOf OnChanged

                AddHandler .Created, AddressOf OnChanged

                'AddHandler .Deleted, AddressOf OnChanged

                'AddHandler .Renamed, AddressOf OnRenamed

                .EnableRaisingEvents = True

            End With

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, "FileObserver"))

        End Try

    End Sub

    Private Sub OnChanged(sender As Object, e As FileSystemEventArgs)

        MsgBox("OnChanged in FileObserver" & e.ChangeType.ToString)

        If Dirty = False Then
            Dirty = True

        End If

    End Sub

    Private Sub OnRenamed(sender As Object, e As RenamedEventArgs)

        MsgBox("OnRenamed in FileObserver")

        If Dirty = False Then
            Dirty = True

        End If

    End Sub

    Sub Undetect()

        file_observer.EnableRaisingEvents = False

        file_observer.Dispose()

    End Sub

End Class