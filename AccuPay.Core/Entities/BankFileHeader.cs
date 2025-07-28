using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("bankfileheader")]
    public partial class BankFileHeader : OrganizationalEntity
    {
        public string CompanyCode { get; set; }
        public string FundingAccountNo { get; set; }
        public string PresentingOfficeNo { get; set; }
        public string BatchNo { get; set; }
    }

    public partial class BankFileHeader
    {
        private BankFileHeader()
        {
        }

        public BankFileHeader(int organizationID,
            int userId,
            string companyCode,
            string fundingAccountNo,
            string presentingOfficeNo,
            string batchNo)
        {
            OrganizationID = organizationID;
            CompanyCode = companyCode;
            FundingAccountNo = fundingAccountNo;
            PresentingOfficeNo = presentingOfficeNo;
            BatchNo = batchNo;
            CreatedBy = userId;
        }

        public static BankFileHeader NewBankFileHeader(int organizationID,
            int userId,
            string companyCode,
            string fundingAccountNo,
            string presentingOfficeNo,
            string batchNo) => new BankFileHeader(organizationID,
                userId: userId,
                companyCode: companyCode,
                fundingAccountNo: fundingAccountNo,
                presentingOfficeNo: presentingOfficeNo,
                batchNo: batchNo);
    }
}
