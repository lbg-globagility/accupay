/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paystubemail` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) NOT NULL,
  `PaystubID` int(10) NOT NULL,
  `ProcessingStarted` datetime DEFAULT NULL,
  `ErrorLogMessage` varchar(200) DEFAULT NULL,
  `Status` enum('WAITING','PROCESSING','FAILED') NOT NULL DEFAULT 'WAITING',
  PRIMARY KEY (`RowID`),
  KEY `FK_paystubemail_user_CreatedBy` (`CreatedBy`),
  KEY `FK_paystubemail_paystub_PaystubId` (`PaystubID`),
  CONSTRAINT `FK_paystubemail_paystub_PaystubId` FOREIGN KEY (`PaystubID`) REFERENCES `paystub` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_paystubemail_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='WAITING, PROCESSING, FAILED\r\nReferenceNumber = Filename of the PDF';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
