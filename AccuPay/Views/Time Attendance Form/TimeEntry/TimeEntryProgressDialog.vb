Option Strict On

Imports AccuPay.Data.Services

Public Class TimeEntryProgressDialog

    Private _generator As TimeEntryGenerator

    Public Sub New(generator As TimeEntryGenerator)
        InitializeComponent()
        _generator = generator
        CompletionProgressBar.Value = 1

        ProgressTimer.Start()
    End Sub

    Private Sub ProgressTimer_Tick(sender As Object, e As EventArgs) Handles ProgressTimer.Tick
        CompletionProgressBar.Value = {_generator.Progress, 1}.Max()
    End Sub

    Private Sub TimeEntryProgressDialog_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ProgressTimer.Stop()
    End Sub

    Private Sub TimeEntryProgressDialog_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ProgressTimer.Dispose()
    End Sub

End Class