Imports System.IO
Imports Accupay.Payslip
Imports CrystalDecisions.Shared
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports PdfSharp.Pdf.Security

Public Class Form1

    Private Sub PrintPayslipButton_Click(sender As Object, e As EventArgs) Handles PrintPayslipButton.Click
        Dim reportDocument As CrystalDecisions.CrystalReports.Engine.ReportClass = GetPayslipReport()

        Dim crvwr As New CrystalReportsFormViewer
        crvwr.CrystalReportViewer1.ReportSource = reportDocument
        crvwr.Show()

    End Sub

    Private Sub PDFPayslipButton_Click(sender As Object, e As EventArgs) Handles PDFPayslipButton.Click
        Dim reportDocument As CrystalDecisions.CrystalReports.Engine.ReportClass = GetPayslipReport()

        Dim pdfName = "Payslip1.pdf"
        Dim password = "admin"
        Dim pdfFullPath As String = Path.Combine("E:\Downloads", pdfName)

        Dim CrExportOptions As CrystalDecisions.[Shared].ExportOptions
        Dim CrDiskFileDestinationOptions As DiskFileDestinationOptions = New DiskFileDestinationOptions()
        Dim CrFormatTypeOptions As PdfRtfWordFormatOptions = New PdfRtfWordFormatOptions()
        CrDiskFileDestinationOptions.DiskFileName = pdfFullPath
        CrExportOptions = reportDocument.ExportOptions

        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile
        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions
        CrExportOptions.FormatOptions = CrFormatTypeOptions

        reportDocument.Export()

        AddPdfPassword(pdfFullPath, password)

        MessageBox.Show("Finished")
    End Sub

    Private Sub AddPdfPassword(filepath As String, password As String)
        Dim document As PdfDocument = PdfReader.Open(filepath)
        Dim securitySettings As PdfSecuritySettings = document.SecuritySettings
        securitySettings.UserPassword = password
        securitySettings.OwnerPassword = password
        securitySettings.PermitAccessibilityExtractContent = False
        securitySettings.PermitAnnotations = False
        securitySettings.PermitAssembleDocument = False
        securitySettings.PermitExtractContent = False
        securitySettings.PermitFormsFill = True
        securitySettings.PermitFullQualityPrint = False
        securitySettings.PermitModifyDocument = True
        securitySettings.PermitPrint = False
        document.Save(filepath)
    End Sub

    Private Function GetPayslipReport() As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim payslipCreator As New PayslipCreator(619, isActual:=False)

        Dim nextPayPeriod As New PayPeriod With {
        .RowID = 620,
        .PayFromDate = New Date(2019, 10, 1),
        .PayToDate = New Date(2019, 10, 15)
        }

        Dim employeeIds = EmployeesTextBox.Text.Split(","c).[Select](AddressOf Integer.Parse).ToArray()

        Dim reportDocument = payslipCreator.
                CreateReportDocument("Cinema 2000s", 2, nextPayPeriod, employeeIds).
                GetReportDocument()

        Return reportDocument
    End Function

    Private Sub ViewPayslipFromLibraryButton_Click(sender As Object, e As EventArgs) Handles ViewPayslipFromLibraryButton.Click
        Dim payslipCreator As New PayslipCreator(619, isActual:=False)

        Dim nextPayPeriod As New PayPeriod With {
        .RowID = 620,
        .PayFromDate = New Date(2019, 10, 1),
        .PayToDate = New Date(2019, 10, 15)
        }

        Dim employeeIds = EmployeesTextBox.Text.Split(","c).[Select](AddressOf Integer.Parse).ToArray()

        Dim saveFolderPath = "E:\Downloads"
        Dim fileName = FileNameTextBox.Text
        Dim password = PasswordTextBox.Text

        Dim reportDocument = payslipCreator.
                CreateReportDocument("Cinema 2000s", 2, nextPayPeriod, employeeIds).
                GeneratePDF(saveFolderPath, fileName).
                AddPdfPassword(password).
                GetPDF()

        MessageBox.Show("Finished")
    End Sub

End Class