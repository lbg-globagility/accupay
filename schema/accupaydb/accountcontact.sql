/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `accountcontact` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `AccountID` int(10) DEFAULT NULL,
  `ContactID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_accountcontact_account` (`AccountID`),
  KEY `FK_accountcontact_contact` (`ContactID`),
  CONSTRAINT `FK_accountcontact_account` FOREIGN KEY (`AccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_accountcontact_contact` FOREIGN KEY (`ContactID`) REFERENCES `contact` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='An Account can have multiple Contacts and a Contact can belong to multiple Accounts.  This is the intersection table that defines the Contact and Account';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
