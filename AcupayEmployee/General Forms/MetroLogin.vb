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

Friend Class EncryptData

    Dim n_ResultValue = Nothing

    Property ResultValue As Object

        Get
            Return n_ResultValue

        End Get

        Set(value As Object)
            n_ResultValue = value

        End Set

    End Property

    Sub New(StringToEncrypt As String)

        n_ResultValue = EncrypedData(StringToEncrypt)

    End Sub

    Private Function EncrypedData(ByVal a As String)

        Dim Encryped = Nothing

        If Not a Is Nothing Then

            For Each x As Char In a

                Dim ToCOn As Integer = Convert.ToInt64(x) + 133

                Encryped &= Convert.ToChar(Convert.ToInt64(ToCOn))

            Next

        End If

        Return Encryped

    End Function

    Private Sub SampleExpression()
        ' Creating an expression tree. 
        Dim expr As Expression(Of Func(Of Integer, Boolean)) =
            Function(num) num < 5

        ' Compiling the expression tree into a delegate. 
        Dim result As Func(Of Integer, Boolean) = expr.Compile()

        ' Invoking the delegate and writing the result to the console.
        Console.WriteLine(result(4))

        ' Prints True. 

        ' You can also use simplified syntax 
        ' to compile and run an expression tree. 
        ' The following line can replace two previous statements.
        Console.WriteLine(expr.Compile()(4))

        ' Also prints True.

    End Sub

End Class