/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `sp_empschedule`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_empschedule`(
    IN `I_OrganizationID` INT(10),
    IN `I_Created` DATETIME,
    IN `I_CreatedBy` INT(10),
    IN `I_Lastupd` DATETIME,
    IN `I_Lastupdby` INT(10),
    IN `I_EmployeeID` INT(10),
    IN `I_LoanNumber` VARCHAR(50),
    IN `I_DedEffectiveDateFrom` DATE,
    IN `I_DedEffectiveDateTo` DATE,
    IN `I_TotalLoanAmount` DECIMAL(10,2),
    IN `I_DeductionSchedule` VARCHAR(50),
    IN `I_TotalBalanceLeft` DECIMAL(10,2),
    IN `I_DeductionAmount` DECIMAL(10,2),
    IN `I_Status` VARCHAR(50),
    IN `I_DeductionPercentage` DECIMAL(10,2),
    IN `I_NoOfPayPeriod` DECIMAL(10,2),
    IN `I_Comments` VARCHAR(2000),
    IN `I_LoanTypeID` INT,
    IN `I_BonusID` INT



)
    DETERMINISTIC
BEGIN

DECLARE MAXLoanNumber VARCHAR(50) DEFAULT 0;

DECLARE strloantype TEXT;

DECLARE empPayFreqID INT(11);

DECLARE paypmonth TEXT;


SELECT PayFrequencyID FROM employee WHERE RowID=I_EmployeeID INTO empPayFreqID;

SELECT MAX(LoanNumber) FROM employeeloanschedule WHERE OrganizationID=I_OrganizationID AND EmployeeID=I_EmployeeID INTO MAXLoanNumber;

SELECT PartNo FROM product WHERE RowID=I_LoanTypeID INTO strloantype;



IF (strloantype IN ('Cash Advance','BIR') OR I_DeductionSchedule = 'End of the month')
    AND empPayFreqID = 4 THEN

    SELECT `Month` FROM payperiod WHERE OrganizationID=I_OrganizationID AND TotalGrossSalary=empPayFreqID AND I_DedEffectiveDateFrom BETWEEN PayFromDate AND PayToDate INTO paypmonth;

    SELECT PayToDate FROM payperiod WHERE OrganizationID=I_OrganizationID AND TotalGrossSalary=empPayFreqID AND `Month`=paypmonth AND `Year`=YEAR(I_DedEffectiveDateFrom) ORDER BY PayFromDate DESC, PayToDate DESC LIMIT 1 INTO I_DedEffectiveDateTo;

END IF;

INSERT INTO employeeloanschedule
(
    OrganizationID,
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    EmployeeID,
    LoanNumber,
    DedEffectiveDateFrom,
    DedEffectiveDateTo,
    TotalLoanAmount,
    DeductionSchedule,
    TotalBalanceLeft,
    DeductionAmount,
    `Status`,
    DeductionPercentage,
    NoOfPayPeriod,
    Comments,
    LoanTypeID,
    LoanPayPeriodLeft,
    BonusID
)
VALUES
(
    I_OrganizationID,
    CURRENT_TIMESTAMP(),
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy,
    I_EmployeeID,
    I_LoanNumber,
    I_DedEffectiveDateFrom,
    PAYTODATE_OF_NoOfPayPeriod(I_DedEffectiveDateFrom, I_NoOfPayPeriod, I_EmployeeID, I_DeductionSchedule),
    I_TotalLoanAmount,
    I_DeductionSchedule,
    I_TotalLoanAmount,
    I_DeductionAmount,
    I_Status,
    I_DeductionPercentage,
    I_NoOfPayPeriod,
    I_Comments,
    I_LoanTypeID,
    I_NoOfPayPeriod,
    I_BonusID
);
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
