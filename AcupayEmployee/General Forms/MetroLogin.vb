Imports System.Linq.Expressions

Public Class MetroLogin

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = Keys.Escape Then
            Me.Close()
            Return True
        ElseIf keyData = Keys.Oem5 Then
            Static thrice As Integer = -1

            thrice += 1
            If thrice = 5 Then
                thrice = 0

                Dim n_ShiftTemplater As _
                    New ViewTimeEntryEmployeeLevel
                n_ShiftTemplater.Dispose()
            End If

            Return True
        Else
            Return MyBase.ProcessCmdKey(msg, keyData)
        End If
    End Function

    Private Sub MetroLogin_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Application.Exit()
    End Sub

    Private Sub lnklblleave_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblleave.LinkClicked

        Dim n_LeaveForm As New LeaveForm

        With n_LeaveForm

            .CboListOfValue1.Visible = False

            .Label3.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub lnklblovertime_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblovertime.LinkClicked

        Dim n_OverTimeForm As New OverTimeForm

        With n_OverTimeForm

            .cboOTStatus.Visible = False

            .Label186.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub lnklblobf_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblobf.LinkClicked

        Dim n_OBFForm As New OBFForm

        With n_OBFForm

            .cboOBFStatus.Visible = False

            .Label186.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked

        Dim my_time_entry As New ViewTimeEntryEmployeeLevel

        my_time_entry.Show()

    End Sub

End Class