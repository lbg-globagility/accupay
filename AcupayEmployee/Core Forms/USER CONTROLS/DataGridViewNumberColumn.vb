Imports Microsoft.Win32

Public Class DataGridViewNumberColumn

    Inherits DataGridViewTextBoxColumn

    Sub New()

        Me.CellTemplate =
            New DataGridViewNumberCell

    End Sub

    Sub DataGridViewDateColumn()

    End Sub

End Class

Public Class DataGridViewNumberCell

    Inherits DataGridViewTextBoxCell

    Dim ValueBefore As Object = Nothing

    Sub New()
        MyBase.ContextMenuStrip = New ContextMenuStrip

        MyBase.MaxInputLength = 11

    End Sub

    Protected Overrides Sub OnEnter(rowIndex As Integer, throughMouseClick As Boolean)

        ValueBefore = ValNoComma(MyBase.Value)

        MyBase.OnEnter(rowIndex, throughMouseClick)

    End Sub

    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs, rowIndex As Integer)

        Dim n_TrapDecimalKey As New TrapDecimalKey(Asc(e.KeyChar), CStr(MyBase.Value))

        e.Handled = n_TrapDecimalKey.ResultTrap

        If n_TrapDecimalKey.ResultTrap Then

            MyBase.OnKeyPress(e, rowIndex)

        End If

    End Sub

    Protected Overrides Sub OnLeave(rowIndex As Integer, throughMouseClick As Boolean)

        Dim thisValue = ValNoComma(MyBase.Value)

        If thisValue = 0 Then

            MyBase.Value = Nothing

            MyBase.ErrorText = Nothing

        ElseIf ValueBefore <> ValNoComma(MyBase.Value) Then

            Try

                MyBase.Value = ValNoComma(MyBase.Value)

                MyBase.ErrorText = Nothing

            Catch ex As Exception

                MyBase.Value = Nothing

                MyBase.ErrorText = "     Invalid" & vbNewLine &
                    "     numeric" & vbNewLine &
                    "     value" & vbNewLine

            End Try

        End If

        MyBase.OnLeave(rowIndex, throughMouseClick)

    End Sub

End Class