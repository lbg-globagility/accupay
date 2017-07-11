/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_totalmonthTaxableSalary`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_totalmonthTaxableSalary`(`EmployeeRowID` INT, `OrganizRowID` INT, `PayPeriodRowID` INT) RETURNS decimal(11,2)
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(11,2);

DECLARE payp_from DATE;

DECLARE payp_to DATE;

DECLARE themonth TEXT;

DECLARE theyear INT(11);


SELECT pyp.`Month`, pyp.`Year` FROM payperiod pyp WHERE pyp.RowID=PayPeriodRowID INTO themonth, theyear;

SELECT pyp.PayFromDate FROM payperiod pyp WHERE pyp.`Month`=themonth AND pyp.`Year`=theyear AND pyp.OrganizationID=OrganizRowID ORDER BY pyp.PayFromDate, pyp.PayToDate LIMIT 1 INTO payp_from;

SELECT pyp.PayToDate FROM payperiod pyp WHERE pyp.`Month`=themonth AND pyp.`Year`=theyear AND pyp.OrganizationID=OrganizRowID ORDER BY pyp.PayFromDate DESC, pyp.PayToDate DESC LIMIT 1 INTO payp_to;

SELECT SUM(ps.TotalTaxableSalary) FROM paystub ps WHERE ps.EmployeeID=EmployeeRowID AND ps.OrganizationID=OrganizRowID AND (ps.PayFromDate >= payp_from OR ps.PayToDate >= payp_from) AND (ps.PayFromDate <= payp_to OR ps.PayToDate <= payp_to) INTO returnvalue;

RETURN IFNULL(returnvalue,0.00);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
