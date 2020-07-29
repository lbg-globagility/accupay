/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paystubemailhistory` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `SentBy` int(10) NOT NULL,
  `PaystubID` int(10) NOT NULL,
  `ReferenceNumber` varchar(50) NOT NULL COMMENT 'Used as file name of the PDF',
  `SentDateTime` datetime NOT NULL,
  `EmailAddress` varchar(100) NOT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_paystubemailhistory_user_SentBy` (`SentBy`),
  KEY `FK_paystubemailhistory_paystub_PaystubID` (`PaystubID`),
  CONSTRAINT `FK_paystubemailhistory_paystub_PaystubID` FOREIGN KEY (`PaystubID`) REFERENCES `paystub` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_paystubemailhistory_user_SentBy` FOREIGN KEY (`SentBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
