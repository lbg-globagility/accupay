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

-- Dumping structure for procedure BULK_INSUPD_employeeshift
DROP PROCEDURE IF EXISTS `BULK_INSUPD_employeeshift`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `BULK_INSUPD_employeeshift`(IN `OrganizID` INT, IN `EmpIDList` VARCHAR(2000), IN `ShiftRowID` INT, IN `ShiftDateFrom` DATE, IN `ShiftDateTo` DATE, IN `Is_RestDay` CHAR(1), IN `UserRowID` INT)
    DETERMINISTIC
BEGIN

DECLARE indx INT(11) DEFAULT 0;

DECLARE empcount INT(11);

DECLARE emp_rowid INT(11);

DECLARE date_diffcount INT(11);

SELECT COUNT(e.RowID) FROM employee e WHERE e.OrganizationID=OrganizID AND FIND_IN_SET(e.RowID,EmpIDList) > 0 INTO empcount;
SET date_diffcount = DATEDIFF(ShiftDateTo,ShiftDateFrom);
loop_label:LOOP

    IF indx < empcount THEN

        SELECT e.RowID FROM employee e WHERE e.OrganizationID=OrganizID AND FIND_IN_SET(e.RowID,EmpIDList) > 0 ORDER BY e.RowID LIMIT indx,1 INTO emp_rowid;



        UPDATE employeeshift esh INNER JOIN (SELECT DateValue FROM dates WHERE DateValue BETWEEN SUBDATE(SUBDATE(ShiftDateFrom,INTERVAL 1 DAY),INTERVAL date_diffcount DAY) AND SUBDATE(ShiftDateFrom,INTERVAL 1 DAY) ORDER BY DateValue DESC) d ON d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
        SET esh.EffectiveTo=d.DateValue
        ,esh.LastUpd=CURRENT_TIMESTAMP()
        ,esh.LastUpdBy=UserRowID
        WHERE esh.EmployeeID=emp_rowid
        AND esh.OrganizationID=OrganizID AND esh.EffectiveFrom < esh.EffectiveTo;

        SET @mycurrtimestamp = CURRENT_TIMESTAMP();

        INSERT INTO employeeshift(RowID,OrganizationID,Created,CreatedBy,EmployeeID,ShiftID,EffectiveFrom,EffectiveTo,NightShift,RestDay) VALUES (NULL,OrganizID,@mycurrtimestamp,UserRowID,emp_rowid,ShiftRowID,ShiftDateFrom,ShiftDateTo,'0',Is_RestDay) ON DUPLICATE KEY UPDATE LastUpd=@mycurrtimestamp,LastUpdBy=UserRowID,ShiftID=ShiftRowID,RestDay=Is_RestDay;



        UPDATE employeeshift esh INNER JOIN (SELECT DateValue FROM dates WHERE DateValue BETWEEN ADDDATE(ShiftDateTo,INTERVAL 1 DAY) AND ADDDATE(ADDDATE(ShiftDateTo,INTERVAL 1 DAY),INTERVAL date_diffcount DAY) ORDER BY DateValue) d ON d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
        SET esh.EffectiveFrom=d.DateValue
        ,esh.LastUpd=CURRENT_TIMESTAMP()
        ,esh.LastUpdBy=UserRowID
        WHERE esh.EmployeeID=emp_rowid
        AND esh.OrganizationID=OrganizID AND esh.EffectiveFrom < esh.EffectiveTo;
















        SET indx = indx + 1;

        ITERATE loop_label;

    ELSE

        LEAVE loop_label;

    END IF;

END LOOP;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
