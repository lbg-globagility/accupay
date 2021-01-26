Option Strict On

Imports System.IO
Imports AccuPay.Core.Services

<TestFixture>
Public Class TimeImporter

    Private _projectPath As String

    <SetUp>
    Public Sub Init()
        _projectPath = Directory.GetParent(
                            Directory.GetParent(
                                AppDomain.CurrentDomain.BaseDirectory
                            ).Parent.FullName
                        ).FullName
    End Sub

    <Test>
    Public Sub ShouldNotImport()
        Dim importer = New TimeLogsReader()

        Dim filename = _projectPath & "\_timelogs_test_errors_sdfsdf.dat"
        Dim importOutput = importer.Read(filename)

        Dim errors = importOutput.Errors

        Assert.That(importOutput.IsImportSuccess = False)
        Assert.That(importOutput.ErrorMessage = TimeLogsReader.FileNotFoundError)

    End Sub

    <Test>
    Public Sub ShouldImport()
        Dim importer = New TimeLogsReader()

        Dim filename = _projectPath & "\_timelogs_test.dat"
        Dim importOutput = importer.Read(filename)

        Dim logs = importOutput.Logs

        Assert.That(logs.Count = 17)
        Assert.That(importOutput.IsImportSuccess = True)

    End Sub

    <Test>
    Public Sub ShouldHaveErrors()
        Dim importer = New TimeLogsReader()

        Dim filename = _projectPath & "\_timelogs_test_errors.dat"

        Dim importOutput = importer.Read(filename)
        Dim errors = importOutput.Errors

        Dim contentFormat = "    {0}" & vbTab & "{1}" & vbTab & "{2}" & vbTab & "{3}" & vbTab & "{4}" & vbTab & "{5}"

        Assert.That(errors.Count = 7)

        Assert.That(errors.Item(0).ErrorMessage = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(0).LineNumber = 3)
        Assert.That(errors.Item(0).LineContent = String.Format(contentFormat,
                    "10123", "201A-06-02 07:00:00", "1", "0", "1", "0"))

        Assert.That(errors.Item(1).ErrorMessage = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(1).LineNumber = 7)
        Assert.That(errors.Item(1).LineContent = String.Format(contentFormat,
                    "10123", "2018-0B-04 20:00:00", "1", "0", "1", "0"))

        Assert.That(errors.Item(2).ErrorMessage = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(2).LineNumber = 9)
        Assert.That(errors.Item(2).LineContent = String.Format(contentFormat,
                    "10123", "2018-06-0C 03:00:00", "1", "0", "1", "0"))

        Assert.That(errors.Item(3).ErrorMessage = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(3).LineNumber = 10)
        Assert.That(errors.Item(3).LineContent = String.Format(contentFormat,
                    "10123", "2018-06-05 0D:00:00", "1", "0", "1", "0"))

        Assert.That(errors.Item(4).ErrorMessage = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(4).LineNumber = 12)
        Assert.That(errors.Item(4).LineContent = String.Format(contentFormat,
                    "10123", "2018-06-05 19:E0:00", "1", "0", "1", "0"))

        Assert.That(errors.Item(5).ErrorMessage = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(5).LineNumber = 15)
        Assert.That(errors.Item(5).LineContent = String.Format(contentFormat,
                    "10123", "2018-06-06 08:00:F0", "1", "0", "1", "0"))

        Assert.That(errors.Item(6).ErrorMessage = "Needs at least 2 items in one line separated by a tab.")
        Assert.That(errors.Item(6).LineNumber = 20)
        Assert.That(errors.Item(6).LineContent = "    101232018-06-07 23:30:001010")
    End Sub

End Class