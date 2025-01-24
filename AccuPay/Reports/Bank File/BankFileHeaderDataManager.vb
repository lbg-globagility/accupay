Imports System.IO

Public Class BankFileHeaderDataManager
    Private Const FILE_NAME As String = "BankFileHeaderData.dat"
    Private ReadOnly _organizationId As Integer
    Private _source As List(Of BankFileHeaderModel)

    Public Sub New(organizationId As Integer)
        _organizationId = organizationId

        _source = New List(Of BankFileHeaderModel)

        If Not IsFileExists Then Return

        For Each line As String In File.ReadAllLines(FILE_NAME)
            Dim fields = line.Split(vbTab)
            _source.Add(New BankFileHeaderModel(organizationId:=organizationId,
                companyCode:=fields(1),
                fundingAccountNo:=fields(2),
                presentingOfficeNo:=fields(3),
                batchNo:=fields(4)))
        Next
    End Sub

    Public Function GetBankFileHeaderModel() As BankFileHeaderModel
        Return _source.FirstOrDefault(Function(t) t.OrganizationId = _organizationId)
    End Function

    Public Sub Save(companyCode As String,
            fundingAccountNo As String,
            presentingOfficeNo As String,
            batchNo As String)

        Dim isHasChanges = HasChanges(companyCode:=companyCode,
            fundingAccountNo:=fundingAccountNo,
            presentingOfficeNo:=presentingOfficeNo,
            batchNo:=batchNo)

        If Not isHasChanges Then Return

        Using sw As New StreamWriter(FILE_NAME)
            For Each item In _source.Where(Function(t) Not t.OrganizationId = _organizationId)
                sw.WriteLine(item.TextData)
            Next

            Dim updatedData = String.Join(vbTab,
                {_organizationId, companyCode, fundingAccountNo, presentingOfficeNo, batchNo})
            sw.WriteLine(updatedData)
        End Using
    End Sub

    Private Function HasChanges(companyCode As String,
            fundingAccountNo As String,
            presentingOfficeNo As String,
            batchNo As String) As Boolean
        Dim model = GetBankFileHeaderModel()
        Return Not model.CompanyCode = companyCode OrElse
            Not model.FundingAccountNo = fundingAccountNo OrElse
            Not model.PresentingOfficeNo = presentingOfficeNo OrElse
            Not model.BatchNo = batchNo
    End Function

    Friend Shared ReadOnly Property IsFileExists As Boolean
        Get
            Return File.Exists(FILE_NAME)
        End Get
    End Property

End Class
