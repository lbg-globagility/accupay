/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `accountfinancialinst` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `AccountID` int(10) NOT NULL,
  `FinancialInstID` int(10) NOT NULL,
  `CheckingAccountNo` int(20) DEFAULT NULL,
  `SavingsAccountNo` int(20) DEFAULT NULL COMMENT 'if finanial institution is bank',
  `CardType` varchar(50) DEFAULT NULL COMMENT 'Visa,Mastercard,Amex (LOV Type="CREDITCARD_TYPE")',
  `CardNo` int(16) DEFAULT NULL,
  `ExpirationDateMonth` int(2) DEFAULT NULL COMMENT 'LOV Type="MONTH_NUM"',
  `ExpirationDateYear` int(4) DEFAULT NULL COMMENT 'LOV Type="YEAR"',
  `Amount` decimal(10,2) DEFAULT NULL,
  `CVVNo` int(4) DEFAULT NULL,
  `Primary` varchar(1) DEFAULT NULL COMMENT 'Checkbox to mark if bank/credit card is primary',
  PRIMARY KEY (`RowID`),
  KEY `FK_AccountFinancialInst_account` (`AccountID`),
  KEY `FK_accountfinancialinst_financialinstitution` (`FinancialInstID`),
  CONSTRAINT `FK_AccountFinancialInst_account` FOREIGN KEY (`AccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_accountfinancialinst_financialinstitution` FOREIGN KEY (`FinancialInstID`) REFERENCES `financialinstitution` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Account Financial Institution Link (M:M) relationship.  an account can have multiple banks/creditcards.  a bank/creditcard can belong to one or more accounts.  This table defines the relationship';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
