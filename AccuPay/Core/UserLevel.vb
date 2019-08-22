Option Strict On

Public Enum UserLevel
    One
    Two
    Three
    Four
    Five
End Enum

Public Class UserLevelHelper

    Public Shared Function GetUserLevelDescription(userLevel As UserLevel) As String

        Select Case (userLevel)
            Case UserLevel.One : Return "One"
            Case UserLevel.Two : Return "Two"
            Case UserLevel.Three : Return "Three"
            Case UserLevel.Four : Return "Four"
            Case UserLevel.Five : Return "Five"

        End Select

        Return ""

    End Function

End Class