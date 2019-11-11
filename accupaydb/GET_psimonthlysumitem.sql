/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_psimonthlysumitem`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_psimonthlysumitem`(`OrganizID` INT, `EmpRowID` INT, `ItemName` TEXT, `PayPMonth` INT, `PayPYear` INT, `EmpPayFrequencyID` INT) RETURNS decimal(11,6)
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(11,6);

DECLARE itemID INT;

SELECT RowID FROM product WHERE PartNo=ItemName AND OrganizationID=OrganizID INTO itemID;

SELECT SUM(PayAmount)
FROM paystubitem
WHERE Undeclared='1'
AND PayStubID IN (
                        SELECT RowID
                        FROM paystub
                        WHERE PayPeriodID IN (
                                                    SELECT RowID
                                                    FROM payperiod
                                                    WHERE `Month`=PayPMonth
                                                    AND `Year`=PayPYear
                                                    AND OrganizationID=OrganizID
                                                    AND TotalGrossSalary=EmpPayFrequencyID)
                        AND OrganizationID=OrganizID
                        AND EmployeeID=EmpRowID
                        )
AND ProductID=itemID
AND OrganizationID=OrganizID
INTO returnvalue;




RETURN IFNULL(returnvalue, 0);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
