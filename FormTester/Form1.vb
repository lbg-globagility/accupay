Option Strict On

Imports System.IO
Imports AccuPay.Core.Interfaces
Imports AccuPay.CrystalReports
Imports CrystalDecisions.Shared
Imports GlobagilityShared.EmailSender
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports PdfSharp.Pdf.Security

Public Class Form1

    Private ReadOnly _payslipBuilder As IPayslipBuilder
    Private ReadOnly _paystubEmailRepository As IPaystubEmailRepository
    Private ReadOnly _encryptor As IEncryption

    Sub New(payslipBuilder As IPayslipBuilder, paystubEmailRepositoryrepo As IPaystubEmailRepository, encryptor As IEncryption)

        InitializeComponent()

        _payslipBuilder = payslipBuilder
        _paystubEmailRepository = paystubEmailRepositoryrepo
        _encryptor = encryptor
    End Sub

    Private Async Sub PrintPayslipButton_Click(sender As Object, e As EventArgs) Handles PrintPayslipButton.Click
        Dim reportDocument As CrystalDecisions.CrystalReports.Engine.ReportClass = Await GetPayslipReport()

        Dim crvwr As New CrystalReportsFormViewer
        crvwr.CrystalReportViewer1.ReportSource = reportDocument
        crvwr.Show()

    End Sub

    Private Async Sub PDFPayslipButton_Click(sender As Object, e As EventArgs) Handles PDFPayslipButton.Click
        Dim reportDocument As CrystalDecisions.CrystalReports.Engine.ReportClass = Await GetPayslipReport()

        Dim pdfName = "Payslip1.pdf"
        Dim password = "admin"
        Dim pdfFullPath As String = Path.Combine("C:\Downloads", pdfName)

        Dim CrExportOptions As ExportOptions
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

    Private Async Function GetPayslipReport() As Task(Of CrystalDecisions.CrystalReports.Engine.ReportClass)
        Dim currentPayPeriod As New PayPeriod(isFirstHalf:=False) With {
            .RowID = 620,
            .PayFromDate = New Date(2019, 10, 1),
            .PayToDate = New Date(2019, 10, 15)
        }

        Dim employeeIds = EmployeesTextBox.Text.Split(","c).[Select](AddressOf Integer.Parse).ToArray()

        Dim builder = Await _payslipBuilder.CreateReportDocumentAsync(
            currentPayPeriod.RowID.Value,
            isActual:=False,
            employeeIds:=employeeIds)

        Return builder.GetReportDocument()
    End Function

    Private Async Sub ViewPayslipFromLibraryButton_Click(sender As Object, e As EventArgs) Handles ViewPayslipFromLibraryButton.Click
        Dim pdfFile = Await GetPDF()

        MessageBox.Show("Finished")
    End Sub

    Private Sub SendEmailButton_Click(sender As Object, e As EventArgs) Handles SendEmailButton.Click

        Dim emailSender As New EmailSender(New EmailConfig())

        Dim sendTo = SendToTextBox.Text
        Dim subject = SubjectTextBox.Text
        Dim body = BodyTextBox.Text

        Dim attachments As String() = Nothing

        If Not String.IsNullOrWhiteSpace(AttachmentTextBox.Text) Then
            attachments = AttachmentTextBox.Text.Split(","c).ToArray()
        End If

        emailSender.SendEmail(sendTo, subject, body, attachments)

        MessageBox.Show("Sent!")

    End Sub

    Private Async Sub EmailPayslipButton_Click(sender As Object, e As EventArgs) Handles EmailPayslipButton.Click
        Dim pdfFile = Await GetPDF()

        Dim emailSender As New EmailSender(New EmailConfig())

        Dim sendTo = SendToTextBox.Text
        Dim subject = SubjectTextBox.Text
        Dim body = BodyTextBox.Text

        Dim attachments As String() = New String() {pdfFile}

        If Not String.IsNullOrWhiteSpace(AttachmentTextBox.Text) Then
            attachments = AttachmentTextBox.Text.Split(","c).ToArray()
        End If

        emailSender.SendEmail(sendTo, subject, body, attachments)

        MessageBox.Show("Finished")

    End Sub

    Private Async Function GetPDF() As Task(Of String)
        Dim currentPayPeriod As New PayPeriod(isFirstHalf:=False) With {
            .RowID = 620,
            .PayFromDate = New Date(2019, 10, 1),
            .PayToDate = New Date(2019, 10, 15)
        }

        Dim employeeIds = EmployeesTextBox.Text.Split(","c).[Select](AddressOf Integer.Parse).ToArray()

        Dim saveFolderPath = "E:\Downloads"
        Dim fileName = FileNameTextBox.Text
        Dim password = PasswordTextBox.Text

        Dim builder = Await _payslipBuilder.
            CreateReportDocumentAsync(
                currentPayPeriod.RowID.Value,
                isActual:=False,
                employeeIds:=employeeIds)

        builder.GeneratePDF(saveFolderPath, fileName)

        Return builder.
                AddPdfPassword(password).
                GetPDF()
    End Function

    Private Async Sub OfficialPayslipButton_Click(sender As Object, e As EventArgs) Handles OfficialPayslipButton.Click
        Dim currentPayPeriod As New PayPeriod(isFirstHalf:=False) With {
            .RowID = 620,
            .PayFromDate = New Date(2019, 10, 1),
            .PayToDate = New Date(2019, 10, 15)
        }

        Dim nextPayPeriod As New PayPeriod(isFirstHalf:=False) With {
            .RowID = 621,
            .PayFromDate = New Date(2019, 10, 16),
            .PayToDate = New Date(2019, 10, 31)
        }

        Dim employeeIds = EmployeesTextBox.Text.Split(","c).[Select](AddressOf Integer.Parse).ToArray()

        Await _payslipBuilder.CreateReportDocumentAsync(
            currentPayPeriod.RowID.Value,
            isActual:=False,
            employeeIds:=employeeIds)

        Dim employee = _payslipBuilder.GetFirstEmployee()

        If employee Is Nothing Then
            Return
        End If

        Dim saveFolderPath = "E:\Downloads"
        Dim fileName = $"Payslip-{currentPayPeriod.PayToDate:yyyy-MM-dd}-11.pdf"
        Dim password = CDate(employee("Birthdate")).ToString("MMddyyyy")

        Dim builder = CType(_payslipBuilder.GeneratePDF(saveFolderPath, fileName), IPayslipBuilder)
        builder.AddPdfPassword(password)

        Dim pdfFile = _payslipBuilder.GetPDF()

        Dim emailSender As New EmailSender(New EmailConfig())

        Dim cutoffDate = currentPayPeriod.PayToDate.ToString("MMMM d, yyyy")

        Dim sendTo = employee("EmailAddress").ToString()
        Dim subject = $"Please see attached payslip for {cutoffDate}"
        Dim body = $"Please see attached payslip for {cutoffDate}.

