Option Strict On

Imports System.IO
Imports System.IO.Compression
Imports System.Threading.Tasks
Imports AccuPay.Data.Repositories

Public Class ExportBankFile

    Private ReadOnly _cutoffStart As Date

    Private ReadOnly _cutoffEnd As Date

    Private ReadOnly _paystubRepository As PaystubRepository

    Public Sub New(cutoffStart As Date, cutoffEnd As Date)
        _cutoffStart = cutoffStart
        _cutoffEnd = cutoffEnd
        _paystubRepository = New PaystubRepository()
    End Sub

    Public Async Function Extract() As Task

        Dim paystubDateKey = New PaystubRepository.DateCompositeKey(z_OrganizationID,
                                                                    payFromDate:=_cutoffStart,
                                                                    payToDate:=_cutoffEnd)
        Dim paystubs = Await _paystubRepository.GetAllWithEmployeeAsync(paystubDateKey)

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
    End Function

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