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

-- Dumping structure for function USER_HAS_PRIVILEGE
DROP FUNCTION IF EXISTS `USER_HAS_PRIVILEGE`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `USER_HAS_PRIVILEGE`(`UserRowID` INT, `AccessingOrganizID` INT, `ViewRowID` INT) RETURNS char(1) CHARSET latin1
    DETERMINISTIC
BEGIN

DECLARE returnvalue CHAR(1);


DECLARE canCreate CHAR(1);

DECLARE canRead CHAR(1);

DECLARE canUpd CHAR(1);

DECLARE canDel CHAR(1);


SELECT pv.`Creates`
,pv.`ReadOnly`
,pv.`Updates`
,pv.`Deleting`
FROM position_view pv
INNER JOIN user u ON u.RowID=UserRowID
INNER JOIN position po ON po.RowID=u.PositionID
WHERE pv.PositionID=po.RowID
AND pv.ViewID=ViewRowID
INTO canCreate
      ,canRead
      ,canUpd
      ,canDel;

IF canCreate IS NULL
    AND canRead IS NULL
    AND canUpd IS NULL
    AND canDel IS NULL THEN

    SET returnvalue = '0';

ELSE

    IF canRead = 'Y' THEN

        SET returnvalue = '1';

    ELSE

        IF canCreate = 'Y'
            OR canUpd = 'Y' THEN

            SET returnvalue = '1';

        ELSE

            IF canDel = 'Y' THEN

                SET returnvalue = '1';

            ELSE

                SET returnvalue = '0';

            END IF;

        END IF;

    END IF;

END IF;




RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
