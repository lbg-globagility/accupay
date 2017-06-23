-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure goldwingspayrolldb.RPT_AgencyFee
DROP PROCEDURE IF EXISTS `RPT_AgencyFee`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `RPT_AgencyFee`(IN `OrganizID` INT, IN `FromPayDate` DATE, IN `ToPayDate` DATE)
    DETERMINISTIC
BEGIN

DECLARE OT_RowID INT(11);

DECLARE Holiday_ID INT(11);

DECLARE Ecola_ID INT(11);


SET @anyint = 0.0;

SELECT p1.RowID
,p2.RowID
,p3.RowID
FROM product p1
INNER JOIN product p2 ON p2.PartNo='Holiday pay' AND p2.OrganizationID=OrganizID
INNER JOIN product p3 ON p3.PartNo='Ecola' AND p3.OrganizationID=OrganizID
WHERE p1.PartNo='Overtime'
AND p1.OrganizationID=OrganizID
INTO OT_RowID
		,Holiday_ID
		,Ecola_ID;

SELECT d.Name
, e.EmployeeID
, GET_employeerateperday(e.RowID,OrganizID,ToPayDate) AS DailyRate
, CONCAT(e.LastName,',',e.FirstName, IF(e.MiddleName='','',','),INITIALS(e.MiddleName,'. ','1')) 'Fullname'
, SUM(agf.DailyFee)
, (SELECT @anyint := COMPUTE_TimeDifference(sh.TimeFrom,sh.TimeTo)) AS ShiftTimeDiff
, (SELECT IF(@anyint IN (4,5), @anyint, (SELECT @anyint := @anyint - 1))) AS ShiftTimeDiffLessBreak
, SUM((SELECT ete.RegularHoursWorked / @anyint)) AS RegularDays
, SUM(ete.RegularHoursAmount) AS RegularHoursAmount
, SUM(ete.OvertimeHoursWorked) AS OvertimeHoursWorked
, psi.PayAmount AS OvertimeHoursAmount
, psi2.PayAmount AS HolidayPay
, psi3.PayAmount AS EcolaAmount
FROM agencyfee agf
INNER JOIN `division` d ON d.RowID=agf.DivisionID
INNER JOIN employee e ON e.RowID=agf.EmployeeID
INNER JOIN employeetimeentry ete ON ete.RowID=agf.TimeEntryID
INNER JOIN employeeshift esh ON esh.RowID=ete.EmployeeShiftID AND esh.RestDay='0'
INNER JOIN shift sh ON sh.RowID=esh.ShiftID

INNER JOIN (SELECT RowID,EmployeeID FROM paystub WHERE OrganizationID=OrganizID AND (PayFromDate >= FromPayDate OR PayToDate >= FromPayDate) AND (PayFromDate <= ToPayDate OR PayToDate <= ToPayDate)) ps ON ps.EmployeeID=agf.EmployeeID

INNER JOIN paystubitem psi ON psi.ProductID=OT_RowID AND psi.OrganizationID=OrganizID AND psi.PayStubID IN (ps.RowID)

INNER JOIN paystubitem psi2 ON psi2.ProductID=Holiday_ID AND psi2.OrganizationID=OrganizID AND psi2.PayStubID IN (ps.RowID)

INNER JOIN paystubitem psi3 ON psi3.ProductID=Ecola_ID AND psi3.OrganizationID=OrganizID AND psi3.PayStubID IN (ps.RowID)

WHERE agf.OrganizationID=OrganizID
AND agf.TimeEntryDate BETWEEN FromPayDate AND ToPayDate
GROUP BY agf.EmployeeID;





END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
