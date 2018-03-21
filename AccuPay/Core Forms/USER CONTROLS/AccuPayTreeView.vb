Public Class AccuPayTreeView
    Inherits TreeView

    Protected Overrides Sub WndProc(ByRef m As Message)
        If (m.Msg = &H203) Then
            m.Msg = &H201
        End If
        MyBase.WndProc(m)
    End Sub

End Class
