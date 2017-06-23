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

-- Dumping structure for function goldwingspayrolldb.INSUPD_employeedependents
DROP FUNCTION IF EXISTS `INSUPD_employeedependents`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeedependents`(`emp_RowID` INT, `emp_CreatedBy` INT, `emp_LastUpdBy` INT, `emp_LastUpd` DATETIME, `emp_OrganizationID` INT, `emp_Salutation` VARCHAR(50), `emp_FirstName` VARCHAR(50), `emp_MiddleName` VARCHAR(50), `emp_LastName` VARCHAR(50), `emp_SurName` VARCHAR(50), `emp_ParentEmployeeID` VARCHAR(50), `emp_TINNo` VARCHAR(50), `emp_SSSNo` VARCHAR(50), `emp_HDMFNo` VARCHAR(50), `emp_PhilHealthNo` VARCHAR(50), `emp_EmailAddress` VARCHAR(50), `emp_WorkPhone` VARCHAR(50), `emp_HomePhone` VARCHAR(50), `emp_MobilePhone` VARCHAR(50), `emp_HomeAddress` VARCHAR(2000), `emp_Nickname` VARCHAR(50), `emp_JobTitle` VARCHAR(50), `emp_Gender` VARCHAR(50), `emp_RelationToEmployee` VARCHAR(50), `emp_ActiveFlag` VARCHAR(50), `emp_Birthdate` DATE, `emp_IsDoneByImporting` TEXT) RETURNS int(11)
    DETERMINISTIC
    COMMENT 'will insert a row and return its RowID if ''empdepenID'' don''t exist in employeedependents table or else will update the table base on ''empdepenID'''
BEGIN

DECLARE empdepenID INT(11);



IF emp_IsDoneByImporting = '1' THEN
	
	INSERT INTO listofval
	(
		DisplayValue,LIC,`Type`,ParentLIC,Active,Description,Created,CreatedBy,LastUpd,OrderBy,LastUpdBy
	)
	VALUES
	(
		emp_ParentEmployeeID,CONCAT(emp_ParentEmployeeID,';EmployeeDependent'),'EmployeeDependent',emp_ParentEmployeeID,'No','',CURRENT_TIMESTAMP(),emp_CreatedBy,CURRENT_TIMESTAMP(),1,emp_CreatedBy
	) ON
	DUPLICATE
	KEY
	UPDATE
		LastUpd=CURRENT_TIMESTAMP();

END IF;


INSERT INTO employeedependents
(
	RowID
	,Created
	,CreatedBy
	,OrganizationID
	,Salutation
	,FirstName
	,MiddleName
	,LastName
	,SurName
	,ParentEmployeeID
	,TINNo
	,SSSNo
	,HDMFNo
	,PhilHealthNo
	,EmailAddress
	,WorkPhone
	,HomePhone
	,MobilePhone
	,HomeAddress
	,Nickname
	,JobTitle
	,Gender
	,RelationToEmployee
	,ActiveFlag
	,Birthdate
) VALUES (
	emp_RowID
	,CURRENT_TIMESTAMP()
	,emp_CreatedBy
	,emp_OrganizationID
	,emp_Salutation
	,emp_FirstName
	,emp_MiddleName
	,emp_LastName
	,emp_SurName
	,emp_ParentEmployeeID
	,emp_TINNo
	,emp_SSSNo
	,emp_HDMFNo
	,emp_PhilHealthNo
	,emp_EmailAddress
	,emp_WorkPhone
	,emp_HomePhone
	,emp_MobilePhone
	,SUBSTRING(emp_HomeAddress,1,1000)
	,emp_Nickname
	,emp_JobTitle
	,emp_Gender
	,emp_RelationToEmployee
	,emp_ActiveFlag
	,emp_Birthdate
) ON
DUPLICATE
KEY
UPDATE
	LastUpdBy=emp_LastUpdBy
	,LastUpd=CURRENT_TIMESTAMP()
	,Salutation=emp_Salutation
	,FirstName=emp_FirstName
	,MiddleName=emp_MiddleName
	,LastName=emp_LastName
	,SurName=emp_SurName
	,ParentEmployeeID=emp_ParentEmployeeID
	,TINNo=emp_TINNo
	,SSSNo=emp_SSSNo
	,HDMFNo=emp_HDMFNo
	,PhilHealthNo=emp_PhilHealthNo
	,EmailAddress=emp_EmailAddress
	,WorkPhone=emp_WorkPhone
	,HomePhone=emp_HomePhone
	,MobilePhone=emp_MobilePhone
	,HomeAddress=emp_HomeAddress
	,Nickname=emp_Nickname
	,JobTitle=emp_JobTitle
	,Gender=emp_Gender
	,RelationToEmployee=emp_RelationToEmployee
	,ActiveFlag=emp_ActiveFlag
	,Birthdate=emp_Birthdate;SELECT @@Identity AS id INTO empdepenID;




RETURN empdepenID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
