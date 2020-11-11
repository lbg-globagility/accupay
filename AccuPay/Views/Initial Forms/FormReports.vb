Option Strict On

Imports Microsoft.Extensions.DependencyInjection

Public Class FormReports

    Public listReportsForm As New List(Of String)

    Sub ChangeForm(ByVal Formname As Form)

        Try
            Application.DoEvents()
            Dim FName As String = Formname.Name
            'Formname.WindowState = FormWindowState.Maximized
            Formname.TopLevel = False
            If listReportsForm.Contains(FName) Then
                Formname.Show()
                Formname.BringToFront()
            Else
                PanelReport.Controls.Add(Formname)
                listReportsForm.Add(Formname.Name)

                Formname.Show()
                Formname.BringToFront()

                'Formname.Location = New Point((PanelGeneral.Width / 2) - (Formname.Width / 2), (PanelGeneral.Height / 2) - (Formname.Height / 2))
                'Formname.Anchor = AnchorStyles.Top And AnchorStyles.Bottom And AnchorStyles.Right And AnchorStyles.Left
                'Formname.WindowState = FormWindowState.Maximized
                Formname.Dock = DockStyle.Fill

            End If
        Catch ex As Exception

            Exit Sub

        End Try

    End Sub

    Private Sub FormReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'lvMainMenu.LargeImageList = ImageList1

        tsbtnstartreport_Click(sender, e)

    End Sub

    Private Sub tsbtnstartreport_Click(sender As Object, e As EventArgs) Handles tsbtnstartreport.Click

        ChangeForm(ReportsList)

        previousForm = ReportsList

    End Sub

End Class
