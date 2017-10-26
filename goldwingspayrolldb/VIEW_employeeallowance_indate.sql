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

-- Dumping structure for procedure VIEW_employeeallowance_indate
DROP PROCEDURE IF EXISTS `VIEW_employeeallowance_indate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeallowance_indate`(IN `eallow_EmployeeID` INT, IN `eallow_OrganizationID` INT, IN `effectivedatefrom` DATE, IN `effectivedateto` DATE, IN `numweekdays` INT)
    DETERMINISTIC
BEGIN

DECLARE numofdaypresent INT(11);

DECLARE emppaytype VARCHAR(50);

DECLARE hourspresent DECIMAL(10,2);

DECLARE shifthoursworked DECIMAL(10,2);

DECLARE shifthoursID INT(11);

DECLARE orgpayfreqID INT(11);

DECLARE thislastdate DATE;

DECLARE minpresentdate DATE;

DECLARE maxpresentdate DATE;

DECLARE timelogcount INT(11);

DECLARE effectivallowance VARCHAR(2000);


SET thislastdate = LAST_DAY(effectivedateto);

SELECT COUNT(RowID) FROM employeetimeentrydetails WHERE OrganizationID=eallow_OrganizationID AND EmployeeID=eallow_EmployeeID AND Date BETWEEN effectivedatefrom AND effectivedateto INTO numofdaypresent;

SELECT EmployeeType FROM employee WHERE RowID=eallow_EmployeeID INTO emppaytype;

SELECT PayFrequencyID FROM organization WHERE RowID=(SELECT OrganizationID FROM employee WHERE RowID=eallow_EmployeeID) INTO orgpayfreqID;




        SELECT SUM((TIME_TO_SEC(TIMEDIFF(TimeOut,TimeIn)) / 60) / 60) FROM employeetimeentrydetails WHERE OrganizationID=eallow_OrganizationID AND EmployeeID=eallow_EmployeeID AND Date BETWEEN effectivedatefrom AND effectivedateto INTO hourspresent;

        SET hourspresent = IFNULL(hourspresent,0);

        SELECT ShiftID FROM employeeshift WHERE EmployeeID=eallow_EmployeeID AND OrganizationID=eallow_OrganizationID AND effectivedateto BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) AND DATEDIFF(effectivedateto,EffectiveFrom) >= 0 AND COALESCE(RestDay,0)=0 ORDER BY DATEDIFF(DATE_FORMAT(effectivedateto,'%Y-%m-%d'),EffectiveFrom) LIMIT 1 INTO shifthoursID;

        SELECT ((TIME_TO_SEC(TIMEDIFF(IF(TimeFrom>TimeTo,ADDTIME(TimeTo,'24:00:00'),TimeTo),TimeFrom)) / 60) / 60) FROM shift WHERE RowID=shifthoursID INTO shifthoursworked;

        SET shifthoursworked = IF(IFNULL(shifthoursworked,0) > 8, (IFNULL(shifthoursworked,0) - 1), IFNULL(shifthoursworked,0));




