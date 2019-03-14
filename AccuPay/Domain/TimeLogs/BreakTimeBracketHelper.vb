Imports AccuPay.Entity

Public Class BreakTimeBracketHelper

    Friend Shared Function GetBreakTimeDuration(
                            breakTimeBrackets As IList(Of BreakTimeBracket),
                            shiftDuration As Double) As Decimal

        breakTimeBrackets = breakTimeBrackets.
                                OrderBy(Function(b) b.ShiftDuration).
                                ThenBy(Function(b) b.BreakDuration).
                                ToList

        Dim lastBreakTimeBracket As New BreakTimeBracket

        For Each breakTimeBracket In breakTimeBrackets

            lastBreakTimeBracket = breakTimeBracket

            If breakTimeBracket.ShiftDuration >= shiftDuration Then

                Exit For

            End If

        Next

        If lastBreakTimeBracket Is Nothing Then

            Return 0

        Else

            Return lastBreakTimeBracket.BreakDuration

        End If

    End Function
End Class