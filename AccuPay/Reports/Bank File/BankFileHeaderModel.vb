Public Class BankFileHeaderModel

    Public Sub New(organizationId As Integer,
            companyCode As String,
            fundingAccountNo As String,
            presentingOfficeNo As String,
            batchNo As String)

        Me.OrganizationId = organizationId
        Me.CompanyCode = companyCode
        Me.FundingAccountNo = fundingAccountNo
        Me.PresentingOfficeNo = presentingOfficeNo
        Me.BatchNo = batchNo
    End Sub

    Public ReadOnly Property OrganizationId As Integer
    Public ReadOnly Property CompanyCode As String
    Public ReadOnly Property FundingAccountNo As String
    Public ReadOnly Property PresentingOfficeNo As String
    Public ReadOnly Property BatchNo As String

    Public ReadOnly Property TextData As String
        Get
            Return String.Join(vbTab, {OrganizationId, CompanyCode, FundingAccountNo, PresentingOfficeNo, BatchNo})
        End Get
    End Property

End Class
