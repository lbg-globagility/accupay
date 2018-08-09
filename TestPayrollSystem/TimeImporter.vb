Option Strict On

Imports AccuPay

<TestFixture>
Public Class TimeImporter

    <Test>
    Public Sub ShouldImport()
        Dim importer = New NewImporter()
        Dim filename = "C:/Users/Aaron/Desktop/1_attlog.dat"
        importer.Import(filename)
    End Sub

End Class
