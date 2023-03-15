Imports System.ComponentModel
Imports System.Threading.Tasks

Public Class SwitchCompanySplashForm
    Private primaryForm As MDIPrimaryForm

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub SwitchCompanySplashForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim t = Task.Run(Async Function()
                             Await Task.Delay(5000)
                             Return Task.FromResult(MDIPrimaryForm)
                         End Function)

        t.Wait()

        If t.IsCompleted Then
            e.Result = t.Result.Result
        End If
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged

    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        primaryForm = CType(e.Result, MDIPrimaryForm)
        Close()
        'MsgBox("BackgroundWorker1_RunWorkerCompleted...")
    End Sub

    Private Sub SwitchCompanySplashForm_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        'primaryForm.Show()
        Dim exemptedNames = {"MetroLogin", Name}
        Dim openedForms = My.Application.OpenForms.
            OfType(Of Form).
            Where(Function(f) Not exemptedNames.Contains(f.Name)).
            ToList()

        For Each form In openedForms
            form.Close()
            form.Dispose()
            Dim formName = form.Name
            If KnownForms.GeneralFormNames.Contains(formName) Then
                GeneralForm.listGeneralForm.Remove(formName)
            ElseIf KnownForms.HrisFormNames.Contains(formName) Then
                HRISForm.listHRISForm.Remove(formName)
            ElseIf KnownForms.TimeAndAttendanceFormNames.Contains(formName) Then
                TimeAttendForm.listTimeAttendForm.Remove(formName)
            ElseIf KnownForms.PayrollFormNames.Contains(formName) Then
                PayrollForm.listPayrollForm.Remove(formName)
                'ElseIf KnownForms.ReportFormNames.Contains(formName) Then
                'PayrollForm.listPayrollForm.Remove(formName)
            End If
        Next
    End Sub

End Class
