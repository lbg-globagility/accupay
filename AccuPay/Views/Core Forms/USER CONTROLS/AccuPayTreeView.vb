Imports System.Windows.Forms.VisualStyles

Public Class AccuPayTreeView
    Inherits TreeView

    Protected Overrides Sub WndProc(ByRef m As Message)
        If (m.Msg = &H203) Then
            m.Msg = &H201
        End If

        MyBase.WndProc(m)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        CheckBoxRenderer.DrawCheckBox(e.Graphics, New Point(0, 0), Bounds,
            Text, Font, False,
            CheckBoxState.MixedNormal)
    End Sub

End Class
