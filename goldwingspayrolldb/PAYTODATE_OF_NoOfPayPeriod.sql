/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `PAYTODATE_OF_NoOfPayPeriod`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `PAYTODATE_OF_NoOfPayPeriod`(`EmpLoanEffectiveDateFrom` DATE, `EmpLoanNoOfPayPeriod` INT, `Employee_RowID` INT, `LoanDeductSched` VARCHAR(100)) RETURNS date
    DETERMINISTIC
BEGIN

DECLARE payp_RowID INT(11);

DECLARE ReturnDate DATE;

DECLARE paypFrom_RowID INT(11);

DECLARE startdate_month CHAR(2);

DECLARE startdate_year CHAR(4);

DECLARE OrganizID INT(11);

SELECT OrganizationID FROM employee WHERE RowID=Employee_RowID INTO OrganizID;


SET @i=0;

SELECT RowID,`Month`,`Year` FROM payperiod WHERE EmpLoanEffectiveDateFrom BETWEEN PayFromDate AND PayToDate AND OrganizationID=OrganizID INTO paypFrom_RowID,startdate_month,startdate_year;

IF LoanDeductSched = 'Per pay period' THEN
    SET LoanDeductSched = 'Per pay period';

    SELECT PayToDate FROM (SELECT RowID,PayToDate,(SELECT @i := @i + 1) `Rank` FROM payperiod WHERE RowID >= paypFrom_RowID LIMIT EmpLoanNoOfPayPeriod) AS `DateRank` WHERE IF(EmpLoanNoOfPayPeriod > `DateRank`.`Rank`, `DateRank`.`Rank`=@i, `DateRank`.`Rank`=EmpLoanNoOfPayPeriod) INTO ReturnDate;

ELSEIF LoanDeductSched = 'End of the month' THEN
    SET LoanDeductSched = 'End of the month';

    SELECT PayToDate FROM (SELECT RowID,PayToDate,(SELECT @i := @i + 1) `Rank` FROM payperiod WHERE RowID >= paypFrom_RowID AND `Half`='0' LIMIT EmpLoanNoOfPayPeriod) AS `DateRank` WHERE IF(EmpLoanNoOfPayPeriod > `DateRank`.`Rank`, `DateRank`.`Rank`=@i, `DateRank`.`Rank`=EmpLoanNoOfPayPeriod) INTO ReturnDate;

ELSEIF LoanDeductSched = 'First half' THEN
    SET LoanDeductSched = 'First half';

    SELECT PayToDate FROM (SELECT RowID,PayToDate,(SELECT @i := @i + 1) `Rank` FROM payperiod WHERE RowID >= paypFrom_RowID AND `Half`='1' LIMIT EmpLoanNoOfPayPeriod) AS `DateRank` WHERE IF(EmpLoanNoOfPayPeriod > `DateRank`.`Rank`, `DateRank`.`Rank`=@i, `DateRank`.`Rank`=EmpLoanNoOfPayPeriod) INTO ReturnDate;

END IF;

IF ReturnDate IS NULL THEN

    SELECT RowID FROM payperiod WHERE ADDDATE(EmpLoanEffectiveDateFrom, INTERVAL 1 YEAR) BETWEEN PayFromDate AND PayToDate AND OrganizationID=OrganizID INTO paypFrom_RowID;

    SELECT PayToDate FROM (SELECT RowID,PayToDate,(SELECT @i := @i + 1) `Rank` FROM payperiod WHERE RowID >= paypFrom_RowID LIMIT EmpLoanNoOfPayPeriod) AS `DateRank` WHERE IF(EmpLoanNoOfPayPeriod > `DateRank`.`Rank`, `DateRank`.`Rank`=@i, `DateRank`.`Rank`=EmpLoanNoOfPayPeriod) INTO ReturnDate;

    SET ReturnDate = SUBDATE(ReturnDate, INTERVAL 1 YEAR);

END IF;

RETURN ReturnDate;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
