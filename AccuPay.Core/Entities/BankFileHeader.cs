using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccuPay.Core.Entities
{
    [Table("bankfileheader")]
    public partial class BankFileHeader : OrganizationalEntity
    {
        public string CompanyCode { get; set; }
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
            string batchNo)
        {
            OrganizationID = organizationID;
            CompanyCode = companyCode;
            BatchNo = batchNo;
            CreatedBy = userId;
        }

        public static BankFileHeader NewBankFileHeader(int organizationID,
            int userId,
            string companyCode,
            string batchNo) => new BankFileHeader(organizationID,
                userId: userId,
                companyCode: companyCode,
                batchNo: batchNo);
    }
}
