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

-- Dumping structure for view cinema2k.vw_disciplinarymemouserdetails
DROP VIEW IF EXISTS `vw_disciplinarymemouserdetails`;
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `vw_disciplinarymemouserdetails`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `vw_disciplinarymemouserdetails` AS SELECT
    U.RowID AS 'UserID',
    U.FirstName,
    U.MiddleName,
    U.LastName,
    TRIM(O.Name) AS 'CompanyName',
    O.Image,
    P.PositionName,
    CONCAT
    (
        IF(U.FirstName IS NULL OR TRIM(U.FirstName) = '', '', CONCAT
            (
             UPPER(SUBSTRING(TRIM(U.FirstName), 1, 1)),
             LOWER(SUBSTRING(TRIM(U.FirstName), 2)),
              ' ' )
            ),
        IF(U.MiddleName IS NULL OR TRIM(U.MiddleName) = '', '', CONCAT
            (
                UPPER(SUBSTRING(TRIM(U.MiddleName), 1, 1)),
                 '. ')
            ),
        IF(U.LastName IS NULL OR TRIM(U.LastName) = '', '', CONCAT
            (
             UPPER(SUBSTRING(TRIM(U.LastName), 1, 1)),
             LOWER(SUBSTRING(TRIM(U.LastName), 2)),
              ' ' )
            )
    ) AS 'FullName'
FROM
    user AS U
LEFT JOIN
    organization AS O
ON
    U.OrganizationID = O.RowID
LEFT JOIN
    position AS P
ON
    U.PositionID = P.RowID
LIMIT 1 ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
