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

-- Dumping structure for function UserAuthentication
DROP FUNCTION IF EXISTS `UserAuthentication`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `UserAuthentication`(`user_name` VARCHAR(90), `pass_word` VARCHAR(90), `organizid` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvaue INT(11) DEFAULT 0;
DECLARE _count TINYINT;
DECLARE _haspriv CHAR(1);



SET @_allow = 'N';

SELECT u.RowID , pv.AllowedToAccess, COUNT(pv.RowID)
FROM `user` u INNER JOIN `position` po ON po.RowID=u.PositionID INNER JOIN `position` p ON p.PositionName=po.PositionName AND

p.OrganizationID=organizid

INNER JOIN position_view pv ON pv.PositionID=p.RowID AND pv.OrganizationID=p.OrganizationID
WHERE
u.UserID=user_name AND u.`Password`=pass_word AND u.`Status`='Active'
GROUP BY u.RowID, pv.ReadOnly

HAVING pv.AllowedToAccess = 'Y'
INTO returnvaue, _haspriv, _count;

IF returnvaue IS NULL THEN
    SET returnvaue = 0;
END IF;

SET @appmodule_count = (SELECT COUNT(DISTINCT v.ViewName) FROM `view` v);

IF returnvaue > 0 AND _haspriv = 'Y' AND _count >= @appmodule_count THEN
    UPDATE `user` SET
    InSession=1
    ,LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=returnvaue
    WHERE RowID=returnvaue;
ELSE
    SET returnvaue = 0;
END IF;

RETURN returnvaue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
