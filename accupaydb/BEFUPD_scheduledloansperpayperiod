BEGIN

IF NEW.PaystubID IS NULL THEN
	SET NEW.PaystubID = (SELECT RowID
								FROM paystub
								WHERE PayPeriodID = NEW.PayPeriodID
								AND EmployeeID = NEW.EmployeeID);
END IF
;

END