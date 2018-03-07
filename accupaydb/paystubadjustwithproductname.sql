/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `paystubadjustwithproductname`;
DROP TABLE IF EXISTS `paystubadjustwithproductname`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `paystubadjustwithproductname` AS SELECT psj.*
,p.PartNo `AdjustmentName`
,FALSE `AsActual`
FROM paystubadjustment psj
INNER JOIN product p ON p.RowID=psj.ProductID AND p.OrganizationID=psj.OrganizationID 

UNION
SELECT psj.*
,p.PartNo `AdjustmentName`
,TRUE `AsActual`
FROM paystubadjustmentactual psj
INNER JOIN product p ON p.RowID=psj.ProductID AND p.OrganizationID=psj.OrganizationID ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
