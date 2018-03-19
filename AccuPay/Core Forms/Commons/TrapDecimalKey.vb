
Public Class TrapDecimalKey

    Dim n_ResultTrap As Boolean = False

    Property ResultTrap As Boolean

        Get
            Return n_ResultTrap

        End Get

        Set(value As Boolean)
            n_ResultTrap = value

        End Set

    End Property

    Sub New(asc_ii As Integer, current_string As String)

        n_ResultTrap = TrapDecimalKey(asc_ii, current_string)

    End Sub

    Private Function TrapDecimalKey(ByVal KCode As String, textboxstring As String) As Boolean    '//textbox keypress event insert number ONLY

        Static onedot As SByte = 0

        If (KCode >= 48 And KCode <= 57) Or KCode = 8 Or KCode = 46 Then

            If KCode = 46 Then

                onedot += 1

                If onedot >= 2 Then

                    If textboxstring.Contains(".") Then
                        TrapDecimalKey = True

                        onedot = 2

                    Else
                        TrapDecimalKey = False

                        onedot = 0

                    End If

                Else
                    If textboxstring.Contains(".") Then
                        TrapDecimalKey = True

                    Else
                        TrapDecimalKey = False

                    End If

                End If

            Else
                TrapDecimalKey = False

            End If

        Else
            TrapDecimalKey = True

        End If

    End Function

End Class
