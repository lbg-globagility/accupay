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

-- Dumping structure for procedure transprintpayrolldb.INSUPD_position
DROP PROCEDURE IF EXISTS `INSUPD_position`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `INSUPD_position`(IN `pos_RowID` INT, IN `pos_PositionName` VARCHAR(50), IN `pos_CreatedBy` INT, IN `pos_OrganizationID` INT, IN `pos_LastUpdBy` INT, IN `pos_ParentPositionID` INT, IN `pos_DivisionId` INT)
    DETERMINISTIC
BEGIN

DECLARE positID
        ,defaultDivisID INT(11);	

DECLARE default_division_name VARCHAR(50) DEFAULT 'Default Division';

SELECT COUNT(RowID) FROM `division` WHERE OrganizationID=pos_OrganizationID AND ParentDivisionID IS NOT NULL INTO defaultDivisID;

IF defaultDivisID > 0 THEN
	
	IF pos_DivisionId IS NULL THEN
		
		SELECT RowID FROM division WHERE Name = default_division_name AND OrganizationID=pos_OrganizationID AND ParentDivisionID IS NOT NULL ORDER BY RowID LIMIT 1 INTO pos_DivisionId;
		
	END IF;
	
	INSERT INTO `position`
	(
		RowID
		,PositionName
		,Created
		,CreatedBy
		,OrganizationID
		,LastUpdBy
		# ,ParentPositionID
		,DivisionId
	) VALUES (
		pos_RowID
		,pos_PositionName
		,CURRENT_TIMESTAMP()
		,pos_CreatedBy
		,pos_OrganizationID
		,pos_LastUpdBy
		# ,pos_ParentPositionID
		,pos_DivisionId
	) ON 
	DUPLICATE 
	KEY 
	UPDATE 
		PositionName=pos_PositionName
		,LastUpd=CURRENT_TIMESTAMP()
		,LastUpdBy=pos_LastUpdBy
		# ,ParentPositionID=pos_ParentPositionID
		,DivisionId=pos_DivisionId;SELECT @@Identity AS Id INTO positID;
	
ELSE

	INSERT INTO `division`
	(
		Name
		,OrganizationID
		,CreatedBy
		,Created
	) VALUES (
		default_division_name
		,pos_OrganizationID
		,pos_CreatedBy
		,CURRENT_TIMESTAMP()
	) ON
	DUPLICATE
	KEY
	UPDATE
		LastUpd=CURRENT_TIMESTAMP()
		,LastUpdBy=pos_LastUpdBy;
	
	SELECT RowID FROM `division` WHERE Name = default_division_name AND OrganizationID=pos_OrganizationID ORDER BY RowID DESC LIMIT 1 INTO defaultDivisID;

	INSERT INTO `position` 
	(
		RowID
		,PositionName
		,Created
		,CreatedBy
		,OrganizationID
		,LastUpdBy
		# ,ParentPositionID
		,DivisionId
	) VALUES (
		pos_RowID
		,pos_PositionName
		,CURRENT_TIMESTAMP()
		,pos_CreatedBy
		,pos_OrganizationID
		,pos_LastUpdBy
		# ,pos_ParentPositionID
		,defaultDivisID
	) ON
	DUPLICATE
	KEY
	UPDATE
		PositionName=pos_PositionName
		,LastUpd=CURRENT_TIMESTAMP()
		,LastUpdBy=pos_LastUpdBy
		# ,ParentPositionID=pos_ParentPositionID
		,DivisionId=defaultDivisID;SELECT @@Identity AS Id INTO positID;
		
END IF;

IF IFNULL(positID,0) = 0 AND IFNULL(pos_RowID,0) != 0 THEN
	SET positID = pos_RowID;
ELSEIF IFNULL(positID,0) = 0 THEN
	SELECT RowID FROM `position` pt WHERE pt.PositionName=pos_PositionName AND pt.OrganizationID=pos_OrganizationID LIMIT 1 INTO positID;
END IF;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
