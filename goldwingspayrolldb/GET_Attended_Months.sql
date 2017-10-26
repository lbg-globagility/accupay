-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure cinema2k.GET_Attended_Months
DROP PROCEDURE IF EXISTS `GET_Attended_Months`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `GET_Attended_Months`(IN `param_OrganizationID` INT, IN `param_year` DATE)
    DETERMINISTIC
BEGIN

DECLARE payfreqID INT(11);

DECLARE FirstDateOfYear DATE;

DECLARE LastDateOfYear DATE;

SELECT PayFrequencyID FROM employee WHERE OrganizationID=param_OrganizationID LIMIT 1 INTO payfreqID;

    IF payfreqID = 0 THEN

SELECT MAKEDATE(YEAR(param_year),1) 'FirstDateOfYear',LAST_DAY(ADDDATE(MAKEDATE(YEAR(param_year),1), INTERVAL 11 MONTH)) 'LastDateOfYear' INTO FirstDateOfYear,LastDateOfYear;

        SELECT
        e.RowID 'EmployeeRowID'
        ,IF(e.StartDate > FirstDateOfYear, (12 - MONTH(e.StartDate)) * 1, 12 * 1) 'CompleteMonthAttended'
        ,IF(e.EmployeeType='Fixed', esal.Salary, IF(e.EmployeeType='Daily', esal.BasicDailyPay, esal.BasicHourlyPay)) 'multiplicand'
        ,IF(e.StartDate > FirstDateOfYear, IF(e.EmployeeType='Fixed', IFNULL(SUM(pstb.TotalGrossSalary),0), 0), 0) 'firstmonthpay'
        ,IF(e.EmployeeType='Fixed', 0, COUNT(etime.RowID)) 'HourlyDailyDaysAttended'
        ,e.EmployeeType
        ,IF(e.EmployeeType='Hourly', IFNULL(((TIME_TO_SEC((SELECT SUBSTRING_INDEX(TIMEDIFF(TimeFrom,IF(TimeFrom>TimeTo,ADDTIME(TimeTo,'24:00:00'),TimeTo)),'-',-1) FROM shift WHERE RowID=(SELECT ShiftID FROM employeeshift WHERE EmployeeID=e.RowID ORDER BY EffectiveTo DESC LIMIT 1))) / 60) / 60),0), 0) 'ShiftHoursCount'
        FROM employee e
        LEFT JOIN employeesalary esal ON esal.EmployeeID=e.RowID AND LastDateOfYear BETWEEN esal.EffectiveDateFrom AND IFNULL(esal.EffectiveDateTo,LastDateOfYear)
        LEFT JOIN paystub pstb ON pstb.EmployeeID=e.RowID AND (MONTH(pstb.PayToDate)=MONTH(e.StartDate) AND YEAR(pstb.PayToDate)=YEAR(e.StartDate))
        LEFT JOIN employeetimeentry etime ON etime.EmployeeID=e.RowID AND IF(etime.RegularHoursWorked!=0, etime.RegularHoursWorked, etime.NightDifferentialHours)=etime.UndertimeHours AND YEAR(etime.Date)=YEAR(LastDateOfYear)
        WHERE e.OrganizationID=param_OrganizationID
        AND e.EmploymentStatus NOT IN ('Resigned','Terminated')
        GROUP BY e.RowID;

    ELSE

        SELECT 0 `CompleteMonthAttended`, 0 `multiplicand`, 0 `firstmonthpay`, 0 `HourlyDailyDaysAttended`, 'Daily' `EmployeeType`, 0 `ShiftHoursCount`;

    END IF;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
