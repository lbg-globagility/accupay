Public Class CountDownExpiration

    Sub New()
        CountDownExpiration()

    End Sub

    Dim n_UsageLeft As Decimal = 0

    Public Property UsageLeft

        Get
            Return n_UsageLeft

        End Get

        Set(value)
            n_UsageLeft = value

        End Set

    End Property

    Sub CountDownExpiration()

        n_UsageLeft = EXECQUER("SELECT TIME_TO_SEC(TIMEDIFF(AES_DECRYPT(`Column3`,'.a.'), AES_DECRYPT(`Column2`,'.a.')))" & _
                                  " FROM expiration" & _
                                  " WHERE AES_DECRYPT(`Column1`,'.a.')='" & My.Resources.SystemOwner & "';")

        If n_UsageLeft <= 0 Then

            Dim dlgbox = _
            MsgBox("Your 30-day trial period has expired." & vbNewLine & vbNewLine & _
                   "All the features has been disabled. You can still launch the program but cannot modify any created data." & vbNewLine & vbNewLine & _
                   "Do you want to continue to full version now ?",
                   MsgBoxStyle.YesNo,
                   "")

            If dlgbox = MsgBoxResult.Yes Then

                Process.Start("http://www.globagilityinc.com/")

            End If

            Application.Exit()

        End If

    End Sub

End Class