IF orgpayfreqID = 1 THEN


    IF emppaytype = 'Fixed' THEN

        IF thislastdate = effectivedateto THEN

            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,eall.AllowanceAmount * numweekdays 'AllowanceAmount'
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='Daily'
            AND eall.OrganizationID=eallow_OrganizationID
            AND IF(EffectiveStartDate > effectivedatefrom AND EffectiveEndDate > effectivedateto
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate < effectivedatefrom AND EffectiveEndDate < effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate <= effectivedatefrom AND EffectiveEndDate >= effectivedateto
            , effectivedateto BETWEEN EffectiveStartDate AND EffectiveEndDate
            , IF(EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveEndDate IS NULL
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            )))))
            UNION
            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,eall.AllowanceAmount
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='Monthly'
            AND eall.OrganizationID=eallow_OrganizationID
            AND IF(EffectiveStartDate > effectivedatefrom AND EffectiveEndDate > effectivedateto
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate < effectivedatefrom AND EffectiveEndDate < effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate <= effectivedatefrom AND EffectiveEndDate >= effectivedateto
            , effectivedateto BETWEEN EffectiveStartDate AND EffectiveEndDate
            , IF(EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveEndDate IS NULL
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            )))))
            UNION
            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,eall.AllowanceAmount
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='One time'
            AND eall.OrganizationID=eallow_OrganizationID
            AND EffectiveStartDate
            BETWEEN effectivedatefrom
            AND effectivedateto
            UNION
            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,eall.AllowanceAmount * numweekdays 'AllowanceAmount'
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='Semi-monthly'
            AND eall.OrganizationID=eallow_OrganizationID
            AND IF(EffectiveStartDate > effectivedatefrom AND EffectiveEndDate > effectivedateto
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate < effectivedatefrom AND EffectiveEndDate < effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate <= effectivedatefrom AND EffectiveEndDate >= effectivedateto
            , effectivedateto BETWEEN EffectiveStartDate AND EffectiveEndDate
            , IF(EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveEndDate IS NULL
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            )))))
            ORDER BY RowID;

        ELSE

            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,eall.AllowanceAmount * numweekdays 'AllowanceAmount'
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='Daily'
            AND eall.OrganizationID=eallow_OrganizationID
            AND IF(EffectiveStartDate > effectivedatefrom AND EffectiveEndDate > effectivedateto
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate < effectivedatefrom AND EffectiveEndDate < effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate <= effectivedatefrom AND EffectiveEndDate >= effectivedateto
            , effectivedateto BETWEEN EffectiveStartDate AND EffectiveEndDate
            , IF(EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveEndDate IS NULL
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            )))))
            UNION
            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,eall.AllowanceAmount
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='One time'
            AND eall.OrganizationID=eallow_OrganizationID
            AND EffectiveStartDate
            BETWEEN effectivedatefrom
            AND effectivedateto
            UNION
            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,eall.AllowanceAmount * numweekdays 'AllowanceAmount'
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='Semi-monthly'
            AND eall.OrganizationID=eallow_OrganizationID
            AND IF(EffectiveStartDate > effectivedatefrom AND EffectiveEndDate > effectivedateto
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate < effectivedatefrom AND EffectiveEndDate < effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate <= effectivedatefrom AND EffectiveEndDate >= effectivedateto
            , effectivedateto BETWEEN EffectiveStartDate AND EffectiveEndDate
            , IF(EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveEndDate IS NULL
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            )))))
            ORDER BY RowID;

        END IF;




    ELSE




        IF thislastdate = effectivedateto THEN

            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,FORMAT((eall.AllowanceAmount / shifthoursworked) * GET_employeehoursworked_indate(eallow_EmployeeID,IF(eall.EffectiveStartDate > effectivedatefrom, eall.EffectiveStartDate, effectivedatefrom),IF(eall.EffectiveEndDate > effectivedateto, effectivedateto, eall.EffectiveEndDate),eallow_OrganizationID),2) 'AllowanceAmount'
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='Daily'
            AND eall.OrganizationID=eallow_OrganizationID
            AND IF(EffectiveStartDate > effectivedatefrom AND EffectiveEndDate > effectivedateto
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate < effectivedatefrom AND EffectiveEndDate < effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate <= effectivedatefrom AND EffectiveEndDate >= effectivedateto
            , effectivedateto BETWEEN EffectiveStartDate AND EffectiveEndDate
            , IF(EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveEndDate IS NULL
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            )))))
            UNION
            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,eall.AllowanceAmount
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='Monthly'
            AND eall.OrganizationID=eallow_OrganizationID
            AND IF(EffectiveStartDate > effectivedatefrom AND EffectiveEndDate > effectivedateto
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate < effectivedatefrom AND EffectiveEndDate < effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate <= effectivedatefrom AND EffectiveEndDate >= effectivedateto
            , effectivedateto BETWEEN EffectiveStartDate AND EffectiveEndDate
            , IF(EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveEndDate IS NULL
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            )))))
            UNION
            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,eall.AllowanceAmount
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='One time'
            AND eall.OrganizationID=eallow_OrganizationID
            AND EffectiveStartDate
            BETWEEN effectivedatefrom
            AND effectivedateto
            UNION
            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,FORMAT(((eall.AllowanceAmount / numweekdays) / shifthoursworked) * GET_employeehoursworked_indate(eallow_EmployeeID,IF(eall.EffectiveStartDate > effectivedatefrom, eall.EffectiveStartDate, effectivedatefrom),IF(eall.EffectiveEndDate > effectivedateto, effectivedateto, eall.EffectiveEndDate),eallow_OrganizationID),2) 'AllowanceAmount'
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='Semi-monthly'
            AND eall.OrganizationID=eallow_OrganizationID
            AND IF(EffectiveStartDate > effectivedatefrom AND EffectiveEndDate > effectivedateto
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate < effectivedatefrom AND EffectiveEndDate < effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate <= effectivedatefrom AND EffectiveEndDate >= effectivedateto
            , effectivedateto BETWEEN EffectiveStartDate AND EffectiveEndDate
            , IF(EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveEndDate IS NULL
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            )))))
            ORDER BY RowID;



        ELSE





            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,FORMAT((eall.AllowanceAmount / shifthoursworked) * GET_employeehoursworked_indate(eallow_EmployeeID,IF(eall.EffectiveStartDate > effectivedatefrom, eall.EffectiveStartDate, effectivedatefrom),IF(eall.EffectiveEndDate > effectivedateto, effectivedateto, eall.EffectiveEndDate),eallow_OrganizationID),2) 'AllowanceAmount'
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='Daily'
            AND eall.OrganizationID=eallow_OrganizationID
            AND IF(EffectiveStartDate > effectivedatefrom AND EffectiveEndDate > effectivedateto
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate < effectivedatefrom AND EffectiveEndDate < effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate <= effectivedatefrom AND EffectiveEndDate >= effectivedateto
            , effectivedateto BETWEEN EffectiveStartDate AND EffectiveEndDate
            , IF(EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveEndDate IS NULL
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            )))))
            UNION
            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,eall.AllowanceAmount
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='One time'
            AND eall.OrganizationID=eallow_OrganizationID
            AND EffectiveStartDate
            BETWEEN effectivedatefrom
            AND effectivedateto
            UNION
            SELECT eall.RowID
            ,p.PartNo 'Type'
            ,FORMAT(((eall.AllowanceAmount / numweekdays) / shifthoursworked) * GET_employeehoursworked_indate(eallow_EmployeeID,IF(eall.EffectiveStartDate > effectivedatefrom, eall.EffectiveStartDate, effectivedatefrom),IF(eall.EffectiveEndDate > effectivedateto, effectivedateto, eall.EffectiveEndDate),eallow_OrganizationID),2) 'AllowanceAmount'
            ,eall.AllowanceFrequency
            ,COALESCE(DATE_FORMAT(eall.EffectiveStartDate,'%m/%d/%Y'),'') 'EffectiveStartDate'
            ,COALESCE(DATE_FORMAT(eall.EffectiveEndDate,'%m/%d/%Y'),'') 'EffectiveEndDate'
            ,IF(TaxableFlag = 0,'No','Yes') 'TaxableFlag'
            ,eall.ProductID
            FROM employeeallowance eall
            LEFT JOIN product p ON eall.ProductID=p.RowID
            WHERE eall.EmployeeID=eallow_EmployeeID
            AND eall.AllowanceFrequency='Semi-monthly'
            AND eall.OrganizationID=eallow_OrganizationID
            AND IF(EffectiveStartDate > effectivedatefrom AND EffectiveEndDate > effectivedateto
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate < effectivedatefrom AND EffectiveEndDate < effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveStartDate <= effectivedatefrom AND EffectiveEndDate >= effectivedateto
            , effectivedateto BETWEEN EffectiveStartDate AND EffectiveEndDate
            , IF(EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            , EffectiveEndDate BETWEEN effectivedatefrom AND effectivedateto
            , IF(EffectiveEndDate IS NULL
            , EffectiveStartDate BETWEEN effectivedatefrom AND effectivedateto
            , EffectiveStartDate >= effectivedatefrom AND EffectiveEndDate <= effectivedateto
            )))))
            ORDER BY RowID;



        END IF;

    END IF;








END IF;





END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
