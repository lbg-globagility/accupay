Option Strict On

Imports System.Text.RegularExpressions

Namespace Global.AccuPay

    Public Class Calendar

        Private Shared HoursInAClock As Integer = 12

        '^(?<hour>0?[0-9]|1[0-9]|2[0-3])(?:\s|\.|\:)*(?<minute>0[0-9]|1[0-9]|2[0-9]|3[0-9]|4[0-9]|5[0-9])\s*(?<clock>a|am|p|pm)?$

        Private Shared TimeRegex As Regex = New Regex(
            "^(0?[0-9]|1[0-9]|2[0-3])(?:\s|\.|\:)*(0[0-9]|1[0-9]|2[0-9]|3[0-9]|4[0-9]|5[0-9])\s*(a|am|p|pm)?$",
            RegexOptions.IgnoreCase)

        Public Shared Function ToDate(text As String) As Date
            Return Nothing
        End Function

        Public Shared Function ToTimespan(text As String) As TimeSpan?
            If String.IsNullOrWhiteSpace(text) Then
                Return Nothing
            End If

            Dim results = TimeRegex.Match(text)
            Dim groups = results.Groups

            If groups.Count < 2 Then
                Return Nothing
            End If

            Dim hour As Integer = If(groups.Count > 1, CInt(groups(1).Value), 0)
            Dim minute As Integer = If(groups.Count > 2, CInt(groups(2).Value), 0)
            Dim clock As String = If(groups.Count > 3, groups(3).Value, String.Empty)

            If String.IsNullOrEmpty(clock) Then
                Return New TimeSpan(hour, minute, 0)
            End If

            If IsAm(clock) Then
                If hour > HoursInAClock Then
                    Return Nothing
                End If

                hour = If(hour = HoursInAClock, 0, hour)

                Return New TimeSpan(hour, minute, 0)
            ElseIf IsPm(clock) Then
                hour = If(hour = HoursInAClock, hour, hour + HoursInAClock)

                Return New TimeSpan(hour, minute, 0)
            Else
                Return Nothing
            End If
        End Function

        Private Shared Function IsAm(clock As String) As Boolean
            Return String.Equals(clock, "am", StringComparison.OrdinalIgnoreCase) Or
                String.Equals(clock, "a", StringComparison.OrdinalIgnoreCase)
        End Function

        Private Shared Function IsPm(clock As String) As Boolean
            Return String.Equals(clock, "pm", StringComparison.OrdinalIgnoreCase) Or
                String.Equals(clock, "p", StringComparison.OrdinalIgnoreCase)
        End Function

    End Class

End Namespace
