/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `GETVIEW_previousemployeetimeentry`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `GETVIEW_previousemployeetimeentry`(IN `OrganizID` INT, IN `prev_payperiodID` INT, IN `WeeklySSSSchedPayPeriodID` INT)
    DETERMINISTIC
BEGIN

DECLARE payfreqID INT(11) DEFAULT NULL;

DECLARE payperiodcount INT(11);

DECLARE isSSSContribSched CHAR(1);

DECLARE customdatefrom DATE;

DECLARE paydate_to DATE;

DECLARE paydate_from DATE;

DECLARE this_month VARCHAR(2);

DECLARE this_year INT(11);

DECLARE prev_payperiodRowID INT(11);

DECLARE payperiod_half CHAR(1);

SELECT `Month`, `Year` FROM payperiod WHERE RowID=WeeklySSSSchedPayPeriodID INTO this_month, this_year;

SELECT RowID,`Half`,TotalGrossSalary FROM payperiod WHERE `Half`='1' AND RowID=prev_payperiodID AND `Month`=this_month AND `Year`=this_year AND OrganizationID=OrganizID ORDER BY PayFromDate,PayToDate LIMIT 1 INTO prev_payperiodRowID,payperiod_half,payfreqID;





SELECT TotalGrossSalary,PayToDate,PayFromDate FROM payperiod WHERE RowID=prev_payperiodRowID INTO payfreqID,paydate_to,paydate_from;

SELECT SSSContribSched FROM payperiod WHERE RowID=WeeklySSSSchedPayPeriodID INTO isSSSContribSched;



IF payfreqID = 1 THEN

    SELECT
    ete.*
    ,IFNULL(emt.emtAmount,0) AS emtAmount
    FROM employeetimeentry ete
    INNER JOIN payperiod pyp ON pyp.RowID=prev_payperiodID
    INNER JOIN employee e ON e.RowID=ete.EmployeeID
    INNER JOIN `position` p ON p.RowID=e.PositionID
    INNER JOIN `division` d ON d.RowID=p.DivisionId
    INNER JOIN (
                    SELECT et.RowID
                    ,et.EmployeeID AS eRowID
                    ,SUM(et.RegularHoursAmount / IF(e.CalcHoliday='1' OR e.CalcSpecialHoliday='1', pr.`PayRate`, 1)) AS emtAmount
                    ,'End of the month' AS DeductSched
                    FROM employeetimeentry et
                    INNER JOIN employee e ON e.RowID=et.EmployeeID
                    INNER JOIN payrate pr ON pr.RowID=et.PayRateID
                    INNER JOIN payperiod pp ON pp.RowID=WeeklySSSSchedPayPeriodID AND pp.Half='0'
                    INNER JOIN payperiod ppp ON ppp.OrganizationID=pp.OrganizationID AND ppp.`Month`=pp.`Month` AND ppp.`Year`=pp.`Year` AND ppp.TotalGrossSalary=pp.TotalGrossSalary AND ppp.Half='1'
                    WHERE et.OrganizationID=OrganizID
                    AND et.`Date` BETWEEN ppp.PayFromDate AND ppp.PayToDate
                UNION
                    SELECT et.RowID
                    ,et.EmployeeID AS eRowID
                    ,SUM(et.RegularHoursAmount / IF(e.CalcHoliday='1' OR e.CalcSpecialHoliday='1', pr.`PayRate`, 1)) AS emtAmount
                    ,'First half' AS DeductSched
                    FROM employeetimeentry et
                    INNER JOIN employee e ON e.RowID=et.EmployeeID
                    INNER JOIN payrate pr ON pr.RowID=et.PayRateID
                    INNER JOIN payperiod pp ON pp.RowID=WeeklySSSSchedPayPeriodID AND pp.Half='1'
                    INNER JOIN payperiod ppp ON ppp.OrganizationID=pp.OrganizationID AND ppp.`Month`=pp.`Month` AND ppp.`Year`=pp.`Year` AND ppp.TotalGrossSalary=pp.TotalGrossSalary AND ppp.Half='0'
                    WHERE et.OrganizationID=OrganizID
                    AND et.`Date` BETWEEN ppp.PayFromDate AND ppp.PayToDate
                UNION
                    SELECT et.RowID
                    ,et.EmployeeID AS eRowID
                    ,SUM(et.RegularHoursAmount / IF(e.CalcHoliday='1' OR e.CalcSpecialHoliday='1', pr.`PayRate`, 1)) AS emtAmount
                    ,'Per pay period' AS DeductSched
                    FROM employeetimeentry et
                    INNER JOIN employee e ON e.RowID=et.EmployeeID
                    INNER JOIN payrate pr ON pr.RowID=et.PayRateID
                    INNER JOIN payperiod pp ON pp.RowID=WeeklySSSSchedPayPeriodID
                    WHERE et.OrganizationID=OrganizID
                    AND et.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate
                GROUP BY et.EmployeeID
    ) emt ON emt.eRowID=ete.EmployeeID AND BINARY d.WTaxDeductSched=BINARY emt.DeductSched
    WHERE ete.`Date` BETWEEN pyp.PayFromDate AND pyp.PayToDate
    AND ete.OrganizationID=OrganizID
    AND e.PayFrequencyID=payfreqID
    GROUP BY ete.EmployeeID;

ELSEIF payfreqID = 4 THEN

    IF isSSSContribSched = '1' THEN

        SET customdatefrom = SUBDATE(paydate_to, INTERVAL 3 WEEK);

        SET customdatefrom = ADDDATE(customdatefrom, INTERVAL 1 DAY);

        SELECT
        ete.*
        FROM employeetimeentry ete
        INNER JOIN employee e ON e.RowID=ete.EmployeeID
        WHERE ete.OrganizationID=OrganizID
        AND e.PayFrequencyID=payfreqID
        AND ete.`Date` BETWEEN customdatefrom AND paydate_to;

    ELSE

        SELECT
        ete.*
        FROM employeetimeentry ete
        INNER JOIN payperiod pyp ON pyp.RowID=prev_payperiodID
        INNER JOIN employee e ON e.RowID=ete.EmployeeID
        WHERE ete.OrganizationID=OrganizID
        AND e.PayFrequencyID=payfreqID
        AND ete.Date IN  (SELECT
                                ADDDATE(pyp.PayFromDate, INTERVAL g.n DAY) AS WkDate
                                FROM generator_16 g
                                INNER JOIN payperiod pp ON pp.RowID=prev_payperiodID
                                INNER JOIN payperiod pyp ON pyp.OrganizationID=pp.OrganizationID AND CONCAT(pyp.`Year`,pyp.`Month`)=CONCAT(pp.`Year`,pp.`Month`) AND pyp.TotalGrossSalary=pp.TotalGrossSalary AND pyp.PayFromDate < pp.PayFromDate AND pyp.PayToDate < pp.PayToDate
                                WHERE g.n <= DATEDIFF(pyp.PayToDate,pyp.PayFromDate)
                                ORDER BY ADDDATE(pyp.PayFromDate, INTERVAL g.n DAY));

    END IF;

ELSE

    SELECT
    ete.*
    ,0 AS emtAmount
    FROM employeetimeentry ete
    WHERE ete.OrganizationID IS NULL;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
