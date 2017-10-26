-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for function INSUPD_employeecertification
DROP FUNCTION IF EXISTS `INSUPD_employeecertification`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeecertification`(`ecer_RowID` INT, `ecer_OrganizationID` INT, `ecer_Created` TIMESTAMP, `ecer_CreatedBy` INT, `ecer_LastUpd` DATETIME, `ecer_LastUpdBy` INT, `ecer_EmployeeID` INT, `ecer_CertificationType` VARCHAR(50), `ecer_IssuingAuthority` VARCHAR(50), `ecer_CertificationNo` VARCHAR(50), `ecer_IssueDate` DATE, `ecer_ExpirationDate` DATE, `ecer_Comments` VARCHAR(2000)) RETURNS int(11)
    DETERMINISTIC
    COMMENT 'will insert a row and return its RowID if ''ecer_int'' don''t exist in employeecertification table or else will update the table base on ''ecer_int'''
BEGIN

DECLARE ecer_int INT(11);

INSERT INTO employeecertification
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,LastUpdBy
    ,EmployeeID
    ,CertificationType
    ,IssuingAuthority
    ,CertificationNo
    ,IssueDate
    ,ExpirationDate
    ,Comments
) VALUES (
    ecer_RowID
    ,ecer_OrganizationID
    ,ecer_Created
    ,ecer_CreatedBy
    ,ecer_LastUpdBy
    ,ecer_EmployeeID
    ,ecer_CertificationType
    ,ecer_IssuingAuthority
    ,ecer_CertificationNo
    ,ecer_IssueDate
    ,ecer_ExpirationDate
    ,ecer_Comments
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=ecer_LastUpdBy
    ,CertificationType=ecer_CertificationType
    ,IssuingAuthority=ecer_IssuingAuthority
    ,CertificationNo=ecer_CertificationNo
    ,IssueDate=ecer_IssueDate
    ,ExpirationDate=ecer_ExpirationDate
    ,Comments=ecer_Comments;SELECT @@Identity AS id INTO ecer_int;

RETURN ecer_int;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
