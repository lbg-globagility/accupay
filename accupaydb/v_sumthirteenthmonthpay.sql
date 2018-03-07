/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `v_sumthirteenthmonthpay`;
DROP TABLE IF EXISTS `v_sumthirteenthmonthpay`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_sumthirteenthmonthpay` AS SELECT
tmp.RowID
,tmp.OrganizationID
,tmp.PaystubID
,tmp.Amount
,ps.PayFromDate
,ps.PayToDate
,ps.EmployeeID
,pp.`Half`
,pp.`Month`
,pp.`Year`
FROM thirteenthmonthpay tmp
INNER JOIN paystub ps ON ps.RowID=tmp.PaystubID AND ps.OrganizationID=tmp.OrganizationID
INNER JOIN payperiod pp ON pp.RowID=ps.PayPeriodID AND pp.OrganizationID=tmp.OrganizationID ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
