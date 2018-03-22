Imports System
Imports System.Configuration

Public Class AboutUs

    Private Sub AboutUs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim appSettings = ConfigurationManager.AppSettings

        Dim versionNumber = appSettings.Get("payroll.version")
        lblVersion.Text = "Version " & versionNumber
    End Sub

End Class