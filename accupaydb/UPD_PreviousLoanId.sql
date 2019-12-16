/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `UPD_PreviousLoanId`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `UPD_PreviousLoanId`(
	IN `organizationId` INT




)
BEGIN

DECLARE loanCount
        , indexNumber
		  , slpRowID
		  , loanId
		  , slpLoanId
		  , previousLoanId INT(11);

DECLARE ids TEXT;

SET loanId = NULL;
SET indexNumber = 0;

SELECT COUNT(slp.RowID)
, GROUP_CONCAT(slp.RowID)
FROM scheduledloansperpayperiod slp
WHERE slp.OrganizationID = organizationId
INTO loanCount
     , ids;

WHILE indexNumber < loanCount DO

	SELECT slp.RowID
	, slp.EmployeeLoanRecordID
	FROM scheduledloansperpayperiod slp
	INNER JOIN payperiod pp ON pp.RowID=slp.PayPeriodID
	WHERE slp.OrganizationID = organizationId
	AND FIND_IN_SET(slp.RowID, ids) > 0
	ORDER BY slp.EmployeeID, slp.EmployeeLoanRecordID, pp.`Year`, pp.OrdinalValue
	LIMIT indexNumber, 1
	INTO slpRowID
	     , slpLoanId;

	IF loanId = slpLoanId THEN
		UPDATE scheduledloansperpayperiod
		SET PreviousId = previousLoanId
		WHERE RowID = slpRowID;
		
		SET previousLoanId = slpRowID;
		
	ELSE
		SET loanId = slpLoanId;
		
		UPDATE scheduledloansperpayperiod
		SET PreviousId = NULL
		WHERE RowID = slpRowID;
		
		SET previousLoanId = slpRowID;
		
	END IF;
	
	SET indexNumber = indexNumber + 1;
END WHILE;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
