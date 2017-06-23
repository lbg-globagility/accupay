-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for function goldwingspayrolldb.INSUPD_account
DROP FUNCTION IF EXISTS `INSUPD_account`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_account`(`acc_RowID` INT, `acc_CompanyName` VARCHAR(100), `acc_TradeName` VARCHAR(100), `acc_MobilePhone` VARCHAR(100), `acc_OrganizationID` INT, `acc_PrimaryContactID` INT, `acc_PrimaryAddressID` INT, `acc_PrimaryBillToAddressID` INT, `acc_PrimarySoldToAddressID` INT, `acc_PrimaryShipToAddressID` INT, `acc_PrimaryAgreementID` INT, `acc_PrimaryBillToPersonID` INT, `acc_PrimaryShipToPersonID` INT, `acc_PrimarySoldToPersonID` INT, `acc_ParentAccountID` INT, `acc_VATRegistrationNo` VARCHAR(50), `acc_ATPNo` VARCHAR(50), `acc_BranchFlg` VARCHAR(50), `acc_BusinessType` VARCHAR(50), `acc_PrimaryHomeAddressID` INT, `acc_FloorArea` DECIMAL(10,2), `acc_AverageMonthlySales` DECIMAL(10,2), `acc_YearsInBusiness` DECIMAL(10,2), `acc_ExistingBranch` VARCHAR(1), `acc_FaxNumber` VARCHAR(50), `acc_Description` VARCHAR(1000), `acc_Active` CHAR(1), `acc_ManagerName` VARCHAR(50), `acc_PaymentTermsID` INT, `acc_BranchType` VARCHAR(50), `acc_AliasName` VARCHAR(100), `acc_CustomerSinceDate` DATE, `acc_CreditDays` INT, `acc_StockHolder1` VARCHAR(100), `acc_StockHolder2` VARCHAR(100), `acc_StockHolder3` VARCHAR(100), `acc_Comments` VARCHAR(2000), `acc_GoodStandingFlg` CHAR(1), `acc_CreatedBy` INT, `acc_LastUpdBy` INT, `acc_PrimaryRepID` INT, `acc_EmailAddress` VARCHAR(100), `acc_AltEmailAddress` VARCHAR(100), `acc_Website` VARCHAR(100), `acc_MainPhone` VARCHAR(100), `acc_AltPhone` VARCHAR(100), `acc_Status` VARCHAR(50), `acc_Capitalization` DECIMAL(10,2), `acc_AverageRating` DECIMAL(10,2), `acc_Image` LONGBLOB) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE acctID INT(11);

DECLARE acctNumber INT(11) DEFAULT 0;

DECLARE maxAcctNumber INT(11) DEFAULT 0;



SELECT AccountNo FROM account WHERE AccountType='Supplier' AND OrganizationID=acc_OrganizationID AND CompanyName=acc_CompanyName ORDER BY RowID DESC LIMIT 1 INTO acctNumber;

SELECT AccountNo FROM account WHERE AccountType='Supplier' AND OrganizationID=acc_OrganizationID ORDER BY RowID DESC LIMIT 1 INTO maxAcctNumber;


INSERT INTO account
(
	RowID
	,CompanyName
	,TradeName
	,MobilePhone
	,OrganizationID
	,AccountNo
	,PrimaryContactID
	,PrimaryAddressID
	,PrimaryBillToAddressID
	,PrimarySoldToAddressID
	,PrimaryShipToAddressID
	,PrimaryAgreementID
	,PrimaryBillToPersonID
	,PrimaryShipToPersonID
	,PrimarySoldToPersonID
	,ParentAccountID
	,VATRegistrationNo
	,ATPNo
	,AccountType
	,BranchFlg
	,BusinessType
	,PrimaryHomeAddressID
	,FloorArea
	,AverageMonthlySales
	,YearsInBusiness
	,ExistingBranch
	,FaxNumber
	,Description
	,Active
	,ManagerName
	,PaymentTermsID
	,BranchType
	,AliasName
	,CustomerSinceDate
	,CreditDays
	,StockHolder1
	,StockHolder2
	,StockHolder3
	,Comments
	,GoodStandingFlg
	,Created
	,CreatedBy
	,LastUpdBy
	,PrimaryRepID
	,EmailAddress
	,AltEmailAddress
	,Website
	,MainPhone
	,AltPhone
	,`Status`
	,Capitalization
	,AverageRating
	,Image
) VALUES (
	acc_RowID
	,acc_CompanyName
	,acc_TradeName
	,acc_MobilePhone
	,acc_OrganizationID
	,(IFNULL(maxAcctNumber,0) + 1)
	,acc_PrimaryContactID
	,acc_PrimaryAddressID
	,acc_PrimaryBillToAddressID
	,acc_PrimarySoldToAddressID
	,acc_PrimaryShipToAddressID
	,acc_PrimaryAgreementID
	,acc_PrimaryBillToPersonID
	,acc_PrimaryShipToPersonID
	,acc_PrimarySoldToPersonID
	,acc_ParentAccountID
	,acc_VATRegistrationNo
	,acc_ATPNo
	,'Supplier'
	,acc_BranchFlg
	,acc_BusinessType
	,acc_PrimaryHomeAddressID
	,acc_FloorArea
	,acc_AverageMonthlySales
	,acc_YearsInBusiness
	,acc_ExistingBranch
	,acc_FaxNumber
	,acc_Description
	,acc_Active
	,acc_ManagerName
	,acc_PaymentTermsID
	,acc_BranchType
	,acc_AliasName
	,acc_CustomerSinceDate
	,acc_CreditDays
	,acc_StockHolder1
	,acc_StockHolder2
	,acc_StockHolder3
	,acc_Comments
	,acc_GoodStandingFlg
	,CURRENT_TIMESTAMP()
	,acc_CreatedBy
	,acc_LastUpdBy
	,acc_PrimaryRepID
	,acc_EmailAddress
	,acc_AltEmailAddress
	,acc_Website
	,acc_MainPhone
	,acc_AltPhone
	,acc_Status
	,acc_Capitalization
	,acc_AverageRating
	,acc_Image
) ON
DUPLICATE
KEY
UPDATE
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=acc_LastUpdBy
	,CompanyName=acc_CompanyName
	,TradeName=acc_TradeName
	,MobilePhone=acc_MobilePhone
	,PrimaryContactID=acc_PrimaryContactID
	,PrimaryAddressID=acc_PrimaryAddressID
	,PrimaryBillToAddressID=acc_PrimaryBillToAddressID
	,PrimarySoldToAddressID=acc_PrimarySoldToAddressID
	,PrimaryShipToAddressID=acc_PrimaryShipToAddressID
	,PrimaryAgreementID=acc_PrimaryAgreementID
	,PrimaryBillToPersonID=acc_PrimaryBillToPersonID
	,PrimaryShipToPersonID=acc_PrimaryShipToPersonID
	,PrimarySoldToPersonID=acc_PrimarySoldToPersonID
	,ParentAccountID=acc_ParentAccountID
	,VATRegistrationNo=acc_VATRegistrationNo
	,ATPNo=acc_ATPNo
	,BranchFlg=acc_BranchFlg
	,BusinessType=acc_BusinessType
	,PrimaryHomeAddressID=acc_PrimaryHomeAddressID
	,FloorArea=acc_FloorArea
	,AverageMonthlySales=acc_AverageMonthlySales
	,YearsInBusiness=acc_YearsInBusiness
	,ExistingBranch=acc_ExistingBranch
	,FaxNumber=acc_FaxNumber
	,Description=acc_Description
	,Active=acc_Active
	,ManagerName=acc_ManagerName
	,PaymentTermsID=acc_PaymentTermsID
	,BranchType=acc_BranchType
	,AliasName=acc_AliasName
	,CustomerSinceDate=acc_CustomerSinceDate
	,CreditDays=acc_CreditDays
	,StockHolder1=acc_StockHolder1
	,StockHolder2=acc_StockHolder2
	,StockHolder3=acc_StockHolder3
	,Comments=acc_Comments
	,GoodStandingFlg=acc_GoodStandingFlg
	,PrimaryRepID=acc_PrimaryRepID
	,EmailAddress=acc_EmailAddress
	,AltEmailAddress=acc_AltEmailAddress
	,Website=acc_Website
	,MainPhone=acc_MainPhone
	,AltPhone=acc_AltPhone
	,`Status`=acc_Status
	,Capitalization=acc_Capitalization
	,AverageRating=acc_AverageRating
	,Image=acc_Image;SELECT @@Identity AS id INTO acctID;

RETURN acctID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
