/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `v_sumpsitaxableallowance`;
DROP TABLE IF EXISTS `v_sumpsitaxableallowance`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `v_sumpsitaxableallowance` AS SELECT
psi.RowID AS psiRowID
,psi.PayAmount
,psi.OrganizationID
,psi.PayStubID
,psi.ProductID
,psi.Undeclared
,p.CategoryID
,p.`Status` AS Taxable
,p.PartNo
FROM paystubitem psi
INNER JOIN product p ON p.RowID=psi.ProductID
ORDER BY psi.PayStubID ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
