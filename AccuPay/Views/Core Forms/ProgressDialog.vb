Option Strict On

Imports AccuPay.Data.Interfaces

Public Class ProgressDialog

    Private _generator As IProgressGenerator

    Public Sub New(generator As IProgressGenerator, Optional title As String = "Progress Dialog")
        InitializeComponent()
        _generator = generator
        CompletionProgressBar.Value = 1

        Me.Text = title

        ProgressTimer.Start()
    End Sub

    Private Sub ProgressTimer_Tick(sender As Object, e As EventArgs) Handles ProgressTimer.Tick
        CompletionProgressBar.Value = {_generator.Progress, 1}.Max()
    End Sub

    Private Sub ProgressDialog_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ProgressTimer.Stop()
    End Sub

    Private Sub ProgressDialog_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ProgressTimer.Dispose()
    End Sub

End Class
