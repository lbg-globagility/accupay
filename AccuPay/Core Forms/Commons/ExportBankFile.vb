Option Strict On

Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class ExportBankFile

    Private _cutoffStart As Date

    Private _cutoffEnd As Date

    Public Sub New(cutoffStart As Date, cutoffEnd As Date)
        _cutoffStart = cutoffStart
        _cutoffEnd = cutoffEnd
    End Sub

    Public Sub Extract()
        Dim paystubs As IList(Of Paystub) = Nothing

        Using context = New PayrollContext()
            paystubs = context.Paystubs.Include(Function(p) p.Employee).
                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                Where(Function(p) p.PayFromdate = _cutoffStart And p.PayToDate = _cutoffEnd).
                ToList()
        End Using

        Dim sortedPaystubs = paystubs.
            Where(Function(p) Not String.IsNullOrWhiteSpace(p.Employee.BankName)).
            GroupBy(Function(p) p.Employee.BankName).ToList()

        Dim factory = New BankExporterFactory()

        Dim directory = CreateRandomDirectory()

        For Each paystubGroup In sortedPaystubs
            Dim bankName = paystubGroup.Key

            Dim exporter = factory.GetBankExporter(bankName)
            Dim stream = exporter.Export(paystubGroup.ToList())

            Dim fileName = $"{directory}/{bankName}.txt"

            WriteToFile(stream, fileName)
        Next

        Dim saveDialog = New SaveFileDialog With {
            .OverwritePrompt = True,
            .FileName = "bank.zip",
            .Filter = "Archive file|*.zip",
            .Title = "Save Zip"
        }

        saveDialog.ShowDialog()

        If saveDialog.FileName <> "" Then
            ZipFile.CreateFromDirectory(directory, saveDialog.FileName)
        End If

        System.IO.Directory.Delete(directory, True)
    End Sub

    Private Function CreateRandomDirectory() As String
        Dim random = Guid.NewGuid.ToString()
        Dim directory = $"{Path.GetTempPath()}/{random}"

        System.IO.Directory.CreateDirectory(directory)

        Return directory
    End Function

    Private Sub WriteToFile(stream As Stream, fileName As String)
        Using file = New FileStream(fileName, FileMode.CreateNew)
            stream.CopyTo(file)
        End Using
    End Sub

End Class
