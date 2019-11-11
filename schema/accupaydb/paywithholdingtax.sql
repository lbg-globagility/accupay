/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paywithholdingtax` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) DEFAULT NULL,
  `PayFrequencyID` int(10) NOT NULL,
  `FilingStatusID` int(10) NOT NULL,
  `EffectiveDateFrom` date DEFAULT NULL,
  `EffectiveDateTo` date DEFAULT NULL,
  `ExemptionAmount` decimal(10,2) DEFAULT NULL,
  `ExemptionInExcessAmount` decimal(10,2) DEFAULT NULL,
  `TaxableIncomeFromAmount` decimal(10,2) DEFAULT NULL,
  `TaxableIncomeToAmount` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 6` (`FilingStatusID`,`TaxableIncomeToAmount`,`TaxableIncomeFromAmount`,`PayFrequencyID`),
  KEY `FK_PayWithholdingTax_user` (`CreatedBy`),
  KEY `FK_PayWithholdingTax_user_2` (`LastUpdBy`),
  KEY `FK_paywithholdingtax_payfrequency` (`PayFrequencyID`),
  CONSTRAINT `FK_PayWithholdingTax_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_PayWithholdingTax_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_paywithholdingtax_filingstatus` FOREIGN KEY (`FilingStatusID`) REFERENCES `filingstatus` (`RowID`),
  CONSTRAINT `FK_paywithholdingtax_payfrequency` FOREIGN KEY (`PayFrequencyID`) REFERENCES `payfrequency` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=321 DEFAULT CHARSET=latin1 COMMENT='BIR Withholding Tax Table';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
