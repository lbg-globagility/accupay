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

-- Dumping structure for function INSUPD_employeetimeentrydetails
DROP FUNCTION IF EXISTS `INSUPD_employeetimeentrydetails`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeetimeentrydetails`(
	`etentd_RowID` INT,
	`etentd_OrganizationID` INT,
	`etentd_CreatedBy` INT,
	`etentd_Created` DATETIME,
	`etentd_LastUpdBy` INT,
	`etentd_EmployeeID` VARCHAR(50),
	`etentd_TimeIn` TIME,
	`etentd_TimeOut` TIME,
	`etentd_Date` DATE,
	`etentd_TimeScheduleType` VARCHAR(50),
	`etentd_TimeEntryStatus` VARCHAR(50),
	`EditAsUnique` CHAR(1),
	`Branch_Code` VARCHAR(150),
	`DateTimeLogIn` VARCHAR(150),
	`DateTimeLogOut` VARCHAR(150)




) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE etentdID INT(11) DEFAULT -1;

DECLARE branch_rowid INT(11);

SET EditAsUnique = 0;

















IF IFNULL(Branch_Code,'') != '' THEN
    INSERT INTO branch(
        OrganizationID,
        Created,
        CreatedBy,
        BranchCode
    )
    VALUES (
        etentd_OrganizationID,
        CURRENT_TIMESTAMP(),
        etentd_CreatedBy,
        Branch_Code
    )
    ON DUPLICATE KEY
    UPDATE
        LastUpd = CURRENT_TIMESTAMP();
END IF;

SELECT RowID
FROM branch
WHERE OrganizationID = etentd_OrganizationID AND
    BranchCode = IFNULL(Branch_Code,'')
LIMIT 1
INTO branch_rowid;

IF EditAsUnique = '1' AND EXISTS(
        SELECT RowID
        FROM employee
        WHERE OrganizationID = etentd_OrganizationID AND
            EmployeeID = etentd_EmployeeID
        LIMIT 1
    ) = '1' THEN

    SET @noop = TRUE;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

    
    

ELSE
    IF EXISTS(
        SELECT RowID
        FROM employee
        WHERE OrganizationID = etentd_OrganizationID AND
            EmployeeID = etentd_EmployeeID
        LIMIT 1
    ) = '1' THEN

    INSERT INTO employeetimeentrydetails(
        RowID,
        OrganizationID,
        Created,
        CreatedBy,
        EmployeeID,
        TimeIn,
        TimeOut,
        `Date`,
        TimeScheduleType,
        TimeEntryStatus,
        ChargeToDivisionID,
        TimeStampIn,
        TimeStampOut
    )
        SELECT
            etentd_RowID,
            etentd_OrganizationID,
            etentd_Created,
            etentd_CreatedBy,
            (
                SELECT RowID
                FROM employee
                WHERE OrganizationID = etentd_OrganizationID AND
                    EmployeeID = etentd_EmployeeID
                LIMIT 1
            ),
            etentd_TimeIn,
            etentd_TimeOut,
            etentd_Date,
            etentd_TimeScheduleType,
            IFNULL(
                etentd_TimeEntryStatus,
                IF(
                    IFNULL(etentd_TimeIn, '') = '',
                    'missing clock in',
                    IF(
                        IFNULL(etentd_TimeOut, '') = '',
                        'missing clock out',
                        ''
                    )
                )
            ),
            branch_rowid,
            TIMESTAMP(ADDDATE(DateTimeLogIn, INTERVAL 0 SECOND)),
            TIMESTAMP(ADDDATE(DateTimeLogOut, INTERVAL 0 SECOND))
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    ON DUPLICATE KEY
    UPDATE
        LastUpd = CURRENT_TIMESTAMP(),
        LastUpdBy = etentd_LastUpdBy,
        TimeIn = etentd_TimeIn,
        TimeOut = etentd_TimeOut,
        `Date` = etentd_Date,
        TimeScheduleType = etentd_TimeScheduleType,
        ChargeToDivisionID = branch_rowid,
        TimeStampIn = TIMESTAMP(ADDDATE(DateTimeLogIn, INTERVAL 0 SECOND)),
        TimeStampOut = TIMESTAMP(ADDDATE(DateTimeLogOut, INTERVAL 0 SECOND));

    SELECT @@Identity AS id
    INTO etentdID;

    END IF;
END IF;

RETURN etentdID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