Kindly contact HRD if you have concerns regarding your salary.

Thank you,
HRD"

        Dim attachments As String() = New String() {pdfFile}

        emailSender.SendEmail(sendTo, subject, body, attachments)

        MessageBox.Show("Finished")

    End Sub

    Private Sub QueryPaystubButton_Click(sender As Object, e As EventArgs) Handles QueryPaystubButton.Click

        Dim paystubEmail = _paystubEmailRepository.FirstOnQueueWithPaystubDetails()

        MessageBox.Show(paystubEmail.PaystubID.ToString())

    End Sub

    Private Sub EncryptButton_Click(sender As Object, e As EventArgs) Handles EncryptButton.Click

        Try

            EncryptOutputTextBox.Text = _encryptor.Encrypt(EncryptInputTextBox.Text)
        Catch ex As Exception

            MessageBox.Show("Cannot encrypt data.")

        End Try

    End Sub

    Private Sub DecryptButton_Click(sender As Object, e As EventArgs) Handles DecryptButton.Click

        Try

            EncryptOutputTextBox.Text = _encryptor.Decrypt(EncryptInputTextBox.Text)
        Catch ex As Exception

            MessageBox.Show("Cannot decrypt data.")

        End Try

    End Sub

    Private Sub CapitalizeButton_Click(sender As Object, e As EventArgs) Handles CapitalizeButton.Click
        EncryptOutputTextBox.Text = EncryptOutputTextBox.Text.ToUpper()
    End Sub

End Class
